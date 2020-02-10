using ImdbParser.Options;
using NUnit.Framework;
using System;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using static ImdbParser.Commands.CommandId;
using static ImdbParser.IntegrationTests.AssemblySetup;
using static ImdbParser.IntegrationTests.TestsBase;
using static ImdbParser.Messages;
using static ImdbParser.Options.OptionId;
using static NUnit.Framework.Assert;
using static NUnit.Framework.Is;
using static System.Globalization.CultureInfo;
using static System.IO.Directory;
using static System.IO.Path;

namespace ImdbParser.IntegrationTests {
    /// <summary>Create command tests.</summary>
    [ExcludeFromCodeCoverage]
    static class CreateCommandTests {
        /// <summary>File to use for links.</summary>
        // ReSharper disable once RedundantDefaultMemberInitializer
        static string _linkFile = default!;

        /// <summary>All tests setup.</summary>
        [OneTimeSetUp]
        public static void OneTimeSetup() {
            _linkFile = Combine(WorkingDirectory.ToString(), GetRandomFileName());
            Move(GetTempFileName(), _linkFile);
        }

#pragma warning disable CS3016
        /// <summary>Invalid parameters count.</summary>
        /// <param name="parameters">Parameters for <see cref="Create" /> command.</param>
        /// <returns><see cref="Task" /> to execute.</returns>
        [TestCase, TestCase("", "")]
#pragma warning restore CS3016
        public static async Task InvalidParametersCount_FailureReturned(params string[] parameters) {
            var values = new List<string> { "-Create" };
            values.AddRange(parameters);
            That((await ExecuteAsync(values.ToArray()).ConfigureAwait(default)).message, EqualTo(Invalid_command_parameters_count.CurrentCultureFormat(Create, 1)));
        }

        /// <summary>Parameter not URL.</summary>
        /// <returns><see cref="Task" /> to execute.</returns>
        [Test]
        public static async Task ParameterNotUrl_FailureReturned() => That((await ExecuteAsync("-Create", string.Empty).ConfigureAwait(default)).message, EqualTo(String_not_URL));

        /// <summary>Parameter not IMDB URL.</summary>
        /// <returns><see cref="Task" /> to execute.</returns>
        [Test]
        public static async Task ParameterNotImdbUrl_FailureReturned() => That((await ExecuteAsync("-Create", "https://imdb.com").ConfigureAwait(default)).message, EqualTo(URL_is_not_IMDB_movie));

        /// <summary>No options.</summary>
        /// <returns><see cref="Task" /> to execute.</returns>
        [Test]
        public static async Task NoOptions_FailureReturned() => That((await ExecuteAsync("-Create", "http://www.imdb.com/title/tt1").ConfigureAwait(default)).message,
            EqualTo(Command_must_have_option.CurrentCultureFormat(Create, ", ".Join(OptionId.Language, Link))));

        /// <summary>No language option.</summary>
        /// <returns><see cref="Task" /> to execute.</returns>
        [Test]
        public static async Task NoLanguageOption_FailureReturned() => That((await ExecuteAsync("-Create", "http://www.imdb.com/title/tt1", "-Link", _linkFile).ConfigureAwait(default)).message,
            EqualTo(Command_must_have_option.CurrentCultureFormat(Create, OptionId.Language)));

        /// <summary>No link option.</summary>
        /// <returns><see cref="Task" /> to execute.</returns>
        [Test]
        public static async Task NoLinkOption_FailureReturned() => That((await ExecuteAsync("-Create", "http://www.imdb.com/title/tt1", "-Language", "Lt").ConfigureAwait(default)).message,
            EqualTo(Command_must_have_option.CurrentCultureFormat(Create, Link)));

        /// <summary>Duplicate options.</summary>
        /// <returns><see cref="Task" /> to execute.</returns>
        [Test]
        public static async Task DuplicateOptions_FailureReturned() =>
            That((await ExecuteAsync("-Create", "http://www.imdb.com/title/tt1", "-Language", "Lt", "-Language", "Lt").ConfigureAwait(default)).message, EqualTo(All_options_must_be_unique));

