using Microsoft.AspNetCore.Mvc;
using CulinariaShare.Models;
using Microsoft.EntityFrameworkCore;
using System.Threading.Tasks;

namespace CulinariaShare.Controllers
{
    /// <summary>
    /// Controlador para gestionar las acciones relacionadas con los roles en la aplicación CulinariaShare.
    /// </summary>
    public class RoleController : Controller
    {
        private readonly ContextoDeDatos _context;

        /// <summary>
        /// Constructor del controlador RoleController.
        /// </summary>
        /// <param name="context">Contexto de datos de la aplicación.</param>
        public RoleController(ContextoDeDatos context)
        {
            _context = context;
        }

        /// <summary>
        /// Muestra una lista de todos los roles.
        /// </summary>
        /// <returns>Vista con la lista de roles.</returns>
        public async Task<IActionResult> Index()
        {
            var roles = await _context.Roles.ToListAsync();
            return View(roles);
        }

        /// <summary>
        /// Muestra los detalles de un rol específico.
        /// </summary>
        /// <param name="id">ID del rol.</param>
        /// <returns>Vista con los detalles del rol.</returns>
        public async Task<IActionResult> Details(int id)
        {
            var role = await _context.Roles.FindAsync(id);
            if (role == null)
            {
                return NotFound();
            }
            return View(role);
        }

        /// <summary>
        /// Muestra la vista para crear un nuevo rol.
        /// </summary>
        /// <returns>Vista para crear un nuevo rol.</returns>
        public IActionResult Create()
        {
            return View();
        }

        /// <summary>
        /// Acción POST para crear un nuevo rol.
        /// </summary>
        /// <param name="role">Datos del rol a crear.</param>
        /// <returns>Redirige al índice si se crea correctamente, de lo contrario, muestra la vista de creación.</returns>
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(Role role)
        {
            try
            {
                if (ModelState.IsValid)
                {
                    _context.Add(role);
                    await _context.SaveChangesAsync();
                    return RedirectToAction(nameof(Index));
                }
            }
            catch
            {
                // Manejar error (registrar, etc.)
            }
            return View(role);
        }

        /// <summary>
        /// Muestra la vista para editar un rol existente.
        /// </summary>
        /// <param name="id">ID del rol a editar.</param>
        /// <returns>Vista para editar el rol.</returns>
        public async Task<IActionResult> Edit(int id)
        {
            var role = await _context.Roles.FindAsync(id);
            if (role == null)
            {
                return NotFound();
            }
            return View(role);
        }

        /// <summary>
        /// Acción POST para editar un rol existente.
        /// </summary>
        /// <param name="id">ID del rol a editar.</param>
        /// <param name="role">Datos actualizados del rol.</param>
        /// <returns>Redirige al índice si se edita correctamente, de lo contrario, muestra la vista de edición.</returns>
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, Role role)
        {
            if (id != role.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(role);
                    await _context.SaveChangesAsync();
                    return RedirectToAction(nameof(Index));
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!RoleExists(role.Id))
                    {
                        return NotFound();
                    }
                    else
                    {
                        throw;
                    }
                }
            }
            return View(role);
        }

        /// <summary>
        /// Muestra la vista para eliminar un rol.
        /// </summary>
        /// <param name="id">ID del rol a eliminar.</param>
        /// <returns>Vista para confirmar la eliminación del rol.</returns>
        public async Task<IActionResult> Delete(int id)
        {
            var role = await _context.Roles.FindAsync(id);
            if (role == null)
            {
                return NotFound();
            }
            return View(role);
        }

        /// <summary>
        /// Acción POST para confirmar la eliminación de un rol.
        /// </summary>
        /// <param name="id">ID del rol a eliminar.</param>
        /// <returns>Redirige al índice después de la eliminación.</returns>
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var role = await _context.Roles.FindAsync(id);
            _context.Roles.Remove(role);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        /// <summary>
        /// Verifica si un rol con el ID especificado existe en la base de datos.
        /// </summary>
        /// <param name="id">ID del rol.</param>
        /// <returns>True si el rol existe, false en caso contrario.</returns>
        private bool RoleExists(int id)
        {
            return _context.Roles.Any(e => e.Id == id);
        }
    }
}
