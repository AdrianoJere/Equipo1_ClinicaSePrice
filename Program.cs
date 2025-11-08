using System;
using System.Windows.Forms;

namespace ClinicaSePrice
{
    static class Program
    {
        [STAThread]
        static void Main()
        {
            // Configuración inicial del entorno de Windows Forms
            Application.EnableVisualStyles();
            Application.SetCompatibleTextRenderingDefault(false);

            // CU01 – Iniciar sesión
            // Se abre el formulario de login como punto de entrada
            //Application.Run(new Forms.LoginForm());
            Application.Run(new Forms.HistoriaClinicaForm());
        }
    }
}
