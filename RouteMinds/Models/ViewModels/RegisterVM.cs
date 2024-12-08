using Microsoft.EntityFrameworkCore;

namespace RouteMinds.Models.ViewModels
{
    [Keyless]
    public class RegisterVM
    {
        public string NombreUsuario { get; set; }
        public string NombreEmpresa { get; set; }
        public string username { get; set; }
        public string password { get; set; }
    }
}
