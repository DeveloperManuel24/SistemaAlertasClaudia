using FluentValidation;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using SistemaAlertasBackEnd;
using SistemaAlertasBackEnd.EndPoints;
using SistemaAlertasBackEnd.Repositorios;
using SistemaAlertasBackEnd.Servicios;
using SistemaAlertasBackEnd.Utilidades;

var builder = WebApplication.CreateBuilder(args);

// Configurar servicios en el contenedor de dependencias
builder.Services.AddControllers();

// Configuraci�n de Entity Framework y base de datos
builder.Services.AddDbContext<ApplicationDbContext>(opciones =>
    opciones.UseSqlServer("name=DefaultConnection"));

// Configuraci�n de Identity
builder.Services.AddIdentityCore<IdentityUser>()
     .AddRoles<IdentityRole>() // Agrega el manejo de roles
     .AddEntityFrameworkStores<ApplicationDbContext>()
     .AddDefaultTokenProviders();

builder.Services.AddScoped<UserManager<IdentityUser>>();
builder.Services.AddScoped<SignInManager<IdentityUser>>();

// Configuraci�n de CORS
var origenesPermitidos = builder.Configuration.GetValue<string>("origenespermitidos")!;
builder.Services.AddCors(opciones =>
{
    opciones.AddDefaultPolicy(configuracion =>
    {
        configuracion.WithOrigins(origenesPermitidos).AllowAnyHeader().AllowAnyMethod();
    });

    opciones.AddPolicy("libre", configuracion =>
    {
        configuracion.AllowAnyOrigin().AllowAnyHeader().AllowAnyMethod();
    });
});

// Configuraci�n de Autenticaci�n y JWT
builder.Services.AddAuthentication().AddJwtBearer(opciones =>
{
    opciones.MapInboundClaims = false;
    opciones.TokenValidationParameters = new TokenValidationParameters
    {
        ValidateIssuer = false,
        ValidateAudience = false,
        ValidateLifetime = true,
        ValidateIssuerSigningKey = true,
        IssuerSigningKey = Llaves.ObtenerLlave(builder.Configuration).First(),
        ClockSkew = TimeSpan.Zero
    };
});

// Creaci�n de Roles y Pol�ticas de Autorizaci�n
builder.Services.AddAuthorization(opciones =>
{
    opciones.AddPolicy("esadmin", politica => politica.RequireClaim("esadmin"));
});

// Configuraci�n de Output Cache
builder.Services.AddOutputCache();

// Configuraci�n de Swagger para la documentaci�n de la API
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

// Inyecci�n de dependencias para servicios personalizados
builder.Services.AddTransient<IServicioUsuarios, ServicioUsuarios>();
builder.Services.AddTransient<ServicioEmail>();

builder.Services.AddScoped<IRepositorioSensor, RepositorioSensor>();
builder.Services.AddScoped<IRepositorioLectura, RepositorioLectura>();
builder.Services.AddScoped<IRepositorioAlerta, RepositorioAlerta>();

// Otros servicios
builder.Services.AddHttpContextAccessor();  // Necesario para obtener el usuario autenticado
builder.Services.AddAutoMapper(typeof(Program));
builder.Services.AddProblemDetails();

// Registro de los validadores de FluentValidation
builder.Services.AddValidatorsFromAssemblyContaining<Program>();

var app = builder.Build();

// Configuraci�n del pipeline HTTP
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();
app.UseCors();
app.UseAuthentication();
app.UseAuthorization();

// Configuraci�n de los EndPoints
app.MapGroup("/usuarios").MapUsuarios();
app.MapGroup("/lecturas").MapLecturas();
app.MapGroup("/api").MapSensores();
app.MapGroup("/alertas").MapAlertas();

app.Run();
