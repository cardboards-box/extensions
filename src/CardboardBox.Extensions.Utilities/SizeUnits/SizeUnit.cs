using System.Diagnostics.CodeAnalysis;
using System.Text.Json.Serialization;

namespace CardboardBox.Extensions.Utilities.SizeUnits;

using static PixelConversions;

/// <summary>
/// Unit of measurement (ex: 100vw, 50vh, 10px, 40%, 1cm, 2in, 2pc, 43pt, 1em, 1mm, 1q, 3rp)
/// </summary>
/// <param name="Type">The type of unit of measurement</param>
/// <param name="Value">The value of the measurement</param>
[JsonConverter(typeof(SizeUnitSerializer))]
public record struct SizeUnit(SizeUnitType Type, double Value)
#if NET10_0_OR_GREATER
	: IParsable<SizeUnit>
#endif
{
	private static SizeUnitMap[]? _units;
	private const string ERROR_PERCENT = "Cannot use percentage measurement as the context of the size is not known";

	/// <summary>
	/// A size of zero
	/// </summary>
	public static SizeUnit Zero { get; } = new(SizeUnitType.Pixel, 0);

	/// <summary>
	/// 100% of the parent size
	/// </summary>
	public static SizeUnit Fill { get; } = new(SizeUnitType.Percentage, 100);

	/// <summary>
	/// 100% of the parent width
	/// </summary>
	public static SizeUnit FillWidth { get; } = new(SizeUnitType.ViewWidth, 100);

	/// <summary>
	/// 100% of the parent height
	/// </summary>
	public static SizeUnit FillHeight { get; } = new(SizeUnitType.ViewHeight, 100);

	/// <summary>
	/// Gets the equivalent number of pixels for this unit of measurement
	/// </summary>
	/// <param name="context">The context of the parent</param>
	/// <param name="isWidth">
	/// Whether or not the unit is for widths/x axis (<see langword="true"/>),
	/// heights/y axis (<see langword="false"/>), 
	/// or <see langword="null"/> if the context isn't known</param>
	/// <returns>The number of pixels</returns>
	public readonly int Pixels(SizeContext? context = null, bool? isWidth = null) => GetPixels(this, context, isWidth);

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
	public static implicit operator string(SizeUnit size) => size.Serialize();

	/// <summary>
	/// Converts the string to a unit of measurement
	/// </summary>
	/// <param name="input">The string version of the measurement</param>
	public static implicit operator SizeUnit(string input) => Parse(input);

	#region Helper Methods
	/// <summary>
	/// Converts the string to a unit of measurement
	/// </summary>
	/// <param name="input">The string version of the measurement</param>
	/// <returns>The parsed size unit</returns>
	public static SizeUnit Parse(string input) => Parse(input, null);

	/// <summary>
	/// Converts the string to a unit of measurement
	/// </summary>
	/// <param name="input">The string version of the measurement</param>
	/// <param name="provider">The format provider (unused)</param>
	/// <returns>The parsed size unit</returns>
	public static SizeUnit Parse(string input, IFormatProvider? provider)
	{
		return TryParse(input, provider, out var result)
			? result : Zero;
	}

	/// <summary>
	/// Attempts to parse the given string into a <see cref="SizeUnit"/>
	/// </summary>
	/// <param name="input">The string version of the measurement</param>
	/// <param name="result">The result of the parse operation</param>
	/// <returns>Whether or not the operation was a success</returns>
	public static bool TryParse([NotNullWhen(true)] string? input, [MaybeNullWhen(false)] out SizeUnit result)
	{
		return TryParse(input, null, out result);
	}

	/// <summary>
	/// Attempts to parse the given string into a <see cref="SizeUnit"/>
	/// </summary>
	/// <param name="input">The string version of the measurement</param>
	/// <param name="provider">The format provider (unused)</param>
	/// <param name="result">The result of the parse operation</param>
	/// <returns>Whether or not the operation was a success</returns>
	public static bool TryParse([NotNullWhen(true)] string? input, IFormatProvider? provider, [MaybeNullWhen(false)] out SizeUnit result)
	{
		result = Zero;

		if (string.IsNullOrWhiteSpace(input)) return false;

		input = input.Trim().ToLower();

		if (input == "0") return true;

		input = RegexUtility.UnitFilter(input);
		if (string.IsNullOrWhiteSpace(input)) return false;

		if (double.TryParse(input, out var single))
		{
			result = new SizeUnit(SizeUnitType.Pixel, single);
			return true;
		}

		try
		{
			var (value, unit) = RegexUtility.UnitParse(input);
			if (string.IsNullOrWhiteSpace(unit))
			{
				result = new SizeUnit(SizeUnitType.Pixel, value);
				return true;
			}

			var unitMatch = Units().FirstOrDefault(u => u.Symbol == unit);
			if (unitMatch is null) return false;

			result = new SizeUnit(unitMatch.Type, value);
			return true;
		}
		catch (UnitParserException)
		{
			return false;
		}
	}

	/// <summary>
	/// Serializes the given <see cref="SizeUnit"/> to a string
	/// </summary>
	/// <param name="size">The unit of measurement</param>
	/// <returns>The string representation of the size</returns>
	public static string SerializeUnit(SizeUnit size)
	{
		var unit = Units().FirstOrDefault(u => u.Type == size.Type);
		if (unit is null) return $"{size.Value}px";

		return $"{size.Value}{unit.Symbol}";
	}

	/// <summary>
	/// Converts the <see cref="SizeUnit"/> into the pixel equivalent
	/// </summary>
	/// <param name="size">The unit to convert</param>
	/// <param name="context">The context of the size request</param>
	/// <param name="isWidth">Whether or not the unit is for widths or heights (or null if the context isn't known)</param>
	/// <returns>The pixel equivalent of the given size</returns>
	/// <exception cref="NullReferenceException">Thrown if the context is missing a required value</exception>
	/// <exception cref="ArgumentException">Thrown if the size is a percentage and the context is not known</exception>
	public static int GetPixels(SizeUnit size, SizeContext? context, bool? isWidth)
	{
		if (size.Value == 0) return 0;

		SizeContext NotNullContext() => context ?? throw new NullReferenceException(ERROR_PERCENT);

		var value = size.Type switch
		{
			SizeUnitType.Pixel => size.Value,
			SizeUnitType.Centimeter => CentimeterToPixel(size.Value),
			SizeUnitType.Millimeter => MillimeterToPixel(size.Value),
			SizeUnitType.QuarterMillimeter => QuarterMillimeterToPixel(size.Value),
			SizeUnitType.Inch => InchToPixel(size.Value),
			SizeUnitType.Pica => PicaToPixel(size.Value),
			SizeUnitType.Point => PointToPixel(size.Value),
			SizeUnitType.RelativePercentage => RelativePercentageToPixel(size.Value, NotNullContext().Width, NotNullContext().Height),
			SizeUnitType.Em => EmToPixel(size.Value, NotNullContext().FontSize),
			SizeUnitType.ViewHeight => PercentToPixel(size.Value, null, NotNullContext().Root.Height, false),
			SizeUnitType.ViewWidth => PercentToPixel(size.Value, NotNullContext().Root.Width, null, true),
			SizeUnitType.Percentage => PercentToPixel(size.Value, NotNullContext().Width, NotNullContext().Height, isWidth),
			_ => 0,
		};

		return (int)Math.Round(value);
	}

	/// <summary>
	/// All of the available units of measurement
	/// </summary>
	/// <returns></returns>
	public static SizeUnitMap[] Units()
	{
		return _units ??=
		[
			new SizeUnitMap(SizeUnitType.Centimeter, "cm"),
			new SizeUnitMap(SizeUnitType.Millimeter, "mm"),
			new SizeUnitMap(SizeUnitType.QuarterMillimeter, "q"),
			new SizeUnitMap(SizeUnitType.Inch, "in"),
			new SizeUnitMap(SizeUnitType.Pica, "pc"),
			new SizeUnitMap(SizeUnitType.Point, "pt"),
			new SizeUnitMap(SizeUnitType.Pixel, "px"),
			new SizeUnitMap(SizeUnitType.Percentage, "%"),
			new SizeUnitMap(SizeUnitType.Em, "em"),
			new SizeUnitMap(SizeUnitType.ViewHeight, "vh"),
			new SizeUnitMap(SizeUnitType.ViewWidth, "vw"),
			new SizeUnitMap(SizeUnitType.RelativePercentage, "rp")
		];
	}
	#endregion

	/// <summary>
	/// Represents an available unit of measurement
	/// </summary>
	/// <param name="Type">The type of unit</param>
	/// <param name="Symbol">The symbol that indicates the unit</param>
	public record class SizeUnitMap(
		SizeUnitType Type,
		string Symbol);
}
