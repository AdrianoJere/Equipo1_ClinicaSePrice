using System;
using System.Drawing;
using System.Windows.Forms;
using ClinicaSePrice.Forms;

namespace ClinicaSePrice.Forms
{
    public class MenuPrincipalForm : Form
    {
        private Label lblTitulo, lblBienvenida;
        private Button btnTurnos, btnPacientes, btnHistoriaClinica, btnProfesionales, btnCerrarSesion;

        public MenuPrincipalForm(string nombreUsuario = "Admin", string rol = "Administrador")
        {
            InitializeComponent();
            lblBienvenida.Text = $"Bienvenido {nombreUsuario} ({rol})";
        }

        private void InitializeComponent()
        {
            // ===== CONFIGURACIÓN GENERAL =====
            Text = "Menú Principal - Clínica SePrice";
            BackColor = Color.WhiteSmoke;
            Font = new Font("Segoe UI", 10F);
            ClientSize = new Size(540, 370);
            FormBorderStyle = FormBorderStyle.FixedDialog;
            StartPosition = FormStartPosition.CenterScreen;
            MaximizeBox = false;

            // ===== TÍTULO =====
            lblTitulo = new Label
            {
                Text = "Clínica SePrice",
                Font = new Font("Segoe UI", 22, FontStyle.Bold),
                ForeColor = Color.FromArgb(45, 65, 200),
                Dock = DockStyle.Top,
                Height = 60,
                TextAlign = ContentAlignment.MiddleCenter
            };

            // ===== BIENVENIDA =====
            lblBienvenida = new Label
            {
                Text = "Bienvenido Admin",
                Font = new Font("Segoe UI", 11F),
                ForeColor = Color.Black,
                Location = new Point(25, 80),
                AutoSize = true
            };

            // ===== BOTONES =====
            btnTurnos = CrearBoton("Turnos", 100, 130);
            btnPacientes = CrearBoton("Pacientes", 280, 130);
            btnHistoriaClinica = CrearBoton("Historia Clínica", 100, 200);
            btnProfesionales = CrearBoton("Profesionales", 280, 200);

            // ===== EVENTOS DE NAVEGACIÓN =====
            btnTurnos.Click += (s, e) =>
            {
                using (var form = new TurnosForm())
                    form.ShowDialog(this);
            };

            btnPacientes.Click += (s, e) =>
            {
                using (var form = new PacientesForm())
                    form.ShowDialog(this);
            };

            btnHistoriaClinica.Click += (s, e) =>
            {
                using (var form = new HistoriaClinicaForm())
                    form.ShowDialog(this);
            };

            btnProfesionales.Click += (s, e) =>
            {
                using (var form = new ProfesionalesForm())
                    form.ShowDialog(this);
            };

            // ===== BOTÓN CERRAR SESIÓN =====
            btnCerrarSesion = new Button
            {
                Text = "Cerrar sesión",
                Font = new Font("Segoe UI", 10F, FontStyle.Bold),
                BackColor = Color.FromArgb(220, 50, 60),
                ForeColor = Color.White,
                FlatStyle = FlatStyle.Flat,
                Size = new Size(160, 45),
                Location = new Point(185, 280),
                Cursor = Cursors.Hand
            };
            btnCerrarSesion.FlatAppearance.BorderSize = 0;
            btnCerrarSesion.Click += BtnCerrarSesion_Click;

            // ===== AGREGAR CONTROLES =====
            Controls.Add(lblTitulo);
            Controls.Add(lblBienvenida);
            Controls.Add(btnTurnos);
            Controls.Add(btnPacientes);
            Controls.Add(btnHistoriaClinica);
            Controls.Add(btnProfesionales);
            Controls.Add(btnCerrarSesion);
        }

        // ===== CREACIÓN UNIFICADA DE BOTONES =====
        private Button CrearBoton(string texto, int x, int y)
        {
            var boton = new Button
            {
                Text = texto,
                Font = new Font("Segoe UI", 10F),
                BackColor = Color.White,
                ForeColor = Color.Black,
                FlatStyle = FlatStyle.Flat,
                Size = new Size(140, 55),
                Location = new Point(x, y),
                Cursor = Cursors.Hand
            };

            boton.FlatAppearance.BorderColor = Color.FromArgb(45, 65, 200);
            boton.FlatAppearance.BorderSize = 1;

            // Efecto hover visual
            boton.MouseEnter += (s, e) => boton.BackColor = Color.Gainsboro;
            boton.MouseLeave += (s, e) => boton.BackColor = Color.White;

            return boton;
        }

        // ===== CIERRE DE SESIÓN =====
        private void BtnCerrarSesion_Click(object sender, EventArgs e)
        {
            var confirm = MessageBox.Show("¿Desea cerrar sesión?", "Confirmar", MessageBoxButtons.YesNo, MessageBoxIcon.Question);
            if (confirm == DialogResult.Yes)
            {
                // Reinicia la aplicación, volviendo al Login
                Application.Restart();
            }
        }
    }
}
