using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using ProyectoFinal.Repository; 

namespace ProyectoFinal.Areas.Administracion.Controllers
{
    [Area("Administracion")]
    [Authorize(Roles = "Administrador")]
    public class AdminController : Controller
    {
        private readonly IUnidadTrabajo _unidadTrabajo;
        public AdminController(IUnidadTrabajo unidadTrabajo)
        {
            _unidadTrabajo = unidadTrabajo;
        }

        // GET: Administracion/Admin/Especialidades
        public IActionResult Especialidades()
        {
            var listaEspecialidades = _unidadTrabajo.Especialidad.GetAll();

            return View(listaEspecialidades);
        }

        public IActionResult Medicos() => View();
        public IActionResult Usuarios() => View();
        public IActionResult Bloqueos() => View();
    }
}