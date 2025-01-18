using Microsoft.AspNetCore.Mvc;

namespace CardboardBox.Extensions.AspNetCore;

using Requesting;

/// <summary>
/// A service used for interacting with boxed requests
/// </summary>
public interface IBoxedRequestInterceptService
{
    /// <summary>
    /// Triggered at the beginning of a boxed request
    /// </summary>
    /// <param name="controller">The controller the request was executed from</param>
    /// <returns>The action result to return with</returns>
    /// <remarks>
    /// If you want to stop the request from proceeding, you can return an <see cref="IActionResult"/>
    /// If you want the request to proceed, return null instead.
    /// This is a blocking action, so it will wait for the result before proceeding with the request.
    /// </remarks>
    Task<IActionResult?> RequestStarted(BaseController controller);

    /// <summary>
    /// Triggered when a request throws an exception
    /// </summary>
    /// <param name="controller">The controller the request was executed from</param>
    /// <param name="exception">The exception that occurred</param>
    /// <param name="result">The error result of the request</param>
    /// <returns>The action result to return with</returns>
    /// <remarks>
    /// If you want to stop the request from proceeding, you can return an <see cref="IActionResult"/>
    /// If you want the request to proceed, return null instead.
    /// This is a blocking action, so it will wait for the result before proceeding with the request.
    /// </remarks>
    Task<IActionResult?> RequestFailed(BaseController controller, Exception exception, Boxed result);

    /// <summary>
    /// Triggered when a request has succeeded
    /// </summary>
    /// <param name="controller">The controller the request was executed from</param>
    /// <param name="result">The result of the request</param>
    /// <returns>The action result to return with</returns>
    /// <remarks>
    /// If you want to stop the request from proceeding, you can return an <see cref="IActionResult"/>
    /// If you want the request to proceed, return null instead.
    /// This is a blocking action, so it will wait for the result before proceeding with the request.
    /// </remarks>
    Task<IActionResult?> RequestSucceeded(BaseController controller, Boxed result);
}
