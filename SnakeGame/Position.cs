using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SnakeGamePersonalizer
{
    public class Position
    {
        public Position(int x, int y) { 
            this.x = x;
            this.y = y;
        }

        public int x { get; set; }
        public int y { get; set; }

        public bool Equals (Position other)
        {
            if (x == other.x && y == other.y) return true;
            return false;
        }

        public string ToString()
        {
            return $"({this.x},{this.y})";
        }

    }
}
