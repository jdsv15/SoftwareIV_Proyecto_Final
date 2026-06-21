using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using ProyectoFinal.Models;
using ProyectoFinal.Models.ViewModels;
using ProyectoFinal.Repository;
using System;
using System.IO;
using System.Linq;
using System.Security.Claims;

namespace ProyectoFinal.Areas.Medicina.Controllers
{
    [Area("Medicina")]
    [Authorize(Roles = "Medico,Administrador")]
    public class PacientesController : Controller
    {
        private readonly IUnidadTrabajo _unidadTrabajo;
        private readonly IWebHostEnvironment _webHostEnvironment;

        // Inyectamos IWebHostEnvironment para poder guardar los archivos físicos en el servidor (wwwroot)
        public PacientesController(IUnidadTrabajo unidadTrabajo, IWebHostEnvironment webHostEnvironment)
        {
            _unidadTrabajo = unidadTrabajo;
            _webHostEnvironment = webHostEnvironment;
        }

        // Listado, búsqueda y ordenamiento
        public IActionResult Index(string busqueda)
        {
            // todos los pacientes
            var pacientes = _unidadTrabajo.Paciente.GetAll();

            // búsqueda
            if (!string.IsNullOrEmpty(busqueda))
            {
                busqueda = busqueda.ToLower();
                pacientes = pacientes.Where(p =>
                    p.Nombre.ToLower().Contains(busqueda) ||
                    p.Apellidos.ToLower().Contains(busqueda) ||
                    p.Cedula.ToLower().Contains(busqueda));
            }

            // Ordenamiento última fecha de atención, más reciente a menos reciente
            pacientes = pacientes.OrderByDescending(p => p.FechaUltimaAtencion).ToList();

            return View(pacientes);
        }

        // Detalle del Expediente
        public IActionResult Expediente(int id)
        {
            var paciente = _unidadTrabajo.Paciente.Get(id);
            if (paciente == null) return NotFound();

            // ViewModel con la info paciente
            var viewModel = new ExpedienteVM
            {
                Paciente = paciente,

                // Traemos historial ordenado por fecha descendente
                Historial = _unidadTrabajo.HistorialClinico.GetAll(h => h.PacienteId == id).OrderByDescending(h => h.FechaHora),

                Archivos = _unidadTrabajo.ArchivoExpediente.GetAll(a => a.PacienteId == id).OrderByDescending(a => a.FechaSubida),

                // Traemos solo los registros activos para mostrar en las tablas principales o manejarlos en la vista
                Padecimientos = _unidadTrabajo.PacientePadecimiento.GetAll(p => p.PacienteId == id, includeProperties: "Padecimiento"),

                Tratamientos = _unidadTrabajo.PacienteTratamiento.GetAll(t => t.PacienteId == id, includeProperties: "Tratamiento"),

                Medicamentos = _unidadTrabajo.PacienteMedicamento.GetAll(m => m.PacienteId == id, includeProperties: "Medicamento")
            };

            // Llenamos los catálogos para los menús desplegables (dropdowns) de los modales
            ViewBag.CatalogoPadecimientos = new SelectList(_unidadTrabajo.Padecimiento.GetAll(), "Id", "Nombre");
            ViewBag.CatalogoTratamientos = new SelectList(_unidadTrabajo.Tratamiento.GetAll(), "Id", "Nombre");
            ViewBag.CatalogoMedicamentos = new SelectList(_unidadTrabajo.Medicamento.GetAll(), "Id", "Nombre");

            return View(viewModel);
        }

        // Agregar Nota al Historial
        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult AgregarNotaHistorial(int pacienteId, string notas)
        {
            if (string.IsNullOrWhiteSpace(notas))
            {
                TempData["Error"] = "La nota clínica no puede estar vacía.";
                return RedirectToAction(nameof(Expediente), new { id = pacienteId });
            }

            var medicoId = User.FindFirstValue(ClaimTypes.NameIdentifier);

            var nuevaNota = new HistorialClinico
            {
                PacienteId = pacienteId,
                Notas = notas,
                FechaHora = DateTime.Now,
                MedicoId = medicoId
            };

            _unidadTrabajo.HistorialClinico.Add(nuevaNota);
            _unidadTrabajo.Guardar();

            TempData["Success"] = "Nota agregada exitosamente al historial.";
            return RedirectToAction(nameof(Expediente), new { id = pacienteId });
        }

        // Agregar Padecimiento
        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult AgregarPadecimiento(int pacienteId, int padecimientoId)
        {
            var medicoId = User.FindFirstValue(ClaimTypes.NameIdentifier);

            var nuevoPad = new PacientePadecimiento
            {
                PacienteId = pacienteId,
                PadecimientoId = padecimientoId,
                FechaDiagnostico = DateTime.Now,
                Activo = true,
                MedicoId = medicoId // Se incluye el id del médico según requerimiento F.IV
            };

            _unidadTrabajo.PacientePadecimiento.Add(nuevoPad);
            _unidadTrabajo.Guardar();

            TempData["Success"] = "Padecimiento agregado exitosamente.";
            return RedirectToAction(nameof(Expediente), new { id = pacienteId });
        }

