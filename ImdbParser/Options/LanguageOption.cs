using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Diagnostics.CodeAnalysis;
using System.Linq;
using static ImdbParser.Language;
using static ImdbParser.Messages;
using static System.Diagnostics.DebuggerBrowsableState;
using static System.FormattableString;
using static System.Result;
using static System.String;

namespace ImdbParser.Options {
    /// <summary>Languages <see cref="IOption" /> that identifies what languages soundtrack is available.</summary>
    [DebuggerDisplay("{" + nameof(DebuggerDisplay) + ",nq}")]
    class LanguageOption : IOption {
        /// <summary>Initializes new <see cref="LanguageOption" /> instance.</summary>
        /// <param name="languages">Languages associated with this sub command.</param>
        private protected LanguageOption(IEnumerable<Language> languages) => Languages = languages.OrderBy(language => language);

        /// <summary>Languages associated with this <see cref="IOption" />.</summary>
        /// <value>Gets languages associated with this <see cref="IOption" />.</value>
        [SuppressMessage("Microsoft.Performance", "CA1822:Mark members as static", Justification = "False positive.")]
        internal IEnumerable<Language> Languages { [DebuggerStepThrough] get; }

        /// <summary>Value used for <see cref="DebuggerDisplayAttribute" />.</summary>
        /// <value>Gets value used for <see cref="DebuggerDisplayAttribute" />.</value>
        [DebuggerBrowsable(Never)]
        // ReSharper disable once UnusedMember.Local
        string DebuggerDisplay { [ExcludeFromCodeCoverage] get => Invariant($"{nameof(Languages)}: {Join(", ", Languages)}"); }

        /// <summary>Tries to get valid unique <see cref="Language" />s from <paramref name="parameters" />.</summary>
        /// <param name="parameters"><see cref="IOption" />'s parameters.</param>
        /// <param name="option"><see cref="IOption" /> used in error messages.</param>
        /// <returns><see cref="Language" /> represented by <paramref name="parameters" />.</returns>
        private protected static Result<HashSet<Language>> GetLanguages(IEnumerable<string> parameters, OptionId option = OptionId.Language) {
            var languages = new HashSet<Language>();
            foreach (var language in parameters.Select(parameter => parameter.ParseEnumerable<Language>())) {
                if (language == None) return Fail<HashSet<Language>>(String_not_language.InvariantCultureFormat(option));
                if (languages.Add(language).Negation()) return Fail<HashSet<Language>>(Language_not_unique);
            }
            return Ok(languages);
        }

        /// <summary>Tries to create <see cref="LanguageOption" /> instance.</summary>
        /// <param name="parameters"><see cref="IOption" />'s parameters.</param>
        /// <returns><see cref="LanguageOption" /> instance if <paramref name="parameters" /> are valid.</returns>
        internal static IResult<LanguageOption> CreateOption(IReadOnlyCollection<string> parameters) => parameters.Any() ?
            GetLanguages(parameters).OnSuccess(languages => new LanguageOption(languages)) : Fail<LanguageOption>(Option_has_no_language.InvariantCultureFormat(OptionId.Language));
    }
}