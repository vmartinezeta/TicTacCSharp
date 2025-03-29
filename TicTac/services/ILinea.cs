using System.Collections.Generic;

namespace TicTac.services
{
    public interface ILinea
    {
        int toBit();

        bool contiene(Ficha ficha);

        bool contieneTodas(Ficha ficha);

        List<Ficha> toFichaList();

        List<Celda> toList();

        Celda getCeldaDisponible();
    }
}
