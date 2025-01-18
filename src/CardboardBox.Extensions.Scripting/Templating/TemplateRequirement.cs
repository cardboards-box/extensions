namespace CardboardBox.Extensions.Scripting.Templating;

using Tokening;

/// <summary>
/// Represents a required input variable for a template
/// </summary>
/// <param name="_type">The data type of the input variable</param>
/// <param name="_parameter">Any optional parameters the type has</param>
/// <param name="_name">The name of the input variable</param>
/// <param name="_description">A description of what the input variable is used for</param>
/// <param name="_source">The token this requirement was found within</param>
public class TemplateRequirement(
    RequirementType _type,
    string? _parameter,
    string _name,
    string _description,
    Token _source)
{
    /// <summary>
    /// The data type of the input variable
    /// </summary>
    public RequirementType Type { get; } = _type;

    /// <summary>
    /// Any optional parameters the type has
    /// </summary>
    public string? Parameter { get; } = _parameter;

    /// <summary>
    /// The name of the input variable
    /// </summary>
    public string Name { get; } = _name;

    /// <summary>
    /// A description of what the input variable is used for
    /// </summary>
    public string Description { get; } = _description;

    /// <summary>
    /// The token this requirement was found within
    /// </summary>
    public Token Source { get; } = _source;

    /// <summary>
    /// Represents a required input variable for a template
    /// </summary>
    /// <param name="type">The data type of the input variable</param>
    /// <param name="parameter">Any optional parameters the type has</param>
    /// <param name="name">The name of the input variable</param>
    /// <param name="description">A description of what the input variable is used for</param>
    /// <param name="source">The token this requirement was found within</param>
    public void Deconstruct(out RequirementType type, out string? parameter, out string name, out string description, out Token source)
    {
        type = Type;
        parameter = Parameter;
        name = Name;
        description = Description;
        source = Source;
    }

}
