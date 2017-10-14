using System;
using System.Collections.Generic;
using System.Text;
using System.Drawing;
using System.Windows.Forms;

namespace z
{
    public class Skin
    {
        public const int WM_NCLBUTTONDOWN = 0xA1;
        public const int HT_CAPTION = 0x2;
        [System.Runtime.InteropServices.DllImportAttribute("user32.dll")]
        public static extern int SendMessage(IntPtr hWnd, int Msg, int wParam, int lParam);
        [System.Runtime.InteropServices.DllImportAttribute("user32.dll")]
        public static extern bool ReleaseCapture();

        public static void TraceWasted()
        {
            Form con = new Form();
            Label lab = new Label();
            con.Controls.Add(lab);
            lab.Dock = DockStyle.Fill;
            for (int a = 0; a < WasteType.Length; a++)
            {
                con.FormBorderStyle = WasteType[a];
                con.Width += 1;
                WasteSize[a] = new Point(
                    con.Width - lab.Width,
                    con.Height - lab.Height);
            }
            lab.Dispose(); con.Dispose();
        }
        public static int GetWasted(Form frm)
        {
            for (int a = 0; a < WasteType.Length; a++)
            {
                if (frm.FormBorderStyle ==
                    WasteType[a]) return a;
            }
            return 0;
        }
        public static Type typFrm = new Form().GetType();
        public static Type typPnl = new Panel().GetType();
        public static ToolTip ttip = new ToolTip();
        public static FormBorderStyle[] WasteType =
          { FormBorderStyle.None,
            FormBorderStyle.Sizable,
            FormBorderStyle.FixedSingle,
            FormBorderStyle.SizableToolWindow,
            FormBorderStyle.FixedToolWindow };
        public static Point[] WasteSize = 
          { new Point(0,0),
            new Point(0,0),
            new Point(0,0),
            new Point(0,0),
            new Point(0,0) };

        public Con cnt = new Con();
        private bool bBusy = false;
        private ConInf conTarget = new ConInf();
        private readonly string sTarget;
        private readonly string sSkinF;
        private string[] sSkin;
        private string sAppPath;
        private Timer tRedraw;
        private Point ptResize;
        private Size szResize;

