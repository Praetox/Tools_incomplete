using System;
using System.Collections.Generic;
using System.Text;

namespace nutlines
{
    class Game
    {
        private Box[][] board;
        private Player[] player;
        public Game(int x, int y)
        {
            board = new Box[y][];
            for (int a = 0; a < y; a++)
                board[y] = new Box[x];
        }
        public bool AddPlayer(Socket sck)
        {
            bool ret = false;
            for (int a = 0; a < player.Length; a++)
                if (player[a] == null)
                {
                    player[a] = new Player(sck);
                    ret = true; break;
                }
            return ret;
        }
    }
}
