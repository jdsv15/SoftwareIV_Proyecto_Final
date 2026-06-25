using Microsoft.AspNetCore.Mvc;
using ProyectoFinal.Repository;

namespace ProyectoFinal.Controllers.Api
{
    [Route("api/mobile/paciente")]
    [ApiController]
    public class PacienteMobileController : ControllerBase
    {
        private readonly IUnidadTrabajo _unidadTrabajo;

        public PacienteMobileController(IUnidadTrabajo unidadTrabajo)
        {
            _unidadTrabajo = unidadTrabajo;
        }

        [HttpGet("{cedula}/padecimientos")]
        public IActionResult GetPadecimientos(string cedula)
        {
            var paciente = _unidadTrabajo.Paciente.GetFirstOrDefault(p => p.Cedula == cedula);

            if (paciente == null)
                return NotFound(new { mensaje = "Paciente no encontrado." });

            var padecimientos = _unidadTrabajo.PacientePadecimiento
                .GetAll(p => p.PacienteId == paciente.Id, includeProperties: "Padecimiento")
                .Select(p => new
                {
                    p.Id,
                    Nombre = p.Padecimiento.Nombre,
                    FechaDiagnostico = p.FechaDiagnostico.ToString("dd/MM/yyyy"),
                    Estado = p.Activo ? "Activo" : "Suspendido"
                });

            return Ok(padecimientos);
        }

        [HttpGet("{cedula}/tratamientos")]
        public IActionResult GetTratamientos(string cedula)
        {
            var paciente = _unidadTrabajo.Paciente.GetFirstOrDefault(p => p.Cedula == cedula);

            if (paciente == null)
                return NotFound(new { mensaje = "Paciente no encontrado." });

            var tratamientos = _unidadTrabajo.PacienteTratamiento
                .GetAll(t => t.PacienteId == paciente.Id, includeProperties: "Tratamiento")
                .Select(t => new
                {
                    t.Id,
                    Nombre = t.Tratamiento.Nombre,
                    FechaAsignacion = t.FechaAsignacion.ToString("dd/MM/yyyy"),
                    Estado = t.Activo ? "Activo" : "Suspendido"
                });

            return Ok(tratamientos);
        }

        [HttpGet("{cedula}/medicamentos")]
        public IActionResult GetMedicamentos(string cedula)
        {
            var paciente = _unidadTrabajo.Paciente.GetFirstOrDefault(p => p.Cedula == cedula);

            if (paciente == null)
                return NotFound(new { mensaje = "Paciente no encontrado." });

            var medicamentos = _unidadTrabajo.PacienteMedicamento
                .GetAll(m => m.PacienteId == paciente.Id, includeProperties: "Medicamento")
                .Select(m => new
                {
                    m.Id,
                    Nombre = m.Medicamento.Nombre,
                    FechaAsignacion = m.FechaAsignacion.ToString("dd/MM/yyyy"),
                    Estado = m.Activo ? "Activo" : "Suspendido"
                });

            return Ok(medicamentos);
        }

        [HttpGet("{cedula}/archivos")]
        public IActionResult GetArchivos(string cedula)
        {
            var paciente = _unidadTrabajo.Paciente.GetFirstOrDefault(p => p.Cedula == cedula);

            if (paciente == null)
                return NotFound(new { mensaje = "Paciente no encontrado." });

            var archivos = _unidadTrabajo.ArchivoExpediente
                .GetAll(a => a.PacienteId == paciente.Id)
                .Select(a => new
                {
                    a.Id,
                    a.Descripcion,
                    a.NombreArchivo,
                    FechaSubida = a.FechaSubida.ToString("dd/MM/yyyy"),
                    UrlArchivo = $"{Request.Scheme}://{Request.Host}{a.UrlArchivo.Replace("\\", "/")}"
                });

            return Ok(archivos);
        }

        [HttpGet("{cedula}/historial")]
        public IActionResult GetHistorial(string cedula)
        {
            var paciente = _unidadTrabajo.Paciente.GetFirstOrDefault(p => p.Cedula == cedula);

            if (paciente == null)
                return NotFound(new { mensaje = "Paciente no encontrado." });

            var historial = _unidadTrabajo.HistorialClinico
                .GetAll(h => h.PacienteId == paciente.Id)
                .OrderByDescending(h => h.FechaHora)
                .Select(h => new
                {
                    h.Id,
                    FechaHora = h.FechaHora.ToString("dd/MM/yyyy hh:mm tt"),
                    Resumen = h.Notas.Length > 80 ? h.Notas.Substring(0, 80) + "..." : h.Notas
                });

            return Ok(historial);
        }

        [HttpGet("{cedula}/info")]
        public IActionResult GetPacienteInfo(string cedula)
        {
            var paciente = _unidadTrabajo.Paciente.GetFirstOrDefault(p => p.Cedula == cedula);

            if (paciente == null)
                return NotFound(new { mensaje = "Paciente no encontrado." });

            return Ok(new
            {
                paciente.Id,
                paciente.Cedula,
                paciente.Nombre
            });
        }

        [HttpGet("historial/detalle/{id}")]
        public IActionResult GetDetalleNota(int id)
        {
            var nota = _unidadTrabajo.HistorialClinico.Get(id);

            if (nota == null)
                return NotFound(new { mensaje = "Nota clínica no encontrada." });

            return Ok(new
            {
                nota.Id,
                FechaHora = nota.FechaHora.ToString("dd/MM/yyyy hh:mm tt"),
                nota.Notas
            });


        }
    }
}