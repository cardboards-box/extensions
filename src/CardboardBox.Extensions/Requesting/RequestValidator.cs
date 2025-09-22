namespace CardboardBox.Extensions.Requesting;

/// <summary>
/// Provides a fluent interface for validating request parameters and collecting validation issues.
/// </summary>
public class RequestValidator
{
    /// <summary>
    /// Stores validation issues encountered during validation checks.
    /// </summary>
    public List<string> Issues { get; set; } = [];

    /// <summary>
    /// Returns true if there are no validation issues; otherwise, false.
    /// </summary>
    public bool Valid => Issues.Count == 0;

    /// <summary>
    /// Executes a custom validation function and records a message if the result is false.
    /// </summary>
    /// <param name="result">A function that returns a boolean result indicating validity.</param>
    /// <param name="message">The message to add if validation fails.</param>
    /// <returns>The current <see cref="RequestValidator"/> instance.</returns>
    public RequestValidator Check(Func<bool> result, string message)
    {
        if (!result()) Issues.Add(message);
        return this;
    }

    /// <summary>
    /// Checks a boolean result and records a message if the result is false.
    /// </summary>
    /// <param name="result">Boolean result of a validation condition.</param>
    /// <param name="message">The message to add if validation fails.</param>
    /// <returns>The current <see cref="RequestValidator"/> instance.</returns>
    public RequestValidator Check(bool result, string message)
    {
        if (!result) Issues.Add(message);
        return this;
    }

    /// <summary>
    /// Validates that an object is not null.
    /// </summary>
    /// <param name="obj">The object to check.</param>
    /// <param name="property">The name of the property being validated.</param>
    /// <returns>The current <see cref="RequestValidator"/> instance.</returns>
    public RequestValidator NotNull(object? obj, string property)
    {
        if (obj is null) Issues.Add($"{property} cannot be null");
        return this;
    }

    /// <summary>
    /// Validates that a string is not null or empty.
    /// </summary>
    /// <param name="obj">The string to check.</param>
    /// <param name="property">The name of the property being validated.</param>
    /// <returns>The current <see cref="RequestValidator"/> instance.</returns>
    public RequestValidator NotNull(string? obj, string property)
    {
        if (string.IsNullOrEmpty(obj)) Issues.Add($"{property} cannot be null or empty");
        return this;
    }

    /// <summary>
    /// Validates that a string is not null, empty, or whitespace.
    /// </summary>
    /// <param name="obj">The string to check.</param>
    /// <param name="property">The name of the property being validated.</param>
    /// <returns>The current <see cref="RequestValidator"/> instance.</returns>
    public RequestValidator NotWhitespace(string? obj, string property)
    {
        if (string.IsNullOrWhiteSpace(obj)) Issues.Add($"{property} cannot be null or whitespace");
        return this;
    }

    /// <summary>
    /// Validates that a value is one of a set of allowed options.
    /// </summary>
    /// <param name="value">The value to check.</param>
    /// <param name="property">The name of the property being validated.</param>
    /// <param name="options">The allowed values.</param>
    /// <returns>The current <see cref="RequestValidator"/> instance.</returns>
    public RequestValidator OneOf(string? value, string property, params string[] options)
    {
        if (!options.Contains(value)) Issues.Add($"{property} must be one of {string.Join(", ", options)}");
        return this;
    }

    /// <summary>
    /// Validates that a string represents a value in a specified enum type.
    /// </summary>
    /// <typeparam name="T">The enum type to validate against.</typeparam>
    /// <param name="value">The string value to parse and validate.</param>
    /// <param name="property">The name of the property being validated.</param>
    /// <param name="res">The parsed enum value, if valid.</param>
    /// <returns>The current <see cref="RequestValidator"/> instance.</returns>
    public RequestValidator IsEnum<T>(string value, string property, out T res) where T : struct, Enum
    {
        if (Enum.TryParse(value, true, out res)) return this;

        Issues.Add($"{property} must be one of {string.Join(", ", Enum.GetNames(typeof(T)))}");
        return this;
    }

