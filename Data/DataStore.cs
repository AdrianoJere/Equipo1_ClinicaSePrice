using System;
using System.Collections.Generic;
using ClinicaSePrice.Models;

namespace ClinicaSePrice.Data
{
    public static class DataStore
    {
        public static List<Paciente> Pacientes = new List<Paciente>
        {
            new Paciente{ Nombre="Juan", Apellido="Perez", Dni="35111222", FechaNacimiento=new DateTime(1990,5,12), Email="juanperez@gmail.com", ObraSocial="OSDE", Telefono="222333"},
            new Paciente{ Nombre="María", Apellido="Gómez", Dni="40123123", FechaNacimiento=new DateTime(1987,2,22), Email="mariagomez@yahoo.com", ObraSocial="Swiss Medical", Telefono="222444"},
            new Paciente{ Nombre="José", Apellido="Rodriguez", Dni="50123123", FechaNacimiento=new DateTime(1981,6,21), Email="pepe@gmail.com", ObraSocial="OSDE", Telefono="222555"},
            new Paciente{ Nombre="Carlos", Apellido="García", Dni="60123123", FechaNacimiento=new DateTime(1994,12,1), Email="charly@hotmail.com", ObraSocial="particular", Telefono="222666"},
            new Paciente{ Nombre="Mirta", Apellido="Legrand", Dni="70123123", FechaNacimiento=new DateTime(1985,8,15), Email="mirta@yahoo.com", ObraSocial="PAMI", Telefono="222777"}
        };

        public static List<Profesional> Profesionales = new List<Profesional>
        {
            new Profesional{ Nombre="Dra. Laura Sosa", Especialidad="Clínica", Matricula="MP12345", Email="laura.sosa@seprice.com" },
            new Profesional{ Nombre="Dr. Martín Ruiz", Especialidad="Cardiología", Matricula="MP98765", Email="martin.ruiz@seprice.com" },
            new Profesional{ Nombre="Dra. Susana Martinez", Especialidad="Pediatra", Matricula="MP98765", Email="susana.martinez@seprice.com" },
            new Profesional{ Nombre="Dr. René Favaloro", Especialidad="Cirugía", Matricula="MP98765", Email="rene.favaloro@seprice.com" }
        };

        public static List<Turno> Turnos = new List<Turno>
        {
            new Turno{ Estado="Programado", FechaHora=new DateTime(2025,11,8,10,00,00), Paciente=new Paciente{ Nombre="Juan", Apellido="Pérez", Dni="35111222", FechaNacimiento=new DateTime(1990,5,12), Email="juanperez@gmail.com", ObraSocial="OSDE"}, Profesional=new Profesional{ Nombre="Dra. Laura Sosa", Especialidad="Clínica", Matricula="MP12345", Email="laura.sosa@seprice.com" }, Observaciones="urgente"},
            new Turno{ Estado="Programado", FechaHora=new DateTime(2025,11,8,10,20,00), Paciente=new Paciente{ Nombre="María", Apellido="Gómez", Dni="40123123", FechaNacimiento=new DateTime(1987,2,22), Email="mariagomez@yahoo.com", ObraSocial="Swiss Medical"}, Profesional=new Profesional{ Nombre="Dra. Laura Sosa", Especialidad="Clínica", Matricula="MP12345", Email="laura.sosa@seprice.com" }, Observaciones="ninguna"},
            new Turno{ Estado="Programado", FechaHora=new DateTime(2025,11,8,10,40,00), Paciente=new Paciente{ Nombre="José", Apellido="Rodriguez", Dni="50123123", FechaNacimiento=new DateTime(1981,6,21), Email="pepe@gmail.com", ObraSocial="OSDE"}, Profesional=new Profesional{ Nombre="Dra. Laura Sosa", Especialidad="Clínica", Matricula="MP12345", Email="laura.sosa@seprice.com" }, Observaciones="ninguna"},
            new Turno{ Estado="Programado", FechaHora=new DateTime(2025,11,8,11,00,00), Paciente=new Paciente{ Nombre="Carlos", Apellido="García", Dni="60123123", FechaNacimiento=new DateTime(1994,12,1), Email="charly@hotmail.com", ObraSocial="particular"}, Profesional=new Profesional{ Nombre="Dra. Laura Sosa", Especialidad="Clínica", Matricula="MP12345", Email="laura.sosa@seprice.com" }, Observaciones="ninguna"},
        };

        public static List<HistoriaClinica> Historias = new List<HistoriaClinica>();
    }
}
