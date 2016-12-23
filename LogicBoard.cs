using System;
using System.Collections.Generic;
using System.Linq;
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

        public bool isPlayable(int column, int line, bool isWhite)
        {
            throw new NotImplementedException();
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
