using System;
using System.Diagnostics.CodeAnalysis;
using static System.String;

namespace ImdbParser.IntegrationTests {
    /// <summary>Test <see cref="IConsole" /> implementation.</summary>
    [ExcludeFromCodeCoverage]
    sealed class TestConsole : IConsole {
        /// <summary>Text passed to <see cref="WriteLine" />.</summary>
        /// <value>Gets text passed to <see cref="WriteLine" />.</value>
        internal string Output { get; private set; } = Empty;

        /// <summary>Writes the specified string value, followed by the current line terminator, to the standard output stream.</summary>
        /// <param name="value">The value to write.</param>
        public void WriteLine(string value) => Output += value;
    }
}