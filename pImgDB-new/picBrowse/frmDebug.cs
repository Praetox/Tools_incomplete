using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;

namespace picBrowse {
    public partial class frmDebug : Form {
        public frmDebug(ImageCollection ic)
        {
            this.ic = ic;
            InitializeComponent();
        }
        ImageCollection ic;

        private void frmDebug_Load(object sender, EventArgs e) {
            Timer t = new Timer();
            t.Tick += delegate(object lol, EventArgs dongs) {
                string str = ic.List(ImageCollection.imType.Any);
                if (str != "") label1.Text = str;
            }; t.Interval = 10; t.Start();
        }
    }
}
