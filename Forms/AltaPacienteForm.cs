using System;
using System.Drawing;
using System.Linq;
using System.Windows.Forms;
using ClinicaSePrice.Data;
using ClinicaSePrice.Models;

namespace ClinicaSePrice.Forms
{
    public class AltaPacienteForm : Form
    {
        private Label lblTitulo;
        private TextBox txtNombre, txtApellido, txtDni, txtEmail, txtTelefono, txtDomicilio;
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
            ClientSize = new Size(520, 420);
            FormBorderStyle = FormBorderStyle.FixedDialog;
            StartPosition = FormStartPosition.CenterParent;

            lblTitulo = new Label
            {
                Text = "Alta de Paciente",
                Font = new Font("Segoe UI", 20, FontStyle.Bold),
                ForeColor = Color.FromArgb(45, 65, 200),
                Dock = DockStyle.Top,
                Height = 60,
                TextAlign = ContentAlignment.MiddleCenter
            };

            var lblN = new Label { Text = "Nombre:", Left = 40, Top = 90, AutoSize = true };
            txtNombre = new TextBox { Left = 150, Top = 85, Width = 300 };

            var lblA = new Label { Text = "Apellido:", Left = 40, Top = 130, AutoSize = true };
            txtApellido = new TextBox { Left = 150, Top = 125, Width = 300 };

            var lblD = new Label { Text = "DNI:", Left = 40, Top = 170, AutoSize = true };
            txtDni = new TextBox { Left = 150, Top = 165, Width = 150 };

            var lblF = new Label { Text = "Fecha Nacimiento:", Left = 40, Top = 210, AutoSize = true };
            dtpNacimiento = new DateTimePicker { Left = 190, Top = 205, Width = 160, Format = DateTimePickerFormat.Short };

            var lblE = new Label { Text = "Email:", Left = 40, Top = 250, AutoSize = true };
            txtEmail = new TextBox { Left = 150, Top = 245, Width = 300 };

            var lblT = new Label { Text = "Teléfono:", Left = 40, Top = 290, AutoSize = true };
            txtTelefono = new TextBox { Left = 150, Top = 285, Width = 180 };

            var lblDom = new Label { Text = "Domicilio:", Left = 40, Top = 330, AutoSize = true };
            txtDomicilio = new TextBox { Left = 150, Top = 325, Width = 300 };

            btnGuardar = new Button
            {
                Text = "Guardar",
                Left = 100,
                Top = 370,
                Size = new Size(140, 35),
                BackColor = Color.FromArgb(0, 150, 90),
                ForeColor = Color.White,
                FlatStyle = FlatStyle.Flat
            };
            btnGuardar.Click += (s, e) => Guardar();

            btnCancelar = new Button
            {
                Text = "Cancelar",
                Left = 280,
                Top = 370,
                Size = new Size(140, 35),
                BackColor = Color.Gray,
                ForeColor = Color.White,
                FlatStyle = FlatStyle.Flat
            };
            btnCancelar.Click += (s, e) => Close();

            Controls.AddRange(new Control[] {
                lblTitulo, lblN, txtNombre, lblA, txtApellido, lblD, txtDni, lblF, dtpNacimiento,
                lblE, txtEmail, lblT, txtTelefono, lblDom, txtDomicilio, btnGuardar, btnCancelar
            });
        }

        private void Guardar()
        {
            if (string.IsNullOrWhiteSpace(txtNombre.Text) ||
                string.IsNullOrWhiteSpace(txtApellido.Text) ||
                string.IsNullOrWhiteSpace(txtDni.Text))
            {
                MessageBox.Show("Complete los campos obligatorios (Nombre, Apellido, DNI).", "Advertencia",
                    MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            bool existe = DataStore.Pacientes.Any(p => p.Dni == txtDni.Text.Trim());
            if (existe)
            {
                MessageBox.Show("Ya existe un paciente con ese DNI.", "Error",
                    MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }

            var nuevo = new Paciente
            {
                Nombre = txtNombre.Text.Trim(),
                Apellido = txtApellido.Text.Trim(),
                Dni = txtDni.Text.Trim(),
                FechaNacimiento = dtpNacimiento.Value.Date,
                Email = txtEmail.Text.Trim(),
                Telefono = txtTelefono.Text.Trim(),
                Domicilio = txtDomicilio.Text.Trim()
            };

            DataStore.Pacientes.Add(nuevo);
            MessageBox.Show("Paciente registrado correctamente.", "Éxito",
                MessageBoxButtons.OK, MessageBoxIcon.Information);
            Close();
        }
    }
}
