namespace ConsultaDeCreditos.API.Middlewares;

/// <summary>
/// Middleware para logging de requisições HTTP
/// </summary>
public class RequestLoggingMiddleware(
    RequestDelegate next,
    ILogger<RequestLoggingMiddleware> logger)
{
    public async Task InvokeAsync(HttpContext context)
    {
        var startTime = DateTime.UtcNow;
        
        logger.LogInformation(
            "Requisição iniciada: {Method} {Path} em {Time}",
            context.Request.Method,
            context.Request.Path,
            startTime);

        await next(context);

        var duration = DateTime.UtcNow - startTime;
        
        logger.LogInformation(
            "Requisição finalizada: {Method} {Path} - Status: {StatusCode} - Duração: {Duration}ms",
            context.Request.Method,
            context.Request.Path,
            context.Response.StatusCode,
            duration.TotalMilliseconds);
    }
}
