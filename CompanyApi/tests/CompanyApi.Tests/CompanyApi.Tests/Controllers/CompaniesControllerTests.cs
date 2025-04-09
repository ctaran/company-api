using CompanyApi.Controllers;
using CompanyApi.DTOs;
using CompanyApi.Services;
using Microsoft.AspNetCore.Mvc;
using Moq;
using Xunit;

namespace CompanyApi.Tests.Controllers;

public class CompaniesControllerTests
{
    private readonly Mock<ICompanyService> _serviceMock;
    private readonly CompaniesController _controller;
    private readonly DateTime _testDate = DateTime.UtcNow;

    public CompaniesControllerTests()
    {
        _serviceMock = new Mock<ICompanyService>();
        _controller = new CompaniesController(_serviceMock.Object);
    }

    [Fact]
    public async Task GetAllCompanies_Should_Return_OkResult_With_Companies()
    {
        // Arrange
        var companies = new List<CompanyDto>
        {
            new(1, "Company 1", "COMP1", "NYSE", "US1234567890", null, _testDate, _testDate),
            new(2, "Company 2", "COMP2", "NYSE", "US0987654321", null, _testDate, _testDate)
        };
        _serviceMock.Setup(s => s.GetAllCompaniesAsync()).ReturnsAsync(companies);

        // Act
        var result = await _controller.GetAllCompanies();

        // Assert
        var okResult = Assert.IsType<OkObjectResult>(result.Result);
        var returnedCompanies = Assert.IsType<List<CompanyDto>>(okResult.Value);
        Assert.Equal(companies, returnedCompanies);
    }

    [Fact]
    public async Task GetCompanyById_Should_Return_OkResult_When_Company_Exists()
    {
        // Arrange
        var company = new CompanyDto(1, "Company 1", "COMP1", "NYSE", "US1234567890", null, _testDate, _testDate);
        _serviceMock.Setup(s => s.GetCompanyByIdAsync(1)).ReturnsAsync(company);

        // Act
        var result = await _controller.GetCompanyById(1);

        // Assert
        var okResult = Assert.IsType<OkObjectResult>(result.Result);
        var returnedCompany = Assert.IsType<CompanyDto>(okResult.Value);
        Assert.Equal(company, returnedCompany);
    }

    [Fact]
    public async Task GetCompanyById_Should_Return_NotFound_When_Company_Does_Not_Exist()
    {
        // Arrange
        _serviceMock.Setup(s => s.GetCompanyByIdAsync(1)).ReturnsAsync((CompanyDto?)null);

        // Act
        var result = await _controller.GetCompanyById(1);

        // Assert
        Assert.IsType<NotFoundResult>(result.Result);
    }

    [Fact]
    public async Task GetCompanyByIsin_Should_Return_OkResult_When_Company_Exists()
    {
        // Arrange
        var company = new CompanyDto(1, "Company 1", "COMP1", "NYSE", "US1234567890", null, _testDate, _testDate);
        _serviceMock.Setup(s => s.GetByIsinAsync("US1234567890")).ReturnsAsync(company);

        // Act
        var result = await _controller.GetCompanyByIsin("US1234567890");

        // Assert
        var okResult = Assert.IsType<OkObjectResult>(result.Result);
        var returnedCompany = Assert.IsType<CompanyDto>(okResult.Value);
        Assert.Equal(company, returnedCompany);
    }

    [Fact]
    public async Task GetCompanyByIsin_Should_Return_NotFound_When_Company_Does_Not_Exist()
    {
        // Arrange
        _serviceMock.Setup(s => s.GetByIsinAsync("US1234567890")).ReturnsAsync((CompanyDto?)null);

        // Act
        var result = await _controller.GetCompanyByIsin("US1234567890");

        // Assert
        Assert.IsType<NotFoundResult>(result.Result);
    }

