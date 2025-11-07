namespace ClinicaSePrice.Models
{
    public class Usuario
    {
        public string Email { get; set; }
        public string Password { get; set; }
        public string NombreCompleto { get; set; }
        public string Rol { get; set; }

        public Usuario(string email, string password, string nombre, string rol)
        {
            Email = email;
            Password = password;
            NombreCompleto = nombre;
            Rol = rol;
        }
    }
}
