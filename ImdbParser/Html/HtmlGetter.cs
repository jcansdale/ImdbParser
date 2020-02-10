using System;
using System.Drawing;
using System.IO;
using System.Threading.Tasks;
using static System.Drawing.Image;
using static System.Net.WebRequest;

namespace ImdbParser.Html {
    /// <summary>Gets HTML content.</summary>
    interface IHtmlGetter {
        /// <summary>Reads HTML content from <paramref name="address" />.</summary>
        /// <param name="address">Address from where to read HTML content.</param>
        /// <returns>Content read from <paramref name="address" />.</returns>
        Task<string> GetHtmlAsync(Uri address);

        /// <summary>Downloads picture from <paramref name="address" />.</summary>
        /// <param name="address">Address of picture to download.</param>
        /// <returns>Downloaded picture.</returns>
        Task<Image> GetPictureAsync(Uri address);
    }

    /// <summary>Gets HTML content.</summary>
    sealed class HtmlGetter : IHtmlGetter {
        /// <summary>Reads HTML content from <paramref name="address" />.</summary>
        /// <param name="address">Address from where to read HTML content.</param>
        /// <returns>Content read from <paramref name="address" />.</returns>
        public async Task<string> GetHtmlAsync(Uri address) {
            using var response = await Create(address).GetResponseAsync().ConfigureAwait(default);
            await using var stream = response.GetResponseStream();
            // ReSharper disable once AssignNullToNotNullAttribute
            using var reader = new StreamReader(stream);
            return await reader.ReadToEndAsync().ConfigureAwait(default);
        }

        /// <summary>Downloads picture from <paramref name="address" />.</summary>
        /// <param name="address">Address of picture to download.</param>
        /// <returns>Downloaded picture.</returns>
        public async Task<Image> GetPictureAsync(Uri address) {
            using var response = await Create(address).GetResponseAsync().ConfigureAwait(default);
            return FromStream(response.GetResponseStream());
        }
    }
}