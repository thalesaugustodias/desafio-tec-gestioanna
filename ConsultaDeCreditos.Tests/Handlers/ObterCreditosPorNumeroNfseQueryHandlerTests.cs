using ConsultaDeCreditos.Application.Handlers;
using ConsultaDeCreditos.Application.Queries;
using ConsultaDeCreditos.Domain.Entidades;
using ConsultaDeCreditos.Domain.Interfaces.Repositorios;
using Microsoft.Extensions.Logging;
using Moq;
using Xunit;

namespace ConsultaDeCreditos.Tests.Handlers;

/// <summary>
/// Testes unitários para ObterCreditosPorNumeroNfseQueryHandler
/// </summary>
public class ObterCreditosPorNumeroNfseQueryHandlerTests
{
    private readonly Mock<ICreditoRepositorio> _mockRepositorio;
    private readonly Mock<ILogger<ObterCreditosPorNumeroNfseQueryHandler>> _mockLogger;
    private readonly ObterCreditosPorNumeroNfseQueryHandler _handler;

    public ObterCreditosPorNumeroNfseQueryHandlerTests()
    {
        _mockRepositorio = new Mock<ICreditoRepositorio>();
        _mockLogger = new Mock<ILogger<ObterCreditosPorNumeroNfseQueryHandler>>();
        _handler = new ObterCreditosPorNumeroNfseQueryHandler(_mockRepositorio.Object, _mockLogger.Object);
    }

    [Fact]
    public async Task Handle_ComNfseExistente_DeveRetornarCreditos()
    {
        // Arrange
        var numeroNfse = "789";
        var creditos = new List<Credito>
        {
            new Credito
            {
                Id = 1,
                NumeroCredito = "123",
                NumeroNfse = numeroNfse,
                DataConstituicao = DateTime.Now,
                SimplesNacional = true
            }
        };

        _mockRepositorio.Setup(x => x.ObterPorNumeroNfseAsync(numeroNfse))
            .ReturnsAsync(creditos);

        var query = new ObterCreditosPorNumeroNfseQuery { NumeroNfse = numeroNfse };

        // Act
        var result = await _handler.Handle(query, CancellationToken.None);

        // Assert
        Assert.NotEmpty(result);
        Assert.Single(result);
        _mockRepositorio.Verify(x => x.ObterPorNumeroNfseAsync(numeroNfse), Times.Once);
    }

    [Fact]
    public async Task Handle_ComNfseInexistente_DeveRetornarListaVazia()
    {
        // Arrange
        var numeroNfse = "999";
        _mockRepositorio.Setup(x => x.ObterPorNumeroNfseAsync(numeroNfse))
            .ReturnsAsync(new List<Credito>());

        var query = new ObterCreditosPorNumeroNfseQuery { NumeroNfse = numeroNfse };

        // Act
        var result = await _handler.Handle(query, CancellationToken.None);

        // Assert
        Assert.Empty(result);
    }
}
