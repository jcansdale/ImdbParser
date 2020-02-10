using System;
using System.Collections.Generic;
using System.Linq;
using static System.String;

namespace ImdbParser.Html {
    /// <summary>Base HTML parser.</summary>
    abstract class HtmlParser {
        /// <summary>Skips characters plus one until first <paramref name="symbolToSkip" /> found.</summary>
        /// <param name="characters">Collection to search <paramref name="symbolToSkip" /> for.</param>
        /// <param name="symbolToSkip">Character until which to skip in <paramref name="characters" />.</param>
        /// <returns>Collection with all elements until first <paramref name="symbolToSkip" /> and bracket skipped.</returns>
        private protected static IEnumerable<char> SkipFirstCharacterFound(IEnumerable<char> characters, char symbolToSkip = '>') => characters.SkipWhile(@char => @char != symbolToSkip).Skip(1);

        /// <summary>Skips characters plus one until first <paramref name="symbolToSkip" /> found and then takes characters while <paramref name="symbolToTake" /> is found, and then joins them into
        /// <see cref="string" />.</summary>
        /// <param name="characters">Collection to manage.</param>
        /// <param name="symbolToSkip">Character until which to skip in <paramref name="characters" />.</param>
        /// <param name="symbolToTake">Character until which to take from <paramref name="characters" /> after <paramref name="symbolToSkip" />.</param>
        /// <returns><see cref="string" /> between <paramref name="symbolToSkip" /> and <paramref name="symbolToTake" />.</returns>
        private protected static string SkipFirstCharacterTakeWhileAndJoin(IEnumerable<char> characters, char symbolToSkip = '>', char symbolToTake = '<') =>
            SkipFirstCharacterFound(characters, symbolToSkip).Map(collection => TakeWhileAndJoin(collection, symbolToTake)).Trim();

        /// <summary>Takes until first <paramref name="symbolToTake" /> found.</summary>
        /// <param name="characters">Collection to search <paramref name="symbolToTake" /> for.</param>
        /// <param name="symbolToTake">Character until which to take from <paramref name="characters" />.</param>
        /// <returns><see cref="string" /> until <paramref name="symbolToTake" /> was found.</returns>
        private protected static string TakeWhileAndJoin(IEnumerable<char> characters, char symbolToTake = '<') => characters.TakeWhile(character => character != symbolToTake).Map(Concat);
    }
}