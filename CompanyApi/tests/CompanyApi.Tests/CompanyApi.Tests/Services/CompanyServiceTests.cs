using AutoMapper;
using CompanyApi.DTOs;
using CompanyApi.Models;
using CompanyApi.Repositories;
using CompanyApi.Services;
using Moq;
using Xunit;

namespace CompanyApi.Tests.Services;

public class CompanyServiceTests
{
    private readonly Mock<ICompanyRepository> _repositoryMock;
    private readonly Mock<IMapper> _mapperMock;
    private readonly CompanyService _service;
    private readonly DateTime _testDate = DateTime.UtcNow;

    public CompanyServiceTests()
    {
        _repositoryMock = new Mock<ICompanyRepository>();
        _mapperMock = new Mock<IMapper>();
        _service = new CompanyService(_repositoryMock.Object, _mapperMock.Object);
    }

    [Fact]
    public async Task GetAllCompanies_Should_Return_All_Companies()
    {
        // Arrange
        var companies = new List<Company>
        {
            new() { Id = 1, Name = "Company 1" },
            new() { Id = 2, Name = "Company 2" }
        };
        var companyDtos = new List<CompanyDto>
        {
            new(1, "Company 1", "COMP1", "NYSE", "US1234567890", null, _testDate, _testDate),
            new(2, "Company 2", "COMP2", "NYSE", "US0987654321", null, _testDate, _testDate)
        };

        _repositoryMock.Setup(r => r.GetAllAsync()).ReturnsAsync(companies);
        _mapperMock.Setup(m => m.Map<IEnumerable<CompanyDto>>(companies)).Returns(companyDtos);

        // Act
        var result = await _service.GetAllCompaniesAsync();

        // Assert
        Assert.Equal(companyDtos, result);
        _repositoryMock.Verify(r => r.GetAllAsync(), Times.Once);
    }

    [Fact]
    public async Task GetCompanyById_Should_Return_Company_When_Exists()
    {
        // Arrange
        var company = new Company { Id = 1, Name = "Company 1" };
        var companyDto = new CompanyDto(1, "Company 1", "COMP1", "NYSE", "US1234567890", null, _testDate, _testDate);

        _repositoryMock.Setup(r => r.GetByIdAsync(1)).ReturnsAsync(company);
        _mapperMock.Setup(m => m.Map<CompanyDto>(company)).Returns(companyDto);

        // Act
        var result = await _service.GetCompanyByIdAsync(1);

        // Assert
        Assert.Equal(companyDto, result);
        _repositoryMock.Verify(r => r.GetByIdAsync(1), Times.Once);
    }

    [Fact]
    public async Task GetCompanyById_Should_Return_Null_When_Not_Exists()
    {
        // Arrange
        _repositoryMock.Setup(r => r.GetByIdAsync(1)).ReturnsAsync((Company?)null);

        // Act
        var result = await _service.GetCompanyByIdAsync(1);

        // Assert
        Assert.Null(result);
        _repositoryMock.Verify(r => r.GetByIdAsync(1), Times.Once);
    }

    [Fact]
    public async Task GetByIsin_Should_Return_Company_When_Exists()
    {
        // Arrange
        var company = new Company { Id = 1, Name = "Company 1", Isin = "US1234567890" };
        var companyDto = new CompanyDto(1, "Company 1", "COMP1", "NYSE", "US1234567890", null, _testDate, _testDate);

        _repositoryMock.Setup(r => r.GetByIsinAsync("US1234567890")).ReturnsAsync(company);
        _mapperMock.Setup(m => m.Map<CompanyDto>(company)).Returns(companyDto);

        // Act
        var result = await _service.GetByIsinAsync("US1234567890");

        // Assert
        Assert.Equal(companyDto, result);
        _repositoryMock.Verify(r => r.GetByIsinAsync("US1234567890"), Times.Once);
    }

    [Fact]
    public async Task GetByIsin_Should_Return_Null_When_Not_Exists()
    {
        // Arrange
        _repositoryMock.Setup(r => r.GetByIsinAsync("US1234567890")).ReturnsAsync((Company?)null);

        // Act
        var result = await _service.GetByIsinAsync("US1234567890");

        // Assert
        Assert.Null(result);
        _repositoryMock.Verify(r => r.GetByIsinAsync("US1234567890"), Times.Once);
    }

    [Fact]
    public async Task CreateCompany_Should_Create_And_Return_Company()
    {
        // Arrange
        var companyDto = new CompanyDto(0, "New Company", "NEWC", "NYSE", "US1234567890", null, _testDate, _testDate);
        var company = new Company { Name = "New Company", StockTicker = "NEWC", Exchange = "NYSE", Isin = "US1234567890" };

        _repositoryMock.Setup(r => r.ExistsByIsinAsync("US1234567890")).ReturnsAsync(false);
        _mapperMock.Setup(m => m.Map<Company>(companyDto)).Returns(company);
        _repositoryMock.Setup(r => r.CreateAsync(company)).ReturnsAsync(company);
        _mapperMock.Setup(m => m.Map<CompanyDto>(company)).Returns(companyDto);

        // Act
        var result = await _service.CreateCompanyAsync(companyDto);

        // Assert
        Assert.Equal(companyDto, result);
        _repositoryMock.Verify(r => r.CreateAsync(company), Times.Once);
    }

