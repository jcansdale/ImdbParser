using ImdbParser.Settings;
using Microsoft.Extensions.Configuration;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using static System.FormattableString;
using static System.IO.Directory;
using static System.IO.Path;

namespace ImdbParser.IntegrationTests {
    /// <summary>Test <see cref="ISettings" /> implementation.</summary>
    [ExcludeFromCodeCoverage]
    sealed class TestSettings : ApplicationSettings {
        /// <summary>Initialize new <see cref="TestSettings" /> instance.</summary>
        internal TestSettings() : base(MockSettings()) {}

        /// <summary>Sets up settings required for test run.</summary>
        /// <returns>Application settings.</returns>
        static IConfigurationRoot MockSettings() {
            var directory = Combine(AssemblySetup.WorkingDirectory.ToString(), GetRandomFileName());
            const string Library = "Library";
            CreateDirectory(Invariant($"{directory}/{Library}"));
            return new ConfigurationBuilder().AddInMemoryCollection(new[] {
                new KeyValuePair<string, string>("CoverName", "Cover.jpg"), new KeyValuePair<string, string>("Folders:Cast", "Cast"),
                new KeyValuePair<string, string>("Folders:Directors", "Directors"), new KeyValuePair<string, string>("Folders:Genres", "Genres"),
                new KeyValuePair<string, string>("Folders:Years", "Years"), new KeyValuePair<string, string>("LibraryDirectory", Library),
                new KeyValuePair<string, string>("MoviesDirectory", directory), new KeyValuePair<string, string>("Tags:Cast", "Actors"),
                new KeyValuePair<string, string>("Tags:Directors", "Directors"), new KeyValuePair<string, string>("Tags:Genres", "Genres"), new KeyValuePair<string, string>("Tags:Years", "Years"),
                new KeyValuePair<string, string>("Tags:Languages", "Languages"), new KeyValuePair<string, string>("Tags:Subtitles", "Subtitles"),
                new KeyValuePair<string, string>("Tags:Title", "Title"), new KeyValuePair<string, string>("Tags:Writers", "Writers"), new KeyValuePair<string, string>("TagsName", "Tags.xml")
            }).Build();
        }
    }
}