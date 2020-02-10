using System;

namespace ImdbParser.Html {
    /// <summary>Creates IMDB pages parsers.</summary>
    interface IInfoParserFactory {
        /// <summary>Creates cast and crew information parser.</summary>
        /// <param name="html">HTML to parse.</param>
        /// <returns>Cast and crew information parser.</returns>
        ICrewAndCastParser CreateCrewAndCastParser(string html);

        /// <summary>Creates movie information parser.</summary>
        /// <param name="html">HTML to parse.</param>
        /// <returns>Movie information parser.</returns>
        IMovieInfoParser CreateMovieInfoParser(string html);
    }

    /// <summary>Creates IMDB pages parsers.</summary>
    sealed class InfoParserFactory : IInfoParserFactory {
        /// <summary>Creates cast and crew information parser.</summary>
        /// <param name="html">HTML to parse.</param>
        /// <returns>Cast and crew information parser.</returns>
        /// <exception cref="ArgumentException"><paramref name="html" /> is empty <see cref="string" /> or contains only white spaces.</exception>
        public ICrewAndCastParser CreateCrewAndCastParser(string html) => new CrewAndCastParser(html);

        /// <summary>Creates movie information parser.</summary>
        /// <param name="html">HTML to parse.</param>
        /// <returns>Movie information parser.</returns>
        /// <exception cref="ArgumentException"><paramref name="html" /> is empty <see cref="string" /> or contains only white spaces.</exception>
        public IMovieInfoParser CreateMovieInfoParser(string html) => new MovieInfoParser(html);
    }
}