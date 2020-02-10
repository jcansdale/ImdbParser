using System.Diagnostics;
using System.Drawing.Imaging;
using System.Runtime.InteropServices;

// ReSharper disable once CheckNamespace
namespace System.Drawing {
    /// <summary>Provides functionality for the <see cref="Bitmap" /> and <see cref="Metafile" /> descended classes.</summary>
    interface IImage : IDisposable {
        /// <summary>Saves this <see cref="IImage" /> to the specified file or stream.</summary>
        /// <param name="filename">A <see cref="string" /> that contains the name of the file to which to save this <see cref="IImage" />.</param>
        /// <exception cref="ArgumentNullException"><paramref name="filename" /> is null.</exception>
        /// <exception cref="ExternalException">The image was saved with the wrong image format. -or- The image was saved to the same file it was created from.</exception>
        void Save(string filename);
    }

    /// <summary>Provides functionality for the <see cref="Bitmap" /> and <see cref="Metafile" /> descended classes.</summary>
    /// <remarks><see cref="Image" /> wrapper.</remarks>
    [DebuggerStepThrough]
    sealed class ImageWrapper : IImage {
        /// <summary><see cref="Image" /> to wrap.</summary>
        readonly Image _image;

        /// <summary>Initialize new <see cref="ImageWrapper" /> instance.</summary>
        /// <param name="image"><see cref="Image" /> to wrap.</param>
        internal ImageWrapper(Image image) => _image = image;

        /// <summary>Performs application-defined tasks associated with freeing, releasing, or resetting unmanaged resources.</summary>
        public void Dispose() => _image.Dispose();

        /// <summary>Saves this <see cref="IImage" /> to the specified file or stream.</summary>
        /// <param name="filename">A <see cref="string" /> that contains the name of the file to which to save this <see cref="IImage" />.</param>
        /// <exception cref="ArgumentNullException"><paramref name="filename" /> is null.</exception>
        /// <exception cref="ExternalException">The image was saved with the wrong image format. -or- The image was saved to the same file it was created from.</exception>
        public void Save(string filename) => _image.Save(filename);
    }
}