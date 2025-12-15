using ConsultaDeCreditos.Application.Factories;
using ConsultaDeCreditos.Application.Queries;
using ConsultaDeCreditos.Domain.DTOs;
using ConsultaDeCreditos.Domain.Interfaces.Repositorios;
using MediatR;
using Microsoft.Extensions.Logging;

namespace ConsultaDeCreditos.Application.Handlers;

public class ObterCreditoPorNumeroCreditoQueryHandler(
    ICreditoRepositorio creditoRepositorio,
    ILogger<ObterCreditoPorNumeroCreditoQueryHandler> logger) : IRequestHandler<ObterCreditoPorNumeroCreditoQuery, CreditoConstituidoResponseDto?>
{
    public async Task<CreditoConstituidoResponseDto?> Handle(ObterCreditoPorNumeroCreditoQuery request, CancellationToken cancellationToken)
    {
        logger.LogInformation("Consultando crédito: {NumeroCredito}", request.NumeroCredito);

        var credito = await creditoRepositorio.ObterPorNumeroCreditoAsync(request.NumeroCredito);

        if (credito == null)
        {
            logger.LogWarning("Crédito não encontrado: {NumeroCredito}", request.NumeroCredito);
            return null;
        }

        logger.LogInformation("Crédito encontrado: {NumeroCredito}", request.NumeroCredito);
        return CreditoFactory.CriarResponseDeEntidade(credito);
    }
}
