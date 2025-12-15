using ConsultaDeCreditos.Domain.Entidades;
using ConsultaDeCreditos.Infrastructure.Persistencia;
using ConsultaDeCreditos.Infrastructure.Repositorios;
using Microsoft.EntityFrameworkCore;
using Xunit;

namespace ConsultaDeCreditos.Tests.Repositorios;

/// <summary>
/// Testes unitários para CreditoRepositorio usando InMemory Database
/// </summary>
public class CreditoRepositorioTests : IDisposable
{
    private readonly ConsultaCreditosDbContext _context;
    private readonly CreditoRepositorio _repositorio;

    public CreditoRepositorioTests()
    {
        var options = new DbContextOptionsBuilder<ConsultaCreditosDbContext>()
            .UseInMemoryDatabase(databaseName: Guid.NewGuid().ToString())
            .Options;

        _context = new ConsultaCreditosDbContext(options);
        _repositorio = new CreditoRepositorio(_context);
    }

    [Fact]
    public async Task AdicionarAsync_DeveAdicionarCreditoComSucesso()
    {
        // Arrange
        var credito = new Credito
        {
            NumeroCredito = "123456",
            NumeroNfse = "789",
            DataConstituicao = DateTime.Now,
            ValorIssqn = 1500.75m,
            TipoCredito = "ISSQN",
            SimplesNacional = true,
            Aliquota = 5.0m,
            ValorFaturado = 30000.00m,
            ValorDeducao = 5000.00m,
            BaseCalculo = 25000.00m
        };

        // Act
        await _repositorio.AdicionarAsync(credito);

        // Assert
        var creditoAdicionado = await _context.Creditos.FirstOrDefaultAsync(c => c.NumeroCredito == "123456");
        Assert.NotNull(creditoAdicionado);
        Assert.Equal("123456", creditoAdicionado.NumeroCredito);
    }

    [Fact]
    public async Task ObterPorNumeroCreditoAsync_ComCreditoExistente_DeveRetornarCredito()
    {
        // Arrange
        var credito = new Credito
        {
            NumeroCredito = "TEST123",
            NumeroNfse = "789",
            DataConstituicao = DateTime.Now,
            SimplesNacional = true
        };
        await _context.Creditos.AddAsync(credito);
        await _context.SaveChangesAsync();

        // Act
        var resultado = await _repositorio.ObterPorNumeroCreditoAsync("TEST123");

        // Assert
        Assert.NotNull(resultado);
        Assert.Equal("TEST123", resultado.NumeroCredito);
    }

    [Fact]
    public async Task ObterPorNumeroNfseAsync_ComMultiplosCreditos_DeveRetornarTodos()
    {
        // Arrange
        var creditos = new List<Credito>
        {
            new Credito { NumeroCredito = "C1", NumeroNfse = "NF001", DataConstituicao = DateTime.Now, SimplesNacional = true },
            new Credito { NumeroCredito = "C2", NumeroNfse = "NF001", DataConstituicao = DateTime.Now, SimplesNacional = false },
            new Credito { NumeroCredito = "C3", NumeroNfse = "NF002", DataConstituicao = DateTime.Now, SimplesNacional = true }
        };
        await _context.Creditos.AddRangeAsync(creditos);
        await _context.SaveChangesAsync();

        // Act
        var resultado = await _repositorio.ObterPorNumeroNfseAsync("NF001");

        // Assert
        Assert.Equal(2, resultado.Count());
    }

    [Fact]
    public async Task ExisteAsync_ComCreditoExistente_DeveRetornarTrue()
    {
        // Arrange
        var credito = new Credito
        {
            NumeroCredito = "EXISTS123",
            NumeroNfse = "789",
            DataConstituicao = DateTime.Now,
            SimplesNacional = true
        };
        await _context.Creditos.AddAsync(credito);
        await _context.SaveChangesAsync();

        // Act
        var existe = await _repositorio.ExisteAsync("EXISTS123");

        // Assert
        Assert.True(existe);
    }

    [Fact]
    public async Task ExisteAsync_ComCreditoInexistente_DeveRetornarFalse()
    {
        // Act
        var existe = await _repositorio.ExisteAsync("NOTEXISTS");

        // Assert
        Assert.False(existe);
    }

    public void Dispose()
    {
        _context.Database.EnsureDeleted();
        _context.Dispose();
    }
}
