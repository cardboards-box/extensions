namespace CardboardBox.Extensions.Excel;

/// <summary>
/// Wrapper for working with Excel. Works with HSSF (1997 Excel) or XSSF (2000+ Excel)
/// </summary>
/// <remarks>
/// Load Excel document from Workbook
/// </remarks>
/// <param name="book">Book to use</param>
public class ExcelWrapper(IWorkbook book) : IDisposable
{
    /// <summary>
    /// The Workbook we are working with
    /// </summary>
    public IWorkbook Workbook { get; private set; } = book;

    /// <summary>
    /// Gets or creates the sheet by the sheet name
    /// </summary>
    /// <param name="sheet">The sheet name</param>
    /// <returns>Gets or creates the sheet by the sheet name</returns>
    public ISheet? this[string sheet]
    {
        get
        {
            if (Workbook == null)
                throw new ArgumentException("Workbook is null");
            return Workbook.GetSheet(sheet);
        }
    }

    /// <summary>
    /// Gets or creates the sheet by the sheet index (0 based)
    /// </summary>
    /// <param name="sheet">The sheet index</param>
    /// <returns>Gets or creates the sheet by the sheet index (0 based)</returns>
    public ISheet? this[int sheet]
    {
        get
        {
            if (Workbook == null)
                throw new ArgumentException("Workbook is null");

            return Workbook.GetSheetAt(sheet);
        }
    }

    /// <summary>
    /// Gets or creates the row by the sheet name
    /// </summary>
    /// <param name="sheet">Sheet name (Will create if doesn't exist)</param>
    /// <param name="row">Row index (Will create if doesn't exist)</param>
    /// <returns>The row requested</returns>
    public IRow? this[string sheet, int row]
    {
        get => this[sheet]?.GetRow(row);
    }

    /// <summary>
    /// Gets or creates the row by the sheet index (0 based)
    /// </summary>
    /// <param name="sheet">Sheet index (Will create if doesn't exist)</param>
    /// <param name="row">Row index (Will create if doesn't exist)</param>
    /// <returns>The row requested</returns>
    public IRow? this[int sheet, int row]
    {
        get => this[sheet]?.GetRow(row);
    }

    /// <summary>
    /// Gets or creates the cell by the sheet name, row index and column index
    /// </summary>
    /// <param name="sheet">Sheet name (Will create if not exists)</param>
    /// <param name="row">Row index (Will create if not exists)</param>
    /// <param name="col">Column Index (Will create if not exists)</param>
    /// <returns>The cell requested</returns>
    public ICell? this[string sheet, int row, int col]
    {
        get => this[sheet, row]?.GetCell(col);
    }

    /// <summary>
    /// Gets or creates the cell by the sheet index (0 based), row index and column index
    /// </summary>
    /// <param name="sheet">Sheet index (Will create if not exists)</param>
    /// <param name="row">Row index (Will create if not exists)</param>
    /// <param name="col">Column Index (Will create if not exists)</param>
    /// <returns>The cell requested</returns>
    public ICell? this[int sheet, int row, int col]
    {
        get => this[sheet, row]?.GetCell(col);
    }

    /// <summary>
    /// Gets or creates the cell by the sheet name and excel map (ex: D100)
    /// </summary>
    /// <param name="sheet">Sheet name (Will create if not exists)</param>
    /// <param name="colRow">Excel Mapping (aka fen). Mapping consistent to Excel, there for is not 0 based for row.</param>
    /// <returns>The cell requested</returns>
    public ICell? this[string sheet, string colRow]
    {
        get
        {
            var rc = FenToRowCol(colRow);
            return this[sheet, rc[0], rc[1]];
        }
    }

    /// <summary>
    /// Gets or creates the cell by the sheet index (0 based) and excel map (ex: D100)
    /// </summary>
    /// <param name="sheet">Sheet index (Will create if not exists)</param>
    /// <param name="colRow">Excel Mapping (aka fen). Mapping consistent to Excel, there for is not 0 based for row.</param>
    /// <returns>The cell requested</returns>
    public ICell? this[int sheet, string colRow]
    {
        get
        {
            var rc = FenToRowCol(colRow);
            return this[sheet, rc[0], rc[1]];
        }
    }

