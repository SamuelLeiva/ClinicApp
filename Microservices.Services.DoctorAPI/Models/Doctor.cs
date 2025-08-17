using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Microservices.Services.DoctorAPI.Models
{
    public class Doctor
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int DoctorId { get; set; }

        [Required(ErrorMessage = "El nombre es obligatorio.")]
        [MaxLength(100)]
        public string Name { get; set; }

        [Required(ErrorMessage = "El apellido es obligatorio.")]
        [MaxLength(100)]
        public string LastName { get; set; }

        [Required(ErrorMessage = "El número de teléfono es obligatorio.")]
        [MaxLength(20)]
        [Phone(ErrorMessage = "Formato de teléfono no válido.")]
        public string PhoneNumber { get; set; }
        [Required(ErrorMessage = "El correo electrónico es obligatorio.")]
        [MaxLength(150)]
        [EmailAddress(ErrorMessage = "Formato de correo electrónico no válido.")]
        public string Email { get; set; }

        [ForeignKey("Specialty")]
        public int SpecialtyId { get; set; }
        public Specialty Specialty { get; set; }

    }
}
