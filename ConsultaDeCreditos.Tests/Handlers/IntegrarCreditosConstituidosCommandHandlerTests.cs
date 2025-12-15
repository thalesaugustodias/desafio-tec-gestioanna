using ConsultaDeCreditos.Application.Commands;
using ConsultaDeCreditos.Application.Handlers;
using ConsultaDeCreditos.Domain.DTOs;
using ConsultaDeCreditos.Domain.Interfaces.Mensageria;
using Microsoft.Extensions.Logging;
using Moq;
using Xunit;

namespace ConsultaDeCreditos.Tests.Handlers;

/// <summary>
/// Testes unitários para IntegrarCreditosConstituidosCommandHandler
/// </summary>
public class IntegrarCreditosConstituidosCommandHandlerTests
{
    private readonly Mock<IServiceBusPublisher> _mockPublisher;
    private readonly Mock<ILogger<IntegrarCreditosConstituidosCommandHandler>> _mockLogger;
    private readonly IntegrarCreditosConstituidosCommandHandler _handler;

    public IntegrarCreditosConstituidosCommandHandlerTests()
    {
        _mockPublisher = new Mock<IServiceBusPublisher>();
        _mockLogger = new Mock<ILogger<IntegrarCreditosConstituidosCommandHandler>>();
        _handler = new IntegrarCreditosConstituidosCommandHandler(_mockPublisher.Object, _mockLogger.Object);
    }

    [Fact]
    public async Task Handle_ComCreditosValidos_DevePublicarMensagensERetornarSucesso()
    {
        // Arrange
        var command = new IntegrarCreditosConstituidosCommand
        {
            Creditos = new List<CreditoConstituidoRequestDto>
            {
                new CreditoConstituidoRequestDto
                {
                    NumeroCredito = "123456",
                    NumeroNfse = "789",
                    DataConstituicao = "2024-02-25",
                    ValorIssqn = 1500.75m,
                    TipoCredito = "ISSQN",
                    SimplesNacional = "Sim",
                    Aliquota = 5.0m,
                    ValorFaturado = 30000.00m,
                    ValorDeducao = 5000.00m,
                    BaseCalculo = 25000.00m
                }
            }
        };

        // Act
        var result = await _handler.Handle(command, CancellationToken.None);

        // Assert
        Assert.True(result.Success);
        _mockPublisher.Verify(x => x.PublicarMensagemAsync(
            It.IsAny<CreditoConstituidoRequestDto>(), 
            "integrar-credito-constituido-entry"), 
            Times.Once);
    }

    [Fact]
    public async Task Handle_ComMultiplosCreditos_DevePublicarTodasMensagens()
    {
        // Arrange
        var command = new IntegrarCreditosConstituidosCommand
        {
            Creditos = new List<CreditoConstituidoRequestDto>
            {
                new CreditoConstituidoRequestDto { NumeroCredito = "123" },
                new CreditoConstituidoRequestDto { NumeroCredito = "456" },
                new CreditoConstituidoRequestDto { NumeroCredito = "789" }
            }
        };

        // Act
        var result = await _handler.Handle(command, CancellationToken.None);

        // Assert
        Assert.True(result.Success);
        _mockPublisher.Verify(x => x.PublicarMensagemAsync(
            It.IsAny<CreditoConstituidoRequestDto>(), 
            It.IsAny<string>()), 
            Times.Exactly(3));
    }

    [Fact]
    public async Task Handle_QuandoPublisherLancaExcecao_DeveRetornarFalha()
    {
        // Arrange
        _mockPublisher.Setup(x => x.PublicarMensagemAsync(
            It.IsAny<CreditoConstituidoRequestDto>(), 
            It.IsAny<string>()))
            .ThrowsAsync(new Exception("Erro ao publicar"));

        var command = new IntegrarCreditosConstituidosCommand
        {
            Creditos = new List<CreditoConstituidoRequestDto>
            {
                new CreditoConstituidoRequestDto { NumeroCredito = "123" }
            }
        };

        // Act
        var result = await _handler.Handle(command, CancellationToken.None);

        // Assert
        Assert.False(result.Success);
        Assert.NotNull(result.Mensagem);
    }
}
