using System.Security;
using System.Xml;
using System.Xml.Linq;

namespace CardboardBox.Extensions;

/// <summary>
/// Utility for managing XML elements.
/// </summary>
public static partial class XmlExtensions
{
    /// <summary>
    /// Creates a new XmlElement instance.
    /// </summary>
    /// <param name="name">The name of the element.</param>
    /// <param name="namespace">The namespace of the element.</param>
    /// <returns>An initialized instance of the XmlElement class.</returns>
    /// <exception cref="ArgumentNullException">The name parameter is null.</exception>
    /// <exception cref="ArgumentException">The name parameter is the empty string.</exception>
    /// <exception cref="XmlException">The name or the namespace parameter is invalid.</exception>
    public static XmlElement Element(string name, string? @namespace = null)
    {
        if (string.IsNullOrEmpty(name))
            throw new ArgumentNullException(nameof(name));
        return new XmlDocument().CreateElement(name, @namespace);
    }

    /// <summary>
    /// Adds the specified element to the end of the list of child nodes, of this node.
    /// </summary>
    /// <param name="e">The XmlElement instance the method is invoked for.</param>
    /// <param name="child">The node to add.</param>
    /// <returns>A reference to the XmlElement instance.</returns>
    public static XmlElement Child(this XmlElement e, XmlElement child)
    {
        XmlNode imported = e.OwnerDocument.ImportNode(child, true);
        e.AppendChild(imported);
        return e;
    }

    /// <summary>
    /// Sets the value of the attribute with the specified name.
    /// </summary>
    /// <param name="e">The XmlElement instance the method is invoked for.</param>
    /// <param name="name">The name of the attribute to create or alter.</param>
    /// <param name="value">The value to set for the attribute.</param>
    /// <returns>A reference to the XmlElement instance.</returns>
    public static XmlElement Attr(this XmlElement e, string name, string value)
    {
        e.SetAttribute(name, value);
        return e;
    }

    /// <summary>
    /// Adds the specified text to the end of the list of child nodes, of this node.
    /// </summary>
    /// <param name="e">The XmlElement instance the method is invoked for.</param>
    /// <param name="text">The text to add.</param>
    /// <returns>A reference to the XmlElement instance.</returns>
    public static XmlElement Text(this XmlElement e, string text)
    {
        e.AppendChild(e.OwnerDocument.CreateTextNode(text));
        return e;
    }

    /// <summary>
    /// Serializes the XmlElement instance into a string.
    /// </summary>
    /// <param name="e">The XmlElement instance the method is invoked for.</param>
    /// <param name="xmlDeclaration">true to include a XML declaration, otherwise false.</param>
    /// <param name="leaveOpen">true to leave the tag of an empty element open, otherwise false.</param>
    /// <returns>A textual representation of the XmlElement instance.</returns>
    public static string ToXMPPXmlString(this XmlElement e, bool xmlDeclaration = false, bool leaveOpen = false)
    {
        // Can't use e.OuterXml because it "messes up" namespaces for elements with
        // a prefix, i.e. stream:stream (What it does is probably correct, but just
        // not what we need for XMPP).
        var b = new StringBuilder("<" + e.Name);
        if (!string.IsNullOrEmpty(e.NamespaceURI))
            b.Append(" xmlns='" + e.NamespaceURI + "'");
        foreach (XmlAttribute a in e.Attributes)
        {
            if (a.Name == "xmlns")
                continue;
            if (a.Value != null)
                b.Append(" " + a.Name + "='" + SecurityElement.Escape(a.Value.ToString())
                    + "'");
        }
        if (e.IsEmpty)
            b.Append("/>");
        else
        {
            b.Append('>');
            foreach (var child in e.ChildNodes)
            {
                if (child is XmlElement element)
                    b.Append(element.ToXMPPXmlString());
                else if (child is XmlText text)
                    b.Append(text.InnerText);
            }
            b.Append("</" + e.Name + ">");
        }
        string xml = b.ToString();
        if (xmlDeclaration)
            xml = "<?xml version='1.0' encoding='UTF-8'?>" + xml;
        if (leaveOpen)
            return XMLClose().Replace(xml, ">");
        return xml;
    }

    /// <summary>
    /// Deserializes the XML into an XmlElement instance.
    /// </summary>
    /// <param name="xml">The XML data as a string</param>
    /// <returns>The XML element that was deserialized</returns>
    public static XmlElement? FromXmlString(this string? xml)
    {
        xml = xml.ForceNull();
        if (xml is null) return null;

        var doc = new XmlDocument();
        doc.LoadXml(xml);
        return doc.FirstChild as XmlElement;
    }

    /// <summary>
    /// Sets or removes the attribute with the specified name.
    /// </summary>
    /// <param name="element">The XmlElement instance the method is invoked for.</param>
    /// <param name="name">The name of the attribute to create or alter.</param>
    /// <param name="value">The value to set for the attribute.</param>
    /// <returns>A reference to the XmlElement instance.</returns>
    public static XmlElement SetAttr(this XmlElement element, string name, string? value)
    {
        value = value?.ForceNull();
        if (value is null)
            element.RemoveAttribute(name);
        else
            element.SetAttribute(name, value);
        return element;
    }

    /// <summary>
    /// Sets or removes the attribute with the specified name.
    /// </summary>
    /// <param name="element">The XmlElement instance the method is invoked for.</param>
    /// <param name="name">The name of the attribute to create or alter.</param>
    /// <param name="value">The value to set for the attribute.</param>
    /// <returns>A reference to the XmlElement instance.</returns>
    public static XmlElement SetAttr(this XmlElement element, string name, int? value)
    {
        return element.SetAttr(name, value?.ToString());
    }

