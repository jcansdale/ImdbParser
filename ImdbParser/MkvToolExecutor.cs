using ImdbParser.Settings;
using Microsoft.Win32;
using System;
using System.Diagnostics;
using System.IO;
using static ImdbParser.Messages;
using static System.Diagnostics.DebuggerBrowsableState;
using static System.FormattableString;
using static System.IO.Path;
using static System.Result;
using static System.String;

namespace ImdbParser {
    /// <summary>Executes MKV Tool Nix commands.</summary>
    interface IMkvToolExecutor {
        /// <summary>Adds tags to <paramref name="movie" />.</summary>
        /// <param name="movie">Movie to which tags should be added.</param>
        /// <returns>Operation result.</returns>
        IResult AddTags(FileInfo movie);

        /// <summary>Extracts tags from <paramref name="movie" />.</summary>
        /// <param name="movie">Movie from which tags should be extracted.</param>
        /// <returns>Operation result.</returns>
        IResult ExtractTags(FileInfo movie);
    }

    /// <summary>Executes MKV Tool Nix commands.</summary>
    sealed class MkvToolExecutor : IMkvToolExecutor {
        /// <summary>MKV Extract executable name.</summary>
        [DebuggerBrowsable(Never)] const string MkvExtractExecutable = "mkvextract";
        /// <summary>MKV Prop Edit executable name.</summary>
        [DebuggerBrowsable(Never)] const string MkvPropEditExecutable = "mkvpropedit";
        /// <summary>Registry path to MKV Tool Nix registry entry.</summary>
        [DebuggerBrowsable(Never)] const string RegistryPath = @"HKEY_LOCAL_MACHINE\SOFTWARE\Microsoft\Windows\CurrentVersion\App Paths\mkvtoolnix-gui.exe";
        /// <summary>Operations with files performer.</summary>
        readonly IFile _file;
        /// <summary>Processes creator.</summary>
        readonly IProcessStatic _process;
        /// <summary>Registry reader.</summary>
        readonly IRegistry _registry;
        /// <summary>Application settings.</summary>
        readonly ISettings _settings;

        /// <summary>Initialize new <see cref="MkvToolExecutor" /> instance.</summary>
        /// <param name="file">Operations with files performer.</param>
        /// <param name="process">Processes creator.</param>
        /// <param name="registry">Registry reader.</param>
        /// <param name="settings">Application settings.</param>
        /// <remarks>Must be public for dependency injection.</remarks>
        public MkvToolExecutor(IFile file, IProcessStatic process, IRegistry registry, ISettings settings) {
            _file = file;
            _process = process;
            _registry = registry;
            _settings = settings;
        }

        /// <summary>Adds tags to <paramref name="movie" />.</summary>
        /// <param name="movie">Movie to which tags should be added.</param>
        /// <returns>Operation result.</returns>
        public IResult AddTags(FileInfo movie) => GetExecutablePath(MkvPropEditExecutable).OnSuccess(executable => {
            using var process = _process.Start(new ProcessStartInfo(executable, Invariant($"\"{movie}\" --tags all:\"{_settings.TagsName}\"")) { UseShellExecute = false });
            process.WaitForExit();
        });

        /// <summary>Extracts tags from <paramref name="movie" />.</summary>
        /// <param name="movie">Movie from which tags should be extracted.</param>
        /// <returns>Operation result.</returns>
        public IResult ExtractTags(FileInfo movie) => GetExecutablePath(MkvExtractExecutable).OnSuccess(executable => {
            using var process = _process.Start(new ProcessStartInfo(executable, Invariant($"tags \"{movie}\" -r \"{_settings.TagsName}\"")) { UseShellExecute = false });
            process.WaitForExit();
        });

        /// <summary>Gets MKV Tool executable path.</summary>
        /// <param name="executable">Executable which needed.</param>
        /// <returns>MKV Tool executable path.</returns>
        IResult<string> GetExecutablePath(string executable) => _registry.GetValue(RegistryPath, Empty, default).MapIf(path => path == default, _ => Fail<string>(MKV_Tool_Nix_is_not_installed),
            path => Combine(GetDirectoryName(path.CastTo<string>())!, Invariant($"{executable}.exe")).
                MapIf(_file.Exists, Ok, exePath => Fail<string>(MKV_Tool_Nix_executable_not_found.InvariantCultureFormat(exePath))));
    }
}