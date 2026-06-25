using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using ProyectoFinal.Models;
using ProyectoFinal.Repository;

namespace ProyectoFinal.Areas.Administracion.Controllers
{
    [Area("Administracion")]
    [Authorize(Roles = "Administrador")]
    public class EspecialidadController : Controller
    {
        private readonly IUnidadTrabajo _unidadTrabajo;

        public EspecialidadController(IUnidadTrabajo unidadTrabajo)
        {
            _unidadTrabajo = unidadTrabajo;
        }

        public IActionResult Index() => View();

        public IActionResult Upsert(int? id)
        {
            Especialidad especialidad = new Especialidad();
            if (id == null || id == 0) return View(especialidad);
            especialidad = _unidadTrabajo.Especialidad.Get(id.GetValueOrDefault());
            return especialidad == null ? NotFound() : View(especialidad);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Upsert(Especialidad especialidad)
        {
            if (ModelState.IsValid)
            {
                if (especialidad.Id == 0) _unidadTrabajo.Especialidad.Add(especialidad);
                else _unidadTrabajo.Especialidad.Update(especialidad);
                _unidadTrabajo.Guardar();
                return RedirectToAction(nameof(Index));
            }
            return View(especialidad);
        }

        #region API Endpoints
        [HttpGet]
        public IActionResult GetAll() => Json(new { data = _unidadTrabajo.Especialidad.GetAll() });

        [HttpDelete]
        public IActionResult Delete(int id)
        {
            var obj = _unidadTrabajo.Especialidad.Get(id);
            if (obj == null) return Json(new { success = false, message = "Especialidad no encontrada." });
            _unidadTrabajo.Especialidad.Remove(obj);
            _unidadTrabajo.Guardar();
            return Json(new { success = true, message = "Especialidad eliminada correctamente." });
        }
        #endregion
    }
}