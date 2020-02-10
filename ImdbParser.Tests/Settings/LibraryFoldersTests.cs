using ImdbParser.Settings;
using NUnit.Framework;
using System;
using System.Diagnostics.CodeAnalysis;
using static NUnit.Framework.Assert;
using Throws = NUnit.Framework.Throws;

namespace ImdbParser.Tests.Settings {
    /// <summary><see cref="LibraryFolders" /> tests.</summary>
    [ExcludeFromCodeCoverage]
    static class LibraryFoldersTests {
#region Constructor tests
        /// <summary><see cref="LibraryFolders()" /> test.</summary>
        [Test]
        public static void Constructor_Called_AllValuesLeftAsNull() {
            var folders = new LibraryFolders();

            Multiple(() => {
                That(folders.Cast, Is.Null);
                That(folders.Directors, Is.Null);
                That(folders.Genres, Is.Null);
                That(folders.Years, Is.Null);
            });
        }

        /// <summary><see cref="LibraryFolders(string, string, string, string)" /> test.</summary>
        /// <param name="cast">Folder where movie actor's shortcuts should be placed.</param>
        [TestCase(""), TestCase(" ")]
        public static void Constructor_CastInvalid_ThrowsArgumentException(string cast) => That(() => new LibraryFolders(cast, default!, default!, default!),
            Throws.ArgumentException.With.Matches<ArgumentException>(exception => exception.ParamName == "cast"));

        /// <summary><see cref="LibraryFolders(string, string, string, string)" /> test.</summary>
        /// <param name="directors">Folder where movie director's shortcuts should be placed.</param>
        [TestCase(""), TestCase(" ")]
        public static void Constructor_DirectorsInvalid_ThrowsArgumentException(string directors) => That(() => new LibraryFolders("Cast", directors, default!, default!),
            Throws.ArgumentException.With.Matches<ArgumentException>(exception => exception.ParamName == "directors"));

        /// <summary><see cref="LibraryFolders(string, string, string, string)" /> test.</summary>
        /// <param name="genres">Folder where movie genre's shortcuts should be placed.</param>
        [TestCase(""), TestCase(" ")]
        public static void Constructor_GenresInvalid_ThrowsArgumentException(string genres) => That(() => new LibraryFolders("Cast", "Directors", genres, default!),
            Throws.ArgumentException.With.Matches<ArgumentException>(exception => exception.ParamName == "genres"));

        /// <summary><see cref="LibraryFolders(string, string, string, string)" /> test.</summary>
        /// <param name="years">Folder where movie year's shortcut should be placed.</param>
        [TestCase(""), TestCase(" ")]
        public static void Constructor_YearsInvalid_ThrowsArgumentException(string years) => That(() => new LibraryFolders("Cast", "Directors", "Genres", years),
            Throws.ArgumentException.With.Matches<ArgumentException>(exception => exception.ParamName == "years"));
#endregion

#region Cast tests
        /// <summary><see cref="LibraryFolders.Cast" /> test.</summary>
        [Test]
        public static void Cast_ValidPassed_ValidReturned() => That(Setup().Cast, Is.SameAs("Cast"));
#endregion

#region Directors tests
        /// <summary><see cref="LibraryFolders.Directors" /> test.</summary>
        [Test]
        public static void Directors_ValidPassed_ValidReturned() => That(Setup().Directors, Is.SameAs("Directors"));
#endregion

#region Genres tests
        /// <summary><see cref="LibraryFolders.Genres" /> test.</summary>
        [Test]
        public static void Genres_ValidPassed_ValidReturned() => That(Setup().Genres, Is.SameAs("Genres"));
#endregion

#region Years tests
        /// <summary><see cref="LibraryFolders.Years" /> test.</summary>
        [Test]
        public static void Years_ValidPassed_ValidReturned() => That(Setup().Years, Is.SameAs("Years"));
#endregion

        /// <summary>Single test setup.</summary>
        /// <returns><see cref="object" /> to test.</returns>
        static LibraryFolders Setup() => new LibraryFolders("Cast", "Directors", "Genres", "Years");
    }
}