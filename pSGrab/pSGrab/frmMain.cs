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

namespace pSGrab
{
    public partial class frmMain : Form
    {
        public frmMain()
        {
            InitializeComponent();
        }
        private void frmMain_Load(object sender, EventArgs e)
        {
            this.Show(); this.Focus(); Application.DoEvents();
            s0.Visible = true; Application.DoEvents();
            GUI.sk = new z.Skin(this, "Main", "skin.papp");
            s1.Visible = true; Application.DoEvents();
            z.Skin.TraceWasted();
            s2.Visible = true; Application.DoEvents();
            GUI.sk.Parse();
            s3.Visible = true; Application.DoEvents();
            GUI.sk.Init(0, null, true);
            s4.Visible = true; Application.DoEvents();
            GUI.sk.Draw();
            s5.Visible = true; Application.DoEvents();
            GUI.sk.Enable();
            tHide.Start();
        }
        private void tHide_Tick(object sender, EventArgs e)
        {
            tHide.Stop(); this.Hide();
            GUI.Main_Load(new object(), new EventArgs());
        }
    }
}
