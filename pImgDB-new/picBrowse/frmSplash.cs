using System;
using System.Drawing;
using System.Drawing.Imaging;
using System.IO;
using System.Runtime.InteropServices;
using System.Windows.Forms;

namespace picBrowse
{
    public partial class frmSplash : Form
    {
        public frmSplash()
        {
            InitializeComponent();
        }

        private const Int32 LWA_COLORKEY = 0x1;
        private const Int32 LWA_ALPHA = 0x2;
        private const Int32 WS_EX_LAYERED = 0x00080000;

        public struct BlendF
        {
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
        public static extern IntPtr GetForegroundWindow();

        Bitmap img;
        private void frmSplash_Load(object sender, EventArgs e)
        {
            img = Image.FromFile("skin\\splash.png") as Bitmap;
            SetBitmap(img, 0); this.Show(); Application.DoEvents();
            for (double a = 0; a <= 255; a+=25.5)
            {
                SetBitmap(img, (byte)a);
                System.Threading.Thread.Sleep(10);
            }
        }
        private void SetBitmap(Bitmap img, byte opacity)
        {
            if (img.PixelFormat != PixelFormat.Format32bppArgb) {
                MessageBox.Show("Only accepts 32bit alphablended .png images");
                System.Diagnostics.Process.GetCurrentProcess().Kill();
            }

            IntPtr screenDc = GetDC(IntPtr.Zero);
            IntPtr memDc = CreateCompatibleDC(screenDc);
            IntPtr hBitmap = IntPtr.Zero;
            IntPtr oldBitmap = IntPtr.Zero;

            try
            {
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
            finally
            {
                ReleaseDC(IntPtr.Zero, screenDc);
                if (hBitmap != IntPtr.Zero)
                {
                    SelectObject(memDc, oldBitmap);
                    DeleteObject(hBitmap);
                }
                DeleteDC(memDc);
            }
        }
        protected override CreateParams CreateParams
        {
            get
            {
                CreateParams cp = base.CreateParams;
                cp.ExStyle |= WS_EX_LAYERED; // This form has to have the WS_EX_LAYERED extended style
                return cp;
            }
        }

        private void frmSplash_FormClosing(object sender, FormClosingEventArgs e)
        {
            for (double a = 255 - 25.5; a >= 0; a -= 25.5)
            {
                SetBitmap(img, (byte)a);
                System.Threading.Thread.Sleep(10);
            }
        }
    }
}
