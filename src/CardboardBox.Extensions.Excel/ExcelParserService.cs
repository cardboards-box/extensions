namespace CardboardBox.Extensions.Excel;

using Attributes;

/// <summary>
/// Provides methods to deserialize Excel sheets into various data structures.
/// </summary>
public interface IExcelParserService
{
    /// <summary>
    /// Deserializes an Excel sheet by name into a collection of objects of type <typeparamref name="T"/>.
    /// </summary>
    /// <typeparam name="T">The type of objects to deserialize each row into.</typeparam>
    /// <param name="excel">The Excel wrapper containing the sheet.</param>
    /// <param name="sheet">The name of the sheet to deserialize.</param>
    /// <param name="headers">Whether the first row contains headers. Defaults to true.</param>
    /// <returns>An enumerable of objects of type <typeparamref name="T"/>.</returns>
    IEnumerable<T> Deserialize<T>(ExcelWrapper excel, string sheet, bool headers = true);

    /// <summary>
    /// Deserializes an Excel sheet by index into a collection of objects of type <typeparamref name="T"/>.
    /// </summary>
    /// <typeparam name="T">The type of objects to deserialize each row into.</typeparam>
    /// <param name="excel">The Excel wrapper containing the sheet.</param>
    /// <param name="sheet">The index of the sheet to deserialize.</param>
    /// <param name="headers">Whether the first row contains headers. Defaults to true.</param>
    /// <returns>An enumerable of objects of type <typeparamref name="T"/>.</returns>
    IEnumerable<T> Deserialize<T>(ExcelWrapper excel, int sheet, bool headers = true);

    /// <summary>
    /// Deserializes a given Excel sheet into a collection of objects of type <typeparamref name="T"/>.
    /// </summary>
    /// <typeparam name="T">The type of objects to deserialize each row into.</typeparam>
    /// <param name="sheet">The specific sheet to deserialize.</param>
    /// <param name="headers">Whether the first row contains headers. Defaults to true.</param>
    /// <returns>An enumerable of objects of type <typeparamref name="T"/>.</returns>
    IEnumerable<T> Deserialize<T>(ISheet sheet, bool headers = true);

    /// <summary>
    /// Deserializes an Excel sheet by name into a collection of objects of a specified type.
    /// </summary>
    /// <param name="excel">The Excel wrapper containing the sheet.</param>
    /// <param name="sheet">The name of the sheet to deserialize.</param>
    /// <param name="type">The type to deserialize each row into.</param>
    /// <param name="headers">Whether the first row contains headers. Defaults to true.</param>
    /// <returns>An enumerable of objects of the specified type.</returns>
    IEnumerable<object> Deserialize(ExcelWrapper excel, string sheet, Type type, bool headers = true);

    /// <summary>
    /// Deserializes an Excel sheet by index into a collection of objects of a specified type.
    /// </summary>
    /// <param name="excel">The Excel wrapper containing the sheet.</param>
    /// <param name="sheet">The index of the sheet to deserialize.</param>
    /// <param name="type">The type to deserialize each row into.</param>
    /// <param name="headers">Whether the first row contains headers. Defaults to true.</param>
    /// <returns>An enumerable of objects of the specified type.</returns>
    IEnumerable<object> Deserialize(ExcelWrapper excel, int sheet, Type type, bool headers = true);

    /// <summary>
    /// Deserializes a given Excel sheet into a collection of objects of a specified type.
    /// </summary>
    /// <param name="sheet">The specific sheet to deserialize.</param>
    /// <param name="type">The type to deserialize each row into.</param>
    /// <param name="headers">Whether the first row contains headers. Defaults to true.</param>
    /// <returns>An enumerable of objects of the specified type.</returns>
    IEnumerable<object> Deserialize(ISheet sheet, Type type, bool headers = true);

    /// <summary>
    /// Deserializes an Excel sheet by index into a DataTable.
    /// </summary>
    /// <param name="excel">The Excel wrapper containing the sheet.</param>
    /// <param name="sheet">The index of the sheet to deserialize.</param>
    /// <returns>A DataTable representing the sheet data.</returns>
    DataTable Deserialize(ExcelWrapper excel, int sheet);

