using System;
using System.Collections;
using System.Drawing;
using System.Windows.Forms;

namespace picBrowse {
    public partial class frmMain : Form {
        public frmMain() {
            InitializeComponent();
        }

        bool bLoaded;
        bool inpResizing;
        bool inpShowThumbs;
        Point ptFormDragOffset;
        Point ptFormResizeOffset;
        NotifyIcon nico = new NotifyIcon();

        enum frmState { Normal, Full, Kiosk }
        frmState fsFrmState = frmState.Normal;
        Rectangle rcFrmSize = Rectangle.Empty;
        
        Database db = new Database();
        ThumbnailViewer thViewer;
        ImageCollection th = new ImageCollection();
        ImageCollection ic = new ImageCollection();
        ImageData[] id = new ImageData[0];
        TextBox tConKeys = new TextBox();
        ArrayList alUndocked = new ArrayList();
        Random rnd = new Random();
        Panel[] pnaSide, pnaMain;
        bool bSetNewPic = false;
        int iCurPic = 0;
        
        PictureBox pnMainBG = null;
        ContentAlignment pnMainBGa = ContentAlignment.MiddleCenter;
        Timer tFixTitle = new Timer();
        Timer tSlideshow = new Timer();
        
        //Form bling
        private void TanMove() {
            if (pnMainBG == null) return;
            Point szPar = (Point)pnMain.Size;
            Point szPic = (Point)pnMainBG.Size;
            Point ptLoc = Point.Empty;
            if (pnMainBGa == ContentAlignment.TopCenter ||
                pnMainBGa == ContentAlignment.MiddleCenter ||
                pnMainBGa == ContentAlignment.BottomCenter)
                ptLoc.X = (szPar.X - szPic.X) / 2;
            if (pnMainBGa == ContentAlignment.TopRight ||
                pnMainBGa == ContentAlignment.MiddleRight ||
                pnMainBGa == ContentAlignment.BottomRight)
                ptLoc.X = szPar.X - szPic.X;
            if (pnMainBGa == ContentAlignment.MiddleLeft ||
                pnMainBGa == ContentAlignment.MiddleCenter ||
                pnMainBGa == ContentAlignment.MiddleRight)
                ptLoc.Y = (szPar.Y - szPic.Y) / 2;
            if (pnMainBGa == ContentAlignment.BottomLeft ||
                pnMainBGa == ContentAlignment.BottomCenter ||
                pnMainBGa == ContentAlignment.BottomRight)
                ptLoc.Y = szPar.Y - szPic.Y;
            pnMainBG.Location = ptLoc;
        }
        private void TanPick() {
            if (pnMainBG == null) return;
            string[] tans = System.IO.Directory.
                GetFiles("skin\\tan", "*.png");
            int i = rnd.Next(0, tans.Length);
            pnMainBG.Image = new Bitmap(tans[i]);
            pnMainBG.Size = pnMainBG.Image.Size;
            string pos = tans[i].Substring(tans[i].LastIndexOf("\\") + 1, 2);
            if (pos == "tl") pnMainBGa = ContentAlignment.TopLeft;
            if (pos == "tc") pnMainBGa = ContentAlignment.TopCenter;
            if (pos == "tr") pnMainBGa = ContentAlignment.TopRight;
            if (pos == "ml") pnMainBGa = ContentAlignment.MiddleLeft;
            if (pos == "mc") pnMainBGa = ContentAlignment.MiddleCenter;
            if (pos == "mr") pnMainBGa = ContentAlignment.MiddleRight;
            if (pos == "bl") pnMainBGa = ContentAlignment.BottomLeft;
            if (pos == "bc") pnMainBGa = ContentAlignment.BottomCenter;
            if (pos == "br") pnMainBGa = ContentAlignment.BottomRight;
            TanMove();
        }
        private void pnMain_DoubleClick(object sender, EventArgs e) {
            TanPick();
        }
        