        public Skin(Control Target, string Alias, string Skinfile)
        {
            sTarget = Alias;
            sSkinF = Skinfile;
            conTarget.oCon = Target;
            conTarget.sName = "SKIN_ROOT";
            sAppPath = Application.StartupPath.Replace("/", "\\");
            if (!sAppPath.EndsWith("\\")) sAppPath += "\\";
            tRedraw = new Timer();
            tRedraw.Interval = 50;
            tRedraw.Enabled = false;
            tRedraw.Tick += new EventHandler(tRedraw_Tick);
        }
        public void Enable()
        {
            //Parse(); Init(0, null); Draw();
            for (int a = 0; a < cnt.Cons.Count; a++)
            {
                if (cnt.Inf(a).uType == z.Con.uFrm)
                {
                    cnt.Inf(a).ptLoc = new string[0];
                    cnt.Inf(a).szSize = new string[0];
                }
            }
        }
        public void Parse()
        {
            if (bBusy) return; bBusy = true;
            System.IO.FileStream fs = new System.IO.FileStream(sSkinF,
                System.IO.FileMode.Open, System.IO.FileAccess.Read);
            byte[] bSkin = new byte[fs.Length];
            fs.Read(bSkin, 0, bSkin.Length);
            fs.Close(); fs.Dispose();

            string tsSkin = System.Text.Encoding.UTF8
                .GetString(bSkin).Replace("\r", "\n");
            while (tsSkin.Contains("\n\n")) tsSkin =
                tsSkin.Replace("\n\n", "\n");
            sSkin = tsSkin.Split('\n');
            for (int a = 0; a < sSkin.Length; a++)
            {
                sSkin[a] = sSkin[a].Trim(' ', '\t');
            }
            bBusy = false;
        }
        public int Init(int a, Control conParent, bool bInitial)
        {
            if (bInitial) bBusy = true;
            int iTTyp = -1; int iThis = -1;
            while (!sSkin[a].StartsWith("!") &&
                !sSkin[a].StartsWith("#")) a++;

            z.ConInf ciParent = new ConInf();
            if (conParent != null)
            {
                ciParent.oCon = conParent;
                ciParent.sName = conParent.Name;
                if (conParent.GetType() == typFrm) ciParent.uType = z.Con.uFrm;
                if (conParent.GetType() == typPnl) ciParent.uType = z.Con.uPnl;
            }

            FontFamily fFamily = new FontFamily("arial");
            FontStyle fStyle = FontStyle.Regular; float fSize = 8F;
            try
            {
                fFamily = ciParent.cCon().Font.FontFamily;
                fStyle = ciParent.cCon().Font.Style;
                fSize = ciParent.cCon().Font.Size;
            }
            catch { }

            if (sSkin[a].StartsWith("#")) //Window
            {
                iTTyp = z.Con.uFrm; iThis = cnt.Cons.Count;
                string sName = sSkin[a].Substring("#".Length).Trim(' ', '{');
                cnt.Add(iTTyp, sName, new z.ConInf()); a++;
                cnt.Get(iThis).Resize += new EventHandler(Skin_Resize);
                cnt.GetFrm(iThis).FormClosed += new FormClosedEventHandler(Skin_FormClosed);
                cnt.Get(iThis).Show();
            }
            if (sSkin[a].StartsWith("!panel ")) //Panel
            {
                iTTyp = z.Con.uPnl; iThis = cnt.Cons.Count;
                string sName = sSkin[a].Substring("!panel ".Length).Trim(' ', '{');
                cnt.Add(iTTyp, sName, ciParent); a++;
            }
            if (sSkin[a].StartsWith("!image ")) //Image
            {
                iTTyp = z.Con.uPic; iThis = cnt.Cons.Count;
                string sName = sSkin[a].Substring("!image ".Length).Trim(' ', '{');
                cnt.Add(iTTyp, sName, ciParent); a++;
            }
            if (sSkin[a].StartsWith("!button ")) //Button
            {
                iTTyp = z.Con.uBtn; iThis = cnt.Cons.Count;
                string sName = sSkin[a].Substring("!button ".Length).Trim(' ', '{');
                cnt.Add(iTTyp, sName, ciParent); a++;
            }
            if (sSkin[a].StartsWith("!listbox ")) //Listbox
            {
                iTTyp = z.Con.uLst; iThis = cnt.Cons.Count;
                string sName = sSkin[a].Substring("!listbox ".Length).Trim(' ', '{');
                cnt.Add(iTTyp, sName, ciParent); a++;
            }
            if (sSkin[a].StartsWith("!dropdown ")) //Combo
            {
                iTTyp = z.Con.uDrp; iThis = cnt.Cons.Count;
                string sName = sSkin[a].Substring("!dropdown ".Length).Trim(' ', '{');
                cnt.Add(iTTyp, sName, ciParent); a++;
            }
            if (sSkin[a].StartsWith("!textbox ")) //Textbox
            {
                iTTyp = z.Con.uTxt; iThis = cnt.Cons.Count;
                string sName = sSkin[a].Substring("!textbox ".Length).Trim(' ', '{');
                cnt.Add(iTTyp, sName, ciParent); a++;
            }
            if (sSkin[a].StartsWith("!checkbox ")) //Checkbox
            {
                iTTyp = z.Con.uChk; iThis = cnt.Cons.Count;
                string sName = sSkin[a].Substring("!checkbox ".Length).Trim(' ', '{');
                cnt.Add(iTTyp, sName, ciParent); a++;
            }
            if (sSkin[a].StartsWith("!label ")) //Label
            {
                iTTyp = z.Con.uLbl; iThis = cnt.Cons.Count;
                string sName = sSkin[a].Substring("!label ".Length).Trim(' ', '{');
                cnt.Add(iTTyp, sName, ciParent); a++;
            }
            while (true)
            {
                while (true)
                {
                    int a2 = a;
                    if (a >= sSkin.Length) break;
                    if (sSkin[a] == "") { a++; continue; }
                    if (sSkin[a].StartsWith("//")) { a++; continue; }
                    if (sSkin[a].StartsWith("!")) a = Init(a, cnt.Get(iThis), false) + 1;
                    if (sSkin[a].StartsWith("#")) a = Init(a, cnt.Get(iThis), false) + 1;
                    if (a == a2) break;
                }
                if (a >= sSkin.Length) break;
                if (sSkin[a] == "}") break;

                string sMod = sSkin[a].Substring(0, sSkin[a].IndexOf(":"));
                string sVal = sSkin[a].Substring(sMod.Length + 1).Trim(' ', '\t');
                if (sVal.EndsWith(";")) sVal = sVal.Substring(0, sVal.Length - 1);
                if (sMod.StartsWith("event-"))
                    cnt.Event(iThis, sMod.Substring(sMod.IndexOf("-") + 1), sVal);

                if (sMod == "size") cnt.Inf(iThis).szSize = sVal.Replace(" ", "").Split(';');
                if (sMod == "location") cnt.Inf(iThis).ptLoc = sVal.Replace(" ", "").Split(';');
                if (sMod == "location-x") cnt.Inf(iThis).ptLoc[0] = sVal.Replace(" ", "");
                if (sMod == "location-y") cnt.Inf(iThis).ptLoc[1] = sVal.Replace(" ", "");
                if (sMod == "size-x") cnt.Inf(iThis).szSize[0] = sVal.Replace(" ", "");
                if (sMod == "size-y") cnt.Inf(iThis).szSize[1] = sVal.Replace(" ", "");
                if (sMod == "text")
                {
                    sVal = sVal.Replace("\\n", "\n");
                    cnt.Get(iThis).Text = sVal.Substring(1, sVal.Length - 2);
                }
                if (sMod == "text-align")
                {
                    HorizontalAlignment horz = new HorizontalAlignment();
                    if (sVal == "l") horz = HorizontalAlignment.Left;
                    if (sVal == "m") horz = HorizontalAlignment.Center;
                    if (sVal == "r") horz = HorizontalAlignment.Right;
                    ContentAlignment cona = new ContentAlignment();
                    if (sVal == "tl") cona = ContentAlignment.TopLeft;
                    if (sVal == "tm") cona = ContentAlignment.TopCenter;
                    if (sVal == "tr") cona = ContentAlignment.TopRight;
                    if (sVal == "ml") cona = ContentAlignment.MiddleLeft;
                    if (sVal == "mm") cona = ContentAlignment.MiddleCenter;
                    if (sVal == "mr") cona = ContentAlignment.MiddleRight;
                    if (sVal == "bl") cona = ContentAlignment.BottomLeft;
                    if (sVal == "bm") cona = ContentAlignment.BottomCenter;
                    if (sVal == "br") cona = ContentAlignment.BottomRight;
                    if (iTTyp == z.Con.uBtn) cnt.GetBtn(iThis).TextAlign = cona;
                    if (iTTyp == z.Con.uLbl) cnt.GetLbl(iThis).TextAlign = cona;
                    if (iTTyp == z.Con.uChk) cnt.GetChk(iThis).TextAlign = cona;
                    if (iTTyp == z.Con.uTxt) cnt.GetTxt(iThis).TextAlign = horz;
                }
                if (sMod == "enabled")
                {
                    if (sVal == "yes") cnt.Get(iThis).Enabled = true;
                    if (sVal == "no") cnt.Get(iThis).Enabled = false;
                }
                if (sMod == "multiline")
                {
                    if (sVal == "yes") cnt.GetTxt(iThis).Multiline = true;
                    if (sVal == "no") cnt.GetTxt(iThis).Multiline = false;
                }
                if (sMod == "checked")
                {
                    if (sVal == "yes") cnt.GetChk(iThis).Checked = true;
                    if (sVal == "no") cnt.GetChk(iThis).Checked = false;
                }
                if (sMod == "draggable")
                {
                    if (sVal == "window") cnt.Get(iThis).MouseDown +=
                        new MouseEventHandler(c_DragForm);
                }
                if (sMod == "resize")
                {
                    if (sVal == "window")
                    {
                        cnt.Get(iThis).MouseDown += new MouseEventHandler(c_ResizeForm_Down);
                        cnt.Get(iThis).MouseMove += new MouseEventHandler(c_ResizeForm_Move);
                    }
                }
                if (sMod == "tip")
                {
                    sVal = sVal.Substring(1, sVal.Length - 2);
                    sVal = sVal.Replace("\\n", "\r\n");
                    ttip.SetToolTip(cnt.Get(iThis), sVal);
                }
                if (sMod == "color-main" || sMod == "color-back" || sMod == "color-trans")
                {
                    Color col = Color.Transparent;
                    if (sVal != "transparent")
                    {
                        int iColor = Convert.ToInt32(sVal, 16);
                        int iR = iColor / 256 / 256;
                        int iG = iColor / 256 - iR * 256;
                        int iB = iColor - iR * 256 * 256 - iG * 256;
                        col = Color.FromArgb(iR, iG, iB);
                    }
                    if (sMod == "color-main") cnt.Get(iThis).ForeColor = col;
                    if (sMod == "color-back") cnt.Get(iThis).BackColor = col;
                    if (sMod == "color-trans") cnt.GetFrm(iThis).TransparencyKey = col;
                }
                if (sMod == "border")
                {
                    FlatStyle flStyle = FlatStyle.Popup;
                    BorderStyle bStyle = BorderStyle.None;
                    if (sVal == "2d") flStyle = FlatStyle.Popup;
                    if (sVal == "2d") bStyle = BorderStyle.FixedSingle;
                    if (sVal == "3d") bStyle = BorderStyle.Fixed3D;
                    if (sVal == "3d") flStyle = FlatStyle.Standard;
                    if (sVal == "none") bStyle = BorderStyle.None;
                    FormBorderStyle fbStyle = FormBorderStyle.Sizable;
                    if (sVal == "none") fbStyle = FormBorderStyle.None;
                    if (sVal == "normal") fbStyle = FormBorderStyle.Sizable;
                    if (sVal == "fixed") fbStyle = FormBorderStyle.FixedSingle;
                    if (sVal == "slim") fbStyle = FormBorderStyle.SizableToolWindow;
                    if (sVal == "slimfixed") fbStyle = FormBorderStyle.FixedToolWindow;
                    if (iTTyp == z.Con.uFrm) cnt.GetFrm(iThis).FormBorderStyle = fbStyle;
                    if (iTTyp == z.Con.uPnl) cnt.GetPnl(iThis).BorderStyle = bStyle;
                    if (iTTyp == z.Con.uPic) cnt.GetPic(iThis).BorderStyle = bStyle;
                    if (iTTyp == z.Con.uLst) cnt.GetLst(iThis).BorderStyle = bStyle;
                    if (iTTyp == z.Con.uDrp) cnt.GetDrp(iThis).FlatStyle = flStyle;
                    if (iTTyp == z.Con.uTxt) cnt.GetTxt(iThis).BorderStyle = bStyle;
                    if (iTTyp == z.Con.uLbl) cnt.GetLbl(iThis).BorderStyle = bStyle;
                }
                if (sMod == "font-family")
                {
                    fFamily = new FontFamily(sVal);
                    cnt.Get(iThis).Font = new Font(fFamily, fSize, fStyle);
                }
                if (sMod == "font-size")
                {
                    fSize = (float)Convert.ToDouble(sVal);
                    cnt.Get(iThis).Font = new Font(fFamily, fSize, fStyle);
                }
                if (sMod == "font-style")
                {
                    sVal = ", " + sVal + ", ";
                    if (sVal.Contains(", bold, ")) fStyle |= FontStyle.Bold;
                    if (sVal.Contains(", italic, ")) fStyle |= FontStyle.Italic;
                    if (sVal.Contains(", underline, ")) fStyle |= FontStyle.Underline;
                    if (sVal.Contains(", strike, ")) fStyle |= FontStyle.Strikeout;
                    cnt.Get(iThis).Font = new Font(fFamily, fSize, fStyle);
                }
                if (sMod == "image")
                {
                    string sImgPath = sAppPath + "skin\\" + sVal.Substring(1, sVal.Length - 2);
                    if (iTTyp == z.Con.uFrm) cnt.Get(iThis).BackgroundImage = new Bitmap(sImgPath);
                    if (iTTyp == z.Con.uPic) cnt.GetPic(iThis).Image = new Bitmap(sImgPath);
                    if (iTTyp == z.Con.uPnl)
                    {
                        cnt.Inf(iThis).bFace[0] = new Bitmap(sImgPath);
                        cnt.Get(iThis).BackgroundImage =
                            cnt.Inf(iThis).bFace[0] as Image;
                    }
                    if (iTTyp == z.Con.uBtn)
                    {
                        cnt.GetBtn(iThis).FlatStyle = FlatStyle.Flat;
                        cnt.GetBtn(iThis).FlatAppearance.BorderSize = 0;
                        cnt.GetBtn(iThis).FlatAppearance.MouseOverBackColor = Color.Transparent;
                        cnt.GetBtn(iThis).FlatAppearance.MouseDownBackColor = Color.Transparent;
                        cnt.Inf(iThis).bFace[0] = new Bitmap(sImgPath);
                        cnt.Get(iThis).BackgroundImage =
                            cnt.Inf(iThis).bFace[0] as Image;
                    }
                }
                if (sMod == "image-focus")
                {
                    string sImgPath = sAppPath + "skin\\" + sVal.Substring(1, sVal.Length - 2);
                    if (iTTyp == z.Con.uPnl) cnt.Inf(iThis).bFace[1] = new Bitmap(sImgPath);
                    if (iTTyp == z.Con.uBtn) cnt.Inf(iThis).bFace[1] = new Bitmap(sImgPath);
                }
                if (sMod == "image-pressed")
                {
                    string sImgPath = sAppPath + "skin\\" + sVal.Substring(1, sVal.Length - 2);
                    if (iTTyp == z.Con.uPnl) cnt.Inf(iThis).bFace[2] = new Bitmap(sImgPath);
                    if (iTTyp == z.Con.uBtn) cnt.Inf(iThis).bFace[2] = new Bitmap(sImgPath);
                }
                if (sMod == "image-style")
                {
                    ImageLayout ilStyle = ImageLayout.Tile;
                    if (sVal == "none") ilStyle = ImageLayout.None;
                    if (sVal == "center") ilStyle = ImageLayout.Center;
                    if (sVal == "stretch") ilStyle = ImageLayout.Stretch;
                    if (sVal == "zoom") ilStyle = ImageLayout.Zoom;
                    PictureBoxSizeMode pbStyle = PictureBoxSizeMode.Normal;
                    if (sVal == "center") pbStyle = PictureBoxSizeMode.CenterImage;
                    if (sVal == "stretch") pbStyle = PictureBoxSizeMode.StretchImage;
                    if (sVal == "zoom") pbStyle = PictureBoxSizeMode.Zoom;
                    if (iTTyp != z.Con.uPic) cnt.Get(iThis).BackgroundImageLayout = ilStyle;
                    if (iTTyp == z.Con.uPic) cnt.GetPic(iThis).SizeMode = pbStyle;
                }
                if (sMod == "add-item")
                {
                    int iAt = sVal.IndexOf(":");
                    int iIdx = Convert.ToInt32(sVal.Substring(0, iAt));
                    sVal = sVal.Substring(iAt + 1);
                    if (iTTyp == z.Con.uDrp) cnt.GetDrp(iThis).Items.Add(sVal);
                    if (iTTyp == z.Con.uLst) cnt.GetLst(iThis).Items.Add(sVal);
                }
                a++;
            }
            if (iTTyp == 1 || iTTyp == 3)
            {
                if (cnt.Inf(iThis).bFace[0] == null) cnt.Inf(iThis).bFace[0] = new Bitmap(1, 1);
                if (cnt.Inf(iThis).bFace[1] == null) cnt.Inf(iThis).bFace[1] = cnt.Inf(iThis).bFace[0];
                if (cnt.Inf(iThis).bFace[2] == null) cnt.Inf(iThis).bFace[2] = cnt.Inf(iThis).bFace[1];
            }
            if (bInitial) bBusy = false;
            return a;
        }
        public void Draw()
        {
            if (bBusy) return; bBusy = true;
            string sCurSize = "DUNNO_LOL";
            Size szCurSize = conTarget.cCon().Size;
            Size szScreen = new Size(
                Screen.PrimaryScreen.WorkingArea.Width,
                Screen.PrimaryScreen.WorkingArea.Height);
            for (int a = 0; a < cnt.Cons.Count; a++)
            {
                int iType = 0; //FormBorderType
                z.ConInf ci = cnt.Inf(a);
                if (ci.cPar.sName != sCurSize)
                {
                    if (ci.uType == z.Con.uFrm)
                    {
                        szCurSize = szScreen;
                        iType = GetWasted(ci.cFrm());
                        szCurSize.Width -= WasteSize[iType].X;
                        szCurSize.Height -= WasteSize[iType].Y;
                    }
                    else
                    {
                        sCurSize = ci.cPar.sName;
                        szCurSize = ci.cCon().Parent.Size;
                        if (ci.cPar.uType == z.Con.uFrm)
                        {
                            iType = GetWasted(ci.cPar.cFrm());
                            szCurSize.Width -= WasteSize[iType].X;
                            szCurSize.Height -= WasteSize[iType].Y;
                        }
                    }
                    /*if (ci.uType != z.Con.uFrm)
                    {
                        sCurSize = ci.cPar.sName;
                        szCurSize = ci.cCon().Parent.Size;
                    }
                    else if (ci.sName != sCurSize)
                    {
                        sCurSize = ci.sName;
                        szCurSize = ci.cCon().Size;
                    }
                    if (ci.uType == z.Con.uFrm)
                    {
                        iType = GetWasted(ci.cFrm());
                        szCurSize.Width -= WasteSize[iType].X;
                        szCurSize.Height -= WasteSize[iType].Y;
                    }
                    if (ci.cPar.uType == z.Con.uFrm)
                    {
                        iType = GetWasted(ci.cPar.cFrm());
                        szCurSize.Width -= WasteSize[iType].X;
                        szCurSize.Height -= WasteSize[iType].Y;
                    }*/
                }
                if (ci.ptLoc.Length > 0 &&
                    ci.szSize.Length > 0)
                {
                    if (ci.uType == z.Con.uFrm)
                    {
                        ci.cCon().Location = ci.GetPos(szScreen);
                        if (!string.IsNullOrEmpty(ci.szSize[0]))
                        {
                            ci.cCon().Size = ci.GetSize(szScreen);
                            ci.cCon().Width += WasteSize[iType].X;
                            ci.cCon().Height += WasteSize[iType].Y;
                        }
                    }
                    else
                    {
                        ci.cCon().Location = ci.GetPos(szCurSize);
                        if (!string.IsNullOrEmpty(ci.szSize[0]))
                            ci.cCon().Size = ci.GetSize(szCurSize);
                    }
                }
            }
            bBusy = false;
            /*if (bPnl)
            {
                Point ptSizePar = (Point)conTarget.cCon.Parent.Size;
                conTarget.Location = conTarget.GetPos(ptSizePar);
                conTarget.Size = conTarget.GetSize(ptSizePar);
            }
            for (int a = 0; a < ConDyn.Length; a++)
                for (int b = 0; b < ConDyn[a].Length; b++)
                {
                    if (a == 0)
                    {
                        aFrm[b].Location = ConDyn[a][b].GetPos(ptSize);
                        aFrm[b].Size = ConDyn[a][b].GetSize(ptSize);
                    }
                    if (a == 1)
                    {
                        aPnl[b].Location = ConDyn[a][b].GetPos(ptSize);
                        aPnl[b].Size = ConDyn[a][b].GetSize(ptSize);
                    }
                    if (a == 2)
                    {
                        aPic[b].Location = ConDyn[a][b].GetPos(ptSize);
                        aPic[b].Size = ConDyn[a][b].GetSize(ptSize);
                    }
                    if (a == 3)
                    {
                        aBtn[b].Location = ConDyn[a][b].GetPos(ptSize);
                        aBtn[b].Size = ConDyn[a][b].GetSize(ptSize);
                    }
                    if (a == 4)
                    {
                        aLst[b].Location = ConDyn[a][b].GetPos(ptSize);
                        aLst[b].Size = ConDyn[a][b].GetSize(ptSize);
                    }
                    if (a == 5)
                    {
                        aTxt[b].Location = ConDyn[a][b].GetPos(ptSize);
                        aTxt[b].Size = ConDyn[a][b].GetSize(ptSize);
                    }
                    if (a == 6)
                    {
                        aLbl[b].Location = ConDyn[a][b].GetPos(ptSize);
                        aLbl[b].Size = ConDyn[a][b].GetSize(ptSize);
                    }
                }*/
        }

