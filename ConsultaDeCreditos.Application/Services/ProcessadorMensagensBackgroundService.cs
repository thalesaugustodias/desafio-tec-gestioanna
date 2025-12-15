using ConsultaDeCreditos.Application.Factories;
using ConsultaDeCreditos.Domain.DTOs;
using ConsultaDeCreditos.Domain.Interfaces.Mensageria;
using ConsultaDeCreditos.Domain.Interfaces.Repositorios;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;

namespace ConsultaDeCreditos.Application.Services;

/// <summary>
/// Background Service que processa mensagens do Service Bus
/// Verifica a cada 500ms se existem novas mensagens e as processa individualmente
/// </summary>
public class ProcessadorMensagensBackgroundService(
    IServiceProvider serviceProvider,
    ILogger<ProcessadorMensagensBackgroundService> logger) : BackgroundService
{
    private const string TopicoIntegracao = "integrar-credito-constituido-entry";
    private const int IntervaloVerificacaoMs = 500;

    protected override async Task ExecuteAsync(CancellationToken stoppingToken)
    {
        logger.LogInformation("Processador de Mensagens iniciado. Verificando a cada {Intervalo}ms", IntervaloVerificacaoMs);

        while (!stoppingToken.IsCancellationRequested)
        {
            try
            {
                await ProcessarMensagensAsync(stoppingToken);
            }
            catch (Exception ex)
            {
                logger.LogError(ex, "Erro ao processar mensagens do Service Bus");
            }

            await Task.Delay(IntervaloVerificacaoMs, stoppingToken);
        }

        logger.LogInformation("Processador de Mensagens finalizado");
    }

    private async Task ProcessarMensagensAsync(CancellationToken cancellationToken)
    {
        using var scope = serviceProvider.CreateScope();
        var consumer = scope.ServiceProvider.GetRequiredService<IServiceBusConsumer>();
        var repositorio = scope.ServiceProvider.GetRequiredService<ICreditoRepositorio>();

        var mensagem = await consumer.ReceberMensagemAsync<CreditoConstituidoRequestDto>(TopicoIntegracao);

        if (mensagem != null)
        {
            logger.LogInformation("Mensagem recebida do tópico {Topico}. NumeroCredito: {NumeroCredito}", 
                TopicoIntegracao, mensagem.NumeroCredito);

            try
            {
                var existe = await repositorio.ExisteAsync(mensagem.NumeroCredito);
                
                if (!existe)
                {
                    var credito = CreditoFactory.CriarEntidadeDeRequest(mensagem);
                    await repositorio.AdicionarAsync(credito);

                    logger.LogInformation("Crédito {NumeroCredito} inserido na base de dados com sucesso", 
                        mensagem.NumeroCredito);
                }
                else
                {
                    logger.LogWarning("Crédito {NumeroCredito} já existe na base de dados. Ignorando duplicação", 
                        mensagem.NumeroCredito);
                }
            }
            catch (Exception ex)
            {
                logger.LogError(ex, "Erro ao processar crédito {NumeroCredito}", mensagem.NumeroCredito);
            }
        }
    }
}
