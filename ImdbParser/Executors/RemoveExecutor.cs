using ImdbParser.Commands;
using ImdbParser.Settings;
using System;
using System.Linq;
using System.Threading.Tasks;
using static System.FormattableString;
using static System.IO.SearchOption;
using static System.Result;
using static System.String;
using static System.Threading.Tasks.Parallel;
using static System.Threading.Tasks.Task;

namespace ImdbParser.Executors {
    /// <summary>Executes <see cref="RemoveCommand" />.</summary>
    sealed class RemoveExecutor : IExecutor<RemoveCommand> {
        /// <summary>Application settings.</summary>
        readonly ISettings _settings;

        /// <summary>Initialize new <see cref="RemoveExecutor" /> instance.</summary>
        /// <param name="settings">Application settings.</param>
        /// <remarks>Must be public for dependency injection.</remarks>
        public RemoveExecutor(ISettings settings) => _settings = settings;

        /// <summary>Executes <paramref name="command" />.</summary>
        /// <param name="command">Command to execute.</param>
        /// <returns>Text to print after operation.</returns>
        public Task<IResult<string>> ExecuteAsync(RemoveCommand command) {
            ForEach(_settings.LibraryDirectory.EnumerateDirectories("*", AllDirectories), directory => {
                var file = directory.EnumerateFiles(Invariant($"*{command.CurrentFileName.Name}.lnk")).FirstOrDefault();
                if (file == default) return;
                file.Delete();
                if (!directory.EnumerateFileSystemInfos().Any()) directory.Delete();
            });
            return FromResult<IResult<string>>(Ok(Empty));
        }
    }
}