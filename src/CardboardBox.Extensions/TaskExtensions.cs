namespace CardboardBox.Extensions;

/// <summary>
/// Extensions for <see cref="Task"/> objects
/// </summary>
public static class TaskExtensions
{
    /// <summary>
    /// Combines two tasks into a single task that returns a tuple of the results
    /// </summary>
    /// <typeparam name="T1">The first type</typeparam>
    /// <typeparam name="T2">The second type</typeparam>
    /// <param name="t1">The first task</param>
    /// <param name="t2">The second task</param>
    /// <returns>The tuple with the results</returns>
    public static Task<(T1, T2)> With<T1, T2>(this Task<T1> t1, Task<T2> t2)
    {
        return Task.WhenAll(t1, t2).ContinueWith(_ => (t1.Result, t2.Result));
    }

    /// <summary>
    /// Combines two tasks into a single task that returns a tuple of the results
    /// </summary>
    /// <typeparam name="T1">The first type</typeparam>
    /// <typeparam name="T2">The second type</typeparam>
    /// <typeparam name="T3">The third type</typeparam>
    /// <param name="t1">The first task</param>
    /// <param name="t2">The second task</param>
    /// <param name="t3">The third task</param>
    /// <returns>The tuple with the results</returns>
    public static Task<(T1, T2, T3)> With<T1, T2, T3>(this Task<T1> t1, Task<T2> t2, Task<T3> t3)
    {
        return Task.WhenAll(t1, t2, t3).ContinueWith(_ => (t1.Result, t2.Result, t3.Result));
    }

    /// <summary>
    /// Combines two tasks into a single task that returns a tuple of the results
    /// </summary>
    /// <typeparam name="T1">The first type</typeparam>
    /// <typeparam name="T2">The second type</typeparam>
    /// <typeparam name="T3">The third type</typeparam>
    /// <typeparam name="T4">The fourth type</typeparam>
    /// <param name="t1">The first task</param>
    /// <param name="t2">The second task</param>
    /// <param name="t3">The third task</param>
    /// <param name="t4">The fourth task</param>
    /// <returns>The tuple with the results</returns>
    public static Task<(T1, T2, T3, T4)> With<T1, T2, T3, T4>(this Task<T1> t1, Task<T2> t2, Task<T3> t3, Task<T4> t4)
    {
        return Task.WhenAll(t1, t2, t3, t4).ContinueWith(_ => (t1.Result, t2.Result, t3.Result, t4.Result));
    }

    /// <summary>
    /// Combines two tasks into a single task that returns a tuple of the results
    /// </summary>
    /// <typeparam name="T1">The first type</typeparam>
    /// <typeparam name="T2">The second type</typeparam>
    /// <typeparam name="T3">The third type</typeparam>
    /// <typeparam name="T4">The fourth type</typeparam>
    /// <typeparam name="T5">The fifth type</typeparam>
    /// <param name="t1">The first task</param>
    /// <param name="t2">The second task</param>
    /// <param name="t3">The third task</param>
    /// <param name="t4">The fourth task</param>
    /// <param name="t5">The fifth task</param>
    /// <returns>The tuple with the results</returns>
    public static Task<(T1, T2, T3, T4, T5)> With<T1, T2, T3, T4, T5>(this Task<T1> t1, Task<T2> t2, Task<T3> t3, Task<T4> t4, Task<T5> t5)
    {
        return Task.WhenAll(t1, t2, t3, t4, t5).ContinueWith(_ => (t1.Result, t2.Result, t3.Result, t4.Result, t5.Result));
    }

    /// <summary>
    /// Combines two tasks into a single task that returns a tuple of the results
    /// </summary>
    /// <typeparam name="T1">The first type</typeparam>
    /// <typeparam name="T2">The second type</typeparam>
    /// <typeparam name="T3">The third type</typeparam>
    /// <typeparam name="T4">The fourth type</typeparam>
    /// <typeparam name="T5">The fifth type</typeparam>
    /// <typeparam name="T6">The sixth type</typeparam>
    /// <param name="t1">The first task</param>
    /// <param name="t2">The second task</param>
    /// <param name="t3">The third task</param>
    /// <param name="t4">The fourth task</param>
    /// <param name="t5">The fifth task</param>
    /// <param name="t6">The sixth task</param>
    /// <returns>The tuple with the results</returns>
    public static Task<(T1, T2, T3, T4, T5, T6)> With<T1, T2, T3, T4, T5, T6>(this Task<T1> t1, Task<T2> t2, Task<T3> t3, Task<T4> t4, Task<T5> t5, Task<T6> t6)
    {
        return Task.WhenAll(t1, t2, t3, t4, t5, t6).ContinueWith(_ => (t1.Result, t2.Result, t3.Result, t4.Result, t5.Result, t6.Result));
    }

