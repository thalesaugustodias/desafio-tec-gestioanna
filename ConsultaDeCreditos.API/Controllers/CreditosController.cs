using ConsultaDeCreditos.Application.Commands;
using ConsultaDeCreditos.Application.Queries;
using ConsultaDeCreditos.Domain.DTOs;
using MediatR;
using Microsoft.AspNetCore.Mvc;

namespace ConsultaDeCreditos.API.Controllers;

/// <summary>
/// Controller para gerenciamento de créditos constituídos
/// </summary>
[ApiController]
[Route("api/creditos")]
public class CreditosController(IMediator mediator, ILogger<CreditosController> logger) : ControllerBase
{

    /// <summary>
    /// Integra uma lista de créditos constituídos
    /// </summary>
    /// <param name="creditos">Lista de créditos a serem integrados</param>
    /// <returns>Resposta com status da operação</returns>
    [HttpPost("integrar-credito-constituido")]
    [ProducesResponseType(typeof(ApiResponseDto), StatusCodes.Status202Accepted)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    public async Task<IActionResult> IntegrarCreditoConstituido([FromBody] List<CreditoConstituidoRequestDto> creditos)
    {
        if (creditos == null || creditos.Count <= 0)
        {
            logger.LogWarning("Requisição recebida sem créditos para integrar");
            return BadRequest(new ApiResponseDto 
            { 
                Success = false, 
                Mensagem = "Lista de créditos não pode estar vazia" 
            });
        }

        var command = new IntegrarCreditosConstituidosCommand { Creditos = creditos };
        var resultado = await mediator.Send(command);

        return AcceptedAtAction(nameof(IntegrarCreditoConstituido), resultado);
    }

    /// <summary>
    /// Obtém créditos por número da NFS-e
    /// </summary>
    /// <param name="numeroNfse">Número da NFS-e</param>
    /// <returns>Lista de créditos encontrados</returns>
    [HttpGet("{numeroNfse}")]
    [ProducesResponseType(typeof(IEnumerable<CreditoConstituidoResponseDto>), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<IActionResult> ObterPorNumeroNfse(string numeroNfse)
    {
        logger.LogInformation("Requisição para obter créditos da NFS-e: {NumeroNfse}", numeroNfse);

        var query = new ObterCreditosPorNumeroNfseQuery { NumeroNfse = numeroNfse };
        var resultado = await mediator.Send(query);

        if (!resultado.Any())
        {
            logger.LogWarning("Nenhum crédito encontrado para NFS-e: {NumeroNfse}", numeroNfse);
            return NotFound(new { mensagem = $"Nenhum crédito encontrado para a NFS-e {numeroNfse}" });
        }

        return Ok(resultado);
    }

    /// <summary>
    /// Obtém crédito por número do crédito
    /// </summary>
    /// <param name="numeroCredito">Número do crédito</param>
    /// <returns>Detalhes do crédito encontrado</returns>
    [HttpGet("credito/{numeroCredito}")]
    [ProducesResponseType(typeof(CreditoConstituidoResponseDto), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<IActionResult> ObterPorNumeroCredito(string numeroCredito)
    {
        logger.LogInformation("Requisição para obter crédito: {NumeroCredito}", numeroCredito);

        var query = new ObterCreditoPorNumeroCreditoQuery { NumeroCredito = numeroCredito };
        var resultado = await mediator.Send(query);

        if (resultado == null)
        {
            logger.LogWarning("Crédito não encontrado: {NumeroCredito}", numeroCredito);
            return NotFound(new { mensagem = $"Crédito {numeroCredito} não encontrado" });
        }

        return Ok(resultado);
    }
}
