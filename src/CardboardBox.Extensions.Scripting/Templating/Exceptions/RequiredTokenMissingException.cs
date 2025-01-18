namespace CardboardBox.Extensions.Scripting.Templating.Exceptions;

/// <summary>
/// Thrown when a required variable is missing from a scripts execution context
/// </summary>
/// <param name="requirement">The required variable that triggered this exception</param>
public class RequiredTokenMissingException(
    TemplateRequirement requirement) 
    : Exception($"Required template parameter is missing: {requirement.Name} - {requirement.Description}")
{
    /// <summary>
    /// The required variable that triggered this exception
    /// </summary>
    public TemplateRequirement Requirement { get; set; } = requirement;
}
