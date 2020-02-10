using NUnit.Framework;
using System.Diagnostics.CodeAnalysis;
using System.IO;
using System.Linq;
using static NUnit.Framework.Assert;
using static NUnit.Framework.Is;
using static System.IO.Directory;
using static System.IO.Path;
using static System.IO.SearchOption;

namespace ImdbParser.Tests.Wrappers {
    /// <summary><see cref="DirectoryInfoWrapper" /> tests.</summary>
    [ExcludeFromCodeCoverage]
    static class DirectoryInfoWrapperTests {
        /// <summary>Test directory to use.</summary>
        static readonly string TestDirectory = Combine(new FileInfo(TestsBase.Path).DirectoryName, GetRandomFileName());

        /// <summary>All tests cleanup.</summary>
        [OneTimeTearDown]
        public static void OnetimeTeardown() => Delete(TestDirectory, true);

#region Constructor tests
        /// <summary><see cref="DirectoryInfoWrapper(DirectoryInfo)" /> test.</summary>
        [Test]
        public static void Constructor_ValidArgumentPassed_NoExceptionThrown() => That(new DirectoryInfoWrapper(new DirectoryInfo(TestsBase.Path)), Not.Null);
#endregion

#region Delete tests
        /// <summary><see cref="DirectoryInfoWrapper.Delete" /> test.</summary>
        [Test]
        public static void Delete_Called_DirectoryDeleted() {
            var directory = new DirectoryInfo(Combine(TestDirectory, GetRandomFileName()));
            directory.Create();

            new DirectoryInfoWrapper(directory).Delete();

            That(directory.Exists, Is.False);
        }
#endregion

#region EnumerateDirectories tests
        /// <summary><see cref="DirectoryInfoWrapper.EnumerateDirectories" /> test.</summary>
        [Test]
        public static void EnumerateDirectories_Called_ValidDirectoriesReturned() {
            var directory = new DirectoryInfo(GetDirectoryName(TestsBase.Path));

            That(new DirectoryInfoWrapper(directory).EnumerateDirectories("*", AllDirectories).Select(innerDirectory => innerDirectory.ToString()),
                EqualTo(directory.EnumerateDirectories("*", AllDirectories).Select(innerDirectory => innerDirectory.ToString())));
        }
#endregion

#region EnumerateFiles tests
        /// <summary><see cref="DirectoryInfoWrapper.EnumerateFiles" /> test.</summary>
        [Test]
        public static void EnumerateFiles_Called_ValidFilesReturned() {
            var directory = new DirectoryInfo(GetDirectoryName(TestsBase.Path));

            That(new DirectoryInfoWrapper(directory).EnumerateFiles("*").Select(file => file.FullName), EqualTo(directory.EnumerateFiles("*").Select(file => file.FullName)));
        }
#endregion

#region EnumerateFileSystemInfos tests
        /// <summary><see cref="DirectoryInfoWrapper.EnumerateFileSystemInfos" /> test.</summary>
        [Test]
        public static void EnumerateFileSystemInfos_Called_ValidItemsReturned() {
            var directory = new DirectoryInfo(GetDirectoryName(TestsBase.Path));

            That(new DirectoryInfoWrapper(directory).EnumerateFileSystemInfos().Select(info => info.FullName), EqualTo(directory.EnumerateFileSystemInfos().Select(info => info.FullName)));
        }
#endregion

#region ToString tests
        /// <summary><see cref="DirectoryInfoWrapper.ToString" /> test.</summary>
        [Test]
        public static void ToString_Called_SameValuesReturned() {
            var directory = new DirectoryInfo(TestsBase.Path);

            That(new DirectoryInfoWrapper(directory).ToString, SameAs(directory.ToString()));
        }
#endregion
    }
}