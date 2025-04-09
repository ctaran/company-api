using CompanyApi.Models;

namespace CompanyApi.Repositories;

public interface ICompanyRepository
{
    Task<Company?> GetByIdAsync(int id);
    Task<Company?> GetByIsinAsync(string isin);
    Task<IEnumerable<Company>> GetAllAsync();
    Task<Company> CreateAsync(Company company);
    Task<Company> UpdateAsync(Company company);
    Task<bool> ExistsByIsinAsync(string isin);
    Task<bool> DeleteAsync(Company company);
} 