        void Skin_FormClosed(object sender, FormClosedEventArgs e)
        {
            Application.Exit();
        }
        void Skin_Resize(object sender, EventArgs e)
        {
            tRedraw.Stop();
            tRedraw.Start();
        }
        void tRedraw_Tick(object sender, EventArgs e)
        {
            if (bBusy) return;
            tRedraw.Stop();
            Draw();
        }
        void c_DragForm(object sender, MouseEventArgs e)
        {
            int iThis = Convert.ToInt32(((Control)sender).Tag.ToString());
            ReleaseCapture();
            IntPtr handle = cnt.Inf(iThis).cPar.cCon().Handle;
            SendMessage(handle, WM_NCLBUTTONDOWN, HT_CAPTION, 0);
        }
        void c_ResizeForm_Down(object sender, MouseEventArgs e)
        {
            if (e.Button != MouseButtons.Left) return;
            int iThis = Convert.ToInt32(((Control)sender).Tag.ToString());
            ptResize = Cursor.Position;
            szResize = cnt.Inf(iThis).cPar.cCon().Size;
        }
        void c_ResizeForm_Move(object sender, MouseEventArgs e)
        {
            if (e.Button != MouseButtons.Left) return;
            int iThis = Convert.ToInt32(((Control)sender).Tag.ToString());
            Size sz = szResize;
            sz.Width += Cursor.Position.X - ptResize.X;
            sz.Height += Cursor.Position.Y - ptResize.Y;
            cnt.Inf(iThis).cPar.cCon().Size = sz;
        }
        /*public int Insit(int iStartAt)
        {
            int iTTyp = -1; int iThis = -1;
            if (iStartAt != -1) bPnl = true;
            else
            {
                iStartAt = 0;
                while (!sSkin[iStartAt].StartsWith("#" + sTarget + " ")) iStartAt++;
                iStartAt++;
            }
            int iDepth = 0;
            string[] sThisLoc = new string[] { "0", "0" };
            string[] sThisSize = new string[] { "1", "1" };
            for (int a = iStartAt; a < sSkin.Length; a++)
            {
                if (sSkin[a].StartsWith("!window "))
                {
                    //iTTyp = 1; aPnl.NewPanel(); iThis = aPnl.Count - 1;
                    //string sName = sSkin[a].Substring("panel ".Length).Trim(' ', '{');
                    //if (sName == "") sName = "gui_elm"; aConName[iTTyp].Add(sName); a++;
                    //Skin skPanel = new Skin(aPnl[iThis], "", sSkinF);
                    //a = skPanel.Enable(a); iThis = -1; iTTyp = -2;
                    iTTyp = z.Con.uFrm; iThis = z.Con.Cons.Count; iDepth++;
                    string sName = sSkin[a].Substring("!window ".Length).Trim(' ', '{');
                    z.Con.Add(iTTyp, sName, conTarget); a++;
                    //Skin skPanel = new Skin(z.Con.Inf(iThis), "", sSkinF);
                    //skPanel.Parse(); a = skPanel.Init(a); iThis = -1; iTTyp = -2;
                }
                if (sSkin[a].StartsWith("!panel "))
                {
                    iTTyp = z.Con.uPnl; iThis = z.Con.Cons.Count; iDepth++;
                    string sName = sSkin[a].Substring("!panel ".Length).Trim(' ', '{');
                    z.Con.Add(iTTyp, sName, conTarget); a++;
                    //Skin skPanel = new Skin(z.Con.Inf(iThis), "", sSkinF);
                    //skPanel.Parse(); a = skPanel.Init(a); //iThis = -1; //iTTyp = -2;
                }
                if (sSkin[a].StartsWith("!image "))
                {
                    iTTyp = z.Con.uPic; iThis = z.Con.Cons.Count; iDepth++;
                    string sName = sSkin[a].Substring("!image ".Length).Trim(' ', '{');
                    z.Con.Add(iTTyp, sName, conTarget); a++;
                    //Skin skPanel = new Skin(z.Con.Inf(iThis), "", sSkinF);
                    //skPanel.Parse(); a = skPanel.Init(a); //iThis = -1; //iTTyp = -2;
                }
                if (sSkin[a].StartsWith("!button "))
                {
                    iTTyp = z.Con.uBtn; iThis = z.Con.Cons.Count; iDepth++;
                    string sName = sSkin[a].Substring("!button ".Length).Trim(' ', '{');
                    z.Con.Add(iTTyp, sName, conTarget); a++;
                    //Skin skPanel = new Skin(z.Con.Inf(iThis), "", sSkinF);
                    //skPanel.Parse(); a = skPanel.Init(a); //iThis = -1; //iTTyp = -2;
                }
                if (sSkin[a].StartsWith("!list "))
                {
                    iTTyp = z.Con.uLst; iThis = z.Con.Cons.Count; iDepth++;
                    string sName = sSkin[a].Substring("!list ".Length).Trim(' ', '{');
                    z.Con.Add(iTTyp, sName, conTarget); a++;
                    //Skin skPanel = new Skin(z.Con.Inf(iThis), "", sSkinF);
                    //skPanel.Parse(); a = skPanel.Init(a); //iThis = -1; //iTTyp = -2;
                }
                if (sSkin[a].StartsWith("!textbox "))
                {
                    iTTyp = z.Con.uTxt; iThis = z.Con.Cons.Count; iDepth++;
                    string sName = sSkin[a].Substring("!textbox ".Length).Trim(' ', '{');
                    z.Con.Add(iTTyp, sName, conTarget); a++;
                    //Skin skPanel = new Skin(z.Con.Inf(iThis), "", sSkinF);
                    //skPanel.Parse(); a = skPanel.Init(a); //iThis = -1; //iTTyp = -2;
                }
                if (sSkin[a].StartsWith("!label "))
                {
                    iTTyp = z.Con.uLbl; iThis = z.Con.Cons.Count; iDepth++;
                    string sName = sSkin[a].Substring("!label ".Length).Trim(' ', '{');
                    z.Con.Add(iTTyp, sName, conTarget); a++;
                    //Skin skPanel = new Skin(z.Con.Inf(iThis), "", sSkinF);
                    //skPanel.Parse(); a = skPanel.Init(a); //iThis = -1; //iTTyp = -2;
                }

                while (true)
                {
                    if (a >= sSkin.Length) break;
                    if (sSkin[a] == "}")
                    {
                        conTarget.ptLoc = sThisLoc;
                        conTarget.szSize = sThisSize;
                        return a;
                    }
                    if (sSkin[a] == "") { a++; continue; }
                    if (sSkin[a].StartsWith("//")) { a++; continue; }
                    if (sSkin[a].StartsWith("!")) { a--; break; }
                    string sMod = sSkin[a].Substring(0, sSkin[a].IndexOf(":"));
                    string sVal = sSkin[a].Substring(sMod.Length + 1).Trim(' ', '\t');
                    if (sVal.EndsWith(";")) sVal = sVal.Substring(0, sVal.Length - 1);

                    if (sMod == "location") sThisLoc = sVal.Main.Split(' ');
                    if (sMod == "size") sThisSize = sVal.Main.Split(' ');
                    if (sMod == "loc.x") sThisLoc[0] = sVal.Replace(" ", "");
                    if (sMod == "loc.y") sThisLoc[1] = sVal.Replace(" ", "");
                    if (sMod == "size.x") sThisSize[0] = sVal.Replace(" ", "");
                    if (sMod == "size.y") sThisSize[1] = sVal.Replace(" ", "");
                    if (sMod == "text")
                    {
                        if (iTTyp == -1) conTarget.cCon().Text = sVal.Substring(1, sVal.Length - 2);
                        else z.Con.Get(iThis).Text = sVal.Substring(1, sVal.Length - 2);
                    }
                    if (sMod == "color-main" || sMod == "color-back")
                    {
                        int iColor = Convert.ToInt32(sVal, 16);
                        int iR = iColor / 256 / 256;
                        int iG = iColor / 256 - iR * 256;
                        int iB = iColor - iR * 256 * 256 - iG * 256;
                        if (sMod == "color-main")
                        {
                            if (iTTyp == -1) conTarget.cCon().ForeColor = Color.FromArgb(iR, iG, iB);
                            else z.Con.Get(iThis).ForeColor = Color.FromArgb(iR, iG, iB);
                        }
                        if (sMod == "color-back")
                        {
                            if (iTTyp == -1) conTarget.cCon().BackColor = Color.FromArgb(iR, iG, iB);
                            else z.Con.Get(iThis).BackColor = Color.FromArgb(iR, iG, iB);
                        }
                    }
                    if (sMod == "border")
                    {
                        BorderStyle bStyle = BorderStyle.None;
                        if (sVal == "3d") bStyle = BorderStyle.Fixed3D;
                        if (sVal == "2d") bStyle = BorderStyle.FixedSingle;
                        FormBorderStyle fbStyle = FormBorderStyle.Sizable;
                        if (sVal == "none") fbStyle = FormBorderStyle.None;
                        if (sVal == "fixed") fbStyle = FormBorderStyle.FixedSingle;
                        if (sVal == "slim") fbStyle = FormBorderStyle.SizableToolWindow;
                        if (sVal == "slimfixed") fbStyle = FormBorderStyle.FixedToolWindow;
                        if (iTTyp == -1) if (!bPnl) (conTarget.cFrm()).FormBorderStyle = fbStyle;
                        if (iTTyp == -1) if (bPnl) (conTarget.cPnl()).BorderStyle = bStyle;
                        if (iTTyp == z.Con.uFrm) z.Con.GetFrm(iThis).FormBorderStyle = fbStyle;
                        if (iTTyp == z.Con.uPnl) z.Con.GetPnl(iThis).BorderStyle = bStyle;
                        if (iTTyp == z.Con.uPic) z.Con.GetPic(iThis).BorderStyle = bStyle;
                        if (iTTyp == z.Con.uTxt) z.Con.GetTxt(iThis).BorderStyle = bStyle;
                        if (iTTyp == z.Con.uLbl) z.Con.GetLbl(iThis).BorderStyle = bStyle;
                    }
                    if (sMod == "image")
                    {
                        string sImgPath = sAppPath + "skin\\" + sVal.Substring(1, sVal.Length - 2);
                        if (iTTyp == -1) conTarget.cCon().BackgroundImage = new Bitmap(sImgPath);
                        if (iTTyp == z.Con.uFrm) z.Con.Get(iThis).BackgroundImage = new Bitmap(sImgPath);
                        if (iTTyp == z.Con.uPic) z.Con.Get(iThis).BackgroundImage = new Bitmap(sImgPath);
                        if (iTTyp == z.Con.uPnl)
                        {
                            z.Con.Inf(iThis).bFace[0] = new Bitmap(sImgPath);
                            z.Con.Get(iThis).BackgroundImage =
                                z.Con.Inf(iThis).bFace[0] as Image;
                        }
                        if (iTTyp == z.Con.uBtn)
                        {
                            z.Con.GetBtn(iThis).FlatStyle = FlatStyle.Flat;
                            z.Con.GetBtn(iThis).FlatAppearance.BorderSize = 0;
                            z.Con.GetBtn(iThis).FlatAppearance.MouseOverBackColor = Color.Transparent;
                            z.Con.GetBtn(iThis).FlatAppearance.MouseDownBackColor = Color.Transparent;
                            z.Con.Inf(iThis).bFace[0] = new Bitmap(sImgPath);
                            z.Con.Get(iThis).BackgroundImage =
                                z.Con.Inf(iThis).bFace[0] as Image;
                        }
                    }
                    if (sMod == "image-focus")
                    {
                        string sImgPath = sAppPath + "skin\\" + sVal.Substring(1, sVal.Length - 2);
                        if (iTTyp == z.Con.uPnl) z.Con.Inf(iThis).bFace[1] = new Bitmap(sImgPath);
                        if (iTTyp == z.Con.uBtn) z.Con.Inf(iThis).bFace[1] = new Bitmap(sImgPath);
                    }
                    if (sMod == "image-pressed")
                    {
                        string sImgPath = sAppPath + "skin\\" + sVal.Substring(1, sVal.Length - 2);
                        if (iTTyp == z.Con.uPnl) z.Con.Inf(iThis).bFace[2] = new Bitmap(sImgPath);
                        if (iTTyp == z.Con.uBtn) z.Con.Inf(iThis).bFace[2] = new Bitmap(sImgPath);
                    }
                    if (sMod == "image-style")
                    {
                        ImageLayout ilStyle = ImageLayout.Tile;
                        if (sVal == "none") ilStyle = ImageLayout.None;
                        if (sVal == "center") ilStyle = ImageLayout.Center;
                        if (sVal == "stretch") ilStyle = ImageLayout.Stretch;
                        if (sVal == "zoom") ilStyle = ImageLayout.Zoom;
                        if (iTTyp == -1) conTarget.cCon().BackgroundImageLayout = ilStyle;
                        else z.Con.Get(iThis).BackgroundImageLayout = ilStyle;
                    }
                    a++;
                }
                if (iTTyp == 1 || iTTyp == 3)
                {
                    if (z.Con.Inf(iThis).bFace[0] == null) z.Con.Inf(iThis).bFace[0] = new Bitmap(1, 1);
                    if (z.Con.Inf(iThis).bFace[1] == null) z.Con.Inf(iThis).bFace[1] = z.Con.Inf(iThis).bFace[0];
                    if (z.Con.Inf(iThis).bFace[2] == null) z.Con.Inf(iThis).bFace[2] = z.Con.Inf(iThis).bFace[1];
                }
                iTTyp = -2;
            }
            return 0;
        }    */
    }
    public class ClickInfo
    {
        public bool bPoll = false;
        public int iIndex = -1;
        public int iKey = 0;
        public int iCount = 0;
        public Point ptLoc = new Point(-1, -1);
        public Point ptRLoc = new Point(-1, -1);
        public long lLast = 0;