        /// <summary>No language passed.</summary>
        /// <returns><see cref="Task" /> to execute.</returns>
        [Test]
        public static async Task NoLanguage_FailureReturned() => That((await ExecuteAsync("-Create", "URL", "-Language").ConfigureAwait(default)).message,
            EqualTo(Option_has_no_language.InvariantCultureFormat(OptionId.Language)));

        /// <summary>Invalid language passed.</summary>
        /// <returns><see cref="Task" /> to execute.</returns>
        [Test]
        public static async Task LanguageInvalid_FailureReturned() => That((await ExecuteAsync("-Create", "URL", "-Language", string.Empty).ConfigureAwait(default)).message,
            EqualTo(String_not_language.InvariantCultureFormat(OptionId.Language)));

        /// <summary>Duplicate languages.</summary>
        /// <returns><see cref="Task" /> to execute.</returns>
        [Test]
        public static async Task DuplicateLanguages_FailureReturned() =>
            That((await ExecuteAsync("-Create", "URL", "-Language", "Lt", "Lt").ConfigureAwait(default)).message, EqualTo(Language_not_unique));

        /// <summary>No link passed.</summary>
        /// <returns><see cref="Task" /> to execute.</returns>
        [Test]
        public static async Task NoLink_FailureReturned() => That((await ExecuteAsync("-Create", "URL", "-Link").ConfigureAwait(default)).message, EqualTo(Link_has_no_path));

        /// <summary>No link passed.</summary>
        /// <returns><see cref="Task" /> to execute.</returns>
        [Test]
        public static async Task LinkDoesNotExist_FailureReturned() =>
            That((await ExecuteAsync("-Create", "URL", "-Link", Combine(WorkingDirectory.ToString(), GetRandomFileName())).ConfigureAwait(default)).message, EqualTo(File_does_not_exist));

        /// <summary>Invalid subtitles passed.</summary>
        /// <returns><see cref="Task" /> to execute.</returns>
        [Test]
        public static async Task SubtitlesInvalid_FailureReturned() => That((await ExecuteAsync("-Create", "URL", "-Subtitles", string.Empty).ConfigureAwait(default)).message,
            EqualTo(String_not_language.InvariantCultureFormat(Subtitles)));

        /// <summary>Duplicate subtitles.</summary>
        /// <returns><see cref="Task" /> to execute.</returns>
        [Test]
        public static async Task DuplicateSubtitles_FailureReturned() =>
            That((await ExecuteAsync("-Create", "URL", "-Subtitles", "Lt", "Lt").ConfigureAwait(default)).message, EqualTo(Language_not_unique));

        /// <summary>Cast created for minimal command.</summary>
        /// <returns><see cref="Task" /> to execute.</returns>
        [Test]
        public static async Task MinimalCommand_CastCreated() {
            var executionInformation = await ExecuteAsync("-Create", "https://www.imdb.com/title/tt0000001", "-Language", "Lt", "-Link", _linkFile).ConfigureAwait(default);
            var castDirectory = new DirectoryInfo(Combine(executionInformation.settings.LibraryDirectory.ToString(), executionInformation.settings.Folders.Cast));
            That(castDirectory.Exists, Is.True, () => executionInformation.message);
            // ReSharper disable once StringLiteralTypo
            var actor = Combine(castDirectory.ToString(), "Carmencita");
            That(castDirectory.EnumerateFileSystemInfos().Select(items => items.ToString()), EqualTo(new[] { actor }), () => executionInformation.message);
            // ReSharper disable once StringLiteralTypo
            That(EnumerateFileSystemEntries(actor).Select(items => items.ToString(InvariantCulture)), EqualTo(new[] { Combine(actor, "Carmencita (1894); Documentary, Short; LT.mkv.lnk") }),
                () => executionInformation.message);
        }

