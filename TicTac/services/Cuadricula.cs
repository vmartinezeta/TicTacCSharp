using System.Collections.Generic;

namespace TicTac.services
{
    public class Cuadricula :ICuadricula, IUpdateCelda
    {
        private Celda[,] _celdas;

        public Cuadricula()
        {
            _celdas = new Celda[3, 3];
            for (int i=0;i<3; i++)
            {
                for (int j=0; j<3;j++)
                {
                    _celdas[i, j] = new Celda(new Ficha(4, "-"), new Punto(i, j));
                }
            }
        }

        public Celda fromXY(int x, int y)
        {
            return _celdas[x, y];
        }

        public List<Celda> toCeldaList()
        {
            var celdas = new List<Celda>();
            for (int i = 0; i < 3; i++)
            {
                for (int j = 0; j < 3; j++)
                {
                    celdas.Add(_celdas[i,j]);
                }
            }
            return celdas;
        }

        public List<Linea> toLineaList()
        {
            List<Linea> lineas = new List<Linea>();
            var diagonalNormales = new List<Celda>();
            var diagonalInvertidas = new List<Celda>();
            for (int i = 0; i < 3; i++)
            {
                var horizontales = new List<Celda>();
                var verticales = new List<Celda>();
                for (int j = 0; j < 3; j++)
                {
                    horizontales.Add(_celdas[i,j]);
                    verticales.Add(_celdas[j,i]);

                    if (i== j)
                    {
                        diagonalNormales.Add(_celdas[i, j]);
                    }
                    if (i + j == 2)
                    {
                        diagonalInvertidas.Add(_celdas[i, j]);
                    }
                }
                lineas.Add(new Linea(horizontales, "HORIZONTAL"));
                lineas.Add(new Linea(verticales, "VERTICAL"));
            }
            lineas.Add(new Linea(diagonalNormales, "DIAGONAL-1"));
            lineas.Add(new Linea(diagonalInvertidas, "DIAGONAL-2"));
            return lineas;
        }

        public List<Celda> getDisponibles()
        {
            var disponibles = new List<Celda>();
            foreach(var c in _celdas)
            {
                if (c.isEspacioDisponible())
                {
                    disponibles.Add(c);
                }
            }
            return disponibles;
        }

        public void updateCelda(Celda celda)
        {
            var origen = celda.Origen;
            this._celdas[origen.X, origen.Y] = celda;
        }

        public bool estaLlena()
        {
            var total = 0;
            foreach(var c in _celdas)
            {
                if (c.Ficha.Id == 4)
                {
                    total++;
                }
            }
            return total == 0;
        }
    }
}