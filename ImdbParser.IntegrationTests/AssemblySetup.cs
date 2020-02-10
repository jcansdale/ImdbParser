using NUnit.Framework;
using System.Diagnostics.CodeAnalysis;
using System.IO;
using static System.IO.Path;

namespace ImdbParser.IntegrationTests {
    /// <summary>All tests setup and cleanup.</summary>
    // ReSharper disable once StringLiteralTypo
    [ExcludeFromCodeCoverage, SetUpFixture, SuppressMessage("Performance", "CA1812:Avoid uninstantiated internal classes", Justification = "SetUpFixture must be non static.")]
    sealed class AssemblySetup {
        /// <summary>Directory where all tests should place their files.</summary>
        // ReSharper disable once RedundantDefaultMemberInitializer
        internal static DirectoryInfo WorkingDirectory { get; private set; } = default!;

        /// <summary>All tests setup.</summary>
        [OneTimeSetUp]
        public static void OneTimeSetup() {
            WorkingDirectory = new DirectoryInfo(Combine(GetTempPath(), GetRandomFileName()));
            WorkingDirectory.Create();
        }

        /// <summary>All tests teardown.</summary>
        [OneTimeTearDown]
        public static void OneTimeTeardown() => WorkingDirectory!.Delete(true);
    }
}