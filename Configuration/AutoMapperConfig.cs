using AutoMapper;
using WebAPI.DTOs;
using WebAPI.Helpers;
using WebAPI.Models;

namespace WebAPI.Configuration
{
    public class AutoMapperConfig : Profile
    {
        public AutoMapperConfig()
        {
            CreateMap<ContaBancaria, ContaBancariaUpdateIdDTO>().ReverseMap();
            CreateMap<ContaBancaria, ContaBancariaAddDTO>().ReverseMap();
            CreateMap<ContaBancaria, ContaBancariaDTO>().ReverseMap();
        }
    }
}
