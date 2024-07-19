using System.ComponentModel.DataAnnotations;

namespace CulinariaShare.Models
{
    /// <summary>
    /// Representa un rol en la aplicación CulinariaShare.
    /// </summary>
    public class Role
    {
        /// <summary>
        /// Identificador único del rol.
        /// </summary>
        [Key]
        public int Id { get; set; }

        /// <summary>
        /// Nombre del rol.
        /// </summary>
        [Required(ErrorMessage = "El nombre del rol es requerido")]
        [MaxLength(50, ErrorMessage = "Máximo 50 caracteres")]
        [Display(Name = "Nombre del Rol")]
        public string? Name { get; set; }
    }
}
