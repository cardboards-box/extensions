namespace CardboardBox.Extensions;

/// <summary>
/// Extensions for <see cref="string"/> objects
/// </summary>
public static partial class StringExtensions
{
    /// <summary>
    /// The default encoding to use for string operations
    /// </summary>
    public static Encoding DefaultEncoding { get; set; } = Encoding.UTF8;

    /// <summary>
    /// The default string comparison to use for string operations
    /// </summary>
    public static StringComparison DefaultIgnoreComparison { get; set; } = StringComparison.InvariantCultureIgnoreCase;

    /// <summary>
    /// Gets the string representation of a byte array
    /// </summary>
    /// <param name="bytes">The value to turn into a string</param>
    /// <param name="encoding">The encoding to use (defaults to <see cref="DefaultEncoding"/>)</param>
    /// <returns>The string representation of the byte array</returns>
    public static string GetString(this byte[] bytes, Encoding? encoding = null)
    {
        return (encoding ?? DefaultEncoding).GetString(bytes);
    }

    /// <summary>
    /// Gets the string representation of a byte array
    /// </summary>
    /// <param name="bytes">The value to turn into a string</param>
    /// <param name="index">The index to start at</param>
    /// <param name="count">The number of value to read </param>
    /// <param name="encoding">The encoding to use (defaults to <see cref="DefaultEncoding"/>)</param>
    /// <returns>The string representation of the byte array</returns>
    public static string GetString(this byte[] bytes, int index, int count, Encoding? encoding = null)
    {
        return (encoding ?? DefaultEncoding).GetString(bytes, index, count);
    }

    /// <summary>
    /// Gets the byte array representation of a string
    /// </summary>
    /// <param name="input">The value string</param>
    /// <param name="encoding">The encoding to use (defaults to <see cref="DefaultEncoding"/>)</param>
    /// <returns>The byte array representation of the string</returns>
    public static byte[] GetBytes(this string input, Encoding? encoding = null)
    {
        return (encoding ?? DefaultEncoding).GetBytes(input);
    }

    /// <summary>
    /// Gets the Base64 representation of a byte array
    /// </summary>
    /// <param name="bytes">The value to convert to a Base 64 string</param>
    /// <returns>The base 64 string representation of the byte array</returns>
    public static string ToBase64(this byte[] bytes) => Convert.ToBase64String(bytes);

    /// <summary>
    /// Gets the byte array representation of a Base64 string
    /// </summary>
    /// <param name="input">The base 64 string</param>
    /// <returns>The byte array converted from the base 64 string</returns>
    public static byte[] FromBase64(this string input) => Convert.FromBase64String(input);

    /// <summary>
    /// Converts the given byte array to a hex string
    /// </summary>
    /// <param name="bytes">The value to convert</param>
    /// <returns>The hex value</returns>
    public static string ToHex(this byte[] bytes)
    {
        return BitConverter.ToString(bytes).Replace("-", "");
    }

    /// <summary>
    /// Forces a string to be null if it is empty or whitespace
    /// </summary>
    /// <param name="input">The value string</param>
    /// <returns>Null if the value is empty or whitespace, or the value if not</returns>
    public static string? ForceNull(this string? input) => string.IsNullOrWhiteSpace(input) ? null : input;

    /// <summary>
    /// Checks to see if two strings are equal, ignoring case
    /// </summary>
    /// <param name="first">The first value to check</param>
    /// <param name="second">The second value to check</param>
    /// <returns>Whether or not the two strings are equal</returns>
    public static bool EqualsIc(this string? first, string? second)
    {
        if (string.IsNullOrEmpty(first) && string.IsNullOrEmpty(second))
            return true;

        if (string.IsNullOrEmpty(first) || string.IsNullOrEmpty(second))
            return false;

        return first.Equals(second, DefaultIgnoreComparison);
    }

    /// <summary>
    /// Checks to see if the first string contains the second string, ignoring case
    /// </summary>
    /// <param name="first">The first value to check</param>
    /// <param name="second">The second value to check</param>
    /// <returns>Whether or not the first string contains the second</returns>
    public static bool ContainsIc(this string? first, string? second)
    {
        if (string.IsNullOrEmpty(first) && string.IsNullOrEmpty(second))
            return true;

        if (string.IsNullOrEmpty(first) || string.IsNullOrEmpty(second))
            return false;

        return first.Contains(second, DefaultIgnoreComparison);
    }

