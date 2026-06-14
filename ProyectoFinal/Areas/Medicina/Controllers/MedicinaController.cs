using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace ProyectoFinal.Areas.Medicina.Controllers 
{
    [Area("Medicina")]
    [Authorize(Roles = "Medico,Administrador")] 
    public class MedicinaController : Controller
    {
        public IActionResult Padecimientos() => View();
        public IActionResult Tratamientos() => View();
        public IActionResult Medicamentos() => View();
        public IActionResult ListaPacientes() => View();
        public IActionResult BuscarPaciente() => View();
    }
}