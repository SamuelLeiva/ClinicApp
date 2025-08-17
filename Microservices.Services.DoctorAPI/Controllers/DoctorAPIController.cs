using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace Microservices.Services.DoctorAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class DoctorAPIController : ControllerBase
    {
        [HttpGet]
        public IActionResult GetSpecialties()
        {
            // This is a placeholder for the actual implementation.
            // In a real application, you would retrieve the specialties from a database or another service.
            var specialties = new List<Models.Dto.SpecialtyDto>
            {
                new Models.Dto.SpecialtyDto { Id = 1, Name = "Cardiology" },
                new Models.Dto.SpecialtyDto { Id = 2, Name = "Neurology" },
                new Models.Dto.SpecialtyDto { Id = 3, Name = "Pediatrics" }
            };
            return Ok(specialties);
        }
    }
}