        /// <summary>Directors created for minimal command.</summary>
        /// <returns><see cref="Task" /> to execute.</returns>
        [Test]
        public static async Task MinimalCommand_DirectorsCreated() {
            var executionInformation = await ExecuteAsync("-Create", "https://www.imdb.com/title/tt0000001", "-Language", "Lt", "-Link", _linkFile).ConfigureAwait(default);
            var directorsDirectory = new DirectoryInfo(Combine(executionInformation.settings.LibraryDirectory.ToString(), executionInformation.settings.Folders.Directors));
            That(directorsDirectory.Exists, Is.True, () => executionInformation.message);
            var director = Combine(directorsDirectory.ToString(), "William K.L. Dickson");
            That(directorsDirectory.EnumerateFileSystemInfos().Select(items => items.ToString()), EqualTo(new[] { director }), () => executionInformation.message);
            // ReSharper disable once StringLiteralTypo
            That(EnumerateFileSystemEntries(director).Select(items => items.ToString(InvariantCulture)), EqualTo(new[] { Combine(director, "Carmencita (1894); Documentary, Short; LT.mkv.lnk") }),
                () => executionInformation.message);
        }

        /// <summary>Genres created for minimal command.</summary>
        /// <returns><see cref="Task" /> to execute.</returns>
        [Test]
        public static async Task MinimalCommand_GenresCreated() {
            var executionInformation = await ExecuteAsync("-Create", "https://www.imdb.com/title/tt0000001", "-Language", "Lt", "-Link", _linkFile).ConfigureAwait(default);
            var genresDirectory = new DirectoryInfo(Combine(executionInformation.settings.LibraryDirectory.ToString(), executionInformation.settings.Folders.Genres));
            That(genresDirectory.Exists, Is.True, () => executionInformation.message);
            var genre1 = Combine(genresDirectory.ToString(), "Documentary");
            var genre2 = Combine(genresDirectory.ToString(), "Short");
            That(genresDirectory.EnumerateFileSystemInfos().Select(items => items.ToString()), EqualTo(new[] { genre1, genre2 }), () => executionInformation.message);
            // ReSharper disable once StringLiteralTypo
            That(EnumerateFileSystemEntries(genre1).Select(items => items.ToString(InvariantCulture)), EqualTo(new[] { Combine(genre1, "Carmencita (1894); Documentary, Short; LT.mkv.lnk") }),
                () => executionInformation.message);
            // ReSharper disable once StringLiteralTypo
            That(EnumerateFileSystemEntries(genre2).Select(items => items.ToString(InvariantCulture)), EqualTo(new[] { Combine(genre2, "Carmencita (1894); Documentary, Short; LT.mkv.lnk") }),
                () => executionInformation.message);
        }

        /// <summary>Years created for minimal command.</summary>
        /// <returns><see cref="Task" /> to execute.</returns>
        [Test]
        public static async Task MinimalCommand_YearsCreated() {
            var executionInformation = await ExecuteAsync("-Create", "https://www.imdb.com/title/tt0000001", "-Language", "Lt", "-Link", _linkFile).ConfigureAwait(default);
            var yearsDirectory = new DirectoryInfo(Combine(executionInformation.settings.LibraryDirectory.ToString(), executionInformation.settings.Folders.Years));
            That(yearsDirectory.Exists, Is.True, () => executionInformation.message);
            var year = Combine(yearsDirectory.ToString(), "1894");
            That(yearsDirectory.EnumerateFileSystemInfos().Select(items => items.ToString()), EqualTo(new[] { year }), () => executionInformation.message);
            // ReSharper disable once StringLiteralTypo
            That(EnumerateFileSystemEntries(year).Select(items => items.ToString(InvariantCulture)), EqualTo(new[] { Combine(year, "Carmencita (1894); Documentary, Short; LT.mkv.lnk") }),
                () => executionInformation.message);
        }

