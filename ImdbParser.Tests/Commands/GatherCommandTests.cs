using ImdbParser.Commands;
using ImdbParser.Options;
using NUnit.Framework;
using System;
using System.Diagnostics.CodeAnalysis;
using static ImdbParser.Commands.GatherCommand;
using static ImdbParser.Messages;
using static ImdbParser.Tests.TestsBase;
using static NUnit.Framework.Assert;
using static NUnit.Framework.Is;
using static System.Array;

namespace ImdbParser.Tests.Commands {
    /// <summary><see cref="GatherCommand" /> tests.</summary>
    [ExcludeFromCodeCoverage]
    static class GatherCommandTests {
#region Create tests
        /// <summary><see cref="Create" /> test.</summary>
        [Test]
        public static void Create_ParametersContainsNoParameters_FailureReturned() =>
            Create(Empty<string>(), Empty<IOption>()).ExpectFail(() => Invalid_command_parameters_count.InvariantCultureFormat(CommandId.Gather, 1));

        /// <summary><see cref="Create" /> test.</summary>
        [Test]
        public static void Create_FailedToGetUrl_FailureReturned() => Create(new[] { "Parameter" }, Empty<IOption>()).ExpectFail(() => String_not_URL);

        /// <summary><see cref="Create" /> test.</summary>
        [Test]
        public static void Create_ImdbMovieUrl_GatherCommandCreated() =>
            Create(new[] { ImdbTestUrl }, new[] { CreateLanguage() }).ExpectSuccess(command => That(command.Url, EqualTo(new Uri(ImdbTestUrl))));

        /// <summary><see cref="Create" /> test.</summary>
        [Test]
        public static void Create_DuplicateOptions_FailureReturned() {
            var option = CreateLanguage();

            Create(new[] { ImdbTestUrl }, new[] { option, option }).ExpectFail(() => All_options_must_be_unique);
        }

        /// <summary><see cref="Create" /> test.</summary>
        [Test]
        public static void Create_NoLanguageOption_FailureReturned() =>
            Create(new[] { ImdbTestUrl }, Empty<IOption>()).ExpectFail(() => Command_must_have_option.InvariantCultureFormat(CommandId.Gather, OptionId.Language));

        /// <summary><see cref="Create" /> test.</summary>
        [Test]
        public static void Create_OnlyLanguageOptionPassed_OnlyLanguageOptionSet() {
            var option = CreateLanguage();

            Create(new[] { ImdbTestUrl }, new[] { option }).ExpectSuccess(command => Multiple(() => {
                That(command.Language, SameAs(option));
                That(command.Subtitles, Is.Null);
            }));
        }

        /// <summary><see cref="Create" /> test.</summary>
        [Test]
        public static void Create_SubtitlesSubCommandPassed_SubtitlesSubCommandSet() {
            var language = CreateLanguage();
            var subtitles = CreateSubtitles();

            Create(new[] { ImdbTestUrl }, new[] { language, subtitles }).ExpectSuccess(command => Multiple(() => {
                That(command.Language, SameAs(language));
                That(command.Subtitles, SameAs(subtitles));
            }));
        }
#endregion
    }
}