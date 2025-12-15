using ConsultaDeCreditos.Domain.DTOs;
using MediatR;

namespace ConsultaDeCreditos.Application.Commands;

public class IntegrarCreditosConstituidosCommand : IRequest<ApiResponseDto>
{
    public List<CreditoConstituidoRequestDto> Creditos { get; set; } = [];
}
