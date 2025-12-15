using ConsultaDeCreditos.Application.Commands;
using ConsultaDeCreditos.Domain.DTOs;
using ConsultaDeCreditos.Domain.Interfaces.Mensageria;
using MediatR;
using Microsoft.Extensions.Logging;

namespace ConsultaDeCreditos.Application.Handlers;

public class IntegrarCreditosConstituidosCommandHandler(
    IServiceBusPublisher serviceBusPublisher,
    ILogger<IntegrarCreditosConstituidosCommandHandler> logger) : IRequestHandler<IntegrarCreditosConstituidosCommand, ApiResponseDto>
{
    private const string TopicoIntegracao = "integrar-credito-constituido-entry";

    public async Task<ApiResponseDto> Handle(IntegrarCreditosConstituidosCommand request, CancellationToken cancellationToken)
    {
        try
        {
            logger.LogInformation("Iniciando integração de {Count} créditos constituídos", request.Creditos.Count);

            foreach (var creditoDto in request.Creditos)
            {
                await serviceBusPublisher.PublicarMensagemAsync(creditoDto, TopicoIntegracao);
                logger.LogInformation("Crédito {NumeroCredito} publicado no tópico {Topico}", 
                    creditoDto.NumeroCredito, TopicoIntegracao);
            }

            logger.LogInformation("Integração concluída com sucesso para {Count} créditos", request.Creditos.Count);

            return new ApiResponseDto
            {
                Success = true
            };
        }
        catch (Exception ex)
        {
            logger.LogError(ex, "Erro ao integrar créditos constituídos");
            return new ApiResponseDto
            {
                Success = false,
                Mensagem = "Erro ao processar a integração de créditos"
            };
        }
    }
}
