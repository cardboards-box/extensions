namespace CardboardBox.Extensions.Scripting.Templating;

using Exceptions;
using Tokening;

/// <summary>
/// A service for parsing and evaluating templates
/// </summary>
public interface ITemplateService
{
    /// <summary>
    /// Fetches all of the files in the given template and parses the requirements 
    /// </summary>
    /// <param name="directory">The directory to scan</param>
    /// <param name="subFolders">Whether or not to check sub-folders within the directory</param>
    /// <param name="config">The template configuration for parsing the requirements</param>
    /// <returns>All of the templates and their associated requirements</returns>
    IAsyncEnumerable<Template> GetTemplates(string directory, bool subFolders = true, TemplateConfig? config = null);

    /// <summary>
    /// Gets all of the requirements from the given template using the given config
    /// </summary>
    /// <param name="template">The template to get the requirements from</param>
    /// <param name="config">The configuration to use to parse the requirements</param>
    /// <returns>A collection of all of the requirements from the template</returns>
    IEnumerable<TemplateRequirement> GetRequirements(string template, TemplateConfig? config = null);

    /// <summary>
    /// Gets all of the requirements from the given template using the given config
    /// </summary>
    /// <param name="template">The template to get the requirements from</param>
    /// <param name="config">The configuration to use to parse the requirements</param>
    /// <returns>A collection of all of the requirements from the template</returns>
    IEnumerable<TemplateRequirement> GetRequirements(string template, TokenParserConfig config);

    /// <summary>
    /// Evaluates the given template
    /// </summary>
    /// <param name="template">The template to evaluate</param>
    /// <param name="parameters">The input variables to use for the template [WARNING: Can have side effects]</param>
    /// <param name="config">The configuration to use to parse the template</param>
    /// <param name="logger">The logging method</param>
    /// <returns>The output of the template evaluation</returns>
    /// <exception cref="RequiredTokenMissingException">Thrown if one of the required input parameters is not specified</exception>
    string Evaluate(string template, Dictionary<string, object?> parameters, TemplateConfig? config = null, Action<string, object[]>? logger = null);

    /// <summary>
    /// Determines a safe file path to save the given file to. 
    /// This will suffix a number to then end when the path already exists (think Windows "Copy 2" mechanic)
    /// </summary>
    /// <param name="path">The path to write to</param>
    /// <returns>A safe path to save the file to</returns>
    string SafeFileName(string path);
}

