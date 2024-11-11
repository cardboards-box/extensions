using System.Security.Cryptography;

namespace CardboardBox.Extensions;

/// <summary>
/// Extensions related to hashing and encryption.
/// </summary>
public static class HashExtensions
{
    /// <summary>
    /// The default encoding to use for hash operations
    /// </summary>
    public static Encoding DefaultEncoding { get; set; } = StringExtensions.DefaultEncoding;

    /// <summary>
    /// Hashes the given value using SHA512
    /// </summary>
    /// <param name="value">The value to hash</param>
    /// <returns>The SHA512 hex hash</returns>
    public static string SHA512Hash(this byte[] value)
    {
#if NET7_0_OR_GREATER
        return SHA512.HashData(value).ToHex();
#else
        using var sha = SHA512.Create();
        return sha.ComputeHash(value).ToHex();
#endif
    }

    /// <summary>
    /// Hashes the given value using SHA384
    /// </summary>
    /// <param name="value">The value to hash</param>
    /// <returns>The SHA384 hex hash</returns>
    public static string SHA384Hash(this byte[] value)
    {
#if NET7_0_OR_GREATER
        return SHA384.HashData(value).ToHex();
#else
        using var sha = SHA384.Create();
        return sha.ComputeHash(value).ToHex();
#endif
    }

    /// <summary>
    /// Hashes the given value using SHA256
    /// </summary>
    /// <param name="value">The value to hash</param>
    /// <returns>The SHA256 hex hash</returns>
    public static string SHA256Hash(this byte[] value)
    {
#if NET7_0_OR_GREATER
        return SHA256.HashData(value).ToHex();
#else
        using var sha = SHA256.Create();
        return sha.ComputeHash(value).ToHex();
#endif
    }

    /// <summary>
    /// Hashes the given value using SHA1
    /// </summary>
    /// <param name="value">The value to hash</param>
    /// <returns>The SHA1 hex hash</returns>
    public static string SHA1Hash(this byte[] value)
    {
#if NET7_0_OR_GREATER
        return SHA1.HashData(value).ToHex();
#else
        using var sha = SHA1.Create();
        return sha.ComputeHash(value).ToHex();
#endif
    }

    /// <summary>
    /// Hashes the given value using MD5
    /// </summary>
    /// <param name="value">The value to hash</param>
    /// <returns>The MD5 hex hash</returns>
    public static string MD5Hash(this byte[] value)
    {
#if NET7_0_OR_GREATER
        return MD5.HashData(value).ToHex();
#else
        using var sha = MD5.Create();
        return sha.ComputeHash(value).ToHex();
#endif
    }

    /// <summary>
    /// Hashes the given value using SHA512
    /// </summary>
    /// <param name="value">The value to hash</param>
    /// <param name="encoding">The encoding to use (defaults to <see cref="DefaultEncoding"/>)</param>
    /// <returns>The SHA512 hex hash</returns>
    public static string SHA512Hash(this string value, Encoding? encoding = null)
    {
        return value.GetBytes(encoding).SHA512Hash();
    }

    /// <summary>
    /// Hashes the given value using SHA384
    /// </summary>
    /// <param name="value">The value to hash</param>
    /// <param name="encoding">The encoding to use (defaults to <see cref="DefaultEncoding"/>)</param>
    /// <returns>The SHA384 hex hash</returns>
    public static string SHA384Hash(this string value, Encoding? encoding = null)
    {
        return value.GetBytes(encoding).SHA384Hash();
    }

    /// <summary>
    /// Hashes the given value using SHA256
    /// </summary>
    /// <param name="value">The value to hash</param>
    /// <param name="encoding">The encoding to use (defaults to <see cref="DefaultEncoding"/>)</param>
    /// <returns>The SHA256 hex hash</returns>
    public static string SHA256Hash(this string value, Encoding? encoding = null)
    {
        return value.GetBytes(encoding).SHA256Hash();
    }

    /// <summary>
    /// Hashes the given value using SHA1
    /// </summary>
    /// <param name="value">The value to hash</param>
    /// <param name="encoding">The encoding to use (defaults to <see cref="DefaultEncoding"/>)</param>
    /// <returns>The SHA1 hex hash</returns>
    public static string SHA1Hash(this string value, Encoding? encoding = null)
    {
        return value.GetBytes(encoding).SHA1Hash();
    }

    /// <summary>
    /// Hashes the given value using MD5
    /// </summary>
    /// <param name="value">The value to hash</param>
    /// <param name="encoding">The encoding to use (defaults to <see cref="DefaultEncoding"/>)</param>
    /// <returns>The MD5 hex hash</returns>
    public static string MD5Hash(this string value, Encoding? encoding = null)
    {
        return value.GetBytes(encoding).MD5Hash();
    }
}
