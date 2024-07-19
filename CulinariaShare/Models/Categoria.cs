using System.ComponentModel.DataAnnotations;

namespace CulinariaShare.Models
{
    public class Categoria
    {
        /// <summary>
        /// Identificador único de la categoría.
        /// </summary>
        [Key]
        public int Id { get; set; }

        /// <summary>
        /// Nombre de la categoría.
        /// </summary>
        [Required(ErrorMessage = "El nombre de la categoría es requerido")]
        [MaxLength(100, ErrorMessage = "Máximo 100 caracteres")]
        [Display(Name = "Nombre de la Categoría")]
        public string? Nombre { get; set; }
    }
}
