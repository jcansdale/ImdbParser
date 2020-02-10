using System;
using System.Collections.Generic;
using System.Linq;
using static ImdbParser.Messages;
using static ImdbParser.Options.OptionId;
using static System.Result;

namespace ImdbParser.Options {
    /// <summary>Languages <see cref="IOption" /> that identifies what languages subtitles are available.</summary>
    sealed class SubtitlesOption : LanguageOption {
        /// <summary>Initializes new <see cref="SubtitlesOption" /> instance.</summary>
        /// <param name="languages">Languages associated with this sub command.</param>
        SubtitlesOption(IEnumerable<Language> languages) : base(languages) {}

        /// <summary>Tries to create <see cref="SubtitlesOption" /> instance.</summary>
        /// <param name="parameters"><see cref="IOption" />'s parameters.</param>
        /// <returns><see cref="SubtitlesOption" /> instance if <paramref name="parameters" /> are valid.</returns>
        internal static new IResult<SubtitlesOption> CreateOption(IReadOnlyCollection<string> parameters) => parameters.Any() ?
            GetLanguages(parameters, Subtitles).OnSuccess(languages => new SubtitlesOption(languages)) : Fail<SubtitlesOption>(Option_has_no_language.InvariantCultureFormat(Subtitles));
    }
}