using CitasMedicas.Web.Models.D;
using CitasMedicas.Web.Models.Dto;

namespace CitasMedicas.Web.Services.IServices
{
    public interface IAuthService
    {
        Task<ResponseDto?> RegisterAsync(RegistrationRequestDto registrationRequestDto);
        Task<ResponseDto?> LoginAsync(LoginRequestDto loginRequestDto);
        Task<ResponseDto?> AssignRoleAsync(RegistrationRequestWithRole registrationRequestDtoRole);
    }
}
