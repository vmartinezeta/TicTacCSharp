
using System.Collections.Generic;

namespace TicTac.services
{
    public class Linea:ILinea, IResumeCuadricula
    {
        public List<Celda> _celdas;
        public string Orientacion { get; }
        public Linea(List<Celda> celdas, string orientacion) 
        {
            _celdas = celdas;
            Orientacion = orientacion;
        }

        public int toBit()
        {
            int total = 0;
            foreach(var celda in _celdas) {
                total += celda.Ficha.Id;
            }
            return total;
        }

        public bool puedeGanar()
        {
            return toBit() == 4 || toBit() == 6;
        }

        public bool hayGanador()
        {
            return toBit() == 0 || toBit() == 3;
        }
        
        public List<Ficha> toFichaList()
        {
            var fichaList = new List<Ficha>();
            foreach (var c in _celdas)
            {
                fichaList.Add(c.Ficha);
            }
            return fichaList;
        }

        public bool contiene(Ficha ficha)
        {
            return toFichaList().Contains(ficha);
        }

        public List<Celda> toList()
        {
            return _celdas;
        }

        public Celda getCeldaDisponible()
        {
            foreach (var c in _celdas)
            {
                if (c.isEspacioDisponible())
                {
                    return c;
                }
            }
            return null;
        }

        public bool contieneTodas(Ficha ficha)
        {
            foreach (var actual in toFichaList())
            {
                if (ficha.Id != actual.Id)
                {
                    return false;
                }
            }
            return true;
        }
    }
}
