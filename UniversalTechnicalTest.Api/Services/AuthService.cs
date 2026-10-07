using Microsoft.EntityFrameworkCore;
using UniversalTechnicalTest.Api.Data;
using UniversalTechnicalTest.Api.DTOs.Auth;
using UniversalTechnicalTest.Api.Exceptions;
using UniversalTechnicalTest.Api.Models;
using UniversalTechnicalTest.Api.Services.Interfaces;

namespace UniversalTechnicalTest.Api.Services
{
    public class AuthService : IAuthService
    {
        private readonly AppDbContext _context;
        private readonly IJwtService _jwtService;

        public AuthService(AppDbContext context, IJwtService jwtService)
        {
            _context = context;
            _jwtService = jwtService;
        }

        public async Task<LoginResponse> LoginAsync(LoginRequest request)
        {
            var normalizedEmail = request.Email.Trim().ToLowerInvariant();

            var user = await _context.Users.FirstOrDefaultAsync(u => u.Email == normalizedEmail);

            if (user is null)
                throw new BadRequestException("Correo o contraseña incorrectos.");

            var passwordIsValid = BCrypt.Net.BCrypt.Verify(request.Password, user.PasswordHash);

            if (!passwordIsValid)
                throw new BadRequestException("Correo o contraseña incorrectos.");

            var token = _jwtService.GenerateToken(user);

            var userToken = new UserToken
            {
                Id = Guid.NewGuid(),
                UserId = user.Id,
                Token = token,
                ExpiresAt = DateTime.UtcNow.AddMinutes(60)
            };

            await _context.UserTokens.AddAsync(userToken);
            await _context.SaveChangesAsync();

            return new LoginResponse
            {
                Token = token
            };

        }

        public async Task<RegisterResponse> RegisterAsync(RegisterRequest request)
        {
            var normalizedEmail = request.Email.Trim().ToLowerInvariant();

            var emailExists = await _context.Users.AnyAsync(u => u.Email == normalizedEmail);

            if (emailExists)
            {
                throw new BadRequestException(
                    "El correo ya se encuentra registrado.");
            }

            var user = new User
            {
                Id = Guid.NewGuid(),
                Name = request.Name.Trim(),
                Email = normalizedEmail,
                PasswordHash = BCrypt.Net.BCrypt.HashPassword(request.Password)
            };

            await _context.Users.AddAsync(user);
            await _context.SaveChangesAsync();

            var token = _jwtService.GenerateToken(user);

            var userToken = new UserToken
            {
                Id = Guid.NewGuid(),
                UserId = user.Id,
                Token = token,
                ExpiresAt = DateTime.UtcNow.AddMinutes(60)
            };

            await _context.UserTokens.AddAsync(userToken);
            await _context.SaveChangesAsync();

            return new RegisterResponse
            {
                Id = user.Id,
                Name = user.Name,
                Email = user.Email,
                Token = token
            };
        }
    }
}
