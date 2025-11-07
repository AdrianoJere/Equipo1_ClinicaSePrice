using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Windows.Forms;
using ClinicaSePrice.Data;
using ClinicaSePrice.Models;

namespace ClinicaSePrice.Forms
{
    public class TurnosForm : Form
    {
        // ---- UI ----
        private Label lblTitulo;
        private DataGridView dgv;

        // Filtros
        private ComboBox cboFiltroProfesional;
        private CheckBox chkFiltroFecha;
        private DateTimePicker dtpFiltroFecha;

        // Alta
        private ComboBox cboPaciente, cboProfesional;
        private DateTimePicker dtpFecha, dtpHora;
        private TextBox txtObs;
        private Button btnReservar, btnCancelarTurno, btnCerrar;

        // ViewModel para el grid (referencia directa al Turno)
        private class TurnoRow
        {
            public DateTime FechaHora { get; set; }
            public string Estado { get; set; }
            public string PacienteNombre { get; set; }
            public string ProfesionalNombre { get; set; }
            public string Observaciones { get; set; }
            public Turno Ref { get; set; }   // <- referencia al Turno original
        }

        public TurnosForm()
        {
            InitUI();
            CargarCombos();
            CargarTabla();
        }

        private void InitUI()
        {
            Text = "Gestión de Turnos - Clínica SePrice";
            BackColor = Color.WhiteSmoke;
            Font = new Font("Segoe UI", 10);
            ClientSize = new Size(920, 580);
            FormBorderStyle = FormBorderStyle.FixedDialog;
            StartPosition = FormStartPosition.CenterParent;
            MaximizeBox = false;

            lblTitulo = new Label
            {
                Text = "Gestión de Turnos",
                Font = new Font("Segoe UI", 20, FontStyle.Bold),
                ForeColor = Color.FromArgb(45, 65, 200),
                Dock = DockStyle.Top,
                Height = 60,
                TextAlign = ContentAlignment.MiddleCenter
            };

            // ----- Filtros -----
            var pnlFiltros = new Panel
            {
                Left = 20,
                Top = 70,
                Width = 880,
                Height = 36
            };

            var lblProf = new Label { Text = "Filtrar por profesional:", Left = 0, Top = 8, AutoSize = true };
            cboFiltroProfesional = new ComboBox
            {
                Left = 145,
                Top = 4,
                Width = 220,
                DropDownStyle = ComboBoxStyle.DropDownList
            };
            cboFiltroProfesional.SelectedIndexChanged += FiltrosChanged;

            chkFiltroFecha = new CheckBox { Left = 390, Top = 7, Text = "Por fecha:", AutoSize = true };
            chkFiltroFecha.CheckedChanged += FiltrosChanged;

            dtpFiltroFecha = new DateTimePicker
            {
                Left = 470,
                Top = 4,
                Width = 140,
                Format = DateTimePickerFormat.Short
            };
            dtpFiltroFecha.ValueChanged += FiltrosChanged;

            pnlFiltros.Controls.Add(lblProf);
            pnlFiltros.Controls.Add(cboFiltroProfesional);
            pnlFiltros.Controls.Add(chkFiltroFecha);
            pnlFiltros.Controls.Add(dtpFiltroFecha);

            // ----- Grid -----
            dgv = new DataGridView
            {
                Left = 20,
                Top = 115,
                Width = 880,
                Height = 280,
                ReadOnly = true,
                AllowUserToAddRows = false,
                SelectionMode = DataGridViewSelectionMode.FullRowSelect,
                AutoGenerateColumns = false,
                BackgroundColor = Color.White
            };

            // Columnas con Name y DataPropertyName (para evitar errores por nombre)
            var colFH = new DataGridViewTextBoxColumn
            {
                Name = "FechaHora",
                DataPropertyName = "FechaHora",
                HeaderText = "Fecha/Hora",
                Width = 160,
                DefaultCellStyle = new DataGridViewCellStyle { Format = "dd/MM/yyyy HH:mm" }
            };
            var colEstado = new DataGridViewTextBoxColumn
            {
                Name = "Estado",
                DataPropertyName = "Estado",
                HeaderText = "Estado",
                Width = 110
            };
            var colPac = new DataGridViewTextBoxColumn
            {
                Name = "PacienteNombre",
                DataPropertyName = "PacienteNombre",
                HeaderText = "Paciente",
                Width = 220
            };
            var colProf = new DataGridViewTextBoxColumn
            {
                Name = "ProfesionalNombre",
                DataPropertyName = "ProfesionalNombre",
                HeaderText = "Profesional",
                Width = 220
            };
            var colObs = new DataGridViewTextBoxColumn
            {
                Name = "Observaciones",
                DataPropertyName = "Observaciones",
                HeaderText = "Observaciones",
                Width = 150
            };

            dgv.Columns.AddRange(new DataGridViewColumn[] { colFH, colEstado, colPac, colProf, colObs });

            // ----- Panel de alta -----
            var panel = new GroupBox
            {
                Text = "Nuevo turno",
                Left = 20,
                Top = 410,
                Width = 880,
                Height = 120
            };

            var lblP = new Label { Text = "Paciente:", Left = 15, Top = 30, AutoSize = true };
            cboPaciente = new ComboBox { Left = 85, Top = 26, Width = 220, DropDownStyle = ComboBoxStyle.DropDownList };

            var lblPr = new Label { Text = "Profesional:", Left = 330, Top = 30, AutoSize = true };
            cboProfesional = new ComboBox { Left = 415, Top = 26, Width = 220, DropDownStyle = ComboBoxStyle.DropDownList };

            var lblF = new Label { Text = "Fecha:", Left = 15, Top = 72, AutoSize = true };
            dtpFecha = new DateTimePicker { Left = 70, Top = 68, Format = DateTimePickerFormat.Short, Width = 120 };

            var lblH = new Label { Text = "Hora:", Left = 200, Top = 72, AutoSize = true };
            dtpHora = new DateTimePicker { Left = 240, Top = 68, Format = DateTimePickerFormat.Time, ShowUpDown = true, Width = 90 };

            var lblO = new Label { Text = "Obs.:", Left = 345, Top = 72, AutoSize = true };
            txtObs = new TextBox { Left = 385, Top = 68, Width = 250 };

            btnReservar = new Button
            {
                Text = "Reservar",
                Left = 660,
                Top = 64,
                Size = new Size(180, 34),
                BackColor = Color.FromArgb(0, 150, 90),
                ForeColor = Color.White,
                FlatStyle = FlatStyle.Flat
            };
            btnReservar.FlatAppearance.BorderSize = 0;
            btnReservar.Click += delegate { Reservar(); };

            panel.Controls.AddRange(new Control[]
            {
                lblP, cboPaciente, lblPr, cboProfesional, lblF, dtpFecha, lblH, dtpHora, lblO, txtObs, btnReservar
            });

            // ----- Botones inferior -----
            btnCancelarTurno = new Button
            {
                Text = "Cancelar turno",
                Left = 20,
                Top = 540,
                Size = new Size(150, 30),
                BackColor = Color.FromArgb(220, 50, 60),
                ForeColor = Color.White,
                FlatStyle = FlatStyle.Flat
            };
            btnCancelarTurno.FlatAppearance.BorderSize = 0;
            btnCancelarTurno.Click += delegate { CancelarTurno(); };

            var btnAcreditar = new Button
            {
                Text = "Confirmar turno",
                Left = 180,
                Top = 540,
                Size = new Size(150, 30),
                BackColor = Color.FromArgb(0, 120, 200),
                ForeColor = Color.White,
                FlatStyle = FlatStyle.Flat
            };
            btnAcreditar.FlatAppearance.BorderSize = 0;
            btnAcreditar.Click += (s, e) => AcreditarTurno();

            Controls.Add(btnAcreditar);

            btnCerrar = new Button
            {
                Text = "Cerrar",
                Left = 750,
                Top = 540,
                Size = new Size(150, 30),
                BackColor = Color.Gray,
                ForeColor = Color.White,
                FlatStyle = FlatStyle.Flat
            };
            btnCerrar.FlatAppearance.BorderSize = 0;
            btnCerrar.Click += delegate { Close(); };

            // Add controls
            Controls.Add(lblTitulo);
            Controls.Add(pnlFiltros);
            Controls.Add(dgv);
            Controls.Add(panel);
            Controls.Add(btnCancelarTurno);
            Controls.Add(btnCerrar);
        }

