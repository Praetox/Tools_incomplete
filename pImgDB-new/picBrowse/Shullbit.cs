using System;
using System.Collections;
using System.ComponentModel;
using System.Data.SQLite;
using System.Drawing;
using System.Windows.Forms;
using System.IO;

namespace picBrowse {
    public class ThumbnailViewer {
        Panel parent; //Parent control
        Point pSize; //Parent size
        bool[] pbHasImg = new bool[0]; //Picturebox is null
        PictureBox[] pb = new PictureBox[0]; //Pictures
        Label[] lb = new Label[0]; //Picture shadows
        string[] hash = new string[0]; //Pic hashes
        Point Range = Point.Empty; //ID+Th range
        bool isLoading = false; //Loading images
        bool isDrawing = false; //Drawing controls
        Timer tMoar; //Poll for more images

        ImageData[] id; //Reference
        ImageCollection th; //Reference
        int Count = 0; //Reference
        int iClickLD = -1;
        int iClickR = -1;

        const int iPadding = 16;
        const int iShadow = 4;
        const int iSpacing = 8;
        struct Efficiency {
            public Point Size;
            public Point Count;
            public Point Wasted;
        }

        public ThumbnailViewer(Panel parent) {
            this.parent = parent;
            pSize = (Point)parent.Size;
            tMoar = new Timer();
            tMoar.Tick += new EventHandler(PollForMore);
            tMoar.Interval = 1;
            tMoar.Start();
        }
        public void Resize() {
            pSize = (Point)parent.Size;
            DrawControls(Count);
            DisplayImages(id, th);
        }
        public void DrawControls(int Count) {
            //tmr tm = new tmr(); tm.Start();
            if (isDrawing) return; isDrawing = true;
            this.Count = Count;
            Efficiency eff = GetBestEfficiency(Count);
            int count = eff.Count.X * eff.Count.Y;
            if (pb != null) {
                parent.Visible = false;
                for (int a = 0; a < pb.Length; a++) {
                    pb[a].Dispose();
                    lb[a].Dispose();
                }
                parent.Visible = true;
            }
            //double d = tm.Stop();
            //if (d > 0.1) MessageBox.Show("" + d);
            lb = new Label[count];
            pb = new PictureBox[count];
            hash = new string[count];
            pbHasImg = new bool[count];
            Point thSpace = GetThumbArea(eff.Size);
            Size lbSize = (Size)eff.Size;
            lbSize.Width -= 2;
            lbSize.Height -= 2;
            for (int y = 0; y < eff.Count.Y; y++)
                for (int x = 0; x < eff.Count.X; x++) {
                    int i = (y * eff.Count.X) + x;
                    pb[i] = new PictureBox();
                    lb[i] = new Label();
                    hash[i] = "";
                    pb[i].Tag = i;
                    lb[i].Tag = i;
                    pb[i].Size = (Size)eff.Size;
                    lb[i].Size = lbSize;
                    pb[i].BackColor = Color.FromArgb(80, 80, 80);
                    lb[i].BackColor = Color.FromArgb(32, 32, 32);
                    pb[i].BorderStyle = BorderStyle.FixedSingle;
                    pb[i].SizeMode = PictureBoxSizeMode.Zoom;
                    pb[i].MouseClick += new MouseEventHandler(thViewerPic_Click);
                    pb[i].MouseDoubleClick += new MouseEventHandler(thViewerPic_DClick);
                    Point loc = new Point(
                        iPadding + x * thSpace.X,
                        iPadding + y * thSpace.Y);
                    pb[i].Location = loc;
                    loc.X += iShadow;
                    loc.Y += iShadow;
                    lb[i].Location = loc;
                    pb[i].Visible = true;
                    lb[i].Visible = true;
                    parent.Controls.Add(pb[i]);
                    parent.Controls.Add(lb[i]);
                }
            isDrawing = false;
        }

        void thViewerPic_Click(object sender, MouseEventArgs e) {
            int i = (int)((PictureBox)sender).Tag;
            if (e.Button == MouseButtons.Left) {
                bool bSelected = id[i + Range.X].bSelFlip();
                if (bSelected) lb[i].BackColor = Color.FromArgb(255, 255, 255);
                else lb[i].BackColor = Color.FromArgb(32, 32, 32);
            }
            if (e.Button == MouseButtons.Right) {
                iClickR = i;
            }
        }
        void thViewerPic_DClick(object sender, MouseEventArgs e) {
            thViewerPic_Click(sender, e);
            int i = (int)((PictureBox)sender).Tag;
            if (e.Button == MouseButtons.Left) {
                iClickLD = i;
            }
        }
        public bool DisplayImages(Point Range) {
            if (isLoading) return false;
            this.Range = Range;
            return DisplayImages(id, th);
        }
        public bool DisplayImages(Point Range, ImageData[] id, ImageCollection th) {
            if (isLoading) return false;
            this.Range = Range;
            return DisplayImages(id, th);
        }
        public bool DisplayImages(ImageData[] id, ImageCollection th) {
            if (id == null || th == null) return false;
            if (isLoading) return false; isLoading = true;
            this.id = id; this.th = th;
            int RangeCnt = Range.Y - Range.X;
            RangeCnt = Math.Min(RangeCnt, pb.Length);
            bool ret = false;
            for (int a = 0; a < RangeCnt; a++) {
                if (hash[a] != id[a + Range.X].sHash) {
                    pbHasImg[a] = false;
                    pb[a].Image = null;
                }
                if (!pbHasImg[a]) { // && th.Has(id[a+Range.X].sHash,
                    //ImageCollection.imType.Thumbnail)) {
                    hash[a] = id[a + Range.X].sHash;
                    pb[a].Image = th.Get(id[a + Range.X].sHash,
                        ImageCollection.imType.Thumbnail);
                    pbHasImg[a] = (pb[a].Image != null);
                    if (pbHasImg[a]) ret = true;
                }
            }
            isLoading = false;
            return ret;
        }
        public void PollForMore(object sender, EventArgs e) {
            if (id == null) return;
            if (th == null) return;
            string[] ready = th.Ready();
            for (int a = 0; a < ready.Length; a++)
                for (int b = 0; b < hash.Length; b++)
                    if (ready[a] == hash[b])
                        pb[b].Image = th.Get(id[b + Range.X].sHash,
                            ImageCollection.imType.Thumbnail);
        }
        public void FlushDisplay() {
            for (int a = 0; a < pb.Length; a++)
                if (pb[a].Image != null)
                    pb[a].Image = null;
        }
        public Point GetRange() { return Range; }
        public int GetClickLD() { int ret = iClickLD; iClickLD = -1; return ret; }
        public int GetClickR() { int ret = iClickR; iClickR = -1; return ret; }
        private Point GetSize(int x, int y) {
            if (y == 0) y = (int)Math.Round((double)x / 16 * 10);
            if (x == 0) x = (int)Math.Round((double)x / 10 * 16);
            return new Point(x, y);
        }
        private Efficiency GetBestEfficiency(int CountHint) {
            Efficiency match1 = new Efficiency();
            Efficiency match2 = new Efficiency();
            int iStartOfs = 0;
            //Find a rough start offset
            for (int a = 1; a < 9001; a += 10) {
                Efficiency eff = GetEfficiency(GetSize(a, 0));
                int count = eff.Count.X * eff.Count.Y;
                if (count < CountHint) break;
                iStartOfs = a;
            }
            //Find the exact matches
            for (int a = iStartOfs; a < 9001; a++) {
                Efficiency eff = GetEfficiency(GetSize(a, 0));
                int count = eff.Count.X * eff.Count.Y;
                if (count >= CountHint) match1 = eff;
                else { match2 = eff; break; }
            }
            int wasted1 = match1.Wasted.X * match1.Wasted.Y;
            int wasted2 = match2.Wasted.X * match2.Wasted.Y;
            if (wasted1 <= wasted2) return match1;
            else return match2;
        }
        private Efficiency GetEfficiency(Point thSize) {
            Efficiency ret = new Efficiency();
            ret.Size = thSize;
            ret.Wasted = GetPaddedSpace(pSize);
            thSize = GetThumbArea(thSize);
            while (ret.Wasted.X > thSize.X) {
                ret.Count.X++;
                ret.Wasted.X -=
                    thSize.X;
            }
            while (ret.Wasted.Y > thSize.Y) {
                ret.Count.Y++;
                ret.Wasted.Y -=
                    thSize.Y;
            }
            return ret;
        }
        private Point GetPaddedSpace(Point area) {
            area.X -= iPadding;
            area.Y -= iPadding;
            area.X -= iPadding - iSpacing;
            area.Y -= iPadding - iSpacing;
            return area;
        }
        private Point GetThumbArea(Point thumb) {
            thumb.X += iShadow;
            thumb.Y += iShadow;
            thumb.X += iSpacing;
            thumb.Y += iSpacing;
            return thumb;
        }
    }
    public class ImageCollection {
        public enum imType {
            Any,
            Resized,
            Fullsize,
            Thumbnail
        };
        struct imInfo {
            public long gen;
            public Bitmap pic;
            public string hash;
            public string path;
            public imType type;
            public State state;
            public enum State { Idle, Loading, Done };
            public imInfo(Bitmap pic, imType type, string hash, string path, long gen) {
                this.pic = pic;
                this.type = type;
                this.hash = hash;
                this.path = path;
                this.gen = gen;
                state = State.Idle;
                if (this.pic != null)
                    state = State.Done;
            }
            public imInfo GetMeta() {
                imInfo ret = new imInfo(null, type, hash, path, gen);
                return ret;
            }
            public imInfo GetMeta(State stat) {
                imInfo ret = GetMeta();
                ret.state = stat;
                return ret;
            }
        }
        long gen = 0;
        ArrayList im = new ArrayList();
        ArrayList ready = new ArrayList();
        Size szThumb = new Size(320, 200);
        Size szShrink = new Size(1920, 1200);
        bool bPause, bCancel, bBusy;
        string sPrioHash = "";
        int iThreads = 2;

