using ConsultaDeCreditos.Application.Factories;
using ConsultaDeCreditos.Domain.DTOs;
using ConsultaDeCreditos.Domain.Entidades;
using Xunit;

namespace ConsultaDeCreditos.Tests.Factories;

/// <summary>
/// Testes unitários para CreditoFactory
/// </summary>
public class CreditoFactoryTests
{
    [Fact]
    public void CriarEntidadeDeRequest_DeveConverterCorretamente()
    {
        // Arrange
        var dto = new CreditoConstituidoRequestDto
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
        };

        // Act
        var entidade = CreditoFactory.CriarEntidadeDeRequest(dto);

        // Assert
        Assert.Equal(dto.NumeroCredito, entidade.NumeroCredito);
        Assert.Equal(dto.NumeroNfse, entidade.NumeroNfse);
        Assert.Equal(dto.ValorIssqn, entidade.ValorIssqn);
        Assert.Equal(dto.TipoCredito, entidade.TipoCredito);
        Assert.True(entidade.SimplesNacional);
        Assert.Equal(dto.Aliquota, entidade.Aliquota);
    }

    [Fact]
    public void CriarEntidadeDeRequest_SimplesNacionalNao_DeveSerFalse()
    {
        // Arrange
        var dto = new CreditoConstituidoRequestDto
        {
            NumeroCredito = "123456",
            NumeroNfse = "789",
            DataConstituicao = "2024-02-25",
            ValorIssqn = 1500.75m,
            TipoCredito = "ISSQN",
            SimplesNacional = "Não",
            Aliquota = 5.0m,
            ValorFaturado = 30000.00m,
            ValorDeducao = 5000.00m,
            BaseCalculo = 25000.00m
        };

        // Act
        var entidade = CreditoFactory.CriarEntidadeDeRequest(dto);

        // Assert
        Assert.False(entidade.SimplesNacional);
    }

    [Fact]
    public void CriarResponseDeEntidade_DeveConverterCorretamente()
    {
        // Arrange
        var entidade = new Credito
        {
            Id = 1,
            NumeroCredito = "123456",
            NumeroNfse = "789",
            DataConstituicao = new DateTime(2024, 2, 25),
            ValorIssqn = 1500.75m,
            TipoCredito = "ISSQN",
            SimplesNacional = true,
            Aliquota = 5.0m,
            ValorFaturado = 30000.00m,
            ValorDeducao = 5000.00m,
            BaseCalculo = 25000.00m
        };

        // Act
        var dto = CreditoFactory.CriarResponseDeEntidade(entidade);

        // Assert
        Assert.Equal(entidade.NumeroCredito, dto.NumeroCredito);
        Assert.Equal(entidade.NumeroNfse, dto.NumeroNfse);
        Assert.Equal("2024-02-25", dto.DataConstituicao);
        Assert.Equal("Sim", dto.SimplesNacional);
    }

    [Fact]
    public void CriarResponsesDeEntidades_DeveConverterLista()
    {
        // Arrange
        var entidades = new List<Credito>
        {
            new Credito { NumeroCredito = "123", NumeroNfse = "789", SimplesNacional = true, DataConstituicao = DateTime.Now },
            new Credito { NumeroCredito = "456", NumeroNfse = "789", SimplesNacional = false, DataConstituicao = DateTime.Now }
        };

        // Act
        var dtos = CreditoFactory.CriarResponsesDeEntidades(entidades).ToList();

        // Assert
        Assert.Equal(2, dtos.Count);
        Assert.Equal("123", dtos[0].NumeroCredito);
        Assert.Equal("456", dtos[1].NumeroCredito);
    }
}
