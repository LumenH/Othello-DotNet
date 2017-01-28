using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Othello
{
    public class Pawn
    {
        public struct Direction
        {
            public int x, y;

            public Direction(int x, int y)
            {
                this.x = x;
                this.y = y;
            }

            public static Direction operator +(Direction d1, Direction d2)
            {
                var tmp = new Direction(x: d1.x, y: d1.y);
                tmp.x += d2.x;
                tmp.y += d2.y;
                return tmp;
            }
        }

        public Direction pos;

        public enum Colors {White, Black};

        Colors color;
        
        public Pawn(Colors color, int x, int y)
        {
            this.color = color;
            pos = new Direction(x:x,y:y);
        }

        public Colors Color => color;

        public void Flip() => color = color == Colors.White ? Colors.Black : Colors.White;

        public override string ToString()
        {
            return $"{color} {pos.x} {pos.y}";
        }
    }
}
