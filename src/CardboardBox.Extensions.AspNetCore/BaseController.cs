using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Http.Extensions;
using Microsoft.AspNetCore.Http.Features;
using Microsoft.Extensions.Logging;
using System.Diagnostics;

namespace CardboardBox.Extensions.AspNetCore;

using Requesting;

/// <summary>
/// An API controller that providers a base for all controllers with handling boxing
/// </summary>
/// <param name="logger">The logger to use</param>
/// <param name="_intercept">The intercept service used to handle requests</param>
[ApiController]
public abstract class BaseController(
    ILogger logger,
    IBoxedRequestInterceptService? _intercept = null) : ControllerBase
{
    /// <summary>
    /// A request validator for the controller
    /// </summary>
    public static RequestValidator Validator => new();

    /// <summary>
    /// The logger for the controller
    /// </summary>
    public ILogger Logger => logger;

    /// <summary>
    /// Handles a request and boxes the result
    /// </summary>
    /// <param name="action">The action to box</param>
    /// <returns>The boxed result</returns>
    [NonAction]
    public async Task<IActionResult> Box(Func<Task<Boxed>> action)
    {
        IActionResult? captured;
        if (_intercept is not null && 
            (captured = await _intercept.RequestStarted(this)) is not null)
            return captured;

        var watch = Stopwatch.StartNew();
        var id = Guid.NewGuid();
        Boxed result;

        try
        {
            result = await action();
        }
        catch (Exception ex)
        {
            var url = Request.HttpContext.Features.Get<IHttpRequestFeature>()?.RawTarget ?? Request.GetDisplayUrl();
            Logger.LogError(ex, "Error occurred during request: {id} >> {url}", id, url);
            result = Boxed.Exception(ex);
            result.Elapsed = watch.Elapsed.TotalMilliseconds;
            result.RequestId = id;

            if (_intercept is not null &&
                (captured = await _intercept.RequestFailed(this, ex, result)) is not null)
                return captured;
        }
        finally
        {
            watch.Stop();
        }

        result.Elapsed = watch.Elapsed.TotalMilliseconds;
        result.RequestId = id;

        if (_intercept is not null &&
            (captured = await _intercept.RequestSucceeded(this, result)) is not null)
            return captured;

        return StatusCode(result.Code, result);
    }

    /// <summary>
    /// Handles a request and boxes the result
    /// </summary>
    /// <param name="action">The action to box</param>
    /// <returns>The boxed result</returns>
    [NonAction]
    public Task<IActionResult> Box(Func<Task> action)
    {
        return Box(() => action().ContinueWith(_ => Boxed.Ok()));
    }

    /// <summary>
    /// Handles a request and boxes the result
    /// </summary>
    /// <param name="action">The action to box</param>
    /// <returns>The boxed result</returns>
    [NonAction]
    public Task<IActionResult> Box<T>(Func<T> action)
    {
        return Box(() => 
        {
            var result = action();
            if (result is Boxed boxed) 
                return Task.FromResult(boxed);
            return Task.FromResult((Boxed)Boxed.Ok(result));
        });
    }

    /// <summary>
    /// Handles a request and boxes the result
    /// </summary>
    /// <param name="action">The action to box</param>
    /// <returns>The boxed result</returns>
    [NonAction]
    public Task<IActionResult> Box<T>(Func<Task<T>> action)
    {
        return Box(() => action().ContinueWith(t =>
        {
            var result = t.Result;
            if (result is Boxed boxed)
                return boxed;
            return Boxed.Ok(t.Result);
        }));
    }
}
