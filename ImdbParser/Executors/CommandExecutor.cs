using ImdbParser.Commands;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Threading.Tasks;

namespace ImdbParser.Executors {
    /// <summary>Performs <see cref="ICommand" /> using available <see cref="IExecutor{TCommand}" />s.</summary>
    interface ICommandExecutor {
        /// <summary>Executes <paramref name="command" />.</summary>
        /// <param name="command">Command to execute.</param>
        /// <returns>Text to print after operation.</returns>
        Task<IResult<string>> ExecuteCommandAsync(ICommand command);
    }

    /// <summary>Performs <see cref="ICommand" /> using available <see cref="IExecutor{TCommand}" />s.</summary>
    sealed class CommandExecutor : ICommandExecutor {
        /// <summary>Dependency injection handler.</summary>
        readonly IServiceProvider _provider;
        /// <summary>Available executors <see cref="Type" />s.</summary>
        readonly IEnumerable<Type> _types;

        /// <summary>Initialize new <see cref="CommandExecutor" /> instance.</summary>
        /// <param name="provider">Dependency injection handler.</param>
        /// <remarks>Must be public for dependency injection.</remarks>
        public CommandExecutor(IServiceProvider provider) : this(provider, typeof(IExecutor<>).Assembly) {}

        /// <summary>Initialize new <see cref="CommandExecutor" /> instance.</summary>
        /// <param name="provider">Dependency injection handler.</param>
        /// <param name="assembly"><see cref="Assembly" /> used to search for types.</param>
        internal CommandExecutor(IServiceProvider provider, Assembly assembly) {
            _provider = provider;
            var executorType = typeof(IExecutor<>);
            _types = assembly.GetTypes().Where(type => type.DoesTypeImplementInterface(executorType));
        }

        /// <summary>Executes <paramref name="command" />.</summary>
        /// <param name="command">Command to execute.</param>
        /// <returns>Text to print after operation.</returns>
        /// <exception cref="NotImplementedException">No <see cref="ICommand" /> execution is provided for <paramref name="command" />.</exception>
        public Task<IResult<string>> ExecuteCommandAsync(ICommand command) => command.GetType().
                Map(commandType => _types.FirstOrDefault(type => type.GetMethods().SelectMany(method => method.GetParameters()).Any(parameter => parameter.ParameterType == commandType)))?.
                Map(executorType => _provider.GetService(executorType)).
                // ReSharper disable once PossibleNullReferenceException
                Map(instance => (Task<IResult<string>>?)instance.GetType().GetMethod(nameof(IExecutor<ICommand>.ExecuteAsync))!.Invoke(instance, new object[] { command })) ??
            throw new NotImplementedException();
    }
}