    /// <summary>
    /// Deserializes an Excel sheet by name into a DataTable.
    /// </summary>
    /// <param name="excel">The Excel wrapper containing the sheet.</param>
    /// <param name="sheet">The name of the sheet to deserialize.</param>
    /// <returns>A DataTable representing the sheet data.</returns>
    DataTable Deserialize(ExcelWrapper excel, string sheet);

    /// <summary>
    /// Deserializes a given Excel sheet into a DataTable.
    /// </summary>
    /// <param name="sheet">The specific sheet to deserialize.</param>
    /// <returns>A DataTable representing the sheet data.</returns>
    DataTable Deserialize(ISheet sheet);

    /// <summary>
    /// Deserializes an Excel sheet by name into a collection of dictionaries, where each dictionary represents a row with header-value pairs.
    /// </summary>
    /// <param name="excel">The Excel wrapper containing the sheet.</param>
    /// <param name="sheet">The name of the sheet to deserialize.</param>
    /// <returns>An enumerable of dictionaries with header-value mappings for each row.</returns>
    IEnumerable<Dictionary<string, object?>> DeserializeHeaderMap(ExcelWrapper excel, string sheet);

    /// <summary>
    /// Deserializes an Excel sheet by index into a collection of dictionaries, where each dictionary represents a row with header-value pairs.
    /// </summary>
    /// <param name="excel">The Excel wrapper containing the sheet.</param>
    /// <param name="sheet">The index of the sheet to deserialize.</param>
    /// <returns>An enumerable of dictionaries with header-value mappings for each row.</returns>
    IEnumerable<Dictionary<string, object?>> DeserializeHeaderMap(ExcelWrapper excel, int sheet);

    /// <summary>
    /// Deserializes a given Excel sheet into a collection of dictionaries, where each dictionary represents a row with header-value pairs.
    /// </summary>
    /// <param name="sheet">The specific sheet to deserialize.</param>
    /// <returns>An enumerable of dictionaries with header-value mappings for each row.</returns>
    IEnumerable<Dictionary<string, object?>> DeserializeHeaderMap(ISheet sheet);

    /// <summary>
    /// Deserializes an Excel sheet by name into a collection of string arrays, where each array represents a row with values in column order.
    /// </summary>
    /// <param name="excel">The Excel wrapper containing the sheet.</param>
    /// <param name="sheet">The name of the sheet to deserialize.</param>
    /// <returns>An enumerable of string arrays with row values.</returns>
    IEnumerable<string[]> DeserializeIndexMap(ExcelWrapper excel, string sheet);

    /// <summary>
    /// Deserializes an Excel sheet by index into a collection of string arrays, where each array represents a row with values in column order.
    /// </summary>
    /// <param name="excel">The Excel wrapper containing the sheet.</param>
    /// <param name="sheet">The index of the sheet to deserialize.</param>
    /// <returns>An enumerable of string arrays with row values.</returns>
    IEnumerable<string[]> DeserializeIndexMap(ExcelWrapper excel, int sheet);

    /// <summary>
    /// Deserializes a given Excel sheet into a collection of string arrays, where each array represents a row with values in column order.
    /// </summary>
    /// <param name="sheet">The specific sheet to deserialize.</param>
    /// <returns>An enumerable of string arrays with row values.</returns>
    IEnumerable<string[]> DeserializeIndexMap(ISheet sheet);
}

/// <summary>
/// Provides methods to deserialize Excel sheets into various data structures.
/// </summary>
public class ExcelParserService : IExcelParserService
{
    #region Instance Deserialization
    /// <summary>
    /// Deserializes an Excel sheet by name into a collection of objects of type <typeparamref name="T"/>.
    /// </summary>
    /// <typeparam name="T">The type of objects to deserialize each row into.</typeparam>
    /// <param name="excel">The Excel wrapper containing the sheet.</param>
    /// <param name="sheet">The name of the sheet to deserialize.</param>
    /// <param name="headers">Whether the first row contains headers. Defaults to true.</param>
    /// <returns>An enumerable of objects of type <typeparamref name="T"/>.</returns>
    public IEnumerable<T> Deserialize<T>(ExcelWrapper excel, string sheet, bool headers = true)
    {
        var target = excel[sheet] ?? throw new ArgumentNullException(nameof(sheet), "Target sheet does not exist");
        return Deserialize<T>(target, headers);
    }

