using NUnit.Framework;
using System.Diagnostics.CodeAnalysis;
using System.IO;
using static NUnit.Framework.Assert;
using static NUnit.Framework.Is;
using static System.IO.Path;

namespace ImdbParser.Tests.Wrappers {
    /// <summary><see cref="FileInfoWrapper" /> tests.</summary>
    [ExcludeFromCodeCoverage]
    static class FileInfoWrapperTests {
#region Constructor tests
        /// <summary><see cref="FileInfoWrapper(FileInfo)" /> test.</summary>
        [Test]
        public static void Constructor_ValidArgumentPassed_NoExceptionThrown() => That(new FileInfoWrapper(new FileInfo(TestsBase.Path)), Not.Null);
#endregion

#region Delete tests
        /// <summary><see cref="FileInfoWrapper.Delete" /> test.</summary>
        [Test]
        public static void Delete_Called_FileDeleted() {
            var file = new FileInfo(GetTempFileName());

            new FileInfoWrapper(file).Delete();

            That(file.Exists, Is.False);
        }
#endregion
    }
}