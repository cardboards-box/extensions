namespace CardboardBox.Extensions.Requesting;

/// <summary>
/// Represents the base return result for all API calls
/// </summary>
public class Boxed
{
    /// <summary>
    /// The error message for 500 errors
    /// </summary>
    public static string ErrorMessage500 { get; set; } = "500 - An error occurred";

    /// <summary>
    /// The error message for 404 errors
    /// </summary>
    public static string ErrorMessage404 { get; set; } = "404 - Something is missing";

    /// <summary>
    /// The error message for 401 errors
    /// </summary>
    public static string ErrorMessage401 { get; set; } = "401 - Unauthorized";

    /// <summary>
    /// The error message for 400 errors
    /// </summary>
    public static string ErrorMessage400 { get; set; } = "400 - User input is bad";

    /// <summary>
    /// The error message for 409 errors
    /// </summary>
    public static string ErrorMessage409 { get; set; } = "409 - Already exists";

    /// <summary>
    /// Result was successful and contained no data
    /// </summary>
    public const string OK = "ok";

    /// <summary>
    /// Result was an error
    /// </summary>
    public const string ERROR = "error";

    /// <summary>
    /// Result was successful and contained data
    /// </summary>
    public const string DATA = "data";

    /// <summary>
    /// Result was successful and contained a collection of data
    /// </summary>
    public const string ARRAY = "array";

    /// <summary>
    /// Result was successful and contained a paged collection of data
    /// </summary>
    public const string PAGED = "paged";

    /// <summary>
    /// Request ID (useful for debugging)
    /// </summary>
    [JsonPropertyName("requestId")]
    public Guid RequestId { get; set; }

    /// <summary>
    /// The boxed code for the result
    /// </summary>
    [JsonPropertyName("type")]
    public string Type { get; set; } = OK;

    /// <summary>
    /// The HTTP status code for the result
    /// </summary>
    [JsonPropertyName("code")]
    public int Code { get; set; }

    /// <summary>
    /// A brief description of the error
    /// </summary>
    [JsonPropertyName("description")]
    public string? Description { get; set; }

    /// <summary>
    /// Any issues that occurred
    /// </summary>
    [JsonPropertyName("errors")]
    public string[]? Errors { get; set; }

    /// <summary>
    /// Whether or not the result was successful
    /// </summary>
    [JsonIgnore]
    public bool Success => Code >= 200 && Code < 300;