        /// <summary>Full path used for full command.</summary>
        /// <returns><see cref="Task" /> to execute.</returns>
        [Test]
        public static async Task FullCommand_FullPathUsed() {
            var executionInformation = await ExecuteAsync("-Create", "https://www.imdb.com/title/tt0000001", "-Language", "Lt", "En", "-Link", _linkFile, "-Subtitles", "Lt", "En").
                ConfigureAwait(default);
            var yearsDirectory = new DirectoryInfo(Combine(executionInformation.settings.LibraryDirectory.ToString(), executionInformation.settings.Folders.Years));
            That(yearsDirectory.Exists, Is.True, () => executionInformation.message);
            var years = Combine(yearsDirectory.ToString(), "1894");
            That(yearsDirectory.EnumerateFileSystemInfos().Select(items => items.ToString()), EqualTo(new[] { years }), () => executionInformation.message);
            // ReSharper disable once StringLiteralTypo
            That(EnumerateFileSystemEntries(years).Select(items => items.ToString(InvariantCulture)),
                EqualTo(new[] { Combine(years, "Carmencita (1894); Documentary, Short; LT, EN; LT, EN.mkv.lnk") }), () => executionInformation.message);
        }

        /// <summary>Original title in case of translation.</summary>
        /// <returns><see cref="Task" /> to execute.</returns>
        [Test]
        public static async Task MultipleLanguagesTitle_OriginalTitleUsed() {
            var executionInformation = await ExecuteAsync("-Create", "https://www.imdb.com/title/tt0000012", "-Language", "Lt", "-Link", _linkFile).ConfigureAwait(default);
            var yearsDirectory = new DirectoryInfo(Combine(executionInformation.settings.LibraryDirectory.ToString(), executionInformation.settings.Folders.Years));
            That(yearsDirectory.Exists, Is.True, () => executionInformation.message);
            var years = Combine(yearsDirectory.ToString(), "1896");
            That(yearsDirectory.EnumerateFileSystemInfos().Select(items => items.ToString()), EqualTo(new[] { years }), () => executionInformation.message);
            That(EnumerateFileSystemEntries(years).Select(items => items.ToString(InvariantCulture)),
                // ReSharper disable StringLiteralTypo
                EqualTo(new[] { Combine(years, "L'arrivée d'un train à La Ciotat (1896); Documentary, Short, Action; LT.mkv.lnk") }), () => executionInformation.message);
            // ReSharper restore StringLiteralTypo
        }

        /// <summary>Single genre without created.</summary>
        /// <returns><see cref="Task" /> to execute.</returns>
        [Test]
        public static async Task SingleGenre_SingleLinkCreated() {
            var executionInformation = await ExecuteAsync("-Create", "https://www.imdb.com/title/tt0000009", "-Language", "Lt", "-Link", _linkFile).ConfigureAwait(default);
            var genresDirectory = new DirectoryInfo(Combine(executionInformation.settings.LibraryDirectory.ToString(), executionInformation.settings.Folders.Genres));
            That(genresDirectory.Exists, Is.True, () => executionInformation.message);
            var genre = Combine(genresDirectory.ToString(), "Romance");
            That(genresDirectory.EnumerateFileSystemInfos().Select(items => items.ToString()), EqualTo(new[] { genre }), () => executionInformation.message);
            That(EnumerateFileSystemEntries(genre).Select(items => items.ToString(InvariantCulture)), EqualTo(new[] { Combine(genre, "Miss Jerry (1894); Romance; LT.mkv.lnk") }),
                () => executionInformation.message);
        }

