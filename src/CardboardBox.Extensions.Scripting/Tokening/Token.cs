namespace CardboardBox.Extensions.Scripting.Tokening;

/// <summary>
/// Represents a parsed token
/// </summary>
/// <param name="_content">The content of the token</param>
/// <param name="_startIndex">The start index of this specific token instance</param>
/// <param name="_length">The length of the full token</param>
/// <param name="_fullToken">The full token as found within the content</param>
public class Token(
    string _content, 
    int _startIndex, 
    int _length, 
    string _fullToken)
{
    /// <summary>
    /// The content of the token
    /// </summary>
    public string Content { get; } = _content;

    /// <summary>
    /// The start index of this specific token instance
    /// </summary>
    public int StartIndex { get; } = _startIndex;

    /// <summary>
    /// The length of the full token
    /// </summary>
    public int Length { get; } = _length;

    /// <summary>
    /// The full token as found within the content
    /// </summary>
    public string FullToken { get; } = _fullToken;

    /// <summary>
    /// Represents a parsed token
    /// </summary>
    /// <param name="content">The content of the token</param>
    /// <param name="startIndex">The start index of this specific token instance</param>
    /// <param name="length">The length of the full token</param>
    /// <param name="fullToken">The full token as found within the content</param>
    public void Deconstruct(out string content, out int startIndex, out int length, out string fullToken)
    {
        content = Content;
        startIndex = StartIndex;
        length = Length;
        fullToken = FullToken;
    }
}
