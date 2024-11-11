namespace CardboardBox.Extensions.Excel.Attributes;

/// <summary>
/// Represents an excel columns parsing data
/// </summary>
[AttributeUsage(AttributeTargets.Property, AllowMultiple = false)]
public class SerialAttribute : Attribute
{
    /// <summary>
    /// Whether or not to ignore this field
    /// </summary>
    public bool Ignore { get; set; } = false;

    /// <summary>
    /// The name of the column (found in the header)
    /// </summary>
    public string? Name { get; set; } = null;

    /// <summary>
    /// Whether or not to show this field
    /// </summary>
    public bool Show { get; set; } = true;

    /// <summary>
    /// Any string formatting to apply to this field
    /// </summary>
    public string? Format { get; set; } = null;

    /// <summary>
    /// Whether or not to convert this field to the absolute value version of itself
    /// </summary>
    public bool AbsoluteValue { get; set; } = false;

    /// <summary>
    /// The index of the column in the excel sheet
    /// </summary>
    public int? Index { get; set; } = null;

    /// <summary>
    /// Represents an excel columns parsing data
    /// </summary>
    /// <param name="name">The name of the column (found in the header)</param>
    public SerialAttribute(string name)
    {
        Name = name;
    }

    /// <summary>
    /// Represents an excel columns parsing data
    /// </summary>
    /// <param name="index">The index of the column in the excel sheet</param>
    public SerialAttribute(int index)
    {
        Index = index;
    }

    /// <summary>
    /// Represents an excel columns parsing data
    /// </summary>
    /// <param name="ignore">Whether or not to ignore this field</param>
    public SerialAttribute(bool ignore)
    {
        Ignore = ignore;
    }
}
