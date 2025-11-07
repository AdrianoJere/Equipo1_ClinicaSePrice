using System.Collections.Generic;
using System.Linq;
using ClinicaSePrice.Models;

namespace ClinicaSePrice.Services
{
    public class AuthService
    {
        private readonly List<Usuario> _usuarios;

        public AuthService()
        {
            _usuarios = new List<Usuario>
            {
                new Usuario("admin@seprice.com", "1234", "Administrador General", "Administrador"),
                new Usuario("recep@seprice.com", "1234", "Laura Torres", "Recepcionista"),
                new Usuario("medico@seprice.com", "1234", "Dr. Fernández", "Profesional")
            };
        }

        public Usuario ValidarCredenciales(string email, string password)
        {
            return _usuarios.FirstOrDefault(u =>
                u.Email.Equals(email, System.StringComparison.OrdinalIgnoreCase) &&
                u.Password == password);
        }
    }
}