        public static long Tick()
        {
            return System.DateTime.Now.Ticks / 10000;
        }
    }
    public class TxChgInfo
    {
        public bool bPoll = false;
        public int iIndex = -1;

        public static long Tick()
        {
            return System.DateTime.Now.Ticks / 10000;
        }
    }
    public class KeyUpInfo
    {
        public bool bPoll = false;
        public int iIndex = -1;
        public Keys kCode = Keys.None;

        public static long Tick()
        {
            return System.DateTime.Now.Ticks / 10000;
        }
    }

    public class ConInf
    {
        public int uType = -1;
        public string sName = "gui_elm";
        public Bitmap[] bFace = new Bitmap[3];
        public bool bKeybind = false;
        public object oCon = null;
        public ConInf cPar = null;
        #region Aids
        public Control cCon()
        {
            return (Control)oCon;
        }
        public Form cFrm()
        {
            return (Form)oCon;
        }
        public Panel cPnl()
        {
            return (Panel)oCon;
        }
        public PictureBox cPic()
        {
            return (PictureBox)oCon;
        }
        public Button cBtn()
        {
            return (Button)oCon;
        }
        public ListBox cLst()
        {
            return (ListBox)oCon;
        }
        public ComboBox cDrp()
        {
            return (ComboBox)oCon;
        }
        public TextBox cTxt()
        {
            return (TextBox)oCon;
        }
        public CheckBox cChk()
        {
            return (CheckBox)oCon;
        }
        public Label cLbl()
        {
            return (Label)oCon;
        }
        #endregion

