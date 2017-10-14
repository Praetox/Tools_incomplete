using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;
using System.Net.Sockets;

namespace nutlines
{
    public partial class frmMain : Form
    {
        public frmMain()
        {
            InitializeComponent();
        }

        private void frmMain_Load(object sender, EventArgs e)
        {
            int myPort = 9048;
            Game game = new Game(38, 10);
            System.Net.IPAddress myIP = System.Net.IPAddress.Parse("127.0.0.1");
            System.Net.IPEndPoint myEP = new System.Net.IPEndPoint(myIP, myPort);
            TcpListener tcpListen = new TcpListener(myEP);
            tcpListen.Start();
            Socket sck = tcpListen.AcceptSocket();
            game.AddPlayer(sck);
            sck.Send(s2ba(MakeAnsi(DrawBoard())));
            while (true) System.Threading.Thread.Sleep(100);
            Application.Exit();
        }
        static byte[] s2ba(string str)
        {
            return System.Text.Encoding.BigEndianUnicode.GetBytes(str);
        }
        static string DrawBoard()
        {
            string sBoard = " ";
            for (double y = 0; y < BoardSize.Y + 0.5; y += 0.5)
            {
                for (double x = 0; x < BoardSize.X + 0.5; x += 0.5)
                {
                    if (x % 1 != 0 && // [ ]
                        y % 1 != 0)   // [ ]
                        sBoard += "/";
                    else if (
                        x % 1 != 0 && // [ ]
                        y % 1 == 0)   // [X]
                        sBoard += "%a";
                    else if (
                        x % 1 == 0 && // [X]
                        y % 1 != 0)   // [ ]
                        sBoard += "%b";
                    else if (
                        x > 0 && x < BoardSize.X &&
                        y > 0 && y < BoardSize.Y)
                        sBoard += "%c";// ┼
                    else if (
                        x == 0 &&
                        y == 0)
                        sBoard += "%d";// ┌
                    else if (
                        x == BoardSize.X &&
                        y == 0)
                        sBoard += "%e";// ┐
                    else if (
                        x == 0 &&
                        y == BoardSize.Y)
                        sBoard += "%f";// └
                    else if (
                        x == BoardSize.X &&
                        y == BoardSize.Y)
                        sBoard += "%g";// ┘
                    else if (
                        x == 0)
                        sBoard += "%h";// ├
                    else if (
                        x == BoardSize.X)
                        sBoard += "%i";// ┤
                    else if (
                        y == 0)
                        sBoard += "%j";// ┬
                    else if (
                        y == BoardSize.Y)
                        sBoard += "%k";// ┴
                    else sBoard += "o";// ?
                }
                if (y < BoardSize.Y)
                    sBoard += "\r\n ";
            }
            return MakeAnsi(sBoard);
        }
        static string MakeAnsi(string str)
        {
            string[] chA = new string[] { "%a", "%b", "%c",
                "%d", "%e", "%f", "%g", "%h", "%i", "%j", "%k" };
            char[] chB = new char[] { (char)196, (char)179, (char)197,
                (char)218, (char)191, (char)192, (char)217, (char)195,
                (char)180, (char)194, (char)193 };
            //char[] chB = new char[] { '─', '│', '┼', 
            //    '┌', '┐', '└', '┘', '├', '┤', '┬', '┴' };
            for (int a = 0; a < chA.Length; a++)
                str = str.Replace(chA[a], "" + chB[a]);
            return str;
        }
    }
}
