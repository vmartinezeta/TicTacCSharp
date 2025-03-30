using System;
using System.Collections.Generic;
using System.Windows.Forms;
using TicTac.services;

namespace TicTac
{
    public partial class Form1 : Form
    {
        private CuadriculaProxy _cuadriculaProxy;
        private Tablero _tablero;
        private Timer _timer;
        private Timer _timerLoading;

        public Form1()
        {
            InitializeComponent();
            _cuadriculaProxy = new CuadriculaProxy();
            _timer = new Timer();
            _timer.Interval = 1500;
            _timer.Tick += TimerColocacionCPU;
            _timerLoading = new Timer();
            _timerLoading.Interval = 100;
            _timerLoading.Tick += TimerLoading;
        }

        
        private void TimerLoading(object sender, EventArgs e) {             
            lblText.Text += ".";
            if (lblText.Text.Length > 10) {
                lblText.Text = "Loading";
            }
        }

        private void TimerColocacionCPU(object sender, EventArgs e)
        {
            _timer.Stop();
            _timerLoading.Stop();
            lblText.Text = "";
            _cuadriculaProxy.updateCpu();
            _tablero = new Tablero(crearButton(), _cuadriculaProxy.Cuadricula);
            notificar();
            btnColocacionJugador.Text = _cuadriculaProxy.FichaEnJuego.Text;
        }

        protected override void OnFormClosing(FormClosingEventArgs e)
        {
            base.OnFormClosing(e);
            if (_timer == null) return;
            _timer.Stop();
            _timer.Dispose();
        }
    
        private List<Button> crearButton()
        {
            var buttonList = new List<Button>();
            buttonList.Add(btnFicha1);
            buttonList.Add(btnFicha2);
            buttonList.Add(btnFicha3);
            buttonList.Add(btnFicha4);
            buttonList.Add(btnFicha5);
            buttonList.Add(btnFicha6);
            buttonList.Add(btnFicha7);
            buttonList.Add(btnFicha8);
            buttonList.Add(btnFicha9);
            return buttonList;
        }

        private void Form1_Load(object sender, EventArgs e)
        {           
            _cuadriculaProxy.updateCpu();
            _tablero = new Tablero(crearButton(), _cuadriculaProxy.Cuadricula);
            btnColocacionJugador.Text = _cuadriculaProxy.FichaEnJuego.Text;
            btnCancelacionJugador.Text = " ";
        }

        private void btnColocacionJugador_Click(object sender, EventArgs e)
        {
            if (_tablero.Celda == null) return;
            _cuadriculaProxy.updateCelda(_tablero.Celda);
            _tablero = new Tablero(crearButton(), _cuadriculaProxy.Cuadricula);
            btnColocacionJugador.Text = _cuadriculaProxy.FichaEnJuego.Text;
            notificar();
            btnColocacionJugador.Text = _cuadriculaProxy.FichaEnJuego.Text;
            if (_cuadriculaProxy.finalizoJuego()) return;
            _timer.Enabled = true;
            _timer.Start();
            lblText.Text = "Loading";
            _timerLoading.Enabled = true;
            _timerLoading.Start();
        }

        private void notificar()
        {
            if (_cuadriculaProxy.hayGanador())
            {
                _tablero.pintar(_cuadriculaProxy.getGanador());
                if (_cuadriculaProxy.ganoJugador())
                {
                    MessageBox.Show("Haz ganado...");
                } else if (_cuadriculaProxy.falloJugador())
                {
                    MessageBox.Show("Haz perdido...");
                }
            }
            if (!_cuadriculaProxy.finalizoJuego()) return;
            _tablero.desabilitarTodo();
            btnColocacionJugador.Enabled = false;
            btnCancelacionJugador.Enabled = false;
        }

        private void btnFicha1_Click(object sender, EventArgs e)
        {
            _tablero = new Tablero(crearButton(), _cuadriculaProxy.Cuadricula);
            _tablero.seleccionar(0, 0);
        }

        private void btnFicha2_Click(object sender, EventArgs e)
        {
            _tablero = new Tablero(crearButton(), _cuadriculaProxy.Cuadricula);
            _tablero.seleccionar(0, 1);
        }

        private void btnFicha3_Click(object sender, EventArgs e)
        {
            _tablero = new Tablero(crearButton(), _cuadriculaProxy.Cuadricula);
            _tablero.seleccionar(0, 2);
        }

        private void btnFicha4_Click(object sender, EventArgs e)
        {
            _tablero = new Tablero(crearButton(), _cuadriculaProxy.Cuadricula);
            _tablero.seleccionar(1, 0);
        }

        private void btnFicha5_Click(object sender, EventArgs e)
        {            
            new Tablero(crearButton(), _cuadriculaProxy.Cuadricula);
            _tablero.seleccionar(1, 1);
        }

        private void btnFicha6_Click(object sender, EventArgs e)
        {
            _tablero = new Tablero(crearButton(), _cuadriculaProxy.Cuadricula);
            _tablero.seleccionar(1, 2);
        }

        private void btnFicha7_Click(object sender, EventArgs e)
        {
            _tablero = new Tablero(crearButton(), _cuadriculaProxy.Cuadricula);
            _tablero.seleccionar(2, 0);
        }

        private void btnFicha8_Click(object sender, EventArgs e)
        {
            _tablero = new Tablero(crearButton(), _cuadriculaProxy.Cuadricula);
            _tablero.seleccionar(2, 1);
        }

        private void btnFicha9_Click(object sender, EventArgs e)
        {
            _tablero = new Tablero(crearButton(), _cuadriculaProxy.Cuadricula);
            _tablero.seleccionar(2, 2);
        }

        private void btnCancelacionJugador_Click(object sender, EventArgs e)
        {
            _tablero = new Tablero(crearButton(), _cuadriculaProxy.Cuadricula);
        }
      
    }
}