using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using CompanyApi.DTOs;
using CompanyApi.Services;

namespace CompanyApi.Controllers;

[ApiController]
[Route("api/[controller]")]
public class AuthController : ControllerBase
{
    private readonly IAuthService _authService;

    public AuthController(IAuthService authService)
    {
        _authService = authService;
    }

    [HttpPost("register")]
    public async Task<ActionResult<AuthResponseDto>> Register(UserRegistrationDto registrationDto)
    {
        try
        {
            var response = await _authService.RegisterAsync(registrationDto);
            return Ok(response);
        }
        catch (InvalidOperationException ex)
        {
            return BadRequest(new { message = ex.Message });
        }
    }

    [HttpPost("login")]
    public async Task<ActionResult<AuthResponseDto>> Login(UserLoginDto loginDto)
    {
        try
        {
            var response = await _authService.LoginAsync(loginDto);
            return Ok(response);
        }
        catch (InvalidOperationException ex)
        {
            return BadRequest(new { message = ex.Message });
        }
    }

    [Authorize]
    [HttpGet("me")]
    public async Task<ActionResult<UserResponseDto>> GetCurrentUser()
    {
        var userId = int.Parse(User.FindFirst("sub")?.Value ?? "0");
        var user = await _authService.GetUserByIdAsync(userId);
        
        if (user == null)
        {
            return NotFound();
        }

        return Ok(user);
    }
} 