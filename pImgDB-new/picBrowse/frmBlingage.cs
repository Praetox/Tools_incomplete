using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;
using System.Drawing.Imaging;
using System.Runtime.InteropServices;

namespace picBrowse {
    public partial class frmBlingage : Form {
        public frmBlingage() {
            InitializeComponent();
        }

        private const Int32 LWA_COLORKEY = 0x1;
        private const Int32 LWA_ALPHA = 0x2;
        private const Int32 WS_EX_LAYERED = 0x00080000;

        public struct BlendF {
            public byte BlendOp;
            public byte BlendFlags;
            public byte SourceConstantAlpha;
            public byte AlphaFormat;
        }

        public const byte AC_SRC_OVER = 0x0;
        public const byte AC_SRC_ALPHA = 0x1;
        public const int WM_NCLBUTTONDOWN = 0xA1;
        public const int HT_CAPTION = 0x2;

        [DllImport("user32.dll")]
        public static extern bool UpdateLayeredWindow(IntPtr hwnd, IntPtr hdcDst, ref Point pptDst, ref Size psize, IntPtr hdcSrc, ref Point pprSrc, Int32 crKey, ref BlendF pblend, Int32 dwFlags);
        [DllImport("user32.dll")]
        public static extern IntPtr GetDC(IntPtr hWnd);
        [DllImport("gdi32.dll")]
        public static extern IntPtr CreateCompatibleDC(IntPtr hDC);
        [DllImport("user32.dll")]
        public static extern int ReleaseDC(IntPtr hWnd, IntPtr hDC);
        [DllImport("gdi32.dll")]
        public static extern bool DeleteDC(IntPtr hdc);
        [DllImport("gdi32.dll")]
        public static extern IntPtr SelectObject(IntPtr hDC, IntPtr hObject);
        [DllImport("gdi32.dll")]
        public static extern bool DeleteObject(IntPtr hObject);

        [DllImport("user32.dll")]
        static extern bool SetWindowPos(IntPtr hWnd, IntPtr hWndInsertAfter, int X,
           int Y, int cx, int cy, uint uFlags);
        static readonly IntPtr HWND_TOPMOST = new IntPtr(-1);
        static readonly IntPtr HWND_NOTOPMOST = new IntPtr(-2);
        static readonly IntPtr HWND_TOP = new IntPtr(0);
        static readonly IntPtr HWND_BOTTOM = new IntPtr(1);
        // From winuser.h
        const UInt32 SWP_NOSIZE = 0x0001;
        const UInt32 SWP_NOMOVE = 0x0002;
        const UInt32 SWP_NOZORDER = 0x0004;
        const UInt32 SWP_NOREDRAW = 0x0008;
        const UInt32 SWP_NOACTIVATE = 0x0010;
        const UInt32 SWP_FRAMECHANGED = 0x0020;  /* The frame changed: send WM_NCCALCSIZE */
        const UInt32 SWP_SHOWWINDOW = 0x0040;
        const UInt32 SWP_HIDEWINDOW = 0x0080;
        const UInt32 SWP_NOCOPYBITS = 0x0100;
        const UInt32 SWP_NOOWNERZORDER = 0x0200;  /* Don't do owner Z ordering */
        const UInt32 SWP_NOSENDCHANGING = 0x0400;  /* Don't send WM_WINDOWPOSCHANGING */
        const UInt32 TOPMOST_FLAGS = SWP_NOMOVE | SWP_NOSIZE;

        int rot = -6;
        float opac1 = 1;
        float opac2 = 1;
        Bitmap b1, b2, b3, bo;
        Point ptFrmOfs = Point.Empty;
        Point ptPicOfs = Point.Empty;
        string[] saList = new string[0];
        bool bEditMode = false;
        int iPic = 0;

