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
        private DataGridView dgvHistorias;
        private ComboBox cboPacientes;
        private Button btnAgregar, btnVerDetalle, btnCerrar;

        public HistoriaClinicaForm()
        {
            InitUI();
            CargarPacientes();
        }

        private void InitUI()
        {
            Text = "Historia Clínica - Clínica SePrice";
            BackColor = Color.WhiteSmoke;
            Font = new Font("Segoe UI", 10);
            ClientSize = new Size(800, 500);
            FormBorderStyle = FormBorderStyle.FixedDialog;
            StartPosition = FormStartPosition.CenterParent;

            lblTitulo = new Label
            {
                Text = "Gestión de Historias Clínicas",
                Font = new Font("Segoe UI", 20, FontStyle.Bold),
                ForeColor = Color.FromArgb(45, 65, 200),
                Dock = DockStyle.Top,
                Height = 60,
                TextAlign = ContentAlignment.MiddleCenter
            };

            var lblPaciente = new Label
            {
                Text = "Paciente:",
                Left = 30,
                Top = 80,
                AutoSize = true
            };

            cboPacientes = new ComboBox
            {
                Left = 100,
                Top = 75,
                Width = 300,
                DropDownStyle = ComboBoxStyle.DropDownList
            };
            cboPacientes.SelectedIndexChanged += (s, e) => CargarHistorias();

            dgvHistorias = new DataGridView
            {
                Location = new Point(30, 120),
                Size = new Size(740, 280),
                ReadOnly = true,
                AllowUserToAddRows = false,
                SelectionMode = DataGridViewSelectionMode.FullRowSelect,
                AutoGenerateColumns = false,
                BackgroundColor = Color.White
            };
            dgvHistorias.Columns.Add(new DataGridViewTextBoxColumn { HeaderText = "Fecha", DataPropertyName = "FechaAtencion", Width = 120, DefaultCellStyle = new DataGridViewCellStyle { Format = "dd/MM/yyyy" } });
            dgvHistorias.Columns.Add(new DataGridViewTextBoxColumn { HeaderText = "Profesional", DataPropertyName = "Profesional", Width = 200 });
            dgvHistorias.Columns.Add(new DataGridViewTextBoxColumn { HeaderText = "Motivo", DataPropertyName = "Motivo", Width = 200 });
            dgvHistorias.Columns.Add(new DataGridViewTextBoxColumn { HeaderText = "Diagnóstico", DataPropertyName = "Diagnostico", Width = 200 });
            dgvHistorias.Columns.Add(new DataGridViewTextBoxColumn { HeaderText = "Indicaciones", DataPropertyName = "Indicaciones", Width = 200 });
            

            btnAgregar = CrearBoton("Agregar", 30, 420, Color.FromArgb(0, 150, 90), BtnAgregar_Click);
            btnVerDetalle = CrearBoton("Ver Detalle", 200, 420, Color.FromArgb(45, 65, 200), BtnVerDetalle_Click);
            btnCerrar = CrearBoton("Cerrar", 370, 420, Color.Gray, (s, e) => Close());

            Controls.AddRange(new Control[] { lblTitulo, lblPaciente, cboPacientes, dgvHistorias, btnAgregar, btnVerDetalle, btnCerrar });
        }

        private Button CrearBoton(string texto, int x, int y, Color color, EventHandler onClick)
        {
            var boton = new Button
            {
                Text = texto,
                Location = new Point(x, y),
                Size = new Size(150, 40),
                BackColor = color,
                ForeColor = Color.White,
                FlatStyle = FlatStyle.Flat,
                Cursor = Cursors.Hand
            };
            boton.FlatAppearance.BorderSize = 0;
            boton.Click += onClick;
            return boton;
        }

        private void CargarPacientes()
        {
            cboPacientes.DataSource = DataStore.Pacientes.ToList();
            cboPacientes.DisplayMember = "NombreCompleto";
        }

        private void CargarHistorias()
        {
            var paciente = cboPacientes.SelectedItem as Paciente;
            if (paciente == null)
            {
                dgvHistorias.DataSource = null;
                return;
            }

            dgvHistorias.DataSource = HistoriaClinicaService.ObtenerPorPaciente(paciente)
                .Select(h => new
                {
                    h.FechaAtencion,
                    h.Motivo,
                    h.Diagnostico,
                    h.Indicaciones,
                    Profesional = h.Profesional?.Nombre
                })
                .ToList();
        }

        private void BtnAgregar_Click(object sender, EventArgs e)
        {
            var paciente = cboPacientes.SelectedItem as Paciente;
            if (paciente == null)
            {
                MessageBox.Show("Seleccione un paciente.", "Advertencia", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            using (var form = new AgregarHistoriaClinicaForm(paciente))
            {
                if (form.ShowDialog(this) == DialogResult.OK)
                {
                    CargarHistorias();
                    MessageBox.Show("Historia clínica agregada correctamente.", "Éxito", MessageBoxButtons.OK, MessageBoxIcon.Information);
                }
            }
        }

        private void BtnVerDetalle_Click(object sender, EventArgs e)
        {
            if (dgvHistorias.SelectedRows.Count == 0)
            {
                MessageBox.Show("Seleccione una historia clínica.", "Advertencia", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            // Obtener el paciente seleccionado
            var pacienteSeleccionado = cboPacientes.SelectedItem as Paciente;

            if (pacienteSeleccionado == null)
            {
                MessageBox.Show("No se pudo determinar el paciente seleccionado.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }

       // Obtener la historia clínica seleccionada desde el DataGridView
            var historiaSeleccionada = dgvHistorias.SelectedRows[0].DataBoundItem;

            if (historiaSeleccionada == null)
            {
                MessageBox.Show("No se pudo determinar la historia clínica seleccionada.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }

            // Crear la instancia de HistoriaClinica a partir de la fila seleccionada
            var historia = new HistoriaClinica
            {
                FechaAtencion = (DateTime)historiaSeleccionada.GetType().GetProperty("FechaAtencion").GetValue(historiaSeleccionada),
                Motivo = (string)historiaSeleccionada.GetType().GetProperty("Motivo").GetValue(historiaSeleccionada),
                Diagnostico = (string)historiaSeleccionada.GetType().GetProperty("Diagnostico").GetValue(historiaSeleccionada),
                Indicaciones = (string)historiaSeleccionada.GetType().GetProperty("Indicaciones").GetValue(historiaSeleccionada),
                Profesional = DataStore.Profesionales.FirstOrDefault(p => p.Nombre == (string)historiaSeleccionada.GetType().GetProperty("Profesional").GetValue(historiaSeleccionada)),
                Paciente = pacienteSeleccionado
            };

            if (historia == null)
            {
                MessageBox.Show("No se pudo encontrar la historia clínica seleccionada.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }

            // Mostrar el formulario de detalle
            using (var form = new AgregarHistoriaClinicaForm(historia))
            {
                form.ShowDialog(this);
            }
        }

        // Embedded AgregarHistoriaClinicaForm
        private class AgregarHistoriaClinicaForm : Form
        {
            private DateTimePicker dtpFecha;
            private TextBox txtMotivo, txtDiagnostico, txtIndicaciones;
            private Button btnGuardar, btnCancelar, btnCerrar;
            private Paciente paciente;
            private ComboBox cboProfesionales;

            public DateTime Fecha { get; private set; }
            public string Motivo { get; private set; }
            public string Diagnostico { get; private set; }
            public string Indicaciones { get; private set; }
            public Profesional ProfesionalSeleccionado { get; private set; }

            // Constructor para agregar una nueva historia
            public AgregarHistoriaClinicaForm(Paciente paciente)
            {
                this.paciente = paciente;
                InitUI();
                CargarProfesionales();
            }

            // Constructor para ver el detalle de una historia existente
            public AgregarHistoriaClinicaForm(HistoriaClinica historia)
            {
                InitUI();
                MostrarDetalleHistoria(historia);
            }

            private void InitUI()
            {
                Text = paciente != null ? $"Nueva Historia Clínica - {paciente.NombreCompleto}" : "Detalle de Historia Clínica";
                BackColor = Color.WhiteSmoke;
                Font = new Font("Segoe UI", 10);
                ClientSize = new Size(400, 400);
                FormBorderStyle = FormBorderStyle.FixedDialog;
                StartPosition = FormStartPosition.CenterParent;

                var lblFecha = new Label { Text = "Fecha:", Left = 20, Top = 40, AutoSize = true };
                dtpFecha = new DateTimePicker
                {
                    Left = 120,
                    Top = 35,
                    Format = DateTimePickerFormat.Short,
                    Width = 250
                };

                var lblProfesional = new Label { Text = "Profesional:", Left = 20, Top = 80, AutoSize = true };
                cboProfesionales = new ComboBox
                {
                    Left = 120,
                    Top = 75,
                    Width = 250,
                    DropDownStyle = ComboBoxStyle.DropDownList
                };

                var lblMotivo = new Label { Text = "Motivo:", Left = 20, Top = 120, AutoSize = true };
                txtMotivo = new TextBox
                {
                    Left = 120,
                    Top = 115,
                    Width = 250
                };

                var lblDiagnostico = new Label { Text = "Diagnóstico:", Left = 20, Top = 160, AutoSize = true };
                txtDiagnostico = new TextBox
                {
                    Left = 120,
                    Top = 155,
                    Width = 250
                };

                var lblIndicaciones = new Label { Text = "Indicaciones:", Left = 20, Top = 200, AutoSize = true };
                txtIndicaciones = new TextBox
                {
                    Left = 120,
                    Top = 195,
                    Width = 250,
                    Height = 100,
                    Multiline = true
                };

                btnGuardar = new Button
                {
                    Text = "Guardar",
                    Left = 120,
                    Top = 320,
                    Size = new Size(100, 35),
                    BackColor = Color.FromArgb(0, 150, 90),
                    ForeColor = Color.White,
                    FlatStyle = FlatStyle.Flat,
                    DialogResult = DialogResult.OK
                };
                btnGuardar.Click += BtnGuardar_Click;

                btnCancelar = new Button
                {
                    Text = "Cancelar",
                    Left = 230,
                    Top = 320,
                    Size = new Size(100, 35),
                    BackColor = Color.Gray,
                    ForeColor = Color.White,
                    FlatStyle = FlatStyle.Flat,
                    DialogResult = DialogResult.Cancel
                };

                btnCerrar = new Button
                {
                    Text = "Cerrar",
                    Left = 120,
                    Top = 320,
                    Size = new Size(100, 35),
                    BackColor = Color.Gray,
                    ForeColor = Color.White,
                    FlatStyle = FlatStyle.Flat,
                    DialogResult = DialogResult.Cancel
                };

                Controls.AddRange(new Control[] { lblFecha, dtpFecha, lblProfesional, cboProfesionales, lblMotivo, txtMotivo, lblDiagnostico, txtDiagnostico, lblIndicaciones, txtIndicaciones, btnGuardar, btnCancelar, btnCerrar });
            }

            private void CargarProfesionales()
            {
                cboProfesionales.DataSource = DataStore.Profesionales.ToList();
                cboProfesionales.DisplayMember = "Nombre";
            }

            private void MostrarDetalleHistoria(HistoriaClinica historia)
            {
                // Set the form title
                Text = "Detalle de Historia Clínica";

                // Populate the fields with the historia data
                dtpFecha.Value = historia.FechaAtencion;
                txtMotivo.Text = historia.Motivo;
                txtDiagnostico.Text = historia.Diagnostico;
                txtIndicaciones.Text = historia.Indicaciones;

                // Set the selected professional
                cboProfesionales.DataSource = DataStore.Profesionales.ToList();
                cboProfesionales.DisplayMember = "Nombre";
                cboProfesionales.SelectedItem = historia.Profesional;


                // Hide the "Guardar" button
                btnGuardar.Visible = false;
                btnCancelar.Visible = false;
                btnCerrar.Visible = true;
            }

            private void BtnGuardar_Click(object sender, EventArgs e)
            {
                if (string.IsNullOrWhiteSpace(txtMotivo.Text) || string.IsNullOrWhiteSpace(txtDiagnostico.Text))
                {
                    MessageBox.Show("Debe completar los campos Motivo y Diagnóstico.", "Advertencia", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    DialogResult = DialogResult.None;
                    return;
                }

                if (paciente == null)
                {
                    MessageBox.Show("El paciente no está definido.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    DialogResult = DialogResult.None;
                    return;
                }

                if (cboProfesionales.SelectedItem == null)
                {
                    MessageBox.Show("Debe seleccionar un profesional.", "Advertencia", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    DialogResult = DialogResult.None;
                    return;
                }

                Fecha = dtpFecha.Value.Date;
                Motivo = txtMotivo.Text.Trim();
                Diagnostico = txtDiagnostico.Text.Trim();
                Indicaciones = txtIndicaciones.Text.Trim();
                ProfesionalSeleccionado = cboProfesionales.SelectedItem as Profesional;

                // Add the new record to the data source
                HistoriaClinicaService.AgregarRegistro(paciente, ProfesionalSeleccionado, Fecha, Motivo, Diagnostico, Indicaciones);
            }
        }
    }
}
