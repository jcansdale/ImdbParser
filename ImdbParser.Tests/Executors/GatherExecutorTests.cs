using ImdbParser.Executors;
using ImdbParser.Html;
using ImdbParser.Settings;
using Moq;
using NUnit.Framework;
using System;
using System.Collections.Immutable;
using System.Diagnostics.CodeAnalysis;
using System.Drawing;
using System.IO;
using System.Threading.Tasks;
using System.Xml.Linq;
using static ImdbParser.Commands.GatherCommand;
using static ImdbParser.Language;
using static ImdbParser.Movie;
using static ImdbParser.Options.SubtitlesOption;
using static ImdbParser.Tests.TestsBase;
using static Moq.Times;
using static NUnit.Framework.Assert;
using static NUnit.Framework.Is;
using static System.Linq.Enumerable;
using static System.Result;

namespace ImdbParser.Tests.Executors {
    /// <summary><see cref="GatherExecutor" /> tests.</summary>
    [ExcludeFromCodeCoverage]
    static class GatherExecutorTests {
#region Constructor tests
        /// <summary><see cref="GatherExecutor(IHtmlManager, ITagsCreator, IFileNameBuilder, ISettings)" /> test.</summary>
        [Test]
        public static void Constructor_ValidArgumentsPassed_NoExceptionThrown() {
            var setup = Setup();

            That(new GatherExecutor(setup.manager.Object, setup.creator.Object, setup.builder.Object, setup.settings.Object), Not.Null);
        }
#endregion

#region ExecuteAsync tests
        /// <summary><see cref="GatherExecutor.ExecuteAsync" /> test.</summary>
        /// <returns><see cref="Task" /> to execute.</returns>
        [Test]
        public static async Task ExecuteAsync_FailedToGatherMovieData_FailureReturned() {
            var setup = Setup();
            var command = CreateGather();
            const string Failure = "Failure";
            using var result = DisposableFail<Movie>(Failure);
            setup.manager.SetupManagerParseMovieAsync(command.Url, result);

            (await setup.executor.ExecuteAsync(command).ConfigureAwait(default)).ExpectFail(() => Failure);
        }

        /// <summary><see cref="GatherExecutor.ExecuteAsync" /> test.</summary>
        /// <returns><see cref="Task" /> to execute.</returns>
        [Test]
        public static async Task ExecuteAsync_MovieDataGather_CoverSaved() {
            var setup = Setup();
            var command = CreateGather();
            var image = new Mock<IImage>();
            setup.settings.SetupSettings();
            using var movie = CreateMovie("Title", 1, new[] { "Genre" }, image.Object, new[] { "Director" }, Empty<string>(), new[] { "Actor" });
            setup.manager.SetupManagerParseMovieAsync(command.Url, movie);
            setup.creator.SetupCreatorCreateTagsFileSave(movie.Value);
            setup.builder.SetupBuilderBuildFileName(movie.Value);

            (await setup.executor.ExecuteAsync(command).ConfigureAwait(default)).ExpectSuccess();

            image.Verify(self => self.Save(@"A:\CoverName"), Once);
        }

        /// <summary><see cref="GatherExecutor.ExecuteAsync" /> test.</summary>
        /// <returns><see cref="Task" /> to execute.</returns>
        [Test]
        public static async Task ExecuteAsync_MovieDataGatherAndNoSubtitles_TagsSaved() {
            var setup = Setup();
            var command = CreateGather();
            setup.settings.SetupSettings();
            using var movie = CreateMovie();
            setup.manager.SetupManagerParseMovieAsync(command.Url, DisposableOk(movie));
            setup.creator.SetupCreatorCreateTagsFileSave(movie);
            setup.builder.SetupBuilderBuildFileName(movie);

            (await setup.executor.ExecuteAsync(command).ConfigureAwait(default)).ExpectSuccess();

            // ReSharper disable once AccessToDisposedClosure
            setup.creator.Verify(self => self.CreateTagsFile(movie, new[] { Lt }, Empty<Language>().ToImmutableList()).Save(@"A:\TagsName"), Once);
        }

        /// <summary><see cref="GatherExecutor.ExecuteAsync" /> test.</summary>
        /// <returns><see cref="Task" /> to execute.</returns>
        [Test]
        public static async Task ExecuteAsync_MovieDataGatherWithSubtitles_TagsSaved() {
            var setup = Setup();
            const Language Language = En;
            var command = Create(new[] { ImdbTestUrl }, new[] { CreateLanguage(), CreateOption(new[] { Language.ToString() }).Value }).Value;
            setup.settings.SetupSettings();
            using var movie = CreateMovie();
            setup.manager.SetupManagerParseMovieAsync(command.Url, DisposableOk(movie));
            setup.creator.SetupCreatorCreateTagsFileSave(movie, Language);
            setup.builder.SetupBuilderBuildFileName(movie, Language);

            (await setup.executor.ExecuteAsync(command).ConfigureAwait(default)).ExpectSuccess();

            // ReSharper disable once AccessToDisposedClosure
            setup.creator.Verify(self => self.CreateTagsFile(movie, new[] { Lt }, new[] { Language }).Save(@"A:\TagsName"), Once);
        }

