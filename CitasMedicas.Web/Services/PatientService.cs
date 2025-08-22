using CitasMedicas.Web.Models.Dto;
using CitasMedicas.Web.Services.IServices;
using CitasMedicas.Web.Utility;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Text;
using System.Text.Json;

namespace CitasMedicas.Web.Services
{
    public class PatientService : IPatientService
    {
        private readonly IBaseService _baseService;

        public PatientService(IBaseService baseService)
        {
            _baseService = baseService;
        }

        public async Task<ResponseDto> CreatePatientAsync(CreatePatientDto patientDto)
        {
            return await _baseService.SendAsync(new RequestDto()
            {
                ApiType = SD.ApiType.POST,
                Data = patientDto,
                Url = SD.PatientAPIBase + "/api/PatientAPI"
            });
        }

        public async Task<ResponseDto> GetAllPatientsAsync()
        {
            return await _baseService.SendAsync(new RequestDto()
            {
                ApiType = SD.ApiType.GET,
                Url = SD.PatientAPIBase + "/api/PatientAPI"
            });
        }

        public async Task<ResponseDto> GetPatientByIdAsync(int patientId)
        {
            return await _baseService.SendAsync(new RequestDto()
            {
                ApiType = SD.ApiType.GET,
                Url = SD.PatientAPIBase + "/api/PatientAPI/" + patientId
            });
        }

        public async Task<ResponseDto> UpdatePatientAsync(UpdatePatientDto patientDto)
        {
            return await _baseService.SendAsync(new RequestDto()
            {
                ApiType = SD.ApiType.PUT,
                Data = patientDto,
                Url = SD.PatientAPIBase + "/api/PatientAPI"
            });
        }   

    }
}
