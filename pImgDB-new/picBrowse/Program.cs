using System;
using System.Collections.Generic;
using System.Windows.Forms;

namespace picBrowse
{
    static class Program
    {
        public static bool Linux;
        /// <summary>
        /// The main entry point for the application.
        /// </summary>
        [STAThread]
        static void Main()
        {
            //Application.EnableVisualStyles();
            Linux = System.IO.File.Exists(".linux");
            Application.SetCompatibleTextRenderingDefault(false);
            Application.Run(new frmMain());
        }
    }
}
