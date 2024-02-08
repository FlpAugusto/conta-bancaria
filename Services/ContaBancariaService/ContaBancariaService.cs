using AutoMapper;
using System.Net;
using System.Text.Json;
using System.Text.RegularExpressions;
using WebAPI.DataContext;
using WebAPI.DTOs;
using WebAPI.ExternalModels;
using WebAPI.Models;

namespace WebAPI.Service.ContaBancariaServices
{
    public class ContaBancariaService : IContaBancariaInterface
    {
        private readonly ApplicationDbContext _context;
        private readonly IMapper _mapper;

        public ContaBancariaService(ApplicationDbContext context, IMapper mapper)
        {
            _context = context;
            _mapper = mapper;
        }

        public async Task<ServiceResponse<ContaBancariaDTO>> GetContaBancariaPorCnpj(string cnpj)
        {
            ServiceResponse<ContaBancariaDTO> response = new();

            cnpj = Regex.Replace(cnpj, "[^0-9]", "");

            try
            {
                var contaBancaria = _context.ContaBancaria.FirstOrDefault(c => c.CNPJ == cnpj);
                response.Dados = _mapper.Map<ContaBancariaDTO>(contaBancaria);
                if (response.Dados == null)
                {
                    response.Mensagem = "Conta bancária não encontrada";
                    response.Sucesso = false;
                    return response;
                }
            }
            catch (Exception ex)
            {
                response.Mensagem = ex.Message;
                response.Sucesso = false;
            }

            return response;
        }

        public async Task<ServiceResponse<List<ContaBancariaDTO>>> GetTodasContasBancarias()
        {
            ServiceResponse<List<ContaBancariaDTO>> response = new();

            try
            {
                var contasBancarias = _context.ContaBancaria.ToList();
                response.Dados = contasBancarias.Select(c => _mapper.Map<ContaBancariaDTO>(c)).ToList();
                if (!response.Dados.Any())
                    response.Mensagem = "Nenhuma conta bancária cadastrada";
            }
            catch (Exception ex)
            {
                response.Mensagem = ex.Message;
                response.Sucesso = false;
            }

            return response;
        }

        public async Task<ServiceResponse<ContaBancariaDTO>> CreateContaBancaria(ContaBancariaAddDTO contaNova)
        {
            ServiceResponse<ContaBancariaDTO> response = new();
            var contaBD = _mapper.Map<ContaBancaria>(contaNova);

            if (PropriedadesObrigatoriasContaBancaria(contaBD, response))
            {
                if (ValidaCnpjCadastrado(contaBD.CNPJ))
                {
                    response.Mensagem = "CNPJ já cadastrado";
                    response.Sucesso = false;
                    return response;
                }

                if (_context.ContaBancaria.Any(c => c.Agencia == contaBD.Agencia && c.Conta == contaBD.Conta))
                {
                    response.Mensagem = "Conta bancária já cadastrada";
                    response.Sucesso = false;
                    return response;
                }

                var receitaWSResponse = await BuscaEmpresa(contaBD);

                switch (contaBD.StatusCode)
                {
                    case HttpStatusCode.TooManyRequests:
                        response.Mensagem = "Limite de requisições atingido";
                        response.Sucesso = false;
                        return response;

                    case HttpStatusCode.GatewayTimeout:
                        response.Mensagem = "Tempo excedido";
                        response.Sucesso = false;
                        return response;

                    case HttpStatusCode.OK:
                        break;

                    default:
                        response.Mensagem = "CNPJ inválido";
                        response.Sucesso = false;
                        return response;
                }

                contaBD.RazaoSocial = receitaWSResponse.Nome;

                if (!string.IsNullOrEmpty(contaNova.DocumentoBase64) && !string.IsNullOrWhiteSpace(contaNova.DocumentoBase64))
                    await UploadDocumento(contaNova.DocumentoBase64, contaBD);

                _context.ContaBancaria.Add(contaBD);
                await _context.SaveChangesAsync();
                response.Mensagem = "Conta bancária cadastrada com sucesso";

                var resultConta = _context.ContaBancaria.FirstOrDefault(c => c.CNPJ == contaBD.CNPJ);
                response.Dados = _mapper.Map<ContaBancariaDTO>(resultConta);
            }

            return response;
        }

