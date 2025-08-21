using CitasMedicas.Web.Models.Dto;

namespace CitasMedicas.Web.Services.IServices
{
    public interface IPatientService
    {
        Task<ResponseDto> GetPatientsAsync();
        Task<ResponseDto> GetPatientByIdAsync(int patientId);
        Task<ResponseDto> CreatePatientAsync(CreatePatientDto patientDto);
        Task<ResponseDto> UpdatePatientAsync(UpdatePatientDto patientDto);
    }
}