        /// <summary><see cref="GatherExecutor.ExecuteAsync" /> test.</summary>
        /// <returns><see cref="Task" /> to execute.</returns>
        [Test]
        public static async Task ExecuteAsync_MovieDataCreated_MoviePathReturned() {
            var setup = Setup();
            var command = CreateGather();
            var image = new Mock<IImage>();
            setup.settings.SetupSettings();
            using var movie = CreateMovie("Title", 1, new[] { "Genre" }, image.Object, new[] { "Director" }, Empty<string>(), new[] { "Actor" });
            setup.manager.SetupManagerParseMovieAsync(command.Url, movie);
            setup.creator.SetupCreatorCreateTagsFileSave(movie.Value);
            setup.builder.SetupBuilderBuildFileName(movie.Value);

            (await setup.executor.ExecuteAsync(command).ConfigureAwait(default)).ExpectSuccess(result => That(result, EqualTo(@"A:\FileName")));
        }
#endregion

        /// <summary>Single test setup.</summary>
        /// <returns><see cref="object" /> to test with its <see cref="Mock{T}" />s.</returns>
        static (GatherExecutor executor, Mock<IHtmlManager> manager, Mock<ITagsCreator> creator, Mock<IFileNameBuilder> builder, Mock<ISettings> settings) Setup() {
            var manager = new Mock<IHtmlManager>();
            var creator = new Mock<ITagsCreator>();
            var builder = new Mock<IFileNameBuilder>();
            var settings = new Mock<ISettings>();
            return (new GatherExecutor(manager.Object, creator.Object, builder.Object, settings.Object), manager, creator, builder, settings);
        }

        /// <summary>Sets up <paramref name="builder" />'s <see cref="IFileNameBuilder.BuildFileName" /> to return valid value.</summary>
        /// <param name="builder"><see cref="IFileNameBuilder" /> <see cref="Mock" /> to set up.</param>
        /// <param name="movie">Movie to expect.</param>
        /// <param name="subtitles">Subtitles to expect.</param>
        static void SetupBuilderBuildFileName(this Mock<IFileNameBuilder> builder, Movie movie, params Language[] subtitles) =>
            builder.Setup(self => self.BuildFileName(movie, new[] { Lt }, subtitles)).Returns(new FileInfo(@"A:\FileName"));

        /// <summary>Sets up <paramref name="creator" />'s <see cref="ITagsCreator.CreateTagsFile" /> <see cref="IXDocument.Save" />.</summary>
        /// <param name="creator"><see cref="ITagsCreator" /> <see cref="Mock" /> to set up.</param>
        /// <param name="movie">Movie to expect.</param>
        /// <param name="subtitles">Subtitles to expect.</param>
        static void SetupCreatorCreateTagsFileSave(this Mock<ITagsCreator> creator, Movie movie, params Language[] subtitles) =>
            creator.Setup(self => self.CreateTagsFile(movie, new[] { Lt }, subtitles).Save(@"A:\TagsName")).Verifiable();

        /// <summary>Sets up <paramref name="manager" />'s <see cref="IHtmlManager.ParseMovieAsync" /> to return <paramref name="result" /> when called with <paramref name="url" />.</summary>
        /// <param name="manager"><see cref="IHtmlManager" /> <see cref="Mock" /> to set up.</param>
        /// <param name="url">URL to expect.</param>
        /// <param name="result">Result to return.</param>
        static void SetupManagerParseMovieAsync(this Mock<IHtmlManager> manager, ImdbFilmUrl url, DisposableResult<Movie> result) =>
            manager.Setup(self => self.ParseMovieAsync(url)).ReturnsAsync(result);

        /// <summary>Sets up <paramref name="settings" /> to return valid values.</summary>
        /// <param name="settings"><see cref="ISettings" /> <see cref="Mock" /> to set up.</param>
        static void SetupSettings(this Mock<ISettings> settings) {
            settings.SetupGet(self => self.CoverName).Returns(new FileInfo(@"A:\CoverName"));
            settings.SetupGet(self => self.TagsName).Returns(new FileInfo(@"A:\TagsName"));
        }
    }
}