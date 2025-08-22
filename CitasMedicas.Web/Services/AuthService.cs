using CitasMedicas.Web.Models.Dto;
using CitasMedicas.Web.Services.IServices;
using CitasMedicas.Web.Utility;

namespace CitasMedicas.Web.Services
{
    public class AuthService : IAuthService
    {
        private readonly IBaseService _baseService;
        public AuthService(IBaseService baseService)
        {
            _baseService = baseService;
        }

        public async Task<ResponseDto?> AssignRoleAsync(RegistrationRequestWithRoleDto registrationRequestWithRoleDto)
        {
            return await _baseService.SendAsync(new RequestDto
            {
                ApiType = SD.ApiType.POST,
                Data = registrationRequestWithRoleDto,
                Url = SD.AuthAPIBase + "/api/AuthAPI/AssignRole"
            });
        }

        public async Task<ResponseDto?> LoginAsync(LoginRequestDto loginRequestDto)
        {
            return await _baseService.SendAsync(new RequestDto()
            {
                ApiType = SD.ApiType.POST,
                Data = loginRequestDto,
                Url = SD.AuthAPIBase + "/api/AuthAPI/Login"
            }, withBearer: false);
        }

        public async Task<ResponseDto?> RegisterAsync(RegistrationRequestWithRoleDto registrationRequestWithRoleDto)
        {
            return await _baseService.SendAsync(new RequestDto()
            {
                ApiType = SD.ApiType.POST,
                Data = registrationRequestWithRoleDto,
                Url = SD.AuthAPIBase + "/api/AuthAPI/Register"
            }, withBearer: false);
        }

    }
}
