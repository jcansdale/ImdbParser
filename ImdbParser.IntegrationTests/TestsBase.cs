using ImdbParser.Settings;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Diagnostics.CodeAnalysis;
using System.Threading.Tasks;

namespace ImdbParser.IntegrationTests {
    /// <summary>Holds information used by all tests.</summary>
    [ExcludeFromCodeCoverage]
    static class TestsBase {
        /// <summary>Executes system under test.</summary>
        /// <param name="arguments">Command line arguments.</param>
        /// <returns>Execution result and settings used for the execution.</returns>
        internal static Task<(string message, ISettings settings)> ExecuteAsync(params string[] arguments) => ExecuteAsync(_ => {}, arguments);

        /// <summary>Executes system under test.</summary>
        /// <param name="builder"><see cref="Action{T}" /> that allows to override dependency injection configuration.</param>
        /// <param name="arguments">Command line arguments.</param>
        /// <returns>Execution result and settings used for the execution.</returns>
        internal static async Task<(string message, ISettings settings)> ExecuteAsync(Action<IServiceCollection> builder, params string[] arguments) {
            var console = new TestConsole();
            var settings = new TestSettings();
            await Start.MainWithConfiguration(arguments, collection => {
                collection.AddSingleton<ISettings>(settings).AddSingleton<IConsole>(console);
                builder(collection);
            }).ConfigureAwait(default);
            return (console.Output, settings);
        }
    }
}