using CitasMedicas.Web.Services;
using CitasMedicas.Web.Services.IServices;
using Microsoft.AspNetCore.Authentication.Cookies;

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

// Configura autenticación con cookie
builder.Services.AddAuthentication(CookieAuthenticationDefaults.AuthenticationScheme)
    .AddCookie(options =>
    {
        options.ExpireTimeSpan = TimeSpan.FromHours(10);
        options.LoginPath = "/Auth/Login";
        options.AccessDeniedPath = "/Auth/AccessDenied";
    });

builder.Services.AddHttpContextAccessor();
builder.Services.AddScoped<IAuthService, AuthService>();
builder.Services.AddScoped<ITokenProvider, TokenProvider>();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Home/Error");
    app.UseHsts();
}

app.UseHttpsRedirection();
app.UseStaticFiles();
app.UseRouting();

app.UseAuthentication();
app.UseAuthorization();

app.MapStaticAssets();

app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Home}/{action=Index}/{id?}")
    .WithStaticAssets();


app.Run();
