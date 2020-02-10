namespace ImdbParser.Options {
    /// <summary>Option identifiers that are accepted from user input.</summary>
    enum OptionId {
        /// <summary>Default value indicating that option is not presented.</summary>
        None,
        /// <summary>Option used to provide soundtrack languages for command.</summary>
        Language,
        /// <summary><see cref="Language" /> short version.</summary>
        La = Language,
        /// <summary>Option used to provide link to the movie.</summary>
        Link,
        /// <summary><see cref="Link" /> short version.</summary>
        Li = Link,
        /// <summary>Option used to provide subtitles languages for command.</summary>
        Subtitles,
        /// <summary><see cref="Subtitles" /> short version.</summary>
        S = Subtitles
    }
}