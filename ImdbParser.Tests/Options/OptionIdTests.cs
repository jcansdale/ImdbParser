using ImdbParser.Options;
using NUnit.Framework;
using System.Diagnostics.CodeAnalysis;
using static ImdbParser.Options.OptionId;
using static NUnit.Framework.Assert;
using static NUnit.Framework.Is;

namespace ImdbParser.Tests.Options {
    /// <summary><see cref="OptionId" /> tests.</summary>
    [ExcludeFromCodeCoverage]
    static class OptionIdTests {
#region Language tests
        /// <summary><see cref="OptionId.Language" /> test.</summary>
        /// <param name="option">Option to convert to <see cref="string" />.</param>
        [TestCase(OptionId.Language), TestCase(La)]
        public static void Language_ToStringCalled_ValidValueReturned(OptionId option) => That(option.ToString, EqualTo("Language"));
#endregion

#region Link tests
        /// <summary><see cref="Link" /> test.</summary>
        /// <param name="option">Option to convert to <see cref="string" />.</param>
        [TestCase(Link), TestCase(Li)]
        public static void Link_ToStringCalled_ValidValueReturned(OptionId option) => That(option.ToString, EqualTo("Link"));
#endregion

#region Subtitles tests
        /// <summary><see cref="Subtitles" /> test.</summary>
        /// <param name="option">Option to convert to <see cref="string" />.</param>
        [TestCase(Subtitles), TestCase(S)]
        public static void Subtitles_ToStringCalled_ValidValueReturned(OptionId option) => That(option.ToString, EqualTo("Subtitles"));
#endregion
    }
}