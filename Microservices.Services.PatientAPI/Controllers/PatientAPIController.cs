using AutoMapper;
using Microservices.Services.PatientAPI.Data;
using Microservices.Services.PatientAPI.Models;
using Microservices.Services.PatientAPI.Models.Dto;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using static Azure.Core.HttpHeader;

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
                if (patientDto != null)
                {
                    var newPatient = new Patient()
                    {
                        Name = patientDto.Name,
                        LastName = patientDto.LastName,
                        DateOfBirth = patientDto.DateOfBirth,
                        PhoneNumber = patientDto.PhoneNumber,
                        Email = patientDto.Email
                    };
                    _dbContext.Patients.Add(newPatient);
                    _dbContext.SaveChanges();

                    _response.Result = newPatient.PatientId;
                    _response.Message = $"Patient {patientDto.PatientId} created successfully.";
                }
                else
                {
                    _response.IsSuccess = false;
                    _response.Message = "Patient data is null or invalid.";
                }
            }
            catch (Exception ex)
            {
                _response.IsSuccess = false;
                _response.Message = $"Error ocurred while creating the patient: {ex.Message}";
            }
            return _response;
        }

        [HttpPut]
        [Authorize(Roles = "Admin")]
        public ResponseDto Put([FromBody] PatientDto patientDto)
        {
            try
            {
                if (patientDto != null && patientDto.PatientId > 0)
                {
                    Patient? existingPatient = _dbContext.Patients.FirstOrDefault(p => p.PatientId == patientDto.PatientId);
                    if (existingPatient != null)
                    {
                        existingPatient.Name = patientDto.Name;
                        existingPatient.LastName = patientDto.LastName;
                        existingPatient.DateOfBirth = patientDto.DateOfBirth;
                        existingPatient.PhoneNumber = patientDto.PhoneNumber;
                        existingPatient.Email = patientDto.Email;
                    }
                    else
                    {
                        _response.IsSuccess = false;
                        _response.Message = "Patient not found.";
                        return _response;
                    }

                    _dbContext.SaveChanges();
                    _response.Result = _mapper.Map<PatientDto>(existingPatient);
                    _response.Message = $"Patient {patientDto.PatientId} updated successfully.";
                }
                else
                {
                    _response.IsSuccess = false;
                    _response.Message = "Invalid patient data.";
                }
            }
            catch (Exception ex)
            {
                _response.IsSuccess = false;
                _response.Message = $"Error ocurred while updating the patient {patientDto.Name} {patientDto.LastName}: {ex.Message}";
            }
            return _response;
        }
    }
}