    /// <summary>
    /// Combines two tasks into a single task that returns a tuple of the results
    /// </summary>
    /// <typeparam name="T1">The first type</typeparam>
    /// <typeparam name="T2">The second type</typeparam>
    /// <typeparam name="T3">The third type</typeparam>
    /// <typeparam name="T4">The fourth type</typeparam>
    /// <typeparam name="T5">The fifth type</typeparam>
    /// <typeparam name="T6">The sixth type</typeparam>
    /// <typeparam name="T7">The seventh type</typeparam>
    /// <param name="t1">The first task</param>
    /// <param name="t2">The second task</param>
    /// <param name="t3">The third task</param>
    /// <param name="t4">The fourth task</param>
    /// <param name="t5">The fifth task</param>
    /// <param name="t6">The sixth task</param>
    /// <param name="t7">The seventh task</param>
    /// <returns>The tuple with the results</returns>
    public static Task<(T1, T2, T3, T4, T5, T6, T7)> With<T1, T2, T3, T4, T5, T6, T7>(this Task<T1> t1, Task<T2> t2, Task<T3> t3, Task<T4> t4, Task<T5> t5, Task<T6> t6, Task<T7> t7)
    {
        return Task.WhenAll(t1, t2, t3, t4, t5, t6, t7).ContinueWith(_ => (t1.Result, t2.Result, t3.Result, t4.Result, t5.Result, t6.Result, t7.Result));
    }

    /// <summary>
    /// Executes the selector against all of the items in the collection and combines them into a single collection
    /// </summary>
    /// <typeparam name="TOut">The type of the output collection</typeparam>
    /// <typeparam name="TIn">The type of the input collection</typeparam>
    /// <param name="input">The collection of input items</param>
    /// <param name="selector">The selector for the output values</param>
    /// <returns>All of the output values</returns>
    public static async IAsyncEnumerable<TOut> SelectManyAsync<TOut, TIn>(this IEnumerable<TIn> input, Func<TIn, IAsyncEnumerable<TOut>> selector)
    {
        foreach (var item in input)
            await foreach (var output in selector(item))
                yield return output;
    }

    /// <summary>
    /// Performs the given action for every item in the collection
    /// </summary>
    /// <typeparam name="T">The type of collection</typeparam>
    /// <param name="data">The target collection</param>
    /// <param name="action">The action to perform on each element in the collection</param>
    public static Task Each<T>(this IAsyncEnumerable<T> data, Action<T> action) => data.Each((i, t) => action(t));

    /// <summary>
    /// Performs the given action for every item in the collection
    /// </summary>
    /// <typeparam name="T">The type of collection</typeparam>
    /// <param name="data">The target collection</param>
    /// <param name="action">The action to perform on each element in the collection</param>
    public static async Task Each<T>(this IAsyncEnumerable<T> data, Action<int, T> action)
    {
        int count = 0;
        await foreach (var item in data)
        {
            action(count, item);
            count++;
        }
    }

    /// <summary>
    /// Skips the last few elements in the collection
    /// </summary>
    /// <typeparam name="T">The type of collection</typeparam>
    /// <param name="data">The target collection</param>
    /// <param name="count">How many elements to skip (default: 1)</param>
    /// <returns>The collection minus the last few elements</returns>
    public static IAsyncEnumerable<T> SkipLast<T>(this IAsyncEnumerable<T> data, int count = 1)
    {
        return data.Reverse().Skip(count).Reverse();
    }

    /// <summary>
    /// Splits the given collection in chunks of a given length
    /// </summary>
    /// <typeparam name="T">The type of collection</typeparam>
    /// <param name="data">The target collection</param>
    /// <param name="chunks">The max number of item per chunk</param>
    /// <returns>The split up chunks of the target collection</returns>
    public static async IAsyncEnumerable<T[]> Chunk<T>(this IAsyncEnumerable<T> data, int chunks)
    {
        var cur = new List<T>();
        int i = 0;
        await foreach (var item in data)
        {
            if (i >= chunks)
            {
                yield return cur.ToArray();
                cur.Clear();
            }

            cur.Add(item);
            i++;
        }

        if (cur.Count > 0)
            yield return cur.ToArray();
    }

    /// <summary>
    /// Chainable <see cref="Task.WhenAll(Task[])"/>
    /// </summary>
    /// <typeparam name="T">The return result of the tasks</typeparam>
    /// <param name="tasks">The collection of tasks to await</param>
    /// <returns>The awaited tasks</returns>
    public static Task<T[]> WhenAll<T>(this IEnumerable<Task<T>> tasks)
    {
        return Task.WhenAll(tasks);
    }

    /// <summary>
    /// Chainable <see cref="Task.WhenAll(Task[])"/>
    /// </summary>
    /// <param name="tasks">The collection of tasks to await</param>
    /// <returns>The awaited tasks</returns>
    public static Task WhenAll(this IEnumerable<Task> tasks)
    {
        return Task.WhenAll(tasks);
    }
}