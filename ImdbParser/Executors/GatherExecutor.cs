using ImdbParser.Commands;
using ImdbParser.Html;
using ImdbParser.Settings;
using System;
using System.Collections.Immutable;
using System.Threading.Tasks;
using static System.Linq.Enumerable;
using static System.Result;

namespace ImdbParser.Executors {
    /// <summary>Executes <see cref="GatherCommand" />.</summary>
    sealed class GatherExecutor : IExecutor<GatherCommand> {
        /// <summary>File names builder.</summary>
        readonly IFileNameBuilder _builder;
        /// <summary><see cref="Movie" /> tags creator.</summary>
        readonly ITagsCreator _creator;
        /// <summary>IMDB pages parser.</summary>
        readonly IHtmlManager _manager;
        /// <summary>Application settings.</summary>
        readonly ISettings _settings;

        /// <summary>Initialize new <see cref="GatherExecutor" /> instance.</summary>
        /// <param name="manager">IMDB pages parser.</param>
        /// <param name="creator"><see cref="Movie" /> tags creator.</param>
        /// <param name="builder">File names builder.</param>
        /// <param name="settings">Application settings.</param>
        /// <remarks>Must be public for dependency injection.</remarks>
        public GatherExecutor(IHtmlManager manager, ITagsCreator creator, IFileNameBuilder builder, ISettings settings) {
            _manager = manager;
            _creator = creator;
            _builder = builder;
            _settings = settings;
        }

        /// <summary>Executes <paramref name="command" />.</summary>
        /// <param name="command">Command to execute.</param>
        /// <returns>Text to print after operation.</returns>
        public async Task<IResult<string>> ExecuteAsync(GatherCommand command) {
            using var result = await _manager.ParseMovieAsync(command.Url).ConfigureAwait(default);
            if (result.IsFailure) return Fail<string>(result);
            var languages = command.Language.Languages.ToImmutableList();
            var subtitles = command.Subtitles?.Languages.ToImmutableList() ?? Empty<Language>().ToImmutableList();
            result.Value.ParallelDo(movie => movie.Cover().Save(_settings.CoverName.FullName), movie => _creator.CreateTagsFile(movie, languages, subtitles).Save(_settings.TagsName.FullName));
            return Ok(_builder.BuildFileName(result.Value, languages, subtitles).FullName);
        }
    }
}