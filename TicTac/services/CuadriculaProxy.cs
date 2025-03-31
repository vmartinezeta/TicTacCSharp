
using System;

namespace TicTac.services
{
    public class CuadriculaProxy : ICuadriculaProxy, IUpdateCelda, IResumeCuadricula, IResumePlayer
    {
        private Ficha _fichaCpuEquis;
        private Ficha _fichaJugadorCero;
        public Ficha FichaEnJuego { get; set; }
        public  Cuadricula Cuadricula { get; }

        public CuadriculaProxy()
        {
            Cuadricula = new Cuadricula();
            _fichaCpuEquis = new Ficha(1, "x");
            _fichaJugadorCero = new Ficha(0, "0");
            FichaEnJuego = _fichaCpuEquis;
        }

        public void cambiarTurno()
        {
            if (FichaEnJuego.IsEquis())
            {
                FichaEnJuego = _fichaJugadorCero;
            } else
            {
                FichaEnJuego = _fichaCpuEquis;
            }
        }

        public void updateCelda(Celda celda)
        {
            celda.Ficha = _fichaJugadorCero;
            Cuadricula.updateCelda(celda);
            cambiarTurno();
        } 

        public void updateCpu()
        {
            Celda celda = null;
            if (puedeGanar()){
                var l = getCandidatoCPU();
                celda = l.getCeldaDisponible();
            } else {
                var disponibles = Cuadricula.getDisponibles();
                var random = new Random();
                var idx = random.Next(0,disponibles.Count);
                celda = disponibles[idx];
            }
            celda.Ficha = _fichaCpuEquis;
            Cuadricula.updateCelda(celda);
            cambiarTurno();
        }

        public bool hayGanador()
        {
            foreach(var l in Cuadricula.toLineaList())
            {
                if(l.hayGanador())
                {
                    return true;
                }
            }
            return false;
        }

        public bool finalizoJuego()
        {
            return hayGanador() || hayEmpate();
        }

        public bool hayEmpate()
        {
            return Cuadricula.estaLlena();
        }

        public Linea getGanador()
        {
            foreach (var l in Cuadricula.toLineaList()) 
            {
                if (l.hayGanador())
                {
                    return l;
                }
            }
            return null;
        }

        public Linea getCandidatoCPU()
        {
            foreach (var l in Cuadricula.toLineaList())
            {
                if (l.puedeGanar() && l.contiene(_fichaCpuEquis)) {
                    return l;
                }
            }
            return null;
        }

        public bool puedeGanar()
        {
            foreach (var l in Cuadricula.toLineaList())
            {
                if (l.puedeGanar() && l.contiene(_fichaCpuEquis)) {
                    return true;
                }
            }
            return false;
        }

        public bool ganoJugador()
        {
            var l = getGanador();
            if (l ==null)
            {
                return false;
            }
            return l.contieneTodas(_fichaJugadorCero);
        }

        public bool falloJugador()
        {
            var l = getGanador();
            if (l == null)
            {
                return false;
            }
            return l.contieneTodas(_fichaCpuEquis);
        }
    }
}