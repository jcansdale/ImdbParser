using NUnit.Framework;
using System.Diagnostics.CodeAnalysis;
using System.IO;
using static NUnit.Framework.Assert;
using static NUnit.Framework.Is;
using static System.IO.Directory;
using static System.IO.Path;

namespace ImdbParser.Tests.Wrappers {
    /// <summary><see cref="DirectoryWrapper" /> tests.</summary>
    [ExcludeFromCodeCoverage]
    static class DirectoryWrapperTests {
        /// <summary>Test directory to use.</summary>
        static readonly string TestDirectory = Combine(new FileInfo(TestsBase.Path).DirectoryName, GetRandomFileName());

        /// <summary>All tests cleanup.</summary>
        [OneTimeTearDown]
        public static void OnetimeTeardown() => Delete(TestDirectory);

#region CreateDirectory tests
        /// <summary><see cref="DirectoryWrapper.CreateDirectory" /> test.</summary>
        [Test]
        public static void CreateDirectory_Called_DirectoryCreated() {
            That(new DirectoryWrapper().CreateDirectory(TestDirectory), Not.Null);
            That(Exists(TestDirectory), Is.True);
        }
#endregion
    }
}