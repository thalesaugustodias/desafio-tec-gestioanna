using ConsultaDeCreditos.Domain.DTOs;
using MediatR;

namespace ConsultaDeCreditos.Application.Queries;

public class ObterCreditosPorNumeroNfseQuery : IRequest<IEnumerable<CreditoConstituidoResponseDto>>
{
    public string NumeroNfse { get; set; } = string.Empty;
}
