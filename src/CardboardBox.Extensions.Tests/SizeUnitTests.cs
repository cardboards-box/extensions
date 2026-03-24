using System.Text.Json;
using System.Text.Json.Serialization;

namespace CardboardBox.Extensions.Tests;

using Utilities.SizeUnits;

[TestClass]
public class SizeUnitTests
{
	private const double Epsilon = 1e-10;

	private static void AssertClose(double expected, double actual, double epsilon = Epsilon)
	{
		Assert.IsLessThanOrEqualTo(epsilon, Math.Abs(expected - actual), 
			$"Expected: {expected}, Actual: {actual}, Diff: {Math.Abs(expected - actual)}, Epsilon: {epsilon}");
	}

	[TestMethod]
	public void InchToPixel_KnownValue_1InchIs96px()
		=> AssertClose(96.0, PixelConversions.InchToPixel(1.0));

	[TestMethod]
	public void PointToPixel_KnownValue_72ptIs96px()
		=> AssertClose(96.0, PixelConversions.PointToPixel(72.0));

	[TestMethod]
	public void PicaToPixel_KnownValue_6PicaIs96px()
		=> AssertClose(96.0, PixelConversions.PicaToPixel(6.0));

	[TestMethod]
	[DataRow(0)]
	[DataRow(1)]
	[DataRow(2.54)]
	[DataRow(10)]
	[DataRow(123.456)]
	public void Centimeter_RoundTrip(double cm)
	{
		var px = PixelConversions.CentimeterToPixel(cm);
		var back = PixelConversions.PixelToCentimeter(px);
		AssertClose(cm, back);
	}

	[TestMethod]
	[DataRow(0)]
	[DataRow(1)]
	[DataRow(10)]
	[DataRow(250.25)]
	public void Millimeter_RoundTrip(double mm)
	{
		var px = PixelConversions.MillimeterToPixel(mm);
		var back = PixelConversions.PixelToMillimeter(px);
		AssertClose(mm, back);
	}

	[TestMethod]
	[DataRow(0)]
	[DataRow(1)]
	[DataRow(4)]
	[DataRow(123.75)]
	public void QuarterMillimeter_RoundTrip(double qmm)
	{
		var px = PixelConversions.QuarterMillimeterToPixel(qmm);
		var back = PixelConversions.PixelToQuarterMillimeter(px);
		AssertClose(qmm, back);
	}

	[TestMethod]
	[DataRow(0)]
	[DataRow(0.5)]
	[DataRow(1)]
	[DataRow(8.25)]
	public void Inch_RoundTrip(double inches)
	{
		var px = PixelConversions.InchToPixel(inches);
		var back = PixelConversions.PixelToInch(px);
		AssertClose(inches, back);
	}

	[TestMethod]
	[DataRow(0)]
	[DataRow(1)]
	[DataRow(6)]
	[DataRow(12.5)]
	public void Pica_RoundTrip(double pica)
	{
		var px = PixelConversions.PicaToPixel(pica);
		var back = PixelConversions.PixelToPica(px);
		AssertClose(pica, back);
	}

	[TestMethod]
	[DataRow(0)]
	[DataRow(1)]
	[DataRow(12)]
	[DataRow(72)]
	[DataRow(123.456)]
	public void Point_RoundTrip(double pt)
	{
		var px = PixelConversions.PointToPixel(pt);
		var back = PixelConversions.PixelToPoint(px);
		AssertClose(pt, back);
	}

	[TestMethod]
	public void PercentToPixel_UsesWidth_WhenIsWidthTrue()
	{
		var px = PixelConversions.PercentToPixel(25, width: 200, height: 999, isWidth: true);
		AssertClose(50.0, px);
	}

	[TestMethod]
	public void PercentToPixel_UsesHeight_WhenIsWidthFalse()
	{
		var px = PixelConversions.PercentToPixel(25, width: 999, height: 200, isWidth: false);
		AssertClose(50.0, px);
	}

	[TestMethod]
	public void Percent_RoundTrip_Width()
	{
		var px = PixelConversions.PercentToPixel(37.5, width: 400, height: 300, isWidth: true);
		var back = PixelConversions.PixelToPercent(px, width: 400, height: 300, isWidth: true);
		AssertClose(37.5, back);
	}

	[TestMethod]
	public void Percent_RoundTrip_Height()
	{
		var px = PixelConversions.PercentToPixel(37.5, width: 400, height: 300, isWidth: false);
		var back = PixelConversions.PixelToPercent(px, width: 400, height: 300, isWidth: false);
		AssertClose(37.5, back);
	}

	[TestMethod]
	public void RelativePercentageToPixel_KnownValue()
	{
		// avg = (200 + 100)/2 = 150, so 20% => 30px
		var px = PixelConversions.RelativePercentageToPixel(20, width: 200, height: 100);
		AssertClose(30.0, px);
	}

	[TestMethod]
	public void RelativePercent_RoundTrip()
	{
		var px = PixelConversions.RelativePercentageToPixel(12.34, width: 640, height: 480);
		var back = PixelConversions.PixelToRelativePercentage(px, width: 640, height: 480);
		AssertClose(12.34, back);
	}

