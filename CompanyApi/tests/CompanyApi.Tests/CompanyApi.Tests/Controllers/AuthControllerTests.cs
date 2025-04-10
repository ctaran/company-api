using System.Security.Claims;
using CompanyApi.Controllers;
using CompanyApi.DTOs;
using CompanyApi.Services;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Moq;
using Xunit;

namespace CompanyApi.Tests.Controllers;

public class AuthControllerTests
{
    private readonly Mock<IAuthService> _mockAuthService;
    private readonly AuthController _controller;

    public AuthControllerTests()
    {
        _mockAuthService = new Mock<IAuthService>();
        _controller = new AuthController(_mockAuthService.Object);
    }

    [Fact]
    public async Task Register_WithValidData_ShouldReturnOkResult()
    {
        // Arrange
        var registrationDto = new UserRegistrationDto
        {
            Username = "testuser",
            Email = "test@example.com",
            Password = "Password123!",
            ConfirmPassword = "Password123!"
        };

        var expectedResponse = new AuthResponseDto
        {
            Token = "test-token",
            User = new UserResponseDto
            {
                Id = 1,
                Username = registrationDto.Username,
                Email = registrationDto.Email
            }
        };

        _mockAuthService.Setup(x => x.RegisterAsync(registrationDto))
            .ReturnsAsync(expectedResponse);

        // Act
        var result = await _controller.Register(registrationDto);

        // Assert
        var okResult = Assert.IsType<OkObjectResult>(result.Result);
        var response = Assert.IsType<AuthResponseDto>(okResult.Value);
        Assert.Equal(expectedResponse.Token, response.Token);
        Assert.Equal(expectedResponse.User.Username, response.User.Username);
    }

    [Fact]
    public async Task Register_WhenServiceThrowsException_ShouldReturnBadRequest()
    {
        // Arrange
        var registrationDto = new UserRegistrationDto
        {
            Username = "testuser",
            Email = "test@example.com",
            Password = "Password123!",
            ConfirmPassword = "Password123!"
        };

        _mockAuthService.Setup(x => x.RegisterAsync(registrationDto))
            .ThrowsAsync(new InvalidOperationException("User already exists"));

        // Act
        var result = await _controller.Register(registrationDto);

        // Assert
        var badRequestResult = Assert.IsType<BadRequestObjectResult>(result.Result);
        Assert.Contains("User already exists", badRequestResult.Value?.ToString());
    }

    [Fact]
    public async Task Login_WithValidCredentials_ShouldReturnOkResult()
    {
        // Arrange
        var loginDto = new UserLoginDto
        {
            Username = "testuser",
            Password = "Password123!"
        };

        var expectedResponse = new AuthResponseDto
        {
            Token = "test-token",
            User = new UserResponseDto
            {
                Id = 1,
                Username = loginDto.Username,
                Email = "test@example.com"
            }
        };

        _mockAuthService.Setup(x => x.LoginAsync(loginDto))
            .ReturnsAsync(expectedResponse);

        // Act
        var result = await _controller.Login(loginDto);

        // Assert
        var okResult = Assert.IsType<OkObjectResult>(result.Result);
        var response = Assert.IsType<AuthResponseDto>(okResult.Value);
        Assert.Equal(expectedResponse.Token, response.Token);
        Assert.Equal(expectedResponse.User.Username, response.User.Username);
    }

    [Fact]
    public async Task Login_WithInvalidCredentials_ShouldReturnBadRequest()
    {
        // Arrange
        var loginDto = new UserLoginDto
        {
            Username = "testuser",
            Password = "wrongpassword"
        };

        _mockAuthService.Setup(x => x.LoginAsync(loginDto))
            .ThrowsAsync(new InvalidOperationException("Invalid username or password"));

        // Act
        var result = await _controller.Login(loginDto);

        // Assert
        var badRequestResult = Assert.IsType<BadRequestObjectResult>(result.Result);
        Assert.Contains("Invalid username or password", badRequestResult.Value?.ToString());
    }

    [Fact]
    public async Task GetCurrentUser_WithValidUser_ShouldReturnOkResult()
    {
        // Arrange
        var userId = 1;
        var expectedUser = new UserResponseDto
        {
            Id = userId,
            Username = "testuser",
            Email = "test@example.com"
        };

        var claims = new List<Claim>
        {
            new Claim("sub", userId.ToString())
        };
        var identity = new ClaimsIdentity(claims);
        var claimsPrincipal = new ClaimsPrincipal(identity);

        _controller.ControllerContext = new ControllerContext
        {
            HttpContext = new DefaultHttpContext
            {
                User = claimsPrincipal
            }
        };

        _mockAuthService.Setup(x => x.GetUserByIdAsync(userId))
            .ReturnsAsync(expectedUser);

        // Act
        var result = await _controller.GetCurrentUser();

        // Assert
        var okResult = Assert.IsType<OkObjectResult>(result.Result);
        var user = Assert.IsType<UserResponseDto>(okResult.Value);
        Assert.Equal(expectedUser.Username, user.Username);
        Assert.Equal(expectedUser.Email, user.Email);
    }

    [Fact]
    public async Task GetCurrentUser_WithInvalidUser_ShouldReturnNotFound()
    {
        // Arrange
        var userId = 999;
        var claims = new List<Claim>
        {
            new Claim("sub", userId.ToString())
        };
        var identity = new ClaimsIdentity(claims);
        var claimsPrincipal = new ClaimsPrincipal(identity);

        _controller.ControllerContext = new ControllerContext
        {
            HttpContext = new DefaultHttpContext
            {
                User = claimsPrincipal
            }
        };

        _mockAuthService.Setup(x => x.GetUserByIdAsync(userId))
            .ReturnsAsync((UserResponseDto?)null);

        // Act
        var result = await _controller.GetCurrentUser();

        // Assert
        Assert.IsType<NotFoundResult>(result.Result);
    }
} 