    /// <summary>
    /// Checks to see if the first string starts with the second string, ignoring case
    /// </summary>
    /// <param name="first">The first value to check</param>
    /// <param name="second">The second value to check</param>
    /// <returns>Whether or not the first string starts with the second</returns>
    public static bool StartsWithIc(this string? first, string? second)
    {
        if (string.IsNullOrEmpty(first) && string.IsNullOrEmpty(second))
            return true;

        if (string.IsNullOrEmpty(first) || string.IsNullOrEmpty(second))
            return false;

        return first.StartsWith(second, DefaultIgnoreComparison);
    }

    /// <summary>
    /// Checks to see if the first string ends with the second string, ignoring case
    /// </summary>
    /// <param name="first">The first value to check</param>
    /// <param name="second">The second value to check</param>
    /// <returns>Whether or not the first string ends with the second</returns>
    public static bool EndsWithIc(this string? first, string? second)
    {
        if (string.IsNullOrEmpty(first) && string.IsNullOrEmpty(second))
            return true;

        if (string.IsNullOrEmpty(first) || string.IsNullOrEmpty(second))
            return false;

        return first.EndsWith(second, DefaultIgnoreComparison);
    }

    /// <summary>
    /// Determines whether or not the given string is comprised only of white space characters.
    /// This includes the zero-width character
    /// </summary>
    /// <param name="value">The string to check</param>
    /// <returns>Whether or not the given string is only comprised of white-space characters.</returns>
    public static bool IsWhiteSpace(this string? value)
    {
        static bool isWs(char c) => char.IsWhiteSpace(c) || c == '\u00A0';
        if (value == null || value.Length == 0) return true;

        for (var i = 0; i < value.Length; i++)
            if (!isWs(value[i])) return false;

        return true;
    }

    /// <summary>
    /// Converts the given string to snake_case (Snake Case -> snake_case)
    /// </summary>
    /// <param name="text">The text to convert</param>
    /// <returns>The snake_case version of the given string</returns>
    public static string? ToSnakeCase(this string? text)
    {
        if (string.IsNullOrEmpty(text)) return text;
        if (text.Length < 2) return text;

        var sb = new StringBuilder();
        sb.Append(char.ToLowerInvariant(text[0]));
        for (int i = 1; i < text.Length; ++i)
        {
            char c = text[i];
            if (!char.IsUpper(c))
            {
                sb.Append(c);
                continue;
            }

            sb.Append('_');
            sb.Append(char.ToLowerInvariant(c));
        }

        return sb.ToString();
    }

    /// <summary>
    /// Converts the given string to pascalCase (PascalCase -> pascalCase)
    /// </summary>
    /// <param name="text">The text to convert</param>
    /// <returns>The pascalCase version of the give string</returns>
    public static string? ToPascalCase(this string? text)
    {
        if (string.IsNullOrEmpty(text)) return text;

        var chars = text.ToCharArray();
        chars[0] = char.ToLowerInvariant(chars[0]);
        return new string(chars);
    }

    /// <summary>
    /// Removes all characters that are invalid for paths and file names.
    /// These characters are sourced from: <see cref="Path.GetInvalidFileNameChars"/> and <see cref="Path.GetInvalidPathChars"/>
    /// </summary>
    /// <param name="text">The string to sanitize</param>
    /// <returns>The sanitized string</returns>
    public static string PurgePathChars(this string text)
    {
        var chars = Path.GetInvalidFileNameChars()
            .Union(Path.GetInvalidPathChars())
            .ToArray();

        foreach (var c in chars)
            text = text.Replace(c.ToString(), "");

        return text;
    }

    /// <summary>
    /// A short hand for <see cref="string.Join{T}(string, IEnumerable{T})"/>"/>
    /// </summary>
    /// <typeparam name="T">The type of the collection</typeparam>
    /// <param name="input">The collection to join</param>
    /// <param name="joiner">The value to use to join the string. Defaults to a space</param>
    /// <returns>The joined string</returns>
    public static string StringJoin<T>(this IEnumerable<T> input, string? joiner = null)
    {
        return string.Join(joiner ?? " ", input);
    }

    /// <summary>
    /// Splits the given string by various new line characters
    /// </summary>
    /// <param name="input">The string to split</param>
    /// <param name="options">The options for splitting the string</param>
    /// <returns>The lines in the files</returns>
    public static string[] SplitLines(this string? input, StringSplitOptions options = StringSplitOptions.None)
    {
        return input?.Split(["\r\n", "\n", "\r"], options) ?? [];
    }

