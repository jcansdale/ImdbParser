using ImdbParser.Options;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Diagnostics.CodeAnalysis;
using System.Linq;
using static ImdbParser.Commands.CommandId;
using static ImdbParser.ImdbFilmUrl;
using static ImdbParser.Messages;
using static System.Diagnostics.DebuggerBrowsableState;
using static System.Result;

namespace ImdbParser.Commands {
    /// <summary>Gather command. Collects information about a movie from IMDB.</summary>
    class GatherCommand : ICommand {
        /// <summary>Arguments that gather command accepts count.</summary>
        [DebuggerBrowsable(Never)] const int ArgumentsCount = 1;

        /// <summary>Initializes new <see cref="GatherCommand" /> instance.</summary>
        /// <param name="url">IMDB URL address from which movie information should be collected.</param>
        /// <param name="language">Languages <see cref="IOption" /> that identifies what languages should be added while performing gather command.</param>
        /// <param name="subtitles">Subtitles <see cref="IOption" /> that identifies what subtitles languages should be added while performing gather command.</param>
        private protected GatherCommand(ImdbFilmUrl url, LanguageOption language, SubtitlesOption? subtitles) {
            Url = url;
            Language = language;
            Subtitles = subtitles;
        }

        /// <summary>Languages <see cref="IOption" /> that identifies what languages should be added while performing gather command.</summary>
        /// <value>Gets languages <see cref="IOption" /> that identifies what languages should be added while performing gather command.</value>
        [SuppressMessage("Microsoft.Performance", "CA1822:Mark members as static", Justification = "False positive.")]
        internal LanguageOption Language { [DebuggerStepThrough] get; }

        /// <summary>Subtitles <see cref="IOption" /> that identifies what subtitles languages should be added while performing gather command.</summary>
        /// <value>Gets subtitles <see cref="IOption" /> that identifies what subtitles languages should be added while performing gather command.</value>
        [SuppressMessage("Microsoft.Performance", "CA1822:Mark members as static", Justification = "False positive.")]
        internal SubtitlesOption? Subtitles { [DebuggerStepThrough] get; }

        /// <summary>IMDB URL address from which movie information should be collected.</summary>
        /// <value>Gets IMDB URL address from which movie information should be collected.</value>
        [SuppressMessage("Microsoft.Performance", "CA1822:Mark members as static", Justification = "False positive.")]
        internal ImdbFilmUrl Url { [DebuggerStepThrough] get; }

        /// <summary>Tries to create <see cref="GatherCommand" /> instance. If any of the parameters passed are invalid then no result is returned.</summary>
        /// <param name="parameters">Direct command parameters.</param>
        /// <param name="options">Options provided for <see cref="GatherCommand" />.</param>
        /// <returns><see cref="GatherCommand" /> instance if parameters are valid.</returns>
        internal static Result<GatherCommand> Create(IReadOnlyList<string> parameters, IReadOnlyCollection<IOption> options) {
            if (parameters.Count != ArgumentsCount) return Fail<GatherCommand>(Invalid_command_parameters_count.CurrentCultureFormat(Gather, ArgumentsCount));
            var url = CreateUrl(parameters[0]);
            return url.IsFailure ? Fail<GatherCommand>(url) : !options.AreUnique() ? Fail<GatherCommand>(All_options_must_be_unique) :
                options.OfType<LanguageOption>().FirstOrDefault()?.Map(language => Ok(new GatherCommand(url.Value, language, options.OfType<SubtitlesOption>().FirstOrDefault()))) ??
                Fail<GatherCommand>(Command_must_have_option.CurrentCultureFormat(Gather, OptionId.Language));
        }
    }
}