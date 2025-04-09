using CompanyApi.DTOs;

namespace CompanyApi.Services;

public interface ICompanyService
{
    Task<IEnumerable<CompanyDto>> GetAllCompaniesAsync();
    Task<CompanyDto?> GetCompanyByIdAsync(int id);
    Task<CompanyDto?> GetByIsinAsync(string isin);
    Task<CompanyDto> CreateCompanyAsync(CompanyDto dto);
    Task<CompanyDto?> UpdateCompanyAsync(int id, CompanyDto dto);
    Task<bool> DeleteCompanyAsync(int id);
} 