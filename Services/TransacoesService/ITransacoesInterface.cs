using WebAPI.DTOs;
using WebAPI.Models;

namespace WebAPI.Services.TransacoesService
{
    public interface ITransacoesInterface
    {
        public Task<ServiceResponse<TransacoesDTO>> Deposito(TransacoesDepositoDTO deposito);

        public Task<ServiceResponse<TransacoesDTO>> Saque(TransacoesSaqueDTO transacoes);

        public Task<ServiceResponse<TransacoesDTO>> Transferencia(TransacoesTransferenciaDTO transacoes);

        public Task<ServiceResponse<ContaBancariaSaldoDTO>> Saldo(int idConta);

        public Task<ServiceResponse<List<ExtratoDTO>>> Extrato(int idConta);
    }
}
