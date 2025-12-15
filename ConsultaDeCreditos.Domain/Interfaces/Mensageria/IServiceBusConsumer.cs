namespace ConsultaDeCreditos.Domain.Interfaces.Mensageria;

/// <summary>
/// Interface para consumo de mensagens do Service Bus
/// </summary>
public interface IServiceBusConsumer
{
    Task<T?> ReceberMensagemAsync<T>(string topico) where T : class;
}