    /// <summary>
    /// Deserializes an Excel sheet by index into a collection of objects of type <typeparamref name="T"/>.
    /// </summary>
    /// <typeparam name="T">The type of objects to deserialize each row into.</typeparam>
    /// <param name="excel">The Excel wrapper containing the sheet.</param>
    /// <param name="sheet">The index of the sheet to deserialize.</param>
    /// <param name="headers">Whether the first row contains headers. Defaults to true.</param>
    /// <returns>An enumerable of objects of type <typeparamref name="T"/>.</returns>
    public IEnumerable<T> Deserialize<T>(ExcelWrapper excel, int sheet, bool headers = true)
    {
        var target = excel[sheet] ?? throw new ArgumentNullException(nameof(sheet), "Target sheet does not exist");
        return Deserialize<T>(target, headers);
    }

    /// <summary>
    /// Deserializes a given Excel sheet into a collection of objects of type <typeparamref name="T"/>.
    /// </summary>
    /// <typeparam name="T">The type of objects to deserialize each row into.</typeparam>
    /// <param name="sheet">The specific sheet to deserialize.</param>
    /// <param name="headers">Whether the first row contains headers. Defaults to true.</param>
    /// <returns>An enumerable of objects of type <typeparamref name="T"/>.</returns>
    public IEnumerable<T> Deserialize<T>(ISheet sheet, bool headers = true)
    {
        return Deserialize(sheet, typeof(T), headers).Select(t => (T)t);
    }

    /// <summary>
    /// Deserializes an Excel sheet by name into a collection of objects of a specified type.
    /// </summary>
    /// <param name="excel">The Excel wrapper containing the sheet.</param>
    /// <param name="sheet">The name of the sheet to deserialize.</param>
    /// <param name="type">The type to deserialize each row into.</param>
    /// <param name="headers">Whether the first row contains headers. Defaults to true.</param>
    /// <returns>An enumerable of objects of the specified type.</returns>
    public IEnumerable<object> Deserialize(ExcelWrapper excel, string sheet, Type type, bool headers = true)
    {
        var target = excel[sheet] ?? throw new ArgumentNullException(nameof(sheet), "Target sheet does not exist");
        return Deserialize(target, type, headers);
    }

    /// <summary>
    /// Deserializes an Excel sheet by index into a collection of objects of a specified type.
    /// </summary>
    /// <param name="excel">The Excel wrapper containing the sheet.</param>
    /// <param name="sheet">The index of the sheet to deserialize.</param>
    /// <param name="type">The type to deserialize each row into.</param>
    /// <param name="headers">Whether the first row contains headers. Defaults to true.</param>
    /// <returns>An enumerable of objects of the specified type.</returns>
    public IEnumerable<object> Deserialize(ExcelWrapper excel, int sheet, Type type, bool headers = true)
    {
        var target = excel[sheet] ?? throw new ArgumentNullException(nameof(sheet), "Target sheet does not exist");
        return Deserialize(target, type, headers);
    }

    /// <summary>
    /// Deserializes a given Excel sheet into a collection of objects of a specified type.
    /// </summary>
    /// <param name="sheet">The specific sheet to deserialize.</param>
    /// <param name="type">The type to deserialize each row into.</param>
    /// <param name="headers">Whether the first row contains headers. Defaults to true.</param>
    /// <returns>An enumerable of objects of the specified type.</returns>
    public IEnumerable<object> Deserialize(ISheet sheet, Type type, bool headers = true)
    {
        var propMap = ExcelUtilities.GetProps(type);
        if (headers) return DeserializeWithHeaders(sheet, type, propMap);

        return DeserializeWithIndex(sheet, type, propMap);
    }

