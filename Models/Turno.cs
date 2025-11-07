using System;

namespace ClinicaSePrice.Models
{
    public class Turno
    {
        public DateTime FechaHora { get; set; }
        public string Estado { get; set; } // Programado | Cancelado | Atendido
        public Paciente Paciente { get; set; }
        public Profesional Profesional { get; set; }
        public string Observaciones { get; set; }

    }
}
