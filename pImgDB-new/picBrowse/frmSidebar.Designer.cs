namespace picBrowse
{
    partial class frmSidebar
    {
        /// <summary>
        /// Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// Clean up any resources being used.
        /// </summary>
        /// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Windows Form Designer generated code

        /// <summary>
        /// Required method for Designer support - do not modify
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(frmSidebar));
            this.pbResize = new System.Windows.Forms.PictureBox();
            this.pnTop = new System.Windows.Forms.Panel();
            this.Main_Close = new System.Windows.Forms.PictureBox();
            this.Main_Title = new System.Windows.Forms.Label();
            this.pnBtm = new System.Windows.Forms.Panel();
            ((System.ComponentModel.ISupportInitialize)(this.pbResize)).BeginInit();
            this.pnTop.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.Main_Close)).BeginInit();
            this.pnBtm.SuspendLayout();
            this.SuspendLayout();
            // 
            // pbResize
            // 
            this.pbResize.Image = ((System.Drawing.Image)(resources.GetObject("pbResize.Image")));
            this.pbResize.Location = new System.Drawing.Point(167, 3);
            this.pbResize.Name = "pbResize";
            this.pbResize.Size = new System.Drawing.Size(11, 11);
            this.pbResize.TabIndex = 3;
            this.pbResize.TabStop = false;
            this.pbResize.MouseMove += new System.Windows.Forms.MouseEventHandler(this.pbResize_MouseMove);
            this.pbResize.MouseDown += new System.Windows.Forms.MouseEventHandler(this.pbResize_MouseDown);
            // 
            // pnTop
            // 
            this.pnTop.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(130)))), ((int)(((byte)(130)))), ((int)(((byte)(130)))));
            this.pnTop.BackgroundImage = ((System.Drawing.Image)(resources.GetObject("pnTop.BackgroundImage")));
            this.pnTop.Controls.Add(this.Main_Close);
            this.pnTop.Controls.Add(this.Main_Title);
            this.pnTop.Location = new System.Drawing.Point(0, 0);
            this.pnTop.Margin = new System.Windows.Forms.Padding(0);
            this.pnTop.Name = "pnTop";
            this.pnTop.Size = new System.Drawing.Size(181, 17);
            this.pnTop.TabIndex = 4;
            // 
            // Main_Close
            // 
            this.Main_Close.BackColor = System.Drawing.Color.Transparent;
            this.Main_Close.Location = new System.Drawing.Point(167, 3);
            this.Main_Close.Margin = new System.Windows.Forms.Padding(4, 3, 4, 4);
            this.Main_Close.Name = "Main_Close";
            this.Main_Close.Size = new System.Drawing.Size(10, 10);
            this.Main_Close.TabIndex = 12;
            this.Main_Close.TabStop = false;
            this.Main_Close.Click += new System.EventHandler(this.Main_Close_Click);
            // 
            // Main_Title
            // 
            this.Main_Title.BackColor = System.Drawing.Color.Transparent;
            this.Main_Title.Font = new System.Drawing.Font("Verdana", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.Main_Title.ForeColor = System.Drawing.Color.Black;
            this.Main_Title.Location = new System.Drawing.Point(0, 2);
            this.Main_Title.Margin = new System.Windows.Forms.Padding(0);
            this.Main_Title.Name = "Main_Title";
            this.Main_Title.Size = new System.Drawing.Size(181, 13);
            this.Main_Title.TabIndex = 13;
            this.Main_Title.Text = "Some sidebar";
            this.Main_Title.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            this.Main_Title.MouseMove += new System.Windows.Forms.MouseEventHandler(this.Main_Title_MouseMove);
            this.Main_Title.MouseDown += new System.Windows.Forms.MouseEventHandler(this.Main_Title_MouseDown);
            // 
            // pnBtm
            // 
            this.pnBtm.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(96)))), ((int)(((byte)(96)))), ((int)(((byte)(96)))));
            this.pnBtm.BackgroundImage = ((System.Drawing.Image)(resources.GetObject("pnBtm.BackgroundImage")));
            this.pnBtm.Controls.Add(this.pbResize);
            this.pnBtm.Location = new System.Drawing.Point(0, 443);
            this.pnBtm.Margin = new System.Windows.Forms.Padding(0);
            this.pnBtm.Name = "pnBtm";
            this.pnBtm.Size = new System.Drawing.Size(181, 17);
            this.pnBtm.TabIndex = 5;
            // 
            // frmSidebar
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(40)))), ((int)(((byte)(40)))), ((int)(((byte)(40)))));
            this.ClientSize = new System.Drawing.Size(181, 460);
            this.Controls.Add(this.pnBtm);
            this.Controls.Add(this.pnTop);
            this.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(208)))), ((int)(((byte)(208)))), ((int)(((byte)(208)))));
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.None;
            this.Name = "frmSidebar";
            this.StartPosition = System.Windows.Forms.FormStartPosition.Manual;
            this.Text = "Some sidebar";
            this.Load += new System.EventHandler(this.frmSidebar_Load);
            this.FormClosing += new System.Windows.Forms.FormClosingEventHandler(this.frmSidebar_FormClosing);
            this.Resize += new System.EventHandler(this.frmSidebar_Resize);
            ((System.ComponentModel.ISupportInitialize)(this.pbResize)).EndInit();
            this.pnTop.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.Main_Close)).EndInit();
            this.pnBtm.ResumeLayout(false);
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.PictureBox pbResize;
        private System.Windows.Forms.Panel pnTop;
        private System.Windows.Forms.PictureBox Main_Close;
        private System.Windows.Forms.Label Main_Title;
        private System.Windows.Forms.Panel pnBtm;
    }
}