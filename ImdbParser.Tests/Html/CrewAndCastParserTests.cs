using ImdbParser.Html;
using NUnit.Framework;
using System;
using System.Diagnostics.CodeAnalysis;
using static NUnit.Framework.Assert;
using static NUnit.Framework.Is;
using static System.FormattableString;
using Throws = NUnit.Framework.Throws;

namespace ImdbParser.Tests.Html {
    /// <summary><see cref="CrewAndCastParser" /> tests.</summary>
    [ExcludeFromCodeCoverage]
    static class CrewAndCastParserTests {
#region Constructor tests
        /// <summary><see cref="CrewAndCastParser(string)" /> test.</summary>
        /// <param name="html">HTML content to parse.</param>
        [TestCase(""), TestCase(" ")]
        public static void Constructor_HtmlInvalid_ThrowsArgumentException(string html) =>
            That(() => new CrewAndCastParser(html), Throws.ArgumentException.With.Matches<ArgumentException>(exception => exception.ParamName == "html"));

        /// <summary><see cref="CrewAndCastParser(string)" /> test.</summary>
        [Test]
        public static void Constructor_ValidArgumentsPassed_NoExceptionThrown() => That(new CrewAndCastParser("HTML"), Not.Null);
#endregion

#region ParseCast tests
        /// <summary><see cref="CrewAndCastParser.ParseCast" /> test.</summary>
        [Test]
        public static void ParseCast_ActorsNotFound_NoValueReturned() => That(new CrewAndCastParser("HTML").ParseCast, Empty);

        /// <summary><see cref="CrewAndCastParser.ParseCast" /> test.</summary>
        [Test]
        public static void ParseCast_SingleActorFound_SingleActorReturned() {
            const string Actor = "Actor";

            // ReSharper disable once StringLiteralTypo
            That(new CrewAndCastParser(Invariant($@"<table class=""cast_list"">
  <tbody>
    <tr>
      <td ></td>
    </tr>
    <tr>
      <td ></td>
      <td><a href=""/name/nm1588970/?ref_=ttfc_fc_cl_t1""> {Actor} </a></td>
      <td >...</td>
      <td ></td>
    </tr>
  </tbody>
</table>")).ParseCast, EqualTo(new[] { Actor }));
        }

        /// <summary><see cref="CrewAndCastParser.ParseCast" /> test.</summary>
        [Test]
        public static void ParseCast_MultipleActorsFound_MultipleActorsReturned() {
            const string Actor1 = "Actor1";
            const string Actor2 = "Actor2";

            // ReSharper disable StringLiteralTypo
            That(new CrewAndCastParser(Invariant($@"<table class=""cast_list"">
  <tbody>
    <tr>
      <td ></td>
    </tr>
    <tr>
      <td ></td>
      <td><a href=""/name/nm1588970/?ref_=ttfc_fc_cl_t1""> {Actor1} </a></td>
      <td >...</td>
      <td ></td>
    </tr>
    <tr>
      <td ></td>
      <td><a href=""/name/nm1588970/?ref_=ttfc_fc_cl_t1""> {Actor2} </a></td>
      <td >...</td>
      <td ></td>
    </tr>
  </tbody>
</table>")).ParseCast, EqualTo(new[] { "Actor1", "Actor2" }));
            // ReSharper restore StringLiteralTypo
        }

        /// <summary><see cref="CrewAndCastParser.ParseCast" /> test.</summary>
        [Test]
        public static void ParseCast_ActorHasNoLink_ActorReturned() {
            const string Actor = "Actor";

            That(new CrewAndCastParser(Invariant($@"<table class=""cast_list"">
  <tbody>
    <tr>
      <td ></td>
    </tr>
    <tr>
      <td ></td>
      <td> {Actor} </td>
      <td >...</td>
      <td ></td>
    </tr>
  </tbody>
</table>")).ParseCast, EqualTo(new[] { Actor }));
        }
#endregion

#region ParseDirectors tests
        /// <summary><see cref="CrewAndCastParser.ParseDirectors" /> test.</summary>
        [Test]
        public static void ParseDirectors_DirectorNotFound_NoValueReturned() => That(new CrewAndCastParser("HTML").ParseDirectors, Empty);

        /// <summary><see cref="CrewAndCastParser.ParseDirectors" /> test.</summary>
        /// <param name="html">HTML content to parse.</param>
        [TestCase("HTML Directed by Other HTML <class=\"name\" Inner content> Director < Content </table>"),
         TestCase("HTML Directed by Other HTML <class=\"name\" Inner content> <a Inner content href=\" Inner Content> Director < Content </table>")]
        public static void ParseDirectors_SingleDirectorFound_SingleDirectorReturned(string html) => That(new CrewAndCastParser(html).ParseDirectors, EqualTo(new[] { "Director" }));

        /// <summary><see cref="CrewAndCastParser.ParseDirectors" /> test.</summary>
        [Test]
        public static void ParseDirectors_MultipleDirectorsFound_MultipleDirectorsReturned() {
            const string Director1 = "Director1";
            const string Director2 = "Director2";

            That(new CrewAndCastParser("HTML Directed by Other HTML <class=\"name\" Inner content> " +
                    Invariant($"{Director1} < Content <class=\"name\" Inner content> <a Inner content href=\" Inner Content> {Director2} < Content </table>")).ParseDirectors,
                EqualTo(new[] { Director1, Director2 }));
        }
#endregion

#region ParseWriters tests
        /// <summary><see cref="CrewAndCastParser.ParseWriters" /> test.</summary>
        [Test]
        public static void ParseWriters_NoWriters_NoValueReturned() => That(new CrewAndCastParser("HTML").ParseWriters, Empty);

        /// <summary><see cref="CrewAndCastParser.ParseWriters" /> test.</summary>
        /// <param name="html">HTML content to parse.</param>
        [TestCase("HTML Writing Credits Other HTML <class=\"name\" Inner content> Writer < Content </table>"),
         TestCase("HTML Writing Credits Other HTML <class=\"name\" Inner content> <a Inner content href=\" Inner Content> Writer < Content </table>")]
        public static void ParseWriters_SingleWriterFound_SingleWriterReturned(string html) => That(new CrewAndCastParser(html).ParseWriters, EqualTo(new[] { "Writer" }));

        /// <summary><see cref="CrewAndCastParser.ParseWriters" /> test.</summary>
        [Test]
        public static void ParseWriters_MultipleWritersFound_MultipleWriterReturned() {
            const string Writer1 = "Writer1";
            const string Writer2 = "Writer2";

            That(new CrewAndCastParser("HTML Writing Credits Other HTML <class=\"name\" Inner content> " +
                    Invariant($"{Writer1} < Content <class=\"name\" Inner content> <a Inner content href=\" Inner Content> {Writer2} < Content </table>")).ParseWriters,
                EqualTo(new[] { Writer1, Writer2 }));
        }
#endregion
    }
}