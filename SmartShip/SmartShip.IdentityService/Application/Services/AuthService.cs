
using SmartShip.IdentityService.Application.DTOs;
using SmartShip.IdentityService.Application.Interfaces;
using SmartShip.IdentityService.Domain.Entities;
using SmartShip.IdentityService.Infrastructure.Data;
// for jwt
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;

using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
namespace SmartShip.IdentityService.Application.Services;

public class AuthService : IAuthService
{
    private readonly IdentityDbContext _context;
    private readonly PasswordHasher<User> _passwordHasher;

    private readonly IConfiguration _configuration; // allow us to access appsettings 

    public AuthService(IdentityDbContext context, IConfiguration configuration)
    {
        _context = context;
        _passwordHasher = new PasswordHasher<User>();
        _configuration = configuration;
    }

    public async Task<RegisterResponseDto> RegisterAsync(RegisterRequestDto request)
    { // check if dupliate user exists

        var existingUser = await _context.Users.AnyAsync(u => u.Email == request.Email);

        if (existingUser)
        {
            return new RegisterResponseDto
            {
                Success = false,
                Message = "User already exists"
            };
        }


        var user = new User
        {
            FullName = request.FullName,
            Email = request.Email,
            Role = "Customer"
        };

        // first user will be created and then password will be hashed
        user.PasswordHash = _passwordHasher.HashPassword(user, request.Password);

        _context.Users.Add(user);
        await _context.SaveChangesAsync();

        return new RegisterResponseDto
        {
            Success = true,
            Message = "Registration Successful"
        };
    }

    public async Task<LoginResponseDto> LoginAsync(LoginRequestDto request)
    {
        var user = await _context.Users
            .FirstOrDefaultAsync(u => u.Email == request.Email);

        if (user == null)
        {
            return new LoginResponseDto
            {
                Success = false,
                Message = "Invalid email or password"
            };
        }

        var result = _passwordHasher.VerifyHashedPassword(
            user,
            user.PasswordHash,
            request.Password);

       

        if (result == PasswordVerificationResult.Failed)
        {
          

            return new LoginResponseDto
            {
                Success = false,
                Message = "Invalid email or password"
                
            };
        }

        var token = GenerateToken(user);
        return new LoginResponseDto
        {
            Success = true,
            Message = "Login successful",
            Token = token
        };
    }

    private string GenerateToken(User user)
    {
        var key = _configuration["Jwt:Key"];

        var securityKey = new SymmetricSecurityKey(
            Encoding.UTF8.GetBytes(key!));

        var credentials = new SigningCredentials(
            securityKey,
            SecurityAlgorithms.HmacSha256);

        var claims = new[]
        {
        new Claim(ClaimTypes.NameIdentifier, user.Id.ToString()),
        new Claim(ClaimTypes.Email, user.Email),
        new Claim(ClaimTypes.Role, user.Role)
    };

        var token = new JwtSecurityToken(
            issuer: _configuration["Jwt:Issuer"],
            audience: _configuration["Jwt:Audience"],
            claims: claims,
            expires: DateTime.UtcNow.AddMinutes(
                Convert.ToDouble(_configuration["Jwt:ExpiryMinutes"])),
            signingCredentials: credentials
        );

        return new JwtSecurityTokenHandler().WriteToken(token);
    }
}

