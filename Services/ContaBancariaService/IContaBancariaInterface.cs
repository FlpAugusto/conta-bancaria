using WebAPI.DTOs;
using WebAPI.Models;

namespace WebAPI.Service.ContaBancariaServices
{
    public interface IContaBancariaInterface
    {
        Task<ServiceResponse<ContaBancariaDTO>> GetContaBancariaPorCnpj(string cnpj);

        Task<ServiceResponse<List<ContaBancariaDTO>>> GetTodasContasBancarias();

        Task<ServiceResponse<ContaBancariaDTO>> CreateContaBancaria(ContaBancariaAddDTO contaBancaria);

        Task<ServiceResponse<ContaBancariaDTO>> UpdateContaBancariaById(ContaBancariaUpdateIdDTO contaBancaria);

        Task<ServiceResponse<ContaBancariaDTO>> DeleteContaBancaria(int id);
    }
}
