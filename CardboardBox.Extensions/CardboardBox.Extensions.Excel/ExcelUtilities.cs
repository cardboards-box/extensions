namespace CardboardBox.Extensions.Excel;

using Attributes;

/// <summary>
/// Utilities for working with excel files
/// </summary>
public static class ExcelUtilities
{
    /// <summary>
    /// Gets all of the properties and their <see cref="SerialAttribute"/>s from the given type 
    /// </summary>
    /// <param name="type">The type to get the properties from</param>
    /// <returns>A collection of properties and their <see cref="SerialAttribute"/>s</returns>
    public static KeyValuePair<PropertyInfo, SerialAttribute?>[] GetProperties(Type type)
    {
        return type.GetProperties()
            .Select(t =>
            {
                var csv = Attribute.IsDefined(t, typeof(SerialAttribute)) ? Attribute.GetCustomAttribute(t, typeof(SerialAttribute)) as SerialAttribute : null;
                return new KeyValuePair<PropertyInfo, SerialAttribute?>(t, csv);
            })
            .Where(t => !(t.Value?.Ignore ?? false))
            .ToArray();
    }

    /// <summary>
    /// Gets all of the properties from the given type
    /// </summary>
    /// <param name="type">The type to fetch the properties from</param>
    /// <returns>All of the properties from the given type.</returns>
    public static SerialMap[] GetProps(Type type)
    {
        return type.GetProperties()
            .Select(t => new SerialMap(t))
            .Where(t => !t.Ignore)
            .OrderBy(t => t.Index)
            .ToArray();
    }

    /// <summary>
    /// Gets the value of the given property from the given record
    /// </summary>
    /// <typeparam name="T">The type of record to fetch from</typeparam>
    /// <param name="record">The record to fetch the value from</param>
    /// <param name="map">The property to fetch from the record</param>
    /// <returns>The string value of the property</returns>
    public static string GetStringValue<T>(T record, SerialMap map)
    {
        return GetStringValue(record, new KeyValuePair<PropertyInfo, SerialAttribute?>(map.Property, map.Attr));
    }

    /// <summary>
    /// Gets the value of the given property from the given record
    /// </summary>
    /// <typeparam name="T">The type of record to fetch from</typeparam>
    /// <param name="record">The record to fetch the value from</param>
    /// <param name="type">The property to fetch from the record</param>
    /// <returns>The string value of the property</returns>
    public static string GetStringValue<T>(T record, KeyValuePair<PropertyInfo, SerialAttribute?> type)
    {
        var propVal = type.Key.GetValue(record);
        var propType = type.Key.PropertyType;

        if (propVal == null)
            return "";

        if (type.Value?.AbsoluteValue ?? false && typeof(IConvertible).IsAssignableFrom(propType))
        {
            propVal = Math.Abs(((IConvertible)propVal).ToDecimal(CultureInfo.CurrentCulture));
        }

        if (string.IsNullOrEmpty(type.Value?.Format))
        {
            return propVal?.ToString() ?? "";
        }

        if (propType == typeof(bool) || propType == typeof(bool?))
        {
            var split = type.Value.Format.Split('|');
            if (split.Length != 2)
                return ((bool)propVal).ToString();

            return (bool)propVal ? split[0] : split[1];
        }

        if (typeof(IFormattable).IsAssignableFrom(propType))
        {
            return ((IFormattable)propVal).ToString(type.Value.Format, CultureInfo.CurrentCulture);
        }

        var underlyingPropertyType = Nullable.GetUnderlyingType(propType);

        if (underlyingPropertyType != null && typeof(IFormattable).IsAssignableFrom(underlyingPropertyType))
        {
            return ((IFormattable)propVal).ToString(type.Value.Format, CultureInfo.CurrentCulture);
        }

        return propVal?.ToString()?.Trim() ?? "";
    }
}