    /// <summary>
    /// Sets or removes the attribute with the specified name.
    /// </summary>
    /// <param name="element">The XmlElement instance the method is invoked for.</param>
    /// <param name="name">The name of the attribute to create or alter.</param>
    /// <param name="value">The value to set for the attribute.</param>
    /// <returns>A reference to the XmlElement instance.</returns>
    public static XmlElement SetAttr(this XmlElement element, string name, DateTime? value)
    {
        return element.SetAttr(name, value?.ToString("O"));
    }

    /// <summary>
    /// Sets or removes the attribute with the specified name.
    /// </summary>
    /// <typeparam name="T">The type of enum to set</typeparam>
    /// <param name="element">The XmlElement instance the method is invoked for.</param>
    /// <param name="name">The name of the attribute to create or alter.</param>
    /// <param name="value">The value to set for the attribute.</param>
    /// <returns>A reference to the XmlElement instance.</returns>
    public static XmlElement SetAttr<T>(this XmlElement element, string name, T? value)
        where T : struct, Enum, IComparable, IFormattable, IConvertible
    {
        return element.SetAttr(name, value?.ToString());
    }

    /// <summary>
    /// Gets the attribute with the specified name.
    /// </summary>
    /// <param name="element">The XmlElement instance the method is invoked for.</param>
    /// <param name="name">The name of the attribute</param>
    /// <returns>The value of the attribute</returns>
    public static string? GetAttr(this XmlElement element, string name)
    {
        return element.GetAttribute(name)?.ForceNull();
    }

    /// <summary>
    /// Gets the attribute with the specified name.
    /// </summary>
    /// <param name="element">The XmlElement instance the method is invoked for.</param>
    /// <param name="name">The name of the attribute</param>
    /// <returns>The value of the attribute</returns>
    public static int? GetAttrInt(this XmlElement element, string name)
    {
        var value = element.GetAttr(name);
        return int.TryParse(value, out var result) ? result : null;
    }

    /// <summary>
    /// Gets the attribute with the specified name.
    /// </summary>
    /// <param name="element">The XmlElement instance the method is invoked for.</param>
    /// <param name="name">The name of the attribute</param>
    /// <returns>The value of the attribute</returns>
    public static DateTime? GetAttrDate(this XmlElement element, string name)
    {
        var value = element.GetAttr(name);
        return DateTime.TryParse(value, out var result) ? result : null;
    }

    /// <summary>
    /// Gets the attribute with the specified name.
    /// </summary>
    /// <typeparam name="T">The type of the enum to get</typeparam>
    /// <param name="element">The XmlElement instance the method is invoked for.</param>
    /// <param name="name">The name of the attribute</param>
    /// <returns>The value of the attribute</returns>
    public static T? GetAttr<T>(this XmlElement element, string name)
        where T : struct, Enum, IComparable, IFormattable, IConvertible
    {
        var value = element.GetAttr(name);
        return Enum.TryParse<T>(value, true, out var result) ? result : null;
    }

    /// <summary>
    /// Adds the given attribute to the target element
    /// </summary>
    /// <param name="el">The target element</param>
    /// <param name="name">The name of the attribute</param>
    /// <param name="value">The value of the attribute</param>
    /// <returns>The target element (for fluent chaining)</returns>
    public static XElement AddAttribute(this XElement el, XName name, object? value)
    {
        el.SetAttributeValue(name, value);
        return el;
    }

    /// <summary>
    /// Adds the given attributes to the target element
    /// </summary>
    /// <param name="el">The target element</param>
    /// <param name="attributes">The attributes to add to the element</param>
    /// <returns>The target element (for fluent chaining)</returns>
    public static XElement AddAttributes(this XElement el, params (XName Attribute, object? Value)[] attributes)
    {
        foreach (var (atr, val) in attributes)
            el.AddAttribute(atr, val);
        return el;
    }

    /// <summary>
    /// Adds the given elements as children to the target element
    /// </summary>
    /// <param name="parent">The target element</param>
    /// <param name="children">The children to add to the element</param>
    /// <returns>The target element (for fluent chaining)</returns>
    public static XElement AddElements(this XElement parent, params object?[] children)
    {
        parent.Add(children);
        return parent;
    }

    /// <summary>
    /// Adds the element as a child to the target element
    /// </summary>
    /// <param name="parent">The target element</param>
    /// <param name="tag">The child elements tag</param>
    /// <param name="value">The child elements value</param>
    /// <param name="attributes">The child elements attributes</param>
    /// <returns>The target element (for fluent chaining)</returns>
    public static XElement AddElement(this XElement parent, XName tag, object? value, params (XName Attribute, object? Value)[] attributes)
    {
        var el = new XElement(tag)
            .AddAttributes(attributes);

        if (value != null)
            el.AddElements(value);

        parent.AddElements(el);
        return parent;
    }

    /// <summary>
    /// Adds the element as a child to the target element, if the value is null, it skips adding the element all together
    /// </summary>
    /// <param name="parent">The target element</param>
    /// <param name="tag">The child elements tag</param>
    /// <param name="value">The child elements value</param>
    /// <param name="attributes">The child elements attributes</param>
    /// <returns>The target element (for fluent chaining)</returns>
    public static XElement AddOptElement(this XElement parent, XName tag, object? value, params (XName Attribute, object? Value)[] attributes)
    {
        if (value == null) return parent;

        return parent.AddElement(tag, value, attributes);
    }


#if NET7_0_OR_GREATER
    [GeneratedRegex("/>$")]
    private static partial Regex XMLClose();
#else
    private static Regex? _xmlClose;
    private static Regex XMLClose() => _xmlClose ??= new("/>$");
#endif
}