        //Temporary
        private void frmMain_DragOver(object sender, DragEventArgs e) {
            e.Effect = DragDropEffects.All;
        }
        private void frmMain_DragDrop(object sender, DragEventArgs e) {
            string[] sFiles = (string[])e.Data.GetData(DataFormats.FileDrop);
            ShowPics(sFiles);
        }
        private void ShowPics(string[] sFiles) {
            bool[] bAdd = new bool[sFiles.Length]; int iAdd = 0;
            for (int a = 0; a < sFiles.Length; a++) {
                sFiles[a] = sFiles[a].ToLower();
                if (sFiles[a].EndsWith(".png") ||
                    sFiles[a].EndsWith(".jpg") ||
                    sFiles[a].EndsWith(".gif")) {
                    bAdd[a] = true;
                    iAdd++;
                }
            }
            int iCnt = 0;
            id = new ImageData[iAdd];
            for (int a = 0; a < sFiles.Length; a++) {
                if (bAdd[a]) {
                    id[iCnt] = new ImageData();
                    id[iCnt].sPath = sFiles[a];
                    id[iCnt].sHash = sFiles[a];
                    iCnt++;
                }
            }
            for (int a = 0; a < id.Length; a++) {
                th.Add(id[a].sHash, id[a].sPath, 0,
                    ImageCollection.imType.Thumbnail);
            }
            thViewer.DrawControls(id.Length);
            thViewer.FlushDisplay();
            Application.DoEvents();
            th.Load();
            thViewer.DisplayImages(new Point(0, id.Length), id, th);
            tShowThumbs.Start();
        }