    /// <summary>
    /// Validates that a string represents a value in a specified enum type.
    /// </summary>
    /// <param name="value">The string value to parse and validate.</param>
    /// <param name="property">The name of the property being validated.</param>
    /// <returns>The current <see cref="RequestValidator"/> instance.</returns>
    public RequestValidator IsEnum<T>(string value, string property) where T : struct, Enum
    {
        return IsEnum<T>(value, property, out _);
    }

    /// <summary>
    /// Validates that a numeric value is greater than a specified minimum.
    /// </summary>
    /// <param name="value">The value to check.</param>
    /// <param name="property">The name of the property being validated.</param>
    /// <param name="min">The minimum allowed value.</param>
    /// <returns>The current <see cref="RequestValidator"/> instance.</returns>
    public RequestValidator GreaterThan(int value, string property, int min)
    {
        if (value > min) return this;

        Issues.Add($"{property} must be greater than {min}");
        return this;
    }

    /// <summary>
    /// Validates that a numeric value is greater than a specified minimum.
    /// </summary>
    /// <param name="value">The value to check.</param>
    /// <param name="property">The name of the property being validated.</param>
    /// <param name="min">The minimum allowed value.</param>
    /// <returns>The current <see cref="RequestValidator"/> instance.</returns>
    public RequestValidator GreaterThan(long value, string property, long min)
    {
        if (value > min) return this;

        Issues.Add($"{property} must be greater than {min}");
        return this;
    }

    /// <summary>
    /// Validates that a numeric value is greater than a specified minimum.
    /// </summary>
    /// <param name="value">The value to check.</param>
    /// <param name="property">The name of the property being validated.</param>
    /// <param name="min">The minimum allowed value.</param>
    /// <returns>The current <see cref="RequestValidator"/> instance.</returns>
    public RequestValidator GreaterThan(float value, string property, float min)
    {
        if (value > min) return this;

        Issues.Add($"{property} must be greater than {min}");
        return this;
    }

    /// <summary>
    /// Validates that a numeric value is greater than a specified minimum.
    /// </summary>
    /// <param name="value">The value to check.</param>
    /// <param name="property">The name of the property being validated.</param>
    /// <param name="min">The minimum allowed value.</param>
    /// <returns>The current <see cref="RequestValidator"/> instance.</returns>
    public RequestValidator GreaterThan(double value, string property, double min)
    {
        if (value > min) return this;

        Issues.Add($"{property} must be greater than {min}");
        return this;
    }

    /// <summary>
    /// Validates that a numeric value is less than a specified maximum.
    /// </summary>
    /// <param name="value">The value to check.</param>
    /// <param name="property">The name of the property being validated.</param>
    /// <param name="max">The maximum allowed value.</param>
    /// <returns>The current <see cref="RequestValidator"/> instance.</returns>
    public RequestValidator LessThan(int value, string property, int max)
    {
        if (value < max) return this;

        Issues.Add($"{property} must be less than {max}");
        return this;
    }

    /// <summary>
    /// Validates that a numeric value is less than a specified maximum.
    /// </summary>
    /// <param name="value">The value to check.</param>
    /// <param name="property">The name of the property being validated.</param>
    /// <param name="max">The maximum allowed value.</param>
    /// <returns>The current <see cref="RequestValidator"/> instance.</returns>
    public RequestValidator LessThan(long value, string property, long max)
    {
        if (value < max) return this;

        Issues.Add($"{property} must be less than {max}");
        return this;
    }

    /// <summary>
    /// Validates that a numeric value is less than a specified maximum.
    /// </summary>
    /// <param name="value">The value to check.</param>
    /// <param name="property">The name of the property being validated.</param>
    /// <param name="max">The maximum allowed value.</param>
    /// <returns>The current <see cref="RequestValidator"/> instance.</returns>
    public RequestValidator LessThan(float value, string property, float max)
    {
        if (value < max) return this;

        Issues.Add($"{property} must be less than {max}");
        return this;
    }

    /// <summary>
    /// Validates that a numeric value is less than a specified maximum.
    /// </summary>
    /// <param name="value">The value to check.</param>
    /// <param name="property">The name of the property being validated.</param>
    /// <param name="max">The maximum allowed value.</param>
    /// <returns>The current <see cref="RequestValidator"/> instance.</returns>
    public RequestValidator LessThan(double value, string property, double max)
    {
        if (value < max) return this;

        Issues.Add($"{property} must be less than {max}");
        return this;
    }

