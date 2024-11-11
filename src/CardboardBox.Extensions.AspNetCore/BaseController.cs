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
[ApiController]
public abstract class BaseController(
    ILogger logger) : ControllerBase
{
    /// <summary>
    /// A request validator for the controller
    /// </summary>
    public RequestValidator Validator => new();

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
        }

        result.Elapsed = watch.Elapsed.TotalMilliseconds;
        result.RequestId = id;
        return StatusCode(result.Code, result);
    }

    /// <summary>
    /// Handles a request and boxes the result
    /// </summary>
    /// <param name="action">The action to box</param>
    /// <returns>The boxed result</returns>
    [NonAction]
    public Task<IActionResult> Box(Func<Boxed> action)
    {
        return Box(() => Task.FromResult(action()));
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
        return Box(() => (Boxed)Boxed.Ok(action()));
    }

    /// <summary>
    /// Handles a request and boxes the result
    /// </summary>
    /// <param name="action">The action to box</param>
    /// <returns>The boxed result</returns>
    [NonAction]
    public Task<IActionResult> Box<T>(Func<Task<T>> action)
    {
        return Box(() => action().ContinueWith(t => (Boxed)Boxed.Ok(t.Result)));
    }
}
