using System;
using System.Drawing;
using System.Windows.Forms;
using ClinicaSePrice.Services;

namespace ClinicaSePrice.Forms
{
    public partial class LoginForm : Form
    {
        private Label lblTitulo, lblEmail, lblPass;
        private TextBox txtEmail, txtPass;
        private Button btnIngresar, btnCancelar;
        private readonly AuthService _auth = new AuthService();

        public LoginForm()
        {
            InitializeComponent();
        }

        private void InitializeComponent()
        {
            // Propiedades generales del formulario
            this.Text = "Login - Clínica SePrice";
            this.BackColor = Color.WhiteSmoke;
            this.Font = new Font("Segoe UI", 10F);
            this.ClientSize = new Size(520, 320);
            this.FormBorderStyle = FormBorderStyle.FixedDialog;
            this.MaximizeBox = false;
            this.StartPosition = FormStartPosition.CenterScreen;

            // ===== TÍTULO =====
            lblTitulo = new Label
            {
                Text = "Clínica SePrice",
                Font = new Font("Segoe UI", 20, FontStyle.Bold),
                ForeColor = Color.FromArgb(45, 65, 200),
                Dock = DockStyle.Top,
                Height = 60,
                TextAlign = ContentAlignment.MiddleCenter
            };

            // ===== EMAIL =====
            lblEmail = new Label { Text = "Email:", Location = new Point(110, 110), AutoSize = true };
            txtEmail = new TextBox { Location = new Point(180, 108), Width = 220 };

            // ===== CONTRASEÑA =====
            lblPass = new Label { Text = "Contraseña:", Location = new Point(90, 150), AutoSize = true };
            txtPass = new TextBox { Location = new Point(180, 148), Width = 220, PasswordChar = '●' };

            // ===== BOTONES =====
            btnIngresar = new Button
            {
                Text = "Ingresar",
                Location = new Point(180, 200),
                Size = new Size(100, 34),
                BackColor = Color.Black,
                ForeColor = Color.White,
                FlatStyle = FlatStyle.Flat
            };

            btnCancelar = new Button
            {
                Text = "Cancelar",
                Location = new Point(300, 200),
                Size = new Size(100, 34),
                BackColor = Color.Gainsboro,
                ForeColor = Color.Black,
                FlatStyle = FlatStyle.Flat
            };

            // Eventos
            btnIngresar.Click += btnIngresar_Click;
            btnCancelar.Click += (s, e) => this.Close();

            // Agregar controles
            this.Controls.Add(lblTitulo);
            this.Controls.Add(lblEmail);
            this.Controls.Add(txtEmail);
            this.Controls.Add(lblPass);
            this.Controls.Add(txtPass);
            this.Controls.Add(btnIngresar);
            this.Controls.Add(btnCancelar);
        }

        // ===== LÓGICA DE LOGIN =====
        private void btnIngresar_Click(object sender, EventArgs e)
        {
            var user = _auth.ValidarCredenciales(txtEmail.Text.Trim(), txtPass.Text.Trim());
            if (user == null)
            {
                MessageBox.Show("Email o contraseña incorrectos.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }

            // Si las credenciales son correctas, abrimos el menú principal
            var menu = new MenuPrincipalForm(user.NombreCompleto, user.Rol);
            menu.Show();
            this.Hide();
        }
    }
}
