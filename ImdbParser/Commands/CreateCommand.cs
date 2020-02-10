using ImdbParser.Options;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Diagnostics.CodeAnalysis;
using System.Linq;
using static ImdbParser.Messages;
using static System.Diagnostics.DebuggerBrowsableState;
using static System.Result;

namespace ImdbParser.Commands {
    /// <summary>Create command. Collects and creates information about a movie from IMDB.</summary>
    sealed class CreateCommand : GatherCommand {
        /// <summary>Arguments that create command accepts count.</summary>
        [DebuggerBrowsable(Never)] const int ArgumentsCount = 1;

        /// <summary>Initializes new <see cref="CreateCommand" /> instance.</summary>
        /// <param name="url">IMDB URL address from which movie information should be collected.</param>
        /// <param name="language">Languages <see cref="IOption" /> that identifies what languages should be added while performing create command.</param>
        /// <param name="subtitles">Subtitles <see cref="IOption" /> that identifies what subtitles languages should be added while performing create command.</param>
        /// <param name="link">Link <see cref="IOption" /> that provide link to the movie.</param>
        CreateCommand(ImdbFilmUrl url, LanguageOption language, SubtitlesOption? subtitles, LinkOption link) : base(url, language, subtitles) => Link = link;

        /// <summary>Link <see cref="IOption" /> that provide link to the movie.</summary>
        /// <value>Gets link <see cref="IOption" /> that provide link to the movie.</value>
        [SuppressMessage("Microsoft.Performance", "CA1822:Mark members as static", Justification = "False positive.")]
        internal LinkOption Link { [DebuggerStepThrough] get; }

        /// <summary>Tries to create <see cref="CreateCommand" /> instance. If any of the parameters passed are invalid then no result is returned.</summary>
        /// <param name="parameters">Direct command parameters.</param>
        /// <param name="options">Options provided for <see cref="CreateCommand" />.</param>
        /// <returns><see cref="CreateCommand" /> instance if parameters are valid.</returns>
        internal static new Result<CreateCommand> Create(IReadOnlyList<string> parameters, IReadOnlyCollection<IOption> options) {
            if (parameters.Count != ArgumentsCount) return Fail<CreateCommand>(Invalid_command_parameters_count.CurrentCultureFormat(CommandId.Create, ArgumentsCount));
            var url = ImdbFilmUrl.CreateUrl(parameters[0]);
            if (url.IsFailure) return Fail<CreateCommand>(url);
            if (!options.AreUnique()) return Fail<CreateCommand>(All_options_must_be_unique);
            var language = options.OfType<LanguageOption>().FirstOrDefault();
            var link = options.OfType<LinkOption>().FirstOrDefault();
            return language == default && link == default ? Fail<CreateCommand>(Command_must_have_option.CurrentCultureFormat(CommandId.Create, ", ".Join(OptionId.Language, OptionId.Link))) :
                language == default ? Fail<CreateCommand>(Command_must_have_option.CurrentCultureFormat(CommandId.Create, OptionId.Language)) : link == default ?
                    Fail<CreateCommand>(Command_must_have_option.CurrentCultureFormat(CommandId.Create, OptionId.Link)) :
                    Ok(new CreateCommand(url.Value, language, options.OfType<SubtitlesOption>().FirstOrDefault(), link));
        }
    }
}