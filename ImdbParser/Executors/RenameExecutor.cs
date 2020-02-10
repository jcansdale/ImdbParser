using ImdbParser.Commands;
using ImdbParser.Settings;
using System;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using static System.FormattableString;
using static System.IO.SearchOption;
using static System.Result;
using static System.String;
using static System.Threading.Tasks.Parallel;
using static System.Threading.Tasks.Task;

namespace ImdbParser.Executors {
    /// <summary>Executes <see cref="RenameCommand" />.</summary>
    sealed class RenameExecutor : IExecutor<RenameCommand> {
        /// <summary>Shortcuts creator.</summary>
        readonly IShortcutCreator _creator;
        /// <summary>Application settings.</summary>
        readonly ISettings _settings;

        /// <summary>Initialize new <see cref="RenameExecutor" /> instance.</summary>
        /// <param name="creator">Shortcuts creator.</param>
        /// <param name="settings">Application settings.</param>
        /// <remarks>Must be public for dependency injection.</remarks>
        public RenameExecutor(IShortcutCreator creator, ISettings settings) {
            _creator = creator;
            _settings = settings;
        }

        /// <summary>Executes <paramref name="command" />.</summary>
        /// <param name="command">Command to execute.</param>
        /// <returns>Text to print after operation.</returns>
        public Task<IResult<string>> ExecuteAsync(RenameCommand command) {
            ForEach(_settings.LibraryDirectory.EnumerateDirectories("*", AllDirectories), directory => RenameSingleFolder(command, directory));
            return FromResult<IResult<string>>(Ok(Empty));
        }

        /// <summary>Renames files in a single folder.</summary>
        /// <param name="command">Command to execute.</param>
        /// <param name="directory">Directory to manage.</param>
        /// <returns>Failures.</returns>
        void RenameSingleFolder(RenameCommand command, IDirectoryInfo directory) => directory.EnumerateFiles(Invariant($"*{command.CurrentFileName.Name}.lnk")).FirstOrDefault()?.
            ParallelDo(fileToUsed => fileToUsed.Delete(), _ => _creator.CreateShortcut(directory, command.NewFile));
    }
}