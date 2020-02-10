using ImdbParser.Commands;
using ImdbParser.Options;
using NUnit.Framework;
using System;
using System.Diagnostics.CodeAnalysis;
using static ImdbParser.Commands.CommandId;
using static ImdbParser.Language;
using static ImdbParser.Messages;
using static ImdbParser.Options.OptionId;
using static ImdbParser.Tests.TestsBase;
using static NUnit.Framework.Assert;
using static NUnit.Framework.Is;
using static System.FormattableString;
using static System.IO.Path;

namespace ImdbParser.Tests {
    /// <summary><see cref="ArgumentsParser" /> tests.</summary>
    [ExcludeFromCodeCoverage]
    static class ArgumentsParserTests {
#region ParseArguments tests
        /// <summary><see cref="ArgumentsParser.ParseArguments" /> test.</summary>
        /// <param name="arguments">Command line arguments from which <see cref="ICommand" /> is parsed.</param>
#pragma warning disable CS3016 // Arrays as attribute arguments is not CLS-compliant
        [TestCase, TestCase(""), TestCase("+Create")]
#pragma warning restore CS3016 // Arrays as attribute arguments is not CLS-compliant
        public static void ParseArguments_ArgumentsInvalid_FailedReturned(params string[] arguments) => arguments.ParseArguments().ExpectFail(() => No_command);

        /// <summary><see cref="ArgumentsParser.ParseArguments" /> test.</summary>
        /// <param name="command">Command to pass.</param>
        [TestCase("-"), TestCase("-Unknown"), TestCase("-None"), TestCase("-127"), TestCase("--Add")]
        public static void ParseArgument_UnknownCommand_FailedReturned(string command) => new[] { command }.ParseArguments().ExpectFail(() => Unknown_command);

        /// <summary><see cref="ArgumentsParser.ParseArguments" /> test.</summary>
        /// <param name="option">Option to pass.</param>
        [TestCase("-"), TestCase("-Unknown"), TestCase("-None"), TestCase("-127"), TestCase("--Language")]
        public static void ParseArgument_UnknownOption_FailedReturned(string option) => new[] { Create.MakeCommand(), option }.ParseArguments().ExpectFail(() => Unknown_option);

        /// <summary><see cref="ArgumentsParser.ParseArguments" /> test.</summary>
        [Test]
        public static void ParseArgument_FullAddCommand_AddCommandReturned() => new[] { Add.MakeCommand(), Path }.ParseArguments().ExpectSuccess(command => {
            That(command, TypeOf(typeof(AddCommand)));
            That(((AddCommand)command).MoviePath.ToString, SameAs(Path));
        });

        /// <summary><see cref="ArgumentsParser.ParseArguments" /> test.</summary>
        [Test]
        public static void ParseArgument_MinimalCreateCommand_CreateCommandReturned() =>
            new[] { Create.MakeCommand(), ImdbTestUrl, OptionId.Language.MakeCommand(), Lt.ToString(), Link.MakeCommand(), Path }.ParseArguments().ExpectSuccess(command => {
                That(command, TypeOf<CreateCommand>());
                var createCommand = (CreateCommand)command;
                Multiple(() => {
                    That(createCommand.Url.ToString(), EqualTo(ImdbTestUrl));
                    That(createCommand.Language.Languages, EquivalentTo(new[] { Lt }));
                    That(createCommand.Link.File.ToString(), SameAs(Path));
                    That(createCommand.Subtitles, Is.Null);
                });
            });

        /// <summary><see cref="ArgumentsParser.ParseArguments" /> test.</summary>
        [Test]
        public static void ParseArgument_FullCreateCommand_CreateCommandReturned() => new[]
                { Create.MakeCommand(), ImdbTestUrl, OptionId.Language.MakeCommand(), Lt.ToString(), En.ToString(), Link.MakeCommand(), Path, Subtitles.MakeCommand(), Lt.ToString(), En.ToString() }.
            ParseArguments().ExpectSuccess(command => {
                That(command, TypeOf(typeof(CreateCommand)));
                var createCommand = (CreateCommand)command;
                Multiple(() => {
                    That(createCommand.Url.ToString(), EqualTo(ImdbTestUrl));
                    That(createCommand.Language.Languages, EquivalentTo(new[] { Lt, En }));
                    That(createCommand.Link.File.ToString(), SameAs(Path));
                    That(createCommand.Subtitles?.Languages, EquivalentTo(new[] { Lt, En }));
                });
            });

