using RentCar.Core.Exceptions;
using System.Net;

namespace RentCar.API.Middlewares;


public class GlobalExceptionMiddleware
{
    private readonly RequestDelegate _next;
    private readonly ILogger<GlobalExceptionMiddleware> _logger;


    public GlobalExceptionMiddleware(RequestDelegate next, ILogger<GlobalExceptionMiddleware> logger)
    {
        _next = next;
        _logger = logger;
    }

    public async Task InvokeAsync(HttpContext context)
    {
        try
        {
            await _next(context); // keyingi middleware/controllerga o‘tkaz
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, $"Exception: {ex.Message}");
            await HandleExceptionAsync(context, ex);
        }
    }

    private static Task HandleExceptionAsync(HttpContext context, Exception exception)
    {
        var statusCode = (int)HttpStatusCode.InternalServerError;
        var title = "Server Error";
        var detail = "Ichki server xatosi yuz berdi.";

        switch (exception)
        {


            case NotFoundException:
                statusCode = (int)HttpStatusCode.NotFound;
                title = "Resource Not Found";
                detail = exception.Message;
                break;

            case UnauthorizedAccessException:
                statusCode = (int)HttpStatusCode.Unauthorized;
                title = "Unauthorized";
                detail = exception.Message;
                break;

            case BadHttpRequestException:
                statusCode = (int)HttpStatusCode.BadRequest;
                title = "Bad Request";
                detail = exception.Message;
                break;

                // boshqa turdagi exceptionlar qo‘shish mumkin
        }

        context.Response.ContentType = "application/json";
        context.Response.StatusCode = statusCode;

        var response = new
        {
            statusCode,
            title,
            detail
        };

        return context.Response.WriteAsJsonAsync(response);
    }
}