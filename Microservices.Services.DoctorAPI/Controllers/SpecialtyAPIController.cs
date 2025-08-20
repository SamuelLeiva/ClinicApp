using AutoMapper;
using Microservices.Services.DoctorAPI.Data;
using Microservices.Services.DoctorAPI.Models;
using Microservices.Services.DoctorAPI.Models.Dto;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Authorization;

namespace Microservices.Services.DoctorAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class SpecialtyAPIController : ControllerBase
    {
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
        public async Task<IActionResult> Get()
        {
            try
            {
                IEnumerable<Specialty> specialtyList = await _dbContext.Specialties.ToListAsync();

                _response.Result = _mapper.Map<List<SpecialtyDto>>(specialtyList);
                _response.Message = "Specialties retrieved successfully.";

                return Ok(_response);
            }
            catch (Exception ex)
            {
                _response.IsSuccess = false;
                _response.Message = $"Error occurred while retrieving the specialties: {ex.Message}";
                return StatusCode(500, _response);
            }
        }

        [HttpGet]
        [Route("{id:int}", Name = "GetSpecialtyById")]
        public async Task<IActionResult> Get(int id)
        {
            try
            {
                if (id <= 0)
                {
                    _response.IsSuccess = false;
                    _response.Message = "Invalid specialty ID.";
                    return BadRequest(_response);
                }

                Specialty? specialty = await _dbContext.Specialties.FirstOrDefaultAsync(s => s.SpecialtyId == id);
                if (specialty == null)
                {
                    _response.IsSuccess = false;
                    _response.Message = "Specialty not found.";
                    return NotFound(_response);
                }

                _response.Result = _mapper.Map<SpecialtyDto>(specialty);
                _response.Message = "Specialty retrieved successfully.";
                return Ok(_response);

            }
            catch (Exception ex)
            {
                _response.IsSuccess = false;
                _response.Message = $"Error occurred while retrieving the specialty: {ex.Message}";
                return StatusCode(500, _response);
            }
        }

        [HttpPost]
        public async Task<IActionResult> Post([FromBody] CreateSpecialtyDto specialtyDto)
        {
            try
            {
                if (specialtyDto == null)
                {
                    _response.IsSuccess = false;
                    _response.Message = "Invalid specialty data.";
                    return BadRequest(_response);
                }

                if (await _dbContext.Specialties.AnyAsync(s => s.Name == specialtyDto.Name))
                {
                    _response.IsSuccess = false;
                    _response.Message = "Specialty already registered.";
                    return BadRequest(_response);
                }

                Specialty newSpecialty = _mapper.Map<Specialty>(specialtyDto);
                await _dbContext.Specialties.AddAsync(newSpecialty);
                await _dbContext.SaveChangesAsync();

                _response.Result = _mapper.Map<SpecialtyDto>(newSpecialty);
                _response.Message = $"Specialty created successfully.";

                return CreatedAtRoute("GetSpecialtyById", new { id = newSpecialty.SpecialtyId }, _response);
            }
            catch (Exception ex)
            {
                _response.IsSuccess = false;
                _response.Message = $"Error occurred while creating the specialty: {ex.Message}";
                return StatusCode(500, _response);
            }
        }

        [HttpPut]
        [Route("{id:int}")]
        public async Task<IActionResult> Update(int id, [FromBody] UpdateSpecialtyDto specialtyDto)
        {
            try
            {
                if (id <= 0 || specialtyDto == null)
                {
                    _response.IsSuccess = false;
                    _response.Message = "Invalid specialty data.";
                    return BadRequest(_response);
                }

                var existingName = await _dbContext.Specialties.FirstOrDefaultAsync(s => s.Name == specialtyDto.Name && s.SpecialtyId != id);
                if (existingName != null)
                {
                    _response.IsSuccess = false;
                    _response.Message = "Specialty already registered.";
                    return BadRequest(_response);
                }

                Specialty? existingSpecialty = await _dbContext.Specialties.FirstOrDefaultAsync(s => s.SpecialtyId == id);
                if (existingSpecialty == null)
                {
                    _response.IsSuccess = false;
                    _response.Message = "Specialty not found.";
                    return NotFound(_response);
                }

                _mapper.Map(specialtyDto, existingSpecialty);
                await _dbContext.SaveChangesAsync();

                _response.Result = _mapper.Map<UpdateSpecialtyDto>(existingSpecialty);
                _response.Message = $"Specialty {existingSpecialty.SpecialtyId} updated successfully.";
                return Ok(_response);

            }
            catch (Exception ex)
            {
                _response.IsSuccess = false;
                _response.Message = $"Error occurred while updating the specialty: {ex.Message}";
                return StatusCode(500, _response);
            }
        }
    }
}
