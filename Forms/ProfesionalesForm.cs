using System;
using System.Drawing;
using System.Linq;
using System.Windows.Forms;
using ClinicaSePrice.Data;
using ClinicaSePrice.Models;

namespace ClinicaSePrice.Forms
{
    public class ProfesionalesForm : Form
    {
        private Label lblTitulo;
        private DataGridView dgv;
        private Button btnAgregar, btnEditar, btnEliminar, btnCerrar;

        public ProfesionalesForm()
        {
            InitUI();
            Cargar();
        }

        private void InitUI()
        {
            Text = "Profesionales - Clínica SePrice";
            BackColor = Color.WhiteSmoke;
            Font = new Font("Segoe UI", 10);
            ClientSize = new Size(800, 470);
            FormBorderStyle = FormBorderStyle.FixedDialog;
            StartPosition = FormStartPosition.CenterParent;

            lblTitulo = new Label
            {
                Text = "Gestión de Profesionales",
                Font = new Font("Segoe UI", 20, FontStyle.Bold),
                ForeColor = Color.FromArgb(45, 65, 200),
                Dock = DockStyle.Top,
                Height = 60,
                TextAlign = ContentAlignment.MiddleCenter
            };

            dgv = new DataGridView
            {
                Location = new Point(30, 80),
                Size = new Size(740, 300),
                ReadOnly = true,
                AllowUserToAddRows = false,
                SelectionMode = DataGridViewSelectionMode.FullRowSelect,
                AutoGenerateColumns = false,
                BackgroundColor = Color.White
            };
            dgv.Columns.Add(new DataGridViewTextBoxColumn { DataPropertyName = "Nombre", HeaderText = "Nombre", Width = 150 });
            dgv.Columns.Add(new DataGridViewTextBoxColumn { DataPropertyName = "Apellido", HeaderText = "Apellido", Width = 150 });
            dgv.Columns.Add(new DataGridViewTextBoxColumn { DataPropertyName = "Especialidad", HeaderText = "Especialidad", Width = 140 });
            dgv.Columns.Add(new DataGridViewTextBoxColumn { DataPropertyName = "Matricula", HeaderText = "Matrícula", Width = 100 });
            dgv.Columns.Add(new DataGridViewTextBoxColumn { DataPropertyName = "Email", HeaderText = "Email", Width = 150 });
            dgv.Columns.Add(new DataGridViewTextBoxColumn
            {
                HeaderText = "Horarios",
                DataPropertyName = "HorariosResumen",
                Width = 180
            });

            btnAgregar = Boton("Agregar", 30, 400, Color.FromArgb(0, 150, 90), (s, e) => AbrirEdicion(null));
            btnEditar = Boton("Editar", 220, 400, Color.FromArgb(45, 65, 200), (s, e) => Editar());
            btnEliminar = Boton("Eliminar", 410, 400, Color.FromArgb(220, 50, 60), (s, e) => Eliminar());
            btnCerrar = Boton("Cerrar", 600, 400, Color.Gray, (s, e) => Close());

            Controls.Add(lblTitulo);
            Controls.Add(dgv);
            Controls.AddRange(new Control[] { btnAgregar, btnEditar, btnEliminar, btnCerrar });
        }

        private Button Boton(string t, int x, int y, Color c, EventHandler onClick)
        {
            var b = new Button
            {
                Text = t,
                Location = new Point(x, y),
                Size = new Size(150, 40),
                BackColor = c,
                ForeColor = Color.White,
                FlatStyle = FlatStyle.Flat,
                Cursor = Cursors.Hand
            };
            b.FlatAppearance.BorderSize = 0;
            b.MouseEnter += (s, e) => b.BackColor = ControlPaint.Light(c);
            b.MouseLeave += (s, e) => b.BackColor = c;
            b.Click += onClick;
            return b;
        }

