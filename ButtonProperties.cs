using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CardGame
{
    public class ButtonProperties
    {
        public ButtonProperties()
        {
            state = State.Inactive;
        }

        public enum State
        {
            Inactive,
            Waiting,
            Hover,
            Press,
            ReleaseAndSend
        }
        public State state { get; set; }
        public string message { get; set; }


    }
}
