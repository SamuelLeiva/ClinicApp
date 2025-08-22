using CitasMedicas.Web.Services;
using CitasMedicas.Web.Services.IServices;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddControllersWithViews();

builder.Services.AddHttpClient("GatewayAPI", client =>
{
    client.BaseAddress = new Uri(builder.Configuration["ServiceUrls:GatewayAPI"]!);
});

// Servicio base para manejar respuestas
builder.Services.AddScoped<IBaseService, BaseService>();

// Registo de servicios
builder.Services.AddScoped<IPatientService, PatientService>();
builder.Services.AddScoped<IDoctorService, DoctorService>();
builder.Services.AddScoped<ISpecialtyService, SpecialtyService>();
builder.Services.AddScoped<IAppointmentService, AppointmentService>();

// Registro del HttpClient para los servicios
builder.Services.AddHttpClient<IAppointmentService, AppointmentService>();
builder.Services.AddHttpClient<IPatientService, PatientService>();
builder.Services.AddHttpClient<IDoctorService, DoctorService>();
builder.Services.AddHttpClient<ISpecialtyService, SpecialtyService>();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Home/Error");
}
app.UseRouting();

app.UseAuthentication();
app.UseAuthorization();

app.MapStaticAssets();

app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Home}/{action=Index}/{id?}")
    .WithStaticAssets();


app.Run();