    /// <summary>
    /// The number of milliseconds the request took to complete
    /// </summary>
    [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
    public double? Elapsed { get; set; }

    /// <summary>
    /// Represents the base return result for all API calls
    /// </summary>
    [JsonConstructor]
    public Boxed() { }

    /// <summary>
    /// Represents the base return result for all API calls
    /// </summary>
    /// <param name="code">The HTTP status code result</param>
    /// <param name="type">The type of result</param>
    public Boxed(int code, string? type = null)
    {
        Code = code;

        if (!string.IsNullOrWhiteSpace(type))
        {
            Type = type;
            return;
        }

        Type = code >= 200 && code < 300 ? OK : ERROR;
    }

    /// <summary>
    /// Represents the base return result for all API calls
    /// </summary>
    /// <param name="code">The HTTP status code</param>
    /// <param name="type">The type of result</param>
    public Boxed(HttpStatusCode code, string? type = null) : this((int)code, type) { }

    /// <summary>
    /// Result was successful and contained no data
    /// </summary>
    /// <returns>The returned result</returns>
    public static Boxed Ok() => new(HttpStatusCode.OK);

    /// <summary>
    /// Result was successful and contained data
    /// </summary>
    /// <typeparam name="T">The type of data</typeparam>
    /// <param name="data">The result data</param>
    /// <returns>The returned result</returns>
    public static Boxed<T> Ok<T>(T data) => new(data);

    /// <summary>
    /// Result was successful and contained a collection of data
    /// </summary>
    /// <typeparam name="T">The type of data</typeparam>
    /// <param name="data">The result data</param>
    /// <returns>The returned result</returns>
    public static BoxedArray<T> Ok<T>(params T[] data) => new(data);

    /// <summary>
    /// Result was successful and contained a collection of data
    /// </summary>
    /// <typeparam name="T">The type of data</typeparam>
    /// <param name="data">The result data</param>
    /// <returns>The returned result</returns>
    public static BoxedArray<T> Ok<T>(IEnumerable<T> data) => new(data.ToArray());

    /// <summary>
    /// Result was successful and contained a collection of data
    /// </summary>
    /// <typeparam name="T">The type of data</typeparam>
    /// <param name="data">The result data</param>
    /// <returns>The returned result</returns>
    public static BoxedArray<T> Ok<T>(List<T> data) => new([.. data]);

    /// <summary>
    /// Result was successful and contained a collection of paged data
    /// </summary>
    /// <typeparam name="T">The type of data</typeparam>
    /// <param name="pages">The number of pages in the result</param>
    /// <param name="total">The total number of results</param>
    /// <param name="data">The result data</param>
    /// <returns>The returned result</returns>
    public static BoxedPaged<T> Ok<T>(int pages, int total, params T[] data)
    {
        return new BoxedPaged<T>(data, pages, total);
    }

    /// <summary>
    /// Result was successful and contained a collection of paged data
    /// </summary>
    /// <typeparam name="T">The type of data</typeparam>
    /// <param name="pages">The number of pages in the result</param>
    /// <param name="total">The total number of results</param>
    /// <param name="data">The result data</param>
    /// <returns>The returned result</returns>
    public static BoxedPaged<T> Ok<T>(int pages, int total, IEnumerable<T> data)
    {
        return new BoxedPaged<T>(data.ToArray(), pages, total);
    }

    /// <summary>
    /// Result was successful and contained a collection of paged data
    /// </summary>
    /// <typeparam name="T">The type of data</typeparam>
    /// <param name="pages">The number of pages in the result</param>
    /// <param name="total">The total number of results</param>
    /// <param name="data">The result data</param>
    /// <returns>The returned result</returns>
    public static BoxedPaged<T> Ok<T>(int pages, int total, List<T> data)
    {
        return new BoxedPaged<T>([.. data], pages, total);
    }

    /// <summary>
    /// Creates an error result
    /// </summary>
    /// <param name="code">The status code for the result</param>
    /// <param name="description">The description of what happened</param>
    /// <param name="errors">The error messages included in the response</param>
    /// <returns>The error result</returns>
    public static Boxed Error(HttpStatusCode code, string description, params string[] errors)
    {
        return new Boxed
        {
            Code = (int)code,
            Type = ERROR,
            Description = description,
            Errors = errors
        };
    }

    /// <summary>
    /// Creates an error result
    /// </summary>
    /// <param name="code">The status code for the result</param>
    /// <param name="description">The description of what happened</param>
    /// <param name="errors">The error messages included in the response</param>
    /// <returns>The error result</returns>
    public static Boxed Error(HttpStatusCode code, string description, params Exception[] errors)
    {
        return new Boxed
        {
            Code = (int)code,
            Type = ERROR,
            Description = description,
            Errors = [..errors.Select(e => e.Message)]
        };
    }

    /// <summary>
    /// Creates an error result
    /// </summary>
    /// <param name="code">The status code for the result</param>
    /// <param name="description">The description of what happened</param>
    /// <param name="errors">The error messages included in the response</param>
    /// <returns>The error result</returns>
    public static Boxed<T> Error<T>(HttpStatusCode code, string description, params string[] errors)
    {
        return new Boxed<T>
        {
            Code = (int)code,
            Type = ERROR,
            Description = description,
            Errors = errors
        };
    }

    /// <summary>
    /// Creates an error result
    /// </summary>
    /// <param name="code">The status code for the result</param>
    /// <param name="description">The description of what happened</param>
    /// <param name="errors">The error messages included in the response</param>
    /// <returns>The error result</returns>
    public static Boxed<T> Error<T>(HttpStatusCode code, string description, params Exception[] errors)
    {
        return new Boxed<T>
        {
            Code = (int)code,
            Type = ERROR,
            Description = description,
            Errors = [.. errors.Select(e => e.Message)]
        };
    }

    /// <summary>
    /// An exception occurred
    /// </summary>
    /// <param name="errors">The error(s) that occurred</param>
    /// <returns>The returned error result</returns>
    public static Boxed Exception(params string[] errors)
    {
        return Error(HttpStatusCode.InternalServerError, ErrorMessage500, errors);
    }

    /// <summary>
    /// An exception occurred
    /// </summary>
    /// <param name="exceptions">The error(s) that occurred</param>
    /// <returns>The returned error result</returns>
    public static Boxed Exception(params Exception[] exceptions)
    {
        return Exception(exceptions.Select(e => e.Message).ToArray());
    }

    /// <summary>
    /// An exception occurred
    /// </summary>
    /// <typeparam name="T">The type of data that should be present</typeparam>
    /// <param name="errors">The error(s) that occurred</param>
    /// <returns>The returned error result</returns>
    public static Boxed<T> Exception<T>(params string[] errors)
    {
        return Error<T>(HttpStatusCode.InternalServerError, ErrorMessage500, errors);
    }

    /// <summary>
    /// An exception occurred
    /// </summary>
    /// <typeparam name="T">The type of data that should be present</typeparam>
    /// <param name="exceptions">The error(s) that occurred</param>
    /// <returns>The returned error result</returns>
    public static Boxed<T> Exception<T>(params Exception[] exceptions)
    {
        return Exception<T>(exceptions.Select(e => e.Message).ToArray());
    }

    /// <summary>
    /// Something is missing
    /// </summary>
    /// <param name="resource">The resource that was missing</param>
    /// <param name="errors">Any other issues that occurred</param>
    /// <returns>The returned error result</returns>
    public static Boxed NotFound(string resource, params string[] errors)
    {
        return Error(HttpStatusCode.NotFound, ErrorMessage404, [$"The requested resource '{resource}' was not found", .. errors]);
    }

    /// <summary>
    /// Something is missing
    /// </summary>
    /// <typeparam name="T">The type of data that should be present</typeparam>
    /// <param name="resource">The resource that was missing</param>
    /// <param name="errors">Any other issues that occurred</param>
    /// <returns>The returned error result</returns>
    public static Boxed<T> NotFound<T>(string resource, params string[] errors)
    {
        return Error<T>(HttpStatusCode.NotFound, ErrorMessage404, [$"The requested resource '{resource}' was not found", .. errors]);
    }

    /// <summary>
    /// User is unauthorized
    /// </summary>
    /// <param name="issues">Any issues that occurred that caused the unauthorized error</param>
    /// <returns>The returned error result</returns>
    public static Boxed Unauthorized(params string[] issues)
    {
        return Error(HttpStatusCode.Unauthorized, ErrorMessage401, ["You are not authorized to access this resource", .. issues]);
    }

    /// <summary>
    /// User is unauthorized
    /// </summary>
    /// <typeparam name="T">The type of data that should be present</typeparam>
    /// <param name="issues">Any issues that occurred that caused the unauthorized error</param>
    /// <returns>The returned error result</returns>
    public static Boxed<T> Unauthorized<T>(params string[] issues)
    {
        return Error<T>(HttpStatusCode.Unauthorized, ErrorMessage401, ["You are not authorized to access this resource", .. issues]);
    }

    /// <summary>
    /// Something the user did was bad
    /// </summary>
    /// <param name="errors">The different errors</param>
    /// <returns>The returned error result</returns>
    public static Boxed Bad(params string[] errors)
    {
        return Error(HttpStatusCode.BadRequest, ErrorMessage400, errors);
    }

    /// <summary>
    /// Something the user did was bad
    /// </summary>
    /// <typeparam name="T">The type of data that should be present</typeparam>
    /// <param name="errors">The different errors</param>
    /// <returns>The returned error result</returns>
    public static Boxed<T> Bad<T>(params string[] errors)
    {
        return Error<T>(HttpStatusCode.BadRequest, ErrorMessage400, errors);
    }

    /// <summary>
    /// Resource already exists
    /// </summary>
    /// <param name="resource">The resource that was conflicting</param>
    /// <returns>The result of the request</returns>
    public static Boxed Conflict(string resource)
    {
        return Error(HttpStatusCode.Conflict, ErrorMessage409, $"The resource '{resource}' already exists");
    }

    /// <summary>
    /// Resource already exists
    /// </summary>
    /// <typeparam name="T">The type of data that should be present</typeparam>
    /// <param name="resource">The resource that was conflicting</param>
    /// <returns>The result of the request</returns>
    public static Boxed<T> Conflict<T>(string resource)
    {
        return Error<T>(HttpStatusCode.Conflict, ErrorMessage409, $"The resource '{resource}' already exists");
    }
}
