using CulinariaShare.Models;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Security.Claims;
using System.Threading.Tasks;
using X.PagedList;

namespace CulinariaShare.Controllers
{
    /// <summary>
    /// Controlador para gestionar las acciones relacionadas con los usuarios en la aplicación CulinariaShare.
    /// </summary>
    public class UsuarioController : Controller
    {
        private readonly ContextoDeDatos _context;

        /// <summary>
        /// Constructor del controlador UsuarioController.
        /// </summary>
        /// <param name="context">Contexto de datos de la aplicación.</param>
        public UsuarioController(ContextoDeDatos context)
        {
            _context = context;
        }

        /// <summary>
        /// Muestra una lista paginada de usuarios.
        /// </summary>
        /// <param name="page">Número de la página actual.</param>
        /// <returns>Vista con la lista de usuarios paginada.</returns>
        public async Task<IActionResult> Index(int? page)
        {
            int pageSize = 5; // Número de registros por página
            int pageNumber = (page ?? 1); // Número de página actual, por defecto 1 si es null

            var usuarios = await _context.Usuarios.ToPagedListAsync(pageNumber, pageSize);
            return View(usuarios);
        }

        /// <summary>
        /// Muestra los detalles de un usuario específico.
        /// </summary>
        /// <param name="id">ID del usuario.</param>
        /// <returns>Vista con los detalles del usuario.</returns>
        public IActionResult Details(int id)
        {
            var usuario = _context.Usuarios.Find(id);
            if (usuario == null)
            {
                return NotFound();
            }
            return View(usuario);
        }

        /// <summary>
        /// Muestra la vista para crear un nuevo usuario.
        /// </summary>
        /// <returns>Vista para crear un nuevo usuario.</returns>
        public IActionResult Create()
        {
            return View();
        }

