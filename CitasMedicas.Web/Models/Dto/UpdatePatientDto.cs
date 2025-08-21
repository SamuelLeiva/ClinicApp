namespace CitasMedicas.Web.Models.Dto
{
    public class UpdatePatientDto
    {
        public string? Name { get; set; }
        public string? LastName { get; set; }
        public DateTime? DateOfBirth { get; set; }
        public string? PhoneNumber { get; set; }
        public string? Email { get; set; }
    }
}