    [Fact]
    public async Task CreateCompany_Should_Throw_When_Isin_Exists()
    {
        // Arrange
        var companyDto = new CompanyDto(0, "New Company", "NEWC", "NYSE", "US1234567890", null, _testDate, _testDate);

        _repositoryMock.Setup(r => r.ExistsByIsinAsync("US1234567890")).ReturnsAsync(true);

        // Act & Assert
        await Assert.ThrowsAsync<InvalidOperationException>(() => _service.CreateCompanyAsync(companyDto));
        _repositoryMock.Verify(r => r.CreateAsync(It.IsAny<Company>()), Times.Never);
    }

    [Fact]
    public async Task UpdateCompany_Should_Update_And_Return_Company_When_Exists()
    {
        // Arrange
        var companyDto = new CompanyDto(1, "Updated Company", "UPDC", "NYSE", "US1234567890", null, _testDate, _testDate);
        var existingCompany = new Company 
        { 
            Id = 1, 
            Name = "Old Company", 
            StockTicker = "OLDC", 
            Exchange = "NYSE", 
            Isin = "US1234567890",
            CreatedAt = _testDate,
            UpdatedAt = _testDate
        };
        var updatedCompany = new Company 
        { 
            Id = 1, 
            Name = "Updated Company", 
            StockTicker = "UPDC", 
            Exchange = "NYSE", 
            Isin = "US1234567890",
            CreatedAt = _testDate,
            UpdatedAt = _testDate
        };

        _repositoryMock.Setup(r => r.GetByIdAsync(1)).ReturnsAsync(existingCompany);
        _repositoryMock.Setup(r => r.ExistsByIsinAsync("US1234567890")).ReturnsAsync(false);
        _mapperMock.Setup(m => m.Map<Company>(companyDto)).Returns(updatedCompany);
        _repositoryMock.Setup(r => r.UpdateAsync(It.Is<Company>(c => 
            c.Id == 1 && 
            c.Name == "Updated Company" && 
            c.StockTicker == "UPDC" && 
            c.Exchange == "NYSE" && 
            c.Isin == "US1234567890" &&
            c.CreatedAt == _testDate
        ))).ReturnsAsync(updatedCompany);
        _mapperMock.Setup(m => m.Map<CompanyDto>(updatedCompany)).Returns(companyDto);

        // Act
        var result = await _service.UpdateCompanyAsync(1, companyDto);

        // Assert
        Assert.Equal(companyDto, result);
        _repositoryMock.Verify(r => r.UpdateAsync(It.Is<Company>(c => 
            c.Id == 1 && 
            c.Name == "Updated Company" && 
            c.StockTicker == "UPDC" && 
            c.Exchange == "NYSE" && 
            c.Isin == "US1234567890" &&
            c.CreatedAt == _testDate
        )), Times.Once);
    }

    [Fact]
    public async Task UpdateCompany_Should_Return_Null_When_Not_Exists()
    {
        // Arrange
        var companyDto = new CompanyDto(1, "Updated Company", "UPDC", "NYSE", "US1234567890", null, _testDate, _testDate);
        _repositoryMock.Setup(r => r.GetByIdAsync(1)).ReturnsAsync((Company?)null);

        // Act
        var result = await _service.UpdateCompanyAsync(1, companyDto);

        // Assert
        Assert.Null(result);
        _repositoryMock.Verify(r => r.UpdateAsync(It.IsAny<Company>()), Times.Never);
    }

    [Fact]
    public async Task UpdateCompany_Should_Throw_When_Isin_Exists_And_Different()
    {
        // Arrange
        var companyDto = new CompanyDto(1, "Updated Company", "UPDC", "NYSE", "US9876543210", null, _testDate, _testDate);
        var existingCompany = new Company { Id = 1, Name = "Old Company", StockTicker = "OLDC", Exchange = "NYSE", Isin = "US1234567890" };

        _repositoryMock.Setup(r => r.GetByIdAsync(1)).ReturnsAsync(existingCompany);
        _repositoryMock.Setup(r => r.ExistsByIsinAsync("US9876543210")).ReturnsAsync(true);

        // Act & Assert
        await Assert.ThrowsAsync<InvalidOperationException>(() => _service.UpdateCompanyAsync(1, companyDto));
        _repositoryMock.Verify(r => r.UpdateAsync(It.IsAny<Company>()), Times.Never);
    }

    [Fact]
    public async Task DeleteCompany_Should_Return_True_When_Exists()
    {
        // Arrange
        var company = new Company { Id = 1, Name = "Company 1" };
        _repositoryMock.Setup(r => r.GetByIdAsync(1)).ReturnsAsync(company);
        _repositoryMock.Setup(r => r.DeleteAsync(company)).ReturnsAsync(true);

        // Act
        var result = await _service.DeleteCompanyAsync(1);

        // Assert
        Assert.True(result);
        _repositoryMock.Verify(r => r.DeleteAsync(company), Times.Once);
    }

    [Fact]
    public async Task DeleteCompany_Should_Return_False_When_Not_Exists()
    {
        // Arrange
        _repositoryMock.Setup(r => r.GetByIdAsync(1)).ReturnsAsync((Company?)null);

        // Act
        var result = await _service.DeleteCompanyAsync(1);

        // Assert
        Assert.False(result);
        _repositoryMock.Verify(r => r.DeleteAsync(It.IsAny<Company>()), Times.Never);
    }
} 