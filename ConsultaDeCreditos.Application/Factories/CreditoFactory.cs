using ConsultaDeCreditos.Domain.DTOs;
using ConsultaDeCreditos.Domain.Entidades;
using System.Globalization;

namespace ConsultaDeCreditos.Application.Factories;

/// <summary>
/// Factory para mapeamento entre DTOs e Entidades de Crédito
/// Implementa padrão Factory para separação de responsabilidades
/// </summary>
public static class CreditoFactory
{
    public static Credito CriarEntidadeDeRequest(CreditoConstituidoRequestDto dto)
    {
        var dataConstituicao = DateTime.Parse(dto.DataConstituicao, CultureInfo.InvariantCulture);
        dataConstituicao = DateTime.SpecifyKind(dataConstituicao, DateTimeKind.Utc);

        return new Credito
        {
            NumeroCredito = dto.NumeroCredito,
            NumeroNfse = dto.NumeroNfse,
            DataConstituicao = dataConstituicao,
            ValorIssqn = dto.ValorIssqn,
            TipoCredito = dto.TipoCredito,
            SimplesNacional = dto.SimplesNacional.Equals("Sim", StringComparison.OrdinalIgnoreCase),
            Aliquota = dto.Aliquota,
            ValorFaturado = dto.ValorFaturado,
            ValorDeducao = dto.ValorDeducao,
            BaseCalculo = dto.BaseCalculo,
            DataCriacao = DateTime.UtcNow
        };
    }

    public static CreditoConstituidoResponseDto CriarResponseDeEntidade(Credito entidade)
    {
        return new CreditoConstituidoResponseDto
        {
            NumeroCredito = entidade.NumeroCredito,
            NumeroNfse = entidade.NumeroNfse,
            DataConstituicao = entidade.DataConstituicao.ToString("yyyy-MM-dd"),
            ValorIssqn = entidade.ValorIssqn,
            TipoCredito = entidade.TipoCredito,
            SimplesNacional = entidade.SimplesNacional ? "Sim" : "Não",
            Aliquota = entidade.Aliquota,
            ValorFaturado = entidade.ValorFaturado,
            ValorDeducao = entidade.ValorDeducao,
            BaseCalculo = entidade.BaseCalculo
        };
    }

    public static IEnumerable<CreditoConstituidoResponseDto> CriarResponsesDeEntidades(IEnumerable<Credito> entidades)
    {
        return entidades.Select(CriarResponseDeEntidade);
    }
}
