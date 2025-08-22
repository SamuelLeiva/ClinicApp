using CitasMedicas.Web.Models.Dto;
using CitasMedicas.Web.Services;
using CitasMedicas.Web.Services.IServices;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;

namespace CitasMedicas.Web.Controllers
{
    [Authorize]
    public class AppointmentController : Controller
    {
        private readonly IAppointmentService _appointmentService;
        private readonly IPatientService _patientService;
        private readonly IDoctorService _doctorService;
        public AppointmentController(IAppointmentService appointmentService, IPatientService patientService, IDoctorService doctorService)
        {
            _appointmentService = appointmentService;
            _patientService = patientService;
            _doctorService = doctorService;
        }

        public async Task<IActionResult> AppointmentIndex()
        {
            List<AppointmentDto>? appointmentList = new();
            ResponseDto? responseDto = await _appointmentService.GetAppointmentsAsync();

            if (responseDto != null && responseDto.IsSuccess)
            {
                appointmentList = JsonConvert.DeserializeObject<List<AppointmentDto>>(Convert.ToString(responseDto.Result));
            }
            else
            {
                TempData["error"] = responseDto?.Message ?? "Error al obtener las citas.";
            }

            return View(appointmentList);
        }

        #region Appointment Details

        public async Task<IActionResult> AppointmentDetails(int id)
        {
            var response = await _appointmentService.GetAppointmentByIdAsync(id);
            if (response?.IsSuccess == true)
            {
                var appointment = JsonConvert.DeserializeObject<AppointmentDto>(
                    Convert.ToString(response.Result)
                );
                return View(appointment);
            }
            TempData["error"] = response?.Message ?? "Cita no encontrada.";
            return NotFound();
        }

        #endregion

        #region Create Appointment

        [HttpGet]
        public async Task<IActionResult> CreateAppointment()
        {
            // Obtener las listas de pacientes y médicos para los dropdowns
            var patientsResponse = await _patientService.GetPatientsAsync();
            var doctorsResponse = await _doctorService.GetDoctorsAsync();

            if (patientsResponse?.IsSuccess == true && doctorsResponse?.IsSuccess == true)
            {
                var patientList = JsonConvert.DeserializeObject<List<PatientDto>>(
                    Convert.ToString(patientsResponse.Result)
                );
                var doctorList = JsonConvert.DeserializeObject<List<DoctorDto>>(
                    Convert.ToString(doctorsResponse.Result)
                );

                ViewBag.Patients = patientList;
                ViewBag.Doctors = doctorList;
                return View();
            }

            TempData["error"] = "No se pudieron cargar los datos necesarios para crear la cita.";
            return RedirectToAction(nameof(Index));
        }

        [HttpPost]
        public async Task<IActionResult> CreateAppointment(CreateAppointmentDto appointmentDto)
        {
            if (ModelState.IsValid)
            {
                var response = await _appointmentService.CreateAppointmentAsync(appointmentDto);
                if (response?.IsSuccess == true)
                {
                    TempData["success"] = "Cita creada exitosamente.";
                    return RedirectToAction(nameof(Index));
                }
                TempData["error"] = response?.Message ?? "Error al crear la cita.";
            }

            // Si falla, recargar las listas para mostrar la vista de nuevo con el error
            var patientsResponse = await _patientService.GetPatientsAsync();
            var doctorsResponse = await _doctorService.GetDoctorsAsync();
            if (patientsResponse?.IsSuccess == true && doctorsResponse?.IsSuccess == true)
            {
                ViewBag.Patients = JsonConvert.DeserializeObject<List<PatientDto>>(
                    Convert.ToString(patientsResponse.Result)
                );
                ViewBag.Doctors = JsonConvert.DeserializeObject<List<DoctorDto>>(
                    Convert.ToString(doctorsResponse.Result)
                );
            }

            return View(appointmentDto);
        }

        #endregion

        #region Get Appointments Filter

        [HttpGet]
        public async Task<IActionResult> Filter(int? patientId, int? doctorId)
        {
            if (patientId == null && doctorId == null)
            {
                TempData["error"] = "Debe seleccionar al menos un paciente o un doctor para filtrar.";
                return RedirectToAction(nameof(Index));
            }

            var response = await _appointmentService.GetAppointmentFilter(patientId.GetValueOrDefault(), doctorId.GetValueOrDefault());

            if (response?.IsSuccess == true)
            {
                var appointmentList = JsonConvert.DeserializeObject<List<AppointmentDto>>(
                    Convert.ToString(response.Result)
                );
                return View("Index", appointmentList); // Reutilizar la vista Index para mostrar los resultados
            }

            TempData["error"] = response?.Message ?? "No se encontraron citas que coincidan con los filtros.";
            return RedirectToAction(nameof(Index));
        }

        #endregion 
    }
}
