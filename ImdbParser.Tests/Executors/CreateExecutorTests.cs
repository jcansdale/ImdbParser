using ImdbParser.Executors;
using ImdbParser.Html;
using ImdbParser.Options;
using Moq;
using NUnit.Framework;
using System;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.Linq;
using System.Threading.Tasks;
using static ImdbParser.Commands.CreateCommand;
using static ImdbParser.Tests.TestsBase;
using static NUnit.Framework.Assert;
using static NUnit.Framework.Is;
using static System.Result;

namespace ImdbParser.Tests.Executors {
    /// <summary><see cref="CreateExecutor" /> tests.</summary>
    [ExcludeFromCodeCoverage]
    static class CreateExecutorTests {
#region Constructor tests
        /// <summary><see cref="CreateExecutor(IHtmlManager, IShortcutCreator)" /> test.</summary>
        [Test]
        public static void Constructor_ValidArgumentsPassed_NoExceptionThrown() {
            var setup = Setup();

            That(new CreateExecutor(setup.manager.Object, setup.creator.Object), Not.Null);
        }
#endregion

#region ExecuteAsync tests
        /// <summary><see cref="CreateExecutor.ExecuteAsync" /> test.</summary>
        /// <returns><see cref="Task" /> to execute.</returns>
        [Test]
        public static async Task ExecuteAsync_FailedToGatherMovieData_FailureReturned() {
            var setup = Setup();
            var command = CreateCreate();
            const string Failure = "Failure";
            using var result = DisposableFail<Movie>(Failure);
            setup.manager.SetupManagerParseMovieAsync(command.Url, result);

            (await setup.executor.ExecuteAsync(command).ConfigureAwait(default)).ExpectFail(() => Failure);
        }

        /// <summary><see cref="CreateExecutor.ExecuteAsync" /> test.</summary>
        /// <returns><see cref="Task" /> to execute.</returns>
        [SuppressMessage("Maintainability", "CA1508:Avoid dead conditional code", Justification = "False positive."), Test]
        public static async Task ExecuteAsync_ShortcutCreated_CreatorCalled() {
            var setup = Setup();
            var language = CreateLanguage();
            var subtitle = CreateSubtitles();
            var command = Create(new[] { ImdbTestUrl }, new IOption[] { language, CreateLink(), subtitle }).Value;
            using var movie = CreateMovie();
            using var result = DisposableOk(movie);
            setup.manager.SetupManagerParseMovieAsync(command.Url, result);

            (await setup.executor.ExecuteAsync(command).ConfigureAwait(default)).ExpectSuccess();

            // ReSharper disable once AccessToDisposedClosure
            setup.creator.Verify(self => self.CreateShortcuts(movie, language.Languages, It.Is<IReadOnlyCollection<Language>>(actualSubtitles => actualSubtitles.SequenceEqual(subtitle.Languages))),
                Times.Once);
        }
#endregion

        /// <summary>Single test setup.</summary>
        /// <returns><see cref="object" /> to test with its <see cref="Mock" />s.</returns>
        static (CreateExecutor executor, Mock<IHtmlManager> manager, Mock<IShortcutCreator> creator) Setup() {
            var manager = new Mock<IHtmlManager>();
            var creator = new Mock<IShortcutCreator>();
            return (new CreateExecutor(manager.Object, creator.Object), manager, creator);
        }

        /// <summary>Sets up <paramref name="manager" />'s <see cref="IHtmlManager.ParseMovieAsync" /> to return <paramref name="result" /> when called with <paramref name="url" />.</summary>
        /// <param name="manager"><see cref="IHtmlManager" /> <see cref="Mock" /> to set up.</param>
        /// <param name="url">URL to expect.</param>
        /// <param name="result">Result to return.</param>
        static void SetupManagerParseMovieAsync(this Mock<IHtmlManager> manager, ImdbFilmUrl url, DisposableResult<Movie> result) =>
            manager.Setup(self => self.ParseMovieAsync(url)).ReturnsAsync(result);
    }
}