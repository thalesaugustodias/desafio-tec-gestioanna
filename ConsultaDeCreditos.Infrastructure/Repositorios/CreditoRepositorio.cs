using ConsultaDeCreditos.Domain.Entidades;
using ConsultaDeCreditos.Domain.Interfaces.Repositorios;
using ConsultaDeCreditos.Infrastructure.Persistencia;
using Microsoft.EntityFrameworkCore;

namespace ConsultaDeCreditos.Infrastructure.Repositorios;

public class CreditoRepositorio(ConsultaCreditosDbContext context) : ICreditoRepositorio
{
    public async Task<Credito?> ObterPorNumeroCreditoAsync(string numeroCredito)
    {
        return await context.Creditos
            .AsNoTracking()
            .FirstOrDefaultAsync(c => c.NumeroCredito == numeroCredito);
    }

    public async Task<IEnumerable<Credito>> ObterPorNumeroNfseAsync(string numeroNfse)
    {
        return await context.Creditos
            .AsNoTracking()
            .Where(c => c.NumeroNfse == numeroNfse)
            .ToListAsync();
    }

    public async Task AdicionarAsync(Credito credito)
    {
        await context.Creditos.AddAsync(credito);
        await context.SaveChangesAsync();
    }

    public async Task<bool> ExisteAsync(string numeroCredito)
    {
        return await context.Creditos
            .AnyAsync(c => c.NumeroCredito == numeroCredito);
    }
}
