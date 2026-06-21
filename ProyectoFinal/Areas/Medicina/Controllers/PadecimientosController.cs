using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using ProyectoFinal.Models;
using ProyectoFinal.Repository;

namespace ProyectoFinal.Areas.Medicina.Controllers
{
    [Area("Medicina")]
    [Authorize(Roles = "Medico,Administrador")]
    public class PadecimientosController : Controller
    {
        private readonly IUnidadTrabajo _unidadTrabajo;

        public PadecimientosController(IUnidadTrabajo unidadTrabajo)
        {
            _unidadTrabajo = unidadTrabajo;
        }

        // Lista
        public IActionResult Index()
        {
            var lista = _unidadTrabajo.Padecimiento.GetAll();
            return View(lista);
        }

        //Crear y Editar get
        public IActionResult Upsert(int? id)
        {
            Padecimiento padecimiento = new Padecimiento();

            if (id == null || id == 0)
            {
                return View(padecimiento);
            }

            padecimiento = _unidadTrabajo.Padecimiento.Get(id.GetValueOrDefault());
            if (padecimiento == null) return NotFound();

            return View(padecimiento);
        }

        //Crear y Editar post
        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Upsert(Padecimiento padecimiento)
        {
            if (ModelState.IsValid)
            {
                if (padecimiento.Id == 0)
                {
                    _unidadTrabajo.Padecimiento.Add(padecimiento);
                }
                else
                {
                    _unidadTrabajo.Padecimiento.Actualizar(padecimiento);
                }

                _unidadTrabajo.Guardar();
                return RedirectToAction(nameof(Index));
            }

            return View(padecimiento);
        }

        //Eliminar
        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Delete(int id)
        {
            var obj = _unidadTrabajo.Padecimiento.Get(id);
            if (obj == null) return NotFound();

            _unidadTrabajo.Padecimiento.Remove(obj);
            _unidadTrabajo.Guardar();
            return RedirectToAction(nameof(Index));
        }
    }
}