        public ImageCollection() {
            int scr = FindBiggestScreen();
            szShrink = Screen.AllScreens[scr].Bounds.Size;
            iThreads = Convert.ToInt32(Environment.
                GetEnvironmentVariable("number_of_processors"));
        }
        public bool Has(string hash, imType type) {
            //Find bitmap matching hash and type.
            //Returns false if image can't be found.
            if (type == imType.Any) {
                if (Has(hash, imType.Thumbnail)) return true;
                if (Has(hash, imType.Resized)) return true;
                if (Has(hash, imType.Fullsize)) return true;
                return false;
            }
            for (int a = 0; a < im.Count; a++) {
                imInfo inf = (imInfo)im[a];
                if (inf.hash == hash &&
                    inf.type == type) {
                    return (inf.pic != null);
                }
            }
            return false;
        }
        public Bitmap Get(string hash, imType type) {
            //Return bitmap matching hash and type.
            //Returns null if image can't be found.
            if (type == imType.Any) {
                Bitmap b;
                b = Get(hash, imType.Thumbnail); if (b != null) return b;
                b = Get(hash, imType.Resized); if (b != null) return b;
                b = Get(hash, imType.Fullsize); if (b != null) return b;
                return null;
            }
            for (int a = 0; a < im.Count; a++) {
                imInfo inf = (imInfo)im[a];
                if (inf.hash == hash &&
                    inf.type == type) {
                    if (hash == sPrioHash &&
                        inf.pic != null)
                        sPrioHash = "";
                    return inf.pic;
                }
            }
            return null;
        }
        public string List(imType type) {
            string ret = "";
            for (int a = 0; a < im.Count; a++) {
                imInfo inf = (imInfo)im[a];
                bool bType = (type == imType.Any);
                if (!bType) bType = (inf.type == type);
                if (inf.pic != null && bType)
                    ret += "(" + inf.gen + ") " +
                        inf.hash + "\r\n";
            } return ret;
        }
        public void SetGen(int igen) {
            //This shit is so awesome, it even has a method to bring
            //you any generation of electronical equipment. Hontou-ni.
            gen += igen;
        }
        public void Add(string hash, string path, int igen, imType type) {
            //Add an image for loading.
            Set(null, hash, path, igen, type);
        }
        public void Set(Bitmap pic, string hash, string path, long igen, imType type) {
            //Store an already-loaded image.
            for (int a = 0; a < im.Count; a++)
                if (((imInfo)im[a]).hash
                    == hash) return;
            im.Add(new imInfo(pic, type,
                hash, path, gen + igen));
        }
        public bool Prio(string hash, imType type) {
            //Prioritize an image for loading
            for (int a = 0; a < im.Count; a++) {
                imInfo inf = (imInfo)im[a];
                if (inf.type == type &&
                    inf.hash == hash) {
                    im.RemoveAt(a);
                    im.Insert(0, inf);
                    sPrioHash = hash;
                    return true;
                }
            }
            return false;
        }
        public void Rinse(int igen, imType type) {
            //Remove entries exceeding gen. limit.
            for (int a = 0; a < im.Count; a++) {
                imInfo inf = (imInfo)im[a];
                bool bType = (type == imType.Any);
                if (!bType) bType = (inf.type == type);
                if (bType && (
                    inf.gen <= gen - igen ||
                    inf.gen >= gen + igen)) {
                    if (inf.pic != null)
                        inf.pic.Dispose();
                    //To AVOID confusion.
                    im.RemoveAt(a); a--;
                }
            }
        }
        public void Del(string hash, imType type) {
            //Remove entries matching hash and type.
            for (int a = 0; a < im.Count; a++) {
                imInfo inf = (imInfo)im[a];
                bool bType = (type == imType.Any);
                if (!bType) bType = (inf.type == type);
                if (bType && inf.hash == hash) {
                    if (inf.pic != null)
                        inf.pic.Dispose();
                    im.RemoveAt(a); return;
                }
            }
        }
        public void Flush(imType type) {
            //Clear images matching type.
            for (int a = 0; a < im.Count; a++) {
                imInfo inf = (imInfo)im[a];
                bool bType = (type == imType.Any);
                if (!bType) bType = (inf.type == type);
                if (inf.pic != null && bType)
                    inf.pic.Dispose();
            }
            im.Clear();
        }
        public string[] Ready() {
            //Get all recently loaded images.
            string[] ret = new string[ready.Count];
            for (int a = 0; a < ret.Length; a++)
                ret[a] = (string)ready[a];
            ready.RemoveRange(0, ret.Length);
            return ret;
        }
        public void Load() {
            //Load all entries with no image.
            if (bBusy) return; bBusy = true;
            bool[] bLoad = new bool[im.Count];
            for (int a = 0; a < im.Count; a++) {
                if (((imInfo)im[a]).pic == null)
                    bLoad[a] = true;
            }
            System.Threading.Thread thr = new System.Threading.Thread(
                new System.Threading.ParameterizedThreadStart(doLoad));
            thr.IsBackground = true;
            thr.Start(bLoad);
        }
        private void doLoad(object arg) {
            //Loader-thread manager
            System.Threading.Thread[] thr = new
                System.Threading.Thread[iThreads];
            bool bDidSomeWork = true;
            while (bDidSomeWork) {
                bDidSomeWork = false;
                for (int a = 0; a < im.Count; a++) {
                    if (bCancel) break;
                    imInfo imPic = (imInfo)im[a];
                    if (imPic.state == imInfo.State.Idle &&
                        !pollStop(imPic)) {
                        bDidSomeWork = true;
                        for (int b = 0; b < thr.Length; b++) {
                            if (thr[b] == null || thr[b].ThreadState ==
                                System.Threading.ThreadState.Stopped) {
                                imPic = imPic.GetMeta(); //threads are fun
                                im[a] = imPic.GetMeta(imInfo.State.Loading);
                                thr[b] = new System.Threading.Thread(new
                                    System.Threading.ParameterizedThreadStart(doLoadTh));
                                thr[b].Priority = System.Threading.ThreadPriority.Lowest;
                                thr[b].IsBackground = true;
                                thr[b].Start(imPic);
                                break;
                            }
                        }
                        System.Threading.Thread.Sleep(1);
                        break; /*Remove for fancy effect*/
                    }
                }
            }
            bBusy = false;
        }
        private void doLoadTh(object arg) {
            //A single image loader thread.
            imInfo imPic = (imInfo)arg;
            if (pollStop(imPic)) return;
            Bitmap b = new Bitmap(imPic.path);
            if (pollStop(imPic)) {
                cancelLoad(imPic);
                b.Dispose(); return;
            }
            if (imPic.type != imType.Fullsize) {
                Size szSize = Size.Empty;
                if (imPic.type == imType.Resized) szSize = szShrink;
                if (imPic.type == imType.Thumbnail) szSize = szThumb;
                Bitmap re = cim.Resize(b, szSize, true, false);
                b.Dispose(); b = re;
            }
            for (int a = 0; a < im.Count; a++) {
                imInfo tmp = (imInfo)im[a];
                if (tmp.hash == imPic.hash &&
                    tmp.type == imPic.type) {
                    im[a] = new imInfo(b,
                        imPic.type, imPic.hash,
                        imPic.path, imPic.gen);
                    ready.Add(imPic.hash);
                    //if (imPic.hash == sPrioHash)
                      //  sPrioHash = "";
                    return;
                }
            }
            b.Dispose();
        }
        private bool pollStop(imInfo lod) {
            do {
                if (bCancel) return true;
                if (sPrioHash != "" &&
                    lod.hash != sPrioHash)
                    return true;

                bool gtfo = true; //assume removed
                for (int a = 0; a < im.Count; a++) {
                    imInfo inf = (imInfo)im[a];
                    bool bType = (lod.type == imType.Any);
                    if (!bType) bType = (inf.type == lod.type);
                    if (bType && lod.hash == inf.hash)
                        gtfo = false; //not removed
                }
                if (gtfo) return true;
                System.Threading.Thread.Sleep(1);
            } while (bPause);
            return false;
        }
        private bool cancelLoad(imInfo imPic) {
            for (int a = 0; a < im.Count; a++) {
                imInfo tmp = (imInfo)im[a];
                if (tmp.hash == imPic.hash &&
                    tmp.type == imPic.type) {
                    im[a] = tmp.GetMeta(imInfo.State.Idle);
                    return true;
                }
            }
            return false;
        }
        private int FindBiggestScreen() {
            long area = 0; int ret = -1;
            for (int a = 0; a < Screen.AllScreens.Length; a++) {
                Size thisSize = Screen.AllScreens[a].Bounds.Size;
                long thisArea = thisSize.Width * thisSize.Height;
                if (thisArea > area) {
                    thisArea = area;
                    ret = a;
                }
            }
            return ret;
        }
    }
    public class ImageData {
        public bool bMod = false;
        public bool bSel = false;
        public bool bDel = false;
        public string sType = "";
        public string sHash = "";
        //public char[] cFlag = new char[] { };
        public string sPath = "";
        //public string sThmb = "";
        public Point ptRes = new Point(-1, -1);
        public long iLen = -1;
        public string sName = "";
        public string sDesc = "";
        public int iRate = -1;
        public string sTGen = "";
        public string sTSrc = "";
        public string sTChr = "";
        public string sTArt = "";
        public string sSrch = "";
        public bool bSelFlip() {
            bSel = !bSel;
            return bSel;
        }
    }
    public class cb {
        public static string IllegalName = "\\/:*?\"<>|";
        public static string IllegalPath = "*?\"<>|";
        public static string[] GetPaths(string sRoot, bool bRecursive) {
            Application.DoEvents();
            sRoot = sRoot.Replace("\\", "/");
            if (!sRoot.EndsWith("/")) sRoot += "/";
            string[] ret = new string[0];
            string[] saFolders, saFiles;
            try {
                saFolders = System.IO.Directory.GetDirectories(sRoot);
                saFiles = System.IO.Directory.GetFiles(sRoot);
            }
            catch {
                saFiles = new string[] { "#ERROR#" };
                saFolders = new string[] { };
            }
            ret = saFiles;
            //for (int a = 0; a < ret.Length; a++)
            //{
            //    ret[a] = ret[a].Replace("\\", "/");
            //}
            for (int a = 0; a < saFolders.Length; a++) {
                if (bRecursive) {
                    string[] saAppend = GetPaths(saFolders[a], bRecursive);
                    string[] saOldRet = ret;
                    ret = new string[saOldRet.Length + saAppend.Length];
                    saOldRet.CopyTo(ret, 0);
                    saAppend.CopyTo(ret, saOldRet.Length);
                }
            }
            return ret;
        }
        public static string MD5File(string sFile) {
            System.IO.FileStream fs = new System.IO.FileStream(
                sFile, System.IO.FileMode.Open, System.IO.FileAccess.Read);
            byte[] bFile = new byte[fs.Length];
            fs.Read(bFile, 0, (int)fs.Length);
            fs.Close(); fs.Dispose();
            return MD5(bFile);
        }
        public static string MD5(byte[] bData) {
            System.Security.Cryptography.MD5CryptoServiceProvider crypt =
                new System.Security.Cryptography.MD5CryptoServiceProvider();
            byte[] bRet = crypt.ComputeHash(bData);
            string ret = "";
            for (int a = 0; a < bRet.Length; a++) {
                //string wat = bRet[a].ToString("x2");
                //while (wat.Length < 2) wat = "0" + wat;
                ret += bRet[a].ToString("x2"); //wat;
            }
            crypt.Clear(); return ret;
        }
        public static bool bCmpBytes(byte[] b1, int iOfs, byte[] b2) {
            if (b1.Length < b2.Length + iOfs) return false;
            for (int a = 0; a < b2.Length; a++)
                if (b1[a + iOfs] != b2[a]) return false;
            return true;
        }
        public static bool ContainsOnly(string str, string vl) {
            for (int a = 0; a < str.Length; a++)
                if (!vl.Contains("" + str[a])) return false;
            return true;
        }
        public static bool ContainsAny(string str, string vl) {
            for (int a = 0; a < vl.Length; a++)
                if (str.Contains("" + vl[a])) return true;
            return false;
        }
        public static string[] Split(string a, string b) {
            return a.Split(new string[] { b },
                StringSplitOptions.None);
        }
        public static bool Reg_DoesExist(string regPath) {
            // This function returns false and makes regkey if not exist.
            try {
                Microsoft.Win32.RegistryKey key = Microsoft.Win32.Registry.CurrentUser;
                long wat = key.OpenSubKey(regPath, true).SubKeyCount; return true;
            }
            catch {
                Microsoft.Win32.RegistryKey key = Microsoft.Win32.Registry.CurrentUser;
                key.CreateSubKey(regPath); return false;
            }
        }
        public static string Reg_Access(string sKey, string sVal) {
            // Read or write to default registry path
            string regPath = "Software\\Praetox Technologies\\" + Application.ProductName;
            Microsoft.Win32.RegistryKey rKey = Microsoft.Win32.Registry.CurrentUser;
            Reg_DoesExist(regPath); rKey = rKey.OpenSubKey(regPath, true);
            if (sVal != "") rKey.SetValue(sKey, sVal);
            string sRet = "";
            try {
                sRet = rKey.GetValue(sKey).ToString();
            }
            catch { }
            rKey.Close(); return sRet;
        }
    }
    public class cim {
        public static byte[] ito1b(Bitmap bSrc) {
            try {
                unsafe {
                    int pxSize = 4;
                    int iW = bSrc.Width; int iH = bSrc.Height;
                    byte[] bPic = new byte[iW * iH * pxSize];

                    System.Drawing.Imaging.BitmapData bData =
                        bSrc.LockBits(new Rectangle(0, 0, iW, iH),
                        System.Drawing.Imaging.ImageLockMode.ReadOnly,
                        System.Drawing.Imaging.PixelFormat.Format32bppArgb);

                    int iStride = bData.Stride;
                    IntPtr pScan0 = bData.Scan0;

                    for (int y = 0; y < iH; y++) {
                        byte* pRow = (byte*)pScan0 + (y * iStride);
                        for (int x = 0; x < iW; x++) {
                            int iLoc = ((y * iW) + x) * pxSize;
                            bPic[iLoc + 0] = pRow[x * pxSize + 3]; //A
                            bPic[iLoc + 1] = pRow[x * pxSize + 2]; //R
                            bPic[iLoc + 2] = pRow[x * pxSize + 1]; //G
                            bPic[iLoc + 3] = pRow[x * pxSize + 0]; //B
                        }
                    }

                    bSrc.UnlockBits(bData);
                    byte[] bResX = BitConverter.GetBytes(iW);
                    byte[] bResY = BitConverter.GetBytes(iH);
                    Array.Reverse(bResX); Array.Reverse(bResY);

                    byte[] bRet = new byte[bPic.Length + bResX.Length + bResY.Length];
                    bResX.CopyTo(bRet, 0);
                    bResY.CopyTo(bRet, bResX.Length);
                    bPic.CopyTo(bRet, bResX.Length + bResY.Length);
                    return bRet;
                }
            }
            catch { return new byte[0]; }
        }
        public static byte[, ,] ito3b(Bitmap bSrc) {
            try {
                unsafe {
                    int pxSize = 4;
                    int iW = bSrc.Width; int iH = bSrc.Height;
                    byte[, ,] ret = new byte[iW, iH, 4];

                    System.Drawing.Imaging.BitmapData bData =
                        bSrc.LockBits(new Rectangle(0, 0, iW, iH),
                        System.Drawing.Imaging.ImageLockMode.ReadOnly,
                        System.Drawing.Imaging.PixelFormat.Format32bppArgb);

                    int iStride = bData.Stride;
                    IntPtr pScan0 = bData.Scan0;

                    for (int y = 0; y < iH; y++) {
                        byte* pRow = (byte*)pScan0 + (y * iStride);
                        for (int x = 0; x < iW; x++) {
                            ret[x, y, 0] = pRow[x * pxSize + 3]; //A
                            ret[x, y, 1] = pRow[x * pxSize + 2]; //R
                            ret[x, y, 2] = pRow[x * pxSize + 1]; //G
                            ret[x, y, 3] = pRow[x * pxSize + 0]; //B
                        }
                    }

                    bSrc.UnlockBits(bData);
                    //bSrc.Dispose();
                    return ret;
                }
            }
            catch { return new byte[0, 0, 0]; }
        }
        public static Bitmap btoi(byte[] bPic) {
            try {
                unsafe {
                    byte[] bResX = new byte[4];
                    byte[] bResY = new byte[4];
                    for (int a = 0; a < 4; a++) {
                        bResX[a] = bPic[a + 0];
                        bResY[a] = bPic[a + 4];
                    }
                    Array.Reverse(bResX); Array.Reverse(bResY);
                    int iW = BitConverter.ToInt32(bResX, 0);
                    int iH = BitConverter.ToInt32(bResY, 0);
                    Bitmap ret = new Bitmap(iW, iH);

                    System.Drawing.Imaging.BitmapData bData =
                        ret.LockBits(new Rectangle(0, 0, iW, iH),
                        System.Drawing.Imaging.ImageLockMode.WriteOnly,
                        System.Drawing.Imaging.PixelFormat.Format32bppArgb);

                    int iStride = bData.Stride;
                    IntPtr pScan0 = bData.Scan0;
                    int pxSize = 4;

                    for (int y = 0; y < iH; y++) {
                        byte* pRow = (byte*)pScan0 + (y * iStride);
                        for (int x = 0; x < iW; x++) {
                            int iLoc = ((y * iW) + x) * pxSize;
                            pRow[x * pxSize + 3] = bPic[iLoc + 0]; //A
                            pRow[x * pxSize + 2] = bPic[iLoc + 1]; //R
                            pRow[x * pxSize + 1] = bPic[iLoc + 2]; //G
                            pRow[x * pxSize + 0] = bPic[iLoc + 3]; //B
                        }
                    }

                    ret.UnlockBits(bData);
                    return ret;
                }
            }
            catch { return new Bitmap(1, 1); }
        }
        public static Bitmap btoi(byte[, ,] baPic) {
            try {
                unsafe {
                    int iW = baPic.GetUpperBound(0) + 1;
                    int iH = baPic.GetUpperBound(1) + 1;
                    Bitmap ret = new Bitmap(iW, iH);

                    System.Drawing.Imaging.BitmapData bData =
                        ret.LockBits(new Rectangle(0, 0, iW, iH),
                        System.Drawing.Imaging.ImageLockMode.WriteOnly,
                        System.Drawing.Imaging.PixelFormat.Format32bppArgb);

                    int iStride = bData.Stride;
                    IntPtr pScan0 = bData.Scan0;
                    int pxSize = 4;

                    for (int y = 0; y < iH; y++) {
                        byte* pRow = (byte*)pScan0 + (y * iStride);
                        for (int x = 0; x < iW; x++) {
                            pRow[x * pxSize + 3] = baPic[x, y, 0]; //A
                            pRow[x * pxSize + 2] = baPic[x, y, 1]; //R
                            pRow[x * pxSize + 1] = baPic[x, y, 2]; //G
                            pRow[x * pxSize + 0] = baPic[x, y, 3]; //B
                        }
                    }

                    ret.UnlockBits(bData);
                    return ret;
                }
            }
            catch { return new Bitmap(1, 1); }
        }
        public static byte[] btob(byte[, ,] baPic) {
            int pxSize = 4;
            int iW = baPic.GetUpperBound(0) + 1;
            int iH = baPic.GetUpperBound(1) + 1;
            byte[] bPic = new byte[iW * iH * pxSize];
            for (int y = 0; y < iH; y++)
                for (int x = 0; x < iW; x++)
                    for (int c = 0; c < pxSize; c++)
                        bPic[(((y * iW) + x) * pxSize) + c] =
                            baPic[x, y, c];

            byte[] bResX = BitConverter.GetBytes(iW);
            byte[] bResY = BitConverter.GetBytes(iH);
            Array.Reverse(bResX); Array.Reverse(bResY);

            byte[] bRet = new byte[bPic.Length + bResX.Length + bResY.Length];
            bResX.CopyTo(bRet, 0);
            bResY.CopyTo(bRet, bResX.Length);
            bPic.CopyTo(bRet, bResX.Length + bResY.Length);
            return bRet;
        }
        public static byte[, ,] btob(byte[] bPic) {
            int pxSize = 4;
            byte[] bResX = new byte[4];
            byte[] bResY = new byte[4];
            for (int a = 0; a < 4; a++) {
                bResX[a] = bPic[a + 0];
                bResY[a] = bPic[a + 4];
            }
            Array.Reverse(bResX); Array.Reverse(bResY);
            int iW = BitConverter.ToInt32(bResX, 0);
            int iH = BitConverter.ToInt32(bResY, 0);

            byte[, ,] ret = new byte[iW, iH, pxSize];
            for (int y = 0; y < iH; y++)
                for (int x = 0; x < iW; x++) {
                    int iLoc = ((y * iW) + x) * pxSize;
                    ret[x, y, 0] = bPic[iLoc + 0]; //A
                    ret[x, y, 1] = bPic[iLoc + 1]; //R
                    ret[x, y, 2] = bPic[iLoc + 2]; //G
                    ret[x, y, 3] = bPic[iLoc + 3]; //B
                }
            return ret;
        }

