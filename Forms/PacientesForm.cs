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

            // ===== Título =====
            lblTitulo = new Label
            {
                Text = "Gestión de Pacientes",
                Font = new Font("Segoe UI", 20F, FontStyle.Bold),
                ForeColor = Color.FromArgb(45, 65, 200),
                Dock = DockStyle.Top,
                Height = 60,
                TextAlign = ContentAlignment.MiddleCenter
            };

            // ===== Buscador =====
            txtBuscar = new TextBox
            {
                Left = 40,
                Top = 70,
                Width = 300,
                ForeColor = Color.Gray,
                Text = "Buscar por nombre..."
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
                    txtBuscar.Text = "Buscar por nombre...";
                    txtBuscar.ForeColor = Color.Gray;
                }
            };
            txtBuscar.TextChanged += (s, e) =>
            {
                if (txtBuscar.ForeColor == Color.Black)
                    Filtrar(txtBuscar.Text);
            };

            // ===== DataGridView =====
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
            dgvPacientes.Columns.Add(new DataGridViewTextBoxColumn { HeaderText = "Nombre", DataPropertyName = "Nombre", Width = 150 });
            dgvPacientes.Columns.Add(new DataGridViewTextBoxColumn { HeaderText = "Apellido", DataPropertyName = "Apellido", Width = 150 });
            dgvPacientes.Columns.Add(new DataGridViewTextBoxColumn { HeaderText = "DNI", DataPropertyName = "Dni", Width = 90 });
            dgvPacientes.Columns.Add(new DataGridViewTextBoxColumn { HeaderText = "Email", DataPropertyName = "Email", Width = 180 });
            dgvPacientes.Columns.Add(new DataGridViewTextBoxColumn { HeaderText = "Teléfono", DataPropertyName = "Telefono", Width = 100 });

            // ===== Botones =====
            btnNuevo = CrearBoton("Nuevo", 40, 380, Color.FromArgb(0, 150, 90));
            btnNuevo.Click += BtnNuevo_Click;

            btnEditar = CrearBoton("Editar", 190, 380, Color.FromArgb(45, 65, 200));
            btnEditar.Click += BtnEditar_Click;

            btnEliminar = CrearBoton("Eliminar", 340, 380, Color.FromArgb(220, 50, 60));
            btnEliminar.Click += BtnEliminar_Click;

            btnCerrar = CrearBoton("Cerrar", 490, 380, Color.Gray);
            btnCerrar.Click += (s, e) => Close();

            // ===== Controles =====
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
            boton.MouseEnter += (s, e) => boton.BackColor = ControlPaint.Light(color);
            boton.MouseLeave += (s, e) => boton.BackColor = color;
            return boton;
        }

        private void CargarTabla()
        {
            dgvPacientes.DataSource = null;
            dgvPacientes.DataSource = pacientes.ToList();
        }

        private void Filtrar(string texto)
        {
            if (string.IsNullOrWhiteSpace(texto))
            {
                dgvPacientes.DataSource = pacientes.ToList();
                return;
            }

            texto = texto.ToLower();

            dgvPacientes.DataSource = null;
            dgvPacientes.DataSource = pacientes
                .Where(p =>
                    (!string.IsNullOrEmpty(p.Nombre) && p.Nombre.ToLower().Contains(texto)) ||
                    (!string.IsNullOrEmpty(p.Apellido) && p.Apellido.ToLower().Contains(texto)))
                .ToList();
        }


        private void BtnNuevo_Click(object sender, EventArgs e)
        {
            var form = new Form
            {
                Text = "Alta de Paciente",
                Size = new Size(400, 420),
                StartPosition = FormStartPosition.CenterParent,
                BackColor = Color.WhiteSmoke,
                Font = new Font("Segoe UI", 10F)
            };

            var lblNombre = new Label { Text = "Nombre:", Left = 20, Top = 20 };
            var txtNombre = new TextBox { Left = 120, Top = 18, Width = 230 };

            var lblApellido = new Label { Text = "Apellido:", Left = 20, Top = 60 };
            var txtApellido = new TextBox { Left = 120, Top = 58, Width = 230 };

            var lblDni = new Label { Text = "DNI:", Left = 20, Top = 100 };
            var txtDni = new TextBox { Left = 120, Top = 98, Width = 230 };

            var lblFecha = new Label { Text = "Fecha Nac.:", Left = 20, Top = 140 };
            var dtpFecha = new DateTimePicker
            {
                Left = 120,
                Top = 136,
                Width = 230,
                Format = DateTimePickerFormat.Short
            };

            var lblTel = new Label { Text = "Teléfono:", Left = 20, Top = 180 };
            var txtTel = new TextBox { Left = 120, Top = 178, Width = 230 };

            var lblMail = new Label { Text = "Email:", Left = 20, Top = 220 };
            var txtMail = new TextBox { Left = 120, Top = 218, Width = 230 };

            var lblObra = new Label { Text = "Obra Social:", Left = 20, Top = 260 };
            var txtObra = new TextBox { Left = 120, Top = 258, Width = 230 };

            var btnOk = new Button
            {
                Text = "Guardar",
                Left = 180,
                Top = 320,
                Size = new Size(90, 35),
                BackColor = Color.FromArgb(0, 150, 90),
                ForeColor = Color.White,
                FlatStyle = FlatStyle.Flat,
                DialogResult = DialogResult.OK
            };
            btnOk.FlatAppearance.BorderSize = 0;

            var btnCancel = new Button
            {
                Text = "Cancelar",
                Left = 280,
                Top = 320,
                Size = new Size(90, 35),
                BackColor = Color.Gray,
                ForeColor = Color.White,
                FlatStyle = FlatStyle.Flat,
                DialogResult = DialogResult.Cancel
            };
            btnCancel.FlatAppearance.BorderSize = 0;

            form.Controls.AddRange(new Control[]
            {
        lblNombre, txtNombre,
        lblApellido, txtApellido,
        lblDni, txtDni,
        lblFecha, dtpFecha,
        lblTel, txtTel,
        lblMail, txtMail,
        lblObra, txtObra,
        btnOk, btnCancel
            });

            form.AcceptButton = btnOk;
            form.CancelButton = btnCancel;

            if (form.ShowDialog() == DialogResult.OK)
            {
                if (string.IsNullOrWhiteSpace(txtNombre.Text) ||
                    string.IsNullOrWhiteSpace(txtApellido.Text) ||
                    string.IsNullOrWhiteSpace(txtDni.Text))
                {
                    MessageBox.Show("Debe completar los campos obligatorios (Nombre, Apellido, DNI).",
                        "Advertencia", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    return;
                }

                var nuevo = new Paciente
                {
                    Nombre = txtNombre.Text.Trim(),
                    Apellido = txtApellido.Text.Trim(),
                    Dni = txtDni.Text.Trim(),
                    FechaNacimiento = dtpFecha.Value,
                    Telefono = txtTel.Text.Trim(),
                    Email = txtMail.Text.Trim(),
                    ObraSocial = txtObra.Text.Trim()
                };

                DataStore.Pacientes.Add(nuevo);
                CargarTabla();

                MessageBox.Show("Paciente agregado correctamente.", "Éxito",
                    MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
        }


        private void BtnEditar_Click(object sender, EventArgs e)
        {
            if (dgvPacientes.SelectedRows.Count == 0)
            {
                MessageBox.Show("Debe seleccionar un paciente.", "Advertencia", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            var seleccionado = dgvPacientes.SelectedRows[0].DataBoundItem as Paciente;
            if (seleccionado == null) return;

            var form = new Form
            {
                Text = "Modificar paciente",
                Size = new Size(350, 150),
                StartPosition = FormStartPosition.CenterParent
            };
            var lbl = new Label { Text = "Ingrese nuevo email:", Left = 15, Top = 20, AutoSize = true };
            var txt = new TextBox { Left = 15, Top = 45, Width = 300, Text = seleccionado.Email };
            var btnOk = new Button { Text = "Aceptar", Left = 160, Top = 80, DialogResult = DialogResult.OK };
            var btnCancel = new Button { Text = "Cancelar", Left = 245, Top = 80, DialogResult = DialogResult.Cancel };
            form.Controls.AddRange(new Control[] { lbl, txt, btnOk, btnCancel });
            form.AcceptButton = btnOk;
            form.CancelButton = btnCancel;

            if (form.ShowDialog() == DialogResult.OK && !string.IsNullOrWhiteSpace(txt.Text))
            {
                seleccionado.Email = txt.Text;
                dgvPacientes.Refresh();
                MessageBox.Show("Datos modificados correctamente.", "Éxito", MessageBoxButtons.OK, MessageBoxIcon.Information);
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

            var confirm = MessageBox.Show($"¿Desea eliminar al paciente {seleccionado.Nombre} {seleccionado.Apellido}?",
                                          "Confirmar eliminación",
                                          MessageBoxButtons.YesNo,
                                          MessageBoxIcon.Question);

            if (confirm == DialogResult.Yes)
            {
                pacientes.Remove(seleccionado);
                CargarTabla();
                MessageBox.Show("Paciente eliminado correctamente.", "Éxito", MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
        }
    }
}
