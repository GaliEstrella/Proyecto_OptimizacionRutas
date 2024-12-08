using System.ComponentModel.DataAnnotations;

namespace RouteMinds.Models
{
    public class Empresa
    {
        [Key]
        public int EmpresaId { get; set; } // Primary Key

        [Required(ErrorMessage = "El nombre de la empresa es obligatorio.")]
        [StringLength(150, ErrorMessage = "El nombre no puede tener más de 150 caracteres.")]
        public string Nombre { get; set; }
    }
}
