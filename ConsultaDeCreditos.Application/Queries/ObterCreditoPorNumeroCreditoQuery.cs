using ConsultaDeCreditos.Domain.DTOs;
using MediatR;

namespace ConsultaDeCreditos.Application.Queries;

public class ObterCreditoPorNumeroCreditoQuery : IRequest<CreditoConstituidoResponseDto?>
{
    public string NumeroCredito { get; set; } = string.Empty;
}
