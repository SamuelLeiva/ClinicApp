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

        [HttpGet]
        public async Task<IActionResult> GetAppointments()
        {
            try
            {
                var appointmentList = await _dbContext.Appointments.ToListAsync();

                var appointmentDtoList = _mapper.Map<List<AppointmentDto>>(appointmentList);

                var response = new ResponseDto
                {
                    Result = appointmentDtoList,
                    IsSuccess = true,
                    Message = "Appointments retrieved successfully."
                };
                return Ok(response);
            }
            catch (Exception ex)
            {
                var response = new ResponseDto
                {
                    IsSuccess = false,
                    Message = $"Error retrieving appointments: {ex.Message}"
                };
                return StatusCode(500, response);
            }
        }

        [HttpGet]
        [Route("{id:int}", Name = "GetAppointmentById")]
        public async Task<IActionResult> GetAppointmentById(int id)
        {
            try
            {
                var appointment = await _dbContext.Appointments.FirstOrDefaultAsync(a => a.AppointmentId == id);
                if (appointment == null)
                {

                    _response.IsSuccess = false;
                    _response.Message = "Appointment not found.";
                    
                    return NotFound(_response);
                }

                var appointmentDto = _mapper.Map<AppointmentDto>(appointment);

                _response.Result = appointmentDto;
                _response.IsSuccess = true;
                _response.Message = "Appointment retrieved successfully.";
                
                return Ok(_response);
            }
            catch (Exception ex)
            {

                _response.IsSuccess = false;
                _response.Message = $"Error retrieving appointment: {ex.Message}";
                
                return StatusCode(500, _response);
            }
        }

        [HttpGet]
        [Route("filter")]
        public async Task<IActionResult> GetAppointmentsByFilter([FromQuery] int? patientId, [FromQuery] int? doctorId)
        {
            try
            {
                // Que es queryable?
                IQueryable<Appointment> query = _dbContext.Appointments.AsQueryable();

                if (patientId.HasValue)
                {
                    query = query.Where(a => a.PatientId == patientId.Value);
                }

                if (doctorId.HasValue)
                {
                    query = query.Where(a => a.DoctorId == doctorId.Value);
                }

                var filteredAppointments = await query.ToListAsync();
                var appointmentDtoList = _mapper.Map<List<AppointmentDto>>(filteredAppointments);

                var response = new ResponseDto
                {
                    Result = appointmentDtoList,
                    IsSuccess = true,
                    Message = "Appointments retrieved successfully based on filter."
                };
                return Ok(response);
            }
            catch (Exception ex)
            {
                var response = new ResponseDto
                {
                    IsSuccess = false,
                    Message = $"Error retrieving appointments with filter: {ex.Message}"
                };
                return StatusCode(500, response);
            }
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

                return CreatedAtRoute("GetAppointmentById", new { id = newAppointment.AppointmentId }, _response);
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
