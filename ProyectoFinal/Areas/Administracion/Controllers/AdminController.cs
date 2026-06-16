using Microsoft.AspNetCore.Mvc;
using ProyectoFinal.Models;
using ProyectoFinal.Repository;

namespace ProyectoFinal.Areas.Administracion.Controllers
{
    [Area("Administracion")]
    public class AdminController : Controller
    {
        private readonly IUnidadTrabajo _unidadTrabajo;

        public AdminController(IUnidadTrabajo unidadTrabajo)
        {
            _unidadTrabajo = unidadTrabajo;
        }

        // GET: Lista de Especialidades
        public IActionResult Especialidades()
        {
            return View();
        }

        // GET: Upsert usuario entra mostrar
        public IActionResult Upsert(int? id)
        {
            Especialidad especialidad = new Especialidad();

            if (id == null || id == 0)  
            {
                // Crear nueva especialidad
                return View(especialidad);
            }

            // Editar especialidad existente
            especialidad = _unidadTrabajo.Especialidad.Get(id.GetValueOrDefault());
            if (especialidad == null)
            {
                return NotFound();
            }
            return View(especialidad);
        }

        // POST: Upsert guardar
        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Upsert(Especialidad especialidad)
        {
            if (ModelState.IsValid)
            {
                if (especialidad.Id == 0)
                {
                    _unidadTrabajo.Especialidad.Add(especialidad);
                }
                else
                {
                    _unidadTrabajo.Especialidad.Update(especialidad);
                }

                _unidadTrabajo.Guardar();
                return RedirectToAction(nameof(Especialidades));
            }
            return View(especialidad);
        }

        #region API Endpoints para DataTables
        [HttpGet]
        public IActionResult GetAll()
        {
            var todos = _unidadTrabajo.Especialidad.GetAll();
            return Json(new { data = todos });
        }

        [HttpDelete]
        public IActionResult Delete(int id)
        {
            var objFromDb = _unidadTrabajo.Especialidad.Get(id);
            if (objFromDb == null)
            {
                return Json(new { success = false, message = "Error al intentar borrar la especialidad" });
            }

            _unidadTrabajo.Especialidad.Remove(objFromDb);
            _unidadTrabajo.Guardar();
            return Json(new { success = true, message = "Especialidad eliminada correctamente" });
        }
        #endregion
    }
}