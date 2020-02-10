using ImdbParser.Options;
using Moq;
using NUnit.Framework;
using System;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.IO;
using static ImdbParser.Messages;
using static ImdbParser.Options.LinkOption;
using static NUnit.Framework.Assert;
using static NUnit.Framework.Is;
using static System.Array;

namespace ImdbParser.Tests.Options {
    /// <summary><see cref="LinkOption" /> tests.</summary>
    [ExcludeFromCodeCoverage]
    static class LinkOptionTests {
#region CreateOption tests
        /// <summary><see cref="CreateOption(IReadOnlyCollection{string})" /> test.</summary>
        [Test]
        public static void CreateOption_ContainsNoParameters_FailureReturned() => CreateOption(Empty<string>()).ExpectFail(() => Link_has_no_path);

        /// <summary><see cref="CreateOption(IReadOnlyCollection{string}, IFile)" /> test.</summary>
        [Test]
        public static void CreateOption_FileDoesNotExist_FailureReturned() => CreateOption(new[] { string.Empty }, new Mock<IFile>().Object).ExpectFail(() => File_does_not_exist);

        /// <summary><see cref="CreateOption(IReadOnlyCollection{string}, IFile)" /> test.</summary>
        [Test]
        public static void CreateOption_ValidPathPassed_OptionCreated() {
            const string Path = "Path";
            var file = new Mock<IFile>();
            file.Setup(self => self.Exists(Path)).Returns(true);

            CreateOption(new[] { Path }, file.Object).ExpectSuccess(option => That(option.File.ToString, SameAs(Path)));
        }
#endregion
    }
}