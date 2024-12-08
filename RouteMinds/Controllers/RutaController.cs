using Microsoft.AspNetCore.Mvc;
using RouteMinds.Data;
using System.Diagnostics;

namespace RouteMinds.Controllers
{
    public class RutaController : Controller
    {
        private readonly ApplicationDbContext _db;

        public RutaController(ApplicationDbContext db)
        {
            _db = db;
        }
        public IActionResult Index()
        {
            return View();
        }

        #region API CALLS

        [HttpGet]
        public IActionResult GetAll()
        {
            return Json(new { data = _db.Rutas });
        }

        [HttpPost]
        public IActionResult EjecutarRutas()
        {
            try
            {
                // Ruta completa al archivo Python
                string pythonScript = @"C:\Users\Colibecas\Desktop\Proyecto RouteMinds\Archivos rutas optimas\rutas2.py";

                // Configurar el proceso
                var processInfo = new ProcessStartInfo
                {
                    FileName = "python", // Asegúrate de tener Python en PATH
                    Arguments = $"\"{pythonScript}\"", // Envolver la ruta en comillas dobles
                    RedirectStandardOutput = true,
                    RedirectStandardError = true,
                    UseShellExecute = false,
                    CreateNoWindow = true
                };

                // Ejecutar el proceso
                using (var process = Process.Start(processInfo))
                {
                    if (process != null)
                    {
                        string output = process.StandardOutput.ReadToEnd();
                        string error = process.StandardError.ReadToEnd();
                        process.WaitForExit();

                        if (process.ExitCode != 0)
                        {
                            return Json(new { success = false, message = $"Error: {error}" });
                        }
                        return Json(new { success = true, message = "Rutas ejecutadas correctamente.", output = output });
                    }
                }
            }
            catch (Exception ex)
            {
                return Json(new { success = false, message = $"Error al ejecutar rutas: {ex.Message}" });
            }
            return Json(new { success = false, message = "Error desconocido." });
        }




        #endregion
    }
}
