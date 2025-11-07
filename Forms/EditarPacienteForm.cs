using System;
using System.Drawing;
using System.Windows.Forms;
using ClinicaSePrice.Models;

namespace ClinicaSePrice
{
    public class EditarPacienteForm : Form
    {
        private Label lblTitulo, lblNombre, lblObraSocial, lblEmail;
        private TextBox txtNombre, txtObraSocial, txtEmail;
        private Button btnGuardar, btnCancelar;
        private readonly Paciente _paciente;

        public Paciente PacienteActualizado { get; private set; }

        public EditarPacienteForm(Paciente paciente)
        {
            _paciente = paciente;
            InicializarComponentes();
            CargarDatos();
        }

        private void InicializarComponentes()
        {
            // Configuración general
            this.Text = "Editar Paciente";
            this.StartPosition = FormStartPosition.CenterParent;
            this.BackColor = Color.WhiteSmoke;
            this.Size = new Size(420, 320);
            this.FormBorderStyle = FormBorderStyle.FixedDialog;
            this.MaximizeBox = false;
            this.Font = new Font("Segoe UI", 10);

            // Título
            lblTitulo = new Label()
            {
                Text = "Modificar Datos del Paciente",
                Font = new Font("Segoe UI", 16, FontStyle.Bold),
                ForeColor = Color.FromArgb(45, 65, 200),
                TextAlign = ContentAlignment.MiddleCenter,
                Dock = DockStyle.Top,
                Height = 60
            };

            // Nombre
            lblNombre = new Label() { Text = "Nombre:", Location = new Point(40, 80), AutoSize = true };
            txtNombre = new TextBox() { Location = new Point(160, 78), Size = new Size(200, 25) };

            // Obra Social
            lblObraSocial = new Label() { Text = "Obra Social:", Location = new Point(40, 120), AutoSize = true };
            txtObraSocial = new TextBox() { Location = new Point(160, 118), Size = new Size(200, 25) };

            // Email
            lblEmail = new Label() { Text = "Email:", Location = new Point(40, 160), AutoSize = true };
            txtEmail = new TextBox() { Location = new Point(160, 158), Size = new Size(200, 25) };

            // Botones
            btnGuardar = new Button()
            {
                Text = "Guardar",
                Location = new Point(100, 220),
                Size = new Size(100, 35),
                BackColor = Color.FromArgb(45, 65, 200),
                ForeColor = Color.White,
                FlatStyle = FlatStyle.Flat,
                Cursor = Cursors.Hand
            };
            btnGuardar.FlatAppearance.BorderSize = 0;
            btnGuardar.Click += BtnGuardar_Click;

            btnCancelar = new Button()
            {
                Text = "Cancelar",
                Location = new Point(220, 220),
                Size = new Size(100, 35),
                BackColor = Color.FromArgb(220, 50, 60),
                ForeColor = Color.White,
                FlatStyle = FlatStyle.Flat,
                Cursor = Cursors.Hand
            };
            btnCancelar.FlatAppearance.BorderSize = 0;
            btnCancelar.Click += BtnCancelar_Click;

            // Agregar controles
            this.Controls.Add(lblTitulo);
            this.Controls.Add(lblNombre);
            this.Controls.Add(txtNombre);
            this.Controls.Add(lblObraSocial);
            this.Controls.Add(txtObraSocial);
            this.Controls.Add(lblEmail);
            this.Controls.Add(txtEmail);
            this.Controls.Add(btnGuardar);
            this.Controls.Add(btnCancelar);
        }

        private void CargarDatos()
        {
            txtNombre.Text = _paciente.Nombre;
            txtObraSocial.Text = _paciente.ObraSocial;
            txtEmail.Text = _paciente.Email;
        }

        private void BtnGuardar_Click(object sender, EventArgs e)
        {
            if (string.IsNullOrWhiteSpace(txtNombre.Text) ||
                string.IsNullOrWhiteSpace(txtObraSocial.Text) ||
                string.IsNullOrWhiteSpace(txtEmail.Text))
            {
                MessageBox.Show("Todos los campos son obligatorios.", "Error",
                    MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }

            _paciente.Nombre = txtNombre.Text.Trim();
            _paciente.ObraSocial = txtObraSocial.Text.Trim();
            _paciente.Email = txtEmail.Text.Trim();
            PacienteActualizado = _paciente;

            MessageBox.Show("Datos modificados correctamente.", "Éxito",
                MessageBoxButtons.OK, MessageBoxIcon.Information);

            this.DialogResult = DialogResult.OK;
            this.Close();
        }

        private void BtnCancelar_Click(object sender, EventArgs e)
        {
            this.DialogResult = DialogResult.Cancel;
            this.Close();
        }
    }
}
