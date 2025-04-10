using CompanyApi.DTOs;
using CompanyApi.Models;
using CompanyApi.Services;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Configuration;
using Moq;
using Xunit;

namespace CompanyApi.Tests.Services;

public class AuthServiceTests
{
    private readonly Mock<UserManager<User>> _mockUserManager;
    private readonly Mock<IConfiguration> _mockConfiguration;
    private readonly AuthService _authService;

    public AuthServiceTests()
    {
        // Setup UserManager mock
        var userStoreMock = new Mock<IUserStore<User>>();
        _mockUserManager = new Mock<UserManager<User>>(
            userStoreMock.Object,
            null, null, null, null, null, null, null, null
        );

        // Setup Configuration mock
        _mockConfiguration = new Mock<IConfiguration>();
        var configurationSection = new Mock<IConfigurationSection>();
        configurationSection.Setup(x => x.Value).Returns("your-256-bit-secret-key-here-for-testing-only");
        _mockConfiguration.Setup(x => x["Jwt:Key"]).Returns("your-256-bit-secret-key-here-for-testing-only");
        _mockConfiguration.Setup(x => x["Jwt:Issuer"]).Returns("test-issuer");
        _mockConfiguration.Setup(x => x["Jwt:Audience"]).Returns("test-audience");

        _authService = new AuthService(_mockUserManager.Object, _mockConfiguration.Object);
    }

    [Fact]
    public async Task RegisterAsync_WithValidData_ShouldCreateUserAndReturnToken()
    {
        // Arrange
        var registrationDto = new UserRegistrationDto
        {
            Username = "testuser",
            Email = "test@example.com",
            Password = "Password123!",
            ConfirmPassword = "Password123!"
        };

        var createdUser = new User
        {
            Id = 1,
            UserName = registrationDto.Username,
            Email = registrationDto.Email
        };

        _mockUserManager.Setup(x => x.CreateAsync(It.IsAny<User>(), registrationDto.Password))
            .ReturnsAsync(IdentityResult.Success);

        _mockUserManager.Setup(x => x.FindByNameAsync(registrationDto.Username))
            .ReturnsAsync((User?)null);

        // Act
        var result = await _authService.RegisterAsync(registrationDto);

        // Assert
        Assert.NotNull(result);
        Assert.NotNull(result.Token);
        Assert.NotNull(result.User);
        Assert.Equal(registrationDto.Username, result.User.Username);
        Assert.Equal(registrationDto.Email, result.User.Email);
    }

    [Fact]
    public async Task RegisterAsync_WithExistingUsername_ShouldThrowException()
    {
        // Arrange
        var registrationDto = new UserRegistrationDto
        {
            Username = "existinguser",
            Email = "test@example.com",
            Password = "Password123!",
            ConfirmPassword = "Password123!"
        };

        _mockUserManager.Setup(x => x.CreateAsync(It.IsAny<User>(), registrationDto.Password))
            .ReturnsAsync(IdentityResult.Failed(new IdentityError { Description = "User already exists" }));

        // Act & Assert
        await Assert.ThrowsAsync<InvalidOperationException>(() => 
            _authService.RegisterAsync(registrationDto));
    }

    [Fact]
    public async Task LoginAsync_WithValidCredentials_ShouldReturnToken()
    {
        // Arrange
        var loginDto = new UserLoginDto
        {
            Username = "testuser",
            Password = "Password123!"
        };

        var user = new User
        {
            Id = 1,
            UserName = loginDto.Username,
            Email = "test@example.com"
        };

        _mockUserManager.Setup(x => x.FindByNameAsync(loginDto.Username))
            .ReturnsAsync(user);

        _mockUserManager.Setup(x => x.CheckPasswordAsync(user, loginDto.Password))
            .ReturnsAsync(true);

        // Act
        var result = await _authService.LoginAsync(loginDto);

        // Assert
        Assert.NotNull(result);
        Assert.NotNull(result.Token);
        Assert.NotNull(result.User);
        Assert.Equal(loginDto.Username, result.User.Username);
    }

    [Fact]
    public async Task LoginAsync_WithInvalidCredentials_ShouldThrowException()
    {
        // Arrange
        var loginDto = new UserLoginDto
        {
            Username = "testuser",
            Password = "wrongpassword"
        };

        var user = new User
        {
            Id = 1,
            UserName = loginDto.Username,
            Email = "test@example.com"
        };

        _mockUserManager.Setup(x => x.FindByNameAsync(loginDto.Username))
            .ReturnsAsync(user);

        _mockUserManager.Setup(x => x.CheckPasswordAsync(user, loginDto.Password))
            .ReturnsAsync(false);

        // Act & Assert
        await Assert.ThrowsAsync<InvalidOperationException>(() => 
            _authService.LoginAsync(loginDto));
    }

    [Fact]
    public async Task GetUserByIdAsync_WithValidId_ShouldReturnUser()
    {
        // Arrange
        var userId = 1;
        var user = new User
        {
            Id = userId,
            UserName = "testuser",
            Email = "test@example.com"
        };

        _mockUserManager.Setup(x => x.FindByIdAsync(userId.ToString()))
            .ReturnsAsync(user);

        // Act
        var result = await _authService.GetUserByIdAsync(userId);

        // Assert
        Assert.NotNull(result);
        Assert.Equal(user.UserName, result.Username);
        Assert.Equal(user.Email, result.Email);
    }

    [Fact]
    public async Task GetUserByIdAsync_WithInvalidId_ShouldReturnNull()
    {
        // Arrange
        var userId = 999;
        _mockUserManager.Setup(x => x.FindByIdAsync(userId.ToString()))
            .ReturnsAsync((User?)null);

        // Act
        var result = await _authService.GetUserByIdAsync(userId);

        // Assert
        Assert.Null(result);
    }
} 