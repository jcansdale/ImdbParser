using NUnit.Framework;
using System;
using System.Diagnostics.CodeAnalysis;
using static NUnit.Framework.Assert;
using Throws = NUnit.Framework.Throws;

namespace ImdbParser.Tests.Wrappers {
    /// <summary><see cref="ConsoleWrapper" /> tests.</summary>
    [ExcludeFromCodeCoverage]
    static class ConsoleWrapperTests {
#region WriteLine tests
        /// <summary><see cref="ConsoleWrapper.WriteLine" /> test.</summary>
        [Test]
        public static void WriteLine_Called_NoExceptionThrows() => That(() => new ConsoleWrapper().WriteLine(default!), Throws.Nothing);
#endregion
    }
}