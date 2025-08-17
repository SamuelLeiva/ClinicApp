using AutoMapper;
using Microservices.Services.PatientAPI.Data;
using Microservices.Services.PatientAPI.Models;
using Microservices.Services.PatientAPI.Models.Dto;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Microservices.Services.PatientAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class PatientAPIController : ControllerBase
    {
        private readonly ApplicationDbContext _dbContext;
        private ResponseDto _response;
        private readonly IMapper _mapper;

        public PatientAPIController(ApplicationDbContext dbContext, IMapper mapper)
        {
            _dbContext = dbContext;
            _response = new ResponseDto();
            _mapper = mapper;
        }

        [HttpGet]
        public ResponseDto Get()
        {
            try
            {
                IEnumerable<Patient> patientList = _dbContext.Patients.ToList();
                _response.Result = _mapper.Map<List<PatientDto>>(patientList);
                _response.Message = "Patients retrieved successfully.";
            }
            catch (Exception ex)
            {
                _response.IsSuccess = false;
                _response.Message = $"Error ocurred while retrieving the patients {ex.Message}";
            }
            return _response;
        }

        [HttpGet]
        [Route("{id:int}")]
        public ResponseDto Get(int id)
        {
            try
            {
                if (id <= 0)
                {
                    _response.IsSuccess = false;
                    _response.Message = "Invalid patient ID.";
                }
                else
                {
                    Patient? patient = _dbContext.Patients.FirstOrDefault(p => p.PatientId == id);
                    if (patient != null)
                    {
                        _response.Result = _mapper.Map<PatientDto>(patient);
                        _response.Message = "Patient retrieved successfully.";
                    } 
                    else
                    {
                        _response.IsSuccess = false;
                        _response.Message = "Patient not found.";
                    }
                }
            }
            catch (Exception ex)
            {
                _response.IsSuccess = false;
                _response.Message = $"Error ocurred while retrieving the patient with id {id}: {ex.Message}";
            }
            return _response;
        }

        [HttpPost]
        [Authorize(Roles = "Admin")]
        public ResponseDto Post([FromBody] PatientDto patientDto)
        {
            try
            {
                if (patientDto == null)
                {
                    _response.IsSuccess = false;
                    _response.Message = "Invalid patient data.";
                    return _response;
                }

                if (_dbContext.Patients.Any(p => p.Email == patientDto.Email))
                {
                    _response.IsSuccess = false;
                    _response.Message = "El correo electrónico ya está registrado.";
                    return _response;
                }

                if (_dbContext.Patients.Any(p => p.PhoneNumber == patientDto.PhoneNumber))
                {
                    _response.IsSuccess = false;
                    _response.Message = "El número de teléfono ya está registrado.";
                    return _response;
                }

                Patient newPatient = _mapper.Map<Patient>(patientDto);
                _dbContext.Patients.Add(newPatient);
                _dbContext.SaveChanges();

                _response.Result = newPatient.PatientId;
                _response.Message = "Paciente creado exitosamente.";
            }
            catch (Exception ex)
            {
                _response.IsSuccess = false;
                _response.Message = $"Error al crear el paciente: {ex.Message}";
            }
            return _response;
        }

        [HttpPut]
        [Authorize(Roles = "Admin")]
        public ResponseDto Put([FromBody] PatientDto patientDto)
        {
            try
            {
                if (patientDto == null || patientDto.PatientId <= 0)
                {
                    _response.IsSuccess = false;
                    _response.Message = "Datos de paciente inválidos para la actualización.";
                    return _response;
                }

                // Validación de unicidad para la actualización
                var existingEmail = _dbContext.Patients.FirstOrDefault(p => p.Email == patientDto.Email && p.PatientId != patientDto.PatientId);
                if (existingEmail != null)
                {
                    _response.IsSuccess = false;
                    _response.Message = "El correo electrónico ya está registrado por otro paciente.";
                    return _response;
                }

                var existingPhone = _dbContext.Patients.FirstOrDefault(p => p.PhoneNumber == patientDto.PhoneNumber && p.PatientId != patientDto.PatientId);
                if (existingPhone != null)
                {
                    _response.IsSuccess = false;
                    _response.Message = "El número de teléfono ya está registrado por otro paciente.";
                    return _response;
                }

                Patient? existingPatient = _dbContext.Patients.FirstOrDefault(p => p.PatientId == patientDto.PatientId);
                if (existingPatient == null)
                {
                    _response.IsSuccess = false;
                    _response.Message = "Paciente no encontrado.";
                    return _response;
                }

                _mapper.Map(patientDto, existingPatient);
                _dbContext.SaveChanges();

                _response.Result = _mapper.Map<PatientDto>(existingPatient);
                _response.Message = $"Paciente {patientDto.PatientId} actualizado exitosamente.";
            }
            catch (Exception ex)
            {
                _response.IsSuccess = false;
                _response.Message = $"Error al actualizar el paciente: {ex.Message}";
            }
            return _response;
        }
    }
}
