using Microservices.Services.AppointmentAPI.Models;
using Microsoft.EntityFrameworkCore;

namespace Microservices.Services.AppointmentAPI.Data
{
    public class ApplicationDbContext : DbContext
    {
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options) : base(options)
        {
        }
        public DbSet<Appointment> Appointments { get; set; }
    }
}
