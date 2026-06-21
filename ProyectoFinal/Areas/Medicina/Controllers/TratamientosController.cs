using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using ProyectoFinal.Models;
using ProyectoFinal.Repository;

namespace ProyectoFinal.Areas.Medicina.Controllers
{
    [Area("Medicina")]
    [Authorize(Roles = "Medico,Administrador")]
    public class TratamientosController : Controller
    {
        private readonly IUnidadTrabajo _unidadTrabajo;

        public TratamientosController(IUnidadTrabajo unidadTrabajo)
        {
            _unidadTrabajo = unidadTrabajo;
        }

        // Lista
        public IActionResult Index()
        {
            var lista = _unidadTrabajo.Tratamiento.GetAll();
            return View(lista);
        }

        // Crear y Editar get
        public IActionResult Upsert(int? id)
        {
            Tratamiento tratamiento = new Tratamiento();

            if (id == null || id == 0)
            {
                return View(tratamiento);
            }

            tratamiento = _unidadTrabajo.Tratamiento.Get(id.GetValueOrDefault());
            if (tratamiento == null) return NotFound();

            return View(tratamiento);
        }

        // Crear y Editar post
        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Upsert(Tratamiento tratamiento)
        {
            if (ModelState.IsValid)
            {
                if (tratamiento.Id == 0)
                {
                    _unidadTrabajo.Tratamiento.Add(tratamiento);
                }
                else
                {
                    _unidadTrabajo.Tratamiento.Actualizar(tratamiento);
                }

                _unidadTrabajo.Guardar();
                return RedirectToAction(nameof(Index));
            }

            return View(tratamiento);
        }

        //Eliminar
        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Delete(int id)
        {
            var obj = _unidadTrabajo.Tratamiento.Get(id);
            if (obj == null) return NotFound();

            _unidadTrabajo.Tratamiento.Remove(obj);
            _unidadTrabajo.Guardar();
            return RedirectToAction(nameof(Index));
        }
    }
}