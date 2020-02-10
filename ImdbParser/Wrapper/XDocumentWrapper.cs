using System.Diagnostics;

// ReSharper disable once CheckNamespace
namespace System.Xml.Linq {
    /// <summary>Represents an XML document.</summary>
    interface IXDocument {
        /// <summary>Serialize this <see cref="IXDocument" /> to a file, overwriting an existing file, if it exists.</summary>
        /// <param name="fileName">A <see cref="string" /> that contains the name of the file.</param>
        void Save(string fileName);

        /// <summary>Returns the indented XML for this node.</summary>
        /// <returns>A <see cref="string" /> containing the indented XML.</returns>
        string ToString();
    }

    /// <summary>Represents an XML document.</summary>
    /// <remarks><see cref="XDocument" /> wrapper.</remarks>
    [DebuggerStepThrough]
    sealed class XDocumentWrapper : IXDocument {
        /// <summary><see cref="XDocument" /> to wrap.</summary>
        readonly XDocument _document;

        /// <summary>Initialize new <see cref="XDocumentWrapper" /> instance.</summary>
        /// <param name="document"><see cref="XDocument" /> to wrap.</param>
        internal XDocumentWrapper(XDocument document) => _document = document;

        /// <summary>Serialize this <see cref="IXDocument" /> to a file, overwriting an existing file, if it exists.</summary>
        /// <param name="fileName">A <see cref="string" /> that contains the name of the file.</param>
        public void Save(string fileName) => _document.Save(fileName);

        /// <summary>Returns the indented XML for this node.</summary>
        /// <returns>A <see cref="string" /> containing the indented XML.</returns>
        public override string ToString() => _document.ToString();
    }
}