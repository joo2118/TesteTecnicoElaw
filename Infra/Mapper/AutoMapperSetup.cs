using Application.DTOs;
using Domain.Entities;

using AutoMapper;


namespace Infra.Mapper
{
    public class AutoMapperSetup : Profile
    {
        public AutoMapperSetup()
        {
            CreateMap<ClienteDTO, Cliente>().CreateValidatedReverseMap();
            CreateMap<EnderecoDTO, Endereco>().CreateValidatedReverseMap();
        }
    }
}
