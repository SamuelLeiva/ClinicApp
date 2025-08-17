using AutoMapper;
using Azure;
using Microservices.Services.DoctorAPI.Data;
using Microservices.Services.DoctorAPI.Models;
using Microservices.Services.DoctorAPI.Models.Dto;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

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
        public ResponseDto Get()
        {
            try
            {
                IEnumerable<Doctor> doctorList = _dbContext.Doctors.ToList();
                _response.Result = _mapper.Map<List<DoctorDto>>(doctorList);
                _response.Message = "Doctors retrieved successfully.";
            }
            catch (Exception ex)
            {
                _response.IsSuccess = false;
                _response.Message = $"Error ocurred while retrieving the doctors {ex.Message}";
            }
            return _response;
        }

        [HttpGet]
        [Route("{id:int}")]
        public ResponseDto Get(int id) {
            try
            {
                if (id <= 0)
                {
                    _response.IsSuccess = false;
                    _response.Message = "Invalid doctor ID.";
                }
                else
                {
                    Doctor? doctor = _dbContext.Doctors.FirstOrDefault(d => d.DoctorId == id);
                    if (doctor != null)
                    {
                        _response.Result = _mapper.Map<DoctorDto>(doctor);
                        _response.Message = "Doctor retrieved successfully.";
                    }
                    else
                    {
                        _response.IsSuccess = false;
                        _response.Message = "Doctor not found.";
                    }
                }
            }
            catch (Exception ex)
            {
                _response.IsSuccess = false;
                _response.Message = $"Error ocurred while retrieving the doctor with id {id}: {ex.Message}";
            }
            return _response;
        }

        [HttpPost]
        [Authorize(Roles = "Admin")]
        public ResponseDto Post([FromBody] DoctorDto doctorDto)
        {
            try
            {
                if (doctorDto == null)
                {
                    _response.IsSuccess = false;
                    _response.Message = "Invalid doctor data.";
                    return _response;
                }

                if (_dbContext.Doctors.Any(p => p.Email == doctorDto.Email))
                {
                    _response.IsSuccess = false;
                    _response.Message = "Email already registered.";
                    return _response;
                }

                if (_dbContext.Doctors.Any(d => d.PhoneNumber == doctorDto.PhoneNumber))
                {
                    _response.IsSuccess = false;
                    _response.Message = "Phone number already registered.";
                    return _response;
                }

                Doctor newDoctor = _mapper.Map<Doctor>(doctorDto);
                _dbContext.Doctors.Add(newDoctor);
                _dbContext.SaveChanges();

                _response.Result = newDoctor.DoctorId;
                _response.Message = "Doctor created successfully.";
            }
            catch (Exception ex)
            {
                _response.IsSuccess = false;
                _response.Message = $"Error ocurred while creating the doctor: {ex.Message}";
            }
            return _response;
        }

        [HttpPut]
        [Authorize(Roles = "Admin")]
        public ResponseDto Put([FromBody] DoctorDto doctorDto)
        {
            try
            {
                if (doctorDto == null || doctorDto.DoctorId <= 0)
                {
                    _response.IsSuccess = false;
                    _response.Message = "Invalid doctor data.";
                    return _response;
                }

                // Validación de unicidad para la actualización
                var existingEmail = _dbContext.Doctors.FirstOrDefault(d => d.Email == doctorDto.Email && d.DoctorId != doctorDto.DoctorId);
                if (existingEmail != null)
                {
                    _response.IsSuccess = false;
                    _response.Message = "Email already registered.";
                    return _response;
                }

                var existingPhone = _dbContext.Doctors.FirstOrDefault(d => d.PhoneNumber == doctorDto.PhoneNumber && d.DoctorId != doctorDto.DoctorId);
                if (existingPhone != null)
                {
                    _response.IsSuccess = false;
                    _response.Message = "Phone number already registered.";
                    return _response;
                }

                Doctor? existingDoctor = _dbContext.Doctors.FirstOrDefault(d => d.DoctorId == doctorDto.DoctorId);
                if (existingDoctor == null)
                {
                    _response.IsSuccess = false;
                    _response.Message = "Doctor not found.";
                    return _response;
                }

                _mapper.Map(doctorDto, existingDoctor);
                _dbContext.SaveChanges();

                _response.Result = _mapper.Map<DoctorDto>(existingDoctor);
                _response.Message = $"Doctor {existingDoctor.DoctorId} updated successfully.";
            }
            catch (Exception ex)
            {
                _response.IsSuccess = false;
                _response.Message = $"Error while updating the doctor: {ex.Message}";
            }
            return _response;
        }

    }
}