        // Agregar Tratamiento
        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult AgregarTratamiento(int pacienteId, int tratamientoId)
        {
            var medicoId = User.FindFirstValue(ClaimTypes.NameIdentifier);

            var nuevoTratamiento = new PacienteTratamiento
            {
                PacienteId = pacienteId,
                TratamientoId = tratamientoId,
                FechaAsignacion = DateTime.Now,
                Activo = true,
                MedicoId = medicoId
            };

            _unidadTrabajo.PacienteTratamiento.Add(nuevoTratamiento);
            _unidadTrabajo.Guardar();

            TempData["Success"] = "Tratamiento asignado correctamente.";
            return RedirectToAction(nameof(Expediente), new { id = pacienteId });
        }

        // Agregar Medicamento
        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult AgregarMedicamento(int pacienteId, int medicamentoId)
        {
            var medicoId = User.FindFirstValue(ClaimTypes.NameIdentifier);

            var nuevoMedicamento = new PacienteMedicamento
            {
                PacienteId = pacienteId,
                MedicamentoId = medicamentoId,
                FechaAsignacion = DateTime.Now,
                Activo = true,
                MedicoId = medicoId
            };

            _unidadTrabajo.PacienteMedicamento.Add(nuevoMedicamento);
            _unidadTrabajo.Guardar();

            TempData["Success"] = "Medicamento recetado correctamente.";
            return RedirectToAction(nameof(Expediente), new { id = pacienteId });
        }

        //Suspender Asignaciones
        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult SuspenderPadecimiento(int id, int pacienteId)
        {
            var registro = _unidadTrabajo.PacientePadecimiento.Get(id);
            if (registro != null)
            {
                registro.Activo = false;
                _unidadTrabajo.Guardar();
                TempData["Success"] = "Padecimiento suspendido correctamente.";
            }
            return RedirectToAction(nameof(Expediente), new { id = pacienteId });
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult SuspenderTratamiento(int id, int pacienteId)
        {
            var registro = _unidadTrabajo.PacienteTratamiento.Get(id);
            if (registro != null)
            {
                registro.Activo = false;
                _unidadTrabajo.Guardar();
                TempData["Success"] = "Tratamiento suspendido correctamente.";
            }
            return RedirectToAction(nameof(Expediente), new { id = pacienteId });
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult SuspenderMedicamento(int id, int pacienteId)
        {
            var registro = _unidadTrabajo.PacienteMedicamento.Get(id);
            if (registro != null)
            {
                registro.Activo = false;
                _unidadTrabajo.Guardar();
                TempData["Success"] = "Medicamento suspendido correctamente.";
            }
            return RedirectToAction(nameof(Expediente), new { id = pacienteId });
        }

        //Subida Examenes
        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult SubirExamen(int pacienteId, string descripcion, IFormFile archivo)
        {
            if (archivo != null && archivo.Length > 0)
            {
                string wwwRootPath = _webHostEnvironment.WebRootPath;
                string extension = Path.GetExtension(archivo.FileName).ToLower();

                //extensiones permitidas requerimiento pdf e imagenes
                if (extension == ".pdf" || extension == ".jpg" || extension == ".jpeg" || extension == ".png")
                {
                    string nombreArchivo = Guid.NewGuid().ToString() + extension;
                    string rutaCarpeta = Path.Combine(wwwRootPath, @"archivos\examenes");

                    if (!Directory.Exists(rutaCarpeta))
                    {
                        Directory.CreateDirectory(rutaCarpeta);
                    }

                    using (var fileStream = new FileStream(Path.Combine(rutaCarpeta, nombreArchivo), FileMode.Create))
                    {
                        archivo.CopyTo(fileStream);
                    }

                    var medicoId = User.FindFirstValue(ClaimTypes.NameIdentifier);

                    var nuevoArchivo = new ArchivoExpediente
                    {
                        PacienteId = pacienteId,
                        NombreArchivo = archivo.FileName,
                        UrlArchivo = @"\archivos\examenes\" + nombreArchivo,
                        Descripcion = descripcion,
                        FechaSubida = DateTime.Now,
                        MedicoId = medicoId
                    };

                    _unidadTrabajo.ArchivoExpediente.Add(nuevoArchivo);
                    _unidadTrabajo.Guardar();

                    TempData["Success"] = "Examen guardado";
                }
                else
                {
                    TempData["Error"] = "Formato no permitido. Solo se aceptan PDFs o imágenes (.jpg, .jpeg, .png).";
                }
            }
            else
            {
                TempData["Error"] = "Por favor seleccione un archivo valido";
            }

            return RedirectToAction(nameof(Expediente), new { id = pacienteId });
        }
    }
}