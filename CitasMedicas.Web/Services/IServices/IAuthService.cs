using CitasMedicas.Web.Models;
using CitasMedicas.Web.Models.Auth;

namespace CitasMedicas.Web.Services.IServices
{
    public interface IAuthService
    {
        Task<ResponseDto?> RegisterAsync(RegistrationRequestWithRoleDto registrationRequestWithRole);
        Task<ResponseDto?> LoginAsync(LoginRequestDto loginRequestDto);
        Task<ResponseDto?> AssignRoleAsync(RegistrationRequestWithRoleDto registrationRequestWithRole);
    }
}
