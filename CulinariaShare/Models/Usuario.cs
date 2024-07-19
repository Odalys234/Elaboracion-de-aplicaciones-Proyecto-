using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace CulinariaShare.Models
{
    /// <summary>
    /// Representa un usuario en la aplicación CulinariaShare.
    /// </summary>
    public class Usuario
    {
        /// <summary>
        /// Identificador único del usuario.
        /// </summary>
        [Key]
        public int Id { get; set; }

        /// <summary>
        /// Nombre de usuario.
        /// </summary>
        [Required(ErrorMessage = "El nombre de usuario es requerido")]
        [MaxLength(50, ErrorMessage = "Máximo 50 caracteres")]
        [Display(Name = "Nombre de Usuario")]
        public string? Username { get; set; }

        /// <summary>
        /// Contraseña del usuario.
        /// </summary>
        [Required(ErrorMessage = "La contraseña es requerida")]
        [MaxLength(8, ErrorMessage = "Máximo 8 caracteres")]
        [DataType(DataType.Password)]
        [Display(Name = "Contraseña")]
        public string? Password { get; set; }

        /// <summary>
        /// Identificador del rol asignado al usuario.
        /// </summary>
        [ForeignKey("Role")]
        [Required(ErrorMessage = "El rol es requerido")]
        [Display(Name = "Rol")]
        public int RoleID { get; set; }

        /// <summary>
        /// Correo electrónico del usuario.
        /// </summary>
        [Required(ErrorMessage = "El correo electrónico es requerido")]
        [EmailAddress(ErrorMessage = "Formato de correo electrónico no válido")]
        [Display(Name = "Correo Electrónico")]
        public string? Email { get; set; }

        /// <summary>
        /// Propiedad de navegación para el rol del usuario.
        /// </summary>
        public Role? Role { get; set; }
    }
}
