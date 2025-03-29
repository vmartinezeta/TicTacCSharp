
namespace TicTac.services
{
    public interface ICuadriculaProxy:IResumePlayer
    {
        void cambiarTurno();

        void updateCpu();

        bool finalizoJuego();

        bool hayEmpate();
        
    }
}