using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Windows.Forms;
using ClinicaSePrice.Models;
using ClinicaSePrice.Data;

namespace ClinicaSePrice.Forms
{
    public class PacientesForm : Form
    {
        private Label lblTitulo;
        private DataGridView dgvPacientes;
        private TextBox txtBuscar;
        private Button btnNuevo, btnEditar, btnEliminar, btnCerrar;
        private List<Paciente> pacientes;

        public PacientesForm()
        {
            pacientes = DataStore.Pacientes;
            InitUI();
            CargarTabla();
        }

        private void InitUI()
        {
            Text = "Gestión de Pacientes - Clínica SePrice";
            BackColor = Color.WhiteSmoke;
            Font = new Font("Segoe UI", 10F);
            ClientSize = new Size(760, 460);
            FormBorderStyle = FormBorderStyle.FixedDialog;
            StartPosition = FormStartPosition.CenterScreen;

            lblTitulo = new Label
            {
                Text = "Gestión de Pacientes",
                Font = new Font("Segoe UI", 20F, FontStyle.Bold),
                ForeColor = Color.FromArgb(45, 65, 200),
                Dock = DockStyle.Top,
                Height = 60,
                TextAlign = ContentAlignment.MiddleCenter
            };

            txtBuscar = new TextBox
            {
                Left = 40,
                Top = 70,
                Width = 300,
                ForeColor = Color.Gray,
                Text = "Buscar por nombre/apellido..."
            };
            txtBuscar.GotFocus += (s, e) =>
            {
                if (txtBuscar.ForeColor == Color.Gray)
                {
                    txtBuscar.Text = "";
                    txtBuscar.ForeColor = Color.Black;
                }
            };
            txtBuscar.LostFocus += (s, e) =>
            {
                if (string.IsNullOrWhiteSpace(txtBuscar.Text))
                {
                    txtBuscar.Text = "Buscar por nombre/apellido...";
                    txtBuscar.ForeColor = Color.Gray;
                }
            };
            txtBuscar.TextChanged += (s, e) =>
            {
                if (txtBuscar.ForeColor == Color.Black)
                    Filtrar(txtBuscar.Text);
            };

            dgvPacientes = new DataGridView
            {
                Location = new Point(40, 110),
                Size = new Size(680, 250),
                ReadOnly = true,
                AllowUserToAddRows = false,
                SelectionMode = DataGridViewSelectionMode.FullRowSelect,
                AutoGenerateColumns = false,
                BackgroundColor = Color.White
            };

            dgvPacientes.Columns.Add(new DataGridViewTextBoxColumn { HeaderText = "Nombre", DataPropertyName = "Nombre", Width = 140 });
            dgvPacientes.Columns.Add(new DataGridViewTextBoxColumn { HeaderText = "Apellido", DataPropertyName = "Apellido", Width = 140 });
            dgvPacientes.Columns.Add(new DataGridViewTextBoxColumn { HeaderText = "DNI", DataPropertyName = "Dni", Width = 100 });
            dgvPacientes.Columns.Add(new DataGridViewTextBoxColumn { HeaderText = "Email", DataPropertyName = "Email", Width = 180 });
            dgvPacientes.Columns.Add(new DataGridViewTextBoxColumn { HeaderText = "Teléfono", DataPropertyName = "Telefono", Width = 100 });
            dgvPacientes.Columns.Add(new DataGridViewTextBoxColumn { HeaderText = "Obra Social", DataPropertyName = "ObraSocial", Width = 100 });
            dgvPacientes.Columns.Add(new DataGridViewTextBoxColumn { HeaderText = "Fecha Nacimiento", DataPropertyName = "FechaNacimiento", Width = 100 });

            btnNuevo = CrearBoton("Nuevo", 40, 380, Color.FromArgb(0, 150, 90));
            btnNuevo.Click += BtnNuevo_Click;

            btnEditar = CrearBoton("Editar", 190, 380, Color.FromArgb(45, 65, 200));
            btnEditar.Click += BtnEditar_Click;

            btnEliminar = CrearBoton("Eliminar", 340, 380, Color.FromArgb(220, 50, 60));
            btnEliminar.Click += BtnEliminar_Click;

            btnCerrar = CrearBoton("Cerrar", 490, 380, Color.Gray);
            btnCerrar.Click += (s, e) => Close();

            Controls.AddRange(new Control[] { lblTitulo, txtBuscar, dgvPacientes, btnNuevo, btnEditar, btnEliminar, btnCerrar });
        }

        private Button CrearBoton(string texto, int x, int y, Color color)
        {
            var boton = new Button
            {
                Text = texto,
                Location = new Point(x, y),
                Size = new Size(120, 40),
                BackColor = color,
                ForeColor = Color.White,
                FlatStyle = FlatStyle.Flat,
                Cursor = Cursors.Hand
            };
            boton.FlatAppearance.BorderSize = 0;
            return boton;
        }

        private void CargarTabla()
        {
            dgvPacientes.DataSource = null;
            dgvPacientes.DataSource = pacientes.ToList();
        }

        private void Filtrar(string texto)
        {
            texto = texto?.ToLower() ?? "";

            dgvPacientes.DataSource = pacientes
                .Where(p =>
                    (!string.IsNullOrEmpty(p.Nombre) && p.Nombre.ToLower().Contains(texto)) ||
                    (!string.IsNullOrEmpty(p.Apellido) && p.Apellido.ToLower().Contains(texto)))
                .ToList();
        }

        private void BtnNuevo_Click(object sender, EventArgs e)
        {
            var alta = new AltaPacienteForm();
            alta.ShowDialog();
            CargarTabla();
        }

