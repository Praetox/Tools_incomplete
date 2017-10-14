using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;

namespace picBrowse
{
    public partial class frmImport : Form
    {
        public frmImport(Database db)
        {
            InitializeComponent();
            this.db = db;
        }
        Database db;

        private bool CheckFolderValid() {
            if (!System.IO.Directory.Exists(lbSource.Text)) {
                MessageBox.Show("Please select a valid root directory.",
                    "Invalid input", MessageBoxButtons.OK,
                    MessageBoxIcon.Exclamation); return false;
            }
            return true;
        }
        private void cmStart_Click(object sender, EventArgs e)
        {
            if (!CheckFolderValid()) return;
            cmStart.Enabled = false;
            cmTest.Enabled = false;
            int iFiles = 0;
            string[] sFiles = cb.GetPaths(lbSource.Text, true);
            bool[] bAdd = new bool[sFiles.Length];
            for (int a = 0; a < bAdd.Length; a++) {
                string sFile = sFiles[a].ToLower();
                if (sFile.EndsWith(".jpg") ||
                    sFile.EndsWith(".png") ||
                    sFile.EndsWith(".gif")) {
                    bAdd[a] = true; iFiles++;
                }
            }
            string sDBName = db.Path.Substring(4);
            sDBName = sDBName.Substring
                (0, sDBName.Length - 3);
            MessageBox.Show(
                "Images: " + iFiles + "\r\n" +
                "Database: " + sDBName + "\r\n\r\n" +
                "Are you sure?", "Affirming user input",
                MessageBoxButtons.YesNo,
                MessageBoxIcon.Question);
        }
        private void cmCancel_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void lbSource_Click(object sender, EventArgs e) {
            FolderBrowserDialog fbd = new FolderBrowserDialog();
            if (!lbSource.Text.StartsWith("Click here to"))
                fbd.SelectedPath = lbSource.Text;
            else fbd.SelectedPath = Application.StartupPath;
            fbd.ShowDialog(); string path = fbd.SelectedPath;
            if (path == "") return; lbSource.Text = path;
        }
        private void cmTest_Click(object sender, EventArgs e) {
            if (!CheckFolderValid()) return;
            OpenFileDialog ofd = new OpenFileDialog();
            ofd.InitialDirectory = lbSource.Text;
            ofd.Filter = "Images (*.png, *.jpg, *.gif)|*.png;*.jpg;*.gif";
            ofd.ShowDialog(); string sFile = ofd.FileName;
            if (sFile == string.Empty) return;

            ImageData id = new ImageData();
            id.sPath = sFile;
            id.sName = txName.Text; id.sDesc = txDesc.Text;
            id.sTGen = txTGen.Text; id.sTSrc = txTSrc.Text;
            id.sTChr = txTChr.Text; id.sTArt = txTArt.Text;
            System.IO.FileStream fs = new System.IO.FileStream(id.sPath,
                System.IO.FileMode.Open, System.IO.FileAccess.Read);
            id = db.GenerateID(id, fs, lbSource.Text, txNameMask.Text);
            MessageBox.Show("The following information was generated." + "\r\n\r\n" +
                "Hash: " + id.sHash + "\r\n" +
                "Type: " + id.sType + "\r\n" +
                "Len: " + id.iLen + "\r\n" +
                "Res: " + id.ptRes + "\r\n\r\n" +
                "Name: " + id.sName + "\r\n" +
                "Description: " + id.sDesc + "\r\n" +
                "Rating: " + id.iRate + "\r\n" +
                "Tags General: " + id.sTGen + "\r\n" +
                "Tags Source: " + id.sTSrc + "\r\n" +
                "Tags Chars: " + id.sTChr + "\r\n" +
                "Tags Artist: " + id.sTArt);
            MessageBox.Show("HOLY SHIT IT WORKED FUCK YEAH");
        }
        private void frmImport_FormClosing(object sender, FormClosingEventArgs e) {
            Bling.fadeOut(this);
        }
        private void frmImport_Load(object sender, EventArgs e) {
            Bling.fadeIn(this);
        }
    }
}
