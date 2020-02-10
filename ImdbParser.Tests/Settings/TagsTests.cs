using ImdbParser.Settings;
using NUnit.Framework;
using System;
using System.Diagnostics.CodeAnalysis;
using static NUnit.Framework.Assert;
using Throws = NUnit.Framework.Throws;

namespace ImdbParser.Tests.Settings {
    /// <summary><see cref="Tags" /> tests.</summary>
    [ExcludeFromCodeCoverage]
    static class TagsTests {
#region Constructor tests
        /// <summary><see cref="Tags()" /> test.</summary>
        [Test]
        public static void Constructor_Called_AllValuesLeftAsNull() {
            var tags = new Tags();

            Multiple(() => {
                That(tags.Cast, Is.Null);
                That(tags.Directors, Is.Null);
                That(tags.Genres, Is.Null);
                That(tags.Languages, Is.Null);
                That(tags.Subtitles, Is.Null);
                That(tags.Title, Is.Null);
                That(tags.Writers, Is.Null);
                That(tags.Years, Is.Null);
            });
        }

        /// <summary><see cref="Tags(string, string, string, string, string, string, string, string)" /> test.</summary>
        /// <param name="cast">Tag used to name movie actors.</param>
        [TestCase(""), TestCase(" ")]
        public static void Constructor_CastInvalid_ThrowsArgumentException(string cast) => That(() => new Tags(cast, default!, default!, default!, default!, default!, default!, default!),
            Throws.ArgumentException.With.Matches<ArgumentException>(exception => exception.ParamName == "cast"));

        /// <summary><see cref="Tags(string, string, string, string, string, string, string, string)" /> test.</summary>
        /// <param name="directors">Tag used to name movie directors.</param>
        [TestCase(""), TestCase(" ")]
        public static void Constructor_DirectorsInvalid_ThrowsArgumentException(string directors) => That(() => new Tags("Cast", directors, default!, default!, default!, default!, default!, default!),
            Throws.ArgumentException.With.Matches<ArgumentException>(exception => exception.ParamName == "directors"));

        /// <summary><see cref="Tags(string, string, string, string, string, string, string, string)" /> test.</summary>
        /// <param name="genres">Tag used to name movie genres.</param>
        [TestCase(""), TestCase(" ")]
        public static void Constructor_GenresInvalid_ThrowsArgumentException(string genres) => That(() => new Tags("Cast", "Directors", genres, default!, default!, default!, default!, default!),
            Throws.ArgumentException.With.Matches<ArgumentException>(exception => exception.ParamName == "genres"));

        /// <summary><see cref="Tags(string, string, string, string, string, string, string, string)" /> test.</summary>
        /// <param name="years">Tag used to name movie years.</param>
        [TestCase(""), TestCase(" ")]
        public static void Constructor_YearsInvalid_ThrowsArgumentException(string years) => That(() => new Tags("Cast", "Directors", "Genres", years, default!, default!, default!, default!),
            Throws.ArgumentException.With.Matches<ArgumentException>(exception => exception.ParamName == "years"));

        /// <summary><see cref="Tags(string, string, string, string, string, string, string, string)" /> test.</summary>
        /// <param name="languages">Tag used to name movie languages.</param>
        [TestCase(""), TestCase(" ")]
        public static void Constructor_LanguagesInvalid_ThrowsArgumentException(string languages) =>
            That(() => new Tags("Cast", "Directors", "Genres", "Years", languages, default!, default!, default!),
                Throws.ArgumentException.With.Matches<ArgumentException>(exception => exception.ParamName == "languages"));

        /// <summary><see cref="Tags(string, string, string, string, string, string, string, string)" /> test.</summary>
        /// <param name="subtitles">Tag used to name movie subtitles.</param>
        [TestCase(""), TestCase(" ")]
        public static void Constructor_SubtitlesInvalid_ThrowsArgumentException(string subtitles) => That(() => new Tags("Cast", "Directors", "Genres", "Years", "Languages", subtitles, default!,
                default!),
            Throws.ArgumentException.With.Matches<ArgumentException>(exception => exception.ParamName == "subtitles"));

        /// <summary><see cref="Tags(string, string, string, string, string, string, string, string)" /> test.</summary>
        /// <param name="title">Tag used to name movie title.</param>
        [TestCase(""), TestCase(" ")]
        public static void Constructor_TitleInvalid_ThrowsArgumentException(string title) => That(() => new Tags("Cast", "Directors", "Genres", "Years", "Languages", "Subtitles", title, default!),
            Throws.ArgumentException.With.Matches<ArgumentException>(exception => exception.ParamName == "title"));

        /// <summary><see cref="Tags(string, string, string, string, string, string, string, string)" /> test.</summary>
        /// <param name="writers">Tag used to name movie writers.</param>
        [TestCase(""), TestCase(" ")]
        public static void Constructor_WritersInvalid_ThrowsArgumentException(string writers) =>
            That(() => new Tags("Cast", "Directors", "Genres", "Years", "Languages", "Subtitles", "Title", writers),
                Throws.ArgumentException.With.Matches<ArgumentException>(exception => exception.ParamName == "writers"));
#endregion

#region Cast tests
        /// <summary><see cref="Tags.Cast" /> test.</summary>
        [Test]
        public static void Cast_ValidPassed_ValidReturned() => That(Setup().Cast, Is.SameAs("Cast"));
#endregion

#region Directors tests
        /// <summary><see cref="Tags.Directors" /> test.</summary>
        [Test]
        public static void Directors_ValidPassed_ValidReturned() => That(Setup().Directors, Is.SameAs("Directors"));
#endregion

#region Genres tests
        /// <summary><see cref="Tags.Genres" /> test.</summary>
        [Test]
        public static void Genres_ValidPassed_ValidReturned() => That(Setup().Genres, Is.SameAs("Genres"));
#endregion

#region Years tests
        /// <summary><see cref="Tags.Years" /> test.</summary>
        [Test]
        public static void Years_ValidPassed_ValidReturned() => That(Setup().Years, Is.SameAs("Years"));
#endregion

#region Languages tests
        /// <summary><see cref="Tags.Languages" /> test.</summary>
        [Test]
        public static void Languages_ValidPassed_ValidReturned() => That(Setup().Languages, Is.SameAs("Languages"));
#endregion

#region Subtitles tests
        /// <summary><see cref="Tags.Subtitles" /> test.</summary>
        [Test]
        public static void Subtitles_ValidPassed_ValidReturned() => That(Setup().Subtitles, Is.SameAs("Subtitles"));
#endregion

#region Title tests
        /// <summary><see cref="Tags.Title" /> test.</summary>
        [Test]
        public static void Title_ValidPassed_ValidReturned() => That(Setup().Title, Is.SameAs("Title"));
#endregion

#region Writers tests
        /// <summary><see cref="Tags.Writers" /> test.</summary>
        [Test]
        public static void Writers_ValidPassed_ValidReturned() => That(Setup().Writers, Is.SameAs("Writers"));
#endregion

        /// <summary>Single test setup.</summary>
        /// <returns><see cref="object" /> to test.</returns>
        static Tags Setup() => new Tags("Cast", "Directors", "Genres", "Years", "Languages", "Subtitles", "Title", "Writers");
    }
}