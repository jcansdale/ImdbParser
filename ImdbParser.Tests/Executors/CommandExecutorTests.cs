using ImdbParser.Commands;
using ImdbParser.Executors;
using Microsoft.Extensions.DependencyInjection;
using Moq;
using NUnit.Framework;
using System;
using System.Diagnostics.CodeAnalysis;
using System.Threading.Tasks;
using static NUnit.Framework.Assert;
using static NUnit.Framework.Is;
using static System.Result;
using static System.Threading.Tasks.Task;
using Throws = NUnit.Framework.Throws;

namespace ImdbParser.Tests.Executors {
    /// <summary><see cref="CommandExecutor" /> tests.</summary>
    [ExcludeFromCodeCoverage]
    static class CommandExecutorTests {
        /// <summary><see cref="ICommand" /> implementation for testing.</summary>
        sealed class Command : ICommand {}

        /// <summary><see cref="IExecutor{TCommand}" /> implementation for testing.</summary>
        // ReSharper disable once StringLiteralTypo
        [SuppressMessage("Performance", "CA1812:Avoid uninstantiated internal classes", Justification = "Type used through reflection.")]
        sealed class Executor : IExecutor<Command> {
            /// <summary>Executes <paramref name="command" />.</summary>
            /// <param name="command">Command to execute.</param>
            /// <returns>Text to print after operation.</returns>
            public Task<IResult<string>> ExecuteAsync(Command command) => FromResult<IResult<string>>(Ok("Result"));
        }

#region Constructor tests
        /// <summary><see cref="CommandExecutor(IServiceProvider)" /> test.</summary>
        [Test]
        public static void Constructor_ValidArgumentPassed_NoExceptionThrown() => That(new CommandExecutor(Setup().provider), Not.Null);
#endregion

#region ExecuteCommandAsync tests
        /// <summary><see cref="CommandExecutor.ExecuteCommandAsync" /> test.</summary>
        [Test]
        public static void ExecuteCommandAsync_UnknownCommandPassed_ThrowsNotImplementedException() {
            var executor = Setup().executor;

            That(async () => await executor.ExecuteCommandAsync(new Mock<ICommand>().Object).ConfigureAwait(default), Throws.TypeOf<NotImplementedException>());
        }

        /// <summary><see cref="CommandExecutor.ExecuteCommandAsync" /> test.</summary>
        [Test]
        public static async Task ExecuteCommandAsync_ValidCommandPassed_ExecutorCalled() =>
            (await Setup().executor.ExecuteCommandAsync(new Command()).ConfigureAwait(default)).ExpectSuccess(result => That(result, SameAs("Result")));
#endregion

        /// <summary>Single test setup.</summary>
        /// <returns><see cref="object" /> to test.</returns>
        static (CommandExecutor executor, ServiceProvider provider) Setup() {
            var provider = new ServiceCollection().AddSingleton<Executor>().BuildServiceProvider();
            return (new CommandExecutor(provider, typeof(CommandExecutorTests).Assembly), provider);
        }
    }
}