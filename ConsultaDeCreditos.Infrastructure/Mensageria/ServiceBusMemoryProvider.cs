using ConsultaDeCreditos.Domain.Interfaces.Mensageria;
using Microsoft.Extensions.Logging;
using System.Collections.Concurrent;
using System.Text.Json;

namespace ConsultaDeCreditos.Infrastructure.Mensageria;

/// <summary>
/// Implementação local do Service Bus usando fila em memória
/// Para produção, seria necessário substituir por Azure Service Bus ou Kafka
/// Implementa padrão Singleton para garantir única instância da fila
/// </summary>
public class ServiceBusMemoryProvider(ILogger<ServiceBusMemoryProvider> logger) : IServiceBusPublisher, IServiceBusConsumer
{
    private readonly ConcurrentDictionary<string, ConcurrentQueue<string>> _topicos = new();

    public Task PublicarMensagemAsync<T>(T mensagem, string topico) where T : class
    {
        try
        {
            var fila = _topicos.GetOrAdd(topico, _ => new ConcurrentQueue<string>());
            var mensagemJson = JsonSerializer.Serialize(mensagem);
            fila.Enqueue(mensagemJson);

            logger.LogDebug("Mensagem publicada no tópico {Topico}", topico);
            return Task.CompletedTask;
        }
        catch (Exception ex)
        {
            logger.LogError(ex, "Erro ao publicar mensagem no tópico {Topico}", topico);
            throw;
        }
    }

    public Task PublicarMensagensAsync<T>(IEnumerable<T> mensagens, string topico) where T : class
    {
        foreach (var mensagem in mensagens)
        {
            PublicarMensagemAsync(mensagem, topico);
        }
        return Task.CompletedTask;
    }

    public Task<T?> ReceberMensagemAsync<T>(string topico) where T : class
    {
        try
        {
            if (_topicos.TryGetValue(topico, out var fila) && fila.TryDequeue(out var mensagemJson))
            {
                var mensagem = JsonSerializer.Deserialize<T>(mensagemJson);
                logger.LogDebug("Mensagem recebida do tópico {Topico}", topico);
                return Task.FromResult(mensagem);
            }

            return Task.FromResult<T?>(null);
        }
        catch (Exception ex)
        {
            logger.LogError(ex, "Erro ao receber mensagem do tópico {Topico}", topico);
            return Task.FromResult<T?>(null);
        }
    }
}
