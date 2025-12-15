using Microsoft.Extensions.Diagnostics.HealthChecks;

namespace ConsultaDeCreditos.API.HealthChecks;

/// <summary>
/// Health check que verifica se o serviço está ativo e respondendo
/// </summary>
public class SelfHealthCheck : IHealthCheck
{
    public Task<HealthCheckResult> CheckHealthAsync(
        HealthCheckContext context,
        CancellationToken cancellationToken = default)
    {
        return Task.FromResult(
            HealthCheckResult.Healthy("O serviço está operacional"));
    }
}
