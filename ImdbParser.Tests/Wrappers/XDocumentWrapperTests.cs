using NUnit.Framework;
using System.Diagnostics.CodeAnalysis;
using System.IO;
using System.Xml.Linq;
using static NUnit.Framework.Assert;
using static NUnit.Framework.Is;
using static System.IO.Directory;
using static System.IO.Path;

namespace ImdbParser.Tests.Wrappers {
    /// <summary><see cref="XDocumentWrapper" /> tests.</summary>
    [ExcludeFromCodeCoverage]
    static class XDocumentWrapperTests {
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
        /// <summary><see cref="XDocumentWrapper(XDocument)" /> tests.</summary>
        [Test]
        public static void Constructor_DocumentPassed_NoExceptionThrown() {
            var document = Setup().document;

            That(new XDocumentWrapper(document), Not.Null);
        }
#endregion

#region Save tests
        /// <summary><see cref="XDocumentWrapper.Save" /> test.</summary>
        [Test]
        public static void Save_Called_DocumentSaved() {
            var setup = Setup();
            setup.document.Add(new XElement("Name"));
            var fileName = Combine(_directory!.FullName, GetRandomFileName());

            setup.wrapper.Save(fileName);

            That(File.Exists(fileName), Is.True);
        }
#endregion

#region ToString tests
        /// <summary><see cref="XDocumentWrapper.ToString" /> test.</summary>
        [Test]
        public static void ToString_Called_SameValueReturned() {
            var setup = Setup();

            That(setup.wrapper.ToString, SameAs(setup.document.ToString()));
        }
#endregion

        /// <summary>Single test setup.</summary>
        /// <returns><see cref="object" /> to test with the <see cref="object" /> to wrap.</returns>
        static (XDocumentWrapper wrapper, XDocument document) Setup() {
            var document = new XDocument();
            return (new XDocumentWrapper(document), document);
        }
    }
}