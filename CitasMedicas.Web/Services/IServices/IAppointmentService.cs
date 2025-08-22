using CitasMedicas.Web.Models.Dto;

namespace CitasMedicas.Web.Services.IServices
{
    public interface IAppointmentService
    {
        Task<ResponseDto> GetAppointmentsAsync();
        Task<ResponseDto> GetAppointmentByIdAsync(int appointmentId);
        Task<ResponseDto> GetAppointmentFilter(int patientId, int doctorId);
        Task<ResponseDto> CreateAppointmentAsync(CreateAppointmentDto appointmentDto);
    }
}
