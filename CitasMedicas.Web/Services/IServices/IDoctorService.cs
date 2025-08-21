using CitasMedicas.Web.Models.Dto;

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
