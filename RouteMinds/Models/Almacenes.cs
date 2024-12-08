using Microsoft.EntityFrameworkCore;

namespace RouteMinds.Models
{
    [Keyless]
    public class Almacenes
    {
        public int ALMACENID { get; set; }
        public string NOMBRE { get; set; }
        public double LATITUD { get; set; }
        public double LONGITUD { get; set; }
    }
}
