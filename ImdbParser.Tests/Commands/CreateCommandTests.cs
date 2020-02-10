using ImdbParser.Commands;
using ImdbParser.Options;
using NUnit.Framework;
using System;
using System.Diagnostics.CodeAnalysis;
using static ImdbParser.Commands.CreateCommand;
using static ImdbParser.Messages;
using static ImdbParser.Options.OptionId;
using static ImdbParser.Tests.TestsBase;
using static NUnit.Framework.Assert;
using static NUnit.Framework.Is;
using static System.Array;

namespace ImdbParser.Tests.Commands {
    /// <summary><see cref="CreateCommand" /> tests.</summary>
    [ExcludeFromCodeCoverage]
    static class CreateCommandTests {
#region Create tests
        /// <summary><see cref="Create" /> test.</summary>
        [Test]
        public static void Create_ParametersContainsNoParameters_FailureReturned() =>
            Create(Empty<string>(), Empty<IOption>()).ExpectFail(() => Invalid_command_parameters_count.InvariantCultureFormat(CommandId.Create, 1));

        /// <summary><see cref="Create" /> test.</summary>
        [Test]
        public static void Create_FailedToGetUrl_FailureReturned() => Create(new[] { "Parameter" }, Empty<IOption>()).ExpectFail(() => String_not_URL);

        /// <summary><see cref="Create" /> test.</summary>
        [Test]
        public static void Create_ImdbMovieUrl_CreateCommandCreated() =>
            Create(new[] { ImdbTestUrl }, new IOption[] { CreateLanguage(), CreateLink() }).ExpectSuccess(command => That(command.Url, EqualTo(new Uri(ImdbTestUrl))));

        /// <summary><see cref="Create" /> test.</summary>
        [Test]
        public static void Create_DuplicateOptions_FailureReturned() {
            var option = CreateLanguage();

            Create(new[] { ImdbTestUrl }, new[] { option, option }).ExpectFail(() => All_options_must_be_unique);
        }

        /// <summary><see cref="Create" /> test.</summary>
        [Test]
        public static void Create_NoOptions_FailureReturned() => Create(new[] { ImdbTestUrl }, Empty<IOption>()).
            ExpectFail(() => Command_must_have_option.InvariantCultureFormat(CommandId.Create, ", ".Join(OptionId.Language, Link)));

        /// <summary><see cref="Create" /> test.</summary>
        [Test]
        public static void Create_NoLanguageOption_FailureReturned() => Create(new[] { ImdbTestUrl }, new[] { CreateLink() }).
            ExpectFail(() => Command_must_have_option.InvariantCultureFormat(CommandId.Create, OptionId.Language));

        /// <summary><see cref="Create" /> test.</summary>
        [Test]
        public static void Create_NoLinkOption_FailureReturned() =>
            Create(new[] { ImdbTestUrl }, new[] { CreateLanguage() }).ExpectFail(() => Command_must_have_option.InvariantCultureFormat(CommandId.Create, Link));

        /// <summary><see cref="Create" /> test.</summary>
        [Test]
        public static void Create_MinimalOptionsPassed_OptionSets() {
            var language = CreateLanguage();
            var link = CreateLink();

            Create(new[] { ImdbTestUrl }, new IOption[] { language, link }).ExpectSuccess(command => Multiple(() => {
                That(command.Language, SameAs(language));
                That(command.Link, SameAs(link));
                That(command.Subtitles, Is.Null);
            }));
        }

        /// <summary><see cref="Create" /> test.</summary>
        [Test]
        public static void Create_AllOptionsPassed_OptionSets() {
            var language = CreateLanguage();
            var link = CreateLink();
            var subtitles = CreateSubtitles();

            Create(new[] { ImdbTestUrl }, new IOption[] { language, link, subtitles }).ExpectSuccess(command => Multiple(() => {
                That(command.Language, SameAs(language));
                That(command.Link, SameAs(link));
                That(command.Subtitles, SameAs(subtitles));
            }));
        }
#endregion
    }
}