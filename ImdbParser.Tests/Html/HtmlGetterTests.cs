using ImdbParser.Html;
using NUnit.Framework;
using System;
using System.Diagnostics.CodeAnalysis;
using System.Threading.Tasks;
using static NUnit.Framework.Assert;
using static NUnit.Framework.Does;
using static System.Environment;

namespace ImdbParser.Tests.Html {
    /// <summary><see cref="HtmlGetter" /> tests.</summary>
    [ExcludeFromCodeCoverage]
    static class HtmlGetterTests {
        /// <summary><see cref="object" /> to test.</summary>
        static readonly HtmlGetter Getter = new HtmlGetter();

#region GetHtmlAsync tests
        /// <summary><see cref="HtmlGetter.GetHtmlAsync" /></summary>
        /// <returns><see cref="Task" /> to execute.</returns>
        [Test]
        public static async Task GetHtmlAsync_AddressPassed_ContentReturned() =>
            // ReSharper disable once StringLiteralTypo
            That((await Getter.GetHtmlAsync(new Uri("https://www.imdb.com")).ConfigureAwait(default)).TrimStart(NewLine.ToCharArray()), StartWith("<!doctype html>"));
#endregion

#region GetPictureAsync tests
        /// <summary><see cref="HtmlGetter.GetPictureAsync" /> test.</summary>
        /// <returns><see cref="Task" /> to execute.</returns>
        [Test]
        public static async Task GetPictureAsync_AddressPassed_ImageReturned() {
            using var image = await Getter.GetPictureAsync(new Uri("https://ia.media-imdb.com/images/G/01/imdb/images/navbar/imdbpro_logo_nb-720143162._CB306318304_.png")).ConfigureAwait(default);

            That(image, Is.Not.Null);
        }
#endregion
    }
}