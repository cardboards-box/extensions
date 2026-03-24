namespace CardboardBox.Extensions.Utilities.SizeUnits;

/// <summary>
/// A class for converting pixel values between different units
/// </summary>
public static class PixelConversions
{
	private const string ERROR_PERCENT = "Cannot use percentage measurement as the context of the size is not known";

	#region From Pixels
	/// <summary>
	/// Converts the given pixels to centimeters
	/// </summary>
	/// <param name="value">The number of pixels</param>
	/// <returns>The equivalent number of centimeters</returns>
	public static double PixelToCentimeter(double value) => value / 37.795275590551181102362204724409;

	/// <summary>
	/// Converts the given pixels to millimeters
	/// </summary>
	/// <param name="value">The number of pixels</param>
	/// <returns>The equivalent number of millimeters</returns>
	public static double PixelToMillimeter(double value) => PixelToCentimeter(value) * 10;

	/// <summary>
	/// Converts the given pixels to quarter millimeters
	/// </summary>
	/// <param name="value">The number of pixels</param>
	/// <returns>The equivalent number of quarter millimeters</returns>
	public static double PixelToQuarterMillimeter(double value) => PixelToMillimeter(value) * 4;

	/// <summary>
	/// Converts the given pixels to inches
	/// </summary>
	/// <param name="value">The number of pixels</param>
	/// <returns>The equivalent number of inches</returns>
	public static double PixelToInch(double value) => value / 96;

	/// <summary>
	/// Converts the given pixels to picas
	/// </summary>
	/// <param name="value">The number of pixels</param>
	/// <returns>The equivalent number of picas</returns>
	public static double PixelToPica(double value) => PixelToInch(value) * 6;

	/// <summary>
	/// Converts the given pixels to points
	/// </summary>
	/// <param name="value">The number of pixels</param>
	/// <returns>The equivalent number of points</returns>
	public static double PixelToPoint(double value) => value * (72.0 / 96.0);

	/// <summary>
	/// Converts the given pixels to a percentage
	/// </summary>
	/// <param name="value">The number of pixels</param>
	/// <param name="width">The width of the parent or image</param>
	/// <param name="height">The height of the parent or image</param>
	/// <param name="isWidth">Whether or not the size represents the width or height</param>
	/// <returns>The equivalent percentage</returns>
	/// <exception cref="ArgumentException">Thrown if the context of the size is not given correctly</exception>
	public static double PixelToPercent(double value, int? width, int? height, bool? isWidth)
	{
		if (isWidth is null) throw new ArgumentException(ERROR_PERCENT);

		if (isWidth.Value && width is null ||
			!isWidth.Value && height is null) throw new ArgumentException(ERROR_PERCENT);

		return value / (isWidth.Value ? width!.Value : height!.Value) * 100;
	}

	/// <summary>
	/// Converts the given pixels to a relative percentage
	/// </summary>
	/// <param name="value">The number of pixels</param>
	/// <param name="width">The width of the parent or image</param>
	/// <param name="height">The height of the parent or image</param>
	/// <returns>The equivalent relative percentage</returns>
	/// <exception cref="ArgumentException">Thrown if the context of the size is not given correctly</exception>
	public static double PixelToRelativePercentage(double value, int? width, int? height)
	{
		if (width is null || height is null) throw new ArgumentException(ERROR_PERCENT);
		return value / ((width.Value + height.Value) / 2.0) * 100;
	}

	/// <summary>
	/// Converts the given pixels to ems relative to the context's font size
	/// </summary>
	/// <param name="value">The number of pixels</param>
	/// <param name="fontSize">The size of the font in the current context</param>
	/// <returns>The equivalent number of ems</returns>
	public static double PixelToEm(double value, int? fontSize)
	{
		if (fontSize is null) throw new ArgumentException("Cannot use em measurement without a font size");

		return value / fontSize.Value;
	}
	#endregion

	#region To Pixels
	/// <summary>
	/// Converts the given centimeters to pixels
	/// </summary>
	/// <param name="value">The number of centimeters</param>
	/// <returns>The equivalent number of pixels</returns>
	public static double CentimeterToPixel(double value) => value * 37.795275590551181102362204724409;

	/// <summary>
	/// Converts the given millimeters to pixels
	/// </summary>
	/// <param name="value">The number of millimeters</param>
	/// <returns>The equivalent number of pixels</returns>
	public static double MillimeterToPixel(double value) => CentimeterToPixel(value) / 10;

	/// <summary>
	/// Converts the given quarter millimeters to pixels
	/// </summary>
	/// <param name="value">The number of quarter millimeters</param>
	/// <returns>The equivalent number of pixels</returns>
	public static double QuarterMillimeterToPixel(double value) => MillimeterToPixel(value) / 4;

	/// <summary>
	/// Converts the given inches to pixels
	/// </summary>
	/// <param name="value">The number of inches</param>
	/// <returns>The equivalent number of pixels</returns>
	public static double InchToPixel(double value) => value * 96;

	/// <summary>
	/// Converts the given picas to pixels
	/// </summary>
	/// <param name="value">The number of picas</param>
	/// <returns>The equivalent number of pixels</returns>
	public static double PicaToPixel(double value) => InchToPixel(value) / 6;

	/// <summary>
	/// Converts the given points to pixels
	/// </summary>
	/// <param name="value">The number of points</param>
	/// <returns>The equivalent number of pixels</returns>
	public static double PointToPixel(double value) => value / (72.0 / 96.0);

	/// <summary>
	/// Converts the given percentage to pixels
	/// </summary>
	/// <param name="value">The percentage of the size</param>
	/// <param name="width">The width of the parent or image</param>
	/// <param name="height">The height of the parent or image</param>
	/// <param name="isWidth">Whether or not the size represents the width or height</param>
	/// <returns>The equivalent number of pixels</returns>
	/// <exception cref="ArgumentException">Thrown if the context of the size is not given correctly</exception>
	public static double PercentToPixel(double value, int? width, int? height, bool? isWidth)
	{
		if (isWidth is null) throw new ArgumentException(ERROR_PERCENT);

		if (isWidth.Value && width is null ||
			!isWidth.Value && height is null) throw new ArgumentException(ERROR_PERCENT);

		return value / 100 * (isWidth.Value ? width!.Value : height!.Value);
	}

	/// <summary>
	/// Converts the given relative percentage to pixels
	/// </summary>
	/// <param name="value">The percentage of the size</param>
	/// <param name="width">The width of the parent or image</param>
	/// <param name="height">The height of the parent or image</param>
	/// <returns>The equivalent number of pixels</returns>
	/// <exception cref="ArgumentException">Thrown if the context of the size is not given correctly</exception>
	public static double RelativePercentageToPixel(double value, int? width, int? height)
	{
		if (width is null || height is null) throw new ArgumentException(ERROR_PERCENT);
		return value / 100 * (width.Value + height.Value) / 2;
	}

	/// <summary>
	/// Converts the given em value to pixels relative to the context's font size
	/// </summary>
	/// <param name="value">The number of ems</param>
	/// <param name="fontSize">The size of the font in the current context</param>
	/// <returns>The equivalent number of pixels</returns>
	public static double EmToPixel(double value, int? fontSize)
	{
		if (fontSize is null) throw new ArgumentException("Cannot use em measurement without a font size");

		return value * fontSize.Value;
	}
	#endregion
}
