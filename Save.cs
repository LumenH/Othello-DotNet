﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Runtime.Serialization;

namespace Othello
{
    [Serializable()]
    public class Save : ISerializable
    {
        public LogicBoard Board;
        public Player CurrentPlayer;
        public Player Player2;
        public int turn;

        public Save(LogicBoard b, Player current, Player other, int t)
        {
            Board = b;
            CurrentPlayer = current;
            Player2 = other;
            turn = t;
        }

        public void GetObjectData(SerializationInfo info, StreamingContext context)
        {
            info.AddValue("Board", Board);
            info.AddValue("CurrentPlayer", CurrentPlayer);
            info.AddValue("Player2", Player2);
            info.AddValue("Turn", turn);
        }
        public Save(SerializationInfo info, StreamingContext ctxt)
        {
            Board = (LogicBoard)info.GetValue("Board", typeof(LogicBoard));
            CurrentPlayer = (Player)info.GetValue("CurrentPlayer", typeof(Player));
            Player2 = (Player)info.GetValue("Player2", typeof(Player));
            turn = (int)info.GetValue("Turn", typeof(int));
        }
    }
}
