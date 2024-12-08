using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Data.SqlClient;
using Microsoft.EntityFrameworkCore;
using RouteMinds.Data;
using RouteMinds.Models.ViewModels;
using System.Security.Claims;

namespace RouteMinds.Controllers
{
    public class LoginController : Controller
    {
        private readonly IConfiguration _configuration;
        private readonly ApplicationDbContext _db;


        public LoginController(IConfiguration configuration, ApplicationDbContext db)
        {
            _db = db;
            _configuration = configuration;
        }
        public IActionResult Index()
        {
            return View();
        }


        [HttpPost]
        public async Task<IActionResult> Index(LoginViewModel model)
        {
            if (!ModelState.IsValid)
            {
                return View(model);
            }

            // Find the user in the database
            var user = _db.Usuarios
                .FirstOrDefault(u => u.Nombre == model.Username && u.Password == model.Password);

            if (user == null)
            {
                ModelState.AddModelError(string.Empty, "Nombre de usuario o contraseña incorrectos.");
                return View(model);
            }
            // Create user claims
            var claims = new List<Claim>
            {
                new Claim(ClaimTypes.Name, user.Nombre),
                new Claim("Correo", user.Correo), // Custom claim
                new Claim(ClaimTypes.Role, "User") // Add role if needed
            };
            var claimsIdentity = new ClaimsIdentity(
                        claims, CookieAuthenticationDefaults.AuthenticationScheme);
            var authProperties = new AuthenticationProperties
            {
                ExpiresUtc = DateTime.Now.AddDays(30),
            };
            await HttpContext.SignInAsync(
                CookieAuthenticationDefaults.AuthenticationScheme,
                new ClaimsPrincipal(claimsIdentity),
                authProperties);
            // Redirect to home page after successful login
            return RedirectToAction("Index", "Home");
        }

        public IActionResult Register()
        {
            return View();
        }
        public IActionResult TestConnection()
        {
            try
            {
                // Attempt to fetch some data from the Usuarios table
                var users = _db.Usuarios.Take(10).ToList();

                if (users.Any())
                {
                    return Json(new { Success = true, Message = "Connection successful", Data = users });
                }
                else
                {
                    return Json(new { Success = true, Message = "Connection successful but no users found." });
                }
            }
            catch (Exception ex)
            {
                // If there is an error, return the exception details
                return Json(new { Success = false, Message = "Connection failed", Error = ex.Message });
            }
        }

        public async Task<IActionResult> Logout()
        {
            await HttpContext.SignOutAsync(CookieAuthenticationDefaults.AuthenticationScheme);
            return RedirectToAction("Index", "Login");
        }

        #region API CALLS

        [HttpPost]
        public async Task<IActionResult> RegisterUsuario(RegisterVM registerVM)
        {
            if (!ModelState.IsValid)
            {
                TempData["error"] = "Por favor, completa todos los campos correctamente.";
                return RedirectToAction("Register");
            }

            try
            {
                // Uso de parámetros para evitar inyección de SQL y errores de formato
                var parameters = new[]
                {
                    new SqlParameter("@NombreUsuario", registerVM.NombreUsuario),
                    new SqlParameter("@NombreEmpresa", registerVM.NombreEmpresa),
                    new SqlParameter("@Correo", registerVM.username),
                    new SqlParameter("@Password", registerVM.password)
                };

                // Ejecutar el procedimiento almacenado con parámetros
                await _db.Database.ExecuteSqlRawAsync(
                    "EXEC [sp_postRegistrarUsuario] @NombreUsuario, @NombreEmpresa, @Correo, @Password",
                    parameters);

                TempData["success"] = "Usuario registrado exitosamente.";
                return RedirectToAction("Index");
            }
            catch (Exception ex)
            {
                TempData["error"] = $"Hubo un error al registrar el usuario: {ex.Message}";
                return RedirectToAction("Register");
            }
        }



        #endregion

    }
}
