using ImdbParser.Options;
using NUnit.Framework;
using System;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.Threading.Tasks;
using static ImdbParser.Commands.CommandId;
using static ImdbParser.IntegrationTests.TestsBase;
using static ImdbParser.Messages;
using static NUnit.Framework.Assert;
using static NUnit.Framework.Is;
using static System.IO.File;
using static System.IO.Path;

namespace ImdbParser.IntegrationTests {
    /// <summary>Gather command tests.</summary>
    [ExcludeFromCodeCoverage]
    static class GatherCommandTests {
#pragma warning disable CS3016
        /// <summary>Invalid parameters count.</summary>
        /// <param name="parameters">Parameters for <see cref="Gather" /> command.</param>
        /// <returns><see cref="Task" /> to execute.</returns>
        [TestCase, TestCase("", "")]
#pragma warning restore CS3016
        public static async Task InvalidParametersCount_FailureReturned(params string[] parameters) {
            var values = new List<string> { "-Gather" };
            values.AddRange(parameters);
            That((await ExecuteAsync(values.ToArray()).ConfigureAwait(default)).message, EqualTo(Invalid_command_parameters_count.CurrentCultureFormat(Gather, 1)));
        }

        /// <summary>Parameter not URL.</summary>
        /// <returns><see cref="Task" /> to execute.</returns>
        [Test]
        public static async Task ParameterNotUrl_FailureReturned() => That((await ExecuteAsync("-Gather", string.Empty).ConfigureAwait(default)).message, EqualTo(String_not_URL));

        /// <summary>Parameter not IMDB URL.</summary>
        /// <returns><see cref="Task" /> to execute.</returns>
        [Test]
        public static async Task ParameterNotImdbUrl_FailureReturned() => That((await ExecuteAsync("-Gather", "https://imdb.com").ConfigureAwait(default)).message, EqualTo(URL_is_not_IMDB_movie));

        /// <summary>No options.</summary>
        /// <returns><see cref="Task" /> to execute.</returns>
        [Test]
        public static async Task NoOptions_FailureReturned() => That((await ExecuteAsync("-Gather", "http://www.imdb.com/title/tt1").ConfigureAwait(default)).message,
            EqualTo(Command_must_have_option.CurrentCultureFormat(Gather, OptionId.Language)));

        /// <summary>Duplicate options.</summary>
        /// <returns><see cref="Task" /> to execute.</returns>
        [Test]
        public static async Task DuplicateOptions_FailureReturned() =>
            That((await ExecuteAsync("-Gather", "http://www.imdb.com/title/tt1", "-Language", "Lt", "-Language", "Lt").ConfigureAwait(default)).message, EqualTo(All_options_must_be_unique));

        /// <summary>No language passed.</summary>
        /// <returns><see cref="Task" /> to execute.</returns>
        [Test]
        public static async Task NoLanguage_FailureReturned() => That((await ExecuteAsync("-Gather", "URL", "-Language").ConfigureAwait(default)).message,
            EqualTo(Option_has_no_language.InvariantCultureFormat(OptionId.Language)));

        /// <summary>Invalid language passed.</summary>
        /// <returns><see cref="Task" /> to execute.</returns>
        [Test]
        public static async Task LanguageInvalid_FailureReturned() => That((await ExecuteAsync("-Gather", "URL", "-Language", string.Empty).ConfigureAwait(default)).message,
            EqualTo(String_not_language.InvariantCultureFormat(OptionId.Language)));

        /// <summary>Duplicate languages.</summary>
        /// <returns><see cref="Task" /> to execute.</returns>
        [Test]
        public static async Task DuplicateLanguages_FailureReturned() =>
            That((await ExecuteAsync("-Gather", "URL", "-Language", "Lt", "Lt").ConfigureAwait(default)).message, EqualTo(Language_not_unique));

        /// <summary>Invalid subtitles passed.</summary>
        /// <returns><see cref="Task" /> to execute.</returns>
        [Test]
        public static async Task SubtitlesInvalid_FailureReturned() => That((await ExecuteAsync("-Gather", "URL", "-Subtitles", string.Empty).ConfigureAwait(default)).message,
            EqualTo(String_not_language.InvariantCultureFormat(OptionId.Subtitles)));

