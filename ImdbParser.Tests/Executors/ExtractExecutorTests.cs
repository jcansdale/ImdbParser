using ImdbParser.Executors;
using Moq;
using NUnit.Framework;
using System;
using System.Diagnostics.CodeAnalysis;
using System.IO;
using System.Threading.Tasks;
using static ImdbParser.Tests.TestsBase;
using static NUnit.Framework.Assert;
using static NUnit.Framework.Is;
using static System.Result;

namespace ImdbParser.Tests.Executors {
    /// <summary><see cref="ExtractExecutor" /> tests.</summary>
    [ExcludeFromCodeCoverage]
    static class ExtractExecutorTests {
#region Constructor tests
        /// <summary><see cref="ExtractExecutor(IMkvToolExecutor)" /> test.</summary>
        [Test]
        public static void Constructor_ValidArgumentPassed_NoExceptionThrown() => That(new ExtractExecutor(Setup().toolExecutor.Object), Not.Null);
#endregion

#region ExecuteAsync tests
        /// <summary><see cref="ExtractExecutor.ExecuteAsync" /> test.</summary>
        /// <returns><see cref="Task" /> to execute.</returns>
        [Test]
        public static async Task ExecuteAsync_FailedToExtractTags_FailureReturned() {
            var setup = Setup();
            var command = CreateExtract();
            const string Failure = "Failure";
            setup.toolExecutor.SetupToolExecutorExtractTags(command.MoviePath, Result.Fail(Failure));

            (await setup.executor.ExecuteAsync(command).ConfigureAwait(default)).ExpectFail(() => Failure);
        }

        /// <summary><see cref="ExtractExecutor.ExecuteAsync" /> test.</summary>
        /// <returns><see cref="Task" /> to execute.</returns>
        [Test]
        public static async Task ExecuteAsync_TagsExtracted_SuccessReturned() {
            var setup = Setup();
            var command = CreateExtract();
            setup.toolExecutor.SetupToolExecutorExtractTags(command.MoviePath, Ok());

            (await setup.executor.ExecuteAsync(command).ConfigureAwait(default)).ExpectSuccess();
        }
#endregion

        /// <summary>Single test setup.</summary>
        /// <returns><see cref="object" /> to test with its <see cref="Mock" />.</returns>
        static (ExtractExecutor executor, Mock<IMkvToolExecutor> toolExecutor) Setup() {
            var executor = new Mock<IMkvToolExecutor>();
            return (new ExtractExecutor(executor.Object), executor);
        }

        /// <summary>Sets up <paramref name="toolExecutor" />'s <see cref="IMkvToolExecutor.ExtractTags" /> to return <paramref name="result" /> when called with <paramref name="file" />.</summary>
        /// <param name="toolExecutor"><see cref="IMkvToolExecutor" /> <see cref="Mock" /> to set up.</param>
        /// <param name="file">File to expect.</param>
        /// <param name="result">Result to return.</param>
        static void SetupToolExecutorExtractTags(this Mock<IMkvToolExecutor> toolExecutor, FileInfo file, IResult result) => toolExecutor.Setup(self => self.ExtractTags(file)).Returns(result);
    }
}