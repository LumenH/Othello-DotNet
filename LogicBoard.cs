using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Runtime.Serialization;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Media.Media3D;
using System.Runtime.Serialization;

namespace Othello
{
    [Serializable()]
     public class LogicBoard : IPlayable, ISerializable

    {
        public static int WIDTH => 8;
        public static int HEIGHT => 8;

        public Pawn[,] Board { get; set; } = new Pawn[WIDTH, HEIGHT];

        readonly Pawn.Direction[] directions = new Pawn.Direction[8]{
            new Pawn.Direction( 0, -1), // North
            new Pawn.Direction( 0,  1), // South
            new Pawn.Direction(-1,  0), // west
            new Pawn.Direction( 1,  0), // east

            new Pawn.Direction(-1, -1), // north west
            new Pawn.Direction( 1,  1), // south east
            new Pawn.Direction( 1, -1), // north east
            new Pawn.Direction(-1,  1), // south west
        };

        public LogicBoard()
        {

        }


        public void addPawn(int x, int y, Pawn.Colors color )
        {
            Board[y,x] = new Pawn(color, x, y);
        }

        public void fillFakeBoard()
        {
            int[,] fake =
            {
                {0,0,0,0,0,0,0,0},
                {0,0,0,0,0,0,0,0},
                {0,0,0,0,0,0,0,0},
                {0,0,0,1,2,0,0,0},
                {0,0,0,2,1,0,0,0},
                {0,0,0,0,0,0,0,0},
                {0,0,0,0,0,0,0,0},
                {0,0,0,0,0,0,0,0},
            };

            for (var i = 0; i < HEIGHT; i++)
            {
                for (var j = 0; j < WIDTH; j++)
                {
                    if (fake[i, j] != 0)
                    {
                        var color = fake[i, j] == 1 ? Pawn.Colors.White : Pawn.Colors.Black;
                        Board[i, j] = new Pawn(color, j, i);
                    }
                    else
                    {
                        Board[i, j] = null;
                    }
                }
            }
        }

        private bool SearchInDirection(Pawn.Colors color,int column, int line, int deltaX, int deltaY, ICollection<Pawn> pawnsCrossed = null)
        {
            var hit = false;
            var stop = false;
            var atLeastOne = false;

            while (!stop)
            {
                line += deltaY;
                column += deltaX;

                if (line >= HEIGHT || line < 0 || column >= WIDTH || column < 0)
                {
                    hit = false;
                    stop = true;
                }
                else
                {
                    var currentPawn = Board[line, column];

                    if (currentPawn == null)
                    {
                        hit = false;
                        stop = true;
                    }

                    else if (currentPawn.Color == color)
                    {
                        hit = atLeastOne;
                        stop = true;
                    }
                    else
                    {
                        atLeastOne = true;
                    }


                    if (!stop)
                    {
                        pawnsCrossed?.Add(currentPawn);
                    }
                }

            }

            return hit;
        }
        
        public bool isPlayable(int column, int line, bool isWhite)
        {
            //Position empty ?
            if (Board[line,column] != null)
                return false;

            var ourColor = isWhite ? Pawn.Colors.White : Pawn.Colors.Black;

            var currentPawn = new Pawn.Direction(x: column, y: line);

            var found = false;

            directions.ToList().ForEach(direction =>
            {
                if (!found && currentPawn.y < HEIGHT && currentPawn.y >= 0 && currentPawn.x < WIDTH && currentPawn.x >= 0)
                {
                    found = SearchInDirection(ourColor, currentPawn.x, currentPawn.y, direction.x, direction.y);
                }
            });

            return found;
        }

        public bool playMove(int column, int line, bool isWhite)
        {
            var color = isWhite ? Pawn.Colors.White : Pawn.Colors.Black;

            directions.ToList().ForEach(direction =>
            {   
                var pawnsCrossed = new List<Pawn>();

                var playable = SearchInDirection(color, column, line, direction.x, direction.y,pawnsCrossed);

                if(playable)
                    pawnsCrossed.ForEach(p => p.Flip());
            });

            addPawn(column, line, color);

            return false;
        }

        public Tuple<char, int> getNextMove(int[,] game, int level, bool whiteTurn)
        {
            throw new NotImplementedException();
        }
        
        /*
        public int getWhiteScore() => Board.Cast<Pawn>().Count(p => p?.Color == Pawn.Colors.White);

        public int getBlackScore() => Board.Cast<Pawn>().Count(p => p?.Color == Pawn.Colors.Black);
        */

        public int getWhiteScore() => (from pawn in Board.Cast<Pawn>()
                                        where pawn?.Color == Pawn.Colors.White
                                        select pawn).Count();
        

        public int getBlackScore() => (from pawn in Board.Cast<Pawn>()
                                       where pawn?.Color == Pawn.Colors.Black
                                       select pawn).Count();

        public LogicBoard(SerializationInfo info, StreamingContext ctxt)
        {
            Board = (Pawn[,])info.GetValue("Board", typeof(Pawn[,]));
            //HEIGHT = (int)info.GetValue("Height", typeof(int));

        }

        public void GetObjectData(SerializationInfo info, StreamingContext ctxt)
        {
            info.AddValue("Board", Board);
            //info.AddValue("Height", HEIGHT);
            //info.AddValue("Width", WIDTH);
        }
    }
}
