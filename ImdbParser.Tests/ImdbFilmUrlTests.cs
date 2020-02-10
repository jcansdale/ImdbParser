using NUnit.Framework;
using System;
using System.Diagnostics.CodeAnalysis;
using static ImdbParser.ImdbFilmUrl;
using static ImdbParser.Messages;
using static ImdbParser.Tests.TestsBase;
using static NUnit.Framework.Assert;
using static NUnit.Framework.Is;
using static System.FormattableString;

namespace ImdbParser.Tests {
    /// <summary><see cref="ImdbFilmUrl" /> tests.</summary>
    [ExcludeFromCodeCoverage]
    static class ImdbFilmUrlTests {
#region CreateUrl tests
        /// <summary><see cref="CreateUrl" /> test.</summary>
        [Test]
        public static void CreateUrl_UriStringIsNotUrl_FailureReturned() => CreateUrl(default!).ExpectFail(() => String_not_URL);

        /// <summary><see cref="CreateUrl" /> test.</summary>
        [TestCase("http://www.imdb.com/title/tt"), TestCase(ImdbTestUrl + "/a"), TestCase(ImdbTestUrl + "a")]
        public static void CreateUrl_NotImdbMovieUrl_FailureReturned(string address) => CreateUrl(address).ExpectFail(() => URL_is_not_IMDB_movie);

        /// <summary><see cref="CreateUrl" /> test.</summary>
        [TestCase(ImdbTestUrl), TestCase("https://www.imdb.com/title/tt1"), TestCase(ImdbTestUrl + "1"), TestCase(ImdbTestUrl + "/"), TestCase(ImdbTestUrl + "?Q"), TestCase(ImdbTestUrl + "/?Q"),
         TestCase("HTTP://WWW.IMDB.COM/TITLE/TT1")]
        public static void CreateCommand_ImdbMovieUrl_GatherCommandCreated(string address) => CreateUrl(address).ExpectSuccess(url => That(url, EqualTo(new Uri(address))));
#endregion

#region CrewAndCastUrl tests
        /// <summary><see cref="ImdbFilmUrl.CrewAndCastUrl" /> test.</summary>
        [Test]
        public static void CrewAndCastUrl_DoesNotHaveTrailingSlash_ValidUrlReturned() =>
            // ReSharper disable once StringLiteralTypo
            CreateUrl(ImdbTestUrl).ExpectSuccess(url => That(url.CrewAndCastUrl.ToString, EqualTo(Invariant($"{ImdbTestUrl}/fullcredits"))));

        /// <summary><see cref="ImdbFilmUrl.CrewAndCastUrl" /> test.</summary>
        [Test]
        public static void CrewAndCastUrl_HaveTrailingSlash_ValidUrlReturned() =>
            // ReSharper disable once StringLiteralTypo
            CreateUrl(Invariant($"{ImdbTestUrl}/")).ExpectSuccess(url => That(url.CrewAndCastUrl.ToString, EqualTo(Invariant($"{ImdbTestUrl}/fullcredits"))));
#endregion
    }
}