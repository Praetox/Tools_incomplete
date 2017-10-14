using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;

namespace picBrowse
{
    public partial class frmSidebar : Form
    {
        public frmSidebar(Panel pn, Point ptLoc)
        {
            this.pn = pn;
            this.Controls.Add(pn);
            InitializeComponent();
            ptLoc.Y -= 18;
            this.Location = ptLoc;
        }

        public Panel pn;
        Point ptFormDragOffset;
        Point ptFormResizeOffset;
        public bool bClosed;
        bool bLoaded;

        private void Main_Title_MouseDown(object sender, MouseEventArgs e)
        {
            if (e.Button == MouseButtons.Left)
                ptFormDragOffset = e.Location;
        }
        private void Main_Title_MouseMove(object sender, MouseEventArgs e)
        {
            if (e.Button == MouseButtons.Left)
            {
                Point ptLoc = this.Location;
                ptLoc.X += e.Location.X - ptFormDragOffset.X;
                ptLoc.Y += e.Location.Y - ptFormDragOffset.Y;
                this.Location = ptLoc;
            }
        }
        private void pbResize_MouseDown(object sender, MouseEventArgs e)
        {
            if (e.Button == MouseButtons.Left)
                ptFormResizeOffset = e.Location;
        }
        private void pbResize_MouseMove(object sender, MouseEventArgs e)
        {
            if (e.Button == MouseButtons.Left)
            {
                Size szFrm = this.Size;
                //szFrm.Width += e.X - ptFormResizeOffset.X;
                szFrm.Height += e.Y - ptFormResizeOffset.Y;
                this.Size = szFrm;
            }
        }

        private void frmSidebar_Resize(object sender, EventArgs e)
        {
            if (!bLoaded) return;
            Point ptSize = (Point)this.Size;
            //Main_Title.Width = ptSize.X;
            //pnTop.Width = ptSize.X;
            //pnBtm.Width = ptSize.X;
            pnBtm.Top = ptSize.Y - 17;
            pn.Height = this.Height - 36;
            //Main_Close.Left = ptSize.X - 14;
            //pbResize.Left = ptSize.X - 14;
        }

        private void frmSidebar_Load(object sender, EventArgs e)
        {
            Main_Close.Image = Resources.getr("close");
            this.Height = pn.Height + 36;
            pn.Location = new Point(1, 18);
            this.Show(); Application.DoEvents(); bLoaded = true;
            frmSidebar_Resize(new object(), new EventArgs());
        }

        private void Main_Close_Click(object sender, EventArgs e)
        {
            bClosed = true;
        }

        private void frmSidebar_FormClosing(object sender, FormClosingEventArgs e)
        {
            e.Cancel = true;
            bClosed = true;
        }
    }
}
