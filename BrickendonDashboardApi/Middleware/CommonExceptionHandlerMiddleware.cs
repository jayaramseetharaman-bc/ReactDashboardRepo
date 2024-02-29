using BrickendonDashboard.Domain.Exceptions;
using Microsoft.EntityFrameworkCore;
using Newtonsoft.Json;
using System.Net;

namespace BrickendonDashboard.Api.Middleware
{
  public class CommonExceptionHandlerMiddleware
  {
    private RequestDelegate _next;
    private readonly ILogger<CommonExceptionHandlerMiddleware> _logger;

    public CommonExceptionHandlerMiddleware(RequestDelegate next, ILogger<CommonExceptionHandlerMiddleware> logger)
    {
      _next=next;
      _logger=logger;
    }
    public async Task Invoke(HttpContext context)
    {
      try
      {
        await _next(context);
      }
      catch (UnauthorizedAccessException ex)
      {
        await HandleException(context, string.Empty, ex, HttpStatusCode.Unauthorized);
      }
      catch (ResourceNotFoundException ex)
      {
        await HandleException(context, string.Empty, ex, HttpStatusCode.NotFound);
      }
      catch (ResourceAlreadyExistsException ex)
      {
        await HandleException(context, ex.Message, ex, HttpStatusCode.Conflict);
      }
      catch (DbUpdateConcurrencyException ex)
      {
        await HandleException(context, string.Empty, ex, HttpStatusCode.Conflict);
      }
      catch (CustomException ex)
      {
        await HandleException(context, ex.Message, ex, HttpStatusCode.BadRequest);
      }
      catch (Exception ex)
      {
#if DEBUG
        await HandleException(context, ex.Message, ex, HttpStatusCode.InternalServerError);
# else
        await HandleException(context, "ERROR", ex, HttpStatusCode.InternalServerError);
#endif
      }

    }

    private async Task HandleException(HttpContext context, string displayError, Exception ex, HttpStatusCode statusCode)
    {
      Console.WriteLine(ex.Message);
      Console.WriteLine(JsonConvert.SerializeObject(ex));
      _logger.LogError(ex, displayError);
      context.Response.StatusCode = (int)statusCode;
      await context.Response.WriteAsync(displayError);
    }
  }


}
