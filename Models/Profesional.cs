using System;
using System.Collections.Generic;

namespace ClinicaSePrice.Models
{
    public class Profesional
    {
        public string Nombre { get; set; }
        public string Apellido { get; set; }
        public string Dni { get; set; }
        public string Matricula { get; set; }
        public string Especialidad { get; set; }
        public string Email { get; set; }

        public List<string> Horarios { get; set; } = new List<string>();

        public override string ToString()
        {
            return $"{Nombre} {Apellido} - {Especialidad}";
        }
    }
}
