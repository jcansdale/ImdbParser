using NUnit.Framework;
using System.Diagnostics.CodeAnalysis;
using System.Drawing;
using System.IO;
using static NUnit.Framework.Assert;
using static NUnit.Framework.Is;
using static NUnit.Framework.Throws;
using static System.IO.Directory;
using static System.IO.Path;

namespace ImdbParser.Tests.Wrappers {
    /// <summary><see cref="ImageWrapper" /> tests.</summary>
    [ExcludeFromCodeCoverage]
    static class ImageWrapperTests {
        /// <summary>Directory used for testing.</summary>
#pragma warning disable 8632 // False positive.
        static DirectoryInfo? _directory;
#pragma warning restore 8632

        /// <summary>All tests setup.</summary>
        [OneTimeSetUp]
        public static void OnetimeSetup() => _directory = CreateDirectory(Combine(GetTempPath(), GetRandomFileName()));

        /// <summary>All tests cleanup.</summary>
        [OneTimeTearDown]
        public static void OnetimeTeardown() => _directory!.Delete(true);

#region Constructor tests
        /// <summary><see cref="ImageWrapper(Image)" /> test.</summary>
        [Test]
        public static void Constructor_ImagePassed_NoExceptionThrown() {
            var setup = Setup();
            using var _ = setup.wrapper;
            using var image = setup.image;

            using var wrapper = new ImageWrapper(image);

            That(wrapper, Not.Null);
        }
#endregion

#region Dispose tests
        /// <summary><see cref="ImageWrapper.Dispose" /> test.</summary>
        [Test]
        public static void Dispose_Called_ObjectDisposed() {
            var setup = Setup();
            using var wrapper = setup.wrapper;
            using var _ = setup.image;

            That(wrapper.Dispose, Nothing);
        }
#endregion

#region Save tests
        /// <summary><see cref="ImageWrapper.Save" /> test.</summary>
        [Test]
        public static void Save_Called_ImageSaved() {
            var setup = Setup();
            using var wrapper = setup.wrapper;
            using var _ = setup.image;
            var fileName = Combine(_directory!.FullName, GetRandomFileName());

            wrapper.Save(fileName);

            That(File.Exists(fileName), Is.True);
        }
#endregion

        /// <summary>Single test setup.</summary>
        /// <returns><see cref="object" /> to test with the <see cref="object" /> to wrap.</returns>
        static (ImageWrapper wrapper, Image image) Setup() {
            var image = new Bitmap(1, 1);
            return (new ImageWrapper(image), image);
        }
    }
}