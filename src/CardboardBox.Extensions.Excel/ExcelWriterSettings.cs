namespace CardboardBox.Extensions.Excel;

/// <summary>
/// Settings for writing excel work book
/// </summary>
public class ExcelWriteSettings
{
    /// <summary>
    /// The factory for creating the header cells' style
    /// </summary>
    public Func<IWorkbook, ICellStyle>? HeaderStyleFactory { get; set; } = null;

    /// <summary>
    /// The factory for creating a data cell's style
    /// </summary>
    public Func<IWorkbook, ICellStyle>? CellStyleFactory { get; set; } = null;

    /// <summary>
    /// The factory for getting the sheet name
    /// </summary>
    public Func<string>? SheetNameFactory { get; set; } = null;

    /// <summary>
    /// Whether or not to auto-size columns
    /// </summary>
    public bool AutoSizeColumns { get; set; } = true;

    /// <summary>
    /// Whether or not to add column filters
    /// </summary>
    public bool AddColumnFilters { get; set; } = true;

    /// <summary>
    /// Whether or not to include headers
    /// </summary>
    public bool IncludeHeaders { get; set; } = true;

    #region Property Resolvers
    internal string GetSheetName()
    {
        return SheetNameFactory?.Invoke() ?? "Export";
    }

    internal ICellStyle GetHeaderStyle(IWorkbook book)
    {
        if (HeaderStyleFactory is not null)
            return HeaderStyleFactory(book);

        var style = book.CreateCellStyle();
        var font = book.CreateFont();
        font.IsBold = true;
        style.SetFont(font);
        style.BorderBottom = BorderStyle.Thin;
        return style;
    }

    internal ICellStyle GetCellStyle(IWorkbook book)
    {
        if (CellStyleFactory is not null)
            return CellStyleFactory(book);

        var style = book.CreateCellStyle();
        var font = book.CreateFont();
        style.SetFont(font);
        return style;
    }
    #endregion

    /// <summary>
    /// Sets the header factory
    /// </summary>
    /// <param name="factory">The header factory</param>
    /// <returns>The current settings for fluent method chaining</returns>
    public ExcelWriteSettings WithHeaderStyleFactory(Func<IWorkbook, ICellStyle> factory)
    {
        HeaderStyleFactory = factory;
        return this;
    }

    /// <summary>
    /// Sets the headers to a default "fancy" style
    /// </summary>
    /// <returns>The current settings for fluent method chaining</returns>
    public ExcelWriteSettings WithFancyStyleHeaders()
    {
        return WithHeaderStyleFactory((book) =>
        {
            var myFont = (XSSFFont)book.CreateFont();
            myFont.FontHeightInPoints = 11;
            myFont.IsBold = true;
            myFont.FontName = "Calibri";
            var headerCellStyle = (XSSFCellStyle)book.CreateCellStyle();
            headerCellStyle.SetFont(myFont);
            headerCellStyle.BorderLeft = BorderStyle.Medium;
            headerCellStyle.BorderTop = BorderStyle.Medium;
            headerCellStyle.BorderRight = BorderStyle.Medium;
            headerCellStyle.BorderBottom = BorderStyle.Medium;
            headerCellStyle.Alignment = HorizontalAlignment.Center;
            headerCellStyle.VerticalAlignment = VerticalAlignment.Bottom;
            headerCellStyle.FillForegroundColor = NPOI.HSSF.Util.HSSFColor.Grey40Percent.Index;
            headerCellStyle.FillPattern = FillPattern.SolidForeground;
            headerCellStyle.WrapText = true;
            return headerCellStyle;
        });
    }

    /// <summary>
    /// Sets the cell style factory
    /// </summary>
    /// <param name="factory">The factory</param>
    /// <returns>The current settings for fluent method chaining</returns>
    public ExcelWriteSettings WithCellStyleFactory(Func<IWorkbook, ICellStyle> factory)
    {
        CellStyleFactory = factory;
        return this;
    }

    /// <summary>
    /// Sets the sheet name to the given name
    /// </summary>
    /// <param name="name">The sheet name to use</param>
    /// <param name="appendDate">Whether or not to append the current date to the sheet name</param>
    /// <param name="dateFormat">The format string for the date (defaults to yyyy-MM-dd)</param>
    /// <returns>The current settings for fluent method chaining</returns>
    public ExcelWriteSettings WithSheetName(string name, bool appendDate = false, string? dateFormat = null)
    {
        SheetNameFactory = () =>
        {
            if (!appendDate) return name;

            var format = dateFormat ?? "yyyy-MM-dd";
            return name + " " + DateTime.Now.ToString(format);
        };
        return this;
    }

    /// <summary>
    /// Sets whether or not to auto-size columns
    /// </summary>
    /// <param name="autoSize">Whether or not to auto-size columns</param>
    /// <returns>The current settings for fluent method chaining</returns>
    public ExcelWriteSettings WithAutoSizeColumns(bool autoSize = true)
    {
        AutoSizeColumns = autoSize;
        return this;
    }

    /// <summary>
    /// Sets whether or not to add column filters
    /// </summary>
    /// <param name="autoFilters">Whether or not to add column filters</param>
    /// <returns>The current settings for fluent method chaining</returns>
    public ExcelWriteSettings WithColumnFilters(bool autoFilters = true)
    {
        AddColumnFilters = autoFilters;
        return this;
    }

    /// <summary>
    /// Sets whether or not to include headers
    /// </summary>
    /// <param name="includeHeaders">Whether or not to include headers</param>
    /// <returns>The current settings for fluent method chaining</returns>
    public ExcelWriteSettings WithHeaders(bool includeHeaders = true)
    {
        IncludeHeaders = includeHeaders;
        return this;
    }
}