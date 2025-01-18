namespace CardboardBox.Extensions.Excel;

using Props = KeyValuePair<PropertyInfo, Attributes.SerialAttribute?>[];

internal class ExcelWriter(ExcelWrapper book, ExcelWriteSettings settings)
{
    public ExcelWrapper Book { get; } = book;
    public ExcelWriteSettings Settings { get; } = settings;

    public int IncludeHeaders(ISheet sheet, IEnumerable<string> headers)
    {
        if (!Settings.IncludeHeaders) return 0;

        var style = Settings.GetHeaderStyle(Book.Workbook);
        var row = sheet.CreateRow(0);

        int i = 0;
        foreach (var header in headers)
        {
            var cell = row.CreateCell(i);
            i++;

            cell.SetCellValue(header);
            cell.CellStyle = style;
        }

        return 1;
    }

    public void WriteSheet(ISheet sheet, IEnumerable<string> headers, IEnumerable<IEnumerable<string>> cells)
    {
        int rowIndex = IncludeHeaders(sheet, headers);
        var style = Settings.GetCellStyle(Book.Workbook);
        foreach (var cellValues in cells)
        {
            var row = sheet.CreateRow(rowIndex);
            var cellIndex = 0;
            foreach (var cellValue in cellValues)
            {
                var cell = row.CreateCell(cellIndex);
                cellIndex++;

                cell.SetCellValue(cellValue);
                cell.CellStyle = style;
            }
            rowIndex++;
        }

        if (Settings.AddColumnFilters) Book.SetAutoFilters(sheet);
        if (Settings.AutoSizeColumns) Book.SetAutoWidth(sheet);
    }

    public void WriteSheet(IEnumerable<string> headers, IEnumerable<IEnumerable<string>> cells)
    {
        var sheetName = Settings.GetSheetName();
        var sheet = Book.Workbook.CreateSheet(sheetName);
        WriteSheet(sheet, headers, cells);
    }

    public void WriteSheet<T>(IEnumerable<T> data)
    {
        static IEnumerable<string> CellValues(T item, Props props)
        {
            for (var col = 0; col < props.Length; col++)
            {
                var prop = props[col];
                yield return ExcelUtilities.GetStringValue(item, prop);
            }
        }

        var props = ExcelUtilities.GetProperties(typeof(T));
        var headers = props.Select(t => t.Value?.Name ?? t.Key.Name);
        var cells = data.Select(item => CellValues(item, props));
        WriteSheet(headers, cells);
    }

    public void WriteSheet(DataTable table)
    {
        var props = table.Columns.Cast<DataColumn>().ToArray();
        var headers = props.Select(t => t.ColumnName);
        WriteSheet(table, headers, props);
    }

    public void WriteSheet(DataTable table, string[] headers)
    {
        var props = table.Columns.Cast<DataColumn>().ToArray();
        WriteSheet(table, headers, props);
    }

    public void WriteSheet(DataTable table, IEnumerable<string> headers, DataColumn[] props)
    {
        static IEnumerable<string> CellValues(DataRow item, DataColumn[] props)
        {
            for (var col = 0; col < props.Length; col++)
            {
                var prop = props[col];
                yield return item.ItemArray[col]?.ToString() ?? string.Empty;
            }
        }

        static IEnumerable<IEnumerable<string>> Rows(DataTable table, DataColumn[] props)
        {
            foreach (DataRow item in table.Rows)
                yield return CellValues(item, props);
        }

        var cells = Rows(table, props);
        WriteSheet(headers, cells);
    }
}