using System;
using System.Collections.Generic;
using System.Linq;
using ClinicaSePrice.Data;
using ClinicaSePrice.Models;

namespace ClinicaSePrice.Services
{
    public static class HistoriaClinicaService
    {
        public static IEnumerable<HistoriaClinica> ObtenerTodos()
            => DataStore.Historias;

        // Buscar por texto en el nombre del paciente (case-insensitive)
        public static IEnumerable<HistoriaClinica> BuscarPorPacienteNombre(string nombre)
        {
            if (string.IsNullOrWhiteSpace(nombre))
                return DataStore.Historias;

            return DataStore.Historias.Where(h =>
                h.Paciente != null &&
                !string.IsNullOrEmpty(h.Paciente.Nombre) &&
                h.Paciente.Nombre.IndexOf(nombre, StringComparison.OrdinalIgnoreCase) >= 0);
        }

        // Obtener todas las historias de un paciente (match por referencia o DNI)
        public static IEnumerable<HistoriaClinica> ObtenerRegistros(Paciente paciente)
        {
            if (paciente == null) return Enumerable.Empty<HistoriaClinica>();

            return DataStore.Historias.Where(h =>
                ReferenceEquals(h.Paciente, paciente) ||
                (h.Paciente != null && h.Paciente.Dni == paciente.Dni));
        }
        public static IEnumerable<HistoriaClinica> ObtenerPorPaciente(Paciente paciente)
        {
            if (paciente == null)
                return Enumerable.Empty<HistoriaClinica>();

            return DataStore.Historias
                .Where(h =>
                    h.Paciente != null &&
                    (ReferenceEquals(h.Paciente, paciente) ||
                     (!string.IsNullOrEmpty(h.Paciente.Dni) && h.Paciente.Dni == paciente.Dni)));
        }

        public static void AgregarRegistro(
            Paciente paciente,
            Profesional profesional,
            DateTime fechaAtencion,
            string motivo,
            string diagnostico,
            string indicaciones)
        {
            if (paciente == null)
                throw new ArgumentException("Paciente requerido.", nameof(paciente));

            DataStore.Historias.Add(new HistoriaClinica
            {
                Paciente = paciente,
                Profesional = profesional,
                FechaAtencion = fechaAtencion,
                Motivo = motivo,
                Diagnostico = diagnostico,
                Indicaciones = indicaciones
            });
        }
    }
}
