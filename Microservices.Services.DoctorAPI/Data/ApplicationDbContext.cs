using Microsoft.EntityFrameworkCore;

namespace Microservices.Services.DoctorAPI.Data
{
    public class ApplicationDbContext : DbContext
    {
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options) : base(options)
        {
        }
        public DbSet<Models.Doctor> Doctors { get; set; }
        public DbSet<Models.Specialty> Specialties { get; set; }
    }
}