    /// <summary>
    /// Splits the given string by the given max length
    /// </summary>
    /// <param name="content">The string to split</param>
    /// <param name="maxLength">The max length for each string segment</param>
    /// <param name="newline">The character to use for new-lines (defaults to <see cref="Environment.NewLine"/>)</param>
    /// <returns>The split chunks of the string</returns>
    public static IEnumerable<string> SplitByLength(this string content, int maxLength = 2000, string? newline = null)
    {
        newline ??= Environment.NewLine;

        if (string.IsNullOrEmpty(content) || content.Length <= maxLength)
        {
            yield return content;
            yield break;
        }

        using var r = new StringReader(content);
        string current = "";
        while (true)
        {
            var line = r.ReadLine();
            if (line == null)
            {
                if (!string.IsNullOrEmpty(current))
                    yield return current;
                yield break;
            }

            var combinedLength = (current + newline + line).Length;
            if (combinedLength > maxLength)
            {
                yield return current;
                current = line;
                continue;
            }

            current += newline + line;
        }
    }

    /// <summary>
    /// Safely executes <see cref="string.Substring(int, int)"/> capping the result at the length of the string
    /// </summary>
    /// <param name="text">The text to substring</param>
    /// <param name="length">How many character to fetch</param>
    /// <param name="start">Where to start the sub-string</param>
    /// <returns>The substring</returns>
    public static string SafeSubString(this string text, int length, int start = 0)
    {
        if (start + length > text.Length)
            return text[start..];

        return text.Substring(start, length);
    }

    /// <summary>
    /// Determines the line endings of a string
    /// </summary>
    /// <param name="input">The string to scan</param>
    /// <returns>The type of line endings</returns>
    public static LineEndings DetermineLineEndings(this string? input)
    {
        if (string.IsNullOrEmpty(input))
            return LineEndings.Unknown;

        if (input.Contains("\r\n")) return LineEndings.CarriageReturnLineFeed;
        if (input.Contains('\n')) return LineEndings.LineFeed;
        if (input.Contains('\r')) return LineEndings.CarriageReturn;
        return LineEndings.Unknown;
    }

    /// <summary>
    /// Strips any punctuation from the string
    /// </summary>
    /// <param name="input">The value string</param>
    /// <returns>The string with punctuation stripped out</returns>
    public static string StripPunctuation(this string input)
    {
        return new string(input.Where(c => !char.IsPunctuation(c)).ToArray());
    }

    /// <summary>
    /// Gets all of the words from the given string, stripping punctuation
    /// </summary>
    /// <param name="input">The value string</param>
    /// <param name="lower">Whether or not to run <see cref="string.ToLowerInvariant"/></param>
    /// <param name="splitter">The character to split by</param>
    /// <returns>The words in the string</returns>
    public static string[] Words(this string? input, bool lower = true, char splitter = ' ')
    {
        if (string.IsNullOrWhiteSpace(input))
            return [];

        if (lower) input = input.ToLowerInvariant();
        return input.StripPunctuation().Split(splitter, StringSplitOptions.RemoveEmptyEntries);
    }

    /// <summary>
    /// Converts the string representation of the name or numeric value of one or more enumerated constants to an equivalent enumerated object.
    /// </summary>
    /// <typeparam name="T">An enumeration type.</typeparam>
    /// <param name="value">A string containing the name or value to convert.</param>
    /// <param name="ignoreCase">true to ignore case; false to regard case.</param>
    /// <returns>An object of the specified enumeration type whose value is represented by value.</returns>
    /// <exception cref="ArgumentNullException">The value parameter is null.</exception>
    /// <exception cref="ArgumentException">The specified type is not an enumeration type, or value is either an empty string or only contains  white space, or value is a name, but not one of the named constants.</exception>
    public static T ParseEnum<T>(this string value, bool ignoreCase = true) where T :
        struct, IComparable, IFormattable, IConvertible
    {
        if (string.IsNullOrEmpty(value)) throw new ArgumentNullException(nameof(value));
        if (!typeof(T).IsEnum) throw new ArgumentException("T must be an enumerated type.");
        return Enum.Parse<T>(value, ignoreCase);
    }

    /// <summary>
    /// Strips all non-alphanumeric characters from a string (excludes spaces)
    /// </summary>
    /// <param name="input">The string to strip</param>
    /// <returns>Only the alpha-numeric characters</returns>
    public static string? StripNonAlphaNumeric(this string? input)
    {
        if (string.IsNullOrEmpty(input)) return input;
        return NonAlphaNumeric().Replace(input, string.Empty);
    }

#if NET7_0_OR_GREATER
    [GeneratedRegex("[^a-zA-Z0-9 ]")]
    private static partial Regex NonAlphaNumeric();
#else
    private static Regex? _nonAlphaNumeric;
    private static Regex NonAlphaNumeric() => _nonAlphaNumeric ??= new("[^a-zA-Z0-9 ]");
#endif
}
