using System.Diagnostics.CodeAnalysis;
using System.Text.Json.Serialization;

namespace CardboardBox.Extensions.Utilities.TimeUnits;

/// <summary>
/// Unit of time. (ex: 20ms, 3.2s, 1m, 2h)
/// </summary>
/// <param name="Type">The type of unit of time</param>
/// <param name="Value">The value of the time unit</param>
[JsonConverter(typeof(TimeUnitSerializer))]
public record struct TimeUnit(TimeUnitType Type, double Value)
#if NET10_0_OR_GREATER
	: IParsable<TimeUnit>
#endif
{
	private static TimeUnitMap[]? _units;

	/// <summary>
	/// A time of zero
	/// </summary>
	public static TimeUnit Zero { get; } = new(TimeUnitType.Millisecond, 0);

	/// <summary>
	/// The number of milliseconds in this time unit
	/// </summary>
	public readonly double Milliseconds => GetMilliseconds(this);

	/// <summary>
	/// The equivalent <see cref="System.TimeSpan"/> of this time unit
	/// </summary>
	public readonly TimeSpan TimeSpan => TimeSpan.FromMilliseconds(Milliseconds);

	/// <summary>
	/// Converts the unit of measurement to a string
	/// </summary>
	/// <returns>The string version of the measurement</returns>
	public readonly string Serialize() => SerializeUnit(this);

	/// <summary>
	/// Converts the unit of measurement to a string
	/// </summary>
	/// <returns>The serialized unit of measurement</returns>
	public override readonly string ToString() => Serialize();

	/// <summary>
	/// Converts the unit of measurement to a string
	/// </summary>
	/// <param name="size">The unit of measurement</param>
	public static implicit operator string(TimeUnit size) => size.Serialize();

	/// <summary>
	/// Converts the string to a unit of measurement
	/// </summary>
	/// <param name="input">The string version of the measurement</param>
	public static implicit operator TimeUnit(string input) => Parse(input);

	#region Helper Methods

	/// <summary>
	/// Converts the string to a unit of measurement
	/// </summary>
	/// <param name="input">The string version of the measurement</param>
	/// <param name="_"></param>
	/// <returns>The parsed time unit</returns>
	public static TimeUnit Parse(string input, IFormatProvider? _)
	{
		if (string.IsNullOrWhiteSpace(input)) throw new ArgumentNullException(nameof(input));
		return TryParse(input, out var unit) ? unit : Zero;
	}

	/// <summary>
	/// Converts the string to a unit of measurement
	/// </summary>
	/// <param name="input">The string version of the measurement</param>
	/// <returns>The parsed time unit</returns>
	public static TimeUnit Parse(string input) => Parse(input, null);

	/// <summary>
	/// Attempts to convert the string to a unit of measurement
	/// </summary>
	/// <param name="input">The string version of the measurement</param>
	/// <param name="_">The format provider - This is unused</param>
	/// <param name="unit">The result of the parse operation</param>
	/// <returns>Whether or not the unit was parsed correctly</returns>
	public static bool TryParse([NotNullWhen(true)] string? input, IFormatProvider? _, [MaybeNullWhen(false)] out TimeUnit unit)
	{
		unit = Zero;
		if (string.IsNullOrWhiteSpace(input)) return false;

		input = input.Trim().ToLower();

		if (input == "0") return false;

		input = RegexUtility.UnitFilter(input);
		if (string.IsNullOrWhiteSpace(input)) return false;

		if (double.TryParse(input, out var single))
		{
			unit = new TimeUnit(TimeUnitType.Millisecond, single);
			return true;
		}

		try
		{
			var (value, result) = RegexUtility.UnitParse(input);
			if (string.IsNullOrWhiteSpace(result))
			{
				unit = new TimeUnit(TimeUnitType.Millisecond, value);
				return true;
			}

			var unitMatch = Units().FirstOrDefault(u => u.Symbol == result);
			if (unitMatch is null) return false;

			unit = new TimeUnit(unitMatch.Type, value);
			return true;
		}
		catch (UnitParserException)
		{
			return false;
		}
	}

	/// <summary>
	/// Attempts to convert the string to a unit of measurement
	/// </summary>
	/// <param name="input">The string version of the measurement</param>
	/// <param name="result">The result of the parse operation</param>
	/// <returns>Whether or not the unit was parsed correctly</returns>
	public static bool TryParse([NotNullWhen(true)] string? input, [MaybeNullWhen(false)] out TimeUnit result)
	{
		return TryParse(input, null, out result);
	}

	/// <summary>
	/// All of the available time units
	/// </summary>
	public static TimeUnitMap[] Units()
	{
		return _units ??=
		[
			new TimeUnitMap(TimeUnitType.Millisecond, "ms"),
			new TimeUnitMap(TimeUnitType.Millisecond, "milli"),
			new TimeUnitMap(TimeUnitType.Millisecond, "millis"),
			new TimeUnitMap(TimeUnitType.Millisecond, "millisecond"),
			new TimeUnitMap(TimeUnitType.Millisecond, "milliseconds"),
			new TimeUnitMap(TimeUnitType.Second, "s"),
			new TimeUnitMap(TimeUnitType.Second, "sec"),
			new TimeUnitMap(TimeUnitType.Second, "secs"),
			new TimeUnitMap(TimeUnitType.Second, "second"),
			new TimeUnitMap(TimeUnitType.Second, "seconds"),
			new TimeUnitMap(TimeUnitType.Minute, "m"),
			new TimeUnitMap(TimeUnitType.Minute, "min"),
			new TimeUnitMap(TimeUnitType.Minute, "mins"),
			new TimeUnitMap(TimeUnitType.Minute, "minute"),
			new TimeUnitMap(TimeUnitType.Minute, "minutes"),
			new TimeUnitMap(TimeUnitType.Hour, "h"),
			new TimeUnitMap(TimeUnitType.Hour, "hr"),
			new TimeUnitMap(TimeUnitType.Hour, "hour"),
			new TimeUnitMap(TimeUnitType.Hour, "hours"),
			new TimeUnitMap(TimeUnitType.Day, "d"),
			new TimeUnitMap(TimeUnitType.Day, "day"),
			new TimeUnitMap(TimeUnitType.Day, "days"),
			new TimeUnitMap(TimeUnitType.Week, "w"),
			new TimeUnitMap(TimeUnitType.Week, "week"),
			new TimeUnitMap(TimeUnitType.Week, "weeks"),
			new TimeUnitMap(TimeUnitType.Month, "mo"),
			new TimeUnitMap(TimeUnitType.Month, "mon"),
			new TimeUnitMap(TimeUnitType.Month, "mons"),
			new TimeUnitMap(TimeUnitType.Month, "month"),
			new TimeUnitMap(TimeUnitType.Month, "months"),
			new TimeUnitMap(TimeUnitType.Quarter, "q"),
			new TimeUnitMap(TimeUnitType.Quarter, "qu"),
			new TimeUnitMap(TimeUnitType.Quarter, "quart"),
			new TimeUnitMap(TimeUnitType.Quarter, "quarter"),
			new TimeUnitMap(TimeUnitType.Quarter, "quarters"),
			new TimeUnitMap(TimeUnitType.Year, "y"),
			new TimeUnitMap(TimeUnitType.Year, "yr"),
			new TimeUnitMap(TimeUnitType.Year, "year"),
			new TimeUnitMap(TimeUnitType.Year, "years"),
			new TimeUnitMap(TimeUnitType.Decade, "de"),
			new TimeUnitMap(TimeUnitType.Decade, "dec"),
			new TimeUnitMap(TimeUnitType.Decade, "decade"),
			new TimeUnitMap(TimeUnitType.Decade, "decades"),
			new TimeUnitMap(TimeUnitType.Century, "c"),
			new TimeUnitMap(TimeUnitType.Century, "cent"),
			new TimeUnitMap(TimeUnitType.Century, "cents"),
			new TimeUnitMap(TimeUnitType.Century, "century"),
			new TimeUnitMap(TimeUnitType.Century, "centuries"),
			new TimeUnitMap(TimeUnitType.Millennium, "mi"),
			new TimeUnitMap(TimeUnitType.Millennium, "mill"),
			new TimeUnitMap(TimeUnitType.Millennium, "millennia"),
			new TimeUnitMap(TimeUnitType.Millennium, "millennium"),
			new TimeUnitMap(TimeUnitType.Millennium, "millenniums")
		];
	}

	/// <summary>
	/// Serializes a unit into a string
	/// </summary>
	/// <param name="unit">The unit of time</param>
	/// <returns>The string representation of the time unit</returns>
	public static string SerializeUnit(TimeUnit unit)
	{
		if (unit == Zero) return "0";

		var unitMatch = Units().FirstOrDefault(u => u.Type == unit.Type);
		if (unitMatch is null) return $"{unit.Value}ms";

		return $"{unit.Value}{unitMatch.Symbol}";
	}

	/// <summary>
	/// Converts the <see cref="TimeUnit"/> into the milliseconds equivalent
	/// </summary>
	/// <param name="unit">The unit to convert</param>
	/// <returns>The milliseconds equivalent of the given time</returns>
	public static double GetMilliseconds(TimeUnit unit)
	{
		if (unit.Value == 0) return 0;

		return unit.Type switch
		{
			TimeUnitType.Millisecond => unit.Value,
			TimeUnitType.Second => unit.Value * 1000,
			TimeUnitType.Minute => unit.Value * 60 * 1000,
			TimeUnitType.Hour => unit.Value * 60 * 60 * 1000,
			TimeUnitType.Day => unit.Value * 24 * 60 * 60 * 1000,
			TimeUnitType.Week => unit.Value * 7 * 24 * 60 * 60 * 1000,
			TimeUnitType.Month => unit.Value * 30 * 24 * 60 * 60 * 1000,
			TimeUnitType.Quarter => unit.Value * 4 * 30 * 24 * 60 * 60 * 1000,
			TimeUnitType.Year => unit.Value * 365 * 24 * 60 * 60 * 1000,
			TimeUnitType.Decade => unit.Value * 10 * 365 * 24 * 60 * 60 * 1000,
			TimeUnitType.Century => unit.Value * 100 * 365 * 24 * 60 * 60 * 1000,
			TimeUnitType.Millennium => unit.Value * 1000 * 365 * 24 * 60 * 60 * 1000,
			_ => 0
		};
	}

	#endregion

	/// <summary>
	/// Represents an available unit of time
	/// </summary>
	/// <param name="Type"></param>
	/// <param name="Symbol"></param>
	public record class TimeUnitMap(
		TimeUnitType Type,
		string Symbol);
}