    /// <summary>
    /// Deserializes a given Excel sheet into a collection of objects of a specified type.
    /// </summary>
    /// <param name="sheet">The specific sheet to deserialize.</param>
    /// <param name="type">The type to deserialize each row into.</param>
    /// <param name="props">The properties from the map</param>
    /// <returns>An enumerable of objects of the specified type.</returns>
    /// <exception cref="Exception">Thrown if there are no properties to parse</exception>
    /// <exception cref="ArgumentException">Thrown if an object instance can't be created</exception>
    public IEnumerable<object> DeserializeWithIndex(ISheet sheet, Type type, SerialMap[] props)
    {
        var map = props.Where(t => t.Index != -1).ToDictionary(t => t.Index);

        if (map.Count <= 0) throw new Exception("Property map returned 0 mapped indexes. Did you forget to add the Serial attribute to your properties?");

        var rows = sheet.LastRowNum;
        var cols = sheet.GetRow(rows).LastCellNum - 1;

        for (var r = 0; r <= rows; r++)
        {
            var row = sheet.GetRow(r);
            if (row == null) continue;

            var instance = Activator.CreateInstance(type) ??
                throw new ArgumentException("Cannot create instance of type", nameof(type));

            foreach (var cell in row)
            {
                var c = cell.ColumnIndex;
                if (!map.ContainsKey(c)) continue;

                map[c].Set(instance, GetCellStringValue(cell));
            }
            yield return instance;
        }
    }

    /// <summary>
    /// Deserializes a given Excel sheet into a collection of objects of a specified type.
    /// </summary>
    /// <param name="sheet">The specific sheet to deserialize.</param>
    /// <param name="type">The type to deserialize each row into.</param>
    /// <param name="props">The properties from the map</param>
    /// <returns>An enumerable of objects of the specified type.</returns>
    /// <exception cref="ArgumentNullException">Thrown if the sheet isn't valid</exception>
    /// <exception cref="Exception">Thrown if there are no properties to parse</exception>
    /// <exception cref="ArgumentException">Thrown if an object instance can't be created</exception>
    public IEnumerable<object> DeserializeWithHeaders(ISheet sheet, Type type, SerialMap[] props)
    {
        var headerRow = sheet.GetRow(0) ?? throw new ArgumentNullException(nameof(sheet), "Header row is not present");
        var headers = headerRow.Select(GetCellStringValue).ToArray();

        var map = headers.Select(t => (t, props.FirstOrDefault(a => a.Name == t))).ToDictionary(t => t.t, t => t.Item2);

        if (map.Count <= 0) throw new Exception("Property map returned 0 mapped indexes. Did you forget to add the Serial attribute to your properties?");

        var rows = sheet.LastRowNum;
        var cols = sheet.GetRow(rows).LastCellNum - 1;

        for (var r = 1; r <= rows; r++)
        {
            var row = sheet.GetRow(r);
            if (row == null) continue;

            var instance = Activator.CreateInstance(type) ??
                throw new ArgumentException("Cannot create instance of type", nameof(type));

            foreach (var cell in row)
            {
                var c = cell.ColumnIndex;
                if (c >= headers.Length) continue;

                var header = headers[c];
                if (!map.ContainsKey(header)) continue;

                var prop = map[header];
                if (prop == null) continue;

                prop.Set(instance, GetCellStringValue(cell));
            }

            yield return instance;
        }
    }
    #endregion

    #region DataTable Deserialization
    /// <summary>
    /// Deserializes an Excel sheet by index into a DataTable.
    /// </summary>
    /// <param name="excel">The Excel wrapper containing the sheet.</param>
    /// <param name="sheet">The index of the sheet to deserialize.</param>
    /// <returns>A DataTable representing the sheet data.</returns>
    public DataTable Deserialize(ExcelWrapper excel, int sheet)
    {
        var target = excel[sheet] ?? throw new ArgumentNullException(nameof(sheet), "Target sheet does not exist");
        return Deserialize(target);
    }

