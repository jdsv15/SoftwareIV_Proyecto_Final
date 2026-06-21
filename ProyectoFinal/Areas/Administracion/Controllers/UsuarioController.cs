using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using ProyectoFinal.Models;
using ProyectoFinal.Repository;

namespace ProyectoFinal.Areas.Administracion.Controllers
{
    [Area("Administracion")]
    [Authorize(Roles = "Administrador")]
    public class UsuarioController : Controller
    {
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly RoleManager<IdentityRole> _roleManager;
        private readonly IUnidadTrabajo _unidadTrabajo;

        public UsuarioController(
            UserManager<ApplicationUser> userManager,
            RoleManager<IdentityRole> roleManager,
            IUnidadTrabajo unidadTrabajo)
        {
            _userManager = userManager;
            _roleManager = roleManager;
            _unidadTrabajo = unidadTrabajo;
        }

        public async Task<IActionResult> Index()
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

                    if (userRole == "Paciente")
                    {
                        CrearOActualizarExpedientePaciente(user);
                        _unidadTrabajo.Guardar();
                    }

                    if (userRole == "Medico")
                    {
                        TempData["Success"] = "Usuario creado. Por favor complete el perfil del médico.";
                        return RedirectToAction("UpsertMedico", "Medico", new { userId = user.Id });
                    }

                    TempData["Success"] = "Usuario creado exitosamente.";
                    return RedirectToAction(nameof(Index));
                }

