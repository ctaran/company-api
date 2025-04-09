using AutoMapper;
using CompanyApi.DTOs;
using CompanyApi.Models;
using CompanyApi.Repositories;

namespace CompanyApi.Services;

public class CompanyService : ICompanyService
{
    private readonly ICompanyRepository _repository;
    private readonly IMapper _mapper;

    public CompanyService(ICompanyRepository repository, IMapper mapper)
    {
        _repository = repository;
        _mapper = mapper;
    }

    public async Task<IEnumerable<CompanyDto>> GetAllCompaniesAsync()
    {
        var companies = await _repository.GetAllAsync();
        return _mapper.Map<IEnumerable<CompanyDto>>(companies);
    }

    public async Task<CompanyDto?> GetCompanyByIdAsync(int id)
    {
        var company = await _repository.GetByIdAsync(id);
        return company != null ? _mapper.Map<CompanyDto>(company) : null;
    }

    public async Task<CompanyDto?> GetByIsinAsync(string isin)
    {
        var company = await _repository.GetByIsinAsync(isin);
        return company != null ? _mapper.Map<CompanyDto>(company) : null;
    }

    public async Task<CompanyDto> CreateCompanyAsync(CompanyDto dto)
    {
        if (await _repository.ExistsByIsinAsync(dto.Isin))
        {
            throw new InvalidOperationException($"A company with ISIN {dto.Isin} already exists.");
        }

        var company = _mapper.Map<Company>(dto);
        var createdCompany = await _repository.CreateAsync(company);
        return _mapper.Map<CompanyDto>(createdCompany);
    }

    public async Task<CompanyDto?> UpdateCompanyAsync(int id, CompanyDto dto)
    {
        var existingCompany = await _repository.GetByIdAsync(id);
        if (existingCompany == null)
        {
            return null;
        }

        if (dto.Isin != existingCompany.Isin && await _repository.ExistsByIsinAsync(dto.Isin))
        {
            throw new InvalidOperationException($"A company with ISIN {dto.Isin} already exists.");
        }

        var updatedCompany = _mapper.Map<Company>(dto);
        updatedCompany.Id = id;
        updatedCompany.CreatedAt = existingCompany.CreatedAt;
        updatedCompany.UpdatedAt = DateTime.UtcNow;

        var result = await _repository.UpdateAsync(updatedCompany);
        return _mapper.Map<CompanyDto>(result);
    }

    public async Task<bool> DeleteCompanyAsync(int id)
    {
        var company = await _repository.GetByIdAsync(id);
        if (company == null)
        {
            return false;
        }

        return await _repository.DeleteAsync(company);
    }
} 