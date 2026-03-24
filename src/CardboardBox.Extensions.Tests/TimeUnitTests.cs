namespace CardboardBox.Extensions.Tests;

using Utilities.TimeUnits;

[TestClass]
public class TimeUnitTests
{
	private const double Tolerance = 1e-9;

	#region Zero / Serialize / ToString / implicit string

	[TestMethod]
	public void Zero_IsMillisecondZero()
	{
		Assert.AreEqual(TimeUnitType.Millisecond, TimeUnit.Zero.Type);
		Assert.AreEqual(0d, TimeUnit.Zero.Value, Tolerance);
	}

	[TestMethod]
	public void Serialize_Zero_Returns0()
	{
		Assert.AreEqual("0", TimeUnit.Zero.Serialize());
		Assert.AreEqual("0", TimeUnit.SerializeUnit(TimeUnit.Zero));
		Assert.AreEqual("0", TimeUnit.Zero.ToString());
		Assert.AreEqual("0", (string)TimeUnit.Zero);
	}

	[TestMethod]
	public void Serialize_UsesFirstSymbolForType()
	{
		var unit = new TimeUnit(TimeUnitType.Second, 2.5);
		// First symbol for Second in Units() is "s"
		Assert.AreEqual("2.5s", unit.Serialize());
		Assert.AreEqual("2.5s", unit.ToString());
		Assert.AreEqual("2.5s", (string)unit);
	}

	[TestMethod]
	public void Serialize_UnknownType_FallsBackToMsSuffix()
	{
		// If TimeUnitType is an enum, it can still hold undefined values.
		var unit = new TimeUnit((TimeUnitType)12345, 7);
		Assert.AreEqual("7ms", unit.Serialize());
	}

	#endregion

	#region GetMilliseconds / Milliseconds / TimeSpan

	[TestMethod]
	public void Milliseconds_WhenValueIsZero_IsZero_ForAnyType()
	{
		Assert.AreEqual(0d, new TimeUnit(TimeUnitType.Millisecond, 0).Milliseconds, Tolerance);
		Assert.AreEqual(0d, new TimeUnit(TimeUnitType.Year, 0).Milliseconds, Tolerance);
	}

	[TestMethod]
	public void GetMilliseconds_ConvertsAllKnownUnitsCorrectly()
	{
		static void AssertMs(TimeUnitType type, double value, double expectedMs)
		{
			var unit = new TimeUnit(type, value);

			Assert.AreEqual(expectedMs, TimeUnit.GetMilliseconds(unit), Tolerance);
			Assert.AreEqual(expectedMs, unit.Milliseconds, Tolerance);
			Assert.AreEqual(TimeSpan.FromMilliseconds(expectedMs), unit.TimeSpan);
		}

		AssertMs(TimeUnitType.Millisecond, 1, 1);
		AssertMs(TimeUnitType.Second, 1, 1_000);
		AssertMs(TimeUnitType.Minute, 1, 60_000);
		AssertMs(TimeUnitType.Hour, 1, 3_600_000);
		AssertMs(TimeUnitType.Day, 1, 86_400_000);
		AssertMs(TimeUnitType.Week, 1, 604_800_000);
		AssertMs(TimeUnitType.Month, 1, 2_592_000_000);          // 30 days
		AssertMs(TimeUnitType.Quarter, 1, 10_368_000_000);       // 4 * 30 days
		AssertMs(TimeUnitType.Year, 1, 31_536_000_000);          // 365 days
		AssertMs(TimeUnitType.Decade, 1, 315_360_000_000);
		AssertMs(TimeUnitType.Century, 1, 3_153_600_000_000);
		AssertMs(TimeUnitType.Millennium, 1, 31_536_000_000_000);
	}

	[TestMethod]
	public void GetMilliseconds_UnknownType_Returns0()
	{
		var unit = new TimeUnit((TimeUnitType)12345, 10);
		Assert.AreEqual(0d, unit.Milliseconds, Tolerance);
	}

	#endregion

	#region Units()

