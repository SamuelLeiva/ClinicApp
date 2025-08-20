using System.Collections.Specialized;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Microservices.Services.AppointmentAPI.Models
{
    public class Appointment
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int AppointmentId { get; set; }

        [Required(ErrorMessage ="PatientId is required")]
        public int PatientId { get; set; }

        [Required(ErrorMessage = "DoctorId is required")]
        public int DoctorId { get; set; }

        [Required(ErrorMessage = "AppointmentDate is required")]
        public DateTime AppointmentDate { get; set; }

        [MaxLength(500, ErrorMessage = "Description cannot exceed 500 characters")]
        public string Description { get; set; }
    }
}
