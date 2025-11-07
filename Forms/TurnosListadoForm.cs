using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Windows.Forms;

namespace ClinicaSePrice.Forms
{
    public partial class TurnosListadoForm : Form
    {
        private Label lblTitulo, lblFiltroProfesional, lblFiltroPaciente;
        private ComboBox cbProfesional, cbPaciente;
        private DataGridView dgvTurnos;
        private Button btnCancelarTurno, btnCerrar;

        // Lista simulada de turnos
        private List<Turno> turnos = new List<Turno>
        {
            new Turno { Paciente = "Juan Pérez", Profesional = "Dra. Sánchez", Fecha = DateTime.Today.AddDays(1), Hora = "10:00", Estado = "Pendiente" },
            new Turno { Paciente = "María Gómez", Profesional = "Dr. Fernández", Fecha = DateTime.Today.AddDays(2), Hora = "14:00", Estado = "Pendiente" },
            new Turno { Paciente = "Carlos Díaz", Profesional = "Dra. Romero", Fecha = DateTime.Today.AddDays(3), Hora = "09:00", Estado = "Pendiente" }
        };

        public TurnosListadoForm()
        {
            InitializeComponent();
        }

        private void InitializeComponent()
        {
            // ===== Configuración general =====
            this.Text = "Listado de Turnos - Clínica SePrice";
            this.BackColor = Color.WhiteSmoke;
            this.Font = new Font("Segoe UI", 10F);
            this.ClientSize = new Size(760, 480);
            this.FormBorderStyle = FormBorderStyle.FixedDialog;
            this.StartPosition = FormStartPosition.CenterScreen;

            // ===== Título =====
            lblTitulo = new Label
            {
                Text = "Listado de Turnos",
                Font = new Font("Segoe UI", 20F, FontStyle.Bold),
                ForeColor = Color.FromArgb(45, 65, 200),
                Dock = DockStyle.Top,
                Height = 60,
                TextAlign = ContentAlignment.MiddleCenter
            };

            // ===== Filtros =====
            lblFiltroProfesional = new Label
            {
                Text = "Profesional:",
                Location = new Point(40, 80),
                AutoSize = true
            };
            cbProfesional = new ComboBox
            {
                Location = new Point(130, 76),
                Width = 200,
                DropDownStyle = ComboBoxStyle.DropDownList
            };
            cbProfesional.Items.AddRange(new string[]
            {
                "Todos",
                "Dra. Sánchez",
                "Dr. Fernández",
                "Dra. Romero"
            });
            cbProfesional.SelectedIndex = 0;
            cbProfesional.SelectedIndexChanged += (s, e) => FiltrarTurnos();

            lblFiltroPaciente = new Label
            {
                Text = "Paciente:",
                Location = new Point(380, 80),
                AutoSize = true
            };
            cbPaciente = new ComboBox
            {
                Location = new Point(460, 76),
                Width = 200,
                DropDownStyle = ComboBoxStyle.DropDownList
            };
            cbPaciente.Items.AddRange(new string[]
            {
                "Todos",
                "Juan Pérez",
                "María Gómez",
                "Carlos Díaz"
            });
            cbPaciente.SelectedIndex = 0;
            cbPaciente.SelectedIndexChanged += (s, e) => FiltrarTurnos();

            // ===== DataGridView =====
            dgvTurnos = new DataGridView
            {
                Location = new Point(40, 120),
                Size = new Size(680, 260),
                ReadOnly = true,
                AllowUserToAddRows = false,
                SelectionMode = DataGridViewSelectionMode.FullRowSelect,
                AutoGenerateColumns = false,
                BackgroundColor = Color.White
            };

            dgvTurnos.Columns.Add("Paciente", "Paciente");
            dgvTurnos.Columns.Add("Profesional", "Profesional");
            dgvTurnos.Columns.Add("Fecha", "Fecha");
            dgvTurnos.Columns.Add("Hora", "Hora");
            dgvTurnos.Columns.Add("Estado", "Estado");

            dgvTurnos.DataSource = turnos;

            // ===== Botones =====
            btnCancelarTurno = CrearBoton("Cancelar turno", 200, 410, Color.FromArgb(220, 50, 60), Color.White);
            btnCancelarTurno.Click += BtnCancelarTurno_Click;

            btnCerrar = CrearBoton("Cerrar", 400, 410, Color.Gray, Color.White);
            btnCerrar.Click += (s, e) => this.Close();

            // ===== Agregar controles =====
            this.Controls.Add(lblTitulo);
            this.Controls.Add(lblFiltroProfesional);
            this.Controls.Add(cbProfesional);
            this.Controls.Add(lblFiltroPaciente);
            this.Controls.Add(cbPaciente);
            this.Controls.Add(dgvTurnos);
            this.Controls.Add(btnCancelarTurno);
            this.Controls.Add(btnCerrar);
        }

        private Button CrearBoton(string texto, int x, int y, Color backColor, Color foreColor)
        {
            var boton = new Button
            {
                Text = texto,
                Location = new Point(x, y),
                Size = new Size(150, 40),
                BackColor = backColor,
                ForeColor = foreColor,
                FlatStyle = FlatStyle.Flat,
                Cursor = Cursors.Hand
            };
            boton.FlatAppearance.BorderSize = 0;
            boton.MouseEnter += (s, e) => boton.BackColor = ControlPaint.Light(backColor);
            boton.MouseLeave += (s, e) => boton.BackColor = backColor;
            return boton;
        }

        // ==== Filtro de turnos ====
        private void FiltrarTurnos()
        {
            var filtroProfesional = cbProfesional.Text;
            var filtroPaciente = cbPaciente.Text;

            var filtrados = turnos
                .Where(t =>
                    (filtroProfesional == "Todos" || t.Profesional == filtroProfesional) &&
                    (filtroPaciente == "Todos" || t.Paciente == filtroPaciente))
                .ToList();

            dgvTurnos.DataSource = null;
            dgvTurnos.DataSource = filtrados;
        }

        // ==== Cancelar turno ====
        private void BtnCancelarTurno_Click(object sender, EventArgs e)
        {
            if (dgvTurnos.SelectedRows.Count == 0)
            {
                MessageBox.Show("Debe seleccionar un turno.", "Advertencia", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            var seleccionado = (Turno)dgvTurnos.SelectedRows[0].DataBoundItem;

            var confirm = MessageBox.Show($"¿Desea cancelar el turno de {seleccionado.Paciente} con {seleccionado.Profesional}?",
                                          "Confirmar cancelación",
                                          MessageBoxButtons.YesNo,
                                          MessageBoxIcon.Question);

            if (confirm == DialogResult.Yes)
            {
                seleccionado.Estado = "Cancelado";
                dgvTurnos.Refresh();
                MessageBox.Show("Turno cancelado correctamente.", "Éxito", MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
        }

        // ==== Clase auxiliar ====
        public class Turno
        {
            public string Paciente { get; set; }
            public string Profesional { get; set; }
            public DateTime Fecha { get; set; }
            public string Hora { get; set; }
            public string Estado { get; set; }
        }
    }
}
