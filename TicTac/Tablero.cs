using System;
using System.Drawing;
using System.IO;
using System.Media;
using System.Windows.Forms;
using TicTac.services;

namespace TicTac
{
    public partial class Tablero : Form
    {
        private CuadriculaProxy _cuadriculaProxy;
        private Timer _timer;
        private Timer _timerLoading;
        private Button[, ] buttons;
        private Panel panel;
        private Panel panelTop;
        private Label lblText;
        private SoundPlayer player;

        public Tablero()
        {
            initComponents();
            iniciarMusicaFondo("fondo.wav");
            cuadricular();
            _cuadriculaProxy = new CuadriculaProxy();
            _cuadriculaProxy.updateCpu();
            actualizar();
            _timer = new Timer();
            _timer.Interval = 1500;
            _timer.Tick += TimerColocacionCPU;
            _timerLoading = new Timer();
            _timerLoading.Interval = 100;
            _timerLoading.Tick += TimerLoading;
        }

        private void initComponents()
        {


            var panelMain = new TableLayoutPanel
            {
                Dock = DockStyle.Fill, // Ocupa todo el formulario
                ColumnCount = 1,      // Una sola columna
                RowCount = 2,
                CellBorderStyle = TableLayoutPanelCellBorderStyle.Single // Bordes opcionales
            };

            // Configuración de filas (alto proporcional o fijo)
            panelMain.RowStyles.Add(new RowStyle(SizeType.AutoSize)); // Fila 1: 30%
            //panelMain.RowStyles.Add(new RowStyle(SizeType.Percent, 40)); // Fila 2: 40%
            panelMain.RowStyles.Add(new RowStyle(SizeType.AutoSize));
            panelMain.Padding = new Padding(20);
            this.Controls.Add(panelMain);


            this.StartPosition = FormStartPosition.CenterScreen;
            this.Size = new Size(340, 400);

            panelTop = new Panel();
            panelMain.Controls.Add(panelTop);

            lblText = new Label();
            lblText.Text = "";
            lblText.Font = new Font("Arial", 14);
            lblText.BringToFront();
            panelTop.Controls.Add(lblText);
            panelTop.BackColor = Color.Orange;
            panelTop.Height = 60;
            panelTop.Padding = new Padding(5);
            panelTop.Dock = DockStyle.Fill;
                


            // Crear el botón de toggle
            Button btnToggleMusica = new Button
            {
                Text = "🔊", // Icono de altavoz (puedes usar imágenes)
                Width = 40,   // Ancho pequeño
                Height = 40,  // Alto pequeño
                FlatStyle = FlatStyle.Flat, // Estilo plano
                BackColor = Color.Transparent,
                Cursor = Cursors.Hand // Cambia el cursor al pasar el mouse
            };

            // Posicionar en la esquina superior derecha del Panel
            btnToggleMusica.Location = new Point(
                panelTop.Width - btnToggleMusica.Width - 5, // X: derecha con margen 5
                5// Y: margen superior 5
            );

            // Anchura para que se mantenga en su lugar al redimensionar
            btnToggleMusica.Anchor = AnchorStyles.Top | AnchorStyles.Right;

            // Evento Click para toggle
            btnToggleMusica.Click += btnToggleMusica_Click;

            // Agregar al Panel
            panelTop.Controls.Add(btnToggleMusica);

            // Traer al frente (sobre otros controles)
            btnToggleMusica.BringToFront();


            panel = new Panel();
            panel.BackColor = Color.AliceBlue;
            panel.Dock = DockStyle.Fill;
            panelMain.Controls.Add(panel);
        }

        private void btnToggleMusica_Click(object sender, EventArgs e)
        {
            if (player != null)
            {
                Button btn = (Button)sender;
                if (btn.Text == "🔊") // Si está sonando
                {
                    player.Stop();
                    btn.Text = "🔈"; // Cambia a icono de silencio
                    btn.BackColor = Color.LightGray; // Feedback visual
                }
                else
                {
                    player.PlayLooping();
                    btn.Text = "🔊";
                    btn.BackColor = Color.Transparent;
                }
            }
        }