        /// <summary>Multiple directors created.</summary>
        /// <returns><see cref="Task" /> to execute.</returns>
        [Test]
        public static async Task MultipleDirectors_MultipleLinksCreated() {
            var executionInformation = await ExecuteAsync("-Create", "https://www.imdb.com/title/tt0000007", "-Language", "Lt", "-Link", _linkFile).ConfigureAwait(default);
            var directorsDirectory = new DirectoryInfo(Combine(executionInformation.settings.LibraryDirectory.ToString(), executionInformation.settings.Folders.Directors));
            That(directorsDirectory.Exists, Is.True, () => executionInformation.message);
            // ReSharper disable once StringLiteralTypo
            var director1 = Combine(directorsDirectory.ToString(), "William Heise");
            var director2 = Combine(directorsDirectory.ToString(), "William K.L. Dickson");
            That(directorsDirectory.EnumerateFileSystemInfos().Select(items => items.ToString()), EqualTo(new[] { director1, director2 }), () => executionInformation.message);
            That(EnumerateFileSystemEntries(director1).Select(items => items.ToString(InvariantCulture)),
                // ReSharper disable StringLiteralTypo
                EqualTo(new[] { Combine(director1, "Corbett and Courtney Before the Kinetograph (1894); Short, Sport; LT.mkv.lnk") }), () => executionInformation.message);
            // ReSharper restore StringLiteralTypo
            That(EnumerateFileSystemEntries(director2).Select(items => items.ToString(InvariantCulture)),
                // ReSharper disable StringLiteralTypo
                EqualTo(new[] { Combine(director2, "Corbett and Courtney Before the Kinetograph (1894); Short, Sport; LT.mkv.lnk") }), () => executionInformation.message);
            // ReSharper restore StringLiteralTypo
        }

        /// <summary>Multiple actors created.</summary>
        /// <returns><see cref="Task" /> to execute.</returns>
        [Test]
        public static async Task MultipleActors_MultipleLinksCreated() {
            var executionInformation = await ExecuteAsync("-Create", "https://www.imdb.com/title/tt0000005", "-Language", "Lt", "-Link", _linkFile).ConfigureAwait(default);
            var castDirectory = new DirectoryInfo(Combine(executionInformation.settings.LibraryDirectory.ToString(), executionInformation.settings.Folders.Cast));
            That(castDirectory.Exists, Is.True, () => executionInformation.message);
            // ReSharper disable once StringLiteralTypo
            var actor1 = Combine(castDirectory.ToString(), "Charles Kayser");
            var actor2 = Combine(castDirectory.ToString(), "John Ott");
            That(castDirectory.EnumerateFileSystemInfos().Select(items => items.ToString()), EqualTo(new[] { actor1, actor2 }), () => executionInformation.message);
            That(EnumerateFileSystemEntries(actor1).Select(items => items.ToString(InvariantCulture)), EqualTo(new[] { Combine(actor1, "Blacksmith Scene (1893); Short, Comedy; LT.mkv.lnk") }),
                () => executionInformation.message);
            That(EnumerateFileSystemEntries(actor2).Select(items => items.ToString(InvariantCulture)), EqualTo(new[] { Combine(actor2, "Blacksmith Scene (1893); Short, Comedy; LT.mkv.lnk") }),
                () => executionInformation.message);
        }

