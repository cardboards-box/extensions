namespace CardboardBox.Extensions.Excel;

/// <summary>
/// Stream to allow for writing the Excel Document to a stream
/// </summary>
public class ExcelStream : MemoryStream
{
    /// <summary>
    /// Override the systems ability to close a stream (for writing purposes)
    /// </summary>
    public bool AllowClose { get; set; } = false;

    /// <summary>
    /// Overriding the close method to stop closing if not allowed
    /// </summary>
    public override void Close()
    {
        if (AllowClose)
            base.Close();
    }
}