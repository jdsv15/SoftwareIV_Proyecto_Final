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
    public class AdminController : Controller
    {
        private readonly IUnidadTrabajo _unidadTrabajo;
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly RoleManager<IdentityRole> _roleManager;
        private readonly IWebHostEnvironment _hostEnvironment;

        public AdminController(IUnidadTrabajo unidadTrabajo, UserManager<ApplicationUser> userManager, RoleManager<IdentityRole> roleManager, IWebHostEnvironment hostEnvironment)
        {
            _unidadTrabajo = unidadTrabajo;
            _userManager = userManager;
            _roleManager = roleManager;
            _hostEnvironment = hostEnvironment;
        }

        // ESPECIALIDADES
        public IActionResult Especialidades() => View();

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
                return RedirectToAction(nameof(Especialidades));
            }
            return View(especialidad);
        }

        #region API Endpoints para Especialidades
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

        // USUARIOS
        public async Task<IActionResult> Usuarios()
        {
            var usuarios = _userManager.Users.ToList();
            foreach (var user in usuarios)
            {
                var roles = await _userManager.GetRolesAsync(user);
                user.Rol = roles.FirstOrDefault() ?? "Sin Rol";
            }
            return View(usuarios);
        }

        public IActionResult CrearUsuario()
        {
            ViewBag.RoleList = _roleManager.Roles.ToList();
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> CrearUsuario(ApplicationUser user, string userRole, string password)
        {
            ModelState.Remove("Id");
            ModelState.Remove("UserName");
            ModelState.Remove("Rol");

            if (ModelState.IsValid)
            {
                user.UserName = user.Email;
                var result = await _userManager.CreateAsync(user, password);

                if (result.Succeeded)
                {
                    if (!string.IsNullOrEmpty(userRole))
                    {
                        await _userManager.AddToRoleAsync(user, userRole);
                    }

                    if (userRole == "Medico")
                    {
                        TempData["Success"] = "Usuario creado. Por favor complete el perfil del Medico.";
                        return RedirectToAction("UpsertMedico", new { userId = user.Id });
                    }

                    TempData["Success"] = "Usuario creado exitosamente.";
                    return RedirectToAction(nameof(Usuarios));
                }
                foreach (var error in result.Errors) ModelState.AddModelError(string.Empty, error.Description);
            }
            ViewBag.RoleList = _roleManager.Roles.ToList();
            return View(user);
        }

        [HttpGet]
        public async Task<IActionResult> EditarUsuario(string id)
        {
            var usuario = await _userManager.FindByIdAsync(id);
            if (usuario == null) return NotFound();

            var roles = await _userManager.GetRolesAsync(usuario);
            ViewBag.RoleList = _roleManager.Roles.ToList();
            ViewBag.CurrentRole = roles.FirstOrDefault();
            return View(usuario);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> EditarUsuario(ApplicationUser model, string userRole)
        {
            var usuario = await _userManager.FindByIdAsync(model.Id);
            if (usuario == null) return NotFound();

            usuario.Nombre = model.Nombre;
            usuario.Cedula = model.Cedula;
            usuario.Email = model.Email;
            usuario.UserName = model.Email;

            var result = await _userManager.UpdateAsync(usuario);
            if (result.Succeeded)
            {
                var currentRoles = await _userManager.GetRolesAsync(usuario);
                await _userManager.RemoveFromRolesAsync(usuario, currentRoles);
                await _userManager.AddToRoleAsync(usuario, userRole);

                if (userRole == "Medico")
                {
                    var perfilMedico = _unidadTrabajo.Medico.GetAll().FirstOrDefault(m => m.UserId == usuario.Id);
                    if (perfilMedico == null)
                    {
                        TempData["Success"] = "Usuario actualizado. Por favor complete el perfil del Medico.";
                        return RedirectToAction("UpsertMedico", new { userId = usuario.Id });
                    }
                }

                TempData["Success"] = "Usuario actualizado exitosamente.";
                return RedirectToAction(nameof(Usuarios));
            }
            ViewBag.RoleList = _roleManager.Roles.ToList();
            return View(model);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteUsuario(string id)
        {
            var usuario = await _userManager.FindByIdAsync(id);
            if (usuario == null) return Json(new { success = false, message = "Usuario no encontrado." });

            var currentUser = await _userManager.GetUserAsync(User);
            if (currentUser != null && currentUser.Id == id)
                return Json(new { success = false, message = "No puedes eliminar tu propio usuario." });

            var result = await _userManager.DeleteAsync(usuario);
            return result.Succeeded
                ? Json(new { success = true, message = "Usuario eliminado correctamente." })
                : Json(new { success = false, message = "Error al eliminar el usuario." });
        }

        // MÉDICOS
        [HttpGet]
        public async Task<IActionResult> Medicos()
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
                    TempData["Success"] = "Perfil médico completado exitosamente.";
                }
                else
                {
                    _unidadTrabajo.Medico.Actualizar(medicoVM.Medico);
                    TempData["Success"] = "Perfil médico actualizado exitosamente.";
                }

                _unidadTrabajo.Guardar();
                return RedirectToAction(nameof(Medicos));
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