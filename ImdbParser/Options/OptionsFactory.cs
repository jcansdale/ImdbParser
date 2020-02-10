using System;
using System.Collections.Generic;
using System.ComponentModel;
using static ImdbParser.Options.OptionId;

namespace ImdbParser.Options {
    /// <summary>Creates <see cref="IOption" /> based on <see cref="OptionId" />.</summary>
    static class OptionsFactory {
        /// <summary>Creates <see cref="IOption" /> based on <paramref name="option" /> with <paramref name="parameters" />.</summary>
        /// <param name="option"><see cref="OptionId" /> that identifies <see cref="IOption" /> to create.</param>
        /// <param name="parameters"><see cref="IOption" /> parameters.</param>
        /// <returns>Created <see cref="IOption" /> based on <paramref name="option" />.</returns>
        /// <exception cref="InvalidEnumArgumentException"><paramref name="option" /> is <see cref="None" />.</exception>
        /// <exception cref="NotImplementedException">No <see cref="IOption" /> creation is provided for <paramref name="option" />.</exception>
        internal static IResult<IOption> CreateOption(this OptionId option, IReadOnlyCollection<string> parameters) =>
            option.ThrowIf(value => value == None, () => new InvalidEnumArgumentException(nameof(option), (int)None, typeof(OptionId))) == OptionId.Language ? LanguageOption.CreateOption(parameters) :
            option == Link ? (IResult<IOption>)LinkOption.CreateOption(parameters) : option == Subtitles ? SubtitlesOption.CreateOption(parameters) : throw new NotImplementedException();
    }
}