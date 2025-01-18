namespace CardboardBox.Extensions.Excel;

/// <summary>
/// Service for writing excel files from C# data-sets
/// </summary>
public interface IExcelWriterService
{
    /// <summary>
    /// Writes the collection to the given work book
    /// </summary>
    /// <typeparam name="T">The type of data to write</typeparam>
    /// <param name="book">The workbook to write to</param>
    /// <param name="data">The data to write</param>
    /// <param name="settings">The settings for the writer</param>
    void Write<T>(ExcelWrapper book, IEnumerable<T> data, ExcelWriteSettings? settings = null);

    /// <summary>
    /// Writes the data table to the given work book
    /// </summary>
    /// <param name="book">The workbook to write to</param>
    /// <param name="data">The data to write</param>
    /// <param name="settings">The settings for the writer</param>
    void Write(ExcelWrapper book, DataTable data, ExcelWriteSettings? settings = null);

    /// <summary>
    /// Creates a workbook and writes the collection to it
    /// </summary>
    /// <typeparam name="T">The type of data to write</typeparam>
    /// <param name="data">The data to write</param>
    /// <param name="settings">The settings for the writer</param>
    /// <returns>The workbook</returns>
    ExcelWrapper Write<T>(IEnumerable<T> data, ExcelWriteSettings? settings = null);

    /// <summary>
    /// Creates a workbook and writes the data table to it
    /// </summary>
    /// <param name="data">The data to write</param>
    /// <param name="settings">The settings for the writer</param>
    /// <returns>The workbook</returns>
    ExcelWrapper Write(DataTable data, ExcelWriteSettings? settings = null);
}

internal class ExcelWriterService : IExcelWriterService
{
    /// <summary>
    /// The default settings for any excel writer method
    /// </summary>
    public static ExcelWriteSettings DefaultSettings { get; set; } = new();

    public void Write<T>(ExcelWrapper book, IEnumerable<T> data, ExcelWriteSettings? settings = null)
    {
        var writer = new ExcelWriter(book, settings ?? DefaultSettings);
        writer.WriteSheet(data);
    }

    public void Write(ExcelWrapper book, DataTable data, ExcelWriteSettings? settings = null)
    {
        var writer = new ExcelWriter(book, settings ?? DefaultSettings);
        writer.WriteSheet(data);
    }

    public ExcelWrapper Write<T>(IEnumerable<T> data, ExcelWriteSettings? settings = null)
    {
        var book = new ExcelWrapper();
        var writer = new ExcelWriter(book, settings ?? DefaultSettings);
        writer.WriteSheet(data);
        return book;
    }

    public ExcelWrapper Write(DataTable data, ExcelWriteSettings? settings = null)
    {
        var book = new ExcelWrapper();
        var writer = new ExcelWriter(book, settings ?? DefaultSettings);
        writer.WriteSheet(data);
        return book;
    }
}