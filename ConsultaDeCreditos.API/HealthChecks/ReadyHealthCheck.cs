using ConsultaDeCreditos.Infrastructure.Persistencia;
using Microsoft.Extensions.Diagnostics.HealthChecks;

namespace ConsultaDeCreditos.API.HealthChecks;

/// <summary>
/// Health check que verifica se o serviço está pronto para receber requisições
/// Valida conectividade com banco de dados e outros recursos críticos
/// </summary>
public class ReadyHealthCheck(
    ConsultaCreditosDbContext dbContext,
    ILogger<ReadyHealthCheck> logger) : IHealthCheck
{
    public async Task<HealthCheckResult> CheckHealthAsync(
        HealthCheckContext context,
        CancellationToken cancellationToken = default)
    {
        try
        {
            await dbContext.Database.CanConnectAsync(cancellationToken);

            return HealthCheckResult.Healthy("O serviço está pronto para receber requisições");
        }
        catch (Exception ex)
        {
            logger.LogError(ex, "Erro ao verificar disponibilidade do serviço");
            return HealthCheckResult.Unhealthy(
                "O serviço não está pronto para receber requisições",
                ex);
        }
    }
}
