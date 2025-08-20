using AutoMapper;
using Azure;
using Microservices.Services.DoctorAPI.Data;
using Microservices.Services.DoctorAPI.Models;
using Microservices.Services.DoctorAPI.Models.Dto;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace Microservices.Services.DoctorAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class DoctorAPIController : ControllerBase
    {
        private readonly ApplicationDbContext _dbContext;
        private readonly IMapper _mapper;
        private ResponseDto _response;

        public DoctorAPIController(ApplicationDbContext dbContext, IMapper mapper)
        {
            _dbContext = dbContext;
            _mapper = mapper;
            _response = new ResponseDto();
        }

        // CRUD Endpoints
        [HttpGet]
        public async Task<IActionResult> Get()
        {
            try
            {
                IEnumerable<Doctor> doctorList = await _dbContext.Doctors.ToListAsync();
                _response.Result = _mapper.Map<List<DoctorDto>>(doctorList);
                _response.Message = "Doctors retrieved successfully.";

                return Ok(_response);
            }
            catch (Exception ex)
            {
                _response.IsSuccess = false;
                _response.Message = $"Error ocurred while retrieving the doctors {ex.Message}";
                return StatusCode(500, _response);
            }
        }

        [HttpGet]
        [Route("{id:int}")]
        public async Task<IActionResult> Get(int id)
        {
            try
            {
                if (id <= 0)
                {
                    _response.IsSuccess = false;
                    _response.Message = "Invalid doctor ID.";
                    return BadRequest(_response);
                }

                Doctor? doctor = await _dbContext.Doctors.FirstOrDefaultAsync(d => d.DoctorId == id);
                if (doctor == null)
                {
                    _response.IsSuccess = false;
                    _response.Message = "Doctor not found.";
                    return NotFound(_response);
                }
                _response.Result = _mapper.Map<DoctorDto>(doctor);
                _response.Message = "Doctor retrieved successfully.";
                return Ok(_response);
            }
            catch (Exception ex)
            {
                _response.IsSuccess = false;
                _response.Message = $"Error ocurred while retrieving the doctor with id {id}: {ex.Message}";
                return StatusCode(500, _response);
            }
        }

        [HttpPost]
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> Post([FromBody] CreateDoctorDto doctorDto)
        {
            try
            {
                if (doctorDto == null)
                {
                    _response.IsSuccess = false;
                    _response.Message = "Invalid doctor data.";
                    return BadRequest(_response);
                }

                if (await _dbContext.Doctors.AnyAsync(p => p.Email == doctorDto.Email))
                {
                    _response.IsSuccess = false;
                    _response.Message = "Email already registered.";
                    return BadRequest(_response);
                }

                if (await _dbContext.Doctors.AnyAsync(d => d.PhoneNumber == doctorDto.PhoneNumber))
                {
                    _response.IsSuccess = false;
                    _response.Message = "Phone number already registered.";
                    return BadRequest(_response);
                }

                Doctor newDoctor = _mapper.Map<Doctor>(doctorDto);
                await _dbContext.Doctors.AddAsync(newDoctor);
                await _dbContext.SaveChangesAsync();

                _response.Result = newDoctor.DoctorId;
                _response.Message = $"Doctor {newDoctor.DoctorId} created successfully.";

                return CreatedAtRoute("Get", new { id = newDoctor.DoctorId }, _response);
            }
            catch (Exception ex)
            {
                _response.IsSuccess = false;
                _response.Message = $"Error ocurred while creating the doctor: {ex.Message}";
                return StatusCode(500, _response);
            }
        }

        [HttpPut]
        [Authorize(Roles = "Admin")]
        [Route("{id:int}")]
        public async Task<IActionResult> Put(int id, [FromBody] UpdateDoctorDto doctorDto)
        {
            try
            {
                if (doctorDto == null || id <= 0)
                {
                    _response.IsSuccess = false;
                    _response.Message = "Invalid doctor data.";
                    return BadRequest(_response);
                }

                // Validación de unicidad para la actualización
                var existingEmail = await _dbContext.Doctors.FirstOrDefaultAsync(d => d.Email == doctorDto.Email && d.DoctorId != id);
                if (existingEmail != null)
                {
                    _response.IsSuccess = false;
                    _response.Message = "Email already registered.";
                    return BadRequest(_response);
                }

                var existingPhone = await _dbContext.Doctors.FirstOrDefaultAsync(d => d.PhoneNumber == doctorDto.PhoneNumber && d.DoctorId != id);
                if (existingPhone != null)
                {
                    _response.IsSuccess = false;
                    _response.Message = "Phone number already registered.";
                    return BadRequest(_response);
                }

                Doctor? existingDoctor = await _dbContext.Doctors.FirstOrDefaultAsync(d => d.DoctorId == id);
                if (existingDoctor == null)
                {
                    _response.IsSuccess = false;
                    _response.Message = "Doctor not found.";
                    return NotFound(_response);
                }

                _mapper.Map(doctorDto, existingDoctor);
                await _dbContext.SaveChangesAsync();

                _response.Result = _mapper.Map<UpdateDoctorDto>(existingDoctor);
                _response.Message = $"Doctor {existingDoctor.DoctorId} updated successfully.";
                return Ok(_response);
            }
            catch (Exception ex)
            {
                _response.IsSuccess = false;
                _response.Message = $"Error while updating the doctor: {ex.Message}";
                return StatusCode(500, _response);
            }
        }

    }
}
