using CompanyApi.DTOs;
using CompanyApi.Services;
using Microsoft.AspNetCore.Mvc;

namespace CompanyApi.Controllers;

[ApiController]
[Route("api/[controller]")]
public class CompaniesController : ControllerBase
{
    private readonly ICompanyService _service;

    public CompaniesController(ICompanyService service)
    {
        _service = service;
    }

    [HttpGet]
    public async Task<ActionResult<IEnumerable<CompanyDto>>> GetAllCompanies()
    {
        var companies = await _service.GetAllCompaniesAsync();
        return Ok(companies);
    }

    [HttpGet("{id:int}")]
    public async Task<ActionResult<CompanyDto>> GetCompanyById(int id)
    {
        var company = await _service.GetCompanyByIdAsync(id);
        if (company == null)
        {
            return NotFound();
        }
        return Ok(company);
    }

    [HttpGet("isin/{isin}")]
    public async Task<ActionResult<CompanyDto>> GetCompanyByIsin(string isin)
    {
        var company = await _service.GetByIsinAsync(isin);
        if (company == null)
        {
            return NotFound();
        }
        return Ok(company);
    }

    [HttpPost]
    public async Task<ActionResult<CompanyDto>> CreateCompany(CompanyDto dto)
    {
        try
        {
            var createdCompany = await _service.CreateCompanyAsync(dto);
            return CreatedAtAction(nameof(GetCompanyById), new { id = createdCompany.Id }, createdCompany);
        }
        catch (InvalidOperationException ex)
        {
            return Conflict(ex.Message);
        }
    }

    [HttpPut("{id:int}")]
    public async Task<ActionResult<CompanyDto>> UpdateCompany(int id, CompanyDto dto)
    {
        try
        {
            var updatedCompany = await _service.UpdateCompanyAsync(id, dto);
            if (updatedCompany == null)
            {
                return NotFound();
            }
            return Ok(updatedCompany);
        }
        catch (InvalidOperationException ex)
        {
            return Conflict(ex.Message);
        }
    }

    [HttpDelete("{id:int}")]
    public async Task<IActionResult> DeleteCompany(int id)
    {
        var result = await _service.DeleteCompanyAsync(id);
        if (!result)
        {
            return NotFound();
        }
        return NoContent();
    }
} 