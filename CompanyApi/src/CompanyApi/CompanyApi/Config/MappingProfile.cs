using AutoMapper;
using CompanyApi.DTOs;
using CompanyApi.Models;

namespace CompanyApi.Config;

public class MappingProfile : Profile
{
    public MappingProfile()
    {
        CreateMap<Company, CompanyDto>();
        CreateMap<CompanyDto, Company>();
        CreateMap<CreateCompanyDto, Company>();
        CreateMap<UpdateCompanyDto, Company>();
    }
} 