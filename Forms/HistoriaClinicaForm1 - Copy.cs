using System;
using System.Drawing;
using System.Linq;
using System.Windows.Forms;
using ClinicaSePrice.Models;
using ClinicaSePrice.Data;
using ClinicaSePrice.Services;

namespace ClinicaSePrice.Forms
{
    public class HistoriaClinicaForm : Form
    {
        private Label lblTitulo;
        private ComboBox cboPaciente;
        private DataGridView dgv;
        private DateTimePicker dtpFecha;
        private TextBox txtMotivo, txtDiagnostico, txtIndicaciones;
        private Button btnGuardar, btnCerrar;

        public HistoriaClinicaForm()
        {
            InitUI();
            CargarPacientes();
        }

        private void InitUI()
        {
            this.Text = "Historia Clínica - Clínica SePrice";
            this.BackColor = Color.WhiteSmoke;
            this.Font = new Font("Segoe UI", 10);
            this.ClientSize = new Size(860, 560);
            this.FormBorderStyle = FormBorderStyle.FixedDialog;
            this.StartPosition = FormStartPosition.CenterParent;

            lblTitulo = new Label
            {
                Text = "Gestión de Historia Clínica",
                Font = new Font("Segoe UI", 20, FontStyle.Bold),
                ForeColor = Color.FromArgb(45, 65, 200),
                Dock = DockStyle.Top,
                Height = 60,
                TextAlign = ContentAlignment.MiddleCenter
            };

            // ===== CABECERA: Paciente =====
            var lblPac = new Label { Text = "Paciente:", Left = 30, Top = 82, AutoSize = true };
            cboPaciente = new ComboBox
            {
                Left = 110,
                Top = 80,                 // alineado con el label
                Width = 250,
                DropDownStyle = ComboBoxStyle.DropDownList
            };
            cboPaciente.SelectedIndexChanged += (s, e) => CargarHistorias();

            // ===== GRID =====
            dgv = new DataGridView
            {
                Location = new Point(30, 120),
                Size = new Size(800, 280),
                ReadOnly = true,
                AllowUserToAddRows = false,
                SelectionMode = DataGridViewSelectionMode.FullRowSelect,
                AutoGenerateColumns = false,
                BackgroundColor = Color.White
            };
            dgv.Columns.Add(new DataGridViewTextBoxColumn { HeaderText = "Fecha", DataPropertyName = "FechaAtencion", Width = 110, DefaultCellStyle = new DataGridViewCellStyle { Format = "dd/MM/yyyy" } });
            dgv.Columns.Add(new DataGridViewTextBoxColumn { HeaderText = "Motivo", DataPropertyName = "Motivo", Width = 210 });
            dgv.Columns.Add(new DataGridViewTextBoxColumn { HeaderText = "Diagnóstico", DataPropertyName = "Diagnostico", Width = 210 });
            dgv.Columns.Add(new DataGridViewTextBoxColumn { HeaderText = "Indicaciones", DataPropertyName = "Indicaciones", Width = 250 });

            // ===== SECCIÓN DE CARGA (alineada en la misma línea) =====
            int baseTop = 420;          // línea base para todos los controles de la fila
            var lblFecha = new Label { Text = "Fecha:", Left = 30, Top = baseTop, AutoSize = true };
            dtpFecha = new DateTimePicker
            {
                Left = 90,
                Top = baseTop,           // alineado con lblFecha
                Format = DateTimePickerFormat.Short,
                Width = 120
            };

            var lblMotivo = new Label { Text = "Motivo:", Left = 230, Top = baseTop, AutoSize = true };
            txtMotivo = new TextBox
            {
                Left = 290,
                Top = baseTop,           // alineado con lblMotivo
                Width = 180
            };

            var lblDiag = new Label { Text = "Diagnóstico:", Left = 490, Top = baseTop, AutoSize = true };
            txtDiagnostico = new TextBox
            {
                Left = 590,
                Top = baseTop,           // alineado con lblDiag
                Width = 240
            };

            // ===== Indicaciones (multilínea) =====
            var lblInd = new Label { Text = "Indicaciones:", Left = 30, Top = 460, AutoSize = true };
            txtIndicaciones = new TextBox
            {
                Left = 130,
                Top = 456,                // 4px arriba para centrar visualmente la altura del TextBox multilínea
                Width = 700,
                Height = 50,
                Multiline = true
            };

            // ===== Botones =====
            btnGuardar = new Button
            {
                Text = "Guardar",
                Left = 550,
                Top = 520,
                Size = new Size(120, 35),
                BackColor = Color.FromArgb(0, 150, 90),
                ForeColor = Color.White,
                FlatStyle = FlatStyle.Flat
            };
            btnGuardar.FlatAppearance.BorderSize = 0;
            btnGuardar.Click += BtnGuardar_Click;

            btnCerrar = new Button
            {
                Text = "Cerrar",
                Left = 690,
                Top = 520,
                Size = new Size(120, 35),
                BackColor = Color.Gray,
                ForeColor = Color.White,
                FlatStyle = FlatStyle.Flat
            };
            btnCerrar.FlatAppearance.BorderSize = 0;
            btnCerrar.Click += (s, e) => this.Close();

            this.Controls.AddRange(new Control[]
            {
        lblTitulo, lblPac, cboPaciente, dgv,
        lblFecha, dtpFecha, lblMotivo, txtMotivo,
        lblDiag, txtDiagnostico, lblInd, txtIndicaciones,
        btnGuardar, btnCerrar
            });
        }

        private void CargarPacientes()
        {
            cboPaciente.DataSource = DataStore.Pacientes.ToList();
            cboPaciente.DisplayMember = "NombreCompleto";
        }

        private void CargarHistorias()
        {
            var p = cboPaciente.SelectedItem as Paciente;
            if (p == null)
            {
                dgv.DataSource = null;
                return;
            }

            dgv.DataSource = null;
            dgv.DataSource = HistoriaClinicaService.ObtenerPorPaciente(p).ToList();
        }

        private void BtnGuardar_Click(object sender, EventArgs e)
        {
            var p = cboPaciente.SelectedItem as Paciente;
            if (p == null)
            {
                MessageBox.Show("Seleccione un paciente.", "Advertencia",
                    MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            if (string.IsNullOrWhiteSpace(txtMotivo.Text) ||
                string.IsNullOrWhiteSpace(txtDiagnostico.Text))
            {
                MessageBox.Show("Debe completar los campos Motivo y Diagnóstico.", "Advertencia",
                    MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            var fecha = dtpFecha.Value.Date;

            var yaExiste = HistoriaClinicaService
                .ObtenerPorPaciente(p)
                .Any(h => h.FechaAtencion.Date == fecha);

            if (yaExiste)
            {
                MessageBox.Show("Ya existe un registro para esta fecha.", "Conflicto",
                    MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            HistoriaClinicaService.AgregarRegistro(
                p,
                fecha,
                txtMotivo.Text.Trim(),
                txtDiagnostico.Text.Trim(),
                txtIndicaciones.Text.Trim()
            );

            txtMotivo.Clear();
            txtDiagnostico.Clear();
            txtIndicaciones.Clear();

            CargarHistorias();

            MessageBox.Show("Registro agregado correctamente.", "Éxito",
                MessageBoxButtons.OK, MessageBoxIcon.Information);
        }
    }
}