        //Where shit happens
        private void frmMain_Load(object sender, EventArgs e) {
            frmSplash splash = new frmSplash();
            splash.Show(); Application.DoEvents();

            //Tray icon
            nico.Icon = this.Icon;
            nico.DoubleClick += delegate(object lol, EventArgs dongs) {
                this.Visible = !this.Visible;
            }; nico.Visible = true;
            //Size in title
            tFixTitle.Interval = 2000;
            tFixTitle.Tick += delegate(object lol, EventArgs dongs) {
                tFixTitle.Stop();
                Main_Title.Text = Application.
                    ProductName + " v0.0.1";
            }; tFixTitle.Start();
            //Slideshow timer
            tSlideshow.Tick += delegate(object lol, EventArgs dongs) {
                Footer_cmNext_Click(lol, dongs);
            }; tSlideshow.Interval = 5000;

            //Thumbnail viewer
            thViewer = new ThumbnailViewer(pnMain);
            pnaSide = new Panel[] { pnSide };
            pnaMain = new Panel[] { pnMain, pnDisp };

            //Main panel bg
            if (System.IO.Directory.Exists("skin\\tan") &&
                System.IO.Directory.GetFiles("skin\\tan", "*.png").Length > 0) {
                pnMainBG = new PictureBox();
                pnMainBG.Visible = true;
                pnMain.Controls.Add(pnMainBG);
                TanPick();
            }

            //Assign graphics
            Resources.Prep();
            Main_Minimize.Image = Resources.getr("min");
            Main_Maximize.Image = Resources.getr("max");
            Main_Close.Image = Resources.getr("close");
            SidebarToggle.Image = Resources.getr("close");
            SidebarUndock.Image = Resources.getr("max");
            SidebarChange.Image = Resources.getr("min");

            //Hotkeys bullshit
            tConKeys.Visible = true;
            tConKeys.Size = new Size(1, 1);
            tConKeys.Location = new Point(-8, -8);
            tConKeys.KeyDown += new KeyEventHandler(bConKeys_KeyDown);
            this.Controls.Add(tConKeys);

            //this.Size = new Size(800, 480);
            this.Opacity = 0; this.Show();
            Application.DoEvents(); bLoaded = true;
            frmMain_Resize(new object(), new EventArgs());

            System.Threading.Thread.Sleep(500);
            for (int a = 0; a <= 10; a++) {
                this.Opacity = (double)a / 10;
                Application.DoEvents();
                System.Threading.Thread.Sleep(10);
            }
            System.Threading.Thread.Sleep(250);
            splash.Close(); splash.Dispose();
        }
        void bConKeys_KeyDown(object sender, KeyEventArgs e) {
            if (e.KeyCode == Keys.F11) {
                if (fsFrmState == frmState.Normal) {
                    rcFrmSize = this.Bounds;
                }
                if (fsFrmState != frmState.Kiosk) {
                    fsFrmState = frmState.Kiosk;
                    this.Bounds = Screen.GetBounds(this);
                    pbDisp.BackColor = Color.FromArgb(0, 0, 0);
                    this.Controls.Add(pbDisp);
                    pbDisp.BringToFront();
                    pbResize.Visible = false;
                }
                else {
                    this.Bounds = rcFrmSize;
                    fsFrmState = frmState.Normal;
                    pbDisp.BackColor = pnDisp.BackColor;
                    pnDisp.Controls.Add(pbDisp);
                    pbResize.Visible = true;
                }
            }
            if (e.KeyCode == Keys.Left) {
                Footer_cmPrev_Click(new object(), new EventArgs());
            }
            if (e.KeyCode == Keys.Right) {
                Footer_cmNext_Click(new object(), new EventArgs());
            }
            this.Text = "";
        }
        private void General_pbDatabase_MouseDown(object sender, MouseEventArgs e) {
            string[] dbs = System.IO.Directory.GetFiles("_db");
            MenuItem[] mi = new MenuItem[dbs.Length + 2];
            for (int a = 0; a < dbs.Length; a++) {
                dbs[a] = dbs[a].Substring(dbs[a].LastIndexOf("\\") + 1);
                dbs[a] = dbs[a].Substring(0, dbs[a].IndexOf(".db"));
                mi[a] = new MenuItem(dbs[a]);
            }
            mi[mi.Length - 2] = new MenuItem("-------------");
            mi[mi.Length - 2].Enabled = false;
            mi[mi.Length - 1] = new MenuItem("Create...");
            ContextMenu cm = new ContextMenu(mi);
            for (int a = 0; a < mi.Length; a++) {
                mi[a].Click += new EventHandler(ddDatabaseSelect);
            }
            Point ptShowAt = Cursor.Position;
            ptShowAt.X -= this.Left;
            ptShowAt.Y -= this.Top;
            cm.Show(this, ptShowAt);
        }
        void ddDatabaseSelect(object sender, EventArgs e) {
            string sDB = ((MenuItem)sender).Text;
            if (sDB == "Create...") {
                ddDatabaseSet("");
                string var = InputBox.Show(
                    "Please enter a name for the new database.",
                    "Create database", "Main").Text;
                if (var == "") return;
                if (cb.ContainsAny(var, cb.IllegalName)) {
                    if (DialogResult.Cancel == MessageBox.Show(
                        "Windows forbids the use of some of the" + "\r\n" +
                        "characters you entered. Please try again." + "\r\n\r\n" +
                        "Forbidden characters:  " + cb.IllegalName, "Oh shi-",
                        MessageBoxButtons.OKCancel, MessageBoxIcon.Exclamation))
                        return;
                    ddDatabaseSelect(sender, e); return;
                }
                sDB = var;
                var = "_db\\" + var + ".db";
                if (System.IO.File.Exists(var)) {
                    DialogResult dr = MessageBox.Show(
                        "WARNING! This database already exists!" + "\r\n\r\n" +
                        "By selecting [Yes], the existing database will be overwritten." + "\r\n" +
                        "Any information stored in that database will be DELETED." + "\r\n\r\n" +
                        "Are you sure you wish to continue?", "Dude...",
                        MessageBoxButtons.YesNoCancel, MessageBoxIcon.Warning);
                    if (dr == DialogResult.Cancel) return;
                    if (dr == DialogResult.No) {
                        ddDatabaseSelect(sender, e);
                        return;
                    }
                }
                bool win = true;
                db.Close(); db = new Database();
                if (!db.Create(var, true)) win = false;
                if (!db.Close()) win = false;
                if (win) MessageBox.Show("Success!" + "\r\n\r\n" +
                    "The database was created successfully." + "\r\n" +
                    "You may continue using " + Application.ProductName + ".",
                    "Fuck yeah", MessageBoxButtons.OK, MessageBoxIcon.Information);
                else MessageBox.Show("Aw balls." + "\r\n\r\n" +
                    "An unexpected error occured while creating the new database." + "\r\n" +
                    "Make sure that " + Application.ProductName + " has write access to the _db folder.",
                    "Oh shi-", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
            }
            ddDatabaseSet(sDB);
        }
        private void ddDatabaseSet(string var) {
            if (!db.Open("_db\\" + var + ".db"))
                var = "-- I/O ERROR --";
            Bitmap tmp = Resources.getc("drop");
            using (Graphics g = Graphics.FromImage(tmp)) {
                g.DrawString(var, new Font(this.Font.FontFamily, 8),
                    Brushes.Black, new PointF(7, 5));
                g.DrawString(var, new Font(this.Font.FontFamily, 8),
                    Brushes.White, new PointF(6, 4));
            }
            if (General_pbDatabase.Image != null)
                General_pbDatabase.Image.Dispose();
            General_pbDatabase.Image = tmp;
        }
        private void DisplayPicChange(int i) {
            i+=iCurPic;
            if (i >= id.Length) i = 0;
            if (i < 0) i = id.Length - 1;
            DisplayPicSet(i);
            tConKeys.Focus();
        }
        private void DisplayPicSet(int i) {
            ic.SetGen(i - iCurPic); //Set generation
            ic.Rinse(3, ImageCollection.imType.Resized);
            for (int a = 0; a < 3; a++) {
                if (id.Length > i + a)
                    ic.Add(id[i + a].sHash, id[i + a].sPath,
                        +a, ImageCollection.imType.Resized);
                if (i - a >= 0)
                    ic.Add(id[i - a].sHash, id[i - a].sPath,
                        -a, ImageCollection.imType.Resized);
            }
            iCurPic = i; //Set new opened image
            bSetNewPic = true; //We need to update display
            if (!DisplayPicSet()) { //Image not in bm cache
                ic.Prio(id[i].sHash, ImageCollection.imType.Resized);
                ic.Load(); //Prioritize and init load of opened image
            }
            pbDisp.Visible = true; //We are displaying an image
            ShowPanel(pnDisp); //Show display in GUI
        }
        private bool DisplayPicSet() {
            if (bSetNewPic && iCurPic < id.Length) {
                Bitmap bm = ic.Get(id[iCurPic].sHash,
                    ImageCollection.imType.Resized);
                //if (pbDisp.Image != null)
                //    pbDisp.Image = null;
                if (bm != null) {
                    pbDisp.Image = bm;
                    bSetNewPic = false;
                    ic.Load();
                    return true;
                }
            }
            return false;
        }
        private void tShowThumbs_Tick(object sender, EventArgs e) {
            if (!bLoaded) return;
            if (inpShowThumbs) return;
            inpShowThumbs = true;
            pnMainBG.SendToBack();
            if (DisplayPicSet())
                Application.DoEvents();
            inpShowThumbs = false;
        }
        private void ShowPanel(Panel pn) {
            ShowPanel(pn, pnaMain);
            ShowPanel(pn, pnaSide);
            pn.Visible = true;
        }
        private void ShowPanel(Panel pn, Panel[] pna) {
            int isMember = -1;
            for (int a = 0; a < pna.Length; a++)
                if (pn == pna[a]) isMember = a;
            if (isMember != -1)
                for (int a = 0; a < pna.Length; a++)
                    if (a != isMember) pna[a].Visible = false;
        }

        //GUI Form
        private void Main_Minimize_Click(object sender, EventArgs e) {
            if (Program.Linux)
                this.FormBorderStyle = FormBorderStyle.Sizable;
            this.WindowState = FormWindowState.Minimized;
        }
        private void Main_Maximize_Click(object sender, EventArgs e) {
            if (fsFrmState == frmState.Kiosk) return;
            if (fsFrmState == frmState.Normal) {
                rcFrmSize = this.Bounds;
                this.Bounds = Screen.GetWorkingArea(this);
                fsFrmState = frmState.Full;
                pbResize.Visible = false;
            }
            else {
                this.Bounds = rcFrmSize;
                fsFrmState = frmState.Normal;
                pbResize.Visible = true;
            }
        }
        private void Main_Close_Click(object sender, EventArgs e) {
            Exit();
        }
        private void pnTop_MouseDown(object sender, MouseEventArgs e) {
            if (e.Button == MouseButtons.Left) {
                ptFormDragOffset = e.Location;
                if (e.Location.X > pnTop.Width - 24 &&
                    e.Location.Y < 16) Exit();
            }
        }
        private void pnTop_MouseMove(object sender, MouseEventArgs e) {
            if (e.Button == MouseButtons.Left &&
                fsFrmState == frmState.Normal) {
                Point ptLoc = this.Location;
                ptLoc.X += e.Location.X - ptFormDragOffset.X;
                ptLoc.Y += e.Location.Y - ptFormDragOffset.Y;
                this.Location = ptLoc;
            }
        }
        private void pnTop_MouseDoubleClick(object sender, MouseEventArgs e) {
            Main_Maximize_Click(new object(), new EventArgs());
        }
        private void pbResize_MouseDown(object sender, MouseEventArgs e) {
            if (e.Button == MouseButtons.Left)
                ptFormResizeOffset = e.Location;
        }
        private void pbResize_MouseMove(object sender, MouseEventArgs e) {
            if (e.Button == MouseButtons.Left) {
                Size szFrm = this.Size;
                szFrm.Width += e.X - ptFormResizeOffset.X;
                szFrm.Height += e.Y - ptFormResizeOffset.Y;
                this.Size = szFrm;
            }
        }
        private void frmMain_Resize(object sender, EventArgs e) {
            if (!bLoaded) return;
            if (inpResizing) return;
            inpResizing = true;
            if (this.WindowState == FormWindowState.Minimized) {
                this.FormBorderStyle = FormBorderStyle.None;
                inpResizing = false; return;
            }
            Point ptSize = (Point)this.Size;
            if (ptSize.X < 640 || ptSize.Y < 400) {
                if (ptSize.X < 640) ptSize.X = 640;
                if (ptSize.Y < 400) ptSize.Y = 400;
                this.Size = (Size)ptSize;
            }
            Size szSide = pnSide.Size;
            Size szMain = pnMain.Size;
            Point ptSide = pnSide.Location;
            Point ptMain = pnMain.Location;
            bool hasSide = false;
            for (int a = 0; a < pnaSide.Length; a++)
                if (pnaSide[a].Visible &&
                    pnaSide[a].Parent == this)
                    hasSide = true;

            pnTop.Width = ptSize.X;
            szSide.Height = ptSize.Y - 97;
            szMain.Height = ptSize.Y - 97;
            if (hasSide) {
                ptMain.X = 180;
                szMain.Width = ptSize.X - 180;
            }
            else {
                ptMain.X = 0;
                szMain.Width = ptSize.X;
            }
            for (int a = 0; a < pnaMain.Length; a++) {
                pnaMain[a].Size = szMain;
                pnaMain[a].Location = ptMain;
            }
            for (int a = 0; a < pnaSide.Length; a++) {
                pnaSide[a].Size = szSide;
                //pnaSide[a].Location = ptSide;
            }
            pnBtm.Width = ptSize.X;
            pnBtm.Top = ptSize.Y - 40;
            TanMove();

            Main_Title.Left = (ptSize.X - Main_Title.Width) / 2;
            Main_Close.Left = ptSize.X - 20;
            Main_Maximize.Left = ptSize.X - 38;
            Main_Minimize.Left = ptSize.X - 56;
            Main_Title.Text = "[" + ptSize.X + "x" + ptSize.Y + "]";
            tFixTitle.Stop(); tFixTitle.Start();
            tResize.Stop(); tResize.Start();
            inpResizing = false;
        }
        private void pnBtm_Resize(object sender, EventArgs e) {
            Point ptSize = (Point)this.Size;
            pbResize.Left = ptSize.X - 14;
        }
        private void tResize_Tick(object sender, EventArgs e) {
            tResize.Stop();
            thViewer.Resize();
            /*if (pnMainBG != null)
            {
                Bitmap bg = new Bitmap(pnMain.Width, pnMain.Height);
                DrawAligned(pnMainBG, bg, pnMainBGa);
                if (pnMain.BackgroundImage != null)
                    pnMain.BackgroundImage.Dispose();
                pnMain.BackgroundImage = bg;
            }*/
        }
        private void Main_Title_DoubleClick(object sender, EventArgs e) {
            string[] str = InputBox.Show("Enter a window size.",
                "Width X Height").Text.Split('x');
            this.Width = Convert.ToInt32(str[0].Trim());
            this.Height = Convert.ToInt32(str[1].Trim());
        }
        private void frmMain_FormClosing(object sender, FormClosingEventArgs e) {
            nico.Dispose();
        }
        private void Exit() {
            this.Close();
        }

        //GUI Sidebar
        private void SidebarToggle_MouseDown(object sender, MouseEventArgs e) {
            bool bVisible = !pnSide.Visible;
            pnSide.Visible = bVisible;
            SidebarUndock.Visible = bVisible;
            SidebarChange.Visible = bVisible;
            if (!bVisible)
                SidebarToggle.Image = Resources.getr("min");
            else SidebarToggle.Image = Resources.getr("close");
            frmMain_Resize(new object(), new EventArgs());
        }
        private void SidebarUndock_MouseDown(object sender, MouseEventArgs e) {
            int iTop = this.Top + pnSide.Top;
            Point ptLoc = new Point(this.Left, iTop);
            frmSidebar frm = new frmSidebar(pnSide, ptLoc);
            frmMain_Resize(new object(), new EventArgs());
            alUndocked.Add(frm); frm.Show();
        }
        private void tRedock_Tick(object sender, EventArgs e) {
            bool bResize = false;
            for (int a = 0; a < alUndocked.Count; a++) {
                frmSidebar frm = ((frmSidebar)alUndocked[a]);
                if (frm.bClosed) {
                    this.Controls.Add(frm.pn);
                    frm.pn.Location = new Point(0, 56);
                    frm.Close(); frm.Dispose();
                    //This is to AVOID confusion.
                    alUndocked.RemoveAt(a); a--;
                    bResize = true;
                }
            }
            if (bResize) frmMain_Resize(new object(), new EventArgs());
        }

        //Menu shit
        private void MenuFileExit_Click(object sender, EventArgs e) {
            Exit();
        }
        private void MenuHelpDocumentation_Click(object sender, EventArgs e) {
            ShowPics(System.IO.Directory.GetFiles("testpics_ifl"));
        }
        private void MenuHelpAbout_Click(object sender, EventArgs e) {
            new frmAbout().Show();
        }
        private void MenuFileImportImNewDir_Click(object sender, EventArgs e) {
            new frmImport(db).Show();
        }
        private void MenuHelpWhatis_Click(object sender, EventArgs e) {
            //id[0] = id[1];
            //thViewer.DisplayImages(new Point(0, id.Length));
            System.ComponentModel.BackgroundWorker bw = new System.ComponentModel.BackgroundWorker();
            bw.DoWork += new System.ComponentModel.DoWorkEventHandler(bw_DoWork);
            bw.RunWorkerAsync(); System.Threading.Thread.Sleep(500);
            bw.WorkerSupportsCancellation = true;
            bw.CancelAsync();
        }
        void bw_DoWork(object sender, System.ComponentModel.DoWorkEventArgs e) {
            System.ComponentModel.BackgroundWorker me = (System.ComponentModel.BackgroundWorker)sender;
            using (Bitmap b = new Bitmap("pic.png")) {
                if (me.CancellationPending) return;
                using (Bitmap c = cim.Resize(b, new Size(2048, 9001), true, false)) {
                    if (me.CancellationPending) return;
                    using (Bitmap d = cim.Resize(b, new Size(1024, 9001), true, false)) {
                        if (me.CancellationPending) return;
                        PictureBox pb = new PictureBox();
                        pb.SizeMode = PictureBoxSizeMode.Zoom;
                        pb.Dock = DockStyle.Fill;
                        pb.Visible = true;
                        pb.Image = d;
                        Form frm = new Form();
                        frm.Size = new Size(1024, 512);
                        frm.Controls.Add(pb);
                        Application.Run(frm);
                    }
                }
            }
        }
        private void MenuDebugDisplay_Click(object sender, EventArgs e) {
            new frmDebug(ic).Show();
        }

        //Motherfucking horrible shit made of fail and aids.
        private void tInterop_Tick(object sender, EventArgs e) {
            if (!bLoaded) return;
            int iClickLD = thViewer.GetClickLD();
            if (iClickLD != -1) {
                ShowPanel(pnDisp);
                DisplayPicSet(iClickLD +
                    thViewer.GetRange().X);
                tConKeys.Focus();
            }
        }
        private void Footer_cmPrev_Click(object sender, EventArgs e) {
            DisplayPicChange(-1);
        }
        private void Footer_cmNext_Click(object sender, EventArgs e) {
            DisplayPicChange(+1);
        }
        private void pbDisp_Click(object sender, EventArgs e) {
            ShowPanel(pnMain);
            pbDisp.Visible = false;
            pbDisp.Image = null;
            tConKeys.Focus();
        }

        private void MenuViewBlingageShow_Click(object sender, EventArgs e) {
            bool enabled = !MenuViewBlingageShow.Checked;
            MenuViewBlingageShow.Checked = enabled;
            frmBlingage bling = new frmBlingage();
            //bling.TopLevel = false;
            bling.TopMost = false;
            bling.Show();
        }
        private void MenuViewBlingageSelSet_Click(object sender, EventArgs e) {
            string vars = "";
            string sPath = "skin\\blingage\\list.txt";
            for (int a = 0; a < id.Length; a++)
                vars += id[a].sPath + "\r\n";
            System.IO.File.WriteAllText(sPath, vars);
        }
        private void MenuViewBlingageSelAdd_Click(object sender, EventArgs e) {
            string sPath = "skin\\blingage\\list.txt";
            string vars = System.IO.File.ReadAllText(sPath);
            for (int a = 0; a < id.Length; a++)
                vars += id[a].sPath + "\r\n";
            System.IO.File.WriteAllText(sPath, vars);
        }

        private void MenuFileImportImSShot_Click(object sender, EventArgs e) {
            new frmScreenshot().Show();
        }

        private void Footer_cmSlide_Click(object sender, EventArgs e) {
            tSlideshow.Enabled = !tSlideshow.Enabled;
        }
        private void Footer_cmSlide_DoubleClick(object sender, EventArgs e) {
            string delay = InputBox.Show("Enter a delay (seconds)").Text;
            try { tSlideshow.Interval = (int)(Convert.ToDouble(delay) * 1000); }
            catch { MessageBox.Show("Invalid formatting."); }
            tSlideshow.Enabled = !tSlideshow.Enabled;
        }
    }
}
