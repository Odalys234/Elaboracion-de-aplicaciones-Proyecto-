using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace CulinariaShare.Models
{
    /// <summary>
    /// Representa una receta en la aplicación CulinariaShare.
    /// </summary>
    public class Receta
    {
        /// <summary>
        /// Identificador único de la receta.
        /// </summary>
        [Key]
        public int Id { get; set; }

        /// <summary>
        /// Título de la receta.
        /// </summary>
        [Required(ErrorMessage = "El título es requerido")]
        [MaxLength(255, ErrorMessage = "Máximo 255 caracteres")]
        [Display(Name = "Título de la Receta")]
        public string? Titulo { get; set; }

        /// <summary>
        /// Descripción de la receta.
        /// </summary>
        [Required(ErrorMessage = "La descripción es requerida")]
        [Display(Name = "Descripción")]
        public string? Descripcion { get; set; }

        /// <summary>
        /// Ingredientes necesarios para la receta.
        /// </summary>
        [Required(ErrorMessage = "Los ingredientes son requeridos")]
        [Display(Name = "Ingredientes")]
        public string? Ingredientes { get; set; }

        /// <summary>
        /// Instrucciones para preparar la receta.
        /// </summary>
        [Required(ErrorMessage = "Las instrucciones son requeridas")]
        [Display(Name = "Instrucciones")]
        public string? Instrucciones { get; set; }

        /// <summary>
        /// Tiempo de preparación en minutos.
        /// </summary>
        [Required(ErrorMessage = "El tiempo de preparación es requerido")]
        [Display(Name = "Tiempo de Preparación (minutos)")]
        public int TiempoPreparacion { get; set; }

        /// <summary>
        /// Número de porciones que se obtienen con la receta.
        /// </summary>
        [Required(ErrorMessage = "El número de porciones es requerido")]
        [Display(Name = "Número de Porciones")]
        public int NumeroPorciones { get; set; }

        /// <summary>
        /// Identificador de la categoría a la que pertenece la receta.
        /// </summary>
        [ForeignKey("Categoria")]
        [Required(ErrorMessage = "La categoría es requerida")]
        [Display(Name = "Categoría")]
        public int CategoriaId { get; set; }

        /// <summary>
        /// Ruta de la foto de la receta.
        /// </summary>
        [Display(Name = "Foto")]
        public string? Foto { get; set; }

        /// <summary>
        /// Identificador del usuario que creó la receta.
        /// </summary>
        [ForeignKey("Usuario")]
        [Required(ErrorMessage = "El ID del usuario es requerido")]
        [Display(Name = "ID del Usuario")]
        public int UserId { get; set; }

        /// <summary>
        /// Propiedad de navegación para el usuario.
        /// </summary>
        public Usuario? Usuario { get; set; }

        /// <summary>
        /// Propiedad de navegación para la categoría.
        /// </summary>
        public Categoria? Categoria { get; set; }
    }
}
