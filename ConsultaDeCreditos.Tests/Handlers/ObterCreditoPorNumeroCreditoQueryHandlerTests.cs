using ConsultaDeCreditos.Application.Handlers;
using ConsultaDeCreditos.Application.Queries;
using ConsultaDeCreditos.Domain.Entidades;
using ConsultaDeCreditos.Domain.Interfaces.Repositorios;
using Microsoft.Extensions.Logging;
using Moq;
using Xunit;

namespace ConsultaDeCreditos.Tests.Handlers;

/// <summary>
/// Testes unitários para ObterCreditoPorNumeroCreditoQueryHandler
/// </summary>
public class ObterCreditoPorNumeroCreditoQueryHandlerTests
{
    private readonly Mock<ICreditoRepositorio> _mockRepositorio;
    private readonly Mock<ILogger<ObterCreditoPorNumeroCreditoQueryHandler>> _mockLogger;
    private readonly ObterCreditoPorNumeroCreditoQueryHandler _handler;

    public ObterCreditoPorNumeroCreditoQueryHandlerTests()
    {
        _mockRepositorio = new Mock<ICreditoRepositorio>();
        _mockLogger = new Mock<ILogger<ObterCreditoPorNumeroCreditoQueryHandler>>();
        _handler = new ObterCreditoPorNumeroCreditoQueryHandler(_mockRepositorio.Object, _mockLogger.Object);
    }

    [Fact]
    public async Task Handle_ComCreditoExistente_DeveRetornarCredito()
    {
        // Arrange
        var numeroCredito = "123456";
        var credito = new Credito
        {
            Id = 1,
            NumeroCredito = numeroCredito,
            NumeroNfse = "789",
            DataConstituicao = DateTime.Now,
            SimplesNacional = true
        };

        _mockRepositorio.Setup(x => x.ObterPorNumeroCreditoAsync(numeroCredito))
            .ReturnsAsync(credito);

        var query = new ObterCreditoPorNumeroCreditoQuery { NumeroCredito = numeroCredito };

        // Act
        var result = await _handler.Handle(query, CancellationToken.None);

        // Assert
        Assert.NotNull(result);
        Assert.Equal(numeroCredito, result.NumeroCredito);
        _mockRepositorio.Verify(x => x.ObterPorNumeroCreditoAsync(numeroCredito), Times.Once);
    }

    [Fact]
    public async Task Handle_ComCreditoInexistente_DeveRetornarNull()
    {
        // Arrange
        var numeroCredito = "999999";
        _mockRepositorio.Setup(x => x.ObterPorNumeroCreditoAsync(numeroCredito))
            .ReturnsAsync((Credito?)null);

        var query = new ObterCreditoPorNumeroCreditoQuery { NumeroCredito = numeroCredito };

        // Act
        var result = await _handler.Handle(query, CancellationToken.None);

        // Assert
        Assert.Null(result);
    }
}