        /// <summary>Duplicate subtitles.</summary>
        /// <returns><see cref="Task" /> to execute.</returns>
        [Test]
        public static async Task DuplicateSubtitles_FailureReturned() =>
            That((await ExecuteAsync("-Gather", "URL", "-Subtitles", "Lt", "Lt").ConfigureAwait(default)).message, EqualTo(Language_not_unique));

        /// <summary>Cover saved.</summary>
        /// <returns><see cref="Task" /> to execute.</returns>
        [Test]
        public static async Task MinimalCommand_CoverDownloaded() {
            var executionInformation = await ExecuteAsync("-Gather", "https://www.imdb.com/title/tt0000001", "-Language", "Lt").ConfigureAwait(default);
            That(executionInformation.settings.CoverName.Exists, Is.True, () => executionInformation.message);
        }

        /// <summary>Tags saved for minimal command.</summary>
        /// <returns><see cref="Task" /> to execute.</returns>
        [Test]
        public static async Task MinimalCommand_TagsCreated() {
            var executionInformation = await ExecuteAsync("-Gather", "https://www.imdb.com/title/tt0000001", "-Language", "Lt").ConfigureAwait(default);
            That(executionInformation.settings.TagsName.Exists, Is.True, () => executionInformation.message);
            // ReSharper disable StringLiteralTypo
            That(await ReadAllTextAsync(executionInformation.settings.TagsName.ToString()).ConfigureAwait(default), EqualTo(@"<?xml version=""1.0"" encoding=""utf-8""?>
<Tags>
  <Tag>
    <Simple>
      <Name>Title</Name>
      <String>Carmencita</String>
    </Simple>
    <Simple>
      <Name>Years</Name>
      <String>1894</String>
    </Simple>
    <Simple>
      <Name>Genres</Name>
      <String>Documentary/Short</String>
    </Simple>
    <Simple>
      <Name>Directors</Name>
      <String>William K.L. Dickson</String>
    </Simple>
    <Simple>
      <Name>Actors</Name>
      <String>Carmencita</String>
    </Simple>
    <Simple>
      <Name>Languages</Name>
      <String>lt</String>
    </Simple>
  </Tag>
</Tags>"));
            // ReSharper restore StringLiteralTypo
        }

        /// <summary>Name returned for minimal command.</summary>
        /// <returns><see cref="Task" /> to execute.</returns>
        [Test]
        public static async Task MinimalCommand_NameReturned() {
            var executionInformation = await ExecuteAsync("-Gather", "https://www.imdb.com/title/tt0000001", "-Language", "Lt").ConfigureAwait(default);
            // ReSharper disable once StringLiteralTypo
            That(executionInformation.message, EqualTo(Combine(executionInformation.settings.MoviesDirectory.ToString(), "Carmencita (1894); Documentary, Short; LT.mkv")));
        }

        /// <summary>Tags saved for full command.</summary>
        /// <returns><see cref="Task" /> to execute.</returns>
        [Test]
        public static async Task FullCommand_TagsCreated() {
            var executionInformation = await ExecuteAsync("-Gather", "https://www.imdb.com/title/tt0000001", "-Language", "Lt", "En", "-Subtitles", "Lt", "En").ConfigureAwait(default);
            That(executionInformation.settings.TagsName.Exists, Is.True, () => executionInformation.message);
            // ReSharper disable StringLiteralTypo
            That(await ReadAllTextAsync(executionInformation.settings.TagsName.ToString()).ConfigureAwait(default), EqualTo(@"<?xml version=""1.0"" encoding=""utf-8""?>
<Tags>
  <Tag>
    <Simple>
      <Name>Title</Name>
      <String>Carmencita</String>
    </Simple>
    <Simple>
      <Name>Years</Name>
      <String>1894</String>
    </Simple>
    <Simple>
      <Name>Genres</Name>
      <String>Documentary/Short</String>
    </Simple>
    <Simple>
      <Name>Directors</Name>
      <String>William K.L. Dickson</String>
    </Simple>
    <Simple>
      <Name>Actors</Name>
      <String>Carmencita</String>
    </Simple>
    <Simple>
      <Name>Languages</Name>
      <String>lt/en</String>
    </Simple>
    <Simple>
      <Name>Subtitles</Name>
      <String>lt/en</String>
    </Simple>
  </Tag>
</Tags>"));
            // ReSharper restore StringLiteralTypo
        }

        /// <summary>Name returned for full command.</summary>
        /// <returns><see cref="Task" /> to execute.</returns>
        [Test]
        public static async Task FullCommand_NameReturned() {
            var executionInformation = await ExecuteAsync("-Gather", "https://www.imdb.com/title/tt0000001", "-Language", "Lt", "En", "-Subtitles", "Lt", "En").ConfigureAwait(default);
            // ReSharper disable once StringLiteralTypo
            That(executionInformation.message, EqualTo(Combine(executionInformation.settings.MoviesDirectory.ToString(), "Carmencita (1894); Documentary, Short; LT, EN; LT, EN.mkv")));
        }

        /// <summary>All languages added to tags.</summary>
        /// <returns><see cref="Task" /> to execute.</returns>
        [Test]
        public static async Task AllLanguagesPassed_AllLanguagesAddedToTags() {
            var executionInformation = await ExecuteAsync("-Gather", "https://www.imdb.com/title/tt0000001", "-Language", "En", "Lt").ConfigureAwait(default);
            That(executionInformation.settings.TagsName.Exists, Is.True, () => executionInformation.message);
            // ReSharper disable StringLiteralTypo
            That(await ReadAllTextAsync(executionInformation.settings.TagsName.ToString()).ConfigureAwait(default), EqualTo(@"<?xml version=""1.0"" encoding=""utf-8""?>
<Tags>
  <Tag>
    <Simple>
      <Name>Title</Name>
      <String>Carmencita</String>
    </Simple>
    <Simple>
      <Name>Years</Name>
      <String>1894</String>
    </Simple>
    <Simple>
      <Name>Genres</Name>
      <String>Documentary/Short</String>
    </Simple>
    <Simple>
      <Name>Directors</Name>
      <String>William K.L. Dickson</String>
    </Simple>
    <Simple>
      <Name>Actors</Name>
      <String>Carmencita</String>
    </Simple>
    <Simple>
      <Name>Languages</Name>
      <String>lt/en</String>
    </Simple>
  </Tag>
</Tags>"));
            // ReSharper restore StringLiteralTypo
        }

        /// <summary>All languages added to file name.</summary>
        /// <returns><see cref="Task" /> to execute.</returns>
        [Test]
        public static async Task AllLanguagesPassed_AllLanguagesAddedToFileName() {
            var executionInformation = await ExecuteAsync("-Gather", "https://www.imdb.com/title/tt0000001", "-Language", "En", "Lt").ConfigureAwait(default);
            That(executionInformation.settings.TagsName.Exists, Is.True, () => executionInformation.message);
            // ReSharper disable once StringLiteralTypo
            That(executionInformation.message, EqualTo(Combine(executionInformation.settings.MoviesDirectory.ToString(), "Carmencita (1894); Documentary, Short; LT, EN.mkv")));
        }

        /// <summary>Subtitles added to tags.</summary>
        /// <returns><see cref="Task" /> to execute.</returns>
        [Test]
        public static async Task SubtitlesPassed_SubtitlesAddedToTags() {
            var executionInformation = await ExecuteAsync("-Gather", "https://www.imdb.com/title/tt0000001", "-Language", "Lt", "-Subtitles", "Lt").ConfigureAwait(default);
            That(executionInformation.settings.TagsName.Exists, Is.True, () => executionInformation.message);
            // ReSharper disable StringLiteralTypo
            That(await ReadAllTextAsync(executionInformation.settings.TagsName.ToString()).ConfigureAwait(default), EqualTo(@"<?xml version=""1.0"" encoding=""utf-8""?>
<Tags>
  <Tag>
    <Simple>
      <Name>Title</Name>
      <String>Carmencita</String>
    </Simple>
    <Simple>
      <Name>Years</Name>
      <String>1894</String>
    </Simple>
    <Simple>
      <Name>Genres</Name>
      <String>Documentary/Short</String>
    </Simple>
    <Simple>
      <Name>Directors</Name>
      <String>William K.L. Dickson</String>
    </Simple>
    <Simple>
      <Name>Actors</Name>
      <String>Carmencita</String>
    </Simple>
    <Simple>
      <Name>Languages</Name>
      <String>lt</String>
    </Simple>
    <Simple>
      <Name>Subtitles</Name>
      <String>lt</String>
    </Simple>
  </Tag>
</Tags>"));
            // ReSharper restore StringLiteralTypo
        }

        /// <summary>Subtitles added to file name.</summary>
        /// <returns><see cref="Task" /> to execute.</returns>
        [Test]
        public static async Task SubtitlesPassed_SubtitlesAddedToFileName() {
            var executionInformation = await ExecuteAsync("-Gather", "https://www.imdb.com/title/tt0000001", "-Language", "Lt", "-Subtitles", "Lt").ConfigureAwait(default);
            That(executionInformation.settings.TagsName.Exists, Is.True, () => executionInformation.message);
            // ReSharper disable once StringLiteralTypo
            That(executionInformation.message, EqualTo(Combine(executionInformation.settings.MoviesDirectory.ToString(), "Carmencita (1894); Documentary, Short; LT; LT.mkv")));
        }

        /// <summary>All subtitles added to tags.</summary>
        /// <returns><see cref="Task" /> to execute.</returns>
        [Test]
        public static async Task AllSubtitlesPassed_AllSubtitlesAddedToTags() {
            var executionInformation = await ExecuteAsync("-Gather", "https://www.imdb.com/title/tt0000001", "-Language", "Lt", "-Subtitles", "En", "Lt").ConfigureAwait(default);
            That(executionInformation.settings.TagsName.Exists, Is.True, () => executionInformation.message);
            // ReSharper disable StringLiteralTypo
            That(await ReadAllTextAsync(executionInformation.settings.TagsName.ToString()).ConfigureAwait(default), EqualTo(@"<?xml version=""1.0"" encoding=""utf-8""?>
<Tags>
  <Tag>
    <Simple>
      <Name>Title</Name>
      <String>Carmencita</String>
    </Simple>
    <Simple>
      <Name>Years</Name>
      <String>1894</String>
    </Simple>
    <Simple>
      <Name>Genres</Name>
      <String>Documentary/Short</String>
    </Simple>
    <Simple>
      <Name>Directors</Name>
      <String>William K.L. Dickson</String>
    </Simple>
    <Simple>
      <Name>Actors</Name>
      <String>Carmencita</String>
    </Simple>
    <Simple>
      <Name>Languages</Name>
      <String>lt</String>
    </Simple>
    <Simple>
      <Name>Subtitles</Name>
      <String>lt/en</String>
    </Simple>
  </Tag>
</Tags>"));
            // ReSharper restore StringLiteralTypo
        }

        /// <summary>All subtitles added to file name.</summary>
        /// <returns><see cref="Task" /> to execute.</returns>
        [Test]
        public static async Task AllSubtitlesPassed_AllSubtitlesAddedToFileName() {
            var executionInformation = await ExecuteAsync("-Gather", "https://www.imdb.com/title/tt0000001", "-Language", "Lt", "-Subtitles", "En", "Lt").ConfigureAwait(default);
            That(executionInformation.settings.TagsName.Exists, Is.True, () => executionInformation.message);
            // ReSharper disable once StringLiteralTypo
            That(executionInformation.message, EqualTo(Combine(executionInformation.settings.MoviesDirectory.ToString(), "Carmencita (1894); Documentary, Short; LT; LT, EN.mkv")));
        }

        /// <summary>Original title in case of translation.</summary>
        /// <returns><see cref="Task" /> to execute.</returns>
        [Test]
        public static async Task MultipleLanguagesTitle_OriginalTitleUsed() {
            var executionInformation = await ExecuteAsync("-Gather", "https://www.imdb.com/title/tt0000012", "-Language", "Lt").ConfigureAwait(default);
            // ReSharper disable once StringLiteralTypo
            Multiple(async () => {
                // ReSharper disable StringLiteralTypo
                That(executionInformation.message,
                    EqualTo(Combine(executionInformation.settings.MoviesDirectory.ToString(), "L'arrivée d'un train à La Ciotat (1896); Documentary, Short, Action; LT.mkv")));
                That(await ReadAllTextAsync(executionInformation.settings.TagsName.ToString()).ConfigureAwait(default), EqualTo(@"<?xml version=""1.0"" encoding=""utf-8""?>
<Tags>
  <Tag>
    <Simple>
      <Name>Title</Name>
      <String>L'arrivée d'un train à La Ciotat</String>
    </Simple>
    <Simple>
      <Name>Years</Name>
      <String>1896</String>
    </Simple>
    <Simple>
      <Name>Genres</Name>
      <String>Documentary/Short/Action</String>
    </Simple>
    <Simple>
      <Name>Directors</Name>
      <String>Auguste Lumière/Louis Lumière</String>
    </Simple>
    <Simple>
      <Name>Actors</Name>
      <String>Madeleine Koehler/Marcel Koehler/Mrs. Auguste Lumiere/Jeanne-Joséphine Lumière/Rose Lumière/Suzanne Lumière</String>
    </Simple>
    <Simple>
      <Name>Languages</Name>
      <String>lt</String>
    </Simple>
  </Tag>
</Tags>"));
                // ReSharper restore StringLiteralTypo
            });
        }

        /// <summary>Single genre without separator.</summary>
        /// <returns><see cref="Task" /> to execute.</returns>
        [Test]
        public static async Task SingleGenre_NoSeparatorAdded() {
            var executionInformation = await ExecuteAsync("-Gather", "https://www.imdb.com/title/tt0000009", "-Language", "Lt").ConfigureAwait(default);
            That(executionInformation.settings.TagsName.Exists, Is.True, () => executionInformation.message);
            // ReSharper disable StringLiteralTypo
            That(await ReadAllTextAsync(executionInformation.settings.TagsName.ToString()).ConfigureAwait(default), EqualTo(@"<?xml version=""1.0"" encoding=""utf-8""?>
<Tags>
  <Tag>
    <Simple>
      <Name>Title</Name>
      <String>Miss Jerry</String>
    </Simple>
    <Simple>
      <Name>Years</Name>
      <String>1894</String>
    </Simple>
    <Simple>
      <Name>Genres</Name>
      <String>Romance</String>
    </Simple>
    <Simple>
      <Name>Directors</Name>
      <String>Alexander Black</String>
    </Simple>
    <Simple>
      <Name>Writers</Name>
      <String>Alexander Black</String>
    </Simple>
    <Simple>
      <Name>Actors</Name>
      <String>Blanche Bayliss/William Courtenay/Chauncey Depew</String>
    </Simple>
    <Simple>
      <Name>Languages</Name>
      <String>lt</String>
    </Simple>
  </Tag>
</Tags>"));
            // ReSharper restore StringLiteralTypo
        }

        /// <summary>Multiple directors separated.</summary>
        /// <returns><see cref="Task" /> to execute.</returns>
        [Test]
        public static async Task MultipleDirectors_DirectorsSeparated() {
            var executionInformation = await ExecuteAsync("-Gather", "https://www.imdb.com/title/tt0000007", "-Language", "Lt").ConfigureAwait(default);
            That(executionInformation.settings.TagsName.Exists, Is.True, () => executionInformation.message);
            // ReSharper disable StringLiteralTypo
            That(await ReadAllTextAsync(executionInformation.settings.TagsName.ToString()).ConfigureAwait(default), EqualTo(@"<?xml version=""1.0"" encoding=""utf-8""?>
<Tags>
  <Tag>
    <Simple>
      <Name>Title</Name>
      <String>Corbett and Courtney Before the Kinetograph</String>
    </Simple>
    <Simple>
      <Name>Years</Name>
      <String>1894</String>
    </Simple>
    <Simple>
      <Name>Genres</Name>
      <String>Short/Sport</String>
    </Simple>
    <Simple>
      <Name>Directors</Name>
      <String>William K.L. Dickson/William Heise</String>
    </Simple>
    <Simple>
      <Name>Actors</Name>
      <String>James J. Corbett/Peter Courtney</String>
    </Simple>
    <Simple>
      <Name>Languages</Name>
      <String>lt</String>
    </Simple>
  </Tag>
</Tags>"));
            // ReSharper restore StringLiteralTypo
        }

        /// <summary>Single writer without separator.</summary>
        /// <returns><see cref="Task" /> to execute.</returns>
        [Test]
        public static async Task HasWriter_WriterAddedAdded() {
            var executionInformation = await ExecuteAsync("-Gather", "https://www.imdb.com/title/tt0000009", "-Language", "Lt").ConfigureAwait(default);
            That(executionInformation.settings.TagsName.Exists, Is.True, () => executionInformation.message);
            // ReSharper disable StringLiteralTypo
            That(await ReadAllTextAsync(executionInformation.settings.TagsName.ToString()).ConfigureAwait(default), EqualTo(@"<?xml version=""1.0"" encoding=""utf-8""?>
<Tags>
  <Tag>
    <Simple>
      <Name>Title</Name>
      <String>Miss Jerry</String>
    </Simple>
    <Simple>
      <Name>Years</Name>
      <String>1894</String>
    </Simple>
    <Simple>
      <Name>Genres</Name>
      <String>Romance</String>
    </Simple>
    <Simple>
      <Name>Directors</Name>
      <String>Alexander Black</String>
    </Simple>
    <Simple>
      <Name>Writers</Name>
      <String>Alexander Black</String>
    </Simple>
    <Simple>
      <Name>Actors</Name>
      <String>Blanche Bayliss/William Courtenay/Chauncey Depew</String>
    </Simple>
    <Simple>
      <Name>Languages</Name>
      <String>lt</String>
    </Simple>
  </Tag>
</Tags>"));
            // ReSharper restore StringLiteralTypo
        }

        /// <summary>Multiple writers separated.</summary>
        /// <returns><see cref="Task" /> to execute.</returns>
        [Test]
        public static async Task MultipleWriters_WritersSeparated() {
            var executionInformation = await ExecuteAsync("-Gather", "https://www.imdb.com/title/tt0000247", "-Language", "Lt").ConfigureAwait(default);
            That(executionInformation.settings.TagsName.Exists, Is.True, () => executionInformation.message);
            // ReSharper disable StringLiteralTypo
            That(await ReadAllTextAsync(executionInformation.settings.TagsName.ToString()).ConfigureAwait(default), EqualTo(@"<?xml version=""1.0"" encoding=""utf-8""?>
<Tags>
  <Tag>
    <Simple>
      <Name>Title</Name>
      <String>King John</String>
    </Simple>
    <Simple>
      <Name>Years</Name>
      <String>1899</String>
    </Simple>
    <Simple>
      <Name>Genres</Name>
      <String>Short/Drama</String>
    </Simple>
    <Simple>
      <Name>Directors</Name>
      <String>Walter Pfeffer Dando/William K.L. Dickson/Herbert Beerbohm Tree</String>
    </Simple>
    <Simple>
      <Name>Writers</Name>
      <String>William Shakespeare/Herbert Beerbohm Tree</String>
    </Simple>
    <Simple>
      <Name>Actors</Name>
      <String>Herbert Beerbohm Tree/Dora Tulloch/Charles Sefton/J. Fisher White/S.A. Cookson/Franklyn McLeay/Lewis Waller/Julia Neilson/William Mollison/Gerald Lawrence/Louis Calvert/Norman McKinnel</String>
    </Simple>
    <Simple>
      <Name>Languages</Name>
      <String>lt</String>
    </Simple>
  </Tag>
</Tags>"));
            // ReSharper restore StringLiteralTypo
        }

        /// <summary>Multiple actors separated.</summary>
        /// <returns><see cref="Task" /> to execute.</returns>
        [Test]
        public static async Task MultipleActors_ActorsSeparated() {
            var executionInformation = await ExecuteAsync("-Gather", "https://www.imdb.com/title/tt0000005", "-Language", "Lt").ConfigureAwait(default);
            That(executionInformation.settings.TagsName.Exists, Is.True, () => executionInformation.message);
            // ReSharper disable StringLiteralTypo
            That(await ReadAllTextAsync(executionInformation.settings.TagsName.ToString()).ConfigureAwait(default), EqualTo(@"<?xml version=""1.0"" encoding=""utf-8""?>
<Tags>
  <Tag>
    <Simple>
      <Name>Title</Name>
      <String>Blacksmith Scene</String>
    </Simple>
    <Simple>
      <Name>Years</Name>
      <String>1893</String>
    </Simple>
    <Simple>
      <Name>Genres</Name>
      <String>Short/Comedy</String>
    </Simple>
    <Simple>
      <Name>Directors</Name>
      <String>William K.L. Dickson</String>
    </Simple>
    <Simple>
      <Name>Actors</Name>
      <String>Charles Kayser/John Ott</String>
    </Simple>
    <Simple>
      <Name>Languages</Name>
      <String>lt</String>
    </Simple>
  </Tag>
</Tags>"));
            // ReSharper restore StringLiteralTypo
        }

        /// <summary>Has actors with rest of cast part.</summary>
        /// <returns><see cref="Task" /> to execute.</returns>
        [Test]
        public static async Task HasRestActorsPart_ActorsAdded() {
            var executionInformation = await ExecuteAsync("-Gather", "https://www.imdb.com/title/tt0000420", "-Language", "Lt").ConfigureAwait(default);
            That(executionInformation.settings.TagsName.Exists, Is.True, () => executionInformation.message);
            // ReSharper disable StringLiteralTypo
            That(await ReadAllTextAsync(executionInformation.settings.TagsName.ToString()).ConfigureAwait(default), EqualTo(@"<?xml version=""1.0"" encoding=""utf-8""?>
<Tags>
  <Tag>
    <Simple>
      <Name>Title</Name>
      <String>Alice in Wonderland</String>
    </Simple>
    <Simple>
      <Name>Years</Name>
      <String>1903</String>
    </Simple>
    <Simple>
      <Name>Genres</Name>
      <String>Fantasy/Short</String>
    </Simple>
    <Simple>
      <Name>Directors</Name>
      <String>Cecil M. Hepworth/Percy Stow</String>
    </Simple>
    <Simple>
      <Name>Writers</Name>
      <String>Lewis Carroll/Cecil M. Hepworth</String>
    </Simple>
    <Simple>
      <Name>Actors</Name>
      <String>May Clark/Cecil M. Hepworth/Blair/Geoffrey Faithfull/Stanley Faithfull/Mrs. Hepworth/Norman Whitten</String>
    </Simple>
    <Simple>
      <Name>Languages</Name>
      <String>lt</String>
    </Simple>
  </Tag>
</Tags>"));
            // ReSharper restore StringLiteralTypo
        }

        /// <summary>Title with invalid characters for file path.</summary>
        /// <returns><see cref="Task" /> to execute.</returns>
        [Test]
        public static async Task TitleContainsInvalidPathCharacters_CharactersRemovedFromFileNameButLeftInTags() {
            var executionInformation = await ExecuteAsync("-Gather", "https://www.imdb.com/title/tt0000013", "-Language", "Lt").ConfigureAwait(default);
            Multiple(async () => {
                // ReSharper disable StringLiteralTypo
                That(executionInformation.message,
                    EqualTo(Combine(executionInformation.settings.MoviesDirectory.ToString(),
                        "Neuville-sur-Saône Débarquement du congrès des photographes à Lyon (1895); Documentary, Short; LT.mkv")));
                That(await ReadAllTextAsync(executionInformation.settings.TagsName.ToString()).ConfigureAwait(default), EqualTo(@"<?xml version=""1.0"" encoding=""utf-8""?>
<Tags>
  <Tag>
    <Simple>
      <Name>Title</Name>
      <String>Neuville-sur-Saône: Débarquement du congrès des photographes à Lyon</String>
    </Simple>
    <Simple>
      <Name>Years</Name>
      <String>1895</String>
    </Simple>
    <Simple>
      <Name>Genres</Name>
      <String>Documentary/Short</String>
    </Simple>
    <Simple>
      <Name>Directors</Name>
      <String>Louis Lumière</String>
    </Simple>
    <Simple>
      <Name>Actors</Name>
      <String>Auguste Lumière/P.J.C. Janssen</String>
    </Simple>
    <Simple>
      <Name>Languages</Name>
      <String>lt</String>
    </Simple>
  </Tag>
</Tags>"));
                // ReSharper restore StringLiteralTypo
            });
        }
    }
}