        private void Cargar()
        {
            dgv.DataSource = null;
            dgv.DataSource = DataStore.Profesionales
                .Select(p => new
                {
                    p.Nombre,
                    p.Apellido,
                    p.Especialidad,
                    p.Matricula,
                    p.Email,
                    HorariosResumen = p.Horarios != null && p.Horarios.Any()
                        ? string.Join(" | ", p.Horarios.Take(2)) + (p.Horarios.Count > 2 ? "..." : "")
                        : "Sin horarios"
                })
                .ToList();
        }

        private void AbrirEdicion(Profesional p)
        {
            using (var f = new Form())
            {
                f.Text = p == null ? "Nuevo profesional" : "Editar profesional";
                f.StartPosition = FormStartPosition.CenterParent;
                f.BackColor = Color.WhiteSmoke;
                f.ClientSize = new Size(460, 420);
                f.FormBorderStyle = FormBorderStyle.FixedDialog;
                f.MaximizeBox = false;
                f.Font = new Font("Segoe UI", 10);

                var lblT = new Label
                {
                    Text = f.Text,
                    Font = new Font("Segoe UI", 16, FontStyle.Bold),
                    ForeColor = Color.FromArgb(45, 65, 200),
                    Dock = DockStyle.Top,
                    Height = 50,
                    TextAlign = ContentAlignment.MiddleCenter
                };

                var lblNom = new Label { Text = "Nombre:", Left = 40, Top = 70 };
                var txtNom = new TextBox { Left = 150, Top = 66, Width = 230, Text = p?.Nombre ?? "" };

                var lblApe = new Label { Text = "Apellido:", Left = 40, Top = 110 };
                var txtApe = new TextBox { Left = 150, Top = 106, Width = 230, Text = p?.Apellido ?? "" };

                var lblEsp = new Label { Text = "Especialidad:", Left = 40, Top = 150 };
                var txtEsp = new TextBox { Left = 150, Top = 146, Width = 230, Text = p?.Especialidad ?? "" };

                var lblMat = new Label { Text = "Matrícula:", Left = 40, Top = 190 };
                var txtMat = new TextBox { Left = 150, Top = 186, Width = 230, Text = p?.Matricula ?? "" };

                var lblMail = new Label { Text = "Email:", Left = 40, Top = 230 };
                var txtMail = new TextBox { Left = 150, Top = 226, Width = 230, Text = p?.Email ?? "" };

                // ===== NUEVO BLOQUE: selección de horarios =====
                var lblHor = new Label { Text = "Horario:", Left = 40, Top = 270 };

                var cboDia = new ComboBox
                {
                    Left = 150,
                    Top = 266,
                    Width = 120,
                    DropDownStyle = ComboBoxStyle.DropDownList
                };
                cboDia.Items.AddRange(new[] { "Lunes", "Martes", "Miércoles", "Jueves", "Viernes", "Sábado" });
                cboDia.SelectedIndex = 0;

                var dtDesde = new DateTimePicker
                {
                    Left = 280,
                    Top = 266,
                    Format = DateTimePickerFormat.Time,
                    ShowUpDown = true,
                    Width = 70
                };
                var dtHasta = new DateTimePicker
                {
                    Left = 360,
                    Top = 266,
                    Format = DateTimePickerFormat.Time,
                    ShowUpDown = true,
                    Width = 70
                };

                var lstHorarios = new ListBox
                {
                    Left = 150,
                    Top = 300,
                    Width = 280,
                    Height = 60
                };
                if (p?.Horarios != null)
                    foreach (var h in p.Horarios)
                        lstHorarios.Items.Add(h);

                var btnAddHorario = new Button
                {
                    Text = "+",
                    Left = 40,
                    Top = 300,
                    Size = new Size(80, 30),
                    BackColor = Color.FromArgb(0, 150, 90),
                    ForeColor = Color.White,
                    FlatStyle = FlatStyle.Flat
                };
                btnAddHorario.FlatAppearance.BorderSize = 0;
                btnAddHorario.Click += (s, e) =>
                {
                    string horario = $"{cboDia.SelectedItem} {dtDesde.Value:HH:mm}-{dtHasta.Value:HH:mm}";
                    if (!lstHorarios.Items.Contains(horario))
                        lstHorarios.Items.Add(horario);
                };
                // ===============================================

                var btnOk = new Button
                {
                    Text = "Guardar",
                    Left = 150,
                    Top = 370,
                    Size = new Size(110, 30),
                    BackColor = Color.FromArgb(45, 65, 200),
                    ForeColor = Color.White,
                    FlatStyle = FlatStyle.Flat,
                    DialogResult = DialogResult.OK
                };
                btnOk.FlatAppearance.BorderSize = 0;

                var btnCan = new Button
                {
                    Text = "Cancelar",
                    Left = 270,
                    Top = 370,
                    Size = new Size(110, 30),
                    BackColor = Color.Gray,
                    ForeColor = Color.White,
                    FlatStyle = FlatStyle.Flat,
                    DialogResult = DialogResult.Cancel
                };
                btnCan.FlatAppearance.BorderSize = 0;

                f.Controls.AddRange(new Control[]
                {
            lblT, lblNom, txtNom, lblApe, txtApe, lblEsp, txtEsp, lblMat, txtMat,
            lblMail, txtMail, lblHor, cboDia, dtDesde, dtHasta, lstHorarios, btnAddHorario, btnOk, btnCan
                });
                f.AcceptButton = btnOk;
                f.CancelButton = btnCan;

                if (f.ShowDialog(this) == DialogResult.OK)
                {
                    if (string.IsNullOrWhiteSpace(txtNom.Text) || string.IsNullOrWhiteSpace(txtApe.Text) ||
                        string.IsNullOrWhiteSpace(txtEsp.Text) || string.IsNullOrWhiteSpace(txtMat.Text) ||
                        string.IsNullOrWhiteSpace(txtMail.Text))
                    {
                        MessageBox.Show("Complete todos los campos obligatorios.", "Advertencia",
                            MessageBoxButtons.OK, MessageBoxIcon.Warning);
                        return;
                    }

                    var horarios = lstHorarios.Items.Cast<string>().ToList();

                    if (p == null)
                    {
                        DataStore.Profesionales.Add(new Profesional
                        {
                            Nombre = txtNom.Text.Trim(),
                            Apellido = txtApe.Text.Trim(),
                            Especialidad = txtEsp.Text.Trim(),
                            Matricula = txtMat.Text.Trim(),
                            Email = txtMail.Text.Trim(),
                            Horarios = horarios
                        });
                    }
                    else
                    {
                        p.Nombre = txtNom.Text.Trim();
                        p.Apellido = txtApe.Text.Trim();
                        p.Especialidad = txtEsp.Text.Trim();
                        p.Matricula = txtMat.Text.Trim();
                        p.Email = txtMail.Text.Trim();
                        p.Horarios = horarios;
                    }

                    Cargar();
                    MessageBox.Show("Datos guardados correctamente.", "Éxito",
                        MessageBoxButtons.OK, MessageBoxIcon.Information);
                }
            }
        }


        private void Editar()
        {
            if (dgv.SelectedRows.Count == 0)
            {
                MessageBox.Show("Seleccione un profesional.", "Advertencia",
                    MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }
            var nombre = dgv.SelectedRows[0].Cells[0].Value.ToString();
            var p = DataStore.Profesionales.FirstOrDefault(x => x.Nombre == nombre);
            AbrirEdicion(p);
        }

        private void Eliminar()
        {
            if (dgv.SelectedRows.Count == 0)
            {
                MessageBox.Show("Seleccione un profesional.", "Advertencia",
                    MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            var nombre = dgv.SelectedRows[0].Cells[0].Value.ToString();
            var p = DataStore.Profesionales.FirstOrDefault(x => x.Nombre == nombre);

            var ok = MessageBox.Show($"¿Eliminar a {p?.Nombre} {p?.Apellido}?", "Confirmar",
                MessageBoxButtons.YesNo, MessageBoxIcon.Question);

            if (ok == DialogResult.Yes && p != null)
            {
                DataStore.Profesionales.Remove(p);
                Cargar();
                MessageBox.Show("Profesional eliminado.", "Información", MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
        }
    }
}
