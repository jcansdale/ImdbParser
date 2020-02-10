using ImdbParser.Commands;
using ImdbParser.Executors;
using ImdbParser.Options;
using IWshRuntimeLibrary;
using Moq;
using NUnit.Framework;
using System;
using System.Diagnostics.CodeAnalysis;
using System.Linq;
using System.Threading.Tasks;
using static ImdbParser.Commands.CommandId;
using static ImdbParser.Language;
using static ImdbParser.Messages;
using static ImdbParser.Start;
using static ImdbParser.Tests.TestsBase;
using static Moq.Times;
using static NUnit.Framework.Assert;
using static NUnit.Framework.Is;
using static NUnit.Framework.Throws;
using static System.Array;
using static System.FormattableString;
using static System.Result;

namespace ImdbParser.Tests {
    /// <summary><see cref="Start" /> tests.</summary>
    [ExcludeFromCodeCoverage]
    static class StartTests {
#region Constructor tests
        /// <summary><see cref="Start(ICommandExecutor, IConsole)" /> test.</summary>
        [Test]
        public static void Constructor_ValidArgumentsPassed_NoExceptionThrown() {
            var setup = Setup();

            That(new Start(setup.executor.Object, setup.wrapper.Object), Not.Null);
        }
#endregion

#region Main tests
        /// <summary><see cref="Main" /> test.</summary>
        [Test]
        public static void Main_ValidArgumentPassed_NoExceptionThrown() => That(async () => await Main(Empty<string>()).ConfigureAwait(default), Nothing);
#endregion

#region MainWithConfiguration tests
        /// <summary><see cref="MainWithConfiguration" /> test.</summary>
        /// <returns><see cref="Task" /> to execute.</returns>
        [Test]
        public static async Task MainWithConfiguration_BuilderPassed_BuilderCalled() {
            var called = false;

            await MainWithConfiguration(Empty<string>(), _ => called = true).ConfigureAwait(default);

            That(called, Is.True);
        }

        /// <summary><see cref="MainWithConfiguration" /> test.</summary>
        /// <returns><see cref="Task" /> to execute.</returns>
        [Test]
        public static async Task MainWithConfiguration_Called_WshShellInstanceRegistered() {
            var called = false;

            await MainWithConfiguration(Empty<string>(), collection => {
                That(collection.First(descriptor => descriptor.ServiceType.FullName == typeof(IWshShell3).FullName).ImplementationFactory(default), Not.Null);
                called = true;
            }).ConfigureAwait(default);

            That(called, Is.True);
        }
#endregion

#region ExecuteAsync tests
        /// <summary><see cref="Start.ExecuteAsync" /> test.</summary>
        /// <returns><see cref="Task" /> to execute.</returns>
        [Test]
        public static async Task ExecuteAsync_FailedToParseArguments_FailureWritten() {
            var setup = Setup();

            await setup.startup.ExecuteAsync(Empty<string>()).ConfigureAwait(default);

            setup.wrapper.Verify(self => self.WriteLine(No_command), Once);
        }

        /// <summary><see cref="Start.ExecuteAsync" /> test.</summary>
        /// <returns><see cref="Task" /> to execute.</returns>
        [Test]
        public static async Task ExecuteAsync_FailedToExecuteCommand_FailureWritten() {
            var setup = Setup();
            const string Failure = "Failure";
            setup.executor.SetupExecutorExecuteCommandAsync(Fail<string>(Failure));

            await setup.startup.ExecuteAsync(GetValidGatherArguments()).ConfigureAwait(default);

            setup.wrapper.Verify(self => self.WriteLine(Failure), Once);
        }

        /// <summary><see cref="Start.ExecuteAsync" /> test.</summary>
        /// <returns><see cref="Task" /> to execute.</returns>
        [Test]
        public static async Task ExecuteAsync_ExecuteCommandSucceeded_ResultWritten() {
            var setup = Setup();
            const string Result = "Result";
            setup.executor.SetupExecutorExecuteCommandAsync(Ok(Result));

            await setup.startup.ExecuteAsync(GetValidGatherArguments()).ConfigureAwait(default);

            setup.wrapper.Verify(self => self.WriteLine(Result), Once);
        }
#endregion

        /// <summary>Gets valid arguments for <see cref="Gather" /> command.</summary>
        /// <returns>Valid arguments for <see cref="Gather" /> command.</returns>
        static string[] GetValidGatherArguments() => new[] { Gather.MakeCommand(), ImdbTestUrl, OptionId.Language.MakeCommand(), Lt.ToString() };

        /// <summary>Creates command from <paramref name="command" />.</summary>
        /// <typeparam name="T"><see cref="Type" /> of command identifier.</typeparam>
        /// <param name="command">Command identifier.</param>
        /// <returns><see cref="string" /> representation of a command.</returns>
        static string MakeCommand<T>(this T command) where T : struct => Invariant($"-{command}");

        /// <summary>Single test setup.</summary>
        /// <returns><see cref="object" /> to test with its <see cref="Mock{T}" />s.</returns>
        static (Start startup, Mock<ICommandExecutor> executor, Mock<IConsole> wrapper) Setup() {
            var executor = new Mock<ICommandExecutor>();
            var wrapper = new Mock<IConsole>();
            return (new Start(executor.Object, wrapper.Object), executor, wrapper);
        }

        /// <summary>Sets up <paramref name="executor" />'s <see cref="ICommandExecutor.ExecuteCommandAsync" /> to return <paramref name="result" />.</summary>
        /// <param name="executor"><see cref="ICommandExecutor" /> <see cref="Mock" /> to set up.</param>
        /// <param name="result">Result to return from <paramref name="executor" />'s <see cref="ICommandExecutor.ExecuteCommandAsync" />.</param>
        static void SetupExecutorExecuteCommandAsync(this Mock<ICommandExecutor> executor, IResult<string> result) =>
            executor.Setup(self => self.ExecuteCommandAsync(It.IsAny<ICommand>())).ReturnsAsync(result);
    }
}