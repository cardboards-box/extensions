using System.Diagnostics.CodeAnalysis;

namespace CardboardBox.Extensions;

/// <summary>
/// Provides some helpful methods for iterating through an enumerator
/// </summary>
/// <typeparam name="T">The type of enumerator</typeparam>
/// <param name="_enumerator">The enumerator</param>
/// <param name="_default">The default value to return if nothing matches</param>
public class CollectionIterator<T>(
    IEnumerator<T> _enumerator,
    T? _default = default)
{
    /// <summary>
    /// Indicates whether we've reached the end of the collection
    /// </summary>
    public bool EndOfCollection { get; private set; } = false;

    /// <summary>
    /// Provides some helpful methods for iterating through an enumerator
    /// </summary>
    /// <param name="collection">The collection to iterate through</param>
    /// <param name="default">The default value to return if nothing matches</param>
    public CollectionIterator(IEnumerable<T> collection, T? @default = default)
        : this(collection.GetEnumerator(), @default) { }

    /// <summary>
    /// The current item in the enumerator
    /// </summary>
    public T Current => _enumerator.Current;

    /// <summary>
    /// Reads the next item from the collection
    /// </summary>
    /// <param name="next">The next item from the collection</param>
    /// <returns>Whether or not there was no more items</returns>
    public bool Next([MaybeNullWhen(false)] out T next)
    {
        next = _default;
        if (!_enumerator.MoveNext())
        {
            EndOfCollection = true;
            return false;
        }

        next = _enumerator.Current;
        return true;
    }

    /// <summary>
    /// Skips all of the items until the predicate is found and then returns the first match
    /// </summary>
    /// <param name="predicate">The predicate to skip until</param>
    /// <returns>The first item to match the predicate or the default item if not match was found</returns>
    public T? SkipUntil(Func<T, bool> predicate)
    {
        while (Next(out var current))
        {
            if (predicate(current))
                return current;
        }

        return _default;
    }

    /// <summary>
    /// Takes all of the items until the predicate is found
    /// </summary>
    /// <param name="predicate">The predicate to match against</param>
    /// <returns>All of the items in the collection until the first item matched the predicate</returns>
    /// <remarks>You can use <see cref="Current"/> to see what item matched the predicate</remarks>
    public IEnumerable<T> TakeUntil(Func<T, bool> predicate)
    {
        while (Next(out var current))
        {
            if (predicate(current)) yield break;

            yield return current;
        }
    }

    /// <summary>
    /// A combination of <see cref="SkipUntil(Func{T, bool})"/> and then <see cref="TakeUntil(Func{T, bool})"/>.
    /// This will skip all of the items that match the <paramref name="skipUntil"/> predicate, 
    /// return the matching one (if <paramref name="includeSkipMatch"/> is true), 
    /// and then take all of the remaining items until a match for the <paramref name="takeUntil"/> predicate is found
    /// </summary>
    /// <param name="skipUntil">The first predicate (used to determine what to skip until)</param>
    /// <param name="takeUntil">The second predicate (used to determine what to take until)</param>
    /// <param name="includeSkipMatch">Whether or not to include the first item to match the first predicate in the return results</param>
    /// <returns>
    /// The first item that matches the <paramref name="skipUntil"/> predicate (if <paramref name="includeSkipMatch"/> is true) 
    /// and all of the remaining items in the <paramref name="takeUntil"/> predicate
    /// </returns>
    public IEnumerable<T> SkipUntilThenTakeUntil(Func<T, bool> skipUntil, Func<T, bool> takeUntil, bool includeSkipMatch = false)
    {
        var af = SkipUntil(skipUntil);
        if (af is null || af.Equals(_default)) yield break;

        if (includeSkipMatch)
            yield return af;

        foreach (var item in TakeUntil(takeUntil))
            yield return item;
    }

    /// <summary>
    /// Skips all of the items until one of the predicates matches and then returns the match and the index of the predicate
    /// </summary>
    /// <param name="predicates">The predicates to skip until</param>
    /// <returns>The item that matched a predicate and the index of the predicate that it matches (or null and -1 if no match was found)</returns>
    public (T? item, int predicateIndex) SkipUntilOneOf(params Func<T, bool>[] predicates)
    {
        while (Next(out var current))
        {
            var index = Array.FindIndex(predicates, t => t(current));
            if (index != -1) return (current, index);
        }

        return (_default, -1);
    }

    /// <summary>
    /// Returns all of the elements until a specific sequence is met
    /// </summary>
    /// <param name="predicates">The predicates to match</param>
    /// <returns>All of the elements found until the sequence was found</returns>
    public T[] TakeUntilSequence(params Func<T, bool>[] predicates)
    {
        if (predicates.Length == 0)
            throw new ArgumentException("At least one predicate is necessary", nameof(predicates));

        int matchIndex = 0;
        var matches = new List<T>();
        while (Next(out var current))
        {
            matches.Add(current);
            if (!predicates[matchIndex](current))
            {
                matchIndex = 0;
                continue;
            }

            matchIndex++;

            if (matchIndex < predicates.Length)
                continue;

            return matches
                .SkipLast(predicates.Length)
                .ToArray();
        }

        return [.. matches];
    }

    /// <summary>
    /// Returns all of the elements until a specific sequence is met
    /// </summary>
    /// <param name="sequence">The sequence of items to match</param>
    /// <returns>All of the elements found until the sequence was found</returns>
    public T[] TakeUntilSequence(params T[] sequence)
    {
        Func<T, bool> Matches(T item)
        {
            return (t) => t is not null && t.Equals(item);
        }

        return TakeUntilSequence(sequence.Select(Matches).ToArray());
    }
}