    /// <summary>
    /// Deserializes an Excel sheet by name into a DataTable.
    /// </summary>
    /// <param name="excel">The Excel wrapper containing the sheet.</param>
    /// <param name="sheet">The name of the sheet to deserialize.</param>
    /// <returns>A DataTable representing the sheet data.</returns>
    public DataTable Deserialize(ExcelWrapper excel, string sheet)
    {
        var target = excel[sheet] ?? throw new ArgumentNullException(nameof(sheet), "Target sheet does not exist");
        return Deserialize(target);
    }

    /// <summary>
    /// Deserializes a given Excel sheet into a DataTable.
    /// </summary>
    /// <param name="sheet">The specific sheet to deserialize.</param>
    /// <returns>A DataTable representing the sheet data.</returns>
    public DataTable Deserialize(ISheet sheet)
    {
        var headerRow = sheet.GetRow(0) ?? throw new ArgumentNullException(nameof(sheet), "Header row is not present");
        var headers = headerRow.Select(GetCellStringValue).ToArray();

        var dt = new DataTable();
        foreach (var header in headers)
            dt.Columns.Add(header, typeof(string));

        var rows = sheet.LastRowNum;
        var cols = sheet.GetRow(rows).LastCellNum - 1;

        for (var r = 1; r <= rows; r++)
        {
            var output = new string[headers.Length];
            var row = sheet.GetRow(r);
            if (row == null) continue;

            foreach (var cell in row)
            {
                var c = cell.ColumnIndex;
                if (c >= headers.Length) continue;

                output[c] = GetCellStringValue(cell);
            }

            dt.Rows.Add(output);
        }

        return dt;
    }
    #endregion

    #region Dictionary Deserialization
    /// <summary>
    /// Deserializes an Excel sheet by name into a collection of dictionaries, where each dictionary represents a row with header-value pairs.
    /// </summary>
    /// <param name="excel">The Excel wrapper containing the sheet.</param>
    /// <param name="sheet">The name of the sheet to deserialize.</param>
    /// <returns>An enumerable of dictionaries with header-value mappings for each row.</returns>
    public IEnumerable<Dictionary<string, object?>> DeserializeHeaderMap(ExcelWrapper excel, string sheet)
    {
        var target = excel[sheet] ?? throw new ArgumentNullException(nameof(sheet), "Target sheet does not exist");
        return DeserializeHeaderMap(target);
    }

    /// <summary>
    /// Deserializes an Excel sheet by index into a collection of dictionaries, where each dictionary represents a row with header-value pairs.
    /// </summary>
    /// <param name="excel">The Excel wrapper containing the sheet.</param>
    /// <param name="sheet">The index of the sheet to deserialize.</param>
    /// <returns>An enumerable of dictionaries with header-value mappings for each row.</returns>
    public IEnumerable<Dictionary<string, object?>> DeserializeHeaderMap(ExcelWrapper excel, int sheet)
    {
        var target = excel[sheet] ?? throw new ArgumentNullException(nameof(sheet), "Target sheet does not exist");
        return DeserializeHeaderMap(target);
    }

    /// <summary>
    /// Deserializes a given Excel sheet into a collection of dictionaries, where each dictionary represents a row with header-value pairs.
    /// </summary>
    /// <param name="sheet">The specific sheet to deserialize.</param>
    /// <returns>An enumerable of dictionaries with header-value mappings for each row.</returns>
    public IEnumerable<Dictionary<string, object?>> DeserializeHeaderMap(ISheet sheet)
    {
        var headerRow = sheet.GetRow(0) ?? throw new ArgumentNullException(nameof(sheet), "Header row is not present");
        var headers = headerRow.Select(GetCellStringValue).ToArray();

        var rows = sheet.LastRowNum;
        var cols = sheet.GetRow(rows).LastCellNum - 1;

        for (var r = 1; r <= rows; r++)
        {
            var row = sheet.GetRow(r);
            if (row == null) continue;

            var dic = new Dictionary<string, object?>();

            foreach (var cell in row)
            {
                var c = cell.ColumnIndex;
                if (c >= headers.Length) continue;

                var header = headers[c];
                if (!dic.ContainsKey(header))
                    dic.Add(header, GetCellValue(cell));
            }

            yield return dic;
        }
    }
    #endregion

