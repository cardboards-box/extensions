namespace CardboardBox.Extensions.Scripting.Templating;

/// <summary>
/// The data type for the requirement
/// </summary>
public enum RequirementType
{
    /// <summary>
    /// Unknown data type
    /// </summary>
    Unknown = 0,
    /// <summary>
    /// String data type
    /// </summary>
    Text = 1,
    /// <summary>
    /// Whole number data type
    /// </summary>
    Integer = 2,
    /// <summary>
    /// Decimal number data type
    /// </summary>
    Decimal = 3,
    /// <summary>
    /// True / False data type
    /// </summary>
    Boolean = 4,
    /// <summary>
    /// Incoming file data type
    /// </summary>
    File = 5,
    /// <summary>
    /// Just the date portion of a <see cref="System.DateTime"/>
    /// </summary>
    Date = 6,
    /// <summary>
    /// Just the time portion of a <see cref="System.DateTime"/>
    /// </summary>
    Time = 7,
    /// <summary>
    /// A <see cref="System.DateTime"/>
    /// </summary>
    DateTime = 8
}
