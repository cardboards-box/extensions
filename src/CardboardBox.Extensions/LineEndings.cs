namespace CardboardBox.Extensions;

/// <summary>
/// Indicates the type of line endings to use
/// </summary>
[Flags]
public enum LineEndings
{
    /// <summary>
    /// Could not determine the line endings
    /// </summary>
    Unknown = 0,

    /// <summary>
    /// The carriage return character "\r"
    /// </summary>
    CarriageReturn = 1,

    /// <summary>
    /// The line feed character "\n"
    /// </summary>
    LineFeed = 2,

    /// <summary>
    /// The carriage return and line feed characters "\r\n"
    /// </summary>
    CarriageReturnLineFeed = CarriageReturn | LineFeed,

    /// <summary>
    /// The line endings used by Windows
    /// </summary>
    Windows = CarriageReturnLineFeed,

    /// <summary>
    /// The line endings used by Unix-based systems
    /// </summary>
    Unix = LineFeed
}