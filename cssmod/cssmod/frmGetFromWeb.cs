using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;

namespace cssmod
{
    public partial class frmGetFromWeb : Form
    {
        public frmGetFromWeb()
        {
            InitializeComponent();
        }

        public string SelectedCSS = "";

        private void cmdParseSite_Click(object sender, EventArgs e)
        {
            if (txtCssFile.Text != "")
            {
                lstCssFiles.Items.Add("x. " + txtCssFile.Text + "  (added manually)");
                numerateItems(); return;
            }
            if (txtWebsite.Text != "")
            {
                lblState.Text = "Reading website..."; Application.DoEvents();
                string suri = txtWebsite.Text;
                if (!suri.StartsWith("http://"))
                    suri = "http://" + suri;
                Uri uri = new Uri(suri);
                string s = new System.Net.WebClient().DownloadString(uri.AbsoluteUri);
                string[] link = cb.split(s, "<link");
                for (int a = 0; a < link.Length; a++)
                {
                    link[a] = link[a].Split('>')[0];
                    if (link[a].Contains("rel=\"stylesheet\""))
                    {
                        string typ = "general";
                        if (link[a].Contains("media=\""))
                            typ = cb.split(cb.split(link[a], "media=\"")[1], "\"")[0];
                        string loc = cb.split(cb.split(link[a], "href=\"")[1], "\"")[0];
                        Uri uloc = new Uri(uri, loc);
                        lstCssFiles.Items.Add("x. " + uloc.AbsoluteUri + "  (" + typ + ")");
                    }
                }
                numerateItems();
                lblState.Text = "Ready.";
                return;
            }
            MessageBox.Show("Please enter exactly one of the following:" + "\r\n\r\n" +
                "    (1) The full URL of a website to parse" + "\r\n" +
                "    (2) The full URL to a single CSS file", "You are doing it wrong.",
                MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
        }

        private void cmdConfirmFile_Click(object sender, EventArgs e)
        {
            lblState.Text = "Reading css..."; Application.DoEvents();
            string sPath = lstCssFiles.SelectedItem.ToString();
            sPath = sPath.Substring(sPath.IndexOf(". ") + 2);
            sPath = sPath.Substring(0, sPath.IndexOf("  ("));
            SelectedCSS = new System.Net.WebClient().
                DownloadString(sPath); this.Close();
        }

        private void numerateItems()
        {
            for (int a = 0; a < lstCssFiles.Items.Count; a++)
            {
                string s = lstCssFiles.Items[a].ToString();
                s = s.Substring(s.IndexOf(". ") + 2);
                lstCssFiles.Items[a] = (a+1) + ". " + s;
            }
            if (lstCssFiles.Items.Count > 0)
                lstCssFiles.SelectedIndex = 0;
        }
    }
}