        private Lundin.ExpressionParser m4th =
            new Lundin.ExpressionParser();
        public string[] szSize = new string[2];
        public string[] ptLoc = new string[2];
        public Point GetPos(Size ptSize)
        {
            Point ptRet = new Point(0, 0);
            //ptSize.Width -= Skin.wasteX; ptSize.Height -= Skin.wasteY;
            ptRet.X = m4th.Parse(ptLoc[0].Replace("all", "" + ptSize.Width));
            ptRet.Y = m4th.Parse(ptLoc[1].Replace("all", "" + ptSize.Height));
            return ptRet;
        }
        public Size GetSize(Size ptSize)
        {
            Point ptRet = new Point(0, 0);
            //ptSize.Width -= Skin.wasteX; ptSize.Height -= Skin.wasteY;
            ptRet.X = m4th.Parse(szSize[0].Replace("all", "" + ptSize.Width));
            ptRet.Y = m4th.Parse(szSize[1].Replace("all", "" + ptSize.Height));
            return (Size)ptRet;
        }
    }
    public class Con
    {
        public System.Collections.ArrayList Cons =
            new System.Collections.ArrayList();
        public System.Collections.ArrayList HKey =
            new System.Collections.ArrayList();

        public static int uFrm = 1;
        public static int uPnl = 2;
        public static int uPic = 3;
        public static int uBtn = 4;
        public static int uLst = 5;
        public static int uDrp = 6;
        public static int uTxt = 7;
        public static int uChk = 8;
        public static int uLbl = 9;

