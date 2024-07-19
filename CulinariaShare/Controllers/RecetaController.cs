using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using CulinariaShare.Models;
using X.PagedList;
using System.Threading.Tasks;
using System.IO;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace CulinariaShare.Controllers
{
    /// <summary>
    /// Controlador para gestionar las acciones relacionadas con las recetas en la aplicación CulinariaShare.
    /// </summary>
    [Authorize]
    public class RecetaController : Controller
    {
        private readonly ContextoDeDatos _contexto;

        /// <summary>
        /// Constructor del controlador RecetaController.
        /// </summary>
        /// <param name="contexto">Contexto de datos de la aplicación.</param>
        public RecetaController(ContextoDeDatos contexto)
        {
            _contexto = contexto;
        }

        /// <summary>
        /// Muestra una lista de todas las recetas con paginación.
        /// </summary>
        /// <param name="page">Número de la página actual.</param>
        /// <returns>Vista con la lista de recetas paginada.</returns>
        public async Task<IActionResult> Index(int? page, string searchString, int? categoryId)
        {
            int pageSize = 3; // Número de registros por página
            int pageNumber = (page ?? 1); // Número de página actual, predeterminado 1 si está vacío

            var recetas = _contexto.Recetas
                .Include(r => r.Categoria)
                .Include(r => r.Usuario)
                .AsQueryable();

            // Filtrar por categoría si está seleccionada
            if (categoryId.HasValue && categoryId > 0)
            {
                recetas = recetas.Where(r => r.CategoriaId == categoryId);
            }

            // Buscar por título si hay una cadena de búsqueda
            if (!string.IsNullOrEmpty(searchString))
            {
                recetas = recetas.Where(r => r.Titulo.Contains(searchString));
            }

            var recetasPaginadas = await recetas.OrderByDescending(r => r.Id).ToPagedListAsync(pageNumber, pageSize);

            ViewBag.Categorias = new SelectList(_contexto.Categorias, "Id", "Nombre");
            ViewBag.SearchString = searchString;
            ViewBag.CategoryId = categoryId;

            return View(recetasPaginadas);
        }


        /// <summary>
        /// Muestra el formulario para crear una nueva receta.
        /// </summary>
        /// <returns>Vista para crear una nueva receta.</returns>
        public async Task<IActionResult> Create()
        {
            var categorias = await _contexto.Categorias.ToListAsync();
            var usuarios = await _contexto.Usuarios.ToListAsync();
            ViewBag.Categorias = categorias;
            ViewBag.Usuarios = usuarios;
            return View();
        }

        /// <summary>
        /// Acción POST para crear una nueva receta.
        /// </summary>
        /// <param name="receta">Datos de la receta a crear.</param>
        /// <param name="foto">Foto de la receta.</param>
        /// <returns>Redirige al índice si se crea correctamente, de lo contrario, muestra la vista de creación.</returns>
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(Receta receta, IFormFile foto)
        {
            if (ModelState.IsValid)
            {
                if (foto != null && foto.Length > 0)
                {
                    var uploads = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot\\images");
                    var filePath = Path.Combine(uploads, foto.FileName);
                    using (var stream = new FileStream(filePath, FileMode.Create))
                    {
                        await foto.CopyToAsync(stream);
                    }
                    receta.Foto = "/images/" + foto.FileName;
                }

                _contexto.Recetas.Add(receta);
                await _contexto.SaveChangesAsync();
                return RedirectToAction("Index");
            }
            var categorias = await _contexto.Categorias.ToListAsync();
            var usuarios = await _contexto.Usuarios.ToListAsync();
            ViewBag.Categorias = categorias;
            ViewBag.Usuarios = usuarios;
            return View(receta);
        }

        /// <summary>
        /// Muestra los detalles de una receta específica.
        /// </summary>
        /// <param name="id">ID de la receta.</param>
        /// <returns>Vista con los detalles de la receta.</returns>
        public async Task<IActionResult> Details(int id)
        {
            var receta = await _contexto.Recetas
                .Include(r => r.Categoria)
                .Include(r => r.Usuario)
                .SingleOrDefaultAsync(r => r.Id == id);

            if (receta == null)
            {
                return NotFound();
            }

            return View(receta);
        }

        /// <summary>
        /// Muestra la vista para editar una receta existente.
        /// </summary>
        /// <param name="id">ID de la receta a editar.</param>
        /// <returns>Vista para editar la receta.</returns>
        public async Task<IActionResult> Edit(int id)
        {
            var receta = await _contexto.Recetas.Include(r => r.Categoria).Include(r => r.Usuario).FirstOrDefaultAsync(r => r.Id == id);
            if (receta == null)
            {
                return NotFound();
            }

            ViewBag.Categorias = await _contexto.Categorias.ToListAsync();
            ViewBag.Usuarios = await _contexto.Usuarios.ToListAsync();
            return View(receta);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, Receta receta, IFormFile foto)
        {
            if (id != receta.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    if (foto != null && foto.Length > 0)
                    {
                        var uploads = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot/images");
                        if (!Directory.Exists(uploads))
                        {
                            Directory.CreateDirectory(uploads);
                        }

                        var filePath = Path.Combine(uploads, foto.FileName);
                        using (var stream = new FileStream(filePath, FileMode.Create))
                        {
                            await foto.CopyToAsync(stream);
                        }
                        receta.Foto = "/images/" + foto.FileName;
                    }
                    else
                    {
                       
                        _contexto.Entry(receta).Property(x => x.Foto).IsModified = false;
                    }

                    _contexto.Update(receta);
                    await _contexto.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!RecetaExists(receta.Id))
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

            ViewBag.Categorias = await _contexto.Categorias.ToListAsync();
            ViewBag.Usuarios = await _contexto.Usuarios.ToListAsync();
            return View(receta);
        }

        private bool RecetaExists(int id)
        {
            return _contexto.Recetas.Any(e => e.Id == id);
        }
        /// <summary>
        /// Muestra la vista para eliminar una receta.
        /// </summary>
        /// <param name="id">ID de la receta a eliminar.</param>
        /// <returns>Vista para confirmar la eliminación de la receta.</returns>
        public async Task<IActionResult> Delete(int id)
        {
            var receta = await _contexto.Recetas
                .Include(r => r.Categoria)
                .Include(r => r.Usuario)
                .SingleOrDefaultAsync(r => r.Id == id);

            if (receta == null)
            {
                return NotFound();
            }

            return View(receta);
        }

        /// <summary>
        /// Acción POST para confirmar la eliminación de una receta.
        /// </summary>
        /// <param name="id">ID de la receta a eliminar.</param>
        /// <param name="receta">Datos de la receta a eliminar.</param>
        /// <returns>Redirige al índice después de la eliminación.</returns>
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var receta = await _contexto.Recetas.SingleOrDefaultAsync(r => r.Id == id);
            if (receta != null)
            {
                _contexto.Recetas.Remove(receta);
                await _contexto.SaveChangesAsync();
                return RedirectToAction("Index");
            }
            return View(receta);
        }
    }
}
