using NUnit.Framework;
using System.Diagnostics;
using System.Diagnostics.CodeAnalysis;
using static NUnit.Framework.Assert;
using static NUnit.Framework.Is;
using static NUnit.Framework.Throws;

namespace ImdbParser.Tests.Wrappers {
    /// <summary><see cref="ProcessWrapper" /> tests.</summary>
    [ExcludeFromCodeCoverage]
    static class ProcessWrapperTests {
#region Constructor tests
        /// <summary><see cref="ProcessWrapper(Process)" /> test.</summary>
        [Test]
        public static void Constructor_Called_NoExceptionThrown() {
            using var process = new Process();

            using var wrapper = new ProcessWrapper(process);

            That(wrapper, Not.Null);
        }
#endregion

#region Dispose tests
        /// <summary><see cref="ProcessWrapper.Dispose" /> test.</summary>
        [Test]
        public static void Dispose_Called_NoExceptionThrown() {
            using var process = new Process();
            using var wrapper = new ProcessWrapper(process);

            That(wrapper.Dispose, Nothing);
        }
#endregion
    }
}