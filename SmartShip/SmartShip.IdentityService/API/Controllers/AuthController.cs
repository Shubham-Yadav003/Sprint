using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using SmartShip.IdentityService.Application.DTOs;
using SmartShip.IdentityService.Application.Interfaces;

namespace SmartShip.IdentityService.API.Controllers;

[ApiController]
[Route("api/[controller]")]
public class AuthController : ControllerBase
{
    private readonly IAuthService _authService;

    public AuthController(IAuthService authService)
    {
        _authService = authService;
    }


    [HttpGet("test")]
    public IActionResult Test()
    {
        return Ok("Identity Service is working");
    }


    [HttpGet("profile")]
    [Authorize]
    public IActionResult Profile()
    {
        return Ok("You are authenticated");
    }


    [HttpPost("register")]
    public async  Task<IActionResult> Register(RegisterRequestDto request)
    {
        var result = await _authService.RegisterAsync(request);

        if (!result.Success)
        {
            return Conflict(result); //HTTP 409 Conflict {resource issue}
        }
        return Ok(result);
    }

    [HttpPost("login")]
    public async Task<IActionResult> Login(LoginRequestDto request)
    {
        var result = await _authService.LoginAsync(request);

        if (!result.Success)
        {
            return Unauthorized(result); //HTTP 401 Unauthorized {invalid credentials}
        }

        return Ok(result);
    }
}