namespace CitasMedicas.Web.Models.Doctor
{
    public class CreateDoctorDto
    {
        public int DoctorId { get; set; }
        public string Name { get; set; }
        public string LastName { get; set; }
        public string PhoneNumber { get; set; }
        public string Email { get; set; }
        public int SpecialtyId { get; set; }
    }
}
