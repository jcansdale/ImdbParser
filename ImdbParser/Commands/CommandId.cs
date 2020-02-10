namespace ImdbParser.Commands {
    /// <summary>Command identifiers that are accepted from user input.</summary>
    enum CommandId {
        /// <summary>Default value indicating that command is not presented.</summary>
        None,
        /// <summary><see cref="Add" /> short version.</summary>
        A,
        /// <summary>Command identifier adding tags to the movie.</summary>
        Add = A,
        /// <summary><see cref="Create" /> short version.</summary>
        C,
        /// <summary>Command identifier which creates information from IMDB.</summary>
        Create = C,
        /// <summary><see cref="Extract" /> short version.</summary>
        E,
        /// <summary>Command identifier extracting tags from the movie.</summary>
        Extract = E,
        /// <summary>Command identifier which gathers information from IMDB.</summary>
        Gather,
        /// <summary><see cref="Gather" /> short version.</summary>
        G = Gather,
        /// <summary>Command identifier for deleting shortcuts.</summary>
        Remove,
        /// <summary><see cref="Remove" /> short version.</summary>
        Rem = Remove,
        /// <summary>Command identifier for renaming shortcuts.</summary>
        Rename,
        /// <summary><see cref="Rename" /> short version.</summary>
        Ren = Rename
    }
}