        public ConInf Inf(int i)
        {
            return (ConInf)Cons[i];
        }
        public Control Get(int i)
        {
            return Inf(i).cCon();
        }
        public int Find(string sName)
        {
            for (int a = 0; a < Cons.Count; a++)
                if (((ConInf)Cons[a]).sName == sName)
                    return a;
            return -1;
        }
        public ConInf Inf(string sName)
        {
            return Inf(Find(sName));
        }
        public Control Get(string sName)
        {
            return Get(Find(sName));
        }
        public ConInf GetRoot(ConInf ciCon)
        {
            while (ciCon.uType != Con.uFrm)
                ciCon = ciCon.cPar;
            return ciCon;
        }
        public void Add(int uType, string sName, ConInf ciPar)
        {
            ConInf cdCon = new ConInf();
            cdCon.cPar = new ConInf();
            if (sName == "") sName = "gui_elm";
            cdCon.uType = uType;
            cdCon.sName = sName;
            if (uType == uFrm) cdCon.oCon = new Form();
            if (uType == uPnl) cdCon.oCon = new Panel();
            if (uType == uPic) cdCon.oCon = new PictureBox();
            if (uType == uBtn) cdCon.oCon = new Button();
            if (uType == uLst) cdCon.oCon = new ListBox();
            if (uType == uDrp) cdCon.oCon = new ComboBox();
            if (uType == uTxt) cdCon.oCon = new TextBox();
            if (uType == uChk) cdCon.oCon = new CheckBox();
            if (uType == uLbl) cdCon.oCon = new Label();
            cdCon.cCon().Tag = Cons.Count;
            cdCon.cCon().Name = sName;
            if (ciPar.oCon != null)
            {
                ciPar.cCon().Controls.Add(cdCon.cCon());
                cdCon.cCon().Font = ciPar.cCon().Font;
                cdCon.cCon().BackColor = ciPar.cCon().BackColor;
                cdCon.cCon().ForeColor = ciPar.cCon().ForeColor;
            }
            cdCon.cPar = ciPar;
            Cons.Add(cdCon);
            if (uType == uPnl || uType == uBtn)
            {
                cdCon.cCon().MouseEnter += new EventHandler(c_MouseEnter);
                cdCon.cCon().MouseLeave += new EventHandler(c_MouseLeave);
                cdCon.cCon().MouseDown += new MouseEventHandler(c_MouseDown);
                cdCon.cCon().MouseUp += new MouseEventHandler(c_MouseUp);
            }
            if (uType == uTxt) (cdCon.cTxt()).BorderStyle = BorderStyle.FixedSingle;
            if (uType == uLst) (cdCon.cLst()).BorderStyle = BorderStyle.FixedSingle;
            if (uType == uDrp) (cdCon.cDrp()).FlatStyle = FlatStyle.Popup;
        }
        #region Aids
        public Form GetFrm(int i)
        {
            return Inf(i).cFrm();
        }
        public Panel GetPnl(int i)
        {
            return Inf(i).cPnl();
        }
        public PictureBox GetPic(int i)
        {
            return Inf(i).cPic();
        }
        public Button GetBtn(int i)
        {
            return Inf(i).cBtn();
        }
        public ListBox GetLst(int i)
        {
            return Inf(i).cLst();
        }
        public ComboBox GetDrp(int i)
        {
            return Inf(i).cDrp();
        }
        public TextBox GetTxt(int i)
        {
            return Inf(i).cTxt();
        }
        public CheckBox GetChk(int i)
        {
            return Inf(i).cChk();
        }
        public Label GetLbl(int i)
        {
            return Inf(i).cLbl();
        }

