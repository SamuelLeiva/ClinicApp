using CitasMedicas.Web.Models.Dto;
using CitasMedicas.Web.Services.IServices;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;

namespace CitasMedicas.Web.Controllers
{
    public class SpecialtyController : Controller
    {
        private readonly ISpecialtyService _specialtyService;
        public SpecialtyController(ISpecialtyService specialtyService)
        {
            _specialtyService = specialtyService;
        }

        [HttpGet]
        public async Task<IActionResult> SpecialtyIndex()
        {
            List<SpecialtyDto>? specialtyList = new();
            ResponseDto? responseDto = await _specialtyService.GetSpecialtiesAsync();

            if (responseDto != null && responseDto.IsSuccess)
            {
                specialtyList = JsonConvert.DeserializeObject<List<SpecialtyDto>>(Convert.ToString(responseDto.Result));
            }
            else
            {
                TempData["error"] = responseDto?.Message ?? "Error al obtener las especialidades.";
            }

            return View(specialtyList);
        }

        #region Create Specialty

        [HttpGet]
        public async Task<IActionResult> SpecialtyCreate()
        {
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> SpecialtyCreate(CreateSpecialtyDto specialtyDto)
        {
            if (ModelState.IsValid)
            {
                ResponseDto responseDto = await _specialtyService.CreateSpecialtyAsync(specialtyDto);
                if (responseDto != null && responseDto.IsSuccess)
                {
                    TempData["success"] = responseDto.Message ?? "Especialidad creada correctamente.";
                    return RedirectToAction(nameof(SpecialtyIndex));
                }
                else
                {
                    TempData["error"] = responseDto?.Message ?? "Error al crear la especialidad.";
                }
            }
            return View(specialtyDto);
        }

        #endregion

        #region Update Specialty

        [HttpGet]
        public async Task<IActionResult> SpecialtyEdit(int specialtyId)
        {
            ResponseDto? responseDto = await _specialtyService.GetSpecialtyByIdAsync(specialtyId);
            if (responseDto != null && responseDto.IsSuccess)
            {
                SpecialtyDto? specialtyDto = JsonConvert.DeserializeObject<SpecialtyDto>(Convert.ToString(responseDto.Result));
                if (specialtyDto != null)
                {
                    return View(new UpdateSpecialtyDto
                    {
                        SpecialtyId = specialtyId,
                        Name = specialtyDto.Name
                    });

                }
                else
                {
                    TempData["error"] = "Especialidad no encontrada.";
                }
            }
            else
            {
                TempData["error"] = responseDto?.Message ?? "Error al obtener el producto.";
            }

            return RedirectToAction(nameof(SpecialtyIndex));
        }

        [HttpPost]
        public async Task<IActionResult> SpecialtyEdit(UpdateSpecialtyDto specialtyDto)
        {
            if (ModelState.IsValid)
            {
                ResponseDto responseDto = await _specialtyService.UpdateSpecialtyAsync(specialtyDto);
                if (responseDto != null && responseDto.IsSuccess)
                {
                    TempData["success"] = responseDto.Message ?? "Especialidad actualizada correctamente.";
                    return RedirectToAction(nameof(SpecialtyIndex));
                }
                else
                {
                    TempData["error"] = responseDto?.Message ?? "Error al actualizar la especialidad.";
                }
            }
            return View(specialtyDto);
        }

        #endregion
    }
}