        public static Bitmap Resize(Bitmap b, Size sz, bool asp, bool inc) {
            Point szo = (Point)b.Size;
            if (!inc && asp) {
                if (szo.X < sz.Width &&
                    szo.Y < sz.Height)
                    return b.Clone(new Rectangle
                        (Point.Empty, b.Size),
                        b.PixelFormat);
            }
            if (asp) {
                double asp1 = (double)b.Width / (double)b.Height;
                double asp2 = (double)sz.Width / (double)sz.Height;
                if (asp2 > asp1) sz.Width = (int)Math.Round((double)sz.Height * asp1);
                if (asp2 < asp1) sz.Height = (int)Math.Round((double)sz.Width / asp1);
            }
            Bitmap bRet = new Bitmap(sz.Width, sz.Height);
            using (Graphics g = Graphics.FromImage(bRet)) {
                g.InterpolationMode = System.Drawing.Drawing2D.InterpolationMode.HighQualityBicubic;
                g.PixelOffsetMode = System.Drawing.Drawing2D.PixelOffsetMode.HighQuality;
                g.SmoothingMode = System.Drawing.Drawing2D.SmoothingMode.HighQuality;
                g.DrawImage(b, new Rectangle(0, 0, sz.Width, sz.Height));
            }
            return bRet;
        }
        public static Bitmap LoadMem(string sPath) {
            System.IO.FileStream fs = new System.IO.FileStream(sPath,
                System.IO.FileMode.Open, System.IO.FileAccess.Read);
            Bitmap b = new Bitmap(fs); fs.Close(); fs.Dispose();
            return b;
        }
        public static Bitmap LoadGDI(string sPath) {
            return new Bitmap(sPath);
        }
        public static Bitmap RotateFlip(Bitmap bSrc, int iRot, bool bH, bool bV) {
            RotateFlipType rft = RotateFlip(iRot, bH, bV);
            if (rft != RotateFlipType.RotateNoneFlipNone)
                bSrc.RotateFlip(rft); return bSrc;
        }
        public static Bitmap RotateFlip(Bitmap bSrc, string sOpt) {
            RotateFlipType rft = RotateFlip(sOpt);
            if (rft != RotateFlipType.RotateNoneFlipNone)
                bSrc.RotateFlip(rft); return bSrc;
        }
        public static RotateFlipType RotateFlip(string sOpt) {
            int iRot = 0;
            sOpt = sOpt.ToLower();
            bool bFlipH = sOpt.Contains("h");
            bool bFlipV = sOpt.Contains("v");
            iRot += sOpt.Split('1').Length * 1;
            iRot += sOpt.Split('2').Length * 2;
            iRot += sOpt.Split('3').Length * 3;
            return RotateFlip(iRot, bFlipH, bFlipV);
        }
        public static RotateFlipType RotateFlip(int iRot, bool bH, bool bV) {
            if (bH && bV) {
                iRot += 2;
                bH = false;
                bV = false;
            }
            iRot %= 4;

            if (bH) {
                if (iRot == 0) return RotateFlipType.RotateNoneFlipX;
                if (iRot == 1) return RotateFlipType.Rotate90FlipX;
                if (iRot == 2) return RotateFlipType.Rotate180FlipX;
                if (iRot == 3) return RotateFlipType.Rotate270FlipX;
            }
            if (bV) {
                if (iRot == 0) return RotateFlipType.RotateNoneFlipY;
                if (iRot == 1) return RotateFlipType.Rotate90FlipY;
                if (iRot == 2) return RotateFlipType.Rotate180FlipY;
                if (iRot == 3) return RotateFlipType.Rotate270FlipY;
            }
            if (!bH && !bV) {
                if (iRot == 0) return RotateFlipType.RotateNoneFlipNone;
                if (iRot == 1) return RotateFlipType.Rotate90FlipNone;
                if (iRot == 2) return RotateFlipType.Rotate180FlipNone;
                if (iRot == 3) return RotateFlipType.Rotate270FlipNone;
            }
            return RotateFlipType.RotateNoneFlipNone;
        }
        public static Bitmap Resize(Bitmap bPic, int iX, int iY, bool bKeepAspect, int iScaleMode, bool bEnlargen) {
            if (!bEnlargen) {
                if (iX > bPic.Width) iX = bPic.Width;
                if (iY > bPic.Height) iY = bPic.Height;
            }
            if (iX < 1) iX = 1; if (iY < 1) iY = 1;
            double dRaX = (double)bPic.Width / (double)iX;
            double dRaY = (double)bPic.Height / (double)iY;
            if (bKeepAspect) {
                if (dRaX > dRaY) iY = (int)Math.Round((double)iX / ((double)bPic.Width / (double)bPic.Height));
                if (dRaY > dRaX) iX = (int)Math.Round((double)iY * ((double)bPic.Width / (double)bPic.Height));
            }
            Bitmap bOut = new Bitmap(iX, iY);
            using (Graphics gOut = Graphics.FromImage((Image)bOut)) {
                gOut.SmoothingMode = System.Drawing.Drawing2D.SmoothingMode.AntiAlias;
                if (iScaleMode == 2) gOut.InterpolationMode =
                      System.Drawing.Drawing2D.InterpolationMode.High;
                if (iScaleMode == 3) gOut.InterpolationMode =
                      System.Drawing.Drawing2D.InterpolationMode.NearestNeighbor;
                gOut.PixelOffsetMode = System.Drawing.Drawing2D.PixelOffsetMode.HighQuality;
                gOut.DrawImage(bPic, 0, 0, iX, iY);
            }
            return bOut;
        }
        public static Bitmap Resize(Bitmap bPic, int iX, int iY, bool bKeepAspect, int iScaleMode) {
            return Resize(bPic, iX, iY, bKeepAspect, iScaleMode, false);
        }
    }
    public class SQLite {
        public bool isOpen;
        public string Path;
        public SQLiteConnection db;
        public bool Create(string sPath, bool bOverwrite) {
            if (db != null) { Close(); db.Dispose(); }
            if (System.IO.File.Exists(sPath)) {
                if (!bOverwrite) return false;
                else try { System.IO.File.Delete(sPath); }
                    catch { return false; }
            }
            try { SQLiteConnection.CreateFile(sPath); }
            catch { return false; }
            return Open(sPath);
        }
        public bool Open(string sPath) {
            if (db != null) { Close(); db.Dispose(); }
            if (!System.IO.File.Exists(sPath)) return false;
            try { db = new SQLiteConnection("Data Source=" + sPath); }
            catch { return false; }
            db.Open(); Path = sPath;
            isOpen = true;
            return true;
        }
        public bool Close() {
            if (!isOpen)
                return true;
            db.Close();
            isOpen = false;
            return true;
        }
    }
    public class Database {
        public int rHash = 0;
        public int rPath = 1;
        public int rType = 2;
        public int rFLen = 3;
        public int rXRes = 4;
        public int rYRes = 5;
        public int rImpN = 6;
        public int rName = 7;
        public int rDesc = 8;
        public int rRate = 9;
        public int rTGen = 10;
        public int rTSrc = 11;
        public int rTChr = 12;
        public int rTArt = 13;
        public int dbVer = 0;
        SQLite db = new SQLite();
        public string Path;
        public bool SuppressMaskErrors;
        public bool Open(string sPath) {
            if (!db.Close())
                return false;
            return db.Open(sPath);
        }
        public bool Create(string sPath, bool bOverwrite) {
            if (!db.Close()) return false;
            if (!db.Create(sPath, bOverwrite)) return false;
            using (SQLiteCommand cmd = db.db.CreateCommand()) {
                cmd.CommandText = "CREATE TABLE `Images`(" +
                    "Hash varchar(32) UNIQUE, " + //Pixel-hash
                    "Path text, " + //Absolute path
                    "Type varchar(4), " + //Filetype
                    "fLen unsigned int, " + //Length
                    "xRes int,  " + //Resolution X
                    "yRes int,  " + //Resolution Y
                    "impN int,  " + //Import batch
                    "Name text, " + //Image name
                    "Desc text, " + //Description
                    "Rate int,  " + //Image rating
                    "tGen text, " + //Tags General
                    "tSrc text, " + //Tags Source
                    "tChr text, " + //Tags Character(s)
                    "tArt text, " + //Tags Artist(s)
                    "PRIMARY KEY (hash))";
                cmd.ExecuteNonQuery();
            }
            using (SQLiteCommand cmd = db.db.CreateCommand()) {
                cmd.CommandText = "CREATE TABLE `Struct`(" +
                    "Tab varchar(16), " + //Table
                    "Col varchar(16), " + //Name
                    "Idx int)"; //Location
                cmd.ExecuteNonQuery();
            }
            using (SQLiteTransaction dbTrs = db.db.BeginTransaction()) {
                using (SQLiteCommand cmd = db.db.CreateCommand()) {
                    cmd.CommandText = "INSERT INTO `Struct`" +
                        "(`Tab`,`Col`,`Idx`) " +
                        "VALUES ('Images',?,?)";
                    SQLiteParameter p1 = cmd.CreateParameter();
                    SQLiteParameter p2 = cmd.CreateParameter();
                    cmd.Parameters.Add(p1);
                    cmd.Parameters.Add(p2);
                    p1.Value = "DVer"; p2.Value = dbVer; cmd.ExecuteNonQuery();
                    p1.Value = "Hash"; p2.Value = rHash; cmd.ExecuteNonQuery();
                    p1.Value = "Path"; p2.Value = rPath; cmd.ExecuteNonQuery();
                    p1.Value = "Type"; p2.Value = rType; cmd.ExecuteNonQuery();
                    p1.Value = "fLen"; p2.Value = rFLen; cmd.ExecuteNonQuery();
                    p1.Value = "xRes"; p2.Value = rXRes; cmd.ExecuteNonQuery();
                    p1.Value = "yRes"; p2.Value = rYRes; cmd.ExecuteNonQuery();
                    p1.Value = "ImpN"; p2.Value = rImpN; cmd.ExecuteNonQuery();
                    p1.Value = "Name"; p2.Value = rName; cmd.ExecuteNonQuery();
                    p1.Value = "Desc"; p2.Value = rDesc; cmd.ExecuteNonQuery();
                    p1.Value = "Rate"; p2.Value = rRate; cmd.ExecuteNonQuery();
                    p1.Value = "TGen"; p2.Value = rTGen; cmd.ExecuteNonQuery();
                    p1.Value = "TSrc"; p2.Value = rTSrc; cmd.ExecuteNonQuery();
                    p1.Value = "TChr"; p2.Value = rTChr; cmd.ExecuteNonQuery();
                    p1.Value = "TArt"; p2.Value = rTArt; cmd.ExecuteNonQuery();
                }
                dbTrs.Commit();
            }
            return true;
        }
        public bool Close() {
            return db.Close();
        }
        public bool ParseStruct() {
            bool ret = true;
            int myDVer = -1;
            int myHash = -1;
            int myPath = -1;
            int myType = -1;
            int myFLen = -1;
            int myXRes = -1;
            int myYRes = -1;
            int myImpN = -1;
            int myName = -1;
            int myDesc = -1;
            int myRate = -1;
            int myTGen = -1;
            int myTSrc = -1;
            int myTChr = -1;
            int myTArt = -1;

            using (SQLiteCommand DBc = db.db.CreateCommand()) {
                DBc.CommandText = "SELECT * FROM 'Struct'";
                using (SQLiteDataReader rd = DBc.ExecuteReader()) {
                    while (rd.Read()) {
                        int iVal = rd.GetInt32(2);
                        string sTyp = rd.GetName(1);
                        if (sTyp == "DVer") myDVer = iVal;
                        if (sTyp == "Hash") myHash = iVal;
                        if (sTyp == "Path") myPath = iVal;
                        if (sTyp == "Type") myType = iVal;
                        if (sTyp == "fLen") myFLen = iVal;
                        if (sTyp == "XRes") myXRes = iVal;
                        if (sTyp == "YRes") myYRes = iVal;
                        if (sTyp == "ImpN") myImpN = iVal;
                        if (sTyp == "Name") myName = iVal;
                        if (sTyp == "Desc") myDesc = iVal;
                        if (sTyp == "Rate") myRate = iVal;
                        if (sTyp == "TGen") myTGen = iVal;
                        if (sTyp == "TSrc") myTSrc = iVal;
                        if (sTyp == "TChr") myTChr = iVal;
                        if (sTyp == "TArt") myTArt = iVal;
                    }
                }
            }
            if (myDVer != dbVer) ret = false; dbVer = myDVer;
            if (myHash != rHash) ret = false; rHash = myHash;
            if (myPath != rPath) ret = false; rPath = myPath;
            if (myType != rType) ret = false; rType = myType;
            if (myFLen != rFLen) ret = false; rFLen = myFLen;
            if (myXRes != rXRes) ret = false; rXRes = myXRes;
            if (myYRes != rYRes) ret = false; rYRes = myYRes;
            if (myImpN != rImpN) ret = false; rImpN = myImpN;
            if (myName != rName) ret = false; rName = myName;
            if (myDesc != rDesc) ret = false; rDesc = myDesc;
            if (myRate != rRate) ret = false; rRate = myRate;
            if (myTGen != rTGen) ret = false; rTGen = myTGen;
            if (myTSrc != rTSrc) ret = false; rTSrc = myTSrc;
            if (myTChr != rTChr) ret = false; rTChr = myTChr;
            if (myTArt != rTArt) ret = false; rTArt = myTArt;
            return ret;
        }
        public struct ImportData {
            int Rate;
            public string Path, Name, Desc, tGen, tSrc, tChr, tArt;
            public ImportData(string sPath, string sName, string sDesc, int iRate,
                string stGen, string stSrc, string stChr, string stArt) {
                Path = sPath; Name = sName; Desc = sDesc; Rate = iRate;
                tGen = stGen; tSrc = stSrc; tChr = stChr; tArt = stArt;
            }
        }
        public enum ImportResult { Imported, Exists, Corrupt, Closed }
        public ImportResult[] Import(ImportData[] id) {
            return new ImportResult[0];
        }
        public string PurifyTags(string s) {
            //WALL OF CODE CRITS yOU FOR 9K+
            string[] sa = s.Split(','); //Split to array.
            bool[] add = new bool[sa.Length]; //Add tag?
            for (int a = 0; a < sa.Length; a++) {
                add[a] = true; //Default is add all.
                sa[a] = sa[a].Trim(); //Remove whitespace.
                if (sa[a] == string.Empty) //Tag is blank?
                    add[a] = false; //Do not add blanks.
                else if (sa[a].StartsWith("{") && //aids?
                    sa[a].EndsWith("}") && //aids in a box?
                    cb.ContainsOnly(sa[a].Substring(1, //{n}?
                    sa[a].Length - 2), "!0123456789")) //{!n}?
                    add[a] = false; //Don't add bloat-{n}'s
                else for (int b = 0; b < a; b++) //Iterate:
                        if (sa[a] == sa[b]) //Tag exists?
                            add[a] = false; //Do not re-add.
            }
            System.Text.StringBuilder sb = //Return-val.
                new System.Text.StringBuilder(128);
            for (int a = 0; a < sa.Length; a++) {
                if (add[a]) //Add this as a tag?
                    sb.Append(sa[a] + ", "); //Yes.
            }
            sb.Remove(sb.Length - 2, 2); //Remove last ,
            return sb.ToString(); //Return cleaned tags.
        }
        public ImageData GenerateID(ImageData id,
            Stream fs, string sRoot, string sFNameMask) {
            id.iLen = fs.Length;
            id.sName = id.sName.Trim();
            id.sDesc = id.sDesc.Trim();
            id.iRate = Math.Min(9, id.iRate); //R<=9
            id.iRate = Math.Max(0, id.iRate); //R>=0
            id.sType = id.sPath.Substring( //Find extension
                id.sPath.LastIndexOf(".") + 1); //:awesome:
            if (id.sType.Length > 4) //No extension?
                id.sType = "jpg"; //Fallback to jpg.
            if (!sRoot.EndsWith("\\")) //No slash?
                sRoot += "\\"; //Add a final slash.
            if (!id.sPath.StartsWith(sRoot)) //Oh no.
                throw new Exception("Eeh~? " +
                    "Picture not a child of root path?" + "\r\n" +
                    "Oh god what is this I don't even");
            string sLocal = id.sPath.Substring(sRoot.Length);
            Bitmap bm = new Bitmap(fs); //Make pic
            id.ptRes = (Point)bm.Size; //Get resolution
            byte[] bmba = cim.ito1b(bm); //Cast to byte[]
            id.sHash = cb.MD5(bmba); //Get pixel-MD5
            bm.Dispose(); //You are useless. Go die.

            string[] saPath = new string[0];
            if (sLocal.Contains("\\")) saPath = sLocal.
                Substring(0, sLocal.LastIndexOf("\\")).Split('\\');
            for (int a = 0; a < saPath.Length; a++) { // <iterate>
                id.sTGen = id.sTGen.Replace("{" + (a + 1) + "}", saPath[a]);
                id.sTSrc = id.sTSrc.Replace("{" + (a + 1) + "}", saPath[a]);
                id.sTChr = id.sTChr.Replace("{" + (a + 1) + "}", saPath[a]);
                id.sTArt = id.sTArt.Replace("{" + (a + 1) + "}", saPath[a]);
            } // </iterate>

            if (sFNameMask != "") { //Namemask up that motherfucker
                bool buggered = false; //...well, not yet atleast.
                string sFName = id.sPath.Substring(id.sPath.LastIndexOf("\\") + 1);
                string[] sFNs = cb.Split(sFNameMask, "{!}"); //{!} delimits a tag
                string[] sFNo = new string[sFNs.Length - 1]; //Number of actual
                //tags is one less than the amount of bloat (tousen desho?)
                for (int a = 0; a < sFNs.Length - 1; a++) {
                    if (!sFName.StartsWith(sFNs[a])) {
                        buggered = true; break;
                    }
                    sFName = sFName.Substring(sFNs[a].Length);
                    int iNext = sFName.IndexOf(sFNs[a + 1]);
                    if (iNext == -1) {
                        buggered = true; break;
                    }
                    sFNo[a] = sFName.Substring(0, iNext);
                    sFName = sFName.Substring(iNext);
                }
                if (buggered) {
                    if (!SuppressMaskErrors)
                        if (DialogResult.Yes == MessageBox.Show
                            ("You have an error in your fnMask syntax." + "\r\n\r\n" +
                            "Your mask:  " + sFNameMask + "\r\n" +
                            "Mismatch:  " + sFName + "\r\n" +
                            "File path:  " + id.sPath + "\r\n\r\n" +
                            "Do you wish to suppress this message? Y/N",
                            "OH FUCK.", MessageBoxButtons.YesNo, MessageBoxIcon.Exclamation))
                            throw new Exception("loldongs");
                }
                else {
                    for (int a = 0; a < sFNo.Length; a++) { // <iterate>
                        id.sTGen = id.sTGen.Replace("{!" + (a + 1) + "}", sFNo[a]);
                        id.sTSrc = id.sTSrc.Replace("{!" + (a + 1) + "}", sFNo[a]);
                        id.sTChr = id.sTChr.Replace("{!" + (a + 1) + "}", sFNo[a]);
                        id.sTArt = id.sTArt.Replace("{!" + (a + 1) + "}", sFNo[a]);
                    } // </iterate>
                }
            }

            id.sTGen = PurifyTags(id.sTGen); //Noone.
            id.sTSrc = PurifyTags(id.sTSrc); //Wants.
            id.sTChr = PurifyTags(id.sTChr); //Effin.
            id.sTArt = PurifyTags(id.sTArt); //Dupes.
            return id; //FUCK YEAH.
        }
    }
    public class Resources {
        static Bitmap[] bres;
        static string[] sres;
        static System.Reflection.Assembly myAsm;
        public static void Prep() {
            myAsm = System.Reflection.
                Assembly.GetExecutingAssembly();
            bres = new Bitmap[]{
                get("Spotify_textfield_dropdown.png"),
                get("Spotify_window_minimize.png"),
                get("Spotify_window_maximize.png"),
                get("Spotify_window_close.png"),
                get("Spotify_window_left.png"),
                get("Spotify_window_right.png"),
                get("Spotify_window_down.png"),
                get("Spotify_window_up.png")};
            sres = new string[]{
                "drop", "min", "max", "close",
                "left", "right", "down", "up"};
        }
        public static Bitmap getr(string var) {
            for (int a = 0; a < sres.Length; a++)
                if (sres[a] == var) return bres[a];
            MessageBox.Show("[CORE-EX] Requested non-existant graphics resource",
                "OH SHI-", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
            return null;
        }
        public static Bitmap getc(string var) {
            //return (Bitmap)getr(var).Clone();
            //Bitmap tmp = getr(var);
            //Bitmap ret = new Bitmap(tmp.Width, tmp.Height);
            //for (int y = 0; y < ret.Height; y++)
            //    for (int x = 0; x < ret.Width; x++)
            //        ret.SetPixel(x, y, tmp.GetPixel(x, y));
            //return ret;
            Bitmap bm = getr(var);
            return bm.Clone(new Rectangle(0, 0, bm.Width - 1, bm.Height - 1),
                System.Drawing.Imaging.PixelFormat.Format32bppArgb);
        }
        private static Bitmap get(string var) {
            using (System.IO.Stream strm = myAsm.
                GetManifestResourceStream(Application.
                ProductName + ".Graphics." + var))
                return new Bitmap(strm);
        }
    }
    public class Bling {
        public static void fadeIn(Form frm) {
            frm.Opacity = 0; frm.Show();
            frm.TopMost = true;
            Application.DoEvents();
            for (double a = 0.1; a < 0.9; a += 0.1) {
                frm.Opacity = a;
                Application.DoEvents();
                System.Threading.Thread.Sleep(10);
            }
            frm.Opacity = 0.95;
            frm.TopMost = false;
        }
        public static void fadeOut(Form frm) {
            for (double a = 0.9; a >= 0; a -= 0.1) {
                frm.Opacity = a;
                Application.DoEvents();
                System.Threading.Thread.Sleep(10);
            }
        }
    }
    public class tmr {
        [System.Runtime.InteropServices.DllImport("Kernel32.dll")]
        static extern bool QueryPerformanceCounter(out double lpPerformanceCount);
        [System.Runtime.InteropServices.DllImport("Kernel32.dll")]
        static extern bool QueryPerformanceFrequency(out double lpFrequency);
        double l1, l2, lf;

