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
using System.Runtime.Serialization;
using static Othello.Pawn;

namespace Othello
{
    [Serializable()]
    public class Player : ISerializable
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

        //Serialization
        public void GetObjectData(SerializationInfo info, StreamingContext context)
        {
            info.AddValue("Name",name);
            info.AddValue("Color", color);
            info.AddValue("TimeLeft", timeLeft);
            info.AddValue("timeMoving", timeMoving);
        }
        public Player(SerializationInfo info, StreamingContext ctxt)
        {
            name = (string)info.GetValue("Name", typeof(string));
            color = (Colors)info.GetValue("Color", typeof(Colors));
            timeLeft = (TimeSpan)info.GetValue("TimeLeft", typeof(TimeSpan));
            timeMoving = (bool)info.GetValue("timeMoving", typeof(bool));
        }

        public int SecondsLeft => (int) timeLeft.TotalSeconds;

        public int MinutesLeft => (int) timeLeft.TotalMinutes;
    }
}
