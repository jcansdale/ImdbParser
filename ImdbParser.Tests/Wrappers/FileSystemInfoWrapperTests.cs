using NUnit.Framework;
using System.Diagnostics.CodeAnalysis;
using System.IO;
using static NUnit.Framework.Assert;
using static NUnit.Framework.Is;

namespace ImdbParser.Tests.Wrappers {
    /// <summary><see cref="FileSystemInfoWrapper" /> tests.</summary>
    [ExcludeFromCodeCoverage]
    static class FileSystemInfoWrapperTests {
        /// <summary><see cref="FileSystemInfoWrapper" /> wrapper.</summary>
        sealed class FileSystemInfoWrapperWrapper : FileSystemInfoWrapper {
            /// <summary>Initialize new <see cref="FileSystemInfoWrapperWrapper" /> instance.</summary>
            /// <param name="info"><see cref="FileSystemInfo" /> to wrap.</param>
            internal FileSystemInfoWrapperWrapper(FileSystemInfo info) : base(info) {}
        }

#region Constructor tests
        /// <summary><see cref="FileSystemInfoWrapper(FileSystemInfo)" /> test.</summary>
        [Test]
        public static void Constructor_ValidArgumentPassed_NoExceptionThrown() => That(new FileSystemInfoWrapperWrapper(new FileInfo(TestsBase.Path)), Not.Null);
#endregion

#region FullName tests
        /// <summary><see cref="FileSystemInfoWrapper.FullName" /> test.</summary>
        [Test]
        public static void FullName_Called_ValidValueReturned() {
            var info = new FileInfo(TestsBase.Path);

            That(new FileSystemInfoWrapperWrapper(info).FullName, SameAs(info.FullName));
        }
#endregion
    }
}