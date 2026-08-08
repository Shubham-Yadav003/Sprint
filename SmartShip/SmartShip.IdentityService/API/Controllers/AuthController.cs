using Microsoft.AspNetCore.Mvc;

namespace SmartShip.IdentityService.API.Controllers;

[ApiController]
[Route("api/[controller]")]
public class AuthController : ControllerBase
{
    [HttpGet("test")]
    public IActionResult Test()
    {
        return Ok("Identity Service is working");
    }
}