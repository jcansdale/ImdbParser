using ImdbParser.Html;
using NUnit.Framework;
using System;
using System.Diagnostics.CodeAnalysis;
using static NUnit.Framework.Assert;
using static NUnit.Framework.Is;
using Throws = NUnit.Framework.Throws;

namespace ImdbParser.Tests.Html {
    /// <summary><see cref="InfoParserFactory" /> tests.</summary>
    [ExcludeFromCodeCoverage]
    static class InfoParserFactoryTests {
        /// <summary><see cref="object" /> to test.</summary>
        static readonly InfoParserFactory Factory = new InfoParserFactory();

#region CreateCrewAndCastParser tests
        /// <summary><see cref="InfoParserFactory.CreateCrewAndCastParser" /> tests.</summary>
        /// <param name="html">HTML to parse.</param>
        [TestCase(""), TestCase(" ")]
        public static void CreateCrewAndCastParser_HtmlInvalid_ThrowsArgumentException(string html) => That(() => Factory.CreateCrewAndCastParser(html),
            Throws.ArgumentException.With.Matches<ArgumentException>(exception => exception.ParamName == "html"));

        /// <summary><see cref="InfoParserFactory.CreateCrewAndCastParser" /> tests.</summary>
        [Test]
        public static void CreateCrewAndCastParser_HtmlPassed_ParserCreated() => That(Factory.CreateCrewAndCastParser("HTML"), TypeOf(typeof(CrewAndCastParser)));
#endregion

#region CreateMovieInfoParser tests
        /// <summary><see cref="InfoParserFactory.CreateMovieInfoParser" /> tests.</summary>
        /// <param name="html">HTML to parse.</param>
        [TestCase(""), TestCase(" ")]
        public static void CreateMovieInfoParser_HtmlInvalid_ThrowsArgumentException(string html) => That(() => Factory.CreateMovieInfoParser(html),
            Throws.ArgumentException.With.Matches<ArgumentException>(exception => exception.ParamName == "html"));

        /// <summary><see cref="InfoParserFactory.CreateMovieInfoParser" /> tests.</summary>
        [Test]
        public static void CreateMovieInfoParser_HtmlPassed_ParserCreated() => That(Factory.CreateMovieInfoParser("HTML"), TypeOf(typeof(MovieInfoParser)));
#endregion
    }
}