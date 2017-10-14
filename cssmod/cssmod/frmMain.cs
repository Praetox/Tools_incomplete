using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;

namespace cssmod
{
    public partial class frmMain : Form
    {
        public frmMain()
        {
            InitializeComponent();
        }

        private string css;

        private void cmdStartClip_Click(object sender, EventArgs e)
        {
            css = Clipboard.GetText();
        }

        private void cmdStartFile_Click(object sender, EventArgs e)
        {
            OpenFileDialog ofd = new OpenFileDialog();
            ofd.Filter = "Cascading Style Sheet (*.cdd)|*.css";
            ofd.ShowDialog(); if (ofd.FileName == "") return;
            css = System.IO.File.ReadAllText(ofd.FileName);
        }

        private void cmdStartWeb_Click(object sender, EventArgs e)
        {
            frmGetFromWeb fGFW = new frmGetFromWeb();
            fGFW.ShowDialog(); if (fGFW.SelectedCSS == "") return;
            css = fGFW.SelectedCSS;
        }

        private void cmdChangeColour_Click(object sender, EventArgs e)
        {
            new frmChangeColour(frmChangeColour.FormType.Change).ShowDialog();
        }

        private void cmdSkewColours_Click(object sender, EventArgs e)
        {
            new frmChangeColour(frmChangeColour.FormType.Skew).ShowDialog();
        }

        private void cmdInvertColours_Click(object sender, EventArgs e)
        {

        }
    }
}
