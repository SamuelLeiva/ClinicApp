using AutoMapper;
using Azure;
using Microservices.Services.AppointmentAPI.Data;
using Microservices.Services.AppointmentAPI.Models;
using Microservices.Services.AppointmentAPI.Models.Dto;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Net.Http;

namespace Microservices.Services.AppointmentAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AppointmentAPIController : ControllerBase
    {

        private readonly ApplicationDbContext _dbContext;
        private readonly IMapper _mapper;
        private readonly IHttpClientFactory _httpClientFactory;
        private ResponseDto _response;

        public AppointmentAPIController(ApplicationDbContext dbContext, IMapper mapper, IHttpClientFactory httpClientFactory)
        {
            _dbContext = dbContext;
            _mapper = mapper;
            _httpClientFactory = httpClientFactory;
            _response = new ResponseDto();
        }

        [HttpPost]
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> Post([FromBody] CreateAppointmentDto appointmentDto)
        {
            try
            {
                // 1. Validar existencia del Paciente
                var patientClient = _httpClientFactory.CreateClient("PatientAPI");
                var patientResponse = await patientClient.GetAsync($"api/patientapi/{appointmentDto.PatientId}");
                if (!patientResponse.IsSuccessStatusCode)
                {
                    _response.IsSuccess = false;
                    _response.Message = "El paciente no existe.";
                    return BadRequest(_response);
                }

                // 2. Validar existencia del Doctor
                var doctorClient = _httpClientFactory.CreateClient("DoctorAPI");
                var doctorResponse = await doctorClient.GetAsync($"api/doctorapi/{appointmentDto.DoctorId}");
                if (!doctorResponse.IsSuccessStatusCode)
                {
                    _response.IsSuccess = false;
                    _response.Message = "El médico no existe.";
                    return BadRequest(_response);
                }

                // 3. Crear y guardar la cita
                var newAppointment = _mapper.Map<Appointment>(appointmentDto);
                await _dbContext.Appointments.AddAsync(newAppointment);
                await _dbContext.SaveChangesAsync();

                _response.Result = _mapper.Map<AppointmentDto>(newAppointment);
                _response.Message = "Cita creada exitosamente.";

                return CreatedAtRoute("Get", new { id = newAppointment.AppointmentId }, _response);
            }
            catch (Exception ex)
            {
                _response.IsSuccess = false;
                _response.Message = $"Error al crear la cita: {ex.Message}";
                return StatusCode(500, _response);
            }
        }
    }
}