    /// <summary>
    /// All of the sheets within this workbook.
    /// </summary>
    public ISheet[] Sheets => AllSheets().ToArray();

    /// <summary>
    /// Default Constructor
    /// Will create a new workbook in XSSF (2000+ excel) workbook
    /// </summary>
    public ExcelWrapper() : this(CreateWorkbook()) { }

    /// <summary>
    /// Load Excel document from path
    /// </summary>
    /// <param name="path">The path to load the document from. XSSF or HSSF are accepted.</param>
    public ExcelWrapper(string path) : this(LoadWorkbookFromFile(path)) { }

    /// <summary>
    /// Load Excel document from stream
    /// </summary>
    /// <param name="input">Stream of workbook</param>
    public ExcelWrapper(Stream input) : this(LoadWorkbookFromStream(input)) { }

    /// <summary>
    /// Chained method for re-evaluating all formulas in the excel document
    /// </summary>
    /// <returns>The current instance of the work book (For Chaining)</returns>
    public ExcelWrapper EvaluateFormulas()
    {
        if (Workbook is XSSFWorkbook)
        {
            BaseFormulaEvaluator.EvaluateAllFormulaCells(Workbook);
            return this;
        }

        HSSFFormulaEvaluator.EvaluateAllFormulaCells(Workbook);
        return this;
    }

    /// <summary>
    /// Iterates through all of the sheets in the workbook
    /// </summary>
    /// <returns>A collection of all of the sheets in the workbook</returns>
    public IEnumerable<ISheet> AllSheets()
    {
        for (int i = 0; i < Workbook.NumberOfSheets; i++)
            yield return Workbook.GetSheetAt(i);
    }

    /// <summary>
    /// Get the sheet and last cell index from the given sheet
    /// </summary>
    /// <param name="sheet">The sheet index</param>
    /// <returns>A tuple containing the <see cref="ISheet"/> and last cell index</returns>
    /// <exception cref="ArgumentNullException"></exception>
    public (ISheet sheet, int cell) LastCellIndex(int sheet)
    {
        var s = this[sheet] ?? throw new ArgumentNullException(nameof(sheet), "Sheet does not exist");
        var r = this[sheet, s.LastRowNum] ?? throw new ArgumentNullException(nameof(sheet), "Sheet has no rows");
        var c = r.LastCellNum - 1;
        return (s, c);
    }

    /// <summary>
    /// Get the sheet and last cell index from the given sheet
    /// </summary>
    /// <param name="sheet">The sheet index</param>
    /// <returns>A tuple containing the <see cref="ISheet"/> and last cell index</returns>
    /// <exception cref="ArgumentNullException"></exception>
    public (ISheet sheet, int cell) LastCellIndex(string sheet)
    {
        var s = this[sheet] ?? throw new ArgumentNullException(nameof(sheet), "Sheet does not exist");
        var r = this[sheet, s.LastRowNum] ?? throw new ArgumentNullException(nameof(sheet), "Sheet has no rows");
        var c = r.LastCellNum - 1;
        return (s, c);
    }

    /// <summary>
    /// Get the last cell index from the given sheet
    /// </summary>
    /// <param name="sheet">The sheet</param>
    /// <returns>The last cell index from the last row in the sheet</returns>
    public static int LastCellIndex(ISheet sheet)
    {
        var r = sheet.GetRow(sheet.LastRowNum);
        var c = r.LastCellNum - 1;
        return c;
    }

    /// <summary>
    /// Chained Method
    /// Automatically sets the first row of the specified sheet index to being filtered.
    /// </summary>
    /// <param name="sheet">The sheet index to set the filters on</param>
    /// <returns>The current instance of the wrapper (for Chaining)</returns>
    public ExcelWrapper SetAutoFilters(int sheet)
    {
        var (s, c) = LastCellIndex(sheet);
        s.SetAutoFilter(new CellRangeAddress(0, s.LastRowNum, 0, c));
        return this;
    }

