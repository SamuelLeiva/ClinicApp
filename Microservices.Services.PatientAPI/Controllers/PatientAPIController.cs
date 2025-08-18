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
        public ResponseDto Post([FromBody] CreatePatientDto patientDto)
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
                    _response.Message = "Email already registered.";
                    return _response;
                }

                if (_dbContext.Patients.Any(p => p.PhoneNumber == patientDto.PhoneNumber))
                {
                    _response.IsSuccess = false;
                    _response.Message = "Phone number already registered.";
                    return _response;
                }

                Patient newPatient = _mapper.Map<Patient>(patientDto);
                _dbContext.Patients.Add(newPatient);
                _dbContext.SaveChanges();

                _response.Result = newPatient.PatientId;
                _response.Message = $"Patient {newPatient.PatientId} created successfully.";
            }
            catch (Exception ex)
            {
                _response.IsSuccess = false;
                _response.Message = $"Error while creating the patient: {ex.Message}";
            }
            return _response;
        }

        [HttpPut]
        [Route("{id:int}")]
        [Authorize(Roles = "Admin")]
        public ResponseDto Put(int id, [FromBody] UpdatePatientDto patientDto)
        {
            try
            {
                if (patientDto == null || id <= 0)
                {
                    _response.IsSuccess = false;
                    _response.Message = "Invalid patient data.";
                    return _response;
                }

                // Validación de unicidad para la actualización
                var existingEmail = _dbContext.Patients.FirstOrDefault(p => p.Email == patientDto.Email && p.PatientId != id);
                if (existingEmail != null)
                {
                    _response.IsSuccess = false;
                    _response.Message = "Email already registered.";
                    return _response;
                }

                var existingPhone = _dbContext.Patients.FirstOrDefault(p => p.PhoneNumber == patientDto.PhoneNumber && p.PatientId != id);
                if (existingPhone != null)
                {
                    _response.IsSuccess = false;
                    _response.Message = "Phone number already registered.";
                    return _response;
                }

                Patient? existingPatient = _dbContext.Patients.FirstOrDefault(p => p.PatientId == id);
                if (existingPatient == null)
                {
                    _response.IsSuccess = false;
                    _response.Message = "Patient not found.";
                    return _response;
                }

                _mapper.Map(patientDto, existingPatient);
                _dbContext.SaveChanges();

                _response.Result = _mapper.Map<UpdatePatientDto>(existingPatient);
                _response.Message = $"Patient {existingPatient.PatientId} updated successfully.";
            }
            catch (Exception ex)
            {
                _response.IsSuccess = false;
                _response.Message = $"Error while updating the patient: {ex.Message}";
            }
            return _response;
        }
    }
}
