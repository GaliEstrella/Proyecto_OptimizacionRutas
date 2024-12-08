using Microsoft.AspNetCore.Mvc;
using Microsoft.Data.SqlClient;
using Microsoft.EntityFrameworkCore;
using RouteMinds.Data;
using RouteMinds.Models;
using RouteMinds.Models.ViewModels;

namespace RouteMinds.Controllers
{
    public class AlmacenController : Controller
    {
        private readonly ApplicationDbContext _db;

        public AlmacenController(ApplicationDbContext db)
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

        [HttpGet]
        public IActionResult GetAll()
        {
            return Json(new { data = _db.Almacenes });

        }

        [HttpGet]
        public IActionResult Edit(int id)
        {
            var almacen = _db.Almacenes.FirstOrDefault(a => a.ALMACENID == id);

            if (almacen == null)
            {
                TempData["error"] = "El almacén no existe.";
                return RedirectToAction("Index");
            }

            var almacenVM = new AlmacenVM
            {
                AlmacenId = almacen.ALMACENID,
                Nombre = almacen.NOMBRE,
                Latitud = almacen.LATITUD,
                Longitud = almacen.LONGITUD
            };

            return View(almacenVM);
        }


        [HttpPost]
        public IActionResult CreateAlmacen(AlmacenVM almacenVM)
        {
            if (!ModelState.IsValid)
            {
                // Si la validación falla, regresar la misma vista con los datos ingresados y errores
                TempData["error"] = "Error en la validación. Por favor, corrige los errores.";
                return View("Create");
            }

            try
            {
                // Configurar los parámetros para el procedimiento almacenado
                var parameters = new[]
                {
                    new SqlParameter("@ALMACENID", almacenVM.AlmacenId),
                    new SqlParameter("@NOMBRE", almacenVM.Nombre),
                    new SqlParameter("@LATITUD", almacenVM.Latitud),
                    new SqlParameter("@LONGITUD", almacenVM.Longitud)
                };

                // Ejecutar el procedimiento almacenado
                _db.Database.ExecuteSqlRaw("EXEC [sp_CreateOrUpdateAlmacen] @ALMACENID, @NOMBRE, @LATITUD, @LONGITUD", parameters);

                // Redirigir a Index con un mensaje de éxito
                TempData["success"] = "Almacén guardado correctamente.";
                return RedirectToAction("Index");
            }
            catch (Exception ex)
            {
                // Si ocurre un error, mostrar un mensaje y regresar a la vista actual
                TempData["error"] = $"Error al guardar el almacén: {ex.Message}";
                return View("Create");
            }
        }

        [HttpPost]
        public IActionResult EditAlmacen(AlmacenVM almacenVM)
        {
            if (!ModelState.IsValid)
            {
                // Si la validación falla, regresar a la misma vista con los datos ingresados y errores
                TempData["error"] = "Error en la validación. Por favor, corrige los errores.";
                return View("Edit");
            }

            try
            {
                // Configurar los parámetros para el procedimiento almacenado
                var parameters = new[]
                {
            new SqlParameter("@ALMACENID", almacenVM.AlmacenId),
            new SqlParameter("@NOMBRE", almacenVM.Nombre),
            new SqlParameter("@LATITUD", almacenVM.Latitud),
            new SqlParameter("@LONGITUD", almacenVM.Longitud)
        };

                // Ejecutar el procedimiento almacenado
                _db.Database.ExecuteSqlRaw("EXEC [sp_CreateOrUpdateAlmacen] @ALMACENID, @NOMBRE, @LATITUD, @LONGITUD", parameters);

                // Redirigir a Index con un mensaje de éxito
                TempData["success"] = "Almacén actualizado correctamente.";
                return RedirectToAction("Index");
            }
            catch (Exception ex)
            {
                // Si ocurre un error, mostrar un mensaje y regresar a la vista actual
                TempData["error"] = $"Error al actualizar el almacén: {ex.Message}";
                return View("Edit");
            }
        }




        // DELETE: Eliminar un almacén
        [HttpDelete]
        public IActionResult Delete(int id)
        {
            try
            {
                // Parámetro del procedimiento almacenado
                var parameter = new SqlParameter("@ALMACENID", id);

                // Ejecutar el procedimiento almacenado
                _db.Database.ExecuteSqlRaw("EXEC [sp_DeleteAlmacen] @ALMACENID", parameter);

                return Json(new { success = true, message = "Almacén eliminado correctamente." });
            }
            catch (Exception ex)
            {
                return Json(new { success = false, message = $"Error: {ex.Message}" });
            }
        }

        #endregion
    }
}
