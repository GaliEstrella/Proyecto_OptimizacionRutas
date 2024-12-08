using System.ComponentModel.DataAnnotations;

namespace RouteMinds.Models.ViewModels
{
    public class TiendaVM
    {
        [Required(ErrorMessage = "El campo Almacén es obligatorio.")]
        [Display(Name = "Almacén")]
        public int Almacen { get; set; }

        [Required(ErrorMessage = "El campo Tienda es obligatorio.")]
        [Display(Name = "Tienda")]
        public int Tienda { get; set; }

        [Required(ErrorMessage = "El nombre de la tienda es obligatorio.")]
        [StringLength(50, ErrorMessage = "El nombre no puede exceder los 50 caracteres.")]
        [Display(Name = "Nombre de la Tienda")]
        public string Nombre { get; set; }

        [Required(ErrorMessage = "La latitud es obligatoria.")]
        [Range(-90, 90, ErrorMessage = "La latitud debe estar entre -90 y 90.")]
        [Display(Name = "Latitud")]
        public double Latitud { get; set; }

        [Required(ErrorMessage = "La longitud es obligatoria.")]
        [Range(-180, 180, ErrorMessage = "La longitud debe estar entre -180 y 180.")]
        [Display(Name = "Longitud")]
        public double Longitud { get; set; }

        [Required(ErrorMessage = "El campo Tarimas es obligatorio.")]
        [Range(0, int.MaxValue, ErrorMessage = "El número de tarimas debe ser un valor positivo.")]
        [Display(Name = "Tarimas")]
        public int Tarimas { get; set; }
    }
}