        public tmr() {
            if (QueryPerformanceFrequency(out lf) == false)
                throw new Win32Exception();
        }
        public void Start() { l1 = 0; l2 = 0; QueryPerformanceCounter(out l1); }
        public double Stop() { QueryPerformanceCounter(out l2); return (l2 - l1) / lf; }
    }
}

#region Hall of deprecated code
/*public class Thumbnails {
        struct dThumbnail {
            public ulong gen;
            public Bitmap pic;
            public string hash;
            public string path;
            public dThumbnail(Bitmap pic, string hash, string path, ulong gen) {
                this.pic = pic;
                this.hash = hash;
                this.path = path;
                this.gen = gen;
            }
        }
        ArrayList th = new ArrayList();
        ArrayList ready = new ArrayList();
        ulong gen = 0;
        bool bCancel, bFinished;

        public Bitmap Get(string hash) {
            for (int a = 0; a < th.Count; a++) {
                if (((dThumbnail)th[a]).hash == hash)
                    return ((dThumbnail)th[a]).pic;
            }
            return null;
        }
        public void Add(string hash, string path) {
            Set(null, hash, path);
        }
        public void Set(Bitmap pic, string hash, string path) {
            for (int a = 0; a < th.Count; a++)
                if (((dThumbnail)th[a])
                    .hash == hash) return;
            th.Add(new dThumbnail(pic, hash, path, gen));
        }
        public void Del(string hash) {
            for (int a = 0; a < th.Count; a++) {
                if (((dThumbnail)th[a]).hash == hash) {
                    if (((dThumbnail)th[a]).pic != null)
                        ((dThumbnail)th[a]).pic.Dispose();
                    th.RemoveAt(a); return;
                }
            }
        }
        public void Flush() {
            for (int a = 0; a < th.Count; a++)
                if (((dThumbnail)th[a]).pic != null)
                    ((dThumbnail)th[a]).pic.Dispose();
            th.Clear();
        }
        public string[] Ready() {
            string[] ret = new string[ready.Count];
            for (int a = 0; a < ret.Length; a++)
                ret[a] = (string)ready[a];
            ready.RemoveRange(0, ret.Length);
            return ret;
        }
        public void Load(Point ptSize, int iThreads) {
            bFinished = false;
            bool[] bLoad = new bool[th.Count];
            for (int a = 0; a < th.Count; a++) {
                if (((dThumbnail)th[a]).pic == null)
                    bLoad[a] = true;
            }
            System.Threading.Thread thr = new System.Threading.Thread(
                new System.Threading.ParameterizedThreadStart(doLoad));
            thr.IsBackground = true;
            thr.Start(new object[] { ptSize, iThreads, bLoad });
        }
        private void doLoad(object arg) {
            object[] args = (object[])arg;
            Point ptSize = (Point)args[0];
            int iThreads = (int)args[1];
            bool[] bLoad = (bool[])args[2];

            System.Threading.Thread[] thr = new
                System.Threading.Thread[iThreads];
            for (int a = 0; a < bLoad.Length; a++) {
                bool bSent = false;
                if (!bLoad[a]) bSent = true;
                while (!bSent) {
                    if (bCancel) break;
                    for (int b = 0; b < thr.Length; b++) {
                        if (thr[b] == null || thr[b].ThreadState ==
                            System.Threading.ThreadState.Stopped) {
                            thr[b] = new System.Threading.Thread(new
                                System.Threading.ParameterizedThreadStart(doLoadTh));
                            thr[b].IsBackground = true;
                            thr[b].Start(new object[] { ptSize, a });
                            bSent = true; break;
                        }
                    }
                    System.Threading.Thread.Sleep(1);
                }
            }
            bFinished = true;
        }
        private void doLoadTh(object arg) {
            object[] args = (object[])arg;
            Point ptSize = (Point)args[0];
            int iPic = (int)args[1];
            string sHash = ((dThumbnail)th[iPic]).hash;
            string sPath = ((dThumbnail)th[iPic]).path;
            Bitmap b = new Bitmap(sPath);
            th[iPic] = new dThumbnail(
                cb.bmResize(b, ptSize, true),
                sHash, sPath, gen);
            ready.Add(sHash);
            b.Dispose();
        }
    }*/

/* Old loader thread manager
 *             bool[] bLoad = (bool[])arg;
            for (int a = 0; a < bLoad.Length; a++) {
                bool bSent = false;
                if (!bLoad[a]) bSent = true;
                while (!bSent) {
                    for (int b = 0; b < thr.Length; b++) {
                        if (thr[b] == null || thr[b].ThreadState ==
                            System.Threading.ThreadState.Stopped) {
                            thr[b] = new System.Threading.Thread(new
                                System.Threading.ParameterizedThreadStart(doLoadTh));
                            thr[b].IsBackground = true;
                            thr[b].Start(a);
                            bSent = true;
                            break;
                        }
                    }
                    System.Threading.Thread.Sleep(1);
                }
            }
            bBusy = false;*/

/* pImgDB importer code
 *         /*public ImportResult[] Import(string[] sPath, string[] sInfo, int iemTags, bool bMove, bool bImportIsLocal) {
            //sInfo: sImageName, sImageComment, sImageRating,
            //sTagsGeneral, sTagsSource, sTagsChars, sTagsArtists
            //
            //iemTag: 0-ignore  1-before  2-after  3-repOne  4-repAll

            ImportResult[] ret = new ImportResult[sPath.Length];
            for (int a = 0; a < ret.Length; a++)
                if (db.isOpen) ret[a] = ImportResult.Imported;
                else ret[a] = ImportResult.Closed;
            if (!db.isOpen) return ret;
            using (SQLiteTransaction dbTrs = db.db.BeginTransaction()) {
                using (SQLiteCommand DBc = db.db.CreateCommand()) {
                    DBc.CommandText = "INSERT INTO 'images' (" +
                        "hash, thmb, type, flen, xres, yres, name, path, flag, " +
                        "cmnt, rate, tgen, tsrc, tchr, tart) " +
                        "VALUES (?, ?, ?, ?, ?, ?, ?, ?, ?, ?, ?, ?, ?, ?, ?)";
                    SQLiteParameter[] dbParam = new SQLiteParameter[15];
                    for (int a = 0; a < dbParam.Length; a++) {
                        dbParam[a] = DBc.CreateParameter();
                        DBc.Parameters.Add(dbParam[a]);
                    }
                    for (int a = 0; a < sPath.Length; a++) {
                        string[] sMyInfo = (string[])sInfo.Clone();
                        sPath[a] = sPath[a].Replace("\\", "/");

                        //  Read embedded tags
                        string[] semTags = new string[0];
                        //if (iemTags != 0) semTags = emTags.TagsRead(sPath[a]);

                        //  Determine full replace
                        bool bemTagsOverride = false;
                        if (iemTags == 4)
                            for (int b = 0; b < semTags.Length; b++)
                                if (semTags[b] != "") bemTagsOverride = true;

                        //  Do information replacing
                        for (int b = 0; b < semTags.Length; b++) {
                            int iAdd = 0; if (b > 1) iAdd++; //Array skew (rating)
                            if (semTags[b] != "") {
                                if (b < 2) //Don't decimate non-tags (name or comment)
                                {
                                    if (iemTags == 1) sMyInfo[b] = semTags[b]; //Override (before)
                                } else //We hit a tag-based bit of information
                                {
                                    if (iemTags == 1) sMyInfo[b + iAdd] = semTags[b] + ", " + sMyInfo[b + iAdd]; //Before
                                    if (iemTags == 2) sMyInfo[b + iAdd] = sMyInfo[b + iAdd] + ", " + semTags[b]; //After
                                }
                                if (iemTags == 3) sMyInfo[b + iAdd] = semTags[b]; //Substitute (replace-one)
                            }
                            if (bemTagsOverride) sMyInfo[b + iAdd] = semTags[b]; //Override (replace-all)
                        }

                        ImageData tID = GenerateID(sPath[a], sMyInfo, bImportIsLocal, true);
                        if (tID.sType == "nil") ret[a] = ImportResult.Corrupt;
                        //if (Contains(tID.sHash, false).sHash!="") ret[a] = 2;
                        //string sNFPath = cb.sAppPath + DB.Path + tID.sPath;
                        if (ret[a] == 0 && !bImportIsLocal) {
                            if (System.IO.File.Exists(sNFPath)) {
                                ret[a] = ImportResult.Exists;
                            }
                        }
                        if (ret[a] == 0) {
                            dbParam[rHash].Value = tID.sHash;
                            dbParam[rType].Value = tID.sType;
                            dbParam[rFLen].Value = tID.iLen;
                            dbParam[rXRes].Value = tID.ptRes.X;
                            dbParam[rYRes].Value = tID.ptRes.Y;
                            dbParam[rName].Value = tID.sName;
                            dbParam[rPath].Value = tID.sPath;
                            dbParam[rDesc].Value = tID.sCmnt;
                            dbParam[rRate].Value = tID.iRate;
                            dbParam[rTGen].Value = tID.sTGen;
                            dbParam[rTSrc].Value = tID.sTSrc;
                            dbParam[rTChr].Value = tID.sTChr;
                            dbParam[rTArt].Value = tID.sTArt;
                            try {
                                DBc.ExecuteNonQuery();
                            } catch (Exception ex) {
                                ret[a] = ImportResult.Exists;
                            }
                            if (!bImportIsLocal && ret[a] == 0) {
                                if (bMove) {
                                    try {
                                        System.IO.File.Move(sPath[a], sNFPath);
                                    } catch {
                                        System.IO.File.Copy(sPath[a], sNFPath);
                                    }
                                } else {
                                    System.IO.File.Copy(sPath[a], sNFPath);
                                }
                            }
                        }
                    }
                }
                dbTrs.Commit();
            }
            return ret;
        }
        public static ImageData GenerateID(string sPath,
    string[] sInfo, bool bImportIsLocal, bool bCheckFBind) {
            //Bitmap bmTmp = null;
            string sName = sInfo[0];
            string sComment = sInfo[1];
            string sRating = sInfo[2];
            string sTGeneral = sInfo[3];
            string sTSource = sInfo[4];
            string sTChars = sInfo[5];
            string sTArtists = sInfo[6];

            sName = sName.Trim(' ');
            sComment = sComment.Trim(' ');
            sRating = sRating.Trim(' ');
            sTGeneral = sTGeneral.Trim(' ', ',');
            sTSource = sTSource.Trim(' ', ',');
            sTChars = sTChars.Trim(' ', ',');
            sTArtists = sTArtists.Trim(' ', ',');
            /*string[] saTGeneral = sTGeneral.Split(','); sTGeneral = "";
            for (int i = 0; i < saTGeneral.Length; i++)
            {
                saTGeneral[i] = saTGeneral[i].Trim(' ');
                sTGeneral += saTGeneral[i] + ",";
            }
            sTGeneral = sTGeneral.Trim(',');
            string[] saTSource = sTSource.Split(','); sTSource = "";
            for (int i = 0; i < saTSource.Length; i++)
            {
                saTSource[i] = saTSource[i].Trim(' ');
                sTSource += saTSource[i] + ",";
            }
            sTSource = sTSource.Trim(',');
            string[] saTChars = sTChars.Split(','); sTChars = "";
            for (int i = 0; i < saTChars.Length; i++)
            {
                saTChars[i] = saTChars[i].Trim(' ');
                sTChars += saTChars[i] + ",";
            }
            sTChars = sTChars.Trim(',');
            string[] saTArtists = sTArtists.Split(','); sTArtists = "";
            for (int i = 0; i < saTArtists.Length; i++)
            {
                saTArtists[i] = saTArtists[i].Trim(' ');
                sTArtists += saTArtists[i] + ",";
            }
            sTArtists = sTArtists.Trim(',');* /

            ImageData ret = new ImageData();
            string[] saE = sPath.Split('/');
            string sFName = "";
            sFName = saE[saE.Length - 1];
            sFName = sFName.Substring(0, sFName.LastIndexOf("."));

            ret.bSel = false;
            ret.sHash = "";
            //ret.sType = saE[saE.Length - 1].Substring(sFName.Length + 1).ToLower();
            ret.ptRes = new Point(0, 0);
            ret.iLen = -1;

            try {
                //MessageBox.Show("pre");
                int iThP = 8; //8*8*3 = 192b
                int iSW = 0, iSH = 0;
                byte[] bST = null;
                using (Bitmap bmTmp = im.LoadMem(sPath)) {
                    ret.ptRes = (Point)bmTmp.Size;
                    iSW = ret.ptRes.X;
                    iSH = ret.ptRes.Y;
                    bST = im.ito1b(bmTmp);
                    ret.sHash = cb.MD5(bST);

                    if (iThumbBusy != 2) {
                        iThumbBusy = 1;
                        if (bmThumb != null) bmThumb.Dispose();
                        bmThumb = (Bitmap)bmTmp.Clone();
                        iThumbBusy = 0; iThumbIndex++;
                    }
                }
                //MessageBox.Show("post");

                /*Bitmap bmSrc = new Bitmap(iSW, iSH);
                using (Graphics gOut = Graphics.FromImage((Image)bmSrc))
                {
                    //ITT: CRUDE HAX
                    gOut.InterpolationMode = System.Drawing.Drawing2D.InterpolationMode.NearestNeighbor;
                    gOut.SmoothingMode = System.Drawing.Drawing2D.SmoothingMode.None;
                    gOut.DrawImage(bmTmp, 0, 0, iSW, iSH);
                }
                System.IO.MemoryStream ms = new System.IO.MemoryStream();
                bmSrc.Save(ms, System.Drawing.Imaging.ImageFormat.Bmp);
                byte[] bST = ms.GetBuffer(); ms.Close(); ms.Dispose();
                ret.sHash = MD5(bST);*/
/*byte[, ,] bSC = new byte[iSW, iSH, 3];
int ibSTLoc = 54;
for (int y = 0; y < iSH; y++)
    for (int x = 0; x < iSW; x++)
    {
        if (bST[ibSTLoc + 2] == 0) bST[ibSTLoc + 2] = 1; //char 0
        if (bST[ibSTLoc + 1] == 0) bST[ibSTLoc + 1] = 1; //should
        if (bST[ibSTLoc + 0] == 0) bST[ibSTLoc + 0] = 1; //an hero
        bSC[x, iSH - y - 1, 0] = bST[ibSTLoc + 2];
        bSC[x, iSH - y - 1, 1] = bST[ibSTLoc + 1];
        bSC[x, iSH - y - 1, 2] = bST[ibSTLoc + 0];
        ibSTLoc += 4;
    }* /

int iPnC = 0;
int iPnW = (int)Math.Floor((double)iSW / (double)iThP);
int iPnH = (int)Math.Floor((double)iSH / (double)iThP);
int iPnWs = (int)Math.Floor((double)(iSW - (iThP * iPnW)) / 2);
int iPnHs = (int)Math.Floor((double)(iSH - (iThP * iPnH)) / 2);
int[,] iaPnCols = new int[(iThP * iThP), 3];
for (int pnY = 0; pnY < iThP; pnY++)
    for (int pnX = 0; pnX < iThP; pnX++) {
        for (int y = pnY * iPnH; y < (pnY + 1) * iPnH; y++)
            for (int x = pnX * iPnW; x < (pnX + 1) * iPnW; x++) {
                /*iaPnCols[iPnC, 0] += bSC[x + iPnWs, y + iPnHs, 0];
                iaPnCols[iPnC, 1] += bSC[x + iPnWs, y + iPnHs, 1];
                iaPnCols[iPnC, 2] += bSC[x + iPnWs, y + iPnHs, 2];* /
                //int iSL = (((iSH - (y + iPnHs)) * iSW - iSW + (x + iPnWs)) * 4) + 54;
                int iSL = ((((y + iPnHs) * iSW) + (x + iPnWs)) * 4) + 8;
                iaPnCols[iPnC, 0] += bST[iSL + 1];
                iaPnCols[iPnC, 1] += bST[iSL + 2];
                iaPnCols[iPnC, 2] += bST[iSL + 3];
            }
        iaPnCols[iPnC, 0] /= iPnW * iPnH;
        iaPnCols[iPnC, 1] /= iPnW * iPnH;
        iaPnCols[iPnC, 2] /= iPnW * iPnH;
        iPnC++;
    }

/*iPnC = 0;
Bitmap bTest = new Bitmap(iThP, iThP);
for (int y = 0; y < iThP; y++)
    for (int x = 0; x < iThP; x++)
    {
        bTest.SetPixel(x, y, Color.FromArgb(
            iaPnCols[iPnC, 0],
            iaPnCols[iPnC, 1],
            iaPnCols[iPnC, 2]));
        iPnC++;
    }
Clipboard.Clear(); Clipboard.SetImage(bTest);
Application.DoEvents(); MessageBox.Show("wat");* /

char[] bTC = new char[iThP * iThP * 3];
for (int a = 0; a < iThP * iThP; a++) {
    bTC[(a * 3) + 0] = (char)iaPnCols[a, 0];
    bTC[(a * 3) + 1] = (char)iaPnCols[a, 1];
    bTC[(a * 3) + 2] = (char)iaPnCols[a, 2];
}
for (int a = 0; a < bTC.Length; a++) {
    if (bTC[a] == '\0') bTC[a] = (char)((int)'\0' + 1); //csharp string terminate
    if (bTC[a] == '\'') bTC[a] = (char)((int)'\'' + 1); //sqlite string terminate
}
ret.sThmb = new string(bTC);

// Test to see whether image is chopped properly
/*iPnC = 0;
Bitmap bTCBT = (Bitmap)bmSrc.Clone();
for (int pnY = 0; pnY < iThP; pnY++)
    for (int pnX = 0; pnX < iThP; pnX++)
    {
        for (int y = pnY * iPnH; y < (pnY + 1) * iPnH; y++)
            for (int x = pnX * iPnW; x < (pnX + 1) * iPnW; x++)
            {
                bTCBT.SetPixel(x + iPnWs, y + iPnHs, Color.FromArgb
                    (iaPnCols[iPnC, 0], iaPnCols[iPnC, 1], iaPnCols[iPnC, 2]));
            }
        iPnC++;
    }
Clipboard.Clear(); Clipboard.SetImage(bTCBT as Image);*/
/*Bitmap bTCB = new Bitmap(iThP, iThP);
for (int y = 0; y < iThP; y++)
    for (int x = 0; x < iThP; x++)
    {
        int i = (y * iThP) + x;
        bTCB.SetPixel(x, y, Color.FromArgb
            (iaPnCols[i, 0], iaPnCols[i, 1], iaPnCols[i, 2]));
    }

dbg.Stop(); dbgt += dbg.Ret;
Clipboard.Clear(); Clipboard.SetImage(bTCB as Image);* /

System.IO.FileStream fs = new System.IO.FileStream(
    sPath, System.IO.FileMode.Open, System.IO.FileAccess.Read);
byte[] bI = new byte[fs.Length];
fs.Read(bI, 0, (int)fs.Length);
fs.Close(); fs.Dispose();

if (bCheckFBind) {
    if (pFBind.isFBind(bI))
        ret.cFlag[DB.flFBind] = '1';
} else ret.cFlag[DB.flFBind] = '?';
ret.iLen = bI.Length;

int iFmtID = emTags.GetType(bI);
if (iFmtID == -1) ret.sType = "bmp";
else ret.sType = emTags.sFmtH[iFmtID];

// OLD CODE
/*Bitmap bmThmb = ResizeBitmap(bmTmp, iThP, iThP, false, 1);
Bitmap bmPrev = (Bitmap)bmTmp.Clone(); bmTmp.Dispose();
Import_pbPreview.Image = bmPrev; Application.DoEvents();
System.IO.MemoryStream ms = new System.IO.MemoryStream();
bmThmb.Save(ms, System.Drawing.Imaging.ImageFormat.Bmp);
//bmThmb.Save("c:\\thumb_dual.bmp");
byte[] bT = ms.GetBuffer(); ms.Close(); ms.Dispose();
char[] bTC = new char[iThP * iThP * 3]; int ibTLoc = 54;
for (int y = 0; y < iThP; y++)
    for (int x = 0; x < iThP; x++)
    {
        int iTL = ((iThP - y) * iThP - iThP + (x)) * 3;
        bTC[iTL + 0] = (char)bT[ibTLoc + 2];
        bTC[iTL + 1] = (char)bT[ibTLoc + 1];
        bTC[iTL + 2] = (char)bT[ibTLoc + 0];
        if (bTC[iTL + 0] == 0) bTC[iTL + 0] = (char)1;
        if (bTC[iTL + 1] == 0) bTC[iTL + 1] = (char)1;
        if (bTC[iTL + 2] == 0) bTC[iTL + 2] = (char)1;
        //int iVal = (int)Math.Round(
        //    (double)bT[ibTLoc + 2] * 0.30 + //lolreverse
        //    (double)bT[ibTLoc + 1] * 0.59 + //lolreverse
        //    (double)bT[ibTLoc + 0] * 0.11); //lolreverse
        //if (iVal == 0) iVal = 1; //Char zero is a bad bitch
        //bTC[(iThP - y) * iThP - iThP + (x)] = //lolreverse
        //    (char)iVal;
        ibTLoc += 4;
    }
ret.sThumb = new string(bTC);
/*Bitmap bTCB = new Bitmap(iThP, iThP);
int iThCol = -3;
for (int y = 0; y < iThP; y++)
    for (int x = 0; x < iThP; x++)
    {
        iThCol+=3;
        bTCB.SetPixel(x, y, Color.FromArgb
            //(bTC[(y * iThP) + x], bTC[(y * iThP) + x], bTC[(y * iThP) + x]));
            (bTC[iThCol+0], bTC[iThCol+1], bTC[iThCol+2]));
    }
bTCB.Save("c:\\thumb_mono1.bmp");
/.*string wut = new string(cTC);
byte[] bTCC = new byte[iThP * iThP];
for (int a = 0; a < bTCC.Length; a++)
    bTCC[a] = (byte)wut[a];
Bitmap bTCBB = new Bitmap(iThP, iThP);
for (int y = 0; y < iThP; y++)
    for (int x = 0; x < iThP; x++)
        bTCBB.SetPixel(x, y, Color.FromArgb
            (bTCC[(y * iThP) + x], bTCC[(y * iThP) + x], bTCC[(y * iThP) + x]));
bTCBB.Save("c:\\thumb_mono2.bmp");
Import_pbPreview.Image = bTCB; Application.DoEvents(); * /
bmThmb.Dispose();

System.IO.FileStream fs = new System.IO.FileStream(
    sPath, System.IO.FileMode.Open, System.IO.FileAccess.Read);
byte[] bI = new byte[fs.Length];
fs.Read(bI, 0, (int)fs.Length);
fs.Close(); fs.Dispose();
ret.iLength = bI.Length;
//int iX = bI[17] + bI[18] * 256 + bI[19] * 256 * 256 + bI[20] * 256 * 256 * 256;
//int iY = bI[21] + bI[22] * 256 + bI[23] * 256 * 256 + bI[24] * 256 * 256 * 256;
ret.ptRes = (Point)Import_pbPreview.Image.Size;
ret.sHash = MD5(bI);
//MessageBox.Show(iX + "x" + iY); * /
} catch {
ret.sType = "nil";
ret.ptRes = new Point(0, 0); //(Point)Import_pbPreview.Image.Size;
}

if (bImportIsLocal) ret.sPath = sPath.Substring(cb.sAppPath.Length + DB.Path.Length);
if (!bImportIsLocal) ret.sPath = ret.sHash + "." + ret.sType;
ret.sPath = ret.sPath.ToLower();

//Image img = im.Load(sPath);
//ret.ptRes = (Point)img.Size;
int iLocal = 0; if (bImportIsLocal)
iLocal = (cb.sAppPath + DB.Path).Split('/').Length - 2;
int iBrack = saE.Length - iLocal - 1; //filename
for (int a = 1; a < iBrack; a++) {
sName = sName.Replace("{" + a + "}", saE[a + iLocal]);
sComment = sComment.Replace("{" + a + "}", saE[a + iLocal]);
sRating = sRating.Replace("{" + a + "}", saE[a + iLocal]);
sTGeneral = sTGeneral.Replace("{" + a + "}", saE[a + iLocal]);
sTSource = sTSource.Replace("{" + a + "}", saE[a + iLocal]);
sTChars = sTChars.Replace("{" + a + "}", saE[a + iLocal]);
sTArtists = sTArtists.Replace("{" + a + "}", saE[a + iLocal]);
}
sName = sName.Replace("{fname}", sFName);
sComment = sComment.Replace("{fname}", sFName);
sRating = sRating.Replace("{fname}", sFName);
sTGeneral = sTGeneral.Replace("{fname}", sFName);
sTSource = sTSource.Replace("{fname}", sFName);
sTChars = sTChars.Replace("{fname}", sFName);
sTArtists = sTArtists.Replace("{fname}", sFName);

int iRating = 0;
if (!Int32.TryParse(sRating,
out iRating)) iRating = -1;
ret.sName = sName;
ret.sCmnt = sComment;
ret.iRate = iRating;
ret.sTGen = cb.RemoveDupes(sTGeneral);
ret.sTSrc = cb.RemoveDupes(sTSource);
ret.sTChr = cb.RemoveDupes(sTChars);
ret.sTArt = cb.RemoveDupes(sTArtists);
return ret;
}*/
#endregion