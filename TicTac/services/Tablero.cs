using System.Collections.Generic;
using System.Drawing;
using System.Windows.Forms;

namespace TicTac.services
{
    public class Tablero
    {
        private List<Button> _buttonList;
        private Button[,] _BotonCuadricula;
        private Cuadricula _cuadricula;
        public Celda Celda { get; set; }

        public Tablero(List<Button> buttons, Cuadricula cuadricula) {
            Celda = null;
            _buttonList = buttons;
            cuadricular();   
            _cuadricula = cuadricula;
            vincular();           
            startup();
        }

        public void desabilitarTodo()
        {
            foreach (Button button in _buttonList)
            {
                button.Enabled = false;
            }
        }

        public void habilitarDisponibles()
        {
            foreach (var celda in _cuadricula.getDisponibles())
            {
                var origen = celda.Origen;
                fromXY(origen.X, origen.Y).Enabled = true;
            }
        }

        public void startup()
        {
            foreach (Button button in _buttonList)
            {
                button.BackColor = Color.Transparent;
            }
            desabilitarTodo();
            habilitarDisponibles();
        }

        public void seleccionar(int x, int y)
        {
            fromXY(x, y).BackColor = Color.LightGreen;
            Celda = _cuadricula.fromXY(x, y);
        }

        public Button fromXY(int x, int y)
        {
            return _BotonCuadricula[x, y];
        }

        public void cuadricular() {
            _BotonCuadricula = new Button[3, 3];
            for (int i = 0; i < 3; i++)
            {
                for (int j = 0; j < 3; j++)
                {
                    _BotonCuadricula[i, j] = _buttonList[3*i+j];
                }
            }
        }

        public void pintar(Linea l)
        {
            foreach (var celda in l.toList())
            {
                var origen = celda.Origen;
                fromXY(origen.X, origen.Y).BackColor = Color.Red;
            }
        }

        public void vincular()
        {
            for (int i = 0; i < _cuadricula.toCeldaList().Count; i++)
            {
                _buttonList[i].Text = _cuadricula.toCeldaList()[i].Ficha.Text;
            }
        }
    }
}