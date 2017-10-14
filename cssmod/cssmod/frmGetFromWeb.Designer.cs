namespace cssmod
{
    partial class frmGetFromWeb
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
            this.label1 = new System.Windows.Forms.Label();
            this.label2 = new System.Windows.Forms.Label();
            this.txtWebsite = new System.Windows.Forms.TextBox();
            this.txtCssFile = new System.Windows.Forms.TextBox();
            this.cmdParseSite = new System.Windows.Forms.Button();
            this.groupBox2 = new System.Windows.Forms.GroupBox();
            this.lstCssFiles = new System.Windows.Forms.ComboBox();
            this.cmdConfirmFile = new System.Windows.Forms.Button();
            this.statusStrip1 = new System.Windows.Forms.StatusStrip();
            this.lblState = new System.Windows.Forms.ToolStripStatusLabel();
            this.groupBox1.SuspendLayout();
            this.groupBox2.SuspendLayout();
            this.statusStrip1.SuspendLayout();
            this.SuspendLayout();
            // 
            // groupBox1
            // 
            this.groupBox1.Controls.Add(this.cmdParseSite);
            this.groupBox1.Controls.Add(this.txtCssFile);
            this.groupBox1.Controls.Add(this.txtWebsite);
            this.groupBox1.Controls.Add(this.label2);
            this.groupBox1.Controls.Add(this.label1);
            this.groupBox1.Location = new System.Drawing.Point(12, 12);
            this.groupBox1.Margin = new System.Windows.Forms.Padding(3, 3, 3, 16);
            this.groupBox1.Name = "groupBox1";
            this.groupBox1.Size = new System.Drawing.Size(268, 100);
            this.groupBox1.TabIndex = 0;
            this.groupBox1.TabStop = false;
            this.groupBox1.Text = "1. Provide a source of CSS files";
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(6, 22);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(49, 13);
            this.label1.TabIndex = 0;
            this.label1.Text = "Website:";
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(6, 48);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(47, 13);
            this.label2.TabIndex = 1;
            this.label2.Text = "CSS file:";
            // 
            // txtWebsite
            // 
            this.txtWebsite.Location = new System.Drawing.Point(61, 19);
            this.txtWebsite.Name = "txtWebsite";
            this.txtWebsite.Size = new System.Drawing.Size(201, 20);
            this.txtWebsite.TabIndex = 2;
            // 
            // txtCssFile
            // 
            this.txtCssFile.Location = new System.Drawing.Point(61, 45);
            this.txtCssFile.Name = "txtCssFile";
            this.txtCssFile.Size = new System.Drawing.Size(201, 20);
            this.txtCssFile.TabIndex = 3;
            // 
            // cmdParseSite
            // 
            this.cmdParseSite.Location = new System.Drawing.Point(6, 71);
            this.cmdParseSite.Name = "cmdParseSite";
            this.cmdParseSite.Size = new System.Drawing.Size(256, 23);
            this.cmdParseSite.TabIndex = 4;
            this.cmdParseSite.Text = "Choose one of the above, then click here";
            this.cmdParseSite.UseVisualStyleBackColor = true;
            this.cmdParseSite.Click += new System.EventHandler(this.cmdParseSite_Click);
            // 
            // groupBox2
            // 
            this.groupBox2.Controls.Add(this.cmdConfirmFile);
            this.groupBox2.Controls.Add(this.lstCssFiles);
            this.groupBox2.Location = new System.Drawing.Point(12, 131);
            this.groupBox2.Margin = new System.Windows.Forms.Padding(3, 3, 3, 16);
            this.groupBox2.Name = "groupBox2";
            this.groupBox2.Size = new System.Drawing.Size(268, 75);
            this.groupBox2.TabIndex = 1;
            this.groupBox2.TabStop = false;
            this.groupBox2.Text = "2. Choose an available CSS file";
            // 
            // lstCssFiles
            // 
            this.lstCssFiles.FormattingEnabled = true;
            this.lstCssFiles.Location = new System.Drawing.Point(6, 19);
            this.lstCssFiles.Name = "lstCssFiles";
            this.lstCssFiles.Size = new System.Drawing.Size(256, 21);
            this.lstCssFiles.TabIndex = 0;
            // 
            // cmdConfirmFile
            // 
            this.cmdConfirmFile.Location = new System.Drawing.Point(6, 46);
            this.cmdConfirmFile.Name = "cmdConfirmFile";
            this.cmdConfirmFile.Size = new System.Drawing.Size(256, 23);
            this.cmdConfirmFile.TabIndex = 1;
            this.cmdConfirmFile.Text = "Continue with this file";
            this.cmdConfirmFile.UseVisualStyleBackColor = true;
            this.cmdConfirmFile.Click += new System.EventHandler(this.cmdConfirmFile_Click);
            // 
            // statusStrip1
            // 
            this.statusStrip1.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.lblState});
            this.statusStrip1.Location = new System.Drawing.Point(0, 222);
            this.statusStrip1.Name = "statusStrip1";
            this.statusStrip1.Size = new System.Drawing.Size(292, 22);
            this.statusStrip1.SizingGrip = false;
            this.statusStrip1.TabIndex = 2;
            this.statusStrip1.Text = "statusStrip1";
            // 
            // lblState
            // 
            this.lblState.Name = "lblState";
            this.lblState.Size = new System.Drawing.Size(42, 17);
            this.lblState.Text = "Ready.";
            // 
            // frmGetFromWeb
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(292, 244);
            this.Controls.Add(this.statusStrip1);
            this.Controls.Add(this.groupBox2);
            this.Controls.Add(this.groupBox1);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedSingle;
            this.MaximizeBox = false;
            this.Name = "frmGetFromWeb";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "Get CSS from website";
            this.groupBox1.ResumeLayout(false);
            this.groupBox1.PerformLayout();
            this.groupBox2.ResumeLayout(false);
            this.statusStrip1.ResumeLayout(false);
            this.statusStrip1.PerformLayout();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.GroupBox groupBox1;
        private System.Windows.Forms.Button cmdParseSite;
        private System.Windows.Forms.TextBox txtCssFile;
        private System.Windows.Forms.TextBox txtWebsite;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.GroupBox groupBox2;
        private System.Windows.Forms.Button cmdConfirmFile;
        private System.Windows.Forms.ComboBox lstCssFiles;
        private System.Windows.Forms.StatusStrip statusStrip1;
        private System.Windows.Forms.ToolStripStatusLabel lblState;
    }
}