        /// <summary>Has actors with rest of cast part.</summary>
        /// <returns><see cref="Task" /> to execute.</returns>
        [Test]
        public static async Task HasRestActorsPart_ActorsAdded() {
            var executionInformation = await ExecuteAsync("-Create", "https://www.imdb.com/title/tt0000420", "-Language", "Lt", "-Link", _linkFile).ConfigureAwait(default);
            var castDirectory = new DirectoryInfo(Combine(executionInformation.settings.LibraryDirectory.ToString(), executionInformation.settings.Folders.Cast));
            That(castDirectory.Exists, Is.True, () => executionInformation.message);
            var actor1 = Combine(castDirectory.ToString(), "Blair");
            // ReSharper disable once StringLiteralTypo
            var actor2 = Combine(castDirectory.ToString(), "Cecil M. Hepworth");
            // ReSharper disable once StringLiteralTypo
            var actor3 = Combine(castDirectory.ToString(), "Geoffrey Faithfull");
            var actor4 = Combine(castDirectory.ToString(), "May Clark");
            // ReSharper disable once StringLiteralTypo
            var actor5 = Combine(castDirectory.ToString(), "Mrs. Hepworth");
            // ReSharper disable once StringLiteralTypo
            var actor6 = Combine(castDirectory.ToString(), "Norman Whitten");
            // ReSharper disable once StringLiteralTypo
            var actor7 = Combine(castDirectory.ToString(), "Stanley Faithfull");
            That(castDirectory.EnumerateFileSystemInfos().Select(items => items.ToString()), EqualTo(new[] { actor1, actor2, actor3, actor4, actor5, actor6, actor7 }),
                () => executionInformation.message);
            That(EnumerateFileSystemEntries(actor1).Select(items => items.ToString(InvariantCulture)), EqualTo(new[] { Combine(actor1, "Alice in Wonderland (1903); Fantasy, Short; LT.mkv.lnk") }),
                () => executionInformation.message);
            That(EnumerateFileSystemEntries(actor2).Select(items => items.ToString(InvariantCulture)), EqualTo(new[] { Combine(actor2, "Alice in Wonderland (1903); Fantasy, Short; LT.mkv.lnk") }),
                () => executionInformation.message);
            That(EnumerateFileSystemEntries(actor3).Select(items => items.ToString(InvariantCulture)), EqualTo(new[] { Combine(actor3, "Alice in Wonderland (1903); Fantasy, Short; LT.mkv.lnk") }),
                () => executionInformation.message);
            That(EnumerateFileSystemEntries(actor4).Select(items => items.ToString(InvariantCulture)), EqualTo(new[] { Combine(actor4, "Alice in Wonderland (1903); Fantasy, Short; LT.mkv.lnk") }),
                () => executionInformation.message);
            That(EnumerateFileSystemEntries(actor5).Select(items => items.ToString(InvariantCulture)), EqualTo(new[] { Combine(actor5, "Alice in Wonderland (1903); Fantasy, Short; LT.mkv.lnk") }),
                () => executionInformation.message);
            That(EnumerateFileSystemEntries(actor6).Select(items => items.ToString(InvariantCulture)), EqualTo(new[] { Combine(actor6, "Alice in Wonderland (1903); Fantasy, Short; LT.mkv.lnk") }),
                () => executionInformation.message);
            That(EnumerateFileSystemEntries(actor7).Select(items => items.ToString(InvariantCulture)), EqualTo(new[] { Combine(actor7, "Alice in Wonderland (1903); Fantasy, Short; LT.mkv.lnk") }),
                () => executionInformation.message);
        }

        /// <summary>Title with invalid characters for file path.</summary>
        /// <returns><see cref="Task" /> to execute.</returns>
        [Test]
        public static async Task TitleContainsInvalidPathCharacters_CharactersRemovedFromFileName() {
            var executionInformation = await ExecuteAsync("-Create", "https://www.imdb.com/title/tt0000013", "-Language", "Lt", "-Link", _linkFile).ConfigureAwait(default);
            var yearsDirectory = new DirectoryInfo(Combine(executionInformation.settings.LibraryDirectory.ToString(), executionInformation.settings.Folders.Years));
            That(yearsDirectory.Exists, Is.True, () => executionInformation.message);
            var years = Combine(yearsDirectory.ToString(), "1895");
            That(yearsDirectory.EnumerateFileSystemInfos().Select(items => items.ToString()), EqualTo(new[] { years }), () => executionInformation.message);
            That(EnumerateFileSystemEntries(years).Select(items => items.ToString(InvariantCulture)),
                // ReSharper disable StringLiteralTypo
                EqualTo(new[] { Combine(years, "Neuville-sur-Saône Débarquement du congrès des photographes à Lyon (1895); Documentary, Short; LT.mkv.lnk") }), () => executionInformation.message);
            // ReSharper restore StringLiteralTypo
        }
    }
}