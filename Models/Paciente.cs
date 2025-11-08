using System;

namespace ClinicaSePrice.Models
{
    public class Paciente
    {
        public string Nombre { get; set; }
        public string Apellido { get; set; }
        public string Dni { get; set; }
        public DateTime FechaNacimiento { get; set; }
        public string Email { get; set; }
        public string Telefono { get; set; }
        public string Domicilio { get; set; }
        public string ObraSocial { get; set; }

        public int Edad => DateTime.Today.Year - FechaNacimiento.Year -
                           (DateTime.Today.DayOfYear < FechaNacimiento.DayOfYear ? 1 : 0);

        public string NombreCompleto => $"{Nombre} {Apellido}";

        public override string ToString()
        {
            return $"{Nombre} ({Dni})";
        }
    }
}
