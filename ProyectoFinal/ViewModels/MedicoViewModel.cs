using Microsoft.AspNetCore.Mvc.Rendering;

namespace ProyectoFinal.Models
{
    public class MedicoViewModel
    {
        public Medico Medico { get; set; } = new Medico();

        public IEnumerable<SelectListItem>? EspecialidadLista { get; set; }

        public List<int> EspecialidadesId { get; set; } = new List<int>();
    }
}