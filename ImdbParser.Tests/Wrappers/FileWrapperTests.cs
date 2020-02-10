using NUnit.Framework;
using System.Diagnostics.CodeAnalysis;
using System.IO;
using static NUnit.Framework.Assert;

namespace ImdbParser.Tests.Wrappers {
    /// <summary><see cref="FileWrapper" /> tests.</summary>
    [ExcludeFromCodeCoverage]
    static class FileWrapperTests {
#region Exists tests
        /// <summary><see cref="FileWrapper.Exists" /> test.</summary>
        [Test]
        public static void Exists_CalledWithNull_FalseReturned() => That(() => new FileWrapper().Exists(default!), Is.False);
#endregion
    }
}