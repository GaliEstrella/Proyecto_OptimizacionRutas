using Microsoft.AspNetCore.Mvc;
using Microsoft.Data.SqlClient;
using Microsoft.EntityFrameworkCore;
using RouteMinds.Data;
using RouteMinds.Models;
using RouteMinds.Models.ViewModels;

namespace RouteMinds.Controllers
{
    public class TiendaController : Controller
    {
        private readonly ApplicationDbContext _db;

        public TiendaController(ApplicationDbContext db)
        {
            _db = db;
        }

        public IActionResult Index()
        {
            return View();
        }

        public IActionResult Create()
        {
            return View();
        }

        public IActionResult Edit()
        {
            return View();
        }

        #region API CALLS

        // GET: Obtener todas las tiendas
        [HttpGet]
        public IActionResult GetAll()
        {
            return Json(new { data = _db.Tiendas });
        }

        // GET: Editar una tienda
        [HttpGet]
        public IActionResult Edit(int almacen, int tienda)
        {
            var tiendaDb = _db.Tiendas.FirstOrDefault(t => t.ALMACEN == almacen && t.TIENDA == tienda);

            if (tiendaDb == null)
            {
                TempData["error"] = "La tienda no existe.";
                return RedirectToAction("Index");
            }

            var tiendaVM = new TiendaVM
            {
                Almacen = tiendaDb.ALMACEN,
                Tienda = tiendaDb.TIENDA,
                Nombre = tiendaDb.NOMBRE,
                Latitud = tiendaDb.LATITUD,
                Longitud = tiendaDb.LONGITUD,
                Tarimas = tiendaDb.TARIMAS
            };

            return View(tiendaVM);
        }

        // POST: Crear una nueva tienda
        [HttpPost]
        public IActionResult CreateTienda(TiendaVM tiendaVM)
        {
            if (!ModelState.IsValid)
            {
                TempData["error"] = "Error en la validación. Por favor, corrige los errores.";
                return View("Create");
            }

            try
            {
                var parameters = new[]
                {
                    new SqlParameter("@ALMACEN", tiendaVM.Almacen),
                    new SqlParameter("@TIENDA", tiendaVM.Tienda),
                    new SqlParameter("@NOMBRE", tiendaVM.Nombre),
                    new SqlParameter("@LATITUD", tiendaVM.Latitud),
                    new SqlParameter("@LONGITUD", tiendaVM.Longitud),
                    new SqlParameter("@TARIMAS", tiendaVM.Tarimas)
                };

                _db.Database.ExecuteSqlRaw("EXEC [sp_CreateOrUpdateTienda] @ALMACEN, @TIENDA, @NOMBRE, @LATITUD, @LONGITUD, @TARIMAS", parameters);

                TempData["success"] = "Tienda guardada correctamente.";
                return RedirectToAction("Index");
            }
            catch (Exception ex)
            {
                TempData["error"] = $"Error al guardar la tienda: {ex.Message}";
                return View("Create");
            }
        }

        // POST: Editar una tienda existente
        [HttpPost]
        public IActionResult EditTienda(TiendaVM tiendaVM)
        {
            if (!ModelState.IsValid)
            {
                TempData["error"] = "Error en la validación. Por favor, corrige los errores.";
                return View("Edit");
            }

            try
            {
                var parameters = new[]
                {
                    new SqlParameter("@ALMACEN", tiendaVM.Almacen),
                    new SqlParameter("@TIENDA", tiendaVM.Tienda),
                    new SqlParameter("@NOMBRE", tiendaVM.Nombre),
                    new SqlParameter("@LATITUD", tiendaVM.Latitud),
                    new SqlParameter("@LONGITUD", tiendaVM.Longitud),
                    new SqlParameter("@TARIMAS", tiendaVM.Tarimas)
                };

                _db.Database.ExecuteSqlRaw("EXEC [sp_CreateOrUpdateTienda] @ALMACEN, @TIENDA, @NOMBRE, @LATITUD, @LONGITUD, @TARIMAS", parameters);

                TempData["success"] = "Tienda actualizada correctamente.";
                return RedirectToAction("Index");
            }
            catch (Exception ex)
            {
                TempData["error"] = $"Error al actualizar la tienda: {ex.Message}";
                return View("Edit");
            }
        }

        // DELETE: Eliminar una tienda
        [HttpDelete]
        public IActionResult Delete(int almacen, int tienda)
        {
            try
            {
                var parameters = new[]
                {
                    new SqlParameter("@TIENDA", tienda)
                };

                _db.Database.ExecuteSqlRaw("EXEC [sp_DeleteTienda] @TIENDA", parameters);

                return Json(new { success = true, message = "Tienda eliminada correctamente." });
            }
            catch (Exception ex)
            {
                return Json(new { success = false, message = $"Error: {ex.Message}" });
            }
        }

        #endregion
    }
}
