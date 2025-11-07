using System.Collections.Generic;
using System.Linq;
using ClinicaSePrice.Models;

namespace ClinicaSePrice.Services
{
    public class PacienteService
    {
        private readonly List<Paciente> _pacientes = new List<Paciente>();

        public void Agregar(Paciente p)
        {
            if (_pacientes.Any(x => x.Dni == p.Dni))
                throw new System.Exception("Ya existe un paciente con ese DNI.");

            _pacientes.Add(p);
        }

        public List<Paciente> ObtenerTodos()
        {
            return _pacientes;
        }

        public void Eliminar(string dni)
        {
            var p = _pacientes.FirstOrDefault(x => x.Dni == dni);
            if (p != null) _pacientes.Remove(p);
        }

        public void Modificar(Paciente actualizado)
        {
            var p = _pacientes.FirstOrDefault(x => x.Dni == actualizado.Dni);
            if (p != null)
            {
                p.Nombre = actualizado.Nombre;
                p.Email = actualizado.Email;
                p.ObraSocial = actualizado.ObraSocial;
            }
        }
    }
}
