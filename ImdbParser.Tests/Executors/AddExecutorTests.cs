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
    /// <summary><see cref="AddExecutor" /> tests.</summary>
    [ExcludeFromCodeCoverage]
    static class AddExecutorTests {
#region Constructor tests
        /// <summary><see cref="AddExecutor(IMkvToolExecutor)" /> test.</summary>
        [Test]
        public static void Constructor_ValidArgumentPassed_NoExceptionThrown() => That(new AddExecutor(Setup().toolExecutor.Object), Not.Null);
#endregion

#region ExecuteAsync tests
        /// <summary><see cref="AddExecutor.ExecuteAsync" /> test.</summary>
        /// <returns><see cref="Task" /> to execute.</returns>
        [Test]
        public static async Task ExecuteAsync_FailedToAddTags_FailureReturned() {
            var setup = Setup();
            var command = CreateAdd();
            const string Failure = "Failure";
            setup.toolExecutor.SetupToolExecutorAddTags(command.MoviePath, Result.Fail(Failure));

            (await setup.executor.ExecuteAsync(command).ConfigureAwait(default)).ExpectFail(() => Failure);
        }

        /// <summary><see cref="AddExecutor.ExecuteAsync" /> test.</summary>
        /// <returns><see cref="Task" /> to execute.</returns>
        [Test]
        public static async Task ExecuteAsync_TagsAdded_SuccessReturned() {
            var setup = Setup();
            var command = CreateAdd();
            setup.toolExecutor.SetupToolExecutorAddTags(command.MoviePath, Ok());

            (await setup.executor.ExecuteAsync(command).ConfigureAwait(default)).ExpectSuccess();
        }
#endregion

        /// <summary>Single test setup.</summary>
        /// <returns><see cref="object" /> to test with its <see cref="Mock" />.</returns>
        static (AddExecutor executor, Mock<IMkvToolExecutor> toolExecutor) Setup() {
            var executor = new Mock<IMkvToolExecutor>();
            return (new AddExecutor(executor.Object), executor);
        }

        /// <summary>Sets up <paramref name="toolExecutor" />'s <see cref="IMkvToolExecutor.AddTags" /> to return <paramref name="result" /> when called with <paramref name="file" />.</summary>
        /// <param name="toolExecutor"><see cref="IMkvToolExecutor" /> <see cref="Mock" /> to set up.</param>
        /// <param name="file">File to expect.</param>
        /// <param name="result">Result to return.</param>
        static void SetupToolExecutorAddTags(this Mock<IMkvToolExecutor> toolExecutor, FileInfo file, IResult result) => toolExecutor.Setup(self => self.AddTags(file)).Returns(result);
    }
}