	[TestMethod]
	public void Units_ReturnsExpectedCommonSymbols()
	{
		var units = TimeUnit.Units();

		Assert.IsTrue(units.Any(u => u.Type == TimeUnitType.Millisecond && u.Symbol == "ms"));
		Assert.IsTrue(units.Any(u => u.Type == TimeUnitType.Second && u.Symbol == "s"));
		Assert.IsTrue(units.Any(u => u.Type == TimeUnitType.Minute && u.Symbol == "m"));
		Assert.IsTrue(units.Any(u => u.Type == TimeUnitType.Hour && u.Symbol == "h"));
		Assert.IsTrue(units.Any(u => u.Type == TimeUnitType.Day && u.Symbol == "d"));
		Assert.IsTrue(units.Any(u => u.Type == TimeUnitType.Week && u.Symbol == "w"));
		Assert.IsTrue(units.Any(u => u.Type == TimeUnitType.Month && u.Symbol == "mo"));
		Assert.IsTrue(units.Any(u => u.Type == TimeUnitType.Year && u.Symbol == "y"));
	}

	#endregion

	#region Parse / TryParse / implicit TimeUnit(string)
	
	[TestMethod]
	public void TryParse_NullOrWhitespace_ReturnsFalseAndZero()
	{
		Assert.IsFalse(TimeUnit.TryParse(null, out var u1));
		Assert.AreEqual(TimeUnit.Zero, u1);

		Assert.IsFalse(TimeUnit.TryParse("   ", out var u2));
		Assert.AreEqual(TimeUnit.Zero, u2);
	}

	[TestMethod]
	public void TryParse_StringZero_ReturnsFalse()
	{
		Assert.IsFalse(TimeUnit.TryParse("0", out var unit));
		Assert.AreEqual(TimeUnit.Zero, unit);
	}

	[TestMethod]
	public void Parse_Invalid_ReturnsZero()
	{
		// Parse uses TryParse and returns Zero on failure.
		var unit = TimeUnit.Parse("not-a-time-unit");
		Assert.AreEqual(TimeUnit.Zero, unit);
	}

	[TestMethod]
	public void Parse_NumberOnly_TreatsAsMilliseconds()
	{
		var unit = TimeUnit.Parse("123.5");
		Assert.AreEqual(TimeUnitType.Millisecond, unit.Type);
		Assert.AreEqual(123.5d, unit.Value, Tolerance);
	}

	[TestMethod]
	public void TryParse_ValidWithUnitSymbol_ParsesCorrectly()
	{
		Assert.IsTrue(TimeUnit.TryParse("20ms", out var ms));
		Assert.AreEqual(TimeUnitType.Millisecond, ms.Type);
		Assert.AreEqual(20d, ms.Value, Tolerance);

		Assert.IsTrue(TimeUnit.TryParse("3.2s", out var s));
		Assert.AreEqual(TimeUnitType.Second, s.Type);
		Assert.AreEqual(3.2d, s.Value, Tolerance);

		Assert.IsTrue(TimeUnit.TryParse("1m", out var m));
		Assert.AreEqual(TimeUnitType.Minute, m.Type);
		Assert.AreEqual(1d, m.Value, Tolerance);

		Assert.IsTrue(TimeUnit.TryParse("2h", out var h));
		Assert.AreEqual(TimeUnitType.Hour, h.Type);
		Assert.AreEqual(2d, h.Value, Tolerance);
	}

	[TestMethod]
	public void TryParse_IsCaseInsensitive_AndTrims()
	{
		Assert.IsTrue(TimeUnit.TryParse("  2H  ", out var unit));
		Assert.AreEqual(TimeUnitType.Hour, unit.Type);
		Assert.AreEqual(2d, unit.Value, Tolerance);
	}

	[TestMethod]
	public void TryParse_UnknownUnitSuffix_ReturnsFalse()
	{
		Assert.IsFalse(TimeUnit.TryParse("5xy", out var unit));
		Assert.AreEqual(TimeUnit.Zero, unit);
	}

	[TestMethod]
	public void Implicit_FromString_UsesParse()
	{
		TimeUnit unit = "15s";
		Assert.AreEqual(TimeUnitType.Second, unit.Type);
		Assert.AreEqual(15d, unit.Value, Tolerance);
	}

	#endregion
}
