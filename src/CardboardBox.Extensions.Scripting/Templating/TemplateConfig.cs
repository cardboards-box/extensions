namespace CardboardBox.Extensions.Scripting.Templating;

using Tokening;

/// <summary>
/// Represents the configuration for parsing templates
/// </summary>
public class TemplateConfig
{
    /// <summary>
    /// Represents the configuration used to parse out the code injection tokens
    /// </summary>
    public TokenParserConfig Inject { get; set; }

    /// <summary>
    /// Represents the configuration used to parse out the required input variable tokens
    /// </summary>
    public TokenParserConfig Require { get; set; }

    /// <summary>
    /// Represents the configuration for parsing templates
    /// </summary>
    /// <param name="inject">Represents the configuration used to parse out the code injection tokens</param>
    /// <param name="require">Represents the configuration used to parse out the required input variable tokens</param>
    public TemplateConfig(TokenParserConfig? inject = null, TokenParserConfig? require = null)
    {
        Inject = inject ?? new() { StartToken = "/*[INJECT]", EndToken = "*/", EscapeToken = "[ESCAPE]" };
        Require = require ?? new() { StartToken = "/*[REQUIRE]", EndToken = "*/", EscapeToken = "[ESCAPE]" };
    }
}