    /// <summary>
    /// Chained Method
    /// Automatically sets the first row of the specified sheet name to being filtered.
    /// </summary>
    /// <param name="sheet">The sheet name to set the filters on</param>
    /// <returns>The current instance of the wrapper (for Chaining)</returns>
    public ExcelWrapper SetAutoFilters(string sheet)
    {
        var (s, c) = LastCellIndex(sheet);
        s.SetAutoFilter(new CellRangeAddress(0, s.LastRowNum, 0, c));

        return this;
    }

    /// <summary>
    /// Chained Method
    /// Automatically sets the first row of the specified sheet name to being filtered.
    /// </summary>
    /// <param name="sheet">The sheet name to set the filters on</param>
    /// <returns>The current instance of the wrapper (for Chaining)</returns>
    public ExcelWrapper SetAutoFilters(ISheet sheet)
    {
        var c = LastCellIndex(sheet);
        sheet.SetAutoFilter(new CellRangeAddress(0, sheet.LastRowNum, 0, c));
        return this;
    }

    /// <summary>
    /// Automatically sets the width of the columns to fit the data specified
    /// </summary>
    /// <param name="sheet">The sheet name</param>
    /// <returns>The instance of the wrapper for Chaining</returns>
    public ExcelWrapper SetAutoWidth(string sheet)
    {
        var (s, c) = LastCellIndex(sheet);

        for (var i = 0; i <= c; i++)
        {
            s.AutoSizeColumn(i);
            var autoWidth = s.GetColumnWidth(i);
            // Leave room for the Filter/Sort button
            var newWidthWithSorting = autoWidth + 400;
            s.SetColumnWidth(i, newWidthWithSorting);
        }

        return this;
    }

    /// <summary>
    /// Automatically sets the width of the columns to fit the data specified
    /// </summary>
    /// <param name="sheet">The sheet index</param>
    /// <returns>The instance of the wrapper for Chaining</returns>
    public ExcelWrapper SetAutoWidth(int sheet)
    {
        var (s, c) = LastCellIndex(sheet);

        for (var i = 0; i <= c; i++)
        {
            s.AutoSizeColumn(i);
            var autoWidth = s.GetColumnWidth(i);
            // Leave room for the Filter/Sort button
            var newWidthWithSorting = autoWidth + 400;
            // widths are restricted to 255 characters.  Width unit is 1/255 of a character so column width can be maximum of 255 * 255
            newWidthWithSorting = newWidthWithSorting > 255 * 255 ? 255 * 255 : newWidthWithSorting;
            s.SetColumnWidth(i, newWidthWithSorting);
        }

        return this;
    }

    /// <summary>
    /// Automatically sets the width of the columns to fit the data specified
    /// </summary>
    /// <param name="sheet">The sheet</param>
    /// <returns>The instance of the wrapper for Chaining</returns>
    public ExcelWrapper SetAutoWidth(ISheet sheet)
    {
        var c = LastCellIndex(sheet);

        for (var i = 0; i <= c; i++)
        {
            sheet.AutoSizeColumn(i);
            var autoWidth = sheet.GetColumnWidth(i);
            // Leave room for the Filter/Sort button
            var newWidthWithSorting = autoWidth + 400;
            // widths are restricted to 255 characters.  Width unit is 1/255 of a character so column width can be maximum of 255 * 255
            newWidthWithSorting = newWidthWithSorting > 255 * 255 ? 255 * 255 : newWidthWithSorting;
            sheet.SetColumnWidth(i, newWidthWithSorting);
        }

        return this;
    }

    /// <summary>
    /// Generates the memory stream from the Excel Workbook
    /// </summary>
    /// <returns>The memory stream</returns>
    public Stream ToStream()
    {
        var ms = new ExcelStream { AllowClose = false };
        Workbook.Write(ms, true);
        ms.Flush();
        ms.Seek(0, SeekOrigin.Begin);
        ms.AllowClose = true;
        return ms;
    }

