namespace TicTac.services
{
    public interface IResumePlayer
    {
        Linea getGanador();
        Linea getCandidatoCPU();
        bool ganoJugador();
        bool falloJugador();
    }
}
