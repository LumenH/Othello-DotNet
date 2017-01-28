using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Runtime.Serialization;

namespace Othello
{
    [Serializable()]
    public class Pawn: ISerializable
    {
        [Serializable()]
        public struct Direction: ISerializable
        {
            public int x, y;

            public Direction(int x, int y)
            {
                this.x = x;
                this.y = y;
            }
            //Constructeur de deserialization
            public Direction(SerializationInfo info, StreamingContext ctxt)
            {
                x = (int)info.GetValue("X", typeof(int));
                y = (int)info.GetValue("Y", typeof(int));
            }
            public void GetObjectData(SerializationInfo info, StreamingContext context)
            {
                info.AddValue("X", x);
                info.AddValue("Y", y);
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

        public bool IsWhite => color == Colors.White;

        public void Flip() => color = color == Colors.White ? Colors.Black : Colors.White;

        public override string ToString()
        {
            return $"{color} {pos.x} {pos.y}";
        }

        public Pawn(SerializationInfo info, StreamingContext ctxt)
        {
            pos = (Direction)info.GetValue("Direction", typeof(Direction));
            color = (Colors)info.GetValue("Colors", typeof(Colors));
        }
        public void GetObjectData(SerializationInfo info, StreamingContext context)
        {
            info.AddValue("Direction", pos);
            info.AddValue("Colors", color);
        }
    }
}
