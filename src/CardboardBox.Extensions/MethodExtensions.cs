namespace CardboardBox.Extensions;

/// <summary>
/// Extensions for <see cref="Action"/>s or <see cref="Func{T, TResult}"/>s
/// </summary>
public static class MethodExtensions
{
    /// <summary>
    /// Debounce an action
    /// </summary>
    /// <param name="func">The function to debounce</param>
    /// <param name="milliseconds">The number of milliseconds to wait before running</param>
    /// <returns>The action to execute</returns>
    public static Action Debounce(this Action func, int milliseconds = 300)
    {
        var last = 0;
        return () =>
        {
            var current = Interlocked.Increment(ref last);
            Task
                .Delay(milliseconds)
                .ContinueWith(task =>
                {
                    if (current == last) func();
                    task.Dispose();
                });
        };
    }

    /// <summary>
    /// Debounce an action
    /// </summary>
    /// <typeparam name="T">The type of argument</typeparam>
    /// <param name="func">The function to debounce</param>
    /// <param name="milliseconds">The number of milliseconds to wait before running</param>
    /// <returns>The action to execute</returns>
    public static Action<T> Debounce<T>(this Action<T> func, int milliseconds = 300)
    {
        var last = 0;
        return (arg) =>
        {
            var current = Interlocked.Increment(ref last);
            Task
                .Delay(milliseconds)
                .ContinueWith(task =>
                {
                    if (current == last) func(arg);
                    task.Dispose();
                });
        };
    }
}
