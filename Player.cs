using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Diagnostics;
using static Othello.Pawn;

namespace Othello
{
    class Player
    {
        string name;
        Colors color;
        Stopwatch clock;
        TimeSpan maxTime = new TimeSpan(0, 30, 0);
        TimeSpan timeLeft; 

        public Player(string n, Colors c)
        {
            timeLeft = maxTime;
            name = n;
            color = c;
            clock = new Stopwatch();
        }

        public void start()
        {
            clock.Start();
        }
        public void stop()
        {
            clock.Stop();
            timeLeft = timeLeft.Subtract(clock.Elapsed);
        }
        //Getter
        public TimeSpan TimeLeft => timeLeft;
    }
}