        private void CargarCombos()
        {
            // Alta
            cboPaciente.DataSource = DataStore.Pacientes.ToList();
            cboPaciente.DisplayMember = "Nombre";
            cboProfesional.DataSource = DataStore.Profesionales.ToList();
            cboProfesional.DisplayMember = "Nombre";

            // Filtro profesional (agrego "Todos")
            var listaProf = new List<Profesional>();
            listaProf.Add(new Profesional { Nombre = "(Todos)" });
            listaProf.AddRange(DataStore.Profesionales);
            cboFiltroProfesional.DataSource = listaProf;
            cboFiltroProfesional.DisplayMember = "Nombre";
            cboFiltroProfesional.SelectedIndex = 0;

            dtpFiltroFecha.Value = DateTime.Today;
        }

        private void FiltrosChanged(object sender, EventArgs e)
        {
            CargarTabla();
        }

        private void CargarTabla()
        {
            Profesional profSel = cboFiltroProfesional.SelectedItem as Profesional;
            bool filtrarFecha = chkFiltroFecha.Checked;
            DateTime fechaSel = dtpFiltroFecha.Value.Date;

            var query = DataStore.Turnos.AsEnumerable();

            if (profSel != null && profSel.Nombre != "(Todos)")
                query = query.Where(t => t.Profesional != null && t.Profesional.Nombre == profSel.Nombre);

            if (filtrarFecha)
                query = query.Where(t => t.FechaHora.Date == fechaSel);

            var lista = query
                .OrderBy(t => t.FechaHora)
                .Select(t => new TurnoRow
                {
                    FechaHora = t.FechaHora,
                    Estado = t.Estado,
                    PacienteNombre = t.Paciente != null ? t.Paciente.Nombre : "",
                    ProfesionalNombre = t.Profesional != null ? t.Profesional.Nombre : "",
                    Observaciones = t.Observaciones,
                    Ref = t
                })
                .ToList();

            dgv.DataSource = null;
            dgv.DataSource = lista;
        }
        private void Reservar()
        {
            if (cboPaciente.SelectedItem == null || cboProfesional.SelectedItem == null)
            {
                MessageBox.Show("Seleccione paciente y profesional.", "Advertencia",
                    MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            // Fecha válida
            if (dtpFecha.Value.Date < DateTime.Today)
            {
                MessageBox.Show("La fecha del turno no puede ser anterior a hoy.", "Error",
                    MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }

            DateTime fh = dtpFecha.Value.Date + dtpHora.Value.TimeOfDay;

            if (fh < DateTime.Now)
            {
                MessageBox.Show("La hora del turno debe ser futura.", "Error",
                    MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }

            // Observación obligatoria
            if (string.IsNullOrWhiteSpace(txtObs.Text))
            {
                MessageBox.Show("Debe ingresar una observación.", "Advertencia",
                    MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            Profesional prof = (Profesional)cboProfesional.SelectedItem;

            // Validar superposición CORREGIDA
            bool existe = DataStore.Turnos.Any(t =>
                t.Profesional == prof &&
                t.FechaHora == fh &&
                (t.Estado == "Programado" || t.Estado == "Confirmado"));

            if (existe)
            {
                MessageBox.Show("Ya existe un turno para ese profesional en esa fecha y hora.",
                    "Conflicto", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            DataStore.Turnos.Add(new Turno
            {
                FechaHora = fh,
                Estado = "Programado",
                Paciente = (Paciente)cboPaciente.SelectedItem,
                Profesional = prof,
                Observaciones = txtObs.Text.Trim()
            });

            CargarTabla();
            MessageBox.Show("Turno registrado correctamente.", "Éxito",
                MessageBoxButtons.OK, MessageBoxIcon.Information);
        }

        private void AcreditarTurno()
        {
            if (dgv.SelectedRows.Count == 0)
            {
                MessageBox.Show("Seleccione un turno.", "Advertencia",
                    MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            var vm = dgv.SelectedRows[0].DataBoundItem as TurnoRow;

            if (vm.Ref.Estado == "Cancelado")
            {
                MessageBox.Show("No puede acreditar un turno cancelado.", "Advertencia",
                    MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            if (vm.Ref.Estado == "Confirmado")
            {
                MessageBox.Show("Este turno ya está confirmado.", "Información",
                    MessageBoxButtons.OK, MessageBoxIcon.Information);
                return;
            }

            if (vm.Ref.FechaHora > DateTime.Now)
            {
                MessageBox.Show("No puede acreditar un turno antes de la hora programada.",
                    "Advertencia", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            if (MessageBox.Show("¿Confirmar el turno seleccionado?",
                "Confirmar acreditación",
                MessageBoxButtons.YesNo,
                MessageBoxIcon.Question) == DialogResult.Yes)
            {
                vm.Ref.Estado = "Confirmado";
                CargarTabla();

                MessageBox.Show("Turno confirmado correctamente.", "Información",
                    MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
        }

        private void CancelarTurno()
        {
            if (dgv.SelectedRows.Count == 0)
            {
                MessageBox.Show("Seleccione un turno para cancelar.", "Advertencia",
                    MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            var vm = dgv.SelectedRows[0].DataBoundItem as TurnoRow;

            if (vm.Ref.Estado == "Confirmado")
            {
                MessageBox.Show("No puede cancelar un turno ya confirmado.",
                    "Advertencia", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            if (MessageBox.Show("¿Desea cancelar el turno seleccionado?",
                "Confirmar",
                MessageBoxButtons.YesNo,
                MessageBoxIcon.Question) == DialogResult.Yes)
            {
                vm.Ref.Estado = "Cancelado";
                CargarTabla();

                MessageBox.Show("Turno cancelado.", "Información",
                    MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
        }
    }
}
