using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.Extensions.Configuration;
using System.Text;
using System.Threading.Tasks;
using UniversalTechnicalTest.Api.DTOs.Auth;
using UniversalTechnicalTest.Api.Exceptions;
using UniversalTechnicalTest.Api.Services;
using UniversalTechnicalTest.Api.Services.Interfaces;
using UniversalTechnicalTest.Api.Data;

namespace UniversalTechnicalTest.Tests
{
    public class AuthServiceTests
    {
        private AppDbContext CreateDbContext()
        {
            var options = new DbContextOptionsBuilder<AppDbContext>()
                .UseInMemoryDatabase(Guid.NewGuid().ToString())
                .Options;

            return new AppDbContext(options);
        }

        private IJwtService CreateJwtService()
        {
            var settings = new Dictionary<string, string?>
            {
                ["Jwt:Key"] = "UniversalTechnicalTestSuperSecretKey2026",
                ["Jwt:Issuer"] = "UniversalTechnicalTest.Api",
                ["Jwt:Audience"] = "UniversalTechnicalTest.Client",
                ["Jwt:ExpirationMinutes"] = "60"

            };

            var configuration = new ConfigurationBuilder()
                .AddInMemoryCollection(settings)
                .Build();

            return new JwtService(configuration);
            
        }

        [Fact]
        public async Task RegisterAsync_WithValidData_ShouldRegisterUser()
        {
            // Arrange
            var context = CreateDbContext();
            var jwtService = CreateJwtService();
            var authService = new AuthService(context, jwtService);

            var request = new RegisterRequest
            {
                Name = "Ramon Perez",
                Email = "ramonperez1@gmail.com",
                Password = "Password@123"
            };

            // Act
            var result = await authService.RegisterAsync(request);

            // Assert
            Assert.NotNull(result);
            Assert.Equal("Ramon Perez", result.Name);
            Assert.Equal("ramonperez1@gmail.com", result.Email);
            Assert.NotEqual(Guid.Empty, result.Id);
            Assert.False(string.IsNullOrWhiteSpace(result.Token));

            var user = await context.Users.FirstOrDefaultAsync();

            Assert.NotNull(user);
            Assert.NotEqual("Password@123", user.PasswordHash);
        }

        [Fact]
        public async Task RegisterAsync_WithDuplicateEmail_ShouldThrowBadRequestException()
        {
            // Arrange
            var context = CreateDbContext();
            var jwtService = CreateJwtService();
            var authService = new AuthService(context, jwtService);

            var request = new RegisterRequest
            {
                Name = "Ramon Perez",
                Email = "ramonperez1@gmail.com",
                Password = "Password@123"
            };

            await authService.RegisterAsync(request);

            // Act
            var exception = await Assert.ThrowsAsync<BadRequestException>(
                () => authService.RegisterAsync(request));

            // Assert
            Assert.Equal(
                "El correo ya se encuentra registrado.",
                exception.Message);
        }

        [Fact]
        public async Task LoginAsync_WithValidCredentials_ShouldReturnToken()
        {
            // Arrange
            var context = CreateDbContext();
            var jwtService = CreateJwtService();
            var authService = new AuthService(context, jwtService);

            var registerRequest = new RegisterRequest
            {
                Name = "Ramon Perez",
                Email = "ramonperez@gmail.com",
                Password = "Password@123"
            };

            await authService.RegisterAsync(registerRequest);

            var loginRequest = new LoginRequest
            {
                Email = "ramonperez@gmail.com",
                Password = "Password@123"
            };

            // Act
            var result = await authService.LoginAsync(loginRequest);

            // Assert
            Assert.NotNull(result);
            Assert.False(string.IsNullOrWhiteSpace(result.Token));
        }

        [Fact]
        public async Task LoginAsync_WithInvalidPassword_ShouldThrowBadRequestException()
        {
            // Arrange
            var context = CreateDbContext();
            var jwtService = CreateJwtService();
            var authService = new AuthService(context, jwtService);

            await authService.RegisterAsync(new RegisterRequest
            {
                Name = "Ramon Perez",
                Email = "ramonperez@gmail.com",
                Password = "Password@123"
            });

            var loginRequest = new LoginRequest
            {
                Email = "ramonperez@gmail.com",
                Password = "WrongPassword@123"
            };

            // Act
            var exception = await Assert.ThrowsAsync<BadRequestException>(
                () => authService.LoginAsync(loginRequest));

            // Assert
            Assert.Equal(
                "Correo o contraseña incorrectos.",
                exception.Message);
        }
    }
}
