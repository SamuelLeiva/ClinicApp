using System.Text;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.IdentityModel.Tokens;
using Ocelot.DependencyInjection;
using Ocelot.Middleware;
using Ocelot.Provider.Consul;
using Ocelot.Provider.Polly;

var builder = WebApplication.CreateBuilder(args);

builder.Configuration.AddJsonFile("ocelot.json", optional: false, reloadOnChange: true);

// JWT Bearer Configuration
var secret = builder.Configuration.GetValue<string>("ApiSettings:Secret");
var key = Encoding.ASCII.GetBytes(secret!);

builder.Services.AddAuthentication(x =>
{
    x.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
    x.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
}).AddJwtBearer(x =>
{
    x.TokenValidationParameters = new TokenValidationParameters
    {
        ValidateIssuerSigningKey = true,
        IssuerSigningKey = new SymmetricSecurityKey(key),
        ValidateIssuer = false,
        ValidateAudience = false
    };
});

// Add Ocelot with Consul and Polly
builder.Services.AddOcelot().AddConsul().AddPolly();

var app = builder.Build();

app.UseHttpsRedirection();

// ...
app.UseAuthentication();
app.UseAuthorization();
await app.UseOcelot();

app.Run();