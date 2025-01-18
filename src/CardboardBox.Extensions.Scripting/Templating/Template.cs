namespace CardboardBox.Extensions.Scripting.Templating;

/// <summary>
/// Represents a template from the disk
/// </summary>
/// <param name="_path">The path to the template</param>
/// <param name="_name">The name of the template</param>
/// <param name="_requirements">Any requirements the template has</param>
public class Template(
    string _path,
    string _name,
    TemplateRequirement[] _requirements)
{
    /// <summary>
    /// The path to the template
    /// </summary>
    public string Path { get; } = _path;

    /// <summary>
    /// The name of the template
    /// </summary>
    public string Name { get; } = _name;

    /// <summary>
    /// Any requirements the template has
    /// </summary>
    public TemplateRequirement[] Requirements { get; } = _requirements;

    /// <summary>
    /// Represents a template from the disk
    /// </summary>
    /// <param name="path">The path to the template</param>
    /// <param name="name">The name of the template</param>
    /// <param name="requirements">Any requirements the template has</param>
    public void Deconstruct(out string path, out string name, out TemplateRequirement[] requirements)
    {
        path = Path;
        name = Name;
        requirements = Requirements;
    }
}
