using Microsoft.EntityFrameworkCore;

namespace CulinariaShare.Models
{
    /// <summary>
    /// Contexto de datos de la aplicación CulinariaShare.
    /// </summary>
    public class ContextoDeDatos : DbContext
    {
        /// <summary>
        /// Constructor de la clase ContextoDeDatos.
        /// </summary>
        /// <param name="opciones">Opciones de configuración del contexto de datos.</param>
        public ContextoDeDatos(DbContextOptions opciones) : base(opciones)
        {
        }

        /// <summary>
        /// Conjunto de entidades de usuarios.
        /// </summary>
        public DbSet<Usuario> Usuarios { get; set; }

        /// <summary>
        /// Conjunto de entidades de roles.
        /// </summary>
        public DbSet<Role> Roles { get; set; }

        /// <summary>
        /// Conjunto de entidades de recetas.
        /// </summary>
        public DbSet<Receta> Recetas { get; set; }

        /// <summary>
        /// Conjunto de entidades de categorías.
        /// </summary>
        public DbSet<Categoria> Categorias { get; set; }
    }
}
