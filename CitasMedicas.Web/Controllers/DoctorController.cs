using CitasMedicas.Web.Models.Dto;
using CitasMedicas.Web.Services.IServices;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;

namespace CitasMedicas.Web.Controllers
{
    public class DoctorController : Controller
    {
        private readonly IDoctorService _doctorService;
        public DoctorController(IDoctorService doctorService)
        {
            _doctorService = doctorService;
        }

        [HttpGet]
        public async Task<IActionResult> DoctorIndex()
        {
            List<DoctorDto>? doctorList = new();
            ResponseDto? responseDto = await _doctorService.GetDoctorsAsync();
            if(responseDto != null && responseDto.IsSuccess)
            {
                doctorList = JsonConvert.DeserializeObject<List<DoctorDto>>(Convert.ToString(responseDto.Result));
            }
            else
            {
                TempData["error"] = responseDto?.Message ?? "Error al obtener los doctores.";
            }
            return View(doctorList);
        }

        #region Create Doctor

        [HttpGet]
        public async Task<IActionResult> DoctorCreate()
        {
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> DoctorCreate(CreateDoctorDto doctorDto)
        {
            if (ModelState.IsValid)
            {
                ResponseDto responseDto = await _doctorService.CreateDoctorAsync(doctorDto);
                if (responseDto != null && responseDto.IsSuccess)
                {
                    TempData["success"] = responseDto.Message ?? "Doctor creado correctamente.";
                    return RedirectToAction(nameof(DoctorIndex));
                }
                else
                {
                    TempData["error"] = responseDto?.Message ?? "Error al crear el doctor.";
                }
            }
            return View(doctorDto);
        }

        #endregion

        #region Update Patient
        [HttpGet]
        public async Task<IActionResult> DoctorUpdate(int doctorId)
        {
            ResponseDto? responseDto = await _doctorService.GetDoctorByIdAsync(doctorId);
            if (responseDto != null && responseDto.IsSuccess)
            {
                DoctorDto? doctorDto = JsonConvert.DeserializeObject<DoctorDto>(Convert.ToString(responseDto.Result));
                if (doctorDto != null)
                {
                    UpdateDoctorDto updateDoctorDto = new()
                    {
                        DoctorId = doctorDto.DoctorId,
                        Name = doctorDto.Name,
                        LastName = doctorDto.LastName,
                        PhoneNumber = doctorDto.PhoneNumber,
                        Email = doctorDto.Email,
                        SpecialtyId = doctorDto.SpecialtyId
                    };
                    return View(updateDoctorDto);
                }
            }
            TempData["error"] = responseDto?.Message ?? "Error al obtener el doctor.";
            return RedirectToAction(nameof(DoctorIndex));
        }

        [HttpPost]
        public async Task<IActionResult> DoctorUpdate(UpdateDoctorDto doctorDto)
        {
            if (ModelState.IsValid)
            {
                ResponseDto responseDto = await _doctorService.UpdateDoctorAsync(doctorDto);
                if (responseDto != null && responseDto.IsSuccess)
                {
                    TempData["success"] = responseDto.Message ?? "Doctor actualizado correctamente.";
                    return RedirectToAction(nameof(DoctorIndex));
                }
                else
                {
                    TempData["error"] = responseDto?.Message ?? "Error al actualizar el doctor.";
                }
            }
            return View(doctorDto);
        }

        #endregion
    }
}
