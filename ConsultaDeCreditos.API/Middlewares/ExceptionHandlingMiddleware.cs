using System.Net;
using System.Text.Json;

namespace ConsultaDeCreditos.API.Middlewares;

/// <summary>
/// Middleware para tratamento global de exceções
/// Captura exceções não tratadas e retorna resposta padronizada
/// </summary>
public class ExceptionHandlingMiddleware(
    RequestDelegate next,
    ILogger<ExceptionHandlingMiddleware> logger)
{
    public async Task InvokeAsync(HttpContext context)
    {
        try
        {
            await next(context);
        }
        catch (Exception ex)
        {
            logger.LogError(ex, "Exceção não tratada: {Message}", ex.Message);
            await HandleExceptionAsync(context, ex);
        }
    }

    private static Task HandleExceptionAsync(HttpContext context, Exception exception)
    {
        context.Response.ContentType = "application/json";
        context.Response.StatusCode = (int)HttpStatusCode.InternalServerError;

        var response = new
        {
            statusCode = context.Response.StatusCode,
            mensagem = "Ocorreu um erro interno no servidor",
            detalhes = exception.Message
        };

        JsonSerializerOptions jsonSerializerOptions = new()
        {
            PropertyNamingPolicy = JsonNamingPolicy.CamelCase
        };

        var options = jsonSerializerOptions;

        return context.Response.WriteAsync(JsonSerializer.Serialize(response, options));
    }
}
