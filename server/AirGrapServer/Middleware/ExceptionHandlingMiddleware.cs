namespace AirGrap.Middleware;

public class ExceptionHandlingMiddleware
{
    private readonly RequestDelegate _next;

    public ExceptionHandlingMiddleware(RequestDelegate next)
    {
        _next = next;
    }

    public async Task InvokeAsync(HttpContext context)
    {
        try
        {
            await _next(context);
        }
        catch (Exception ex)
        {
            Logger.AddErrorLog(
                $"Unhandled exception: {ex.GetType().Name}",
                $"{context.Request.Method} {context.Request.Path}\n{ex}");
            context.Response.StatusCode = StatusCodes.Status500InternalServerError;
            await context.Response.WriteAsJsonAsync(
                new { error = "Internal server error, please try again" });
        }
    }
}
