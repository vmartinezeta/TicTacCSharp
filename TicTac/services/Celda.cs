
namespace TicTac.services
{
    
    public class Celda
    {
        public Ficha Ficha { get; set; }
        public Punto Origen { get; }
        

        public Celda(Ficha ficha, Punto origen)
        {
            Ficha = ficha;
            Origen = origen;
        }

        public bool isEspacioDisponible()
        {
            return Ficha.isEspacio();
        }
    }
}
