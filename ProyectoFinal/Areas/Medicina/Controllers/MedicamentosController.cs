using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using ProyectoFinal.Models;
using ProyectoFinal.Repository;

namespace ProyectoFinal.Areas.Medicina.Controllers
{
    [Area("Medicina")]
    [Authorize(Roles = "Medico,Administrador")]
    public class MedicamentosController : Controller
    {
        private readonly IUnidadTrabajo _unidadTrabajo;

        public MedicamentosController(IUnidadTrabajo unidadTrabajo)
        {
            _unidadTrabajo = unidadTrabajo;
        }

        // Lista
        public IActionResult Index()
        {
            var lista = _unidadTrabajo.Medicamento.GetAll();
            return View(lista);
        }

        // Crear y Editar get
        public IActionResult Upsert(int? id)
        {
            Medicamento medicamento = new Medicamento();

            if (id == null || id == 0)
            {
                return View(medicamento);
            }

            medicamento = _unidadTrabajo.Medicamento.Get(id.GetValueOrDefault());
            if (medicamento == null) return NotFound();

            return View(medicamento);
        }

        // Crear y Editar post
        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Upsert(Medicamento medicamento)
        {
            if (ModelState.IsValid)
            {
                if (medicamento.Id == 0)
                {
                    _unidadTrabajo.Medicamento.Add(medicamento);
                }
                else
                {
                    _unidadTrabajo.Medicamento.Actualizar(medicamento);
                }

                _unidadTrabajo.Guardar();
                return RedirectToAction(nameof(Index));
            }

            return View(medicamento);
        }

        // Eliminar
        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Delete(int id)
        {
            var obj = _unidadTrabajo.Medicamento.Get(id);
            if (obj == null) return NotFound();

            _unidadTrabajo.Medicamento.Remove(obj);
            _unidadTrabajo.Guardar();
            return RedirectToAction(nameof(Index));
        }
    }
}