	[TestMethod]
	public void EmToPixel_KnownValue()
	{
		var px = PixelConversions.EmToPixel(2.0, fontSize: 16);
		AssertClose(32.0, px);
	}

	[TestMethod]
	public void Em_RoundTrip()
	{
		var px = PixelConversions.EmToPixel(1.25, fontSize: 20);
		var back = PixelConversions.PixelToEm(px, fontSize: 20);
		AssertClose(1.25, back);
	}

	[TestMethod]
	public void DeserializeTests()
	{
		(string test, SizeUnit output)[] tests =
		[
			("-2.3cm", new(SizeUnitType.Centimeter, -2.3)),
			("3.3mm", new(SizeUnitType.Millimeter, 3.3)),
			("1q", new(SizeUnitType.QuarterMillimeter, 1)),
			("00100.000in", new(SizeUnitType.Inch, 100)),
			("1.2pc", new(SizeUnitType.Pica, 1.2)),
			("1.2pt", new(SizeUnitType.Point, 1.2)),
			("1.2%", new(SizeUnitType.Percentage, 1.2)),
			("1.2em", new(SizeUnitType.Em, 1.2)),
			("1.2vh", new(SizeUnitType.ViewHeight, 1.2)),
			("1.2vw", new(SizeUnitType.ViewWidth, 1.2)),
			("1.2rp", new(SizeUnitType.RelativePercentage, 1.2)),
			("1.2", new(SizeUnitType.Pixel, 1.2))
		];

		foreach (var (test, output) in tests)
		{
			var parsed = SizeUnit.Parse(test);
			Assert.AreEqual(output.Type, parsed.Type, $"Type Test: {test}");
			Assert.AreEqual(output.Value, parsed.Value, $"Value Test: {test}");
		}
	}

	[TestMethod]
	public void SerializeTests()
	{
		(SizeUnit test, string output)[] tests =
		[
			(new(SizeUnitType.Centimeter, -2.3), "-2.3cm"),
			(new(SizeUnitType.Millimeter, 3.3), "3.3mm"),
			(new(SizeUnitType.QuarterMillimeter, 1), "1q"),
			(new(SizeUnitType.Inch, 100), "100in"),
			(new(SizeUnitType.Pica, 1.2), "1.2pc"),
			(new(SizeUnitType.Point, 1.2), "1.2pt"),
			(new(SizeUnitType.Percentage, 1.2), "1.2%"),
			(new(SizeUnitType.Em, 1.2), "1.2em"),
			(new(SizeUnitType.ViewHeight, 1.2), "1.2vh"),
			(new(SizeUnitType.ViewWidth, 1.2), "1.2vw"),
			(new(SizeUnitType.RelativePercentage, 1.2), "1.2rp"),
			(new(SizeUnitType.Pixel, 1.2), "1.2px")
		];

		foreach (var (test, output) in tests)
		{
			var serialized = test.Serialize();
			Assert.AreEqual(output, serialized, $"Serialize Test: {test.Type} - {test.Value}");
		}
	}

	[TestMethod]
	public void CalculatePixelTests()
	{
		var root = new SizeContext(0, 0, 200, 100, 15, string.Empty);

		var context = root.GetContext(0, 0, 100, 50);

		(string test, int output, bool? isWidth)[] tests =
		[
			("12px", 12, null),
			("22cm", 831, null),
			("-30.50mm", -115, null),
			("00999.00q", 944, null),
			("-.4in", -38, null),
			("1,000.30pc", 16005, null),
			("-69.69pt", -93, null),
			("100vw", 200, null),
			("80vw", 160, null),
			("50vh", 50, null),
			("200vh", 200, null),
			("20em", 300, null),
			("30%", 30, true),
			("60%", 30, false)
		];

		foreach (var (test, output, isWidth) in tests)
		{
			var size = SizeUnit.Parse(test);
			var px = size.Pixels(context, isWidth);
			Assert.AreEqual(output, px, $"Pixel Test: {test}");
		}
	}

	[TestMethod]
	public void JsonTests()
	{
		var values = new[]
		{
			"-2.3cm",
			"3.3mm",
			"1q",
			"100in",
			"1.2pc",
			"1.2pt",
			"1.2%",
			"1.2em",
			"1.2vh",
			"1.2vw",
			"1.2rp",
			"1.2px"
		}.Select(SizeUnit.Parse);

		foreach (var size in values)
		{
			var testObj = new JsonSizeUnitTest { Size = size };
			var json = JsonSerializer.Serialize(testObj);
			var obj = JsonSerializer.Deserialize<JsonSizeUnitTest>(json);

			Assert.AreEqual(size.ToString(), obj?.Size?.Serialize(), $"Json Test: {size}");
		}
	}

	[TestMethod]
	public void JsonNullTests()
	{
		var testObj = new JsonSizeUnitTest { Size = null };
		var json = JsonSerializer.Serialize(testObj);
		var obj = JsonSerializer.Deserialize<JsonSizeUnitTest>(json);

		Assert.IsNotNull(obj, "Json Null Test - Parent");
		Assert.IsNull(obj.Size, "Json Null Test - Value");
	}

	internal class JsonSizeUnitTest
	{
		[JsonPropertyName("size")]
		public SizeUnit? Size { get; set; }
	}
}
