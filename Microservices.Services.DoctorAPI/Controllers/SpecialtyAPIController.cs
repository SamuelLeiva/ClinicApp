using AutoMapper;
using Microservices.Services.DoctorAPI.Data;
using Microservices.Services.DoctorAPI.Models;
using Microservices.Services.DoctorAPI.Models.Dto;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace Microservices.Services.DoctorAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class SpecialtyAPIController : ControllerBase
    {
        // API similar a la de DoctorAPIController, pero para Specialty
        private readonly ApplicationDbContext _dbContext;
        private readonly IMapper _mapper;
        private ResponseDto _response;

        public SpecialtyAPIController(ApplicationDbContext dbContext, IMapper mapper)
        {
            _dbContext = dbContext;
            _mapper = mapper;
            _response = new ResponseDto();
        }

        [HttpGet]
        public ResponseDto Get()
        {
            try
            {
                var specialtyList = _dbContext.Specialties.ToList();
                _response.Result = _mapper.Map<List<SpecialtyDto>>(specialtyList);
                _response.Message = "Specialties retrieved successfully.";
            }
            catch (Exception ex)
            {
                _response.IsSuccess = false;
                _response.Message = $"Error occurred while retrieving the specialties: {ex.Message}";
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
                    _response.Message = "Invalid specialty ID.";
                }
                else
                {
                    var specialty = _dbContext.Specialties.FirstOrDefault(s => s.SpecialtyId == id);
                    if (specialty != null)
                    {
                        _response.Result = _mapper.Map<SpecialtyDto>(specialty);
                        _response.Message = "Specialty retrieved successfully.";
                    }
                    else
                    {
                        _response.IsSuccess = false;
                        _response.Message = "Specialty not found.";
                    }
                }
            }
            catch (Exception ex)
            {
                _response.IsSuccess = false;
                _response.Message = $"Error occurred while retrieving the specialty: {ex.Message}";
            }
            return _response;

        }

        [HttpPost]
        public ResponseDto Create([FromBody] SpecialtyDto specialtyDto)
        {
            try
            {
                if (specialtyDto == null || string.IsNullOrEmpty(specialtyDto.Name))
                {
                    _response.IsSuccess = false;
                    _response.Message = "Invalid specialty data.";
                }
                else
                {
                    var specialty = _mapper.Map<Specialty>(specialtyDto);
                    _dbContext.Specialties.Add(specialty);
                    _dbContext.SaveChanges();
                    _response.Result = _mapper.Map<SpecialtyDto>(specialty);
                    _response.Message = "Specialty created successfully.";
                }
            }
            catch (Exception ex)
            {
                _response.IsSuccess = false;
                _response.Message = $"Error occurred while creating the specialty: {ex.Message}";
            }
            return _response;
        }

        [HttpPut]
        [Route("{id:int}")]
        public ResponseDto Update(int id, [FromBody] SpecialtyDto specialtyDto)
        {
            try
            {
                if (id <= 0 || specialtyDto == null || string.IsNullOrEmpty(specialtyDto.Name))
                {
                    _response.IsSuccess = false;
                    _response.Message = "Invalid specialty data.";
                }
                else
                {
                    var specialty = _dbContext.Specialties.FirstOrDefault(s => s.SpecialtyId == id);
                    if (specialty != null)
                    {
                        specialty.Name = specialtyDto.Name;
                        _dbContext.SaveChanges();
                        _response.Result = _mapper.Map<SpecialtyDto>(specialty);
                        _response.Message = "Specialty updated successfully.";
                    }
                    else
                    {
                        _response.IsSuccess = false;
                        _response.Message = "Specialty not found.";
                    }
                }
            }
            catch (Exception ex)
            {
                _response.IsSuccess = false;
                _response.Message = $"Error occurred while updating the specialty: {ex.Message}";
            }
            return _response;
        }
    }
}
