using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using CulinariaShare.Models;
using X.PagedList;
using System.Threading.Tasks;

namespace CulinariaShare.Controllers
{
    /// <summary>
    /// Controlador para gestionar las acciones relacionadas con las categorías en la aplicación CulinariaShare.
    /// </summary>
    [Authorize]
    public class CategoriaController : Controller
    {
        private readonly ContextoDeDatos _contexto;

        /// <summary>
        /// Constructor del controlador CategoriaController.
        /// </summary>
        /// <param name="contexto">Contexto de datos de la aplicación.</param>
        public CategoriaController(ContextoDeDatos contexto)
        {
            _contexto = contexto;
        }

        /// <summary>
        /// Muestra una lista de todas las categorías con paginación.
        /// </summary>
        /// <param name="page">Número de la página actual.</param>
        /// <returns>Vista con la lista de categorías paginada.</returns>
        public async Task<IActionResult> Index(int? page)
        {
            int pageSize = 5; // Número de registros por página
            int pageNumber = (page ?? 1); // Número de página actual, predeterminado 1 si está vacío

            var categorias = await _contexto.Categorias
                .OrderByDescending(c => c.Id)
                .ToPagedListAsync(pageNumber, pageSize);

            return View(categorias);
        }

        /// <summary>
        /// Muestra los detalles de una categoría específica.
        /// </summary>
        /// <param name="id">ID de la categoría.</param>
        /// <returns>Vista con los detalles de la categoría.</returns>
        public async Task<IActionResult> Details(int id)
        {
            var categoria = await _contexto.Categorias.SingleOrDefaultAsync(c => c.Id == id);
            if (categoria == null)
            {
                return NotFound();
            }
            return View(categoria);
        }

        /// <summary>
        /// Muestra el formulario para crear una nueva categoría.
        /// </summary>
        /// <returns>Vista para crear una nueva categoría.</returns>
        public IActionResult Create()
        {
            return View();
        }

        /// <summary>
        /// Acción POST para crear una nueva categoría.
        /// </summary>
        /// <param name="categoria">Datos de la categoría a crear.</param>
        /// <returns>Redirige al índice si se crea correctamente, de lo contrario, muestra la vista de creación.</returns>
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(Categoria categoria)
        {
            if (ModelState.IsValid)
            {
                _contexto.Categorias.Add(categoria);
                await _contexto.SaveChangesAsync();
                return RedirectToAction("Index");
            }
            return View(categoria);
        }

        /// <summary>
        /// Muestra la vista para editar una categoría existente.
        /// </summary>
        /// <param name="id">ID de la categoría a editar.</param>
        /// <returns>Vista para editar la categoría.</returns>
        public async Task<IActionResult> Edit(int id)
        {
            var categoria = await _contexto.Categorias.SingleOrDefaultAsync(c => c.Id == id);
            if (categoria == null)
            {
                return NotFound();
            }
            return View(categoria);
        }

        /// <summary>
        /// Acción POST para editar una categoría existente.
        /// </summary>
        /// <param name="categoria">Datos actualizados de la categoría.</param>
        /// <returns>Redirige al índice si se edita correctamente, de lo contrario, muestra la vista de edición.</returns>
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(Categoria categoria)
        {
            if (ModelState.IsValid)
            {
                var temp = await _contexto.Categorias.SingleOrDefaultAsync(c => c.Id == categoria.Id);
                if (temp == null)
                {
                    return NotFound();
                }
                temp.Nombre = categoria.Nombre;
                _contexto.Categorias.Update(temp);
                await _contexto.SaveChangesAsync();
                return RedirectToAction("Index");
            }
            return View(categoria);
        }

        /// <summary>
        /// Muestra la vista para eliminar una categoría.
        /// </summary>
        /// <param name="id">ID de la categoría a eliminar.</param>
        /// <returns>Vista para confirmar la eliminación de la categoría.</returns>
        public async Task<IActionResult> Delete(int id)
        {
            var categoria = await _contexto.Categorias.SingleOrDefaultAsync(c => c.Id == id);
            if (categoria == null)
            {
                return NotFound();
            }
            return View(categoria);
        }

        /// <summary>
        /// Acción POST para confirmar la eliminación de una categoría.
        /// </summary>
        /// <param name="id">ID de la categoría a eliminar.</param>
        /// <returns>Redirige al índice después de la eliminación.</returns>
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var categoria = await _contexto.Categorias.SingleOrDefaultAsync(c => c.Id == id);
            if (categoria != null)
            {
                _contexto.Categorias.Remove(categoria);
                await _contexto.SaveChangesAsync();
            }
            return RedirectToAction("Index");
        }
    }
}
