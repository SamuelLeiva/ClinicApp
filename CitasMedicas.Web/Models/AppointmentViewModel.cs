namespace CitasMedicas.Web.Models
{
    public class AppointmentViewModel
    {
        public int AppointmentId { get; set; }
        public DateTime AppointmentDate { get; set; }
        public string Description { get; set; }
        public string PatientName { get; set; }
        public string DoctorName { get; set; }
    }
}