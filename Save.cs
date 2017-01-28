using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Runtime.Serialization;

namespace Othello
{
    [Serializable()]
    class Save : ISerializable
    {
        public LogicBoard Board;
        public Player CurrentPlayer;
        public Player Player2;
        public void GetObjectData(SerializationInfo info, StreamingContext context)
        {
            throw new NotImplementedException();
        }
    }
}
