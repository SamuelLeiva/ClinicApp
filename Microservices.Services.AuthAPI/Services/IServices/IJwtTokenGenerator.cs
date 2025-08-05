using Microservices.Services.AuthAPI.Models;

namespace Microservices.Services.AuthAPI.Services.IServices
{
    public interface IJwtTokenGenerator
    {
        string GenerateToken(ApplicationUser applicationUser, IEnumerable<string> roles);
    }
}
