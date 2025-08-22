using CitasMedicas.Web.Models;
using CitasMedicas.Web.Models.Specialty;

namespace CitasMedicas.Web.Services.IServices
{
    public interface ISpecialtyService
    {
        Task<ResponseDto> GetSpecialtiesAsync();
        Task<ResponseDto> GetSpecialtyByIdAsync(int specialtyId);
        Task<ResponseDto> CreateSpecialtyAsync(CreateSpecialtyDto specialtyDto);
        Task<ResponseDto> UpdateSpecialtyAsync(UpdateSpecialtyDto specialtyDto);
    }
}