    /// <summary>
    /// Validates that a numeric value is between a specified minimum and maximum.
    /// </summary>
    /// <param name="value">The value to check.</param>
    /// <param name="property">The name of the property being validated.</param>
    /// <param name="min">The minimum allowed value.</param>
    /// <param name="max">The maximum allowed value.</param>
    /// <returns>The current <see cref="RequestValidator"/> instance.</returns>
    public RequestValidator Between(int value, string property, int min, int max)
    {
        if (value > min && value < max) return this;

        Issues.Add($"{property} must be between {min} and {max}");
        return this;
    }

    /// <summary>
    /// Validates that a numeric value is between a specified minimum and maximum.
    /// </summary>
    /// <param name="value">The value to check.</param>
    /// <param name="property">The name of the property being validated.</param>
    /// <param name="min">The minimum allowed value.</param>
    /// <param name="max">The maximum allowed value.</param>
    /// <returns>The current <see cref="RequestValidator"/> instance.</returns>
    public RequestValidator Between(long value, string property, long min, long max)
    {
        if (value > min && value < max) return this;

        Issues.Add($"{property} must be between {min} and {max}");
        return this;
    }

    /// <summary>
    /// Validates that a numeric value is between a specified minimum and maximum.
    /// </summary>
    /// <param name="value">The value to check.</param>
    /// <param name="property">The name of the property being validated.</param>
    /// <param name="min">The minimum allowed value.</param>
    /// <param name="max">The maximum allowed value.</param>
    /// <returns>The current <see cref="RequestValidator"/> instance.</returns>
    public RequestValidator Between(float value, string property, float min, float max)
    {
        if (value > min && value < max) return this;

        Issues.Add($"{property} must be between {min} and {max}");
        return this;
    }

    /// <summary>
    /// Validates that a numeric value is between a specified minimum and maximum.
    /// </summary>
    /// <param name="value">The value to check.</param>
    /// <param name="property">The name of the property being validated.</param>
    /// <param name="min">The minimum allowed value.</param>
    /// <param name="max">The maximum allowed value.</param>
    /// <returns>The current <see cref="RequestValidator"/> instance.</returns>
    public RequestValidator Between(double value, string property, double min, double max)
    {
        if (value > min && value < max) return this;

        Issues.Add($"{property} must be between {min} and {max}");
        return this;
    }

    /// <summary>
    /// Validates that a string represents a valid GUID.
    /// </summary>
    /// <param name="value">The string to check.</param>
    /// <param name="property">The name of the property being validated.</param>
    /// <param name="res">The parsed GUID value, if valid.</param>
    /// <returns>The current <see cref="RequestValidator"/> instance.</returns>
    public RequestValidator IsGuid(string value, string property, out Guid res)
    {
        if (Guid.TryParse(value, out res)) return this;

        Issues.Add($"{property} must be a valid GUID");
        return this;
    }

    /// <summary>
    /// Validates and wraps the result in a boxed result object, returning true if valid.
    /// </summary>
    /// <param name="boxed">The resulting boxed validation outcome.</param>
    /// <returns>True if validation passed, otherwise false.</returns>
    public bool Validate(out Boxed boxed)
    {
        if (Issues.Count == 0)
        {
            boxed = Boxed.Ok();
            return true;
        }

        boxed = Boxed.Bad([..Issues]);
        return false;
    }

    /// <summary>
    /// Validates and wraps the result in a boxed result object, returning true if valid.
    /// </summary>
    /// <typeparam name="T">The type of data that should be present</typeparam>
    /// <param name="boxed">The resulting boxed validation outcome.</param>
    /// <returns>True if validation passed, otherwise false.</returns>
    public bool Validate<T>(out Boxed<T> boxed)
    {
        if (Issues.Count == 0)
        {
            boxed = Boxed.Ok(default(T)!);
            return true;
        }

        boxed = Boxed.Bad<T>([.. Issues]);
        return false;
    }
}