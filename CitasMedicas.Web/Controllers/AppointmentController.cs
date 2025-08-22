using CitasMedicas.Web.Models;
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
            // 1. Obtener los datos de la API
            ResponseDto? appointmentsResponse = await _appointmentService.GetAppointmentsAsync();
            ResponseDto? patientsResponse = await _patientService.GetPatientsAsync();
            ResponseDto? doctorsResponse = await _doctorService.GetDoctorsAsync();

            // 2. Manejar posibles errores
            if (appointmentsResponse == null || !appointmentsResponse.IsSuccess ||
                patientsResponse == null || !patientsResponse.IsSuccess ||
                doctorsResponse == null || !doctorsResponse.IsSuccess)
            {
                TempData["error"] = "Error al obtener los datos para las citas.";
                return View(new List<AppointmentViewModel>());
            }

            // 3. Deserializar los datos
            var appointmentListDto = JsonConvert.DeserializeObject<List<AppointmentDto>>(Convert.ToString(appointmentsResponse.Result));
            var patientList = JsonConvert.DeserializeObject<List<PatientDto>>(Convert.ToString(patientsResponse.Result));
            var doctorList = JsonConvert.DeserializeObject<List<DoctorDto>>(Convert.ToString(doctorsResponse.Result));

            // 4. Crear el ViewModel
            var appointmentViewModelList = new List<AppointmentViewModel>();

            // 5. Unir los datos (Join)
            foreach (var appointmentDto in appointmentListDto)
            {
                var patient = patientList.FirstOrDefault(p => p.PatientId == appointmentDto.PatientId);
                var doctor = doctorList.FirstOrDefault(d => d.DoctorId == appointmentDto.DoctorId);

                appointmentViewModelList.Add(new AppointmentViewModel
                {
                    AppointmentId = appointmentDto.AppointmentId,
                    AppointmentDate = appointmentDto.AppointmentDate,
                    Description = appointmentDto.Description,
                    PatientName = $"{patient?.Name} {patient?.LastName}",
                    DoctorName = $"{doctor?.Name} {doctor?.LastName}"
                });
            }

            // 6. Pasar el ViewModel a la vista
            return View(appointmentViewModelList);
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
