using Microsoft.EntityFrameworkCore;

namespace RouteMinds.Models
{
    [Keyless]
    public class Tiendas
    {
        public int ALMACEN { get; set; }
        public int TIENDA { get; set; }
        public string NOMBRE { get; set; }
        public double LATITUD { get; set; }
        public double LONGITUD { get; set; }
        public int TARIMAS { get; set; }
    }
}
