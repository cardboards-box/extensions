namespace CardboardBox.Extensions.Utilities;

/// <summary>
/// Exception thrown when a unit parser fails
/// </summary>
/// <param name="message">The error that occurred</param>
public class UnitParserException(string message) : Exception(message)
{
	internal static UnitParserException RegexFailed() => new("Input string is not a valid unit (Failed validation)");

	internal static UnitParserException NoMatches() => new("Input string is not a valid unit (Failed validation - no matches)");

	internal static UnitParserException NoValue() => new("Input string is not a valid unit (No value)");

	internal static UnitParserException InvalidValue() => new("Input string is not a valid unit (Unit value was not a valid number)");
}