    /// <summary>
    /// Saves file to the specified path
    /// </summary>
    /// <param name="path">The path to save to</param>
    public void SaveToFile(string path)
    {
        if (Workbook == null)
            throw new NullReferenceException($"{nameof(Workbook)} has not been created.");

        using var f = File.OpenWrite(path);
        Workbook.Write(f, true);
        f.Flush();
    }

    /// <summary>
    /// Reads the given sheet as an array of dictionary objects
    /// </summary>
    /// <param name="sheet">The name of the sheet</param>
    /// <returns>The contents of the sheet</returns>
    public dynamic[] ReadSheetAsMap(string sheet)
    {
        var parser = new ExcelParserService();
        return parser.DeserializeHeaderMap(this, sheet)
            .Select(t =>
            {
                var eo = new ExpandoObject();
                var eoc = eo as ICollection<KeyValuePair<string, object?>>;

                foreach (var kvp in t)
                    eoc.Add(kvp);

                return eo;
            }).ToArray();
    }

    /// <summary>
    /// Reads the given sheet as an array of dictionary objects
    /// </summary>
    /// <param name="sheet">The index of the sheet</param>
    /// <returns>The contents of the sheet</returns>
    public dynamic[] ReadSheetAsMap(int sheet)
    {
        var parser = new ExcelParserService();
        return parser.DeserializeHeaderMap(this, sheet)
            .Select(t =>
            {
                var eo = new ExpandoObject();
                var eoc = eo as ICollection<KeyValuePair<string, object?>>;

                foreach (var kvp in t)
                    eoc.Add(kvp);

                return eo;
            }).ToArray();
    }

    /// <summary>
    /// Reads the given sheet as an array of CSV rows
    /// </summary>
    /// <param name="sheet">The name of the sheet</param>
    /// <returns>The contents of the sheet</returns>
    public string[][] ReadSheet(string sheet)
    {
        var parser = new ExcelParserService();
        return parser.DeserializeIndexMap(this, sheet).ToArray();
    }

    /// <summary>
    /// Reads the given sheet as an array of CSV rows
    /// </summary>
    /// <param name="sheet">The index of the sheet</param>
    /// <returns>The contents of the sheet</returns>
    public string[][] ReadSheet(int sheet)
    {
        var parser = new ExcelParserService();
        return parser.DeserializeIndexMap(this, sheet).ToArray();
    }

    /// <summary>
    /// Reads the first record for the spreadsheet
    /// </summary>
    /// <param name="sheet">The name of the sheet</param>
    /// <returns>The header records from the sheet</returns>
    public string[] ReadSheetHeaders(string sheet)
    {
        var item = this[sheet] ?? throw new ArgumentException($"Couldn't find the specified spreadsheet: `{sheet}`", nameof(sheet));
        return ReadSheetHeaders(item);
    }

    /// <summary>
    /// Reads the first record for the spreadsheet
    /// </summary>
    /// <param name="sheet">The index of the sheet</param>
    /// <returns>The header records from the sheet</returns>
    public string[] ReadSheetHeaders(int sheet)
    {
        var item = this[sheet] ?? throw new ArgumentException($"Couldn't find the specified spreadsheet: `{sheet}`", nameof(sheet));
        return ReadSheetHeaders(item);
    }

    /// <summary>
    /// Reads the first record for the spreadsheet
    /// </summary>
    /// <param name="sheet">The sheet to read from</param>
    /// <returns>The header records from the sheet</returns>
    public static string[] ReadSheetHeaders(ISheet sheet)
    {
        var parser = new ExcelParserService();
        var headerRow = sheet.GetRow(0) ?? throw new ArgumentException("Header row could not be found", nameof(sheet));
        return headerRow.Select(parser.GetCellStringValue).ToArray();
    }

