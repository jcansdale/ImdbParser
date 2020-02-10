using System.Collections.Generic;
using System.Linq;

namespace ImdbParser.Options {
    /// <summary>Contains common options validations.</summary>
    static class OptionsValidator {
        /// <summary>Checks if all options are unique.</summary>
        /// <param name="options"><see cref="IOption" />s to validate.</param>
        /// <returns>True if all options are unique; false - otherwise.</returns>
        internal static bool AreUnique(this IEnumerable<IOption> options) => options.GroupBy(option => option.GetType()).All(collection => 1 == collection.Count());
    }
}