using Microsoft.Win32;
using NUnit.Framework;
using System.Diagnostics.CodeAnalysis;
using static Microsoft.Win32.Registry;
using static NUnit.Framework.Assert;
using static NUnit.Framework.Is;

namespace ImdbParser.Tests.Wrappers {
    /// <summary><see cref="RegistryWrapper" /> tests.</summary>
    [ExcludeFromCodeCoverage]
    static class RegistryWrapperTests {
#region GetValue tests
        /// <summary><see cref="RegistryWrapper.GetValue" /> test.</summary>
        [Test]
        public static void GetValue_Called_SameValuesReturned() => That(new RegistryWrapper().GetValue(LocalMachine.Name, string.Empty, default), SameAs(LocalMachine.GetValue(string.Empty)));
#endregion
    }
}