    /// <summary>
    /// Converts a row/column combination to an Excel Fen Mapping
    /// </summary>
    /// <param name="row">Row to use</param>
    /// <param name="col">Column to use</param>
    /// <param name="includeLock">Whether to include the $ between row and column</param>
    /// <returns>The Excel Fen Mapping</returns>
    public static string RowColToFen(int row, int col, bool includeLock = false)
    {
        var fen = NumberToExcelColumnName(col);
        return $"{fen}{(includeLock ? "$" : "")}{row}";
    }

    /// <summary>
    /// Converts excel fen mapping to row/col
    /// </summary>
    /// <param name="colRow">The fen mapping</param>
    /// <returns>The row (0) and column (1) </returns>
    public static int[] FenToRowCol(string colRow)
    {
        var fen = "";
        foreach (var car in colRow)
        {
            if (int.TryParse(car.ToString(), out int tmp))
                break;

            fen += car.ToString();
        }

        var col = ExcelColumnNameToNumber(fen.TrimEnd('$')) - 1;
        var row = int.Parse(colRow.Remove(0, fen.Length)) - 1;
        return [row, col];
    }

    /// <summary>
    /// Convert column letter to numeric column (0 based)
    /// </summary>
    /// <param name="columnName">The column letter</param>
    /// <returns>The column index</returns>
    public static int ExcelColumnNameToNumber(string columnName)
    {
        if (string.IsNullOrEmpty(columnName)) throw new ArgumentNullException(nameof(columnName));

        columnName = columnName.ToUpperInvariant();

        int sum = 0;

        for (int i = 0; i < columnName.Length; i++)
        {
            sum *= 26;
            sum += (columnName[i] - 'A' + 1);
        }

        return sum;
    }

    /// <summary>
    /// Coverts Column index to Column letter
    /// </summary>
    /// <param name="columnNumber">Index 0 column number</param>
    /// <returns>The column letter</returns>
    public static string NumberToExcelColumnName(int columnNumber)
    {
        int dividend = columnNumber;
        string columnName = string.Empty;
        int modulo;

        while (dividend > 0)
        {
            modulo = (dividend - 1) % 26;
            columnName = Convert.ToChar(65 + modulo).ToString() + columnName;
            dividend = (dividend - modulo) / 26;
        }

        return columnName;
    }

    /// <summary>
    /// Loads workbook from data stream
    /// </summary>
    /// <param name="dataInput">The input stream</param>
    /// <param name="xSSF">Whether a XSSF or HSSF book</param>
    /// <returns>The workbook loaded</returns>
    private static IWorkbook LoadWorkbookFromStream(Stream dataInput, bool xSSF = true)
    {
        return xSSF ? new XSSFWorkbook(dataInput) : new HSSFWorkbook(dataInput);
    }

    /// <summary>
    /// Loads the workbook from a file
    /// </summary>
    /// <returns>The workbook loaded</returns>
    private static IWorkbook LoadWorkbookFromFile(string path)
    {
        if (!File.Exists(path))
            throw new FileNotFoundException("File not found at " + path);

        var ex = Path.GetExtension(path);

        using var fs = new FileStream(path, FileMode.Open, FileAccess.Read);
        return ex == ".xlsx" ? new XSSFWorkbook(fs) : new HSSFWorkbook(fs);
    }

    /// <summary>
    /// Creates a workbook in memory
    /// </summary>
    /// <returns>The workbook created</returns>
    private static IWorkbook CreateWorkbook()
    {
        return new XSSFWorkbook();
    }

    /// <summary>
    /// Disposes of the workbook
    /// </summary>
    public void Dispose()
    {
        Workbook?.Dispose();
        GC.SuppressFinalize(this);
    }

    /// <summary>
    /// Allow for straight casting to excel wrapper from loaded workbook
    /// </summary>
    /// <param name="book">Workbook to load from</param>
    public static implicit operator ExcelWrapper(XSSFWorkbook book)
    {
        return new ExcelWrapper(book);
    }

    /// <summary>
    /// Allow for straight casting to excel wrapper from loaded workbook
    /// </summary>
    /// <param name="book">Workbook to load from</param>
    public static implicit operator ExcelWrapper(HSSFWorkbook book)
    {
        return new ExcelWrapper(book);
    }
}
