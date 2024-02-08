using Microsoft.AspNetCore.Mvc;
using WebAPI.DTOs;
using WebAPI.Models;
using WebAPI.Service.ContaBancariaServices;

namespace WebAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ContaBancariaController : ControllerBase
    {
        private readonly IContaBancariaInterface _contaBancariaInterface;

        public ContaBancariaController(IContaBancariaInterface contaBancariaInterface) => _contaBancariaInterface = contaBancariaInterface;

        [HttpGet("{cnpj}")]
        public async Task<ActionResult<ServiceResponse<ContaBancaria>>> GetContaBancariaPorCnpj(string cnpj)
        {
            ServiceResponse<ContaBancariaDTO> serviceResponse = await _contaBancariaInterface.GetContaBancariaPorCnpj(cnpj);
            return Ok(serviceResponse);
        }

        [HttpGet]
        public async Task<ActionResult<ServiceResponse<List<ContaBancaria>>>> GetTodasContasBancarias()
        {
            return Ok(await _contaBancariaInterface.GetTodasContasBancarias());
        }

        [HttpPost]
        public async Task<ActionResult<ServiceResponse<List<ContaBancaria>>>> CreateContaBancaria(ContaBancariaAddDTO contaBancaria)
        {
            return Ok(await _contaBancariaInterface.CreateContaBancaria(contaBancaria));
        }

        [HttpPut]
        public async Task<ActionResult<ServiceResponse<ContaBancaria>>> UpdateContaBancariaById(ContaBancariaUpdateIdDTO contaBancaria)
        {
            ServiceResponse<ContaBancariaDTO> serviceResponse = await _contaBancariaInterface.UpdateContaBancariaById(contaBancaria);
            return Ok(serviceResponse);
        }

        [HttpDelete("{id}")]
        public async Task<ActionResult<ServiceResponse<ContaBancaria>>> DeleteContaBancaria(int id)
        {
            ServiceResponse<ContaBancariaDTO> serviceResponse = await _contaBancariaInterface.DeleteContaBancaria(id);
            return Ok(serviceResponse);
        }
    }
}
