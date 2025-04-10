using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using CompanyApi.DTOs;
using CompanyApi.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.IdentityModel.Tokens;

namespace CompanyApi.Services;

public class AuthService : IAuthService
{
    private readonly UserManager<User> _userManager;
    private readonly IConfiguration _configuration;

    public AuthService(UserManager<User> userManager, IConfiguration configuration)
    {
        _userManager = userManager;
        _configuration = configuration;
    }

    public async Task<AuthResponseDto> RegisterAsync(UserRegistrationDto registrationDto)
    {
        var user = new User
        {
            UserName = registrationDto.Username,
            Email = registrationDto.Email
        };

        var result = await _userManager.CreateAsync(user, registrationDto.Password);
        if (!result.Succeeded)
        {
            throw new InvalidOperationException("Failed to register user");
        }

        var token = GenerateJwtToken(user);
        return new AuthResponseDto
        {
            Token = token,
            User = new UserResponseDto
            {
                Id = user.Id,
                Username = user.UserName!,
                Email = user.Email!
            }
        };
    }

    public async Task<AuthResponseDto> LoginAsync(UserLoginDto loginDto)
    {
        var user = await _userManager.FindByNameAsync(loginDto.Username);
        if (user == null)
        {
            throw new InvalidOperationException("Invalid username or password");
        }

        var isPasswordValid = await _userManager.CheckPasswordAsync(user, loginDto.Password);
        if (!isPasswordValid)
        {
            throw new InvalidOperationException("Invalid username or password");
        }

        var token = GenerateJwtToken(user);
        return new AuthResponseDto
        {
            Token = token,
            User = new UserResponseDto
            {
                Id = user.Id,
                Username = user.UserName!,
                Email = user.Email!
            }
        };
    }

    public async Task<UserResponseDto?> GetUserByIdAsync(int userId)
    {
        var user = await _userManager.FindByIdAsync(userId.ToString());
        if (user == null)
        {
            return null;
        }

        return new UserResponseDto
        {
            Id = user.Id,
            Username = user.UserName!,
            Email = user.Email!
        };
    }

    public string GenerateJwtToken(User user)
    {
        var claims = new List<Claim>
        {
            new Claim(ClaimTypes.NameIdentifier, user.Id.ToString()),
            new Claim(ClaimTypes.Name, user.UserName!),
            new Claim(ClaimTypes.Email, user.Email!)
        };

        var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_configuration["Jwt:Key"]!));
        var creds = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);
        var expires = DateTime.Now.AddDays(1);

        var token = new JwtSecurityToken(
            _configuration["Jwt:Issuer"],
            _configuration["Jwt:Audience"],
            claims,
            expires: expires,
            signingCredentials: creds
        );

        return new JwtSecurityTokenHandler().WriteToken(token);
    }
} 