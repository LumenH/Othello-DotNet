using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Diagnostics;
using System.Runtime.Remoting.Channels;
using System.Timers;
using System.Windows.Threading;
using static Othello.Pawn;

namespace Othello
{
    public class Player
    {
        string name;
        Pawn.Colors color;
        TimeSpan maxTime = new TimeSpan(0, 30, 0);
        TimeSpan timeLeft;
        private bool timeMoving = false;

        public Player(string n, Colors c)
        {
            timeLeft = maxTime;
            name = n;
            color = c;
            
        }

        public void start()
        {
            timeMoving = true;
        }

        public void stop()
        {
            timeMoving = false;
        }

        public void tick()
        {
            if (timeMoving)
                timeLeft = timeLeft.Subtract(TimeSpan.FromSeconds(1));
        }
        
        public int SecondsLeft => (int) timeLeft.Seconds;

        public int MinutesLeft => (int) timeLeft.Minutes;
    }
}
