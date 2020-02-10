using ImdbParser.Html;
using NUnit.Framework;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using static NUnit.Framework.Assert;
using static NUnit.Framework.Is;

namespace ImdbParser.Tests.Html {
    /// <summary><see cref="HtmlParser" /> tests.</summary>
    [ExcludeFromCodeCoverage]
    static class HtmlParserTests {
        /// <summary><see cref="HtmlParser" /> wrapper to access protected members.</summary>
        // ReSharper disable once StringLiteralTypo
        [SuppressMessage("Performance", "CA1812:Avoid uninstantiated internal classes", Justification = "Type used through reflection.")]
        //// ReSharper disable once ClassNeverInstantiated.Local
        sealed class HtmlParserWrapper : HtmlParser {
            /// <summary>Skips characters plus one until first <paramref name="character" /> found.</summary>
            /// <param name="characters">Collection to search <paramref name="character" /> for.</param>
            /// <param name="character">Character to search for in <paramref name="characters" />.</param>
            /// <returns>Collection with all elements until first <paramref name="character" /> and bracket skipped.</returns>
            internal static IEnumerable<char> SkipFirstCharacterFoundWrapper(IEnumerable<char> characters, char character = '>') => SkipFirstCharacterFound(characters, character);

            /// <summary>Skips characters plus one until first <paramref name="symbolToSkip" /> found and then takes characters while <paramref name="symbolToTake" /> is found, and then joins them
            /// into <see cref="string" />.</summary>
            /// <param name="characters">Collection to manage.</param>
            /// <param name="symbolToSkip">Character until which to skip in <paramref name="characters" />.</param>
            /// <param name="symbolToTake">Character until which to take from <paramref name="characters" /> after <paramref name="symbolToSkip" />.</param>
            /// <returns><see cref="string" /> between <paramref name="symbolToSkip" /> and <paramref name="symbolToTake" />.</returns>
            internal static string SkipFirstCharacterTakeWhileAndJoinWrapper(IEnumerable<char> characters, char symbolToSkip = '>', char symbolToTake = '<') =>
                SkipFirstCharacterTakeWhileAndJoin(characters, symbolToSkip, symbolToTake);

            /// <summary>Takes until first <paramref name="symbolToTake" /> found.</summary>
            /// <param name="characters">Collection to search <paramref name="symbolToTake" /> for.</param>
            /// <param name="symbolToTake">Character until which to take from <paramref name="characters" />.</param>
            /// <returns><see cref="string" /> until <paramref name="symbolToTake" /> was found.</returns>
            internal static string TakeWhileAndJoinWrapper(IEnumerable<char> characters, char symbolToTake = '<') => TakeWhileAndJoin(characters, symbolToTake);
        }

#region SkipFirstCharacterFound tests
        /// <summary><see cref="HtmlParser.SkipFirstCharacterFound" /> test.</summary>
        [Test]
        public static void SkipFirstCharacterFound_SymbolNotFound_EmptyEnumerableReturned() => That(HtmlParserWrapper.SkipFirstCharacterFoundWrapper(new[] { 'a' }), Empty);

        /// <summary><see cref="HtmlParser.SkipFirstCharacterFound" /> test.</summary>
        [Test]
        public static void SkipFirstCharacterFound_SymbolIsLastSymbol_EmptyEnumerableReturned() => That(HtmlParserWrapper.SkipFirstCharacterFoundWrapper(new[] { 'a', '>' }), Empty);

        /// <summary><see cref="HtmlParser.SkipFirstCharacterFound" /> test.</summary>
        [Test]
        public static void SkipFirstCharacterFound_SymbolIsNotLastSymbol_SymbolsLeftReturned() =>
            That(HtmlParserWrapper.SkipFirstCharacterFoundWrapper(new[] { 'a', '>', 'b', 'c' }), EquivalentTo(new[] { 'b', 'c' }));
#endregion

#region SkipFirstCharacterTakeWhileAndJoin tests
        /// <summary><see cref="HtmlParser.SkipFirstCharacterTakeWhileAndJoin" /> test.</summary>
        [Test]
        public static void SkipFirstCharacterTakeWhileAndJoin_EnumerableNotNull_InnerStringParsed() =>
            That(HtmlParserWrapper.SkipFirstCharacterTakeWhileAndJoinWrapper(new[] { ' ', '>', ' ', 'a', 'b', ' ', '<', 'd', ' ' }), EqualTo("ab"));
#endregion

#region TakeWhileAndJoin tests
        /// <summary><see cref="HtmlParser.TakeWhileAndJoin" /> test.</summary>
        [Test]
        public static void TakeWhileAndJoin_SymbolNotFound_AllStringReturned() => That(HtmlParserWrapper.TakeWhileAndJoinWrapper(new[] { 'a', 'b' }), EqualTo("ab"));

        /// <summary><see cref="HtmlParser.TakeWhileAndJoin" /> test.</summary>
        [Test]
        public static void TakeWhileAndJoin_SymbolFound_StringBeforeSymbolReturned() => That(HtmlParserWrapper.TakeWhileAndJoinWrapper(new[] { 'a', 'b', '<', 'd' }), EqualTo("ab"));

        /// <summary><see cref="HtmlParser.TakeWhileAndJoin" /> test.</summary>
        [Test]
        public static void TakeWhileAndJoin_SymbolIsFirstSymbol_EmptyStringReturned() => That(HtmlParserWrapper.TakeWhileAndJoinWrapper(new[] { '<' }), EqualTo(string.Empty));
#endregion
    }
}