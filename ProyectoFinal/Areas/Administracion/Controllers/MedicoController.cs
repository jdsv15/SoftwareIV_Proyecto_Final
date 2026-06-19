using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using ProyectoFinal.Models;
using ProyectoFinal.Repository;

namespace ProyectoFinal.Areas.Administracion.Controllers
{
    [Area("Administracion")]
    [Authorize(Roles = "Administrador")]
    public class MedicoController : Controller
    {
        private readonly IUnidadTrabajo _unidadTrabajo;
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly IWebHostEnvironment _hostEnvironment;

        public MedicoController(IUnidadTrabajo unidadTrabajo, UserManager<ApplicationUser> userManager, IWebHostEnvironment hostEnvironment)
        {
            _unidadTrabajo = unidadTrabajo;
            _userManager = userManager;
            _hostEnvironment = hostEnvironment;
        }

        [HttpGet]
        public async Task<IActionResult> Index()
        {
            var usuariosMedicos = await _userManager.GetUsersInRoleAsync("Medico");
            var perfilesMedicos = _unidadTrabajo.Medico.GetAll().ToList();
            ViewBag.Perfiles = perfilesMedicos;
            return View(usuariosMedicos);
        }

        [HttpGet]
        public async Task<IActionResult> UpsertMedico(int? id, string? userId)
        {
            MedicoViewModel medicoVM = new MedicoViewModel()
            {
                Medico = new Medico(),
                EspecialidadLista = _unidadTrabajo.Especialidad.GetAll().Select(e => new SelectListItem
                {
                    Text = e.Nombre,
                    Value = e.Id.ToString()
                })
            };

            if (id == null || id == 0)
            {
                if (!string.IsNullOrEmpty(userId))
                {
                    medicoVM.Medico.UserId = userId;
                    var user = await _userManager.FindByIdAsync(userId);
                    ViewBag.NombreUsuario = user?.Nombre;
                }
                return View(medicoVM);
            }

            medicoVM.Medico = _unidadTrabajo.Medico.Get(id.GetValueOrDefault());
            if (medicoVM.Medico == null) return NotFound();

            return View(medicoVM);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult UpsertMedico(MedicoViewModel medicoVM, IFormFile? file)
        {
            ModelState.Remove("Medico.User");

            if (ModelState.IsValid)
            {
                string wwwRootPath = _hostEnvironment.WebRootPath;
                if (file != null)
                {
                    string fileName = Guid.NewGuid().ToString();
                    var uploads = Path.Combine(wwwRootPath, @"imagenes\medicos");
                    var extension = Path.GetExtension(file.FileName);

                    if (!Directory.Exists(uploads)) Directory.CreateDirectory(uploads);

                    if (medicoVM.Medico.Fotografia != null)
                    {
                        var oldImagePath = Path.Combine(wwwRootPath, medicoVM.Medico.Fotografia.TrimStart('\\'));
                        if (System.IO.File.Exists(oldImagePath)) System.IO.File.Delete(oldImagePath);
                    }

                    using (var fileStreams = new FileStream(Path.Combine(uploads, fileName + extension), FileMode.Create))
                    {
                        file.CopyTo(fileStreams);
                    }
                    medicoVM.Medico.Fotografia = @"\imagenes\medicos\" + fileName + extension;
                }

                var especialidadesDB = _unidadTrabajo.Especialidad.GetAll()
                                        .Where(e => medicoVM.EspecialidadesId.Contains(e.Id)).ToList();
                medicoVM.Medico.Especialidades = especialidadesDB;

                if (medicoVM.Medico.Id == 0)
                {
                    _unidadTrabajo.Medico.Add(medicoVM.Medico);
                    TempData["Success"] = "Perfil medico completado exitosamente.";
                }
                else
                {
                    _unidadTrabajo.Medico.Actualizar(medicoVM.Medico);
                    TempData["Success"] = "Perfil medico actualizado exitosamente.";
                }

                _unidadTrabajo.Guardar();
                return RedirectToAction(nameof(Index));
            }

            medicoVM.EspecialidadLista = _unidadTrabajo.Especialidad.GetAll().Select(e => new SelectListItem
            {
                Text = e.Nombre,
                Value = e.Id.ToString()
            });
            return View(medicoVM);
        }
    }
}