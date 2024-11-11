namespace CardboardBox.Extensions;

/// <summary>
/// Extensions related to streams
/// </summary>
public static class StreamExtensions
{
    /// <summary>
    /// The default encoding to use for string operations
    /// </summary>
    public static Encoding DefaultEncoding { get; set; } = StringExtensions.DefaultEncoding;

    /// <summary>
    /// Converts the given string to a stream
    /// </summary>
    /// <param name="content">The content to stream</param>
    /// <param name="encoding">The optional encoding to use (defaults to <see cref="DefaultEncoding"/>)</param>
    /// <returns>The contents in stream form</returns>
    public static Stream ToStream(this string content, Encoding? encoding = null)
    {
        var bytes = (encoding ?? DefaultEncoding).GetBytes(content);
        return ToStream(bytes);
    }

    /// <summary>
    /// Converts the given byte array to a stream
    /// </summary>
    /// <param name="content">The byte array to stream</param>
    /// <returns>The contents in stream form</returns>
    public static Stream ToStream(this byte[] content)
    {
        return new MemoryStream(content);
    }
}
