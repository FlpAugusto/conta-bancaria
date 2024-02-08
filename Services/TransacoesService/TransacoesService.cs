using AutoMapper;
using WebAPI.DataContext;
using WebAPI.DTOs;
using WebAPI.Enums;
using WebAPI.Helpers;
using WebAPI.Models;

namespace WebAPI.Services.TransacoesService
{
    public class TransacoesService : ITransacoesInterface
    {
        private readonly ApplicationDbContext _context;
        private readonly IMapper _mapper;

        public TransacoesService(ApplicationDbContext context, IMapper mapper)
        {
            _context = context;
            _mapper = mapper;
        }

        public async Task<ServiceResponse<TransacoesDTO>> Deposito(TransacoesDepositoDTO deposito)
        {
            ServiceResponse<TransacoesDTO> response = new();

            var contaBancaria = _context.ContaBancaria.FirstOrDefault(c => c.Conta == deposito.Conta && c.Agencia == deposito.Agencia);
            if (contaBancaria == null)
            {
                response.Mensagem = "Conta bancária não encontrada";
                response.Sucesso = false;
                return response;
            }

            if (deposito.Valor <= 0)
            {
                response.Mensagem = "Valor inválido";
                response.Sucesso = false;
                return response;
            }

            await AtualizaSaldoConta(contaBancaria, response, deposito.Valor, TipoOperacao.Deposito);
            await CriarRegistroTransacao(contaBancaria.Id, deposito.Valor, TipoOperacao.Deposito);
            return response;
        }

        public async Task<ServiceResponse<TransacoesDTO>> Saque(TransacoesSaqueDTO saque)
        {
            ServiceResponse<TransacoesDTO> response = new();

            var contaBancaria = _context.ContaBancaria.FirstOrDefault(c => c.Id == saque.IdContaOrigem);
            if (contaBancaria == null)
            {
                response.Mensagem = "Conta bancária não encontrada";
                response.Sucesso = false;
                return response;
            }

            if (saque.Valor <= 0)
            {
                response.Mensagem = "Valor inválido";
                response.Sucesso = false;
                return response;
            }

            await AtualizaSaldoConta(contaBancaria, response, saque.Valor, TipoOperacao.Saque);
            await CriarRegistroTransacao(contaBancaria.Id, saque.Valor, TipoOperacao.Saque);
            return response;
        }

        public async Task<ServiceResponse<TransacoesDTO>> Transferencia(TransacoesTransferenciaDTO transferencia)
        {
            using var transaction = _context.Database.BeginTransaction();

            ServiceResponse<TransacoesDTO> response = new();

            try
            {
                var contaOrigem = _context.ContaBancaria.FirstOrDefault(c => c.Id == transferencia.IdContaOrigem);
                if (contaOrigem == null)
                {
                    response.Mensagem = "Conta bancária de origem não encontrada";
                    response.Sucesso = false;
                    return response;
                }

                if (contaOrigem.Saldo < transferencia.Valor)
                {
                    response.Mensagem = "Saldo insuficiente";
                    response.Sucesso = false;
                    return response;
                }

                var contaDestino = _context.ContaBancaria.FirstOrDefault(c => c.Conta == transferencia.ContaDestino && c.Agencia == transferencia.AgenciaDestino);
                if (contaDestino == null)
                {
                    response.Mensagem = "Conta bancária de destino não encontrada";
                    response.Sucesso = false;
                    return response;
                }

                await RealizaTransferencia(contaOrigem, contaDestino, response, transferencia.Valor);
                await CriarRegistroTransacao(contaOrigem.Id, transferencia.Valor, TipoOperacao.Transferencia, contaDestino.Id);

                transaction.Commit();
            }
            catch (Exception)
            {
                transaction.Rollback();
                response.Mensagem = "Erro ao realizar transferência";
                response.Sucesso = false;
            }

            return response;
        }

        public async Task<ServiceResponse<ContaBancariaSaldoDTO>> Saldo(int idConta)
        {
            ServiceResponse<ContaBancariaSaldoDTO> response = new();

            var contaBancaria = _context.ContaBancaria.FirstOrDefault(c => c.Id == idConta);
            if (contaBancaria == null)
            {
                response.Mensagem = "Conta bancária não encontrada";
                response.Sucesso = false;
                return response;
            }

            response.Dados = new ContaBancariaSaldoDTO
            {
                SaldoConta = $"Saldo disponível: R$ {contaBancaria.Saldo}"
            };

            return response;
        }

        public async Task<ServiceResponse<List<ExtratoDTO>>> Extrato(int idConta)
        {
            ServiceResponse<List<ExtratoDTO>> response = new();

            var transacoes = _context.Transacoes.Where(t => t.FK_Conta == idConta).ToList();
            if (!transacoes.Any())
            {
                response.Mensagem = "Nenhuma transação encontrada para esta conta";
                response.Sucesso = false;
                return response;
            }

            response.Dados = new List<ExtratoDTO>();
            for (int i = 0; i < transacoes.Count; i++)
            {
                var transacao = transacoes[i];

                var contaBancaria = _context.ContaBancaria.FirstOrDefault(c => c.Id == transacao.FK_Conta);
                if (contaBancaria == null)
                    continue;

                response.Dados.Add(new ExtratoDTO
                {
                    TipoOperacao = transacao.TipoOperacao.GetDescription(),
                    RazaoSocialContaDestino = contaBancaria.RazaoSocial,
                    Valor = transacao.Valor,
                });
            }

            return response;
        }

        private async Task<ServiceResponse<TransacoesDTO>> AtualizaSaldoConta(ContaBancaria contaBancaria, ServiceResponse<TransacoesDTO> response, decimal valor, TipoOperacao tipoOperacao)
        {
            switch (tipoOperacao)
            {
                case TipoOperacao.Saque:

                    if (contaBancaria.Saldo < valor)
                    {
                        response.Mensagem = "Saldo insuficiente";
                        response.Sucesso = false;
                        return response;
                    }

                    contaBancaria.Saldo -= valor;
                    response.Mensagem = "Saque efetuado com sucesso";
                    break;

                case TipoOperacao.Deposito:
                    contaBancaria.Saldo += valor;
                    response.Mensagem = "Depósito efetuado com sucesso";
                    break;
            }

            _context.ContaBancaria.Update(contaBancaria);
            await _context.SaveChangesAsync();
            return response;
        }

        private async Task<ServiceResponse<TransacoesDTO>> CriarRegistroTransacao(int idConta, decimal valorTransacao, TipoOperacao tipoOperacao, int? contaDestino = null)
        {
            ServiceResponse<TransacoesDTO> response = new();

            Transacoes transacoes = new()
            {
                FK_Conta = idConta,
                TipoOperacao = tipoOperacao,
                Valor = valorTransacao,
                ContaDestino_Id = contaDestino
            };

            _context.Transacoes.Add(transacoes);
            await _context.SaveChangesAsync();
            return response;
        }

        private async Task<ServiceResponse<TransacoesDTO>> RealizaTransferencia(ContaBancaria contaOrigem, ContaBancaria contaDestino, ServiceResponse<TransacoesDTO> response, decimal valor)
        {
            contaOrigem.Saldo -= valor;
            contaDestino.Saldo += valor;

            _context.ContaBancaria.Update(contaOrigem);
            _context.ContaBancaria.Update(contaDestino);
            await _context.SaveChangesAsync();

            response.Mensagem = "Transferência realizada com sucesso";
            response.Sucesso = true;
            return response;
        }
    }
}
