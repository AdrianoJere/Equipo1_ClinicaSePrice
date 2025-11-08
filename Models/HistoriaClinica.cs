using System;

namespace ClinicaSePrice.Models
{
    public class HistoriaClinica
    {
        public int Id { get; set; }
        public DateTime FechaAtencion { get; set; }
        public Paciente Paciente { get; set; }
        public Profesional Profesional { get; set; }
        public string Diagnostico { get; set; }
        public string Motivo { get; set; }
        public string Indicaciones { get; set; }
    }
}
