using System.Xml;

namespace CardboardBox.Extensions;

/// <summary>
/// XML helper methods for creating elements
/// </summary>
public static class X
{
    /// <summary>
    /// Creates an XML element
    /// </summary>
    /// <param name="name">The name of the element</param>
    /// <param name="namespace">The namespace of the XML element</param>
    /// <param name="attributes">The attributes for the element</param>
    /// <param name="children">All of the children in the element</param>
    /// <returns>The created XML element</returns>
    public static XmlElement C(string name, string? @namespace, (string name, string? value)[] attributes, params XmlElement[] children)
    {
        var e = XmlExtensions.Element(name, @namespace);
        foreach (var (n, v) in attributes)
            e.SetAttr(n, v);
        foreach (var c in children)
            e.Child(c);
        return e;
    }

    /// <summary>
    /// Creates an XML element
    /// </summary>
    /// <param name="name">The name of the element</param>
    /// <param name="namespace">The namespace of the XML element</param>
    /// <param name="attributes">The attributes for the element</param>
    /// <param name="text">The value of the XML element</param>
    /// <returns>The created XML element</returns>
    public static XmlElement C(string name, string? @namespace, (string name, string? value)[] attributes, string text)
    {
        var e = XmlExtensions.Element(name, @namespace);
        foreach (var (n, v) in attributes)
            e.SetAttr(n, v);
        e.InnerText = text;
        return e;
    }

    /// <summary>
    /// Creates an XML element
    /// </summary>
    /// <param name="name">The name of the element</param>
    /// <param name="namespace">The namespace of the XML element</param>
    /// <param name="attributes">The attributes for the element</param>
    /// <returns>The created XML element</returns>
    public static XmlElement C(string name, string? @namespace, (string name, string? value)[] attributes)
    {
        var e = XmlExtensions.Element(name, @namespace);
        foreach (var (n, v) in attributes)
            e.SetAttr(n, v);
        return e;
    }
}