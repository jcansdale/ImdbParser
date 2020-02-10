using NUnit.Framework;
using System.Diagnostics.CodeAnalysis;
using System.Threading.Tasks;
using static ImdbParser.IntegrationTests.TestsBase;
using static ImdbParser.Messages;
using static NUnit.Framework.Assert;
using static NUnit.Framework.Is;

namespace ImdbParser.IntegrationTests {
    /// <summary>General arguments validation tests.</summary>
    [ExcludeFromCodeCoverage]
    static class GeneralArgumentsValidationTests {
        /// <summary>Try to call without command.</summary>
        /// <param name="arguments">Command line arguments.</param>
        /// <returns><see cref="Task" /> to execute.</returns>
#pragma warning disable CS3016 // Arrays as attribute arguments is not CLS-compliant
        [TestCase, TestCase(""), TestCase("+Create")]
#pragma warning restore CS3016 // Arrays as attribute arguments is not CLS-compliant
        public static async Task InvalidArguments_FailureReturned(params string[] arguments) => That((await ExecuteAsync(arguments).ConfigureAwait(default)).message, EqualTo(No_command));

        /// <summary>Try to call with invalid command.</summary>
        /// <param name="arguments">Command line arguments.</param>
        /// <returns><see cref="Task" /> to execute.</returns>
        [TestCase("-"), TestCase("-Unknown"), TestCase("-None"), TestCase("-127"), TestCase("--Create")]
        public static async Task InvalidCommand_FailureReturned(string arguments) => That((await ExecuteAsync(arguments).ConfigureAwait(default)).message, EqualTo(Unknown_command));

        /// <summary>Try to call with invalid option.</summary>
        /// <param name="arguments">Command line arguments.</param>
        /// <returns><see cref="Task" /> to execute.</returns>
        [TestCase("-"), TestCase("-Unknown"), TestCase("-None"), TestCase("-127"), TestCase("--Language")]
        public static async Task InvalidOption_FailureReturned(string arguments) => That((await ExecuteAsync("-Gather", arguments).ConfigureAwait(default)).message, EqualTo(Unknown_option));
    }
}