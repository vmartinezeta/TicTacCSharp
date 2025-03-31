using System;
using System.Drawing;
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
        private Label lblText;

        public Tablero()
        {
            init();
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

        private void init ()
        {
            this.StartPosition = FormStartPosition.CenterScreen;
            this.Size = new Size(340, 360);
            this.Padding = new Padding(40);
            panel = new Panel();
            panel.Size = new Size(340, 360);
            panel.Dock = DockStyle.Fill;
            this.Controls.Add(panel);
            lblText = new Label();
            lblText.Text = "";
            lblText.Font = new Font("Arial", 14);
            lblText.BringToFront();
            this.Controls.Add(lblText);
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

        private void Tablero_Load(object sender, EventArgs e)
        {

        }
    }
}