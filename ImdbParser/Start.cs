using ImdbParser.Commands;
using ImdbParser.Executors;
using ImdbParser.Html;
using ImdbParser.Settings;
using IWshRuntimeLibrary;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Win32;
using System;
using System.Diagnostics;
using System.IO;
using System.Threading.Tasks;
using IFile = System.IO.IFile;

namespace ImdbParser {
    /// <summary>Entry point manager.</summary>
    sealed class Start {
        /// <summary><see cref="ICommand" /> performer.</summary>
        readonly ICommandExecutor _executor;
        /// <summary>The standard output.</summary>
        readonly IConsole _wrapper;

        /// <summary>Initialize new <see cref="Start" /> instance.</summary>
        /// <param name="executor"><see cref="ICommand" /> performer.</param>
        /// <param name="wrapper">The standard output.</param>
        /// <exception cref="ArgumentNullException"><paramref name="executor" /> or <paramref name="wrapper" /> is null.</exception>
        /// <remarks>Must be public for dependency injection.</remarks>
        public Start(ICommandExecutor executor, IConsole wrapper) {
            _executor = executor.ThrowIfNull(nameof(executor));
            _wrapper = wrapper.ThrowIfNull(nameof(wrapper));
        }

        /// <summary>Program entry point.</summary>
        /// <param name="args">Command line arguments.</param>
        /// <returns><see cref="Task" /> to execute.</returns>
        internal static Task Main(string[] args) => MainWithConfiguration(args, _ => {});

        /// <summary>Program entry point.</summary>
        /// <param name="args">Command line arguments.</param>
        /// <param name="builder"><see cref="Action{T}" /> that allows to override dependency injection configuration.</param>
        /// <returns><see cref="Task" /> to execute.</returns>
        internal static Task MainWithConfiguration(string[] args, Action<IServiceCollection> builder) => ConfigureDependencyInjection(builder).GetService<Start>().ExecuteAsync(args);

        /// <summary>Configures dependency injection.</summary>
        /// <param name="builder"><see cref="Action{T}" /> that allows to override dependency injection configuration.</param>
        static ServiceProvider ConfigureDependencyInjection(Action<IServiceCollection> builder) {
            var collection = new ServiceCollection().AddTransient<IWshShell3>(_ => new WshShell()).AddTransient<AddExecutor>().AddTransient<ICommandExecutor, CommandExecutor>().
                AddTransient<CreateExecutor>().AddTransient<ExtractExecutor>().AddTransient<GatherExecutor>().AddTransient<RemoveExecutor>().AddTransient<RenameExecutor>().
                AddTransient<IHtmlGetter, HtmlGetter>().AddTransient<IHtmlManager, HtmlManager>().AddTransient<IInfoParserFactory, InfoParserFactory>().AddSingleton<ISettings, ApplicationSettings>().
                AddTransient<IConsole, ConsoleWrapper>().AddTransient<IDirectory, DirectoryWrapper>().AddTransient<IFile, FileWrapper>().AddTransient<IProcessStatic, ProcessStaticWrapper>().
                AddTransient<IRegistry, RegistryWrapper>().AddTransient<IFileNameBuilder, FileNameBuilder>().AddTransient<IMkvToolExecutor, MkvToolExecutor>().
                AddTransient<IShortcutCreator, ShortcutCreator>().AddTransient<Start>().AddTransient<ITagsCreator, TagsCreator>();
            builder(collection);
            return collection.BuildServiceProvider();
        }

        /// <summary>Executes application.</summary>
        /// <param name="arguments">Command line arguments.</param>
        /// <returns><see cref="Task" /> to execute.</returns>
        internal async Task ExecuteAsync(string[] arguments) {
            var command = arguments.ParseArguments();
            _wrapper.WriteLine(command.IsSuccess ? (await _executor.ExecuteCommandAsync(command.Value).ConfigureAwait(default)).Map(result => result.IsSuccess ? result.Value : result.Failure) :
                command.Failure);
        }
    }
}