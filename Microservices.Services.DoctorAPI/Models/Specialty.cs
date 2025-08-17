using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Microservices.Services.DoctorAPI.Models
{
    public class Specialty
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int SpecialtyId { get; set; }

        [Required(ErrorMessage = "El nombre de la especialidad es obligatorio.")]
        [MaxLength(50)]
        public string Name { get; set; }

        public ICollection<Doctor> Doctors { get; set; }
    }
}
