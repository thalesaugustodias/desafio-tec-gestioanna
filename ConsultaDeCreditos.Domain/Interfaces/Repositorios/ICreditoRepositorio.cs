using ConsultaDeCreditos.Domain.Entidades;

namespace ConsultaDeCreditos.Domain.Interfaces.Repositorios;

public interface ICreditoRepositorio
{
    Task<Credito?> ObterPorNumeroCreditoAsync(string numeroCredito);
    Task<IEnumerable<Credito>> ObterPorNumeroNfseAsync(string numeroNfse);
    Task AdicionarAsync(Credito credito);
    Task<bool> ExisteAsync(string numeroCredito);
}