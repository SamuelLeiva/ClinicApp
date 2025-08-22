using CitasMedicas.Web.Models;
using CitasMedicas.Web.Models.Patient;
using CitasMedicas.Web.Services.IServices;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;

namespace CitasMedicas.Web.Controllers
{
    public class PatientController : Controller
    {
        private readonly IPatientService _patientService;
        public PatientController(IPatientService patientService)
        {
            _patientService = patientService;
        }

        [HttpGet]
        public async Task<IActionResult> PatientIndex()
        {
            List<PatientDto>? patientList = new();
            ResponseDto? responseDto = await _patientService.GetPatientsAsync();

            if(responseDto != null && responseDto.IsSuccess)
            {
                patientList = JsonConvert.DeserializeObject<List<PatientDto>>(Convert.ToString(responseDto.Result));
            }
            else
            {
                TempData["error"] = responseDto?.Message ?? "Error al obtener los pacientes.";
            }

            return View(patientList);
        }

        #region Create Patient

        [HttpGet]
        public async Task<IActionResult> PatientCreate()
        {
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> PatientCreate(CreatePatientDto patientDto)
        {
            if (ModelState.IsValid)
            {
                ResponseDto responseDto = await _patientService.CreatePatientAsync(patientDto);
                if (responseDto != null && responseDto.IsSuccess)
                {
                    TempData["success"] = responseDto.Message ?? "Paciente creado correctamente.";
                    return RedirectToAction(nameof(PatientIndex));
                }
                else
                {
                    TempData["error"] = responseDto?.Message ?? "Error al crear el paciente.";
                }
            }

            return View(patientDto);
        }

        #endregion

        #region Update Patient

        [HttpGet]
        public async Task<IActionResult> PatientEdit(int patientId)
        {
            ResponseDto? responseDto = await _patientService.GetPatientByIdAsync(patientId);
            if (responseDto != null && responseDto.IsSuccess)
            {
                PatientDto? patientDto = JsonConvert.DeserializeObject<PatientDto>(Convert.ToString(responseDto.Result));
                if (patientDto != null)
                {
                    return View(new UpdatePatientDto
                    {
                        PatientId = patientId,
                        Name = patientDto.Name,
                        LastName = patientDto.LastName,
                        DateOfBirth = patientDto.DateOfBirth,
                        PhoneNumber = patientDto.PhoneNumber,
                        Email = patientDto.Email
                    });
                }
                else
                {
                    TempData["error"] = "Paciente no encontrado.";
                }
            }
            else
            {
                TempData["error"] = responseDto?.Message ?? "Error al obtener el paciente.";
            }

            return RedirectToAction(nameof(PatientIndex));
        }

        [HttpPost]
        public async Task<IActionResult> PatientEdit(UpdatePatientDto patientDto)
        {
            if (ModelState.IsValid)
            {
                ResponseDto responseDto = await _patientService.UpdatePatientAsync(patientDto);
                if (responseDto != null && responseDto.IsSuccess)
                {
                    TempData["success"] = responseDto.Message ?? "Paciente actualizado correctamente.";
                    return RedirectToAction(nameof(PatientIndex));
                }
                else
                {
                    TempData["error"] = responseDto?.Message ?? "Error al actualizar el paciente.";
                }
            }
            return View(patientDto);
        }

        #endregion
    }
}
