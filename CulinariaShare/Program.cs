using CulinariaShare.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Authentication.Cookies;

var builder = WebApplication.CreateBuilder(args);

// Añadir servicios al contenedor de dependencias.
builder.Services.AddControllersWithViews();

// Configuración de la inyección de dependencias del contexto de datos
builder.Services.AddDbContext<ContextoDeDatos>(opciones =>
    opciones.UseSqlServer(builder.Configuration.GetConnectionString("conexion")));

// Configuración de la autenticación basada en cookies
builder.Services.AddAuthentication(CookieAuthenticationDefaults.AuthenticationScheme)
    .AddCookie(options =>
    {
        options.LoginPath = "/Usuario/Login"; // Ruta de la página de inicio de sesión
        options.AccessDeniedPath = "/Usuario/Login"; // Ruta de la página de acceso denegado
        options.ExpireTimeSpan = TimeSpan.FromHours(8); // Tiempo de expiración de la cookie
        options.SlidingExpiration = true; // Renueva la cookie en cada solicitud
        options.Cookie.HttpOnly = true; // La cookie no es accesible mediante scripts del lado del cliente
    });

var app = builder.Build();

// Configuración del pipeline de solicitudes HTTP.
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Home/Error"); // Usar controlador de errores en producción
    app.UseHsts(); // Aplicar estricta política de transporte HTTP
}

app.UseHttpsRedirection(); // Redirigir solicitudes HTTP a HTTPS
app.UseStaticFiles(); // Servir archivos estáticos

app.UseRouting(); // Habilitar enrutamiento

// Configuración del middleware de autenticación y autorización
app.UseAuthentication(); // Habilitar autenticación
app.UseAuthorization(); // Habilitar autorización

// Configuración de las rutas de los controladores
app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Home}/{action=Index}/{id?}"); // Ruta predeterminada

app.Run(); // Ejecutar la aplicación
