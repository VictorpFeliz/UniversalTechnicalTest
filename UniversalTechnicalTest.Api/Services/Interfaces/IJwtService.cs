using UniversalTechnicalTest.Api.Models;

namespace UniversalTechnicalTest.Api.Services.Interfaces
{
    public interface IJwtService
    {
        string GenerateToken(User user);
    }
}