        private void BtnEditar_Click(object sender, EventArgs e)
        {
            if (dgvPacientes.SelectedRows.Count == 0)
            {
                MessageBox.Show("Debe seleccionar un paciente.", "Advertencia",
                    MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            var paciente = dgvPacientes.SelectedRows[0].DataBoundItem as Paciente;
            if (paciente == null) return;

            // === Ventanita propia (mini formulario interno) ===
            Form f = new Form();
            f.Text = "Editar Paciente";
            f.StartPosition = FormStartPosition.CenterParent;
            f.Size = new Size(400, 400);
            f.FormBorderStyle = FormBorderStyle.FixedDialog;
            f.MaximizeBox = false;
            f.MinimizeBox = false;

            // Labels and TextBoxes for each field
            Label lblNombre = new Label { Text = "Nombre:", Left = 15, Top = 20, AutoSize = true };
            TextBox txtNombre = new TextBox { Left = 150, Top = 15, Width = 200, Text = paciente.Nombre };

            Label lblApellido = new Label { Text = "Apellido:", Left = 15, Top = 60, AutoSize = true };
            TextBox txtApellido = new TextBox { Left = 150, Top = 55, Width = 200, Text = paciente.Apellido };

            Label lblDni = new Label { Text = "DNI:", Left = 15, Top = 100, AutoSize = true };
            TextBox txtDni = new TextBox { Left = 150, Top = 95, Width = 200, Text = paciente.Dni };

            Label lblTelefono = new Label { Text = "Teléfono:", Left = 15, Top = 140, AutoSize = true };
            TextBox txtTelefono = new TextBox { Left = 150, Top = 135, Width = 200, Text = paciente.Telefono };

            Label lblFechaNacimiento = new Label { Text = "Fecha de Nacimiento:", Left = 15, Top = 180, AutoSize = true };
            DateTimePicker dtpFechaNacimiento = new DateTimePicker
            {
                Left = 150,
                Top = 175,
                Width = 200,
                Format = DateTimePickerFormat.Short,
                Value = paciente.FechaNacimiento
            };

            Label lblObraSocial = new Label { Text = "Obra Social:", Left = 15, Top = 220, AutoSize = true };
            TextBox txtObraSocial = new TextBox { Left = 150, Top = 215, Width = 200, Text = paciente.ObraSocial };

            Label lblEmail = new Label { Text = "Email:", Left = 15, Top = 260, AutoSize = true };
            TextBox txtEmail = new TextBox { Left = 150, Top = 255, Width = 200, Text = paciente.Email };

            // Buttons
            Button btnOk = new Button
            {
                Text = "Aceptar",
                Left = 150,
                Top = 300,
                Width = 80,
                DialogResult = DialogResult.OK
            };
            Button btnCancel = new Button
            {
                Text = "Cancelar",
                Left = 240,
                Top = 300,
                Width = 80,
                DialogResult = DialogResult.Cancel
            };

            // Add controls to the form
            f.Controls.AddRange(new Control[]
            {
        lblNombre, txtNombre,
        lblApellido, txtApellido,
        lblDni, txtDni,
        lblTelefono, txtTelefono,
        lblFechaNacimiento, dtpFechaNacimiento,
        lblObraSocial, txtObraSocial,
        lblEmail, txtEmail,
        btnOk, btnCancel
            });

            f.AcceptButton = btnOk;
            f.CancelButton = btnCancel;

            // Show the form and handle the result
            if (f.ShowDialog() == DialogResult.OK)
            {
                // Validate inputs (basic validation)
                if (string.IsNullOrWhiteSpace(txtNombre.Text) || string.IsNullOrWhiteSpace(txtApellido.Text))
                {
                    MessageBox.Show("El nombre y apellido son obligatorios.", "Error",
                        MessageBoxButtons.OK, MessageBoxIcon.Error);
                    return;
                }

                if (!DateTime.TryParse(dtpFechaNacimiento.Text, out DateTime fechaNacimiento))
                {
                    MessageBox.Show("La fecha de nacimiento no es válida.", "Error",
                        MessageBoxButtons.OK, MessageBoxIcon.Error);
                    return;
                }

                // Update the paciente object
                paciente.Nombre = txtNombre.Text.Trim();
                paciente.Apellido = txtApellido.Text.Trim();
                paciente.Dni = txtDni.Text.Trim();
                paciente.Telefono = txtTelefono.Text.Trim();
                paciente.FechaNacimiento = fechaNacimiento;
                paciente.ObraSocial = txtObraSocial.Text.Trim();
                paciente.Email = txtEmail.Text.Trim();

                // Refresh the DataGridView
                dgvPacientes.Refresh();

                MessageBox.Show("Paciente modificado correctamente.", "Éxito",
                    MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
        }


        private void BtnEliminar_Click(object sender, EventArgs e)
        {
            if (dgvPacientes.SelectedRows.Count == 0)
            {
                MessageBox.Show("Debe seleccionar un paciente.", "Advertencia", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            var seleccionado = dgvPacientes.SelectedRows[0].DataBoundItem as Paciente;

            if (seleccionado == null) return;

            var ok = MessageBox.Show(
                $"¿Desea eliminar a {seleccionado.Nombre} {seleccionado.Apellido}?",
                "Confirmar eliminación",
                MessageBoxButtons.YesNo,
                MessageBoxIcon.Question);

            if (ok == DialogResult.Yes)
            {
                pacientes.Remove(seleccionado);
                CargarTabla();
                MessageBox.Show("Paciente eliminado.", "Información", MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
        }
    }
}
