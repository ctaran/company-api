using CompanyApi.DTOs;
using CompanyApi.Validators;
using FluentValidation;
using Xunit;

namespace CompanyApi.Tests.Validators;

public class CompanyValidatorTests
{
    private readonly CreateCompanyDtoValidator _createValidator;
    private readonly UpdateCompanyDtoValidator _updateValidator;

    public CompanyValidatorTests()
    {
        _createValidator = new CreateCompanyDtoValidator();
        _updateValidator = new UpdateCompanyDtoValidator();
    }

    [Fact]
    public void Should_Have_Error_When_Name_Is_Empty_Create()
    {
        // Arrange
        var company = new CreateCompanyDto("", "TEST", "NYSE", "US1234567890", null);

        // Act
        var result = _createValidator.Validate(company);

        // Assert
        Assert.False(result.IsValid);
        Assert.Contains(result.Errors, e => e.PropertyName == nameof(CreateCompanyDto.Name));
    }

    [Fact]
    public void Should_Have_Error_When_StockTicker_Is_Empty_Create()
    {
        // Arrange
        var company = new CreateCompanyDto("Test Company", "", "NYSE", "US1234567890", null);

        // Act
        var result = _createValidator.Validate(company);

        // Assert
        Assert.False(result.IsValid);
        Assert.Contains(result.Errors, e => e.PropertyName == nameof(CreateCompanyDto.StockTicker));
    }

    [Fact]
    public void Should_Have_Error_When_Exchange_Is_Empty_Create()
    {
        // Arrange
        var company = new CreateCompanyDto("Test Company", "TEST", "", "US1234567890", null);

        // Act
        var result = _createValidator.Validate(company);

        // Assert
        Assert.False(result.IsValid);
        Assert.Contains(result.Errors, e => e.PropertyName == nameof(CreateCompanyDto.Exchange));
    }

    [Fact]
    public void Should_Have_Error_When_Isin_Is_Invalid_Create()
    {
        // Arrange
        var company = new CreateCompanyDto("Test Company", "TEST", "NYSE", "INVALID", null);

        // Act
        var result = _createValidator.Validate(company);

        // Assert
        Assert.False(result.IsValid);
        Assert.Contains(result.Errors, e => e.PropertyName == nameof(CreateCompanyDto.Isin));
    }

    [Fact]
    public void Should_Not_Have_Error_When_All_Fields_Are_Valid_Create()
    {
        // Arrange
        var company = new CreateCompanyDto(
            "Test Company",
            "TEST",
            "NYSE",
            "US1234567890",
            "https://test.com"
        );

        // Act
        var result = _createValidator.Validate(company);

        // Assert
        Assert.True(result.IsValid);
        Assert.Empty(result.Errors);
    }

    [Fact]
    public void Should_Have_Error_When_Name_Is_Empty_Update()
    {
        // Arrange
        var company = new UpdateCompanyDto("", "TEST", "NYSE", "US1234567890", null);

        // Act
        var result = _updateValidator.Validate(company);

        // Assert
        Assert.False(result.IsValid);
        Assert.Contains(result.Errors, e => e.PropertyName == nameof(UpdateCompanyDto.Name));
    }

    [Fact]
    public void Should_Have_Error_When_StockTicker_Is_Empty_Update()
    {
        // Arrange
        var company = new UpdateCompanyDto("Test Company", "", "NYSE", "US1234567890", null);

        // Act
        var result = _updateValidator.Validate(company);

        // Assert
        Assert.False(result.IsValid);
        Assert.Contains(result.Errors, e => e.PropertyName == nameof(UpdateCompanyDto.StockTicker));
    }

    [Fact]
    public void Should_Have_Error_When_Exchange_Is_Empty_Update()
    {
        // Arrange
        var company = new UpdateCompanyDto("Test Company", "TEST", "", "US1234567890", null);

        // Act
        var result = _updateValidator.Validate(company);

        // Assert
        Assert.False(result.IsValid);
        Assert.Contains(result.Errors, e => e.PropertyName == nameof(UpdateCompanyDto.Exchange));
    }

    [Fact]
    public void Should_Have_Error_When_Isin_Is_Invalid_Update()
    {
        // Arrange
        var company = new UpdateCompanyDto("Test Company", "TEST", "NYSE", "INVALID", null);

        // Act
        var result = _updateValidator.Validate(company);

        // Assert
        Assert.False(result.IsValid);
        Assert.Contains(result.Errors, e => e.PropertyName == nameof(UpdateCompanyDto.Isin));
    }

    [Fact]
    public void Should_Not_Have_Error_When_All_Fields_Are_Valid_Update()
    {
        // Arrange
        var company = new UpdateCompanyDto(
            "Test Company",
            "TEST",
            "NYSE",
            "US1234567890",
            "https://test.com"
        );

        // Act
        var result = _updateValidator.Validate(company);

        // Assert
        Assert.True(result.IsValid);
        Assert.Empty(result.Errors);
    }
} 