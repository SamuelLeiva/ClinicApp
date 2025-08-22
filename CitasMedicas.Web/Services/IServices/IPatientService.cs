using CitasMedicas.Web.Models;
using CitasMedicas.Web.Models.Patient;

namespace CitasMedicas.Web.Services.IServices
{
    public interface IPatientService
    {
        Task<ResponseDto?> GetPatientsAsync();
        Task<ResponseDto?> GetPatientByIdAsync(int patientId);
        Task<ResponseDto?> CreatePatientAsync(CreatePatientDto patientDto);
        Task<ResponseDto?> UpdatePatientAsync(UpdatePatientDto patientDto);
    }
}
