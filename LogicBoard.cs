using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Media.Media3D;

namespace Othello
{
    class LogicBoard : IPlayable
    {
        const int WIDTH = 8;
        const int HEIGHT = 8;
        Pawn[,] board = new Pawn[WIDTH, HEIGHT];

        public void fillFakeBoard()
        {
            int[,] fake =
            {
                {0,0,0,0,0,0,0,0},
                {0,0,0,0,0,0,0,0},
                {0,0,2,2,2,1,0,0},
                {0,0,2,0,0,0,0,0},
                {0,0,0,2,0,0,0,0},
                {0,0,0,0,2,0,0,0},
                {0,0,0,0,0,0,0,0},
                {0,0,0,0,0,0,0,0},
            };

            for (var i = 0; i < HEIGHT; i++)
            {
                for (var j = 0; j < WIDTH; j++)
                {
                    if (fake[i, j] != 0)
                    {
                        var color = fake[i, j] == 1 ? Pawn.Colors.Withe : Pawn.Colors.Black;
                        board[i, j] = new Pawn(color, i, j);
                    }
                    else
                    {
                        board[i, j] = null;
                    }
                }
            }
        }

        private bool searchInDirection(Pawn.Colors color,int column, int line, int deltaX, int deltaY)
        {
            var hit = false;
            var stop = false;
            while (!stop)
            {
                line += deltaY;
                column += deltaX;

                Console.WriteLine($"GOING TO LINE : {line} AND COLUMN {column}");

                if (line >= HEIGHT || line < 0 || column >= WIDTH || column < 0)
                {
                    hit = false;
                    stop = true;
                }
                else
                {
                    var currentPawn = board[line, column];

                    if (currentPawn == null)
                    {
                        hit = false;
                        stop = true;
                    }

                    else if (currentPawn.Color == color)
                    {
                        hit = true;
                        stop = true;
                    }
                }
            }

            return hit;
        }
        
        public bool isPlayable(int column, int line, bool isWhite)
        {
            // https://user.xmission.com/~sgigo/elec/ocreversi/legalmoves.html
            //check if we have the same color on the line or diag, without gaps

            //Position empty ?
            if (board[line,column] != null)
                return false;

            var ourColor = isWhite ? Pawn.Colors.Withe : Pawn.Colors.Black;
            var otherColor = isWhite ? Pawn.Colors.Black : Pawn.Colors.Withe;

            var currentPawn = new Pawn.Direction(x: column, y: line);

            var directions = new Pawn.Direction[8]{
                new Pawn.Direction( 0, -1), // North
                new Pawn.Direction( 0,  1), // South
                new Pawn.Direction(-1,  0), // west
                new Pawn.Direction( 1,  0), // east

                new Pawn.Direction(-1, -1), // north west
                new Pawn.Direction( 1,  1), // south east
                new Pawn.Direction( 1, -1), // north east
                new Pawn.Direction(-1,  1), // south west
            };

            var found = false;
            foreach (var direction in directions)
            {

                if (!found && currentPawn.y < HEIGHT && currentPawn.y >= 0 && currentPawn.x < WIDTH && currentPawn.x > 0)
                {
                    found = searchInDirection(ourColor, currentPawn.x, currentPawn.y, direction.x,
                    direction.y);
                }
            }




            return found;
        }

        public bool playMove(int column, int line, bool isWhite)
        {
            throw new NotImplementedException();
        }

        public Tuple<char, int> getNextMove(int[,] game, int level, bool whiteTurn)
        {
            throw new NotImplementedException();
        }

        public int getWhiteScore()
        {
            throw new NotImplementedException();
        }

        public int getBlackScore()
        {
            throw new NotImplementedException();
        }
    }
}
