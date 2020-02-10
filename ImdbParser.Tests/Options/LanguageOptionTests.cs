using ImdbParser.Options;
using NUnit.Framework;
using System;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using static ImdbParser.Language;
using static ImdbParser.Messages;
using static ImdbParser.Options.LanguageOption;
using static NUnit.Framework.Assert;
using static NUnit.Framework.Is;
using static System.Array;

namespace ImdbParser.Tests.Options {
    /// <summary><see cref="LanguageOption" /> tests.</summary>
    [ExcludeFromCodeCoverage]
    static class LanguageOptionTests {
        /// <summary><see cref="LanguageOption" /> wrapper to access protected members.</summary>
        sealed class LanguageOptionWrapper : LanguageOption {
            /// <summary>Initializes new <see cref="LanguageOptionWrapper" /> instance.</summary>
            /// <param name="languages">Languages associated with this sub command.</param>
            internal LanguageOptionWrapper(IEnumerable<Language> languages) : base(languages) {}

            /// <summary>Tries to get valid unique <see cref="Language" />s from <paramref name="parameters" />.</summary>
            /// <param name="parameters">Options parameters.</param>
            /// <returns><see cref="Language" /> represented by <paramref name="parameters" />.</returns>
            internal static Result<HashSet<Language>> GetLanguagesWrapper(IEnumerable<string> parameters) => GetLanguages(parameters);
        }

#region Constructor tests
        /// <summary><see cref="LanguageOption(IEnumerable{Language})" /> test.</summary>
        [Test]
        public static void Constructor_LanguagesPassed_LanguagesSet() {
            var languages = new[] { Lt };

            That(new LanguageOptionWrapper(languages).Languages, EqualTo(languages));
        }
#endregion

#region CreateOption tests
        /// <summary><see cref="CreateOption" /> test.</summary>
        [Test]
        public static void CreateOption_ContainsNoParameters_FailureReturned() => CreateOption(Empty<string>()).ExpectFail(() => Option_has_no_language.InvariantCultureFormat(OptionId.Language));

        /// <summary><see cref="CreateOption" /> test.</summary>
        [Test]
        public static void CreateOption_ParameterIsNotLanguage_FailureReturned() => CreateOption(new[] { "Parameter" }).ExpectFail(() => String_not_language.InvariantCultureFormat(OptionId.Language));

        /// <summary><see cref="CreateOption" /> test.</summary>
        [Test]
        public static void CreateOption_NoneLanguagePassed_FailureReturned() => CreateOption(new[] { None.ToString() }).ExpectFail(() => String_not_language.InvariantCultureFormat(OptionId.Language));

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

#region GetLanguages tests
        /// <summary><see cref="GetLanguages" /> test.</summary>
        [Test]
        public static void GetLanguages_EmptyParametersPassed_EmptyResultReturned() => LanguageOptionWrapper.GetLanguagesWrapper(Empty<string>()).ExpectSuccess(set => That(set, Is.Empty));

        /// <summary><see cref="GetLanguages" /> test.</summary>
        [Test]
        public static void GetLanguages_ParameterIsNotLanguage_FailureReturned() =>
            LanguageOptionWrapper.GetLanguagesWrapper(new[] { "Parameter" }).ExpectFail(() => String_not_language.InvariantCultureFormat(OptionId.Language));

        /// <summary><see cref="GetLanguages" /> test.</summary>
        [Test]
        public static void GetLanguages_DuplicateLanguage_FailureReturned() {
            var language = Lt.ToString();

            LanguageOptionWrapper.GetLanguagesWrapper(new[] { language, language }).ExpectFail(() => Language_not_unique);
        }

        /// <summary><see cref="GetLanguages" /> test.</summary>
        [Test]
        public static void GetLanguages_ValidLanguagesPassed_LanguagesReturned() =>
            LanguageOptionWrapper.GetLanguagesWrapper(new[] { Lt.ToString(), En.ToString() }).ExpectSuccess(set => That(set, EqualTo(new[] { Lt, En })));
#endregion
    }
}