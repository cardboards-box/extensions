﻿namespace CardboardBox.Extensions.Requesting;

/// <summary>
/// Represents the result of a successful API call that returns paged data
/// </summary>
/// <typeparam name="T">The type of data</typeparam>
/// <param name="data">The results of the request</param>
/// <param name="pages">The number of pages in the result</param>
/// <param name="total">The total number of results</param>
/// <param name="code">The result code</param>
/// <param name="type">The type of result</param>
public class BoxedPaged<T>(
    T[] data, int pages, int total,
    HttpStatusCode? code = null, string? type = null)
    : BoxedArray<T>(data, code ?? HttpStatusCode.OK, type ?? PAGED)
{
    /// <summary>
    /// The number of pages in the result
    /// </summary>
    [JsonPropertyName("pages")]
    public int Pages { get; set; } = pages;

    /// <summary>
    /// The total number of results
    /// </summary>
    [JsonPropertyName("total")]
    public int Total { get; set; } = total;

    /// <summary>
    /// Represents the result of a successful API call that returns paged data
    /// </summary>
    [JsonConstructor]
    public BoxedPaged() : this([], 0, 0) { }
}