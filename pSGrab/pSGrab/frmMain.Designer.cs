namespace pSGrab
{
    partial class frmMain
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
            this.components = new System.ComponentModel.Container();
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(frmMain));
            this.s2 = new System.Windows.Forms.Label();
            this.s3 = new System.Windows.Forms.Label();
            this.s4 = new System.Windows.Forms.Label();
            this.s5 = new System.Windows.Forms.Label();
            this.s0 = new System.Windows.Forms.Label();
            this.tHide = new System.Windows.Forms.Timer(this.components);
            this.s1 = new System.Windows.Forms.Label();
            this.SuspendLayout();
            // 
            // s2
            // 
            this.s2.BackColor = System.Drawing.Color.Transparent;
            this.s2.Font = new System.Drawing.Font("Verdana", 14.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.s2.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(77)))), ((int)(((byte)(60)))), ((int)(((byte)(44)))));
            this.s2.Location = new System.Drawing.Point(58, 137);
            this.s2.Name = "s2";
            this.s2.Size = new System.Drawing.Size(282, 30);
            this.s2.TabIndex = 0;
            this.s2.Text = "* Parsing papp";
            this.s2.Visible = false;
            // 
            // s3
            // 
            this.s3.BackColor = System.Drawing.Color.Transparent;
            this.s3.Font = new System.Drawing.Font("Verdana", 14.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.s3.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(77)))), ((int)(((byte)(60)))), ((int)(((byte)(44)))));
            this.s3.Location = new System.Drawing.Point(58, 167);
            this.s3.Name = "s3";
            this.s3.Size = new System.Drawing.Size(282, 30);
            this.s3.TabIndex = 1;
            this.s3.Text = "* Applying skin";
            this.s3.Visible = false;
            // 
            // s4
            // 
            this.s4.BackColor = System.Drawing.Color.Transparent;
            this.s4.Font = new System.Drawing.Font("Verdana", 14.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.s4.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(77)))), ((int)(((byte)(60)))), ((int)(((byte)(44)))));
            this.s4.Location = new System.Drawing.Point(58, 197);
            this.s4.Name = "s4";
            this.s4.Size = new System.Drawing.Size(282, 30);
            this.s4.TabIndex = 2;
            this.s4.Text = "* Drawing init.form";
            this.s4.Visible = false;
            // 
            // s5
            // 
            this.s5.BackColor = System.Drawing.Color.Transparent;
            this.s5.Font = new System.Drawing.Font("Verdana", 14.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.s5.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(77)))), ((int)(((byte)(60)))), ((int)(((byte)(44)))));
            this.s5.Location = new System.Drawing.Point(58, 227);
            this.s5.Name = "s5";
            this.s5.Size = new System.Drawing.Size(282, 30);
            this.s5.TabIndex = 3;
            this.s5.Text = "* Holy shit it worked";
            this.s5.Visible = false;
            // 
            // s0
            // 
            this.s0.BackColor = System.Drawing.Color.Transparent;
            this.s0.Font = new System.Drawing.Font("Verdana", 14.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.s0.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(77)))), ((int)(((byte)(60)))), ((int)(((byte)(44)))));
            this.s0.Location = new System.Drawing.Point(58, 77);
            this.s0.Name = "s0";
            this.s0.Size = new System.Drawing.Size(282, 30);
            this.s0.TabIndex = 4;
            this.s0.Text = "* Initializing";
            this.s0.Visible = false;
            // 
            // tHide
            // 
            this.tHide.Tick += new System.EventHandler(this.tHide_Tick);
            // 
            // s1
            // 
            this.s1.BackColor = System.Drawing.Color.Transparent;
            this.s1.Font = new System.Drawing.Font("Verdana", 14.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.s1.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(77)))), ((int)(((byte)(60)))), ((int)(((byte)(44)))));
            this.s1.Location = new System.Drawing.Point(58, 107);
            this.s1.Name = "s1";
            this.s1.Size = new System.Drawing.Size(282, 30);
            this.s1.TabIndex = 5;
            this.s1.Text = "* Tracing env.";
            this.s1.Visible = false;
            // 
            // frmMain
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackColor = System.Drawing.Color.Fuchsia;
            this.BackgroundImage = ((System.Drawing.Image)(resources.GetObject("$this.BackgroundImage")));
            this.ClientSize = new System.Drawing.Size(400, 300);
            this.Controls.Add(this.s1);
            this.Controls.Add(this.s0);
            this.Controls.Add(this.s5);
            this.Controls.Add(this.s4);
            this.Controls.Add(this.s3);
            this.Controls.Add(this.s2);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.None;
            this.Name = "frmMain";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "pAppSkin";
            this.TransparencyKey = System.Drawing.Color.Fuchsia;
            this.Load += new System.EventHandler(this.frmMain_Load);
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.Label s2;
        private System.Windows.Forms.Label s3;
        private System.Windows.Forms.Label s4;
        private System.Windows.Forms.Label s5;
        private System.Windows.Forms.Label s0;
        private System.Windows.Forms.Timer tHide;
        private System.Windows.Forms.Label s1;
    }
}