    [Fact]
    public async Task CreateCompany_Should_Return_CreatedAtAction_When_Valid()
    {
        // Arrange
        var companyDto = new CompanyDto(0, "New Company", "NEWC", "NYSE", "US1234567890", null, _testDate, _testDate);
        var createdCompany = new CompanyDto(1, "New Company", "NEWC", "NYSE", "US1234567890", null, _testDate, _testDate);
        _serviceMock.Setup(s => s.CreateCompanyAsync(companyDto)).ReturnsAsync(createdCompany);

        // Act
        var result = await _controller.CreateCompany(companyDto);

        // Assert
        var createdAtResult = Assert.IsType<CreatedAtActionResult>(result.Result);
        Assert.Equal(nameof(CompaniesController.GetCompanyById), createdAtResult.ActionName);
        Assert.NotNull(createdAtResult.RouteValues);
        Assert.Equal(createdCompany.Id, createdAtResult.RouteValues["id"]);
        var returnedCompany = Assert.IsType<CompanyDto>(createdAtResult.Value);
        Assert.Equal(createdCompany, returnedCompany);
    }

    [Fact]
    public async Task CreateCompany_Should_Return_Conflict_When_Isin_Exists()
    {
        // Arrange
        var companyDto = new CompanyDto(0, "New Company", "NEWC", "NYSE", "US1234567890", null, _testDate, _testDate);
        _serviceMock.Setup(s => s.CreateCompanyAsync(companyDto))
            .ThrowsAsync(new InvalidOperationException("A company with ISIN US1234567890 already exists."));

        // Act
        var result = await _controller.CreateCompany(companyDto);

        // Assert
        var conflictResult = Assert.IsType<ConflictObjectResult>(result.Result);
        Assert.Equal("A company with ISIN US1234567890 already exists.", conflictResult.Value);
    }

    [Fact]
    public async Task UpdateCompany_Should_Return_OkResult_When_Successful()
    {
        // Arrange
        var companyDto = new CompanyDto(1, "Updated Company", "UPDC", "NYSE", "US1234567890", null, _testDate, _testDate);
        _serviceMock.Setup(s => s.UpdateCompanyAsync(1, companyDto)).ReturnsAsync(companyDto);

        // Act
        var result = await _controller.UpdateCompany(1, companyDto);

        // Assert
        var okResult = Assert.IsType<OkObjectResult>(result.Result);
        var returnedCompany = Assert.IsType<CompanyDto>(okResult.Value);
        Assert.Equal(companyDto, returnedCompany);
    }

    [Fact]
    public async Task UpdateCompany_Should_Return_NotFound_When_Company_Does_Not_Exist()
    {
        // Arrange
        var companyDto = new CompanyDto(1, "Updated Company", "UPDC", "NYSE", "US1234567890", null, _testDate, _testDate);
        _serviceMock.Setup(s => s.UpdateCompanyAsync(1, companyDto)).ReturnsAsync((CompanyDto?)null);

        // Act
        var result = await _controller.UpdateCompany(1, companyDto);

        // Assert
        Assert.IsType<NotFoundResult>(result.Result);
    }

    [Fact]
    public async Task UpdateCompany_Should_Return_Conflict_When_Isin_Exists()
    {
        // Arrange
        var companyDto = new CompanyDto(1, "Updated Company", "UPDC", "NYSE", "US1234567890", null, _testDate, _testDate);
        _serviceMock.Setup(s => s.UpdateCompanyAsync(1, companyDto))
            .ThrowsAsync(new InvalidOperationException("A company with ISIN US1234567890 already exists."));

        // Act
        var result = await _controller.UpdateCompany(1, companyDto);

        // Assert
        var conflictResult = Assert.IsType<ConflictObjectResult>(result.Result);
        Assert.Equal("A company with ISIN US1234567890 already exists.", conflictResult.Value);
    }

    [Fact]
    public async Task DeleteCompany_Should_Return_NoContent_When_Successful()
    {
        // Arrange
        _serviceMock.Setup(s => s.DeleteCompanyAsync(1)).ReturnsAsync(true);

        // Act
        var result = await _controller.DeleteCompany(1);

        // Assert
        Assert.IsType<NoContentResult>(result);
    }

    [Fact]
    public async Task DeleteCompany_Should_Return_NotFound_When_Company_Does_Not_Exist()
    {
        // Arrange
        _serviceMock.Setup(s => s.DeleteCompanyAsync(1)).ReturnsAsync(false);

        // Act
        var result = await _controller.DeleteCompany(1);

        // Assert
        Assert.IsType<NotFoundResult>(result);
    }
} 