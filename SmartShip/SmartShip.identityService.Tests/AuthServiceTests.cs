using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using SmartShip.IdentityService.Application.DTOs;
using SmartShip.IdentityService.Application.Services;
using SmartShip.IdentityService.Infrastructure.Data;

namespace SmartShip.IdentityService.Tests
{
    public class AuthServiceTests
    {
        [Fact]
        public async Task RegisterAsync_ShouldRegisterNewUser()
        {
            var options = new DbContextOptionsBuilder<IdentityDbContext>()
                .UseInMemoryDatabase("TestDatabase")
                .Options;

            using var context = new IdentityDbContext(options);

            var configuration = new ConfigurationBuilder()
                .AddInMemoryCollection(new Dictionary<string, string?>
                {
                    ["Jwt:Key"] = "SmartShipSecretKeyForDevelopmentOnly12345",
                    ["Jwt:Issuer"] = "SmartShip",
                    ["Jwt:Audience"] = "SmartShipUsers",
                    ["Jwt:ExpiryMinutes"] = "60"
                })
                .Build();

            var service = new AuthService(
                context,
                configuration);

            var request = new RegisterRequestDto
            {
                FullName = "Test User",
                Email = "test@test.com",
                Password = "Test123"
            };

            var result = await service.RegisterAsync(request);

            Assert.True(result.Success);
        }


        [Fact]
        public async Task RegisterAsync_ShouldRejectDuplicateEmail()
        {
            var options = new DbContextOptionsBuilder<IdentityDbContext>()
                .UseInMemoryDatabase("DuplicateEmailTest")
                .Options;

            using var context = new IdentityDbContext(options);

            var configuration = new ConfigurationBuilder()
                .AddInMemoryCollection(new Dictionary<string, string?>
                {
                    ["Jwt:Key"] = "SmartShipSecretKeyForDevelopmentOnly12345",
                    ["Jwt:Issuer"] = "SmartShip",
                    ["Jwt:Audience"] = "SmartShipUsers",
                    ["Jwt:ExpiryMinutes"] = "60"
                })
                .Build();

            var service = new AuthService(
                context,
                configuration);

            var firstRequest = new RegisterRequestDto
            {
                FullName = "Test User",
                Email = "test@test.com",
                Password = "Test123"
            };

            await service.RegisterAsync(firstRequest);

            var secondRequest = new RegisterRequestDto
            {
                FullName = "Another User",
                Email = "test@test.com",
                Password = "Test456"
            };

            var result = await service.RegisterAsync(secondRequest);

            Assert.False(result.Success);
        }


        [Fact]
        public async Task LoginAsync_ShouldLoginWithCorrectPassword()
        {
            var options = new DbContextOptionsBuilder<IdentityDbContext>()
                .UseInMemoryDatabase("LoginSuccessTest")
                .Options;

            using var context = new IdentityDbContext(options);

            var configuration = new ConfigurationBuilder()
                .AddInMemoryCollection(new Dictionary<string, string?>
                {
                    ["Jwt:Key"] = "SmartShipSecretKeyForDevelopmentOnly12345",
                    ["Jwt:Issuer"] = "SmartShip",
                    ["Jwt:Audience"] = "SmartShipUsers",
                    ["Jwt:ExpiryMinutes"] = "60"
                })
                .Build();

            var service = new AuthService(
                context,
                configuration);

            var registerRequest = new RegisterRequestDto
            {
                FullName = "Test User",
                Email = "login@test.com",
                Password = "Test123"
            };

            await service.RegisterAsync(registerRequest);

            var loginRequest = new LoginRequestDto
            {
                Email = "login@test.com",
                Password = "Test123"
            };

            var result = await service.LoginAsync(loginRequest);

            Assert.True(result.Success);
            Assert.NotEmpty(result.Token);
        }



        [Fact]
        public async Task LoginAsync_ShouldRejectWrongPassword()
        {
            var options = new DbContextOptionsBuilder<IdentityDbContext>()
                .UseInMemoryDatabase("LoginFailureTest")
                .Options;

            using var context = new IdentityDbContext(options);

            var configuration = new ConfigurationBuilder()
                .AddInMemoryCollection(new Dictionary<string, string?>
                {
                    ["Jwt:Key"] = "SmartShipSecretKeyForDevelopmentOnly12345",
                    ["Jwt:Issuer"] = "SmartShip",
                    ["Jwt:Audience"] = "SmartShipUsers",
                    ["Jwt:ExpiryMinutes"] = "60"
                })
                .Build();

            var service = new AuthService(
                context,
                configuration);

            var registerRequest = new RegisterRequestDto
            {
                FullName = "Test User",
                Email = "wrong@test.com",
                Password = "Test123"
            };

            await service.RegisterAsync(registerRequest);

            var loginRequest = new LoginRequestDto
            {
                Email = "wrong@test.com",
                Password = "WrongPassword"
            };

            var result = await service.LoginAsync(loginRequest);

            Assert.False(result.Success); // we expect result to be wrong password {false}
            Assert.Empty(result.Token); // We expect result.Token to be empty.
            // Because login failed, we should not generate a JWT.
        }

    }

   
}