using NUnit.Framework;
using System.Diagnostics;
using System.Diagnostics.CodeAnalysis;
using static ImdbParser.Tests.TestsBase;
using static NUnit.Framework.Assert;
using static NUnit.Framework.Is;

namespace ImdbParser.Tests.Wrappers {
    /// <summary><see cref="ProcessStaticWrapper" /> tests.</summary>
    [ExcludeFromCodeCoverage]
    static class ProcessStaticWrapperTests {
#region Start tests
        /// <summary><see cref="ProcessStaticWrapper.Start" /> test.</summary>
        [Test]
        public static void Start_Called_ProcessCreated() {
            using var process = new ProcessStaticWrapper().Start(new ProcessStartInfo(Path));

            That(process, Not.Null);
        }
#endregion
    }
}