                foreach (var error in result.Errors)
                {
                    ModelState.AddModelError(string.Empty, error.Description);
                }
            }

            ViewBag.RoleList = _roleManager.Roles.ToList();
            return View(user);
        }

        [HttpGet]
        public async Task<IActionResult> EditarUsuario(string id)
        {
            var usuario = await _userManager.FindByIdAsync(id);

            if (usuario == null)
            {
                return NotFound();
            }

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

            if (usuario == null)
            {
                return NotFound();
            }

            usuario.Nombre = model.Nombre;
            usuario.Cedula = model.Cedula;
            usuario.Email = model.Email;
            usuario.UserName = model.Email;

            var result = await _userManager.UpdateAsync(usuario);

            if (result.Succeeded)
            {
                var currentRoles = await _userManager.GetRolesAsync(usuario);

                await _userManager.RemoveFromRolesAsync(usuario, currentRoles);

                if (!string.IsNullOrEmpty(userRole))
                {
                    await _userManager.AddToRoleAsync(usuario, userRole);
                }

                if (userRole == "Paciente")
                {
                    CrearOActualizarExpedientePaciente(usuario);
                    _unidadTrabajo.Guardar();
                }
                else
                {
                    EliminarExpedientesDeUsuarioPaciente(usuario);
                    _unidadTrabajo.Guardar();
                }

                if (userRole == "Medico")
                {
                    var perfilMedico = _unidadTrabajo.Medico
                        .GetAll()
                        .FirstOrDefault(m => m.UserId == usuario.Id);

                    if (perfilMedico == null)
                    {
                        TempData["Success"] = "Usuario actualizado. Por favor complete el perfil del médico.";
                        return RedirectToAction("UpsertMedico", "Medico", new { userId = usuario.Id });
                    }
                }

                TempData["Success"] = "Usuario actualizado exitosamente.";
                return RedirectToAction(nameof(Index));
            }

            ViewBag.RoleList = _roleManager.Roles.ToList();
            return View(model);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteUsuario(string id)
        {
            var usuario = await _userManager.FindByIdAsync(id);

            if (usuario == null)
            {
                return Json(new { success = false, message = "Usuario no encontrado." });
            }

            var currentUser = await _userManager.GetUserAsync(User);

            if (currentUser != null && currentUser.Id == id)
            {
                return Json(new { success = false, message = "No puedes eliminar tu propio usuario." });
            }

            var roles = await _userManager.GetRolesAsync(usuario);

            DesasociarMedicoResponsable(usuario.Id);

            if (roles.Contains("Paciente"))
            {
                EliminarExpedientesDeUsuarioPaciente(usuario);
            }

            if (roles.Contains("Medico"))
            {
                var perfilMedico = _unidadTrabajo.Medico
                    .GetAll()
                    .FirstOrDefault(m => m.UserId == usuario.Id);

                if (perfilMedico != null)
                {
                    _unidadTrabajo.Medico.Remove(perfilMedico);
                }
            }

            _unidadTrabajo.Guardar();

            var result = await _userManager.DeleteAsync(usuario);

            return result.Succeeded
                ? Json(new { success = true, message = "Usuario y registros asociados eliminados correctamente." })
                : Json(new { success = false, message = "Error al eliminar el usuario." });
        }

        [HttpGet]
        public async Task<IActionResult> Bloqueos()
        {
            var currentUser = await _userManager.GetUserAsync(User);

            var usuarios = _userManager.Users
                .Where(u => currentUser == null || u.Id != currentUser.Id)
                .ToList();

            foreach (var user in usuarios)
            {
                var roles = await _userManager.GetRolesAsync(user);
                user.Rol = roles.FirstOrDefault() ?? "Sin Rol";
            }

            return View(usuarios);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> BloquearDesbloquear(string id)
        {
            var usuario = await _userManager.FindByIdAsync(id);

            if (usuario == null)
            {
                return Json(new { success = false, message = "Usuario no encontrado." });
            }

            usuario.LockoutEnabled = true;

            if (usuario.LockoutEnd != null && usuario.LockoutEnd > DateTime.Now)
            {
                usuario.LockoutEnd = DateTime.Now;
                await _userManager.UpdateAsync(usuario);

                return Json(new { success = true, message = "Usuario desbloqueado correctamente." });
            }
            else
            {
                usuario.LockoutEnd = DateTime.Now.AddYears(100);
                await _userManager.UpdateAsync(usuario);

                return Json(new { success = true, message = "Usuario bloqueado correctamente." });
            }
        }

        private void CrearOActualizarExpedientePaciente(ApplicationUser usuario)
        {
            var paciente = _unidadTrabajo.Paciente
                .GetFirstOrDefault(p => p.UsuarioId == usuario.Id);

            if (paciente == null)
            {
                paciente = _unidadTrabajo.Paciente
                    .GetFirstOrDefault(p => p.Cedula == usuario.Cedula);
            }

            if (paciente == null)
            {
                paciente = new Paciente
                {
                    UsuarioId = usuario.Id,
                    Nombre = usuario.Nombre,
                    Cedula = usuario.Cedula,
                    FechaNacimiento = DateTime.Now
                };

                _unidadTrabajo.Paciente.Add(paciente);
            }
            else
            {
                paciente.UsuarioId = usuario.Id;
                paciente.Nombre = usuario.Nombre;
                paciente.Cedula = usuario.Cedula;
            }
        }

        private void EliminarExpedientesDeUsuarioPaciente(ApplicationUser usuario)
        {
            var expedientes = _unidadTrabajo.Paciente
                .GetAll(p => p.UsuarioId == usuario.Id || p.Cedula == usuario.Cedula)
                .ToList();

            foreach (var expediente in expedientes)
            {
                EliminarExpedienteCompleto(expediente);
            }
        }

        private void EliminarExpedienteCompleto(Paciente paciente)
        {
            var historiales = _unidadTrabajo.HistorialClinico
                .GetAll(h => h.PacienteId == paciente.Id)
                .ToList();

            foreach (var historial in historiales)
            {
                _unidadTrabajo.HistorialClinico.Remove(historial);
            }

            var archivos = _unidadTrabajo.ArchivoExpediente
                .GetAll(a => a.PacienteId == paciente.Id)
                .ToList();

            foreach (var archivo in archivos)
            {
                _unidadTrabajo.ArchivoExpediente.Remove(archivo);
            }

            var padecimientos = _unidadTrabajo.PacientePadecimiento
                .GetAll(p => p.PacienteId == paciente.Id)
                .ToList();

            foreach (var padecimiento in padecimientos)
            {
                _unidadTrabajo.PacientePadecimiento.Remove(padecimiento);
            }

            var tratamientos = _unidadTrabajo.PacienteTratamiento
                .GetAll(t => t.PacienteId == paciente.Id)
                .ToList();

            foreach (var tratamiento in tratamientos)
            {
                _unidadTrabajo.PacienteTratamiento.Remove(tratamiento);
            }

            var medicamentos = _unidadTrabajo.PacienteMedicamento
                .GetAll(m => m.PacienteId == paciente.Id)
                .ToList();

            foreach (var medicamento in medicamentos)
            {
                _unidadTrabajo.PacienteMedicamento.Remove(medicamento);
            }

            _unidadTrabajo.Paciente.Remove(paciente);
        }

        private void DesasociarMedicoResponsable(string medicoId)
        {
            var pacientes = _unidadTrabajo.Paciente
                .GetAll(p => p.MedicoId == medicoId)
                .ToList();

            foreach (var paciente in pacientes)
            {
                paciente.MedicoId = null;
            }
        }
    }
}