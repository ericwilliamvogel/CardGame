using System;
using System.Collections.Generic;
using System.Text;

namespace CardGameServer
{

        public enum ServerPackets
        {
            // Sent from server to client
            welcome = 1
        }
        public enum ClientPackets
        {
            // Sent from client to server
            welcomeReceived = 1,
        }
    

}
