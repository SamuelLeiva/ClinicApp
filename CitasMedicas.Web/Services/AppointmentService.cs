using CitasMedicas.Web.Models.Dto;
using CitasMedicas.Web.Services.IServices;
using CitasMedicas.Web.Utility;

namespace CitasMedicas.Web.Services
{
    public class AppointmentService : IAppointmentService
    {
        private readonly IBaseService _baseService;
        public AppointmentService(IBaseService baseService)
        {
            _baseService = baseService;
        }
        public Task<ResponseDto> CreateAppointmentAsync(CreateAppointmentDto appointmentDto)
        {
            return _baseService.SendAsync(new RequestDto()
            {
                ApiType = SD.ApiType.POST,
                Url = SD.AppointmentAPIBase + "/api/AppointmentAPI",
                Data = appointmentDto
            });
        }

        public Task<ResponseDto> GetAppointmentByIdAsync(int appointmentId)
        {
            return _baseService.SendAsync(new RequestDto()
            {
                ApiType = SD.ApiType.GET,
                Url = SD.AppointmentAPIBase + "/api/AppointmentAPI/" + appointmentId
            });
        }

        public Task<ResponseDto> GetAppointmentFilter(int patientId, int doctorId)
        {
            return _baseService.SendAsync(new RequestDto()
            {
                ApiType = SD.ApiType.GET,
                Url = SD.AppointmentAPIBase + $"/api/AppointmentAPI/filter?patientId={patientId}&doctorId={doctorId}"
            });
        }

        public Task<ResponseDto> GetAppointmentsAsync()
        {
            return _baseService.SendAsync(new RequestDto()
            {
                ApiType = SD.ApiType.GET,
                Url = SD.AppointmentAPIBase + "/api/AppointmentAPI"
            });
        }
    }
}
