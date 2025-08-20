using AutoMapper;
using Microservices.Services.PatientAPI.Data;
using Microservices.Services.PatientAPI.Models;
using Microservices.Services.PatientAPI.Models.Dto;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

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
        public async Task<IActionResult> Get()
        {
            try
            {
                IEnumerable<Patient> patientList = await _dbContext.Patients.ToListAsync();

                _response.Result = _mapper.Map<List<PatientDto>>(patientList);
                _response.Message = "Patients retrieved successfully.";

                return Ok(_response);
            }
            catch (Exception ex)
            {
                _response.IsSuccess = false;
                _response.Message = $"Error ocurred while retrieving the patients {ex.Message}";
                return StatusCode(500, _response);
            }
        }

        [HttpGet]
        [Route("{id:int}", Name = "GetPatientById")]
        public async Task<IActionResult> Get(int id)
        {
            try
            {
                if (id <= 0)
                {
                    _response.IsSuccess = false;
                    _response.Message = "Invalid patient ID.";
                    return BadRequest(_response);
                }

                Patient? patient = await _dbContext.Patients.FirstOrDefaultAsync(p => p.PatientId == id);
                if (patient == null)
                {
                    _response.IsSuccess = false;
                    _response.Message = "Patient not found.";
                    return NotFound(_response);
                }

                _response.Result = _mapper.Map<PatientDto>(patient);
                _response.Message = "Patient retrieved successfully.";
                return Ok(_response);

            }
            catch (Exception ex)
            {
                _response.IsSuccess = false;
                _response.Message = $"Error ocurred while retrieving the patient with id {id}: {ex.Message}";
                return StatusCode(500, _response);
            }
        }

        [HttpPost]
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> Post([FromBody] CreatePatientDto patientDto)
        {
            try
            {
                if (patientDto == null)
                {
                    _response.IsSuccess = false;
                    _response.Message = "Invalid patient data.";
                    return BadRequest(_response);
                }

                if ( await _dbContext.Patients.AnyAsync(p => p.Email == patientDto.Email))
                {
                    _response.IsSuccess = false;
                    _response.Message = "Email already registered.";
                    return BadRequest(_response);
                }

                if (await _dbContext.Patients.AnyAsync(p => p.PhoneNumber == patientDto.PhoneNumber))
                {
                    _response.IsSuccess = false;
                    _response.Message = "Phone number already registered.";
                    return BadRequest(_response);
                }

                Patient newPatient = _mapper.Map<Patient>(patientDto);
                await _dbContext.Patients.AddAsync(newPatient);
                await _dbContext.SaveChangesAsync();

                _response.Result = _mapper.Map<PatientDto>(newPatient);
                _response.Message = $"Patient created successfully.";

                return CreatedAtRoute("GetPatientById", new { id = newPatient.PatientId }, _response);
            }
            catch (Exception ex)
            {
                _response.IsSuccess = false;
                _response.Message = $"Error while creating the patient: {ex.Message}";
                return StatusCode(500, _response);
            }
        }

        [HttpPut]
        [Route("{id:int}")]
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> Put(int id, [FromBody] UpdatePatientDto patientDto)
        {
            try
            {
                if (patientDto == null || id <= 0)
                {
                    _response.IsSuccess = false;
                    _response.Message = "Invalid patient data.";
                    return BadRequest(_response);
                }

                // Validación de unicidad para la actualización
                var existingEmail = await _dbContext.Patients.FirstOrDefaultAsync(p => p.Email == patientDto.Email && p.PatientId != id);
                if (existingEmail != null)
                {
                    _response.IsSuccess = false;
                    _response.Message = "Email already registered.";
                    return BadRequest(_response);
                }

                var existingPhone = await _dbContext.Patients.FirstOrDefaultAsync(p => p.PhoneNumber == patientDto.PhoneNumber && p.PatientId != id);
                if (existingPhone != null)
                {
                    _response.IsSuccess = false;
                    _response.Message = "Phone number already registered.";
                    return BadRequest(_response);
                }

                Patient? existingPatient = await _dbContext.Patients.FirstOrDefaultAsync(p => p.PatientId == id);
                if (existingPatient == null)
                {
                    _response.IsSuccess = false;
                    _response.Message = "Patient not found.";
                    return NotFound(_response);
                }

                _mapper.Map(patientDto, existingPatient);
                await _dbContext.SaveChangesAsync();

                _response.Result = _mapper.Map<UpdatePatientDto>(existingPatient);
                _response.Message = $"Patient {existingPatient.PatientId} updated successfully.";
                return Ok(_response);
            }
            catch (Exception ex)
            {
                _response.IsSuccess = false;
                _response.Message = $"Error while updating the patient: {ex.Message}";
                return StatusCode(500, _response);
            }
        }
    }
}
