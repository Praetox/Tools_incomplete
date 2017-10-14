using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;

namespace cssmod
{
    public partial class frmChangeColour : Form
    {
        public frmChangeColour(FormType typ)
        {
            this.typ = typ;
            InitializeComponent();
            if (typ == FormType.Skew ||
                typ == FormType.Change)
            {
                b_Diff_Hue.Enabled = false;
                b_Diff_Sat.Enabled = false;
                b_Diff_Bri.Enabled = false;
            }
            if (typ == FormType.Skew)
            {
                a_Diff_Hue.Enabled = false;
                a_Diff_Sat.Enabled = false;
                a_Diff_Bri.Enabled = false;
            }
        }
        public enum FormType { Full, Change, Skew };
        private FormType typ = FormType.Full;

        public ColorHSB cOne = new ColorHSB(Color.Black);
        public ColorHSB cTwo = new ColorHSB(Color.Black);
        public ColorHSB cOneD = new ColorHSB(Color.Black);
        public ColorHSB cTwoD = new ColorHSB(Color.Black);

        private void a_Dunno_CheckedChanged(object sender, EventArgs e)
        {
            if (!a_Dunno.Checked) return;
            MessageBox.Show("wat");
            a_HEX.Checked = true;
        }

        private void b_Dunno_CheckedChanged(object sender, EventArgs e)
        {
            if (!b_Dunno.Checked) return;
            MessageBox.Show("wat");
            b_HEX.Checked = true;
        }

        private void a_RGB_CheckedChanged(object sender, EventArgs e)
        {
            if (a_ManValue.Text.Contains("R") ||
                a_ManValue.Text.Contains("H"))
            {
                a_ManValue.Text = "RRR, GGG, BBB";
            }
            else RefreshControls(false, true, false);
        }

        private void a_HEX_CheckedChanged(object sender, EventArgs e)
        {
            if (a_ManValue.Text.Contains("R") ||
                a_ManValue.Text.Contains("H"))
            {
                a_ManValue.Text = "#RRGGBB";
            }
            else RefreshControls(false, true, false);
        }

        private void a_HSB_CheckedChanged(object sender, EventArgs e)
        {
            if (a_ManValue.Text.Contains("R") ||
                a_ManValue.Text.Contains("H"))
            {
                a_ManValue.Text = "HHH.SSS.BBB";
            }
            else RefreshControls(false, true, false);
        }

        private void b_RGB_CheckedChanged(object sender, EventArgs e)
        {
            if (b_ManValue.Text.Contains("R") ||
                b_ManValue.Text.Contains("H"))
            {
                b_ManValue.Text = "RRR, GGG, BBB";
            }
            else RefreshControls(false, true, false);
        }

        private void b_HEX_CheckedChanged(object sender, EventArgs e)
        {
            if (b_ManValue.Text.Contains("R") ||
                b_ManValue.Text.Contains("H"))
            {
                b_ManValue.Text = "#RRGGBB";
            }
            else RefreshControls(false, true, false);
        }

        private void b_HSB_CheckedChanged(object sender, EventArgs e)
        {
            if (b_ManValue.Text.Contains("R") ||
                b_ManValue.Text.Contains("H"))
            {
                b_ManValue.Text = "HHH.SSS.BBB";
            }
            else RefreshControls(false, true, false);
        }

