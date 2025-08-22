namespace CitasMedicas.Web.Utility
{
    public class SD
    {
        public static string PatientAPIBase { get; set; }
        public static string AuthAPIBase { get; set; }
        public static string AppointmentAPIBase { get; set; }
        public static string DoctorAPIBase { get; set; }
        public static string SpecialtyAPIBase { get; set; }

        public const string RoleAdmin = "ADMIN";
        public const string RolePatient = "PATIENT";

        public const string TokenCookie = "JwtToken";
        public enum ApiType
        {
            GET,
            POST,
            PUT,
            DELETE
        }

        public enum ContentType
        {
            Json,
            MultiPartFormData
        }
    }
}
