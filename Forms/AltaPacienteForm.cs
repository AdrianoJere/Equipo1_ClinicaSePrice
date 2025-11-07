using System;
using System.Drawing;
using System.Linq;
using System.Text.RegularExpressions;
using System.Windows.Forms;
using ClinicaSePrice.Data;
using ClinicaSePrice.Models;

namespace ClinicaSePrice.Forms
{
    public class AltaPacienteForm : Form
    {
        private TextBox txtNombre, txtApellido, txtDni, txtEmail, txtTelefono, txtObra;
        private DateTimePicker dtpNacimiento;
        private Button btnGuardar, btnCancelar;

        public AltaPacienteForm()
        {
            InitUI();
        }

        private void InitUI()
        {
            Text = "Alta de Paciente - Clínica SePrice";
            BackColor = Color.WhiteSmoke;
            Font = new Font("Segoe UI", 10);
            ClientSize = new Size(480, 420);
            FormBorderStyle = FormBorderStyle.FixedDialog;
            StartPosition = FormStartPosition.CenterParent;

            var lblTitulo = new Label
            {
                Text = "Alta de Paciente",
                Font = new Font("Segoe UI", 20, FontStyle.Bold),
                ForeColor = Color.FromArgb(45, 65, 200),
                Dock = DockStyle.Top,
                Height = 60,
                TextAlign = ContentAlignment.MiddleCenter
            };

            var lblN = new Label { Text = "Nombre:", Left = 40, Top = 90 };
            txtNombre = new TextBox { Left = 150, Top = 85, Width = 260 };

            var lblA = new Label { Text = "Apellido:", Left = 40, Top = 130 };
            txtApellido = new TextBox { Left = 150, Top = 125, Width = 260 };

            var lblD = new Label { Text = "DNI:", Left = 40, Top = 170 };
            txtDni = new TextBox { Left = 150, Top = 165, Width = 160 };

            var lblF = new Label { Text = "Fecha Nac.:", Left = 40, Top = 210 };
            dtpNacimiento = new DateTimePicker
            {
                Left = 150,    
                Top = 205,
                Width = 160,
                Format = DateTimePickerFormat.Short
            };
            var lblE = new Label { Text = "Email:", Left = 40, Top = 250 };
            txtEmail = new TextBox { Left = 150, Top = 245, Width = 260 };

            var lblT = new Label { Text = "Teléfono:", Left = 40, Top = 290 };
            txtTelefono = new TextBox { Left = 150, Top = 285, Width = 160 };

            var lblO = new Label { Text = "Obra Social:", Left = 40, Top = 330 };
            txtObra = new TextBox { Left = 150, Top = 325, Width = 260 };

            btnGuardar = new Button
            {
                Text = "Guardar",
                Left = 120,
                Top = 370,
                Size = new Size(120, 35),
                BackColor = Color.FromArgb(0, 150, 90),
                ForeColor = Color.White,
                FlatStyle = FlatStyle.Flat
            };
            btnGuardar.Click += (s, e) => Guardar();

            btnCancelar = new Button
            {
                Text = "Cancelar",
                Left = 260,
                Top = 370,
                Size = new Size(120, 35),
                BackColor = Color.Gray,
                ForeColor = Color.White,
                FlatStyle = FlatStyle.Flat
            };
            btnCancelar.Click += (s, e) => Close();

            Controls.AddRange(new Control[]
            {
                lblTitulo, lblN, txtNombre, lblA, txtApellido, lblD, txtDni, lblF, dtpNacimiento,
                lblE, txtEmail, lblT, txtTelefono, lblO, txtObra, btnGuardar, btnCancelar
            });
        }

        private bool EmailValido(string email)
        {
            if (string.IsNullOrWhiteSpace(email)) return true;

            return Regex.IsMatch(email,
                @"^[^@\s]+@[^@\s]+\.[^@\s]+$",
                RegexOptions.IgnoreCase);
        }

        private bool SoloNumeros(string texto)
        {
            return texto.All(char.IsDigit);
        }

        private void Guardar()
        {
            // -------- VALIDACIONES --------

            if (string.IsNullOrWhiteSpace(txtNombre.Text) ||
                string.IsNullOrWhiteSpace(txtApellido.Text) ||
                string.IsNullOrWhiteSpace(txtDni.Text))
            {
                MessageBox.Show("Nombre, Apellido y DNI son obligatorios.",
                    "Advertencia", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            if (!SoloNumeros(txtDni.Text))
            {
                MessageBox.Show("El DNI debe contener solo números.",
                    "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }

            if (!string.IsNullOrWhiteSpace(txtTelefono.Text) &&
                !SoloNumeros(txtTelefono.Text))
            {
                MessageBox.Show("El teléfono debe contener solo números.",
                    "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }

            if (!EmailValido(txtEmail.Text))
            {
                MessageBox.Show("Ingrese un Email válido.",
                    "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }

            if (dtpNacimiento.Value > DateTime.Today)
            {
                MessageBox.Show("La fecha de nacimiento no puede ser futura.",
                    "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }

            bool existe = DataStore.Pacientes.Any(p => p.Dni == txtDni.Text.Trim());
            if (existe)
            {
                MessageBox.Show("Ya existe un paciente con ese DNI.",
                    "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }

            // -------- CREAR PACIENTE --------

            var nuevo = new Paciente
            {
                Nombre = txtNombre.Text.Trim(),
                Apellido = txtApellido.Text.Trim(),
                Dni = txtDni.Text.Trim(),
                FechaNacimiento = dtpNacimiento.Value,
                Email = txtEmail.Text.Trim(),
                Telefono = txtTelefono.Text.Trim(),
                ObraSocial = txtObra.Text.Trim()
            };

            DataStore.Pacientes.Add(nuevo);

            MessageBox.Show("Paciente registrado correctamente.",
                "Éxito", MessageBoxButtons.OK, MessageBoxIcon.Information);

            Close();
        }
    }
}
