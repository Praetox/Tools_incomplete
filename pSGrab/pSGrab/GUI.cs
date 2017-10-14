using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
//using System.Linq;
using System.Text;
using System.Windows.Forms;
using System.Net;
using System.IO;
using System.Collections;
//using System.Threading;

namespace pSGrab
{
    class GUI
    {
        public static z.Skin sk;
        public static string sAppPath = "";
        public static string sAppVer = "";
        private static ArrayList ScriptQue = new ArrayList(); //ScrDDL
        
        public static string DispRsp = "";
        public static string DispRspV = "WHO WAS PHONE?! 58189512";
        public static string DispRspW = "There is no phone. 58189512";
        public static ArrayList DispQue = new ArrayList(); //String
        private static Timer DispTim = new Timer();
        private static Timer DownTim = new Timer();
        private static bool DispTimB = false;
        private static bool bGrabDone = false;
        private static Color ParamBack = Color.Black;
        private static Color ParamFore = Color.White;
        public static bool bDebug;

        #region Generic shit
        public static void FileWrite(string sPath, string sValue, bool bAppend)
        {
            FileMode fmod = FileMode.Create; if (bAppend) fmod = FileMode.Append;
            System.IO.FileStream fs = new FileStream(sPath, fmod, FileAccess.Write);
            byte[] bValue = System.Text.Encoding.UTF8.GetBytes(sValue);
            fs.Write(bValue, 0, bValue.Length); fs.Close(); fs.Dispose();
        }
        public static string FileRead(string sPath)
        {
            FileStream fs = new FileStream(sPath, FileMode.Open, FileAccess.Read);
            byte[] bValue = new byte[fs.Length]; fs.Read(bValue, 0, bValue.Length);
            fs.Close(); fs.Dispose(); return System.Text.Encoding.UTF8.GetString(bValue);
        }
        public static string[] Split(string str, string del)
        {
            return str.Split(new string[]{del}, StringSplitOptions.None);
        }
        #endregion

        public static void Main_Load(object sender, EventArgs e)
        {
            sAppPath = Application.StartupPath.Replace("\\", "/");
            if (!sAppPath.EndsWith("/")) sAppPath += "/";
            sAppVer = Application.ProductVersion;
            sAppVer = sAppVer.Substring(0, sAppVer.LastIndexOf(".")-1);
            //sk.cnt.GetFrm("Main").Invalidate();
            Application.DoEvents();
            //new frmTemplate().Show();

            if (!System.IO.Directory.Exists("Scripts"))
                System.IO.Directory.CreateDirectory("Scripts");
            if (!System.IO.Directory.Exists("Params"))
                System.IO.Directory.CreateDirectory("Params");

            //sk.cnt.GetLst("Scr_Log").Items.Add("Scriptable File Downloader          v1.0.0");
            DispQue.Add("Scriptable File Downloader          v1.0.0                                     #");
            DispQue.Add("");
            DispQue.Add(" ####    ####   ####  ####    ###   ####  ");
            DispQue.Add(" #   #  #      #      #   #  #   #  #   # ");
            DispQue.Add(" ####    ###   #  ##  ####   #####  ####  ");
            DispQue.Add(" #          #  #   #  #   #  #   #  #   # ");
            DispQue.Add(" #      ####    ####  #   #  #   #  ####  ");
            DispQue.Add("");
            DispQue.Add("For 80x25 consoles      http://praetox.com");
            DispQue.Add("");
            Scr_cmdScript_Click(new object(), new
                MouseEventArgs(MouseButtons.Left, 1, 1, 1, 1));

            DispTim.Tick += new EventHandler(DispTim_Tick);
            DownTim.Tick += new EventHandler(DownTim_Tick);
            DispTim.Interval = 1;
            DownTim.Interval = 50;
            DispTim.Start();
            DownTim.Start();
        }
        static void DispTim_Tick(object sender, EventArgs e)
        {
            if (DispTimB) return; DispTimB = true;
            while (DispQue.Count > 0)
            {
                string s = (string)DispQue[0];
                if (s.StartsWith("++"))
                {
                    s = s.Substring(2);
                    int i = sk.cnt.GetLst("Scr_Log").Items.Count;
                    sk.cnt.GetLst("Scr_Log").Items[i - 1] += s;
                }
                else sk.cnt.GetLst("Scr_Log").Items.Add(DispQue[0]);
                DispQue.RemoveAt(0);
                sk.cnt.GetLst("Scr_Log").SelectedIndex =
                    sk.cnt.GetLst("Scr_Log").Items.Count - 1;
            }
            if (bGrabDone)
            {
                bGrabDone = false;
                sk.cnt.GetBtn("JobQue_cmdExecSel").Enabled = true;
                sk.cnt.GetBtn("JobQue_cmdExecAll").Enabled = true;
                sk.cnt.GetBtn("JobQue_cmdLoad").Enabled = true;
                sk.cnt.GetBtn("Scr_cmdStart").Enabled = true;
            }
            if (DispRsp == DispRspV)
            {
                DispRsp = DispRspW;
                ParamBack = sk.cnt.Get("Scr_Param").BackColor;
                ParamFore = sk.cnt.Get("Scr_Param").ForeColor;
                sk.cnt.Get("Scr_Param").BackColor = Color.Maroon;
                sk.cnt.Get("Scr_Param").ForeColor = Color.White;
            }
            DispTimB = false;
        }
        static void DownTim_Tick(object sender, EventArgs e)
        {
            string sColA = ""; string sColB = "";
            for (int a = 0; a < DLMan.Cli.Length; a++)
            {
                if (DLMan.Cli[a] != null)
                {
                    sColB += DLMan.Cli[a].Info.sFName + "\r\n";
                    if (DLMan.Cli[a].State == WebGet.eState.Connecting) sColA += "conn" + "\r\n";
                    if (DLMan.Cli[a].State == WebGet.eState.Completed) sColA += "done" + "\r\n";
                    if (DLMan.Cli[a].State == WebGet.eState.Downloading) sColA += Math.Round(
                        (100 / (double)DLMan.Cli[a].iLen) *
                        (double)DLMan.Cli[a].iRecv) + "%" + "\r\n";
                }
                else
                {
                    sColA += "n/a" + "\r\n";
                    sColB += "n/a" + "\r\n";
                }
            }
            sk.cnt.Get("cli_Progress").Text = sColA;
            sk.cnt.Get("cli_Filename").Text = sColB;
        }

