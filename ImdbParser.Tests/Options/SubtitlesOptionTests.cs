using ImdbParser.Options;
using NUnit.Framework;
using System;
using System.Diagnostics.CodeAnalysis;
using static ImdbParser.Language;
using static ImdbParser.Messages;
using static ImdbParser.Options.OptionId;
using static ImdbParser.Options.SubtitlesOption;
using static NUnit.Framework.Assert;
using static NUnit.Framework.Is;
using static System.Array;

namespace ImdbParser.Tests.Options {
    /// <summary><see cref="SubtitlesOption" /> tests.</summary>
    [ExcludeFromCodeCoverage]
    static class SubtitlesOptionTests {
#region CreateOption tests
        /// <summary><see cref="CreateOption" /> test.</summary>
        [Test]
        public static void CreateOption_ContainsNoParameters_FailureReturned() => CreateOption(Empty<string>()).ExpectFail(() => Option_has_no_language.InvariantCultureFormat(Subtitles));

        /// <summary><see cref="CreateOption" /> test.</summary>
        [Test]
        public static void CreateOption_ParameterIsNotLanguage_FailureReturned() => CreateOption(new[] { "Parameter" }).ExpectFail(() => String_not_language.InvariantCultureFormat(Subtitles));

        /// <summary><see cref="CreateOption" /> test.</summary>
        [Test]
        public static void CreateOption_NoneLanguagePassed_FailureReturned() =>
            CreateOption(new[] { Language.None.ToString() }).ExpectFail(() => String_not_language.InvariantCultureFormat(Subtitles));

        /// <summary><see cref="CreateOption" /> test.</summary>
        [Test]
        public static void CreateOption_DuplicateLanguage_FailureReturned() {
            var language = Lt.ToString();

            CreateOption(new[] { language, language }).ExpectFail(() => Language_not_unique);
        }

        /// <summary><see cref="CreateOption" /> test.</summary>
        [Test]
        public static void CreateOption_ValidLanguagesPassed_OptionCreated() =>
            CreateOption(new[] { Lt.ToString(), En.ToString() }).ExpectSuccess(option => That(option.Languages, EqualTo(new[] { Lt, En })));

        /// <summary><see cref="CreateOption" /> test.</summary>
        [Test]
        public static void CreateOption_LanguagesPassed_LanguagesOrdered() =>
            CreateOption(new[] { En.ToString(), Lt.ToString() }).ExpectSuccess(option => That(option.Languages, EqualTo(new[] { Lt, En })));
#endregion
    }
}