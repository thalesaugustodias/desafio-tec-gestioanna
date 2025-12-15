namespace ConsultaDeCreditos.Domain.Interfaces.Mensageria;

/// <summary>
/// Interface para publicação de mensagens no Service Bus
/// </summary>
public interface IServiceBusPublisher
{
    Task PublicarMensagemAsync<T>(T mensagem, string topico) where T : class;
    Task PublicarMensagensAsync<T>(IEnumerable<T> mensagens, string topico) where T : class;
}
