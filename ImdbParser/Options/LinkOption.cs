using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Diagnostics.CodeAnalysis;
using System.IO;
using System.Linq;
using static ImdbParser.Messages;

namespace ImdbParser.Options {
    /// <summary>Link <see cref="IOption" /> used to provide link to the movie.</summary>
    sealed class LinkOption : IOption {
        /// <summary>Initialize new <see cref="LinkOption" /> instance.</summary>
        /// <param name="file">File associated with this <see cref="IOption" />.</param>
        LinkOption(FileInfo file) => File = file;

        /// <summary>File associated with this <see cref="IOption" />.</summary>
        /// <value>Gets file associated with this <see cref="IOption" />.</value>
        [SuppressMessage("Microsoft.Performance", "CA1822:Mark members as static", Justification = "False positive.")]
        internal FileInfo File { [DebuggerStepThrough] get; }

        /// <summary>Tries to create <see cref="LanguageOption" /> instance.</summary>
        /// <param name="parameters"><see cref="IOption" />'s parameters.</param>
        /// <returns><see cref="LanguageOption" /> instance if <paramref name="parameters" /> are valid.</returns>
        internal static IResult<LinkOption> CreateOption(IReadOnlyCollection<string> parameters) => CreateOption(parameters, new FileWrapper());

        /// <summary>Tries to create <see cref="LanguageOption" /> instance.</summary>
        /// <param name="parameters"><see cref="IOption" />'s parameters.</param>
        /// <param name="file">Operations with files performer.</param>
        /// <returns><see cref="LanguageOption" /> instance if <paramref name="parameters" /> are valid.</returns>
        internal static IResult<LinkOption> CreateOption(IReadOnlyCollection<string> parameters, IFile file) {
            if (!parameters.Any()) return Result.Fail<LinkOption>(Link_has_no_path);
            var path = parameters.First();
            return file.Exists(path) ? Result.Ok(new LinkOption(new FileInfo(path))) : Result.Fail<LinkOption>(File_does_not_exist);
        }
    }
}