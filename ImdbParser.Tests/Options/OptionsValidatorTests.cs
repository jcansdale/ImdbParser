using ImdbParser.Options;
using NUnit.Framework;
using System.Diagnostics.CodeAnalysis;
using static NUnit.Framework.Assert;
using static System.Array;

namespace ImdbParser.Tests.Options {
    /// <summary><see cref="OptionsValidator" /> tests.</summary>
    [ExcludeFromCodeCoverage]
    static class OptionsValidatorTests {
        /// <summary>First option used for testing.</summary>
        sealed class FirstOption : IOption {}

        /// <summary>Second option used for testing.</summary>
        sealed class SecondOption : IOption {}

#region AreUnique tests
        /// <summary><see cref="OptionsValidator.AreUnique" /> test.</summary>
        [Test]
        public static void AreUnique_CollectionEmpty_TrueReturned() => That(Empty<IOption>().AreUnique(), Is.True);

        /// <summary><see cref="OptionsValidator.AreUnique" /> test.</summary>
        [Test]
        public static void AreUnique_ContainsSingleValue_TrueReturned() => That(new[] { new FirstOption() }.AreUnique(), Is.True);

        /// <summary><see cref="OptionsValidator.AreUnique" /> test.</summary>
        [Test]
        public static void AreUnique_ContainsUniqueValues_TrueReturned() => That(new IOption[] { new FirstOption(), new SecondOption() }.AreUnique(), Is.True);

        /// <summary><see cref="OptionsValidator.AreUnique" /> test.</summary>
        [Test]
        public static void AreUnique_ContainsDuplicateValues_FalseReturned() => That(new[] { new FirstOption(), new FirstOption() }.AreUnique(), Is.False);
#endregion
    }
}