        public async Task<ServiceResponse<ContaBancariaDTO>> UpdateContaBancariaById(ContaBancariaUpdateIdDTO contaAtualizada)
        {
            ServiceResponse<ContaBancariaDTO> response = new();
            var contaExistente = _mapper.Map<ContaBancaria>(contaAtualizada);
            contaExistente = _context.ContaBancaria.FirstOrDefault(c => c.Id == contaExistente.Id);
            if (contaExistente == null)
            {
                response.Mensagem = "Conta bancária não cadastrada";
                response.Sucesso = false;
                return response;
            }

            contaExistente.Conta = contaAtualizada.Conta;
            contaExistente.Agencia = contaAtualizada.Agencia;

            _context.ContaBancaria.Update(contaExistente);
            await _context.SaveChangesAsync();
            response.Mensagem = "Conta bancária atualizada com sucesso";

            var resultConta = _context.ContaBancaria.FirstOrDefault(c => c.Id == contaExistente.Id);
            response.Dados = _mapper.Map<ContaBancariaDTO>(resultConta);

            return response;
        }

        public async Task<ServiceResponse<ContaBancariaDTO>> DeleteContaBancaria(int id)
        {
            ServiceResponse<ContaBancariaDTO> response = new();

            ContaBancaria contaExistente = _context.ContaBancaria.FirstOrDefault(c => c.Id == id);
            if (contaExistente == null)
            {
                response.Dados = null;
                response.Mensagem = "Conta bancária não cadastrada";
                response.Sucesso = false;
                return response;
            }

            _context.ContaBancaria.Remove(contaExistente);
            await _context.SaveChangesAsync();

            response.Mensagem = "Conta bancária deletada com sucesso";
            response.Dados = _mapper.Map<ContaBancariaDTO>(contaExistente);
            return response;
        }

        private async Task<ReceitaWSResponse> BuscaEmpresa(ContaBancaria contaNova)
        {
            var url = new HttpRequestMessage(HttpMethod.Get, $"https://receitaws.com.br/v1/cnpj/{contaNova.CNPJ}");
            var response = new ReceitaWSResponse();

            using (var client = new HttpClient())
            {
                var request = await client.SendAsync(url);
                var contentResp = await request.Content.ReadAsStringAsync();
                contaNova.StatusCode = request.StatusCode;
                response = JsonSerializer.Deserialize<ReceitaWSResponse>(contentResp);
            }

            return response;
        }

        private bool PropriedadesObrigatoriasContaBancaria(ContaBancaria contaNova, ServiceResponse<ContaBancariaDTO> response)
        {
            if (contaNova == null)
            {
                response.Mensagem = "Conta inválida";
                response.Sucesso = false;
                return false;
            }

            if (string.IsNullOrEmpty(contaNova.CNPJ) || string.IsNullOrWhiteSpace(contaNova.CNPJ))
            {
                response.Mensagem = "CNPJ não foi informado";
                response.Sucesso = false;
                return false;
            }

            if (contaNova.Agencia == 0)
            {
                response.Mensagem = "Agência não foi informado";
                response.Sucesso = false;
                return false;
            }

            if (contaNova.Conta == 0)
            {
                response.Mensagem = "Conta não foi informado";
                response.Sucesso = false;
                return false;
            }

            contaNova.CNPJ = Regex.Replace(contaNova.CNPJ, "[^0-9]", "");
            return true;
        }

        private bool ValidaCnpjCadastrado(string cnpj)
        {
            return _context.ContaBancaria.Any(c => c.CNPJ == cnpj);
        }

        private async Task UploadDocumento(string base64File, ContaBancaria contaNova)
        {
            byte[] bytes = Convert.FromBase64String(base64File);
            string directoryPath = Path.Combine(Directory.GetCurrentDirectory(), "DocumentosDeContas");

            if (!Directory.Exists(directoryPath))
                Directory.CreateDirectory(directoryPath);

            string fileName = $"{Guid.NewGuid()}.jpg";
            string filePath = Path.Combine(directoryPath, fileName);
            contaNova.NomeDocumento = fileName;
            await File.WriteAllBytesAsync(filePath, bytes);
        }
    }
}
