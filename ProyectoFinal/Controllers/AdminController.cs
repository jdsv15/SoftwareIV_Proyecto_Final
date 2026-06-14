using Microsoft.AspNetCore.Mvc;

public class AdminController : Controller
{
    public IActionResult Especialidades() => View();
    public IActionResult Medicos() => View();
    public IActionResult Usuarios() => View();
    public IActionResult Bloqueos() => View();
}