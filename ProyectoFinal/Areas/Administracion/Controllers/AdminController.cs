using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using ProyectoFinal.Repository;

namespace ProyectoFinal.Areas.Administracion.Controllers
{
    [Area("Administracion")]
    [Authorize(Roles = "Administrador")]
    public class AdminController : Controller
    {
        private readonly IUnidadTrabajo _unidadTrabajo;
        private readonly UserManager<IdentityUser> _userManager;

        public AdminController(IUnidadTrabajo unidadTrabajo, UserManager<IdentityUser> userManager)
        {
            _unidadTrabajo = unidadTrabajo;
            _userManager = userManager;
        }

        // Especialidades
        public IActionResult Especialidades()
        {
            var lista = _unidadTrabajo.Especialidad.GetAll();
            return View(lista);
        }

        [HttpGet]
        public IActionResult CrearEspecialidad()
        {
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> CrearEspecialidad(ProyectoFinal.Models.Especialidad especialidad)
        {
            if (ModelState.IsValid)
            {
                _unidadTrabajo.Especialidad.Add(especialidad);
                await _unidadTrabajo.SaveAsync();
                return RedirectToAction("Especialidades");
            }
            return View(especialidad);
        }

        // Usuarios
        public IActionResult Usuarios()
        {
            var usuarios = _userManager.Users.ToList();
            return View(usuarios);
        }

        public IActionResult Medicos() => View();
        public IActionResult Bloqueos() => View();
    }
}