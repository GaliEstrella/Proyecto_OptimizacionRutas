using Microsoft.EntityFrameworkCore;

namespace RouteMinds.Models
{
    [Keyless]
    public class Rutas
    {
        public int Almacen { get; set; }
        public string Ruta { get; set; }
        public int Tienda { get; set; }
        public int Tarimas { get; set; }
        public double Distancia { get; set; } 
    }
}
