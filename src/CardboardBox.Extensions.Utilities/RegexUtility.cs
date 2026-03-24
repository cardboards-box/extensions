using System.Text.RegularExpressions;

namespace CardboardBox.Extensions.Utilities;

/// <summary>
/// A utility class for working with regular expressions
/// </summary>
public static partial class RegexUtility
{
	/// <summary>
	/// Strips all non-alphanumeric characters from a string (excluding percentages, periods, and hyphens)
	/// </summary>
	/// <param name="input">The string to strip</param>
	/// <returns>The stripped string</returns>
	public static string UnitFilter(string input)
	{
		return UnitFilterRegex().Replace(input, "");
	}

	/// <summary>
	/// Gets the value and unit of a unit
	/// </summary>
	/// <param name="input">The given unit</param>
	/// <returns>The parsed value and unit</returns>
	public static (double value, string unit) UnitParse(string input)
	{
		var regex = UnitParserRegex();
		var match = regex.Match(input);
		if (!match.Success) throw UnitParserException.RegexFailed();
		if (match.Groups.Count <= 1) throw UnitParserException.NoMatches();

		var strValue = match.Groups[1].Value;
		if (string.IsNullOrEmpty(strValue))
			throw UnitParserException.NoValue();

		if (!double.TryParse(strValue, out var value)) throw UnitParserException.InvalidValue();

		if (match.Groups.Count == 1) return (value, string.Empty);

		var unit = match.Groups[2].Value;
		return (value, unit);
	}

	// language=regex
	private const string UnitFilterPattern = @"[^a-zA-Z0-9\.%-]";
	// language=regex
	private const string UnitParserPattern = @"(-?[0-9]{0,}\.?[0-9]{0,}?)([a-z%]{1,})?$";

#if NETSTANDARD2_1
	private static Regex UnitFilterRegex() => new(UnitFilterPattern, RegexOptions.Compiled);

	private static Regex UnitParserRegex() => new(UnitParserPattern, RegexOptions.Compiled);
#else
	[GeneratedRegex(UnitFilterPattern, RegexOptions.Compiled)]
	private static partial Regex UnitFilterRegex();

	[GeneratedRegex(UnitParserPattern, RegexOptions.Compiled)]
	private static partial Regex UnitParserRegex();
#endif
}
