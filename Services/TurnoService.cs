using System;
using System.Collections.Generic;
using System.Linq;
using ClinicaSePrice.Models;
using ClinicaSePrice.Data;

namespace ClinicaSePrice.Services
{
    public static class TurnoService
    {
        public static List<Turno> ObtenerTodos()
        {
            return DataStore.Turnos.ToList();
        }

        public static void RegistrarTurno(Paciente paciente, Profesional profesional, DateTime fechaHora, string observaciones)
        {
            if (paciente == null || profesional == null)
                throw new ArgumentException("Debe seleccionar paciente y profesional.");

            var existe = DataStore.Turnos.Any(t =>
                t.Profesional.Nombre == profesional.Nombre &&
                t.FechaHora == fechaHora &&
                t.Estado == "Programado");

            if (existe)
                throw new InvalidOperationException("Ya existe un turno para ese profesional en esa fecha y hora.");

            var nuevo = new Turno
            {
                Paciente = paciente,
                Profesional = profesional,
                FechaHora = fechaHora,
                Estado = "Programado",
                Observaciones = observaciones
            };

            DataStore.Turnos.Add(nuevo);
        }

        public static bool CancelarTurno(Turno turno)
        {
            if (turno == null)
                return false;

            turno.Estado = "Cancelado";
            return true;
        }

        public static List<Turno> Buscar(string criterio)
        {
            if (string.IsNullOrWhiteSpace(criterio))
                return ObtenerTodos();

            criterio = criterio.ToLower();

            return DataStore.Turnos
                .Where(t =>
                    t.Paciente.Nombre.ToLower().Contains(criterio) ||
                    t.Profesional.Nombre.ToLower().Contains(criterio) ||
                    t.Estado.ToLower().Contains(criterio))
                .ToList();
        }

        public static List<Turno> BuscarPorFecha(DateTime fecha)
        {
            return DataStore.Turnos
                .Where(t => t.FechaHora.Date == fecha.Date)
                .ToList();
        }

        public static List<Turno> ObtenerProximos()
        {
            return DataStore.Turnos
                .Where(t => t.FechaHora >= DateTime.Now && t.Estado == "Programado")
                .OrderBy(t => t.FechaHora)
                .ToList();
        }
    }
}