        /// <summary><see cref="ArgumentsParser.ParseArguments" /> test.</summary>
        [Test]
        public static void ParseArgument_FullExtractCommand_ExtractCommandReturned() => new[] { Extract.MakeCommand(), Path }.ParseArguments().ExpectSuccess(command => {
            That(command, TypeOf(typeof(ExtractCommand)));
            That(((ExtractCommand)command).MoviePath.ToString, SameAs(Path));
        });

        /// <summary><see cref="ArgumentsParser.ParseArguments" /> test.</summary>
        [Test]
        public static void ParseArgument_MinimalGatherCommand_GatherCommandReturned() => new[] { Gather.MakeCommand(), ImdbTestUrl, OptionId.Language.MakeCommand(), Lt.ToString() }.ParseArguments().
            ExpectSuccess(command => {
                That(command, TypeOf<GatherCommand>());
                var gatherCommand = (GatherCommand)command;
                Multiple(() => {
                    That(gatherCommand.Url.ToString(), EqualTo(ImdbTestUrl));
                    That(gatherCommand.Language.Languages, EquivalentTo(new[] { Lt }));
                    That(gatherCommand.Subtitles, Is.Null);
                });
            });

        /// <summary><see cref="ArgumentsParser.ParseArguments" /> test.</summary>
        [Test]
        public static void ParseArgument_FullGatherCommand_GatherCommandReturned() => new[] {
            Gather.MakeCommand(), ImdbTestUrl, OptionId.Language.MakeCommand(), Lt.ToString(), En.ToString(), Subtitles.MakeCommand(), Lt.ToString(), En.ToString()
        }.ParseArguments().ExpectSuccess(command => {
            That(command, TypeOf(typeof(GatherCommand)));
            var gatherCommand = (GatherCommand)command;
            Multiple(() => {
                That(gatherCommand.Url.ToString(), EqualTo(ImdbTestUrl));
                That(gatherCommand.Language.Languages, EquivalentTo(new[] { Lt, En }));
                That(gatherCommand.Subtitles?.Languages, EquivalentTo(new[] { Lt, En }));
            });
        });

        /// <summary><see cref="ArgumentsParser.ParseArguments" /> test.</summary>
        [Test]
        public static void ParseArgument_FullRemoveCommand_RemoveCommandReturned() {
            var currentFileName = GetRandomFileName();

            new[] { Remove.MakeCommand(), currentFileName }.ParseArguments().ExpectSuccess(command => {
                That(command, TypeOf(typeof(RemoveCommand)));
                That(((RemoveCommand)command).CurrentFileName.ToString, SameAs(currentFileName));
            });
        }

        /// <summary><see cref="ArgumentsParser.ParseArguments" /> test.</summary>
        [Test]
        public static void ParseArgument_FullRenameCommand_RenameCommandReturned() {
            var currentFileName = GetRandomFileName();

            new[] { Rename.MakeCommand(), currentFileName, Path }.ParseArguments().ExpectSuccess(command => {
                That(command, TypeOf(typeof(RenameCommand)));
                var renameCommand = (RenameCommand)command;
                Multiple(() => {
                    That(renameCommand.CurrentFileName.ToString, SameAs(currentFileName));
                    That(renameCommand.NewFile.ToString, SameAs(Path));
                });
            });
        }
#endregion

        /// <summary>Creates command from <paramref name="command" />.</summary>
        /// <typeparam name="T"><see cref="Type" /> of command identifier.</typeparam>
        /// <param name="command">Command identifier.</param>
        /// <returns><see cref="string" /> representation of a command.</returns>
        static string MakeCommand<T>(this T command) where T : struct => Invariant($"-{command}");
    }
}