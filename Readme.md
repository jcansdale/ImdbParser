# IMDB Parser
![Build status](https://github.com/SDLionas/ImdbParser/workflows/Build/badge.svg)

Parses movie information from [IMDB](https://www.imdb.com).
>To run `Extract` and `Remove` commands, you need [MKV Tool Nix](https://mkvtoolnix.download) installed.

Possible commands (some commands needs options):
- `Add` (`A`). Adds tags to the movie:
    >`-Add <Path to movie>`
- `Create` (`C`). Collects and creates information about a movie from IMDB:
    >`-Create <IMDB URL> -Language -Link {-Subtitles}`
- `Extact` (`E`). Extracts tags from the movie:
    >`-Extact <Path to movie>`
- `Gather` (`G`). Collects information about a movie from IMDB:
    >`-Gather <IMDB URL> -Language {-Subtitles}`
- `Remove` (`Rem`). Removes existing shortcuts:
    >`-Remove <Current file to remove>`
- `Rename` (`Ren`). Renames existing shortcuts to the new one:
    >`-Rename <Current file to rename> <Path to movie for new shotcut>`

Possible options for commands:
- `Language` (`La`). Identifies what languages soundtrack is available:
    >`-Language <Lt/En>`
- `Link` (`Li`). Provide link to the movie:
    >`-Link <Path to movie>`
- `Subtitles` (`S`). Identifies what languages subtitles are available:
    >`-Subtitles <Lt/En>`