using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Runtime.Serialization;
using System.Runtime.Serialization.Formatters.Binary;

namespace LearningGame.Core
{
    [Serializable()]
    public class PlayerState : ISerializable 
    {
        public int Gold;

        public PlayerState(int gold)
        {
            Gold = gold;
        }

        //Deserialization constructor.
        public PlayerState(SerializationInfo info, StreamingContext ctxt)
        {
            //Get the values from info and assign them to the appropriate properties
            Gold = (int)info.GetValue("Gold", typeof(int));
        }

        //Serialization function.
        public void GetObjectData(SerializationInfo info, StreamingContext ctxt)
        {
            info.AddValue("Gold", Gold);
        }
    }
}
