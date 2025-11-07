using System;
using System.Collections.Generic;
using ClinicaSePrice.Models;

namespace ClinicaSePrice.Data
{
    public static class DataStore
    {
        public static List<Paciente> Pacientes = new List<Paciente>
        {
            new Paciente{ Nombre="Juan Pérez", Dni="35111222", FechaNacimiento=new DateTime(1990,5,12), Email="juanperez@gmail.com", ObraSocial="OSDE"},
            new Paciente{ Nombre="María Gómez", Dni="40123123", FechaNacimiento=new DateTime(1987,2,22), Email="mariagomez@yahoo.com", ObraSocial="Swiss Medical"}
        };

        public static List<Profesional> Profesionales = new List<Profesional>
        {
            new Profesional{ Nombre="Dra. Laura Sosa", Especialidad="Clínica", Matricula="MP12345", Email="laura.sosa@seprice.com" },
            new Profesional{ Nombre="Dr. Martín Ruiz", Especialidad="Cardiología", Matricula="MP98765", Email="martin.ruiz@seprice.com" }
        };

        public static List<Turno> Turnos = new List<Turno>();

        public static List<HistoriaClinica> Historias = new List<HistoriaClinica>();
    }
}
