using ImdbParser.Commands;
using ImdbParser.Html;
using System;
using System.Collections.Immutable;
using System.Threading.Tasks;
using static System.Array;

namespace ImdbParser.Executors {
    /// <summary>Executes <see cref="CreateCommand" />.</summary>
    sealed class CreateExecutor : IExecutor<CreateCommand> {
        /// <summary>Shortcuts creator.</summary>
        readonly IShortcutCreator _creator;
        /// <summary>IMDB pages parser.</summary>
        readonly IHtmlManager _manager;

        /// <summary>Initialize new <see cref="CreateExecutor" /> instance.</summary>
        /// <param name="manager">IMDB pages parser.</param>
        /// <param name="creator">Shortcuts creator.</param>
        /// <remarks>Must be public for dependency injection.</remarks>
        public CreateExecutor(IHtmlManager manager, IShortcutCreator creator) {
            _manager = manager;
            _creator = creator;
        }

        /// <summary>Executes <paramref name="command" />.</summary>
        /// <param name="command">Command to execute.</param>
        /// <returns>Text to print after operation.</returns>
        public async Task<IResult<string>> ExecuteAsync(CreateCommand command) {
            using var movie = await _manager.ParseMovieAsync(command.Url).ConfigureAwait(default);
            return movie.OnSuccess(value => _creator.CreateShortcuts(value, command.Language.Languages, command.Subtitles?.Languages.ToImmutableList() ?? Empty<Language>().ToImmutableList())).
                OnSuccess(value => string.Empty);
        }
    }
}