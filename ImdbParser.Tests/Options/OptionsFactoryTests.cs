using ImdbParser.Options;
using NUnit.Framework;
using System;
using System.ComponentModel;
using System.Diagnostics.CodeAnalysis;
using static ImdbParser.Language;
using static ImdbParser.Messages;
using static ImdbParser.Options.OptionId;
using static ImdbParser.Tests.TestsBase;
using static NUnit.Framework.Assert;
using static NUnit.Framework.Is;
using static System.Array;
using static System.Int32;
using Throws = NUnit.Framework.Throws;

namespace ImdbParser.Tests.Options {
    /// <summary><see cref="OptionsFactory" /> tests.</summary>
    [ExcludeFromCodeCoverage]
    static class OptionsFactoryTests {
#region CreateOption tests
        /// <summary><see cref="OptionsFactory.CreateOption" /> test.</summary>
        [Test]
        public static void CreateOption_NoneType_ThrowsInvalidEnumArgumentException() => That(() => OptionId.None.CreateOption(default!),
            Throws.TypeOf<InvalidEnumArgumentException>().With.Matches<InvalidEnumArgumentException>(exception => exception.ParamName == "option"));

        /// <summary><see cref="OptionsFactory.CreateOption" /> test.</summary>
        [Test]
        public static void CreateOption_UnknownType_ThrowsNotImplementedException() => That(() => ((OptionId)MaxValue).CreateOption(Empty<string>()), Throws.TypeOf<NotImplementedException>());

        /// <summary><see cref="OptionsFactory.CreateOption" /> test.</summary>
        [Test]
        public static void CreateOption_CreateFailed_FailureReturned() =>
            OptionId.Language.CreateOption(Empty<string>()).ExpectFail(() => Option_has_no_language.InvariantCultureFormat(OptionId.Language));

        /// <summary><see cref="OptionsFactory.CreateOption" /> test.</summary>
        [Test]
        public static void CreateOption_LanguageOptionPassed_LanguageOptionReturned() =>
            OptionId.Language.CreateOption(new[] { Lt.ToString() }).ExpectSuccess(option => That(option, TypeOf(typeof(LanguageOption))));

        /// <summary><see cref="OptionsFactory.CreateOption" /> test.</summary>
        [Test]
        public static void CreateOption_LinkOptionPassed_LinkOptionReturned() => Link.CreateOption(new[] { Path }).ExpectSuccess(option => That(option, TypeOf(typeof(LinkOption))));

        /// <summary><see cref="OptionsFactory.CreateOption" /> test.</summary>
        [Test]
        public static void CreateOption_SubtitlesOptionPassed_SubtitlesOptionReturned() =>
            Subtitles.CreateOption(new[] { Lt.ToString() }).ExpectSuccess(option => That(option, TypeOf(typeof(SubtitlesOption))));
#endregion
    }
}