        /// <summary>
        /// Acción POST para crear un nuevo usuario.
        /// </summary>
        /// <param name="usuario">Datos del usuario a crear.</param>
        /// <returns>Redirige al índice si se crea correctamente, de lo contrario, muestra la vista de creación.</returns>
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(Usuario usuario)
        {
            if (ModelState.IsValid)
            {
                _context.Add(usuario);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            return View(usuario);
        }

        /// <summary>
        /// Muestra la vista para editar un usuario existente.
        /// </summary>
        /// <param name="id">ID del usuario a editar.</param>
        /// <returns>Vista para editar el usuario.</returns>
        public IActionResult Edit(int id)
        {
            var usuario = _context.Usuarios.Find(id);
            if (usuario == null)
            {
                return NotFound();
            }
            return View(usuario);
        }

        /// <summary>
        /// Acción POST para editar un usuario existente.
        /// </summary>
        /// <param name="id">ID del usuario a editar.</param>
        /// <param name="usuario">Datos actualizados del usuario.</param>
        /// <returns>Redirige al índice si se edita correctamente, de lo contrario, muestra la vista de edición.</returns>
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, Usuario usuario)
        {
            if (id != usuario.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(usuario);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!UsuarioExists(usuario.Id))
                    {
                        return NotFound();
                    }
                    else
                    {
                        throw;
                    }
                }
                return RedirectToAction(nameof(Index));
            }
            return View(usuario);
        }

        /// <summary>
        /// Muestra la vista para eliminar un usuario.
        /// </summary>
        /// <param name="id">ID del usuario a eliminar.</param>
        /// <returns>Vista para confirmar la eliminación del usuario.</returns>
        public IActionResult Delete(int id)
        {
            var usuario = _context.Usuarios.Find(id);
            if (usuario == null)
            {
                return NotFound();
            }
            return View(usuario);
        }

        /// <summary>
        /// Acción POST para confirmar la eliminación de un usuario.
        /// </summary>
        /// <param name="id">ID del usuario a eliminar.</param>
        /// <returns>Redirige al índice después de la eliminación.</returns>
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var usuario = await _context.Usuarios.FindAsync(id);
            _context.Usuarios.Remove(usuario);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        /// <summary>
        /// Muestra la vista de inicio de sesión.
        /// </summary>
        /// <param name="returnUrl">URL a la que se redirigirá después de iniciar sesión.</param>
        /// <returns>Vista de inicio de sesión.</returns>
        public IActionResult Login(string returnUrl)
        {
            ViewBag.Url = returnUrl;
            ViewBag.Error = "";
            return View();
        }

        /// <summary>
        /// Acción POST para iniciar sesión.
        /// </summary>
        /// <param name="pUser">Datos del usuario que intenta iniciar sesión.</param>
        /// <param name="pReturnURL">URL a la que se redirigirá después de iniciar sesión.</param>
        /// <returns>Redirige a la URL especificada o al índice del Home si el inicio de sesión es exitoso, de lo contrario, muestra la vista de inicio de sesión con error.</returns>
        [HttpPost]
        public async Task<IActionResult> Login(Usuario pUser, string pReturnURL = null)
        {
            try
            {
                var user = await _context.Usuarios.FirstOrDefaultAsync(u => u.Email == pUser.Email && u.Password == pUser.Password);
                if (user != null && user.Id > 0 && pUser.Email == user.Email)
                {
                    var role = await _context.Roles.FindAsync(user.RoleID);

                    var claims = new[] { new Claim(ClaimTypes.Name, user.Email), new Claim(ClaimTypes.Role, role.Name) };
                    var identity = new ClaimsIdentity(claims, CookieAuthenticationDefaults.AuthenticationScheme);
                    await HttpContext.SignInAsync(CookieAuthenticationDefaults.AuthenticationScheme, new ClaimsPrincipal(identity));
                }
                else
                {
                    throw new Exception("Credenciales incorrectas");
                }
                if (!string.IsNullOrWhiteSpace(pReturnURL))
                {
                    return Redirect(pReturnURL);
                }
                else
                {
                    return RedirectToAction("Index", "Home");
                }
            }
            catch (Exception ex)
            {
                ViewBag.Url = pReturnURL;
                ViewBag.Error = ex.Message;
                return View(new Usuario { Email = pUser.Email });
            }
        }

        /// <summary>
        /// Cierra la sesión del usuario actual.
        /// </summary>
        /// <returns>Redirige a la vista de inicio de sesión.</returns>
        public async Task<IActionResult> Logout()
        {
            await HttpContext.SignOutAsync(CookieAuthenticationDefaults.AuthenticationScheme);
            return RedirectToAction("Login", "Usuario");
        }

        /// <summary>
        /// Muestra la vista de registro de usuario.
        /// </summary>
        /// <param name="returnUrl">URL a la que se redirigirá después del registro.</param>
        /// <returns>Vista de registro de usuario.</returns>
        public IActionResult Register(string returnUrl)
        {
            ViewBag.Url = returnUrl;
            ViewBag.Error = "";
            return View();
        }

        /// <summary>
        /// Acción POST para registrar un nuevo usuario.
        /// </summary>
        /// <param name="usuario">Datos del usuario a registrar.</param>
        /// <param name="pReturnURL">URL a la que se redirigirá después del registro.</param>
        /// <returns>Redirige al login después del registro, de lo contrario, muestra la vista de registro con error.</returns>
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Register(Usuario usuario, string pReturnURL = null)
        {
            try
            {
                if (ModelState.IsValid)
                {
                    // Verificar si el correo electrónico ya está registrado
                    var existingUser = await _context.Usuarios.FirstOrDefaultAsync(u => u.Email == usuario.Email);
                    if (existingUser != null)
                    {
                        throw new Exception("Ya existe un usuario con ese correo electrónico.");
                    }

                    // Asignar el rol de usuario por defecto
                    var userRole = await _context.Roles.FirstOrDefaultAsync(r => r.Name == "Usuario");
                    if (userRole != null)
                    {
                        usuario.RoleID = userRole.Id;
                    }
                    else
                    {
                        throw new Exception("Rol de usuario no encontrado.");
                    }

                    _context.Add(usuario);
                    await _context.SaveChangesAsync();

                    // Redirigir al login después del registro
                    return RedirectToAction("Login", "Usuario");
                }
                return View(usuario);
            }
            catch (Exception ex)
            {
                ViewBag.Error = ex.Message;
                return View(usuario);
            }
        }

        /// <summary>
        /// Verifica si un usuario con el ID especificado existe en la base de datos.
        /// </summary>
        /// <param name="id">ID del usuario.</param>
        /// <returns>True si el usuario existe, false en caso contrario.</returns>
        private bool UsuarioExists(int id)
        {
            return _context.Usuarios.Any(e => e.Id == id);
        }
    }
}
