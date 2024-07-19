using CulinariaShare.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Authentication.Cookies;

var builder = WebApplication.CreateBuilder(args);

// A�adir servicios al contenedor de dependencias.
builder.Services.AddControllersWithViews();

// Configuraci�n de la inyecci�n de dependencias del contexto de datos
builder.Services.AddDbContext<ContextoDeDatos>(opciones =>
    opciones.UseSqlServer(builder.Configuration.GetConnectionString("conexion")));

// Configuraci�n de la autenticaci�n basada en cookies
builder.Services.AddAuthentication(CookieAuthenticationDefaults.AuthenticationScheme)
    .AddCookie(options =>
    {
        options.LoginPath = "/Usuario/Login"; // Ruta de la p�gina de inicio de sesi�n
        options.AccessDeniedPath = "/Usuario/Login"; // Ruta de la p�gina de acceso denegado
        options.ExpireTimeSpan = TimeSpan.FromHours(8); // Tiempo de expiraci�n de la cookie
        options.SlidingExpiration = true; // Renueva la cookie en cada solicitud
        options.Cookie.HttpOnly = true; // La cookie no es accesible mediante scripts del lado del cliente
    });

var app = builder.Build();

// Configuraci�n del pipeline de solicitudes HTTP.
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Home/Error"); // Usar controlador de errores en producci�n
    app.UseHsts(); // Aplicar estricta pol�tica de transporte HTTP
}

app.UseHttpsRedirection(); // Redirigir solicitudes HTTP a HTTPS
app.UseStaticFiles(); // Servir archivos est�ticos

app.UseRouting(); // Habilitar enrutamiento

// Configuraci�n del middleware de autenticaci�n y autorizaci�n
app.UseAuthentication(); // Habilitar autenticaci�n
app.UseAuthorization(); // Habilitar autorizaci�n

// Configuraci�n de las rutas de los controladores
app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Home}/{action=Index}/{id?}"); // Ruta predeterminada

app.Run(); // Ejecutar la aplicaci�n