        public static void Scr_cmdScript_Click(object sender, MouseEventArgs e)
        {
            string sScriptPath = sAppPath + "Scripts";
            string[] saScripts = System.IO.Directory.GetFiles(sScriptPath);
            sk.cnt.GetDrp("Scr_cbScript").Items.Clear();
            for (int a = 0; a < saScripts.Length; a++)
                sk.cnt.GetDrp("Scr_cbScript").Items.Add
                    (saScripts[a].Substring(sScriptPath.Length + 1));
        }
        public static void Scr_cmdPath_Click(object sender, MouseEventArgs e)
        {
            FolderBrowserDialog fbd = new FolderBrowserDialog();
            fbd.ShowDialog(); string sPath = fbd.SelectedPath;
            if (sPath == "") return;
            sk.cnt.Get("Scr_txtPath").Text = sPath;
        }
        public static void ParQue_cmdSave_Click(object sender, MouseEventArgs e)
        {
            string sScrPath = sk.cnt.Get("Scr_cbScript").Text;
            sScrPath = sScrPath.Substring(0, sScrPath.Length - 3) + "prm";
            string sScrPrms = sk.cnt.Get("ParQue_txtQue").Text;
            FileWrite(sAppPath + "params\\" + sScrPath, sScrPrms, false);
        }
        public static void ParQue_cmdLoad_Click(object sender, MouseEventArgs e)
        {
            string sScrPath = sk.cnt.Get("Scr_cbScript").Text;
            sScrPath = sScrPath.Substring(0, sScrPath.Length - 3) + "prm";
            sk.cnt.Get("ParQue_txtQue").Text = FileRead(sAppPath + "params\\" + sScrPath);
        }
        public static void Scr_cmdAdd_Click(object sender, MouseEventArgs e)
        {
            ScrDDL myScrDDL = MakeScrDDL(
                sk.cnt.Get("Scr_cbScript").Text,
                sk.cnt.Get("Scr_txtPath").Text,
                sk.cnt.Get("ParQue_txtQue").Text);
            ScriptQue.Add(myScrDDL);
            RedrawDDLQue();
        }
        public static void Scr_cmdStart_Click(object sender, MouseEventArgs e)
        {
            ScrDDL myScrDDL = MakeScrDDL(
                sk.cnt.Get("Scr_cbScript").Text,
                sk.cnt.Get("Scr_txtPath").Text,
                sk.cnt.Get("ParQue_txtQue").Text);
        }
        public static void RedrawDDLQue()
        {
            sk.cnt.GetLst("JobQue_lstQue").Items.Clear();
            for (int a = 0; a < ScriptQue.Count; a++)
            {
                string sAdd = ((ScrDDL)ScriptQue[a]).sPath;
                for (int b = 0; b < 3; b++)
                {
                    if (b >= ((ScrDDL)ScriptQue[a]).aPr.Count) break;
                    sAdd += "  -  " + ((string)((ScrDDL)ScriptQue[a]).aPr[b]);
                }
                sk.cnt.GetLst("JobQue_lstQue").Items.Add(sAdd);
            }
        }
        private static ScrDDL MakeScrDDL(string sPath, string sTarg, string sParms)
        {
            ScrDDL ret = new ScrDDL();
            ret.sPath = sPath;
            ret.sTarg = sTarg;
            
            int iThreads = Convert.ToInt32(sk.cnt.Get("ddl_threads").Text);
            ret.scConf = new ServCF(Math.Min(10, iThreads));
            
            string sScrpt = FileRead(sAppPath + "scripts\\" + sPath);
            string[] aScrpt = sScrpt.Replace("\r", "").Split('\n');
            for (int a = 0; a < aScrpt.Length; a++)
                if (aScrpt[a] != "") ret.aSc.Add(aScrpt[a]);

            string[] aParms = Split(sParms.Replace("\r", ""), "\n");
            for (int a = 0; a < aParms.Length; a++)
                if (aParms[a] != "") ret.aPr.Add(aParms[a]);

            return ret;
        }
        public static void JobQue_lstQue_Select(object sender, EventArgs e)
        {
            int iSel = sk.cnt.GetLst("JobQue_lstQue").SelectedIndex;
            if (iSel < 0) return;
            ScrDDL mySqrDDL = (ScrDDL)ScriptQue[iSel];
            for (int a = 0; a < sk.cnt.GetDrp("Scr_cbScript").Items.Count; a++)
            {
                if (sk.cnt.GetDrp("Scr_cbScript").Items[a].ToString() == mySqrDDL.sPath)
                    sk.cnt.GetDrp("Scr_cbScript").SelectedIndex = a;
            }
            if (sk.cnt.GetDrp("Scr_cbScript").Text != mySqrDDL.sPath)
                sk.cnt.GetDrp("Scr_cbScript").Text = "UNKNOWN SCRIPT";

            sk.cnt.Get("Scr_txtPath").Text = mySqrDDL.sTarg;

            sk.cnt.Get("ParQue_txtQue").Text = "";
            for (int a = 0; a < mySqrDDL.aPr.Count; a++)
            {
                sk.cnt.Get("ParQue_txtQue").Text += (string)mySqrDDL.aPr[a] + "\r\n";
            }
        }
        public static void JobQue_lstQue_RemSel()
        {
            int iSel = sk.cnt.GetLst("JobQue_lstQue").SelectedIndex;
            while (sk.cnt.GetLst("JobQue_lstQue").SelectedIndices.Count > 0)
            {
                int iRem = sk.cnt.GetLst("JobQue_lstQue").SelectedIndices[0];
                sk.cnt.GetLst("JobQue_lstQue").Items.RemoveAt(iRem);
                ScriptQue.RemoveAt(iRem);
            }
            RedrawDDLQue();
        }
        public static void JobQue_lstQue_UpdSel()
        {
            int iSel = sk.cnt.GetLst("JobQue_lstQue").SelectedIndex;
            ScrDDL myScrDDL = MakeScrDDL(
                sk.cnt.Get("Scr_cbScript").Text,
                sk.cnt.Get("Scr_txtPath").Text,
                sk.cnt.Get("ParQue_txtQue").Text);
            ScriptQue[iSel] = myScrDDL;
            RedrawDDLQue();
        }
        public static void JobQue_cmdSave_Click(object sender, MouseEventArgs e)
        {
            string sWrite = "";
            for (int a = 0; a < ScriptQue.Count; a++)
            {
                sWrite += "SCR " + ((ScrDDL)ScriptQue[a]).sPath + "\r\n";
                sWrite += "TRG " + ((ScrDDL)ScriptQue[a]).sTarg + "\r\n";
                for (int b = 0; b < ((ScrDDL)ScriptQue[a]).aPr.Count; b++)
                {
                    string sPar = ((string)((ScrDDL)ScriptQue[a]).aPr[b]);
                    if (sPar != "") sWrite += "PRM " + sPar + "\r\n";
                }
            }
            FileWrite("ddlQue.txt", sWrite, false);
            RedrawDDLQue();
        }
        public static void JobQue_cmdLoad_Click(object sender, MouseEventArgs e)
        {
            ScriptQue.Clear();
            string[] sRaw = FileRead("ddlQue.txt").Replace("\r", "").Split('\n');
            int i = 0; while (i < sRaw.Length)
            {
                if (sRaw[i].StartsWith("SCR "))
                {
                    ScrDDL myScrDDL = new ScrDDL();

                    myScrDDL.sPath = sRaw[i].Substring(4);
                    string sScrpt = FileRead(sAppPath + "scripts\\" + myScrDDL.sPath);
                    string[] aScrpt = sScrpt.Replace("\r", "").Split('\n');
                    for (int a = 0; a < aScrpt.Length; a++)
                        if (aScrpt[a] != "") myScrDDL.aSc.Add(aScrpt[a]);

                    while (true)
                    {
                        i++;
                        if (i >= sRaw.Length) break;
                        if (sRaw[i].StartsWith("SCR ")) break;
                        if (sRaw[i].StartsWith("TRG "))
                            myScrDDL.sTarg = sRaw[i].Substring(4);
                        if (sRaw[i].StartsWith("PRM "))
                            myScrDDL.aPr.Add(sRaw[i].Substring(4));
                    }
                    ScriptQue.Add(myScrDDL);
                }
                else i++;
            }
            RedrawDDLQue();
        }
        public static void JobQue_cmdExecAll_Click(object sender, MouseEventArgs e)
        {
            for (int a = 0; a < ScriptQue.Count; a++)
            {
                ((ScrDDL)ScriptQue[a]).bGet = true;
            }
            doGrabs();
        }
        public static void JobQue_cmdExecSel_Click(object sender, MouseEventArgs e)
        {
            for (int a = 0; a < ScriptQue.Count; a++)
            {
                ((ScrDDL)ScriptQue[a]).bGet = false;
            }
            doGrabs();
        }
        public static void Scr_Param_Confirm()
        {
            sk.cnt.Get("Scr_Param").BackColor = ParamBack;
            sk.cnt.Get("Scr_Param").ForeColor = ParamFore;
            string sTxt = sk.cnt.Get("Scr_Param").Text;
            sk.cnt.Get("Scr_Param").Text = "";
            DispRsp = sTxt;
        }
        public static void doGrabs()
        {
            sk.cnt.GetBtn("Scr_cmdStart").Enabled = false;
            sk.cnt.GetBtn("JobQue_cmdLoad").Enabled = false;
            sk.cnt.GetBtn("JobQue_cmdExecAll").Enabled = false;
            sk.cnt.GetBtn("JobQue_cmdExecSel").Enabled = false;
            bDebug = sk.cnt.GetChk("Scr_Debug").Checked;
            
            System.Threading.Thread th =
                new System.Threading.Thread(new
                    System.Threading.ThreadStart(doGrabsExec));
            th.IsBackground = true; th.Start();
        }
        public static void doGrabsExec()
        {
            for (int a = 0; a < ScriptQue.Count; a++)
            {
                ScrDDL sd = (ScrDDL)ScriptQue[a];
                PSG grabber = new PSG(sd);
                grabber.Exec();
            }
            bGrabDone = true;
            for (int a = 0; a < DLMan.Cli.Length; a++)
                DLMan.Cli[a] = null;
        }
        public static void ddl_cmdSave_Click(object sender, MouseEventArgs e)
        {

        }
        public static void ddl_cmdLoad_Click(object sender, MouseEventArgs e)
        {

        }
    }
}
