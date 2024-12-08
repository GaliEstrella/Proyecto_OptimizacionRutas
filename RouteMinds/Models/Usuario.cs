using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

namespace RouteMinds.Models
{
    public class Usuario
    {
        [Key]
        public int UsuarioId { get; set; } // Primary Key

        [Required(ErrorMessage = "El nombre es obligatorio.")]
        [StringLength(100, ErrorMessage = "El nombre no puede tener más de 100 caracteres.")]
        public string Nombre { get; set; }

        [Required(ErrorMessage = "El correo es obligatorio.")]
        [EmailAddress(ErrorMessage = "Debe ser un correo electrónico válido.")]
        [StringLength(150, ErrorMessage = "El correo no puede tener más de 150 caracteres.")]
        public string Correo { get; set; }

        [Required(ErrorMessage = "La contraseña es obligatoria.")]
        [StringLength(100, MinimumLength = 6, ErrorMessage = "La contraseña debe tener entre 6 y 100 caracteres.")]
        [DataType(DataType.Password)]
        public string Password { get; set; }

        [ForeignKey("Empresa")]
        public int? EmpresaId { get; set; } // Foreign Key (nullable in case a user doesn't belong to an Empresa)

        public Empresa Empresa { get; set; } // Navigation Property
    }
}
