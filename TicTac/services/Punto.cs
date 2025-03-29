using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TicTac.services
{
    public  class Punto
    {
        public int X { get; }
        public int Y { get; }

        public Punto(int x, int y)
        {
            X = x;
            Y = y;
        }

        public string toString()
        {
            return this.X + "," + this.Y;
        }
    }
}
