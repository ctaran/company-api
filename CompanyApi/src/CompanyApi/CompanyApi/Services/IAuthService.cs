using CompanyApi.DTOs;
using CompanyApi.Models;

namespace CompanyApi.Services;

public interface IAuthService
{
    Task<AuthResponseDto> RegisterAsync(UserRegistrationDto registrationDto);
    Task<AuthResponseDto> LoginAsync(UserLoginDto loginDto);
    Task<UserResponseDto?> GetUserByIdAsync(int userId);
    string GenerateJwtToken(User user);
} 