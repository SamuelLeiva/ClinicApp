using CitasMedicas.Web.Models;
using CitasMedicas.Web.Models.Doctor;
using CitasMedicas.Web.Services.IServices;
using CitasMedicas.Web.Utility;

namespace CitasMedicas.Web.Services
{
    public class DoctorService : IDoctorService
    {
        private readonly IBaseService _baseService;
        public DoctorService(IBaseService baseService)
        {
            _baseService = baseService;
        }

        public async Task<ResponseDto> CreateDoctorAsync(CreateDoctorDto doctorDto)
        {
            return await _baseService.SendAsync(new RequestDto()
            {
                ApiType = SD.ApiType.POST,
                Data = doctorDto,
                Url = SD.DoctorAPIBase + "/api/DoctorAPI"
            });
        }

        public async Task<ResponseDto> GetDoctorByIdAsync(int doctorId)
        {
            return await _baseService.SendAsync(new RequestDto()
            {
                ApiType = SD.ApiType.GET,
                Url = SD.DoctorAPIBase + "/api/DoctorAPI/" + doctorId
            });
        }

        public Task<ResponseDto> GetDoctorsAsync()
        {
            return _baseService.SendAsync(new RequestDto()
            {
                ApiType = SD.ApiType.GET,
                Url = SD.DoctorAPIBase + "/api/DoctorAPI"
            });
        }

        public Task<ResponseDto> UpdateDoctorAsync(UpdateDoctorDto doctorDto)
        {
            return _baseService.SendAsync(new RequestDto()
            {
                ApiType = SD.ApiType.PUT,
                Data = doctorDto,
                Url = SD.DoctorAPIBase + $"/api/DoctorAPI/{doctorDto.DoctorId}"
            });
        }
    }
}
