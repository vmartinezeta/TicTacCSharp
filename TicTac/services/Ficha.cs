
namespace TicTac.services
{
    public class Ficha
    {
        public int Id { get; }
        public string Text { get; }

        public Ficha(int id, string text)
        {
            Id = id;
            Text = text;
        }

        public bool IsCero()
        {
            return Id == 0;
        }

        public bool IsEquis()
        {
            return Id == 1;
        }

        public bool isEspacio()
        {
            return Id == 4;
        }
    }
}
