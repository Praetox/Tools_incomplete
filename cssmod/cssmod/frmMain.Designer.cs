namespace cssmod
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
            this.groupBox1 = new System.Windows.Forms.GroupBox();
            this.cmdStartWeb = new System.Windows.Forms.Button();
            this.cmdStartFile = new System.Windows.Forms.Button();
            this.cmdStartClip = new System.Windows.Forms.Button();
            this.groupBox2 = new System.Windows.Forms.GroupBox();
            this.cmdSkewColours = new System.Windows.Forms.Button();
            this.cmdChangeColour = new System.Windows.Forms.Button();
            this.cmdInvertColours = new System.Windows.Forms.Button();
            this.statusStrip1 = new System.Windows.Forms.StatusStrip();
            this.groupBox1.SuspendLayout();
            this.groupBox2.SuspendLayout();
            this.SuspendLayout();
            // 
            // groupBox1
            // 
            this.groupBox1.Controls.Add(this.cmdStartWeb);
            this.groupBox1.Controls.Add(this.cmdStartFile);
            this.groupBox1.Controls.Add(this.cmdStartClip);
            this.groupBox1.Location = new System.Drawing.Point(12, 12);
            this.groupBox1.Margin = new System.Windows.Forms.Padding(3, 3, 3, 16);
            this.groupBox1.Name = "groupBox1";
            this.groupBox1.Size = new System.Drawing.Size(268, 106);
            this.groupBox1.TabIndex = 0;
            this.groupBox1.TabStop = false;
            this.groupBox1.Text = "1. Read original CSS file";
            // 
            // cmdStartWeb
            // 
            this.cmdStartWeb.Location = new System.Drawing.Point(6, 77);
            this.cmdStartWeb.Name = "cmdStartWeb";
            this.cmdStartWeb.Size = new System.Drawing.Size(256, 23);
            this.cmdStartWeb.TabIndex = 2;
            this.cmdStartWeb.Text = "c) Read css code from website";
            this.cmdStartWeb.UseVisualStyleBackColor = true;
            this.cmdStartWeb.Click += new System.EventHandler(this.cmdStartWeb_Click);
            // 
            // cmdStartFile
            // 
            this.cmdStartFile.Location = new System.Drawing.Point(6, 48);
            this.cmdStartFile.Name = "cmdStartFile";
            this.cmdStartFile.Size = new System.Drawing.Size(256, 23);
            this.cmdStartFile.TabIndex = 1;
            this.cmdStartFile.Text = "b) Read a file on local storage";
            this.cmdStartFile.UseVisualStyleBackColor = true;
            this.cmdStartFile.Click += new System.EventHandler(this.cmdStartFile_Click);
            // 
            // cmdStartClip
            // 
            this.cmdStartClip.Location = new System.Drawing.Point(6, 19);
            this.cmdStartClip.Name = "cmdStartClip";
            this.cmdStartClip.Size = new System.Drawing.Size(256, 23);
            this.cmdStartClip.TabIndex = 0;
            this.cmdStartClip.Text = "a) Get text from the clipboard";
            this.cmdStartClip.UseVisualStyleBackColor = true;
            this.cmdStartClip.Click += new System.EventHandler(this.cmdStartClip_Click);
            // 
            // groupBox2
            // 
            this.groupBox2.Controls.Add(this.cmdSkewColours);
            this.groupBox2.Controls.Add(this.cmdChangeColour);
            this.groupBox2.Controls.Add(this.cmdInvertColours);
            this.groupBox2.Location = new System.Drawing.Point(12, 137);
            this.groupBox2.Margin = new System.Windows.Forms.Padding(3, 3, 3, 16);
            this.groupBox2.Name = "groupBox2";
            this.groupBox2.Size = new System.Drawing.Size(268, 106);
            this.groupBox2.TabIndex = 1;
            this.groupBox2.TabStop = false;
            this.groupBox2.Text = "2. Select what to do with it";
            // 
            // cmdSkewColours
            // 
            this.cmdSkewColours.Location = new System.Drawing.Point(6, 48);
            this.cmdSkewColours.Name = "cmdSkewColours";
            this.cmdSkewColours.Size = new System.Drawing.Size(256, 23);
            this.cmdSkewColours.TabIndex = 5;
            this.cmdSkewColours.Text = "b) Skew all colours";
            this.cmdSkewColours.UseVisualStyleBackColor = true;
            this.cmdSkewColours.Click += new System.EventHandler(this.cmdSkewColours_Click);
            // 
            // cmdChangeColour
            // 
            this.cmdChangeColour.Location = new System.Drawing.Point(6, 19);
            this.cmdChangeColour.Name = "cmdChangeColour";
            this.cmdChangeColour.Size = new System.Drawing.Size(256, 23);
            this.cmdChangeColour.TabIndex = 4;
            this.cmdChangeColour.Text = "a) Change a colour";
            this.cmdChangeColour.UseVisualStyleBackColor = true;
            this.cmdChangeColour.Click += new System.EventHandler(this.cmdChangeColour_Click);
            // 
            // cmdInvertColours
            // 
            this.cmdInvertColours.Location = new System.Drawing.Point(6, 77);
            this.cmdInvertColours.Name = "cmdInvertColours";
            this.cmdInvertColours.Size = new System.Drawing.Size(256, 23);
            this.cmdInvertColours.TabIndex = 3;
            this.cmdInvertColours.Text = "c) Invert all colours";
            this.cmdInvertColours.UseVisualStyleBackColor = true;
            this.cmdInvertColours.Click += new System.EventHandler(this.cmdInvertColours_Click);
            // 
            // statusStrip1
            // 
            this.statusStrip1.Location = new System.Drawing.Point(0, 259);
            this.statusStrip1.Name = "statusStrip1";
            this.statusStrip1.Size = new System.Drawing.Size(292, 22);
            this.statusStrip1.TabIndex = 2;
            this.statusStrip1.Text = "statusStrip1";
            // 
            // frmMain
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(292, 281);
            this.Controls.Add(this.statusStrip1);
            this.Controls.Add(this.groupBox2);
            this.Controls.Add(this.groupBox1);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedSingle;
            this.MaximizeBox = false;
            this.Name = "frmMain";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "CssMOD";
            this.groupBox1.ResumeLayout(false);
            this.groupBox2.ResumeLayout(false);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.GroupBox groupBox1;
        private System.Windows.Forms.Button cmdStartWeb;
        private System.Windows.Forms.Button cmdStartFile;
        private System.Windows.Forms.Button cmdStartClip;
        private System.Windows.Forms.GroupBox groupBox2;
        private System.Windows.Forms.Button cmdSkewColours;
        private System.Windows.Forms.Button cmdChangeColour;
        private System.Windows.Forms.Button cmdInvertColours;
        private System.Windows.Forms.StatusStrip statusStrip1;

    }
}

