using UniversalTechnicalTest.Api.DTOs.Auth;

namespace UniversalTechnicalTest.Api.Services.Interfaces
{
    public interface IAuthService
    {
        Task<RegisterResponse> RegisterAsync(RegisterRequest request);

        Task<LoginResponse> LoginAsync(LoginRequest request);
    }
}