internal class TemplateService(
    ITokenService _token) : ITemplateService
{

    /// <summary>
    /// Fetches all of the files in the given template and parses the requirements 
    /// </summary>
    /// <param name="directory">The directory to scan</param>
    /// <param name="subFolders">Whether or not to check sub-folders within the directory</param>
    /// <param name="config">The template configuration for parsing the requirements</param>
    /// <returns>All of the templates and their associated requirements</returns>
    public async IAsyncEnumerable<Template> GetTemplates(string directory, bool subFolders = true, TemplateConfig? config = null)
    {
        if (!Directory.Exists(directory)) throw new DirectoryNotFoundException("Could not find directory: " + directory);

        var files = Directory.GetFiles(directory, "*",
            subFolders ? SearchOption.AllDirectories : SearchOption.TopDirectoryOnly);

        foreach (var file in files)
        {
            var contents = await File.ReadAllTextAsync(file);
            if (string.IsNullOrWhiteSpace(contents)) continue;

            var requirements = GetRequirements(contents, config).ToArray();
            var name = file.Remove(0, directory.Length).TrimStart('\\', '/');
            yield return new Template(file, name, requirements);
        }
    }

    /// <summary>
    /// Gets all of the requirements from the given template using the given config
    /// </summary>
    /// <param name="template">The template to get the requirements from</param>
    /// <param name="config">The configuration to use to parse the requirements</param>
    /// <returns>A collection of all of the requirements from the template</returns>
    public IEnumerable<TemplateRequirement> GetRequirements(string template, TemplateConfig? config = null)
    {
        config ??= new TemplateConfig();
        return GetRequirements(template, config.Require);
    }

    /// <summary>
    /// Gets all of the requirements from the given template using the given config
    /// </summary>
    /// <param name="template">The template to get the requirements from</param>
    /// <param name="config">The configuration to use to parse the requirements</param>
    /// <returns>A collection of all of the requirements from the template</returns>
    public IEnumerable<TemplateRequirement> GetRequirements(string template, TokenParserConfig config)
    {
        var requires = _token.ParseTokens(template, config);

        foreach (var token in requires)
        {
            var lines = token.Content.Split(new[] { '\r', '\n' }, StringSplitOptions.RemoveEmptyEntries);
            if (lines.Length == 0) continue;

            foreach (var line in lines)
            {
                var ast = line.Split(new[] { ' ' }, StringSplitOptions.RemoveEmptyEntries);
                if (ast.Length < 3) throw new RequiredTokenInvalidException(line, "Not enough parameters, please specify the data-type, name, and the description");

                var (type, par) = DetermineType(ast[0]);
                var name = ast[1];

                if (ast[2] != "-") throw new RequiredTokenInvalidException(line, "Description is not present");

                var description = string.Join(" ", ast.Skip(3));

                yield return new(type, par, name, description, token);
            }
        }
    }

    /// <summary>
    /// Takes the template requirement type and determines the raw type and it's parameters
    /// </summary>
    /// <param name="type">The template requirement type</param>
    /// <returns>The template requirement data type and the optional parameter</returns>
    public static (RequirementType type, string? parameter) DetermineType(string type)
    {
        string? par = null;
        if (type.Contains(':'))
        {
            var ast = type.Split([ ":" ], StringSplitOptions.RemoveEmptyEntries);
            type = ast.First().Trim();
            par = string.Join(':', ast.Skip(1)).Trim();
        }

        return type.ToLower() switch
        {
            "text" => (RequirementType.Text, par),
            "int" => (RequirementType.Integer, par),
            "float" => (RequirementType.Decimal, par),
            "number" => (RequirementType.Decimal, par),
            "file" => (RequirementType.File, par),
            "bit" => (RequirementType.Boolean, par),
            "boolean" => (RequirementType.Boolean, par),
            "bool" => (RequirementType.Boolean, par),
            "date" => (RequirementType.Date, par),
            "time" => (RequirementType.Time, par),
            "datetime" => (RequirementType.DateTime, par),
            _ => (RequirementType.Unknown, $"{type}:{par}"),
        };
    }

    /// <summary>
    /// Evaluates the given template
    /// </summary>
    /// <param name="template">The template to evaluate</param>
    /// <param name="parameters">The input variables to use for the template [WARNING: Can have side effects]</param>
    /// <param name="config">The configuration to use to parse the template</param>
    /// <param name="logger">The logging method</param>
    /// <returns>The output of the template evaluation</returns>
    /// <exception cref="RequiredTokenMissingException">Thrown if one of the required input parameters is not specified</exception>
    public string Evaluate(string template, Dictionary<string, object?> parameters, TemplateConfig? config = null, Action<string, object[]>? logger = null)
    {
        config ??= new TemplateConfig();
        var replacements = new Dictionary<string, string>();
        var requirements = GetRequirements(template, config);
        foreach (var requirement in requirements)
        {
            if (!parameters.ContainsKey(requirement.Name))
                throw new RequiredTokenMissingException(requirement);

            replacements[requirement.Source.FullToken] = string.Empty;
        }

        var injects = _token.ParseTokens(template, config.Inject);
        foreach (var token in injects)
        {
            if (replacements.ContainsKey(token.FullToken)) continue;

            var engine = new ScriptEngine(logger, token.Content, parameters);
            var result = engine.Evaluate();
            replacements.Add(token.FullToken, result);
        }

        foreach (var (key, val) in replacements)
            template = template.Replace(key, val);

        return template;
    }

    /// <summary>
    /// Determines a safe file path to save the given file to. 
    /// This will suffix a number to then end when the path already exists (think Windows "Copy 2" mechanic)
    /// </summary>
    /// <param name="path">The path to write to</param>
    /// <returns>A safe path to save the file to</returns>
    public string SafeFileName(string path)
    {
        if (!File.Exists(path)) return path;

        int count = 1;
        var dir = Path.GetDirectoryName(path) ?? string.Empty;
        var filename = Path.GetFileNameWithoutExtension(path);
        var extension = Path.GetExtension(path).TrimStart('.');

        string suffix(int count) => Path.Combine(dir, $"{filename}-{count}.{extension}");
        var output = suffix(count);
        while (File.Exists(output)) output = suffix(count++);

        return output;
    }
}
