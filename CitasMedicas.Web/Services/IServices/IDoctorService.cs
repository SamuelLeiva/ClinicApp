using CitasMedicas.Web.Models;
using CitasMedicas.Web.Models.Doctor;

namespace CitasMedicas.Web.Services.IServices
{
    public interface IDoctorService
    {
        Task<ResponseDto> GetDoctorsAsync();
        Task<ResponseDto> GetDoctorByIdAsync(int doctorId);
        Task<ResponseDto> CreateDoctorAsync(CreateDoctorDto doctorDto);
        Task<ResponseDto> UpdateDoctorAsync(UpdateDoctorDto doctorDto);
    }
}