        //WHY YOU READING THIS

        public Form GetFrm(string sName)
        {
            return Inf(Find(sName)).cFrm();
        }
        public Panel GetPnl(string sName)
        {
            return Inf(Find(sName)).cPnl();
        }
        public PictureBox GetPic(string sName)
        {
            return Inf(Find(sName)).cPic();
        }
        public Button GetBtn(string sName)
        {
            return Inf(Find(sName)).cBtn();
        }
        public ListBox GetLst(string sName)
        {
            return Inf(Find(sName)).cLst();
        }
        public ComboBox GetDrp(string sName)
        {
            return Inf(Find(sName)).cDrp();
        }
        public TextBox GetTxt(string sName)
        {
            return Inf(Find(sName)).cTxt();
        }
        public CheckBox GetChk(string sName)
        {
            return Inf(Find(sName)).cChk();
        }
        public Label GetLbl(string sName)
        {
            return Inf(Find(sName)).cLbl();
        }
        #endregion

        //Applies events to controls
        public void Event(int i, string OnEvent, string Action)
        {
            if (OnEvent == "click")
            {
                //Internal Handlers
                if (Action == "AppExit") Get(i).MouseClick += new MouseEventHandler(ih_AppExit);
                if (Action == "Minimize") Get(i).MouseClick += new MouseEventHandler(ih_wsMinimize);
                if (Action == "Restore") Get(i).MouseClick += new MouseEventHandler(ih_wsRestore);
                if (Action == "Maximze") Get(i).MouseClick += new MouseEventHandler(ih_wsMaximize);
                if (Action == "ResTgl") Get(i).MouseClick += new MouseEventHandler(ih_wsResTgl);

                //Custom Handlers
                if (Action == "Scr_cmdScript_Click") Get(i).MouseClick += new MouseEventHandler(pSGrab.GUI.Scr_cmdScript_Click);
                if (Action == "Scr_cmdPath_Click") Get(i).MouseClick += new MouseEventHandler(pSGrab.GUI.Scr_cmdPath_Click);
                if (Action == "ParQue_cmdSave_Click") Get(i).MouseClick += new MouseEventHandler(pSGrab.GUI.ParQue_cmdSave_Click);
                if (Action == "ParQue_cmdLoad_Click") Get(i).MouseClick += new MouseEventHandler(pSGrab.GUI.ParQue_cmdLoad_Click);
                if (Action == "Scr_cmdAdd_Click") Get(i).MouseClick += new MouseEventHandler(pSGrab.GUI.Scr_cmdAdd_Click);
                if (Action == "Scr_cmdStart_Click") Get(i).MouseClick += new MouseEventHandler(pSGrab.GUI.Scr_cmdStart_Click);
                if (Action == "JobQue_cmdSave_Click") Get(i).MouseClick += new MouseEventHandler(pSGrab.GUI.JobQue_cmdSave_Click);
                if (Action == "JobQue_cmdLoad_Click") Get(i).MouseClick += new MouseEventHandler(pSGrab.GUI.JobQue_cmdLoad_Click);
                if (Action == "JobQue_cmdExecAll_Click") Get(i).MouseClick += new MouseEventHandler(pSGrab.GUI.JobQue_cmdExecAll_Click);
                if (Action == "JobQue_cmdExecSel_Click") Get(i).MouseClick += new MouseEventHandler(pSGrab.GUI.JobQue_cmdExecSel_Click);
                if (Action == "ddl_cmdSave_Click") Get(i).MouseClick += new MouseEventHandler(pSGrab.GUI.ddl_cmdSave_Click);
                if (Action == "ddl_cmdLoad_Click") Get(i).MouseClick += new MouseEventHandler(pSGrab.GUI.ddl_cmdLoad_Click);
            }
            if (OnEvent == "select")
            {
                if (Action == "JobQue_lstQue_Select") GetLst(i).SelectedIndexChanged += new EventHandler(pSGrab.GUI.JobQue_lstQue_Select);
            }
            if (OnEvent == "keypress")
            {
                string Key = Action.Substring(0, Action.IndexOf(":"));
                HKeyInf hki = new HKeyInf();
                hki.act = Action.Substring(Action.IndexOf(":") + 1);
                hki.key = (Keys)new KeysConverter().ConvertFromInvariantString(Key);
                if (!Inf(i).bKeybind) Get(i).KeyUp += new KeyEventHandler(ConHotkeys);
                Inf(i).bKeybind = true;
                HKey.Add(hki);
            }
        }
        public void ConHotkeys(object sender, KeyEventArgs e)
        {
            int iThis = Convert.ToInt32(((Control)sender).Tag.ToString());
            for (int a = 0; a < HKey.Count; a++)
            {
                if (e.KeyCode == ((z.HKeyInf)HKey[a]).key)
                {
                    string act = ((z.HKeyInf)HKey[a]).act;
                    if (act == "JobQue_lstQue_RemSel") pSGrab.GUI.JobQue_lstQue_RemSel();
                    if (act == "JobQue_lstQue_UpdSel") pSGrab.GUI.JobQue_lstQue_UpdSel();
                    if (act == "Scr_Param_Confirm") pSGrab.GUI.Scr_Param_Confirm();
                }
            }
        }

