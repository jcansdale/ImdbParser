using System;
using System.Collections.Generic;
using System.Collections.Immutable;
using System.Collections.ObjectModel;
using System.Diagnostics;
using System.Linq;
using ImdbParser.Commands;
using ImdbParser.Options;
using static ImdbParser.Messages;
using static System.Diagnostics.DebuggerBrowsableState;
using static System.Result;
using static System.StringComparison;

namespace ImdbParser {
    /// <summary>Parses command line arguments into <see cref="ICommand" />.</summary>
    static class ArgumentsParser {
        /// <summary>Commands start symbol.</summary>
        [DebuggerBrowsable(Never)] const string CommandStartSymbol = "-";

        /// <summary>Parses command line arguments into <see cref="ICommand" />.</summary>
        /// <param name="arguments">Command line arguments from which <see cref="ICommand" /> is parsed.</param>
        /// <returns><see cref="ICommand" /> parsed from command line arguments.</returns>
        internal static IResult<ICommand> ParseArguments(this IReadOnlyCollection<string> arguments) => arguments.ElementAtOrDefault(0).MapIf(IsNotCommandText, _ => Fail<ICommand>(No_command),
            argument => argument.ParseEnumWithStartSymbol<CommandId>(() => Unknown_command).OnSuccess(command => command.CreateCommand(arguments.Skip(1).ToImmutableList())));

        /// <summary>Creates <see cref="ICommand" /> based on <paramref name="commandId" /> and <paramref name="parametersAndOptions" /> passed to it.</summary>
        /// <param name="commandId">Identifier based on which <see cref="ICommand" /> should be created.</param>
        /// <param name="parametersAndOptions">Parameters and options passed to <see cref="ICommand" />.</param>
        /// <returns><see cref="ICommand" /> created.</returns>
        static IResult<ICommand> CreateCommand(this CommandId commandId, IReadOnlyCollection<string> parametersAndOptions) {
            var parameters = parametersAndOptions.TakeWhile(IsNotCommandText).ToImmutableList();
            var options = new Collection<IOption>();
            foreach (var option in parametersAndOptions.Skip(parameters.Count).ToImmutableList().CreateOption()) {
                if (option.IsFailure) return Fail<ICommand>(option.Failure);
                options.Add(option.Value);
            }
            return commandId.CreateCommand(parameters, options);
        }

        /// <summary>Creates <see cref="IOption" />s based on <paramref name="optionsWithParameters" />.</summary>
        /// <param name="optionsWithParameters"><see cref="IOption" />s and parameters passed to them.</param>
        /// <returns><see cref="IOption" />s created.</returns>
        static IEnumerable<IResult<IOption>> CreateOption(this IReadOnlyList<string> optionsWithParameters) {
            while (true) {
                if (!optionsWithParameters.Any()) yield break;
                var parameters = optionsWithParameters.Skip(1).TakeWhile(IsNotCommandText).ToImmutableList();
                yield return optionsWithParameters[0].ParseEnumWithStartSymbol<OptionId>(() => Unknown_option).OnSuccess(optionId => optionId.CreateOption(parameters));
                optionsWithParameters = optionsWithParameters.Skip(parameters.Count + 1).ToImmutableList();
            }
        }

        /// <summary>Checks if argument passed does not identify command.</summary>
        /// <param name="argument">Argument to check.</param>
        /// <returns>True if <paramref name="argument" /> does not identify command; false - otherwise.</returns>
        static bool IsNotCommandText(this string argument) => argument?.StartsWith(CommandStartSymbol, InvariantCultureIgnoreCase).Negation() ?? true;

        /// <summary>Parses command identifier from argument with start symbol.</summary>
        /// <typeparam name="T"><see cref="Type" /> to parse.</typeparam>
        /// <param name="argument">Argument with start symbol passed to command prompt.</param>
        /// <param name="failure">Method used to generate failure message.</param>
        /// <returns>Command identifier.</returns>
        static Result<T> ParseEnumWithStartSymbol<T>(this string argument, Func<string> failure) where T : struct =>
            argument.Substring(CommandStartSymbol.Length).ParseEnumerable<T>().MapIf(value => value.Equals(default(T)), _ => Fail<T>(failure()), Ok);
    }
}