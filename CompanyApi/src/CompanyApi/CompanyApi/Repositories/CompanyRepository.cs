using CompanyApi.Data;
using CompanyApi.Models;
using Microsoft.EntityFrameworkCore;

namespace CompanyApi.Repositories;

public class CompanyRepository : ICompanyRepository
{
    private readonly ApplicationDbContext _context;

    public CompanyRepository(ApplicationDbContext context)
    {
        _context = context;
    }

    public async Task<Company?> GetByIdAsync(int id)
    {
        return await _context.Companies.FindAsync(id);
    }

    public async Task<Company?> GetByIsinAsync(string isin)
    {
        return await _context.Companies
            .FirstOrDefaultAsync(c => c.Isin == isin);
    }

    public async Task<IEnumerable<Company>> GetAllAsync()
    {
        return await _context.Companies.ToListAsync();
    }

    public async Task<Company> CreateAsync(Company company)
    {
        _context.Companies.Add(company);
        await _context.SaveChangesAsync();
        return company;
    }

    public async Task<Company> UpdateAsync(Company company)
    {
        company.UpdatedAt = DateTime.UtcNow;
        
        // Get the existing entity
        var existingCompany = await _context.Companies.FindAsync(company.Id);
        if (existingCompany == null)
        {
            throw new InvalidOperationException($"Company with ID {company.Id} not found.");
        }
        
        // Update properties
        _context.Entry(existingCompany).CurrentValues.SetValues(company);
        
        await _context.SaveChangesAsync();
        return existingCompany;
    }

    public async Task<bool> ExistsByIsinAsync(string isin)
    {
        return await _context.Companies.AnyAsync(c => c.Isin == isin);
    }

    public async Task<bool> DeleteAsync(Company company)
    {
        _context.Companies.Remove(company);
        var deleted = await _context.SaveChangesAsync();
        return deleted > 0;
    }
} 