        //Internal Handlers
        void ih_AppExit(object sender, MouseEventArgs e)
        {
            Application.Exit();
        }
        void ih_wsMinimize(object sender, MouseEventArgs e)
        {
            int iThis = Convert.ToInt32(((Control)sender).Tag.ToString());
            ConInf ciCon = GetRoot(Inf(iThis));
            ciCon.cFrm().WindowState =
                FormWindowState.Minimized;
        }
        void ih_wsRestore(object sender, MouseEventArgs e)
        {
            int iThis = Convert.ToInt32(((Control)sender).Tag.ToString());
            ConInf ciCon = GetRoot(Inf(iThis));
            ciCon.cFrm().WindowState =
                FormWindowState.Normal;
        }
        void ih_wsMaximize(object sender, MouseEventArgs e)
        {
            int iThis = Convert.ToInt32(((Control)sender).Tag.ToString());
            ConInf ciCon = GetRoot(Inf(iThis));
            ciCon.cFrm().WindowState =
                FormWindowState.Maximized;
        }
        void ih_wsResTgl(object sender, MouseEventArgs e)
        {
            int iThis = Convert.ToInt32(((Control)sender).Tag.ToString());
            ConInf ciCon = GetRoot(Inf(iThis));
            FormWindowState fwState = ciCon.cFrm().WindowState;
            if (fwState == FormWindowState.Minimized)
                ciCon.cFrm().WindowState = FormWindowState.Normal;
            if (fwState == FormWindowState.Maximized)
                ciCon.cFrm().WindowState = FormWindowState.Normal;
            if (fwState == FormWindowState.Normal)
                ciCon.cFrm().WindowState = FormWindowState.Maximized;
        }

        void c_MouseEnter(object sender, EventArgs e)
        {
            //((Control)sender).Text = "MouseOver";
            int iThis = Convert.ToInt32(((Control)sender).Tag.ToString());
            if (!Get(iThis).Enabled) return;
            Bitmap b = ((ConInf)Cons[iThis]).bFace[1];
            if (b != null)
                ((Control)sender).BackgroundImage = b;
        }
        void c_MouseLeave(object sender, EventArgs e)
        {
            //((Control)sender).Text = "MouseOver";
            int iThis = Convert.ToInt32(((Control)sender).Tag.ToString());
            Bitmap b = ((ConInf)Cons[iThis]).bFace[0];
            if (b != null)
                ((Control)sender).BackgroundImage = b;
        }
        void c_MouseDown(object sender, MouseEventArgs e)
        {
            //((Control)sender).Text = "MouseOver";
            int iThis = Convert.ToInt32(((Control)sender).Tag.ToString());
            if (!Get(iThis).Enabled) return;
            Bitmap b = ((ConInf)Cons[iThis]).bFace[2];
            if (b != null)
                ((Control)sender).BackgroundImage = b;
        }
        void c_MouseUp(object sender, MouseEventArgs e)
        {
            //((Control)sender).Text = "MouseOver";
            int iThis = Convert.ToInt32(((Control)sender).Tag.ToString());
            Bitmap b = ((ConInf)Cons[iThis]).bFace[1];
            if (b != null)
                ((Control)sender).BackgroundImage = b;
        }
    }
    public class HKeyInf
    {
        public Keys key;
        public string act;
    }
}