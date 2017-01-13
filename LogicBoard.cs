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

        private void searchInDirection(int column, int line, int deltaX, int deltaY)
        {
            
        }
        
        public bool isPlayable(int column, int line, bool isWhite)
        {
            // https://user.xmission.com/~sgigo/elec/ocreversi/legalmoves.html
            //check if we have the same color on the line or diag, without gaps

            //Position empty ?
            if (board[line, column] != null)
                return false;

            bool validMove = false;

            int numFlipped = 0;
            int tempX = column, tempY = line;
            var ourColor = isWhite ? Pawn.Colors.Withe : Pawn.Colors.Black;
            var otherColor = isWhite ? Pawn.Colors.Black : Pawn.Colors.Withe;

            int[,] deltas = new int[8,2]{
                { 0, -1}, // North
                { 0,  1}, // South
                { -1, 0}, // west
                { 1,  0}, // east

                {-1, -1}, // north west
                { 1,  1}, // south east
                { 1, -1}, // north east
                { -1, 1}, // south west
            };

           
            for(int i = 0 ; i < 8; i++)
            {
                searchInDirection(tempX,tempY,deltas[i,0], deltas[i,1]);
            }



            return validMove;
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
