using System.Text.Json;
using FluentValidation;
using RequirementAI.Business.Interfaces;
using RequirementAI.Contract.Dto.LLMDtos;
using RequirementAI.Contract.Exceptions;

namespace RequirementAI.Business.Helpers;

internal static class LLMResponseRetryHelper
{
    private const int MaxAttempts = 3;

    public static async Task<TDto> GetValidatedResponse<TDto>(
        ILLMProvider llmProvider,
        LLMRequestDto request,
        Func<TDto, CancellationToken, Task> validate,
        string schemaErrorMessage,
        CancellationToken ct)
    {
        for (var attempt = 1; attempt <= MaxAttempts; attempt++)
        {
            try
            {
                var response = await llmProvider.GetResponse(request, ct);
                var result = JsonSerializer.Deserialize<TDto>(response)
                             ?? throw new JsonException("The LLM response could not be deserialized to the expected JSON schema.");

                await validate(result, ct);
                return result;
            }
            catch (JsonException)
            {
                if (attempt == MaxAttempts)
                    throw new BusinessException(schemaErrorMessage);

                AddRetryInstruction(request);
            }
            catch (ValidationException ex)
            {
                if (attempt == MaxAttempts)
                    throw;

                AddRetryInstruction(request, ex.Message);
            }
            catch (Exception ex)
            {
                if (attempt == MaxAttempts || !IsTransientRequestFailure(ex, ct))
                    throw;

                AddRetryInstruction(request, $"Previous request failed with a transient error: {ex.Message}");
            }
        }

        throw new BusinessException(schemaErrorMessage);
    }

    private static bool IsTransientRequestFailure(Exception ex, CancellationToken ct)
    {
        if (ex is OperationCanceledException)
            return !ct.IsCancellationRequested;

        if (ex is HttpRequestException httpRequestException)
            return httpRequestException.StatusCode is not { } statusCode
                   || IsTransientStatusCode((int)statusCode);

        if (TryGetExceptionStatusCode(ex, out var exceptionStatusCode))
            return IsTransientStatusCode(exceptionStatusCode);

        return ex is TimeoutException or IOException;
    }

    // Retry request timeouts, rate limits, and temporary server/gateway failures.
    private static bool IsTransientStatusCode(int statusCode)
    {
        return statusCode is 408 or 429 or 500 or 502 or 503 or 504;
    }

    private static bool TryGetExceptionStatusCode(Exception ex, out int statusCode)
    {
        var statusProperty = ex.GetType().GetProperty("Status");
        if (statusProperty?.PropertyType == typeof(int)
            && statusProperty.GetValue(ex) is int value)
        {
            statusCode = value;
            return true;
        }

        statusCode = 0;
        return false;
    }

    private static void AddRetryInstruction(LLMRequestDto request, string? validationError = null)
    {
        request.Prompt += $"""

                           RETRY INSTRUCTIONS:
                           ---
                           The previous request or response failed.
                           The response could not contain anything but raw JSON that strictly matches the provided JSON schema.
                           Do not include markdown fences, comments, explanations, or any text outside the JSON value.
                           {BuildPreviousFailureMessage(validationError)}
                           ---
                           """;
    }

    private static string BuildPreviousFailureMessage(string? validationError)
    {
        return string.IsNullOrWhiteSpace(validationError)
            ? string.Empty
            : $"Previous failure: {validationError}";
    }
}