        private void cfgLoad() {
            if (System.IO.File.Exists("skin\\blingage\\conf.ini")) {
                string vars = System.IO.File.ReadAllText("skin\\blingage\\conf.ini");
                string[] tmpa = cb.Split(cb.Split(vars, "frmLoc ")[1], "@")[0].Split('x');
                this.Location = new Point(Convert.ToInt32(tmpa[0]), Convert.ToInt32(tmpa[1]));
                string[] tmpb = cb.Split(cb.Split(vars, "picLoc ")[1], "@")[0].Split('x');
                ptPicOfs = new Point(Convert.ToInt32(tmpb[0]), Convert.ToInt32(tmpb[1]));
                rot = Convert.ToInt32(cb.Split(cb.Split(vars, "picRot ")[1], "@")[0]);
                opac1 = (float)Convert.ToDouble(cb.Split(cb.Split(vars, "opacA ")[1], "@")[0]);
                opac2 = (float)Convert.ToDouble(cb.Split(cb.Split(vars, "opacB ")[1], "@")[0]);
                iPic = Convert.ToInt32(cb.Split(cb.Split(vars, "n ")[1], "@")[0]);
            }
            if (System.IO.File.Exists("skin\\blingage\\list.txt")) {
                saList = System.IO.File.ReadAllText
                    ("skin\\blingage\\list.txt")
                    .Replace("\r", "").Trim('\n')
                    .Split('\n');
            }
            else {
                MessageBox.Show("No images have been picked for use with blingage." + "\r\n" +
                    "In other words, you are doing it wrong." + "\r\n" +
                    "You should be ashamed of yourself.", "HELLO :D",
                    MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
                this.Close();
            }
        }
        private void cfgSave() {
            System.IO.File.WriteAllText("skin\\blingage\\conf.ini",
                "frmLoc " + this.Left + "x" + this.Top + "@\r\n" +
                "picLoc " + ptPicOfs.X + "x" + ptPicOfs.Y + "@\r\n" +
                "picRot " + rot + "@\r\n" + "opacA " + opac1 + "@\r\n" +
                "opacB " + opac2 + "@\r\n" + "n " + iPic + "@\r\n");
        }
        private void frmBlingage_Load(object sender, EventArgs e) {
            cfgLoad();
            Render();
        }
        private void frmBlingage_DoubleClick(object sender, EventArgs e) {
            Render();
        }
        private void Render() {
            if (b2 != null) b2.Dispose();
            if (iPic >= saList.Length) iPic = 0;
            if (iPic < 0) iPic = saList.Length - 1;
            b1 = new Bitmap("skin\\blingage\\main.png");
            b2 = new Bitmap("skin\\blingage\\mask.png");
            b3 = new Bitmap(saList[iPic]);
            Point sz1 = (Point)b1.Size;
            Point sz3 = (Point)b3.Size;
            bo = new Bitmap(sz1.X, sz1.Y);
            using (Graphics g = Graphics.FromImage(bo)) {
                //string sFName = saList[iPic].Substring(saList[iPic].LastIndexOf("\\") + 1);
                g.CompositingMode = System.Drawing.Drawing2D.CompositingMode.SourceOver;
                g.CompositingQuality = System.Drawing.Drawing2D.CompositingQuality.HighQuality;
                g.InterpolationMode = System.Drawing.Drawing2D.InterpolationMode.HighQualityBicubic;
                g.PixelOffsetMode = System.Drawing.Drawing2D.PixelOffsetMode.HighQuality;
                g.SmoothingMode = System.Drawing.Drawing2D.SmoothingMode.HighQuality;
                g.TextRenderingHint = System.Drawing.Text.TextRenderingHint.AntiAlias;
                ColorMatrix cm = new ColorMatrix(new float[][]{
                    new float[]{1,0,0,0,0},
                    new float[]{0,1,0,0,0},
                    new float[]{0,0,1,0,0},
                    new float[]{0,0,0,1,0},
                    new float[]{0,0,0,0,1}});

                g.TranslateTransform(
                    (float)(sz1.X / 2),
                    (float)(sz1.Y / 2));
                g.RotateTransform(rot);
                g.TranslateTransform(
                    -(float)(sz1.X / 2),
                    -(float)(sz1.Y / 2));
                using (ImageAttributes attr = new ImageAttributes()) {
                    cm[3, 3] = opac2;
                    attr.SetColorMatrix(cm);
                    int ofsX = (int)Math.Round((sz1.X - 258) / 2.0) + ptPicOfs.X;
                    int ofsY = (int)Math.Round((sz1.Y - 160) / 2.0) + ptPicOfs.Y;
                    g.DrawImage(b3, new Rectangle(ofsX, ofsY, 258, 160),
                        0, 0, sz3.X, sz3.Y, GraphicsUnit.Pixel, attr);
                    //g.DrawString(sFName, new Font(FontFamily.GenericSansSerif, 8),
                    //    Brushes.Black, (float)ofsX + 6f, (float)ofsY + 5f);
                    //g.DrawString(sFName, new Font(FontFamily.GenericSansSerif, 8),
                    //    Brushes.White, (float)ofsX + 5f, (float)ofsY + 4f);
                }

                g.TranslateTransform(
                    (float)(sz1.X / 2),
                    (float)(sz1.Y / 2));
                g.RotateTransform(-rot);
                g.TranslateTransform(
                    -(float)(sz1.X / 2),
                    -(float)(sz1.Y / 2));
                using (ImageAttributes attr = new ImageAttributes()) {
                    cm[3, 3] = opac1;
                    attr.SetColorMatrix(cm);
                    g.DrawImage(b1, new Rectangle(0, 0, sz1.X, sz1.Y),
                        0, 0, sz1.X, sz1.Y, GraphicsUnit.Pixel, attr);
                }
            }
            b1.Dispose(); b3.Dispose();
            this.Size = (Size)sz1;
            SetBitmap(bo, 255);
        }
        private void frmBlingage_KeyDown(object sender, KeyEventArgs e) {
            if (bEditMode) {
                Point ptOld = ptPicOfs;
                if (e.KeyCode == Keys.Escape) {
                    bEditMode = false;
                    cfgLoad();
                }
                if (e.KeyCode == Keys.Enter) {
                    bEditMode = false;
                    cfgSave();
                }
                if (e.KeyCode == Keys.Up) ptPicOfs.Y -= 1;
                if (e.KeyCode == Keys.Down) ptPicOfs.Y += 1;
                if (e.KeyCode == Keys.Left) ptPicOfs.X -= 1;
                if (e.KeyCode == Keys.Right) ptPicOfs.X += 1;
                if (e.KeyCode == Keys.Q) rot += 1;
                if (e.KeyCode == Keys.A) rot -= 1;
                if (e.KeyCode == Keys.W) opac1 += 0.025f;
                if (e.KeyCode == Keys.S) opac1 -= 0.025f;
                if (e.KeyCode == Keys.E) opac2 += 0.025f;
                if (e.KeyCode == Keys.D) opac2 -= 0.025f;
                while (rot < 0) rot += 360;
                while (rot >= 360) rot -= 360;
                opac1 = Math.Min(Math.Max(opac1, 0), 1);
                opac2 = Math.Min(Math.Max(opac2, 0), 1);
                Render();
            }
        }
        private void frmBlingage_MouseUp(object sender, MouseEventArgs e) {
            Color c = b2.GetPixel(e.X, e.Y);
            if (c == Color.FromArgb(255,255,0,255)) this.Close();
            if (c == Color.FromArgb(255, 255, 0, 0)) { iPic--; Render(); }
            if (c == Color.FromArgb(255, 0, 0, 255)) { iPic++; Render(); }
            if (c == Color.FromArgb(255, 0, 255, 0)) { tSlide.Enabled = !tSlide.Enabled; }
        }



        private void SetBitmap(Bitmap img, byte opacity) {
            if (img.PixelFormat != PixelFormat.Format32bppArgb) {
                MessageBox.Show("Image format not accepted."+ "\r\n\r\n" +
                    "Please use one of the allowed formats:" + "\r\n" +
                    " -- 32bit alphablended (rgba) .PNG",
                    "Oy, bakayaro.", MessageBoxButtons.OK,
                    MessageBoxIcon.Exclamation);
                this.Close(); return;
            }

            IntPtr screenDc = GetDC(IntPtr.Zero);
            IntPtr memDc = CreateCompatibleDC(screenDc);
            IntPtr hBitmap = IntPtr.Zero;
            IntPtr oldBitmap = IntPtr.Zero;

            try {
                hBitmap = img.GetHbitmap(Color.FromArgb(0));
                oldBitmap = SelectObject(memDc, hBitmap);

                Size size = new Size(img.Width, img.Height);
                Point pointSource = new Point(0, 0);
                Point topPos = new Point(Left, Top);
                BlendF blend = new BlendF();
                blend.BlendOp = AC_SRC_OVER;
                blend.BlendFlags = 0;
                blend.SourceConstantAlpha = opacity;
                blend.AlphaFormat = AC_SRC_ALPHA;

                UpdateLayeredWindow(this.Handle, screenDc, ref topPos, ref size,
                    memDc, ref pointSource, 0, ref blend, LWA_ALPHA);
            }
            finally {
                ReleaseDC(IntPtr.Zero, screenDc);
                if (hBitmap != IntPtr.Zero) {
                    SelectObject(memDc, oldBitmap);
                    DeleteObject(hBitmap);
                }
                DeleteDC(memDc);
            }
        }
        protected override CreateParams CreateParams {
            get {
                CreateParams cp = base.CreateParams;
                cp.ExStyle |= WS_EX_LAYERED;
                return cp;
            }
        }
        private void frmBlingage_MouseDown(object sender, MouseEventArgs e) {
            if (e.Button == MouseButtons.Right) {
                MenuItem[] mi = new MenuItem[3];
                mi[0] = new MenuItem("Edit mode");
                mi[1] = new MenuItem("Wallpaper");
                mi[2] = new MenuItem("Exit");
                mi[0].Click += new EventHandler(mi_Editmode);
                mi[1].Click += new EventHandler(mi_Wallpaper);
                mi[2].Click += new EventHandler(mi_Exit);
                ContextMenu cm = new ContextMenu(mi);
                Point ptLoc = Cursor.Position;
                ptLoc.X -= this.Left;
                ptLoc.Y -= this.Top;
                cm.Show(this, ptLoc);
            }
            if (e.Button == MouseButtons.Left) {
                ptFrmOfs = e.Location;
            }
        }
        private void frmBlingage_MouseMove(object sender, MouseEventArgs e) {
            if (e.Button == MouseButtons.Left)
                this.Location = new Point(
                    this.Left + e.X - ptFrmOfs.X,
                    this.Top + e.Y - ptFrmOfs.Y);
        }
        void mi_Editmode(object sender, EventArgs e) {
            bEditMode = true;
            MessageBox.Show("Edit mode enabled." + "\r\n\r\n" +
                "Arrows  -  move picture" + "\r\n" +
                "Q / A  -  rotate picture" + "\r\n" +
                "W / S  -  background opacity" + "\r\n" +
                "E / D  -  picture opacity" + "\r\n\r\n" +
                "Escape  -  cancel" + "\r\n" +
                "Enter  -  save", "Edit mode",
                MessageBoxButtons.OK, MessageBoxIcon.Information);
        }
        void mi_Wallpaper(object sender, EventArgs e) {
            
        }
        void mi_Exit(object sender, EventArgs e) {
            this.Close();
        }

        private void frmBlingage_FormClosing(object sender, FormClosingEventArgs e) {
            cfgSave();
            for (double a = 255 - 25; a >= 0; a -= 255 / 20.0) {
                SetBitmap(bo, (byte)a);
                Application.DoEvents();
                System.Threading.Thread.Sleep(10);
            }
        }
        private void tPosition_Tick(object sender, EventArgs e) {
            //this.SendToBack();
            this.WindowState = FormWindowState.Normal;
        }
        private void frmBlingage_Enter(object sender, EventArgs e) {
            this.SendToBack();
        }

        private void tSlide_Tick(object sender, EventArgs e) {
            iPic++; Render();
        }
    }
}
