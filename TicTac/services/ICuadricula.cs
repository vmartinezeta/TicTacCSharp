
using System.Collections.Generic;

namespace TicTac.services
{
    public interface ICuadricula
    {
        Celda fromXY(int x, int y);
        List<Celda> toCeldaList();
        List<Linea> toLineaList();
        List<Celda> getDisponibles();
        bool estaLlena();
    }
}
