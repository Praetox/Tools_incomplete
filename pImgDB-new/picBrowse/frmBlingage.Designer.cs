namespace picBrowse {
    partial class frmBlingage {
        /// <summary>
        /// Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// Clean up any resources being used.
        /// </summary>
        /// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
        protected override void Dispose(bool disposing) {
            if (disposing && (components != null)) {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Windows Form Designer generated code

        /// <summary>
        /// Required method for Designer support - do not modify
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent() {
            this.components = new System.ComponentModel.Container();
            this.tPosition = new System.Windows.Forms.Timer(this.components);
            this.tSlide = new System.Windows.Forms.Timer(this.components);
            this.SuspendLayout();
            // 
            // tPosition
            // 
            this.tPosition.Enabled = true;
            this.tPosition.Interval = 1;
            this.tPosition.Tick += new System.EventHandler(this.tPosition_Tick);
            // 
            // tSlide
            // 
            this.tSlide.Interval = 5000;
            this.tSlide.Tick += new System.EventHandler(this.tSlide_Tick);
            // 
            // frmBlingage
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(292, 266);
            this.ControlBox = false;
            this.DoubleBuffered = true;
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.None;
            this.Name = "frmBlingage";
            this.ShowIcon = false;
            this.ShowInTaskbar = false;
            this.SizeGripStyle = System.Windows.Forms.SizeGripStyle.Hide;
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "frmBlingage";
            this.Load += new System.EventHandler(this.frmBlingage_Load);
            this.MouseUp += new System.Windows.Forms.MouseEventHandler(this.frmBlingage_MouseUp);
            this.DoubleClick += new System.EventHandler(this.frmBlingage_DoubleClick);
            this.Enter += new System.EventHandler(this.frmBlingage_Enter);
            this.MouseDown += new System.Windows.Forms.MouseEventHandler(this.frmBlingage_MouseDown);
            this.FormClosing += new System.Windows.Forms.FormClosingEventHandler(this.frmBlingage_FormClosing);
            this.MouseMove += new System.Windows.Forms.MouseEventHandler(this.frmBlingage_MouseMove);
            this.KeyDown += new System.Windows.Forms.KeyEventHandler(this.frmBlingage_KeyDown);
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.Timer tPosition;
        private System.Windows.Forms.Timer tSlide;
    }
}