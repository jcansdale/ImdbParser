using System.Diagnostics;
using System.IO;

// ReSharper disable once CheckNamespace
namespace System {
    /// <summary>Represents the standard input, output, and error streams for console applications.</summary>
    interface IConsole {
        /// <summary>Writes the specified string value, followed by the current line terminator, to the standard output stream.</summary>
        /// <param name="value">The value to write.</param>
        /// <exception cref="IOException">An I/O error occurred.</exception>
        void WriteLine(string value);
    }

    /// <summary>Represents the standard input, output, and error streams for console applications.</summary>
    /// <remarks><see cref="Console" /> wrapper.</remarks>
    [DebuggerStepThrough]
    sealed class ConsoleWrapper : IConsole {
        /// <summary>Writes the specified string value, followed by the current line terminator, to the standard output stream.</summary>
        /// <param name="value">The value to write.</param>
        /// <exception cref="IOException">An I/O error occurred.</exception>
        public void WriteLine(string value) => Console.WriteLine(value);
    }
}