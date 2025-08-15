using Microservices.Services.PatientAPI.Models;
using Microsoft.EntityFrameworkCore;

namespace Microservices.Services.PatientAPI.Data
{
    public class ApplicationDbContext : DbContext
    {
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options) : base(options)
        {
        }
        public DbSet<Patient> Patients { get; set; }
    }
}
