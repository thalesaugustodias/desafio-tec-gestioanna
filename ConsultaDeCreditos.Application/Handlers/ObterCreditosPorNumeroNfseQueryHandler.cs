using ConsultaDeCreditos.Application.Factories;
using ConsultaDeCreditos.Application.Queries;
using ConsultaDeCreditos.Domain.DTOs;
using ConsultaDeCreditos.Domain.Interfaces.Repositorios;
using MediatR;
using Microsoft.Extensions.Logging;

namespace ConsultaDeCreditos.Application.Handlers;

public class ObterCreditosPorNumeroNfseQueryHandler(
    ICreditoRepositorio creditoRepositorio,
    ILogger<ObterCreditosPorNumeroNfseQueryHandler> logger) : IRequestHandler<ObterCreditosPorNumeroNfseQuery, IEnumerable<CreditoConstituidoResponseDto>>
{
    public async Task<IEnumerable<CreditoConstituidoResponseDto>> Handle(ObterCreditosPorNumeroNfseQuery request, CancellationToken cancellationToken)
    {
        logger.LogInformation("Consultando créditos para NFS-e: {NumeroNfse}", request.NumeroNfse);

        var creditos = await creditoRepositorio.ObterPorNumeroNfseAsync(request.NumeroNfse);
        var resultado = CreditoFactory.CriarResponsesDeEntidades(creditos);

        logger.LogInformation("Encontrados {Count} créditos para NFS-e: {NumeroNfse}", 
            creditos.Count(), request.NumeroNfse);

        return resultado;
    }
}
