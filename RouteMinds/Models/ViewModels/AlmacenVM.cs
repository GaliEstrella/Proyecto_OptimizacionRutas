using System.ComponentModel.DataAnnotations;

namespace RouteMinds.Models.ViewModels
{
    public class AlmacenVM
    {
        [Required(ErrorMessage = "El ID del almacén es obligatorio.")]
        [Range(1, int.MaxValue, ErrorMessage = "El ID del almacén debe ser mayor a 0.")]
        public int AlmacenId { get; set; }

        [Required(ErrorMessage = "El nombre del almacén es obligatorio.")]
        [MaxLength(255, ErrorMessage = "El nombre del almacén no puede exceder 255 caracteres.")]
        public string Nombre { get; set; }

        [Required(ErrorMessage = "La latitud es obligatoria.")]
        [Range(-90, 90, ErrorMessage = "La latitud debe estar entre -90 y 90.")]
        public double Latitud { get; set; }

        [Required(ErrorMessage = "La longitud es obligatoria.")]
        [Range(-180, 180, ErrorMessage = "La longitud debe estar entre -180 y 180.")]
        public double Longitud { get; set; }
    }
}