    #region Array Deserialization
    /// <summary>
    /// Deserializes an Excel sheet by name into a collection of string arrays, where each array represents a row with values in column order.
    /// </summary>
    /// <param name="excel">The Excel wrapper containing the sheet.</param>
    /// <param name="sheet">The name of the sheet to deserialize.</param>
    /// <returns>An enumerable of string arrays with row values.</returns>
    public IEnumerable<string[]> DeserializeIndexMap(ExcelWrapper excel, string sheet)
    {
        var target = excel[sheet] ?? throw new ArgumentNullException(nameof(sheet), "Target sheet does not exist");
        return DeserializeIndexMap(target);
    }

    /// <summary>
    /// Deserializes an Excel sheet by index into a collection of string arrays, where each array represents a row with values in column order.
    /// </summary>
    /// <param name="excel">The Excel wrapper containing the sheet.</param>
    /// <param name="sheet">The index of the sheet to deserialize.</param>
    /// <returns>An enumerable of string arrays with row values.</returns>
    public IEnumerable<string[]> DeserializeIndexMap(ExcelWrapper excel, int sheet)
    {
        var target = excel[sheet] ?? throw new ArgumentNullException(nameof(sheet), "Target sheet does not exist");
        return DeserializeIndexMap(target);
    }

    /// <summary>
    /// Deserializes a given Excel sheet into a collection of dictionaries, where each dictionary represents a row with header-value pairs.
    /// </summary>
    /// <param name="sheet">The specific sheet to deserialize.</param>
    /// <returns>An enumerable of dictionaries with header-value mappings for each row.</returns>
    public IEnumerable<string[]> DeserializeIndexMap(ISheet sheet)
    {
        var rows = sheet.LastRowNum;
        var cols = sheet.GetRow(rows).LastCellNum - 1;

        for (var r = 0; r <= rows; r++)
        {
            var row = sheet.GetRow(r);
            if (row == null) continue;

            var output = new string[cols + 1];

            foreach (var cell in row)
            {
                var c = cell.ColumnIndex;
                output[c] = GetCellStringValue(cell);
            }

            yield return output;
        }
    }
    #endregion

    /// <summary>
    /// Gets the value of the cell
    /// </summary>
    /// <param name="cell">The cell</param>
    /// <returns>The cell value</returns>
    public string GetCellStringValue(ICell cell)
    {
        return (cell.CellType switch
        {
            CellType.Blank => string.Empty,
            CellType.Boolean => cell.BooleanCellValue == true ? "Y" : "N",
            CellType.String => cell.StringCellValue,
            CellType.Numeric => DateUtil.IsCellDateFormatted(cell) ? cell.DateCellValue.ToString() : cell.NumericCellValue.ToString(),
            CellType.Formula => cell.CachedFormulaResultType switch
            {
                CellType.Blank => string.Empty,
                CellType.String => cell.StringCellValue,
                CellType.Boolean => cell.BooleanCellValue == true ? "Y" : "N",
                CellType.Numeric => DateUtil.IsCellDateFormatted(cell) ? cell.DateCellValue.ToString() : cell.NumericCellValue.ToString(),
                _ => string.Empty,
            },
            _ => cell.StringCellValue,
        }) ?? string.Empty;
    }

    /// <summary>
    /// Gets the value of the cell
    /// </summary>
    /// <param name="cell">The cell</param>
    /// <returns>The cell value</returns>
    public object? GetCellValue(ICell cell)
    {
        return cell.CellType switch
        {
            CellType.Blank => null,
            CellType.Boolean => cell.BooleanCellValue,
            CellType.String => cell.StringCellValue,
            CellType.Numeric => DateUtil.IsCellDateFormatted(cell) ? cell.DateCellValue : cell.NumericCellValue,
            CellType.Formula => cell.CachedFormulaResultType switch
            {
                CellType.String => cell.StringCellValue,
                CellType.Boolean => cell.BooleanCellValue,
                CellType.Numeric => DateUtil.IsCellDateFormatted(cell) ? cell.DateCellValue : cell.NumericCellValue,
                _ => null,
            },
            _ => cell.StringCellValue,
        };
    }
}
