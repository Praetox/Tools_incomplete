using System;
using System.Collections.Generic;
using System.Net.Sockets;
using System.Text;

namespace nutlines
{
    class Player
    {
        public bool connected;
        private Socket sck;
        public Player(Socket sck)
        {
            this.sck = sck;
            connected = true;
        }
    }
}
