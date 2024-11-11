using System.Reflection;

namespace CardboardBox.Extensions.Excel.Attributes;

/// <summary>
/// Represents an excel column that has been mapped to an object property
/// </summary>
/// <param name="prop">The property the column hsa been mapped to</param>
public class SerialMap(PropertyInfo prop)
{
    /// <summary>
    /// The property the column hsa been mapped to
    /// </summary>
    public PropertyInfo Property { get; set; } = prop;
    /// <summary>
    /// Any serialization data the column has
    /// </summary>
    public SerialAttribute? Attr { get; set; } = Attribute.GetCustomAttribute(prop, typeof(SerialAttribute)) as SerialAttribute;

    /// <summary>
    /// Whether or not to ignore this column
    /// </summary>
    public bool Ignore => Attr?.Ignore ?? false;

    /// <summary>
    /// The name of the column
    /// </summary>
    public string? Name => Attr?.Name ?? Property.Name;

    /// <summary>
    /// The string formatting options for this column
    /// </summary>
    public string? Format => Attr?.Format ?? null;

    /// <summary>
    /// The index of the column or -1 if none is specified
    /// </summary>
    public int Index => Attr?.Index ?? -1;

    /// <summary>
    /// Whether or not to absolute value the contents of this cell
    /// </summary>
    public bool AbsoluteValue => Attr?.AbsoluteValue ?? false;

    /// <summary>
    /// Sets the property of the given object
    /// </summary>
    /// <param name="instance">The object to set the property on</param>
    /// <param name="value">The value to set on the property</param>
    public void Set(object instance, object value)
    {
        try
        {
            Property.SetValue(instance, Convert.ChangeType(value, Property.PropertyType));
        }
        catch { }
    }
}
