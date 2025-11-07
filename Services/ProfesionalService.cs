using System.Collections.Generic;
using System.Linq;
using ClinicaSePrice.Models;

namespace ClinicaSePrice.Services
{
    public class ProfesionalService
    {
        private readonly List<Profesional> _profesionales = new List<Profesional>();

        public void Agregar(Profesional p)
        {
            if (_profesionales.Any(x => x.Matricula == p.Matricula))
                throw new System.Exception("Ya existe un profesional con esa matrícula.");

            _profesionales.Add(p);
        }

        public List<Profesional> ObtenerTodos()
        {
            return _profesionales;
        }

        public Profesional BuscarPorMatricula(string matricula)
        {
            return _profesionales.FirstOrDefault(x => x.Matricula == matricula);
        }
    }
}