        private void SetFromSliders()
        {
            float fh = a_Set_Hue.Value;
            float fs = a_Set_Sat.Value;
            float fv = a_Set_Bri.Value;
            float fhd = a_Diff_Hue.Value;
            float fsd = a_Diff_Sat.Value;
            float fvd = a_Diff_Bri.Value;
            cOne = new ColorHSB(fh, fs, fv);
            cOneD = new ColorHSB(fhd, fsd, fvd);

            float th = b_Set_Hue.Value;
            float ts = b_Set_Sat.Value;
            float tv = b_Set_Bri.Value;
            float thd = b_Diff_Hue.Value;
            float tsd = b_Diff_Sat.Value;
            float tvd = b_Diff_Bri.Value;
            cTwo = new ColorHSB(th, ts, tv);
            cTwoD = new ColorHSB(thd, tsd, tvd);
            RefreshControls(true, true, false);
        }
        private void RefreshControls(bool bGUI, bool bManual, bool bSliders)
        {
            int fr = cOne.Color.R;
            int fg = cOne.Color.G;
            int fb = cOne.Color.B;
            int fh = (int)Math.Round(cOne.H);
            int fs = (int)Math.Round(cOne.S);
            int fv = (int)Math.Round(cOne.B);
            if (fh == 360) fh = 0;

            int fhd = (int)Math.Round(cOneD.H);
            int fsd = (int)Math.Round(cOneD.S);
            int fvd = (int)Math.Round(cOneD.B);
            if (fhd == 360) fhd = 0;

            int tr = cTwo.Color.R;
            int tg = cTwo.Color.G;
            int tb = cTwo.Color.B;
            int th = (int)Math.Round(cTwo.H);
            int ts = (int)Math.Round(cTwo.S);
            int tv = (int)Math.Round(cTwo.B);
            if (th == 360) th = 0;

            int thd = (int)Math.Round(cTwoD.H);
            int tsd = (int)Math.Round(cTwoD.S);
            int tvd = (int)Math.Round(cTwoD.B);
            if (thd == 360) thd = 0;

            if (bManual)
            {
                if (a_RGB.Checked) a_ManValue.Text = fr.ToString("d") + ", " + fg.ToString("d") + ", " + fb.ToString("d");
                if (a_HSB.Checked) a_ManValue.Text = fh.ToString("d") + "." + fs.ToString("d") + "." + fv.ToString("d");
                if (a_HEX.Checked) a_ManValue.Text = "#" + fr.ToString("X2") + fg.ToString("X2") + fb.ToString("X2");

                if (b_RGB.Checked) b_ManValue.Text = tr.ToString("d") + ", " + tg.ToString("d") + ", " + tb.ToString("d");
                if (b_HSB.Checked) b_ManValue.Text = th.ToString("d") + "." + ts.ToString("d") + "." + tv.ToString("d");
                if (b_HEX.Checked) b_ManValue.Text = "#" + tr.ToString("X2") + tg.ToString("X2") + tb.ToString("X2");
            }
            if (bSliders)
            {
                a_Set_Hue.Value = fh;
                a_Set_Sat.Value = fs;
                a_Set_Bri.Value = fv;
                a_Diff_Hue.Value = fhd;
                a_Diff_Sat.Value = fsd;
                a_Diff_Bri.Value = fvd;

                b_Set_Hue.Value = th;
                b_Set_Sat.Value = ts;
                b_Set_Bri.Value = tv;
                b_Diff_Hue.Value = thd;
                b_Diff_Sat.Value = tsd;
                b_Diff_Bri.Value = tvd;
            }
            if (bGUI)
            {
                ColorHSB cMinA = new ColorHSB(cOne.H - fhd, cOne.S - fsd, cOne.B - fvd);
                ColorHSB cMaxA = new ColorHSB(cOne.H + fhd, cOne.S + fsd, cOne.B + fvd);
                a_Min.BackColor = cMinA.Color;
                a_Avg.BackColor = cOne.Color;
                a_Max.BackColor = cMaxA.Color;

                ColorHSB cMinB = new ColorHSB(cTwo.H - thd, cTwo.S - tsd, cTwo.B - tvd);
                ColorHSB cMaxB = new ColorHSB(cTwo.H + thd, cTwo.S + tsd, cTwo.B + tvd);
                b_Min.BackColor = cMinB.Color;
                b_Avg.BackColor = cTwo.Color;
                b_Max.BackColor = cMaxB.Color;
            }
        }
        private void a_WinPick_Click(object sender, EventArgs e)
        {
            ColorDialog cd = new ColorDialog();
            cd.FullOpen = true; cd.ShowDialog();
            if (cd.Color == null) return;
            cOne = new ColorHSB(cd.Color);
            RefreshControls(true, true, true);
        }
        private void b_WinPick_Click(object sender, EventArgs e)
        {
            ColorDialog cd = new ColorDialog();
            cd.FullOpen = true; cd.ShowDialog();
            if (cd.Color == null) return;
            cTwo = new ColorHSB(cd.Color);
            RefreshControls(true, true, true);
        }
        private void SetManually(int iSauce, string sValues, bool bRGB, bool bHEX, bool bHSB)
        {
            if (bRGB)
            {
                try
                {
                    string[] vals = sValues.Split(',');
                    if (vals.Length != 3) throw new Exception();
                    int r = Convert.ToInt32(vals[0].Trim());
                    int g = Convert.ToInt32(vals[1].Trim());
                    int b = Convert.ToInt32(vals[2].Trim());
                    ColorHSB col = new ColorHSB(Color.FromArgb(r, g, b));
                    if (iSauce == 1) cOne = col; else cTwo = col;
                }
                catch
                {
                    MessageBox.Show("That is not a valid RGB colour!" + "\r\n\r\n" +
                        "Valid example: 96, 192, 255", "You are doing it wrong.",
                        MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
                }
            }
            if (bHEX)
            {
                string val = sValues.Substring(1);
                if (val.Length == 3)
                {
                    try
                    {
                        int r = Convert.ToInt32(val[0].ToString(), 16) * 17;
                        int g = Convert.ToInt32(val[1].ToString(), 16) * 17;
                        int b = Convert.ToInt32(val[2].ToString(), 16) * 17;
                        ColorHSB col = new ColorHSB(Color.FromArgb(r, g, b));
                        if (iSauce == 1) cOne = col; else cTwo = col;
                    }
                    catch
                    {
                        MessageBox.Show("That is not a valid 3-digit hexadecimal colour!" + "\r\n\r\n" +
                            "Valid example: #8ac" + "\r\n" + "Valid example: #1379AE",
                            "You are doing it wrong.", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
                    }
                }
                else if (val.Length == 6)
                {
                    try
                    {
                        int r = Convert.ToInt32(val.Substring(0, 2), 16);
                        int g = Convert.ToInt32(val.Substring(2, 2), 16);
                        int b = Convert.ToInt32(val.Substring(4, 2), 16);
                        ColorHSB col = new ColorHSB(Color.FromArgb(r, g, b));
                        if (iSauce == 1) cOne = col; else cTwo = col;
                    }
                    catch
                    {
                        MessageBox.Show("That is not a valid 6-digit hexadecimal colour!" + "\r\n\r\n" +
                            "Valid example: #8ac" + "\r\n" + "Valid example: #1379AE",
                            "You are doing it wrong.", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
                    }
                }
                else
                {
                    MessageBox.Show("That is not a valid hexadecimal colour!" + "\r\n\r\n" +
                        "Valid example: #8ac" + "\r\n" + "Valid example: #1379AE",
                        "You are doing it wrong.", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
                }
            }
            if (bHSB)
            {
                try
                {
                    string[] vals = sValues.Split('.');
                    if (vals.Length != 3) throw new Exception();
                    int h = Convert.ToInt32(vals[0].Trim());
                    int s = Convert.ToInt32(vals[1].Trim());
                    int b = Convert.ToInt32(vals[2].Trim());
                    ColorHSB col = new ColorHSB(h, s, b);
                    if (iSauce == 1) cOne = col; else cTwo = col;
                }
                catch
                {
                    MessageBox.Show("That is not a valid HSV colour!" + "\r\n\r\n" +
                        "Valid example: 320, 224, 128", "You are doing it wrong.",
                        MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
                }
            }
            RefreshControls(true, true, true);
        }
        private void a_Manually_Click(object sender, EventArgs e)
        {
            SetManually(1, a_ManValue.Text, a_RGB.Checked, a_HEX.Checked, a_HSB.Checked);
        }
        private void b_Manually_Click(object sender, EventArgs e)
        {
            SetManually(2, b_ManValue.Text, b_RGB.Checked, b_HEX.Checked, b_HSB.Checked);
        }

        private void a_Set_Hue_Scroll(object sender, EventArgs e)
        {
            SetFromSliders();
        }
        private void a_Set_Sat_Scroll(object sender, EventArgs e)
        {
            SetFromSliders();
        }
        private void a_Set_Bri_Scroll(object sender, EventArgs e)
        {
            SetFromSliders();
        }
        private void a_Diff_Hue_Scroll(object sender, EventArgs e)
        {
            if (typ != FormType.Full)
                b_Diff_Hue.Value = a_Diff_Hue.Value;
            SetFromSliders();
        }
        private void a_Diff_Sat_Scroll(object sender, EventArgs e)
        {
            if (typ != FormType.Full)
                b_Diff_Sat.Value = a_Diff_Sat.Value; 
            SetFromSliders();
        }
        private void a_Diff_Bri_Scroll(object sender, EventArgs e)
        {
            if (typ != FormType.Full)
                b_Diff_Bri.Value = a_Diff_Bri.Value; 
            SetFromSliders();
        }
        private void b_Set_Hue_Scroll(object sender, EventArgs e)
        {
            SetFromSliders();
        }
        private void b_Set_Sat_Scroll(object sender, EventArgs e)
        {
            SetFromSliders();
        }
        private void b_Set_Bri_Scroll(object sender, EventArgs e)
        {
            SetFromSliders();
        }
        private void b_Diff_Hue_Scroll(object sender, EventArgs e)
        {
            if (typ == FormType.Full)
            SetFromSliders();
        }
        private void b_Diff_Sat_Scroll(object sender, EventArgs e)
        {
            if (typ == FormType.Full)
            SetFromSliders();
        }
        private void b_Diff_Bri_Scroll(object sender, EventArgs e)
        {
            if (typ == FormType.Full)
            SetFromSliders();
        }

        private void cmdGo_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void cmdCancel_Click(object sender, EventArgs e)
        {
            cOne = null; cTwo = null;
            cOneD = null; cTwoD = null;
            this.Close();
        }

        private void frmChangeColour_Load(object sender, EventArgs e)
        {

        }
    }
}
