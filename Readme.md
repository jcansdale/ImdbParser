# Template
This repository contains build properties, code analysis rulesets, and code style that can be used as a template for any new repository.
## Build properties
`Build properties` folder contains build properties that can be used in other projects:
- [Base](Build%20properties/Base.props) - contains base properties that all projects should have.
- [CodeAnalysis](Build%20properties/CodeAnalysis.props) - contains code analysis NuGet packages.
- [NetCoreBase](Build%20properties/NetCoreBase.props) - extends [base](Build%20properties/Base.props) properties for .NET Core.
- [NetStandardBase](Build%20properties/NetStandardBase.props) - extends [base](Build%20properties/Base.props) properties for .NET Standard.
- [NetStandardCodeAnalysis](Build%20properties/NetStandardCodeAnalysis.props) - sets up code analysis for .NET Standard projects.
- [Tests](Build%20properties/Tests.props) - contains NuGet packages required for tests to run.
- [TestsCodeAnalysis](Build%20properties/TestsCodeAnalysis.props) - sets up code analysis for test projects.
## Code analysis rulesets
`Code analysis rulesets` folder contains code analysis rule sets that can be used in other projects:
- [.NET Standard](Code%20analysis%20rulesets/.NET%20Standard.ruleset) - contains all .NET Standard rules set as errors.
- [Tests](Code%20analysis%20rulesets/Tests.ruleset) - contains all tests rules set as errors.
## Code style
The root folder contains code style files that can be used in other projects:
- [.editorconfig](.editorconfig) - defines code style and naming conventions that should be followed.
- [{Solution name}.sln.DotSettings]({Solution%20Name}.sln.DotSettings) - defines ReSharper settings that should be followed. This file contains settings from all ReSharper extensions.
  >**Note!** This file overrides only default ReSharper settings and does not contain all available settings.
  
  >If you want to override any settings you should do that in solution personal layer (`*.sln.DotSettings.user` file).

  >The file also contains some dictionary entries bellow `<!--##ReSpeller-->` comment.