using Microsoft.AspNetCore.Mvc;
using WebAPI.DTOs;
using WebAPI.Models;
using WebAPI.Services.TransacoesService;

namespace WebAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class TransacoesController : ControllerBase
    {
        private readonly ITransacoesInterface _transacoesInterface;

        public TransacoesController(ITransacoesInterface transacoesInterface) => _transacoesInterface = transacoesInterface;

        [HttpPost("Deposito")]
        public async Task<ActionResult<ServiceResponse<Transacoes>>> Deposito(TransacoesDepositoDTO transacoes)
        {
            return Ok(await _transacoesInterface.Deposito(transacoes));
        }

        [HttpPost("Saque")]
        public async Task<ActionResult<ServiceResponse<Transacoes>>> Saque(TransacoesSaqueDTO transacoes)
        {
            return Ok(await _transacoesInterface.Saque(transacoes));
        }

        [HttpPost("Transferencia")]
        public async Task<ActionResult<ServiceResponse<Transacoes>>> Transferencia(TransacoesTransferenciaDTO transacoes)
        {
            return Ok(await _transacoesInterface.Transferencia(transacoes));
        }

        [HttpGet("Saldo")]
        public async Task<ActionResult<ServiceResponse<ContaBancariaSaldoDTO>>> Saldo(int idConta)
        {
            return Ok(await _transacoesInterface.Saldo(idConta));
        }

        [HttpGet("Extrato")]
        public async Task<ActionResult<ServiceResponse<ExtratoDTO>>> Extrato(int idConta)
        {
            return Ok(await _transacoesInterface.Extrato(idConta));
        }
    }
}
