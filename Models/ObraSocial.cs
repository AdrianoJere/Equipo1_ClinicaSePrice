namespace ClinicaSePrice.Models
{
    public class ObraSocial
    {
        public string Nombre { get; set; }
        public string Codigo { get; set; }
        public string Plan { get; set; }

        public override string ToString()
        {
            return $"{Nombre} - {Plan}";
        }
    }
}
