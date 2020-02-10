using ImdbParser.Commands;
using System;
using System.Threading.Tasks;

namespace ImdbParser.Executors {
    /// <summary>Executor interface.</summary>
    /// <typeparam name="TCommand"><see cref="Type" /> supported by this executor.</typeparam>
    interface IExecutor<in TCommand> where TCommand : ICommand {
        /// <summary>Executes <paramref name="command" />.</summary>
        /// <param name="command">Command to execute.</param>
        /// <returns>Text to print after operation.</returns>
        Task<IResult<string>> ExecuteAsync(TCommand command);
    }
}