        private void iniciarMusicaFondo(string nombreArchivo)
        {
            try
            {
                string rutaMusica = Path.Combine(Application.StartupPath, nombreArchivo);

                if (File.Exists(rutaMusica))
                {
                    player = new SoundPlayer(rutaMusica);
                    player.PlayLooping();
                }
                else
                {
                    MessageBox.Show($"¡Archivo de música no encontrado en:\n{rutaMusica}", "Advertencia", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Error al cargar la música:\n{ex.Message}", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void cuadricular () {
            buttons = new Button[3,3];

            for (int i = 0; i < 3; i++)
            {
                for (int j = 0; j < 3; j++)
                {
                    buttons[i, j] = new Button();                     
                    buttons[i, j].Size = new Size(80, 80);
                    buttons[i, j].Location = new Point(i * 80, j * 80);
                    buttons[i, j].Font = new Font("Arial", 24);
                    buttons[i, j].Click += colocacionJugador;
                    panel.Controls.Add(buttons[i, j]);
                }
            }
        }

        private void actualizar()
        {
            for (int i = 0; i < 3; i++)
            {
                for (int j = 0; j < 3; j++)
                {
                    buttons[i, j].Text = _cuadriculaProxy.Cuadricula.fromXY(i, j).Ficha.Text;
                }
            }
        }

        private Button fromXY(int x, int y)
        {
            return buttons[x, y];
        }

        private Button fromPunto(Punto origen)
        {
            return fromXY(origen.X, origen.Y);
        }

        private Celda fromButton(Button actual)
        {
            for (int i = 0; i < 3; i++)
            {
                for (int j = 0; j < 3; j++)
                {
                    var boton = buttons[i, j];
                    if (actual == boton)
                    {
                        return  _cuadriculaProxy.Cuadricula.fromXY(i, j);
                    }
                }
            }
            return null;
        }

        private void colocacionJugador(object sender, EventArgs evt)
        {
            var activo = (Button)sender;
            var celda = fromButton(activo);
            _cuadriculaProxy.updateCelda(celda);
            actualizar();
            bloquearTodo();
            if (_cuadriculaProxy.finalizoJuego())
            {
                notificar();
                return;
            }
            _timer.Enabled = true;
            _timer.Start();
            _timerLoading.Enabled = true;
            _timerLoading.Start();
            lblText.Text = "Loading";
        }

        private void habilitarDisponibles()
        {
            foreach (var celda in _cuadriculaProxy.Cuadricula.getDisponibles())
            {
                var origen = celda.Origen;
                fromPunto(origen).Enabled = true;
            }
        }

        private void bloquearTodo()
        {
            for (int i = 0; i < 3; i++)
            {
                for (int j = 0; j < 3; j++)
                {
                    fromXY(i,j).Enabled = false;
                }
            }
        }

        private void TimerColocacionCPU(object sender, EventArgs e)
        {
            _timer.Stop();
            _timerLoading.Stop();
            _cuadriculaProxy.updateCpu();
            actualizar();
            lblText.Text = "";
            if (_cuadriculaProxy.finalizoJuego())
            {
                notificar();
                return;
            }
            habilitarDisponibles();
           
        }

        private void TimerLoading( object sender, EventArgs evt)
        {
            lblText.Text += ".";
            if (lblText.Text.Length > 10) {
                lblText.Text = "Loading";
            }

        }

        protected override void OnFormClosing(FormClosingEventArgs e)
        {
            base.OnFormClosing(e);
            if (_timer == null || _timerLoading==null) return;
            _timer.Stop();
            _timerLoading.Stop();
            _timer.Dispose();
            _timerLoading.Dispose();
            player?.Stop();
        }

        private void pintar(Linea l)
        {
            foreach (var celda in l.toList())
            {
                var origen = celda.Origen;
                fromPunto(origen).BackColor = Color.Red;
            }
        }

        private void notificar()
        {
            if (_cuadriculaProxy.hayGanador())
            {
                pintar(_cuadriculaProxy.getGanador());
                if (_cuadriculaProxy.ganoJugador())
                {
                    MessageBox.Show("Haz ganado...");
                } else if (_cuadriculaProxy.falloJugador())
                {
                    MessageBox.Show("Haz perdido...");
                }
            }
        }

    }
}