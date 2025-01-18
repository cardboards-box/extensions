using Jint.Native;
using Jint;

namespace CardboardBox.Extensions.Scripting;

/// <summary>
/// Executes a script and keeps track of it's context while executing
/// </summary>
public class ScriptEngine(
    Action<string, object[]>? _logger,
    string _script,
    Dictionary<string, object?> _parameters)
{
    private readonly StringBuilder _output = new();
    private readonly Engine _engine = new();

    /// <summary>
    /// The output result from the script execution
    /// </summary>
    public JsValue? ReturnResult { get; private set; }

    /// <summary>
    /// Logs the given message and args to the provided logger
    /// </summary>
    /// <param name="message">The message to write</param>
    /// <param name="args">The arguments to put into the message</param>
    private void Log(string message, params object[] args) => _logger?.Invoke(message, args);

    /// <summary>
    /// Sets a runtime variable within the engine and the contextual parameters
    /// </summary>
    /// <param name="key">The name of the variable to set</param>
    /// <param name="value">The value to set the variable to</param>
    private void Set(string key, object? value)
    {
        _parameters[key] = value;
        _engine.SetValue(key, value);
    }

    /// <summary>
    /// Writes the given content to the template
    /// </summary>
    /// <param name="content">The content to write</param>
    private void Write(string content) => _output.Append(content);

    /// <summary>
    /// Writes the given content to the template followed be a newline character
    /// </summary>
    /// <param name="content">The content to write</param>
    private void WriteLine(string content) => _output.AppendLine(content);

    /// <summary>
    /// Paginates the given data
    /// </summary>
    /// <param name="data">The data to paginate</param>
    /// <param name="page">The page of data to return</param>
    /// <param name="size">The size of each page</param>
    /// <returns>The paginated chunk of data</returns>
    private object[] Page(object[] data, int page, int size)
    {
        return data.Page(page, size).ToArray();
    }

    /// <summary>
    /// Removes all none alpha numeric characters
    /// </summary>
    /// <param name="name">The input text</param>
    /// <returns>The escaped value</returns>
    private string Escape(string name)
    {
        var regex = new Regex("[^a-z0-9A-Z_]");
        return regex.Replace(name.Replace(" ", ""), "");
    }

    /// <summary>
    /// Evaluates the script and returns the results of the script
    /// </summary>
    /// <returns>The results of the script</returns>
    public string Evaluate()
    {
        _engine
            .SetValue("write", Write)
            .SetValue("writeLine", WriteLine)
            .SetValue("log", Log)
            .SetValue("set", Set)
            .SetValue("page", Page)
            .SetValue("escape", Escape);

        foreach (var (k, v) in _parameters)
            _engine.SetValue(k, v);

        ReturnResult = _engine.Evaluate(_script);

        return _output.ToString();
    }
}
