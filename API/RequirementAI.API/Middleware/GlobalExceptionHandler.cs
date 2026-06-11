using System.Net;
using Microsoft.AspNetCore.Diagnostics;
using RequirementAI.Contract.Dto.ResponseWrappers;
using RequirementAI.Contract.Exceptions;

namespace RequirementAI.API.Middleware;

public class GlobalExceptionHandler : IExceptionHandler
{
    public async ValueTask<bool> TryHandleAsync(HttpContext context, Exception exception, CancellationToken ct)
    {
        var (statusCode, message) = exception switch
        {
            AuthorizationException ae => ((int)HttpStatusCode.Unauthorized, ae.Message),
            EntityNotFoundException e => ((int)HttpStatusCode.NotFound, e.Message),
            RequirementAIException e => ((int)HttpStatusCode.BadRequest, e.Message),
            _ => ((int)HttpStatusCode.InternalServerError, "Unknown problem has occured. Please contact support.")
        };

        context.Response.StatusCode = statusCode;
        context.Response.ContentType = "application/json";

        var payload = ResponseDto.Fail(message);

        await context.Response.WriteAsJsonAsync(payload, ct);
        return true;
    }
}