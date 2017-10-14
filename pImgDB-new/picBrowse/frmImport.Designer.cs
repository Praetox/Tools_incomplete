namespace picBrowse
{
    partial class frmImport
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(frmImport));
            this.pictureBox1 = new System.Windows.Forms.PictureBox();
            this.label1 = new System.Windows.Forms.Label();
            this.label2 = new System.Windows.Forms.Label();
            this.lbSource = new System.Windows.Forms.Label();
            this.ckXMP = new System.Windows.Forms.CheckBox();
            this.ckpImgDB = new System.Windows.Forms.CheckBox();
            this.ckSuggest = new System.Windows.Forms.CheckBox();
            this.lbSuggest = new System.Windows.Forms.Label();
            this.label3 = new System.Windows.Forms.Label();
            this.label4 = new System.Windows.Forms.Label();
            this.txNameMask = new System.Windows.Forms.TextBox();
            this.txName = new System.Windows.Forms.TextBox();
            this.label5 = new System.Windows.Forms.Label();
            this.txDesc = new System.Windows.Forms.TextBox();
            this.label6 = new System.Windows.Forms.Label();
            this.txTGen = new System.Windows.Forms.TextBox();
            this.label7 = new System.Windows.Forms.Label();
            this.txTSrc = new System.Windows.Forms.TextBox();
            this.label8 = new System.Windows.Forms.Label();
            this.txTChr = new System.Windows.Forms.TextBox();
            this.label9 = new System.Windows.Forms.Label();
            this.txTArt = new System.Windows.Forms.TextBox();
            this.label10 = new System.Windows.Forms.Label();
            this.label11 = new System.Windows.Forms.Label();
            this.label12 = new System.Windows.Forms.Label();
            this.lbProg = new System.Windows.Forms.Label();
            this.cmStart = new System.Windows.Forms.Button();
            this.cmCancel = new System.Windows.Forms.Button();
            this.cmTest = new System.Windows.Forms.Button();
            ((System.ComponentModel.ISupportInitialize)(this.pictureBox1)).BeginInit();
            this.SuspendLayout();
            // 
            // pictureBox1
            // 
            this.pictureBox1.Image = ((System.Drawing.Image)(resources.GetObject("pictureBox1.Image")));
            this.pictureBox1.Location = new System.Drawing.Point(12, 12);
            this.pictureBox1.Name = "pictureBox1";
            this.pictureBox1.Size = new System.Drawing.Size(64, 64);
            this.pictureBox1.TabIndex = 0;
            this.pictureBox1.TabStop = false;
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Font = new System.Drawing.Font("Microsoft Sans Serif", 15.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label1.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(224)))), ((int)(((byte)(224)))), ((int)(((byte)(224)))));
            this.label1.Location = new System.Drawing.Point(82, 12);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(226, 25);
            this.label1.TabIndex = 1;
            this.label1.Text = "PicSys Image Importer";
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.ForeColor = System.Drawing.Color.Silver;
            this.label2.Location = new System.Drawing.Point(82, 37);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(204, 13);
            this.label2.TabIndex = 2;
            this.label2.Text = "This will add new images to the database.";
            // 
            // lbSource
            // 
            this.lbSource.Cursor = System.Windows.Forms.Cursors.Hand;
            this.lbSource.ForeColor = System.Drawing.Color.White;
            this.lbSource.Location = new System.Drawing.Point(82, 63);
            this.lbSource.Name = "lbSource";
            this.lbSource.Size = new System.Drawing.Size(344, 13);
            this.lbSource.TabIndex = 4;
            this.lbSource.Text = "Click here to select a root directory";
            this.lbSource.Click += new System.EventHandler(this.lbSource_Click);
            // 
            // ckXMP
            // 
            this.ckXMP.AutoSize = true;
            this.ckXMP.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(224)))), ((int)(((byte)(224)))), ((int)(((byte)(224)))));
            this.ckXMP.Location = new System.Drawing.Point(432, 12);
            this.ckXMP.Name = "ckXMP";
            this.ckXMP.Size = new System.Drawing.Size(173, 17);
            this.ckXMP.TabIndex = 5;
            this.ckXMP.Text = "Look for embedded tags - XMP";
            this.ckXMP.UseVisualStyleBackColor = true;
            // 
            // ckpImgDB
            // 
            this.ckpImgDB.AutoSize = true;
            this.ckpImgDB.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(224)))), ((int)(((byte)(224)))), ((int)(((byte)(224)))));
            this.ckpImgDB.Location = new System.Drawing.Point(432, 35);
            this.ckpImgDB.Name = "ckpImgDB";
            this.ckpImgDB.Size = new System.Drawing.Size(188, 17);
            this.ckpImgDB.TabIndex = 6;
            this.ckpImgDB.Text = "Look for embedded tags - pImgDB";
            this.ckpImgDB.UseVisualStyleBackColor = true;
            // 
            // ckSuggest
            // 
            this.ckSuggest.AutoSize = true;
            this.ckSuggest.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(224)))), ((int)(((byte)(224)))), ((int)(((byte)(224)))));
            this.ckSuggest.Location = new System.Drawing.Point(432, 58);
            this.ckSuggest.Name = "ckSuggest";
            this.ckSuggest.Size = new System.Drawing.Size(88, 17);
            this.ckSuggest.TabIndex = 7;
            this.ckSuggest.Text = "Suggest tags";
            this.ckSuggest.UseVisualStyleBackColor = true;
            // 
            // lbSuggest
            // 
            this.lbSuggest.AutoSize = true;
            this.lbSuggest.Cursor = System.Windows.Forms.Cursors.Hand;
            this.lbSuggest.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Underline, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lbSuggest.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(224)))), ((int)(((byte)(224)))), ((int)(((byte)(224)))));
            this.lbSuggest.Location = new System.Drawing.Point(526, 59);
            this.lbSuggest.Name = "lbSuggest";
            this.lbSuggest.Size = new System.Drawing.Size(57, 13);
            this.lbSuggest.TabIndex = 8;
            this.lbSuggest.Text = "(construct)";
            // 
            // label3
            // 
            this.label3.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(40)))), ((int)(((byte)(40)))), ((int)(((byte)(40)))));
            this.label3.Location = new System.Drawing.Point(12, 85);
            this.label3.Margin = new System.Windows.Forms.Padding(3, 6, 3, 6);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(608, 1);
            this.label3.TabIndex = 9;
            // 
            // label4
            // 
            this.label4.AutoSize = true;
            this.label4.ForeColor = System.Drawing.Color.Silver;
            this.label4.Location = new System.Drawing.Point(12, 98);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(42, 13);
            this.label4.TabIndex = 10;
            this.label4.Text = "fnMask";
            // 
            // txNameMask
            // 
            this.txNameMask.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(48)))), ((int)(((byte)(48)))), ((int)(((byte)(48)))));
            this.txNameMask.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.txNameMask.ForeColor = System.Drawing.Color.White;
            this.txNameMask.Location = new System.Drawing.Point(85, 95);
            this.txNameMask.Name = "txNameMask";
            this.txNameMask.Size = new System.Drawing.Size(535, 20);
            this.txNameMask.TabIndex = 11;
            // 
            // txName
            // 
            this.txName.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(48)))), ((int)(((byte)(48)))), ((int)(((byte)(48)))));
            this.txName.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.txName.ForeColor = System.Drawing.Color.White;
            this.txName.Location = new System.Drawing.Point(85, 121);
            this.txName.Name = "txName";
            this.txName.Size = new System.Drawing.Size(535, 20);
            this.txName.TabIndex = 13;
            this.txName.Text = "I am imaeg naim";
            // 
            // label5
            // 
            this.label5.AutoSize = true;
            this.label5.ForeColor = System.Drawing.Color.Silver;
            this.label5.Location = new System.Drawing.Point(12, 124);
            this.label5.Name = "label5";
            this.label5.Size = new System.Drawing.Size(65, 13);
            this.label5.TabIndex = 12;
            this.label5.Text = "Image name";
            // 
            // txDesc
            // 
            this.txDesc.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(48)))), ((int)(((byte)(48)))), ((int)(((byte)(48)))));
            this.txDesc.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.txDesc.ForeColor = System.Drawing.Color.White;
            this.txDesc.Location = new System.Drawing.Point(85, 147);
            this.txDesc.Name = "txDesc";
            this.txDesc.Size = new System.Drawing.Size(535, 20);
            this.txDesc.TabIndex = 15;
            this.txDesc.Text = "This are sum imaeg descripshun. This is awesome.";
            // 
            // label6
            // 
            this.label6.AutoSize = true;
            this.label6.ForeColor = System.Drawing.Color.Silver;
            this.label6.Location = new System.Drawing.Point(12, 150);
            this.label6.Name = "label6";
            this.label6.Size = new System.Drawing.Size(60, 13);
            this.label6.TabIndex = 14;
            this.label6.Text = "Description";
            // 
            // txTGen
            // 
            this.txTGen.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(48)))), ((int)(((byte)(48)))), ((int)(((byte)(48)))));
            this.txTGen.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.txTGen.ForeColor = System.Drawing.Color.White;
            this.txTGen.Location = new System.Drawing.Point(85, 173);
            this.txTGen.Name = "txTGen";
            this.txTGen.Size = new System.Drawing.Size(535, 20);
            this.txTGen.TabIndex = 17;
            this.txTGen.Text = "{em-xmp}, {em-pdb}, {1}, {2}, {3}, Fuckwin";
            // 
            // label7
            // 
            this.label7.AutoSize = true;
            this.label7.ForeColor = System.Drawing.Color.Silver;
            this.label7.Location = new System.Drawing.Point(12, 176);
            this.label7.Name = "label7";
            this.label7.Size = new System.Drawing.Size(57, 13);
            this.label7.TabIndex = 16;
            this.label7.Text = "T. General";
            // 
            // txTSrc
            // 
            this.txTSrc.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(48)))), ((int)(((byte)(48)))), ((int)(((byte)(48)))));
            this.txTSrc.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.txTSrc.ForeColor = System.Drawing.Color.White;
            this.txTSrc.Location = new System.Drawing.Point(85, 199);
            this.txTSrc.Name = "txTSrc";
            this.txTSrc.Size = new System.Drawing.Size(535, 20);
            this.txTSrc.TabIndex = 19;
            this.txTSrc.Text = "{em-xmp}, {em-pdb}, {1}, {2}, {3}, Hurrr";
            // 
            // label8
            // 
            this.label8.AutoSize = true;
            this.label8.ForeColor = System.Drawing.Color.Silver;
            this.label8.Location = new System.Drawing.Point(12, 202);
            this.label8.Name = "label8";
            this.label8.Size = new System.Drawing.Size(54, 13);
            this.label8.TabIndex = 18;
            this.label8.Text = "T. Source";
            // 
            // txTChr
            // 
            this.txTChr.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(48)))), ((int)(((byte)(48)))), ((int)(((byte)(48)))));
            this.txTChr.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.txTChr.ForeColor = System.Drawing.Color.White;
            this.txTChr.Location = new System.Drawing.Point(85, 225);
            this.txTChr.Name = "txTChr";
            this.txTChr.Size = new System.Drawing.Size(535, 20);
            this.txTChr.TabIndex = 21;
            this.txTChr.Text = "{em-xmp}, {em-pdb}, {1}, {2}, {3}, Durr";
            // 
            // label9
            // 
            this.label9.AutoSize = true;
            this.label9.ForeColor = System.Drawing.Color.Silver;
            this.label9.Location = new System.Drawing.Point(12, 228);
            this.label9.Name = "label9";
            this.label9.Size = new System.Drawing.Size(47, 13);
            this.label9.TabIndex = 20;
            this.label9.Text = "T. Chars";
            // 
            // txTArt
            // 
            this.txTArt.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(48)))), ((int)(((byte)(48)))), ((int)(((byte)(48)))));
            this.txTArt.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.txTArt.ForeColor = System.Drawing.Color.White;
            this.txTArt.Location = new System.Drawing.Point(85, 251);
            this.txTArt.Name = "txTArt";
            this.txTArt.Size = new System.Drawing.Size(535, 20);
            this.txTArt.TabIndex = 23;
            this.txTArt.Text = "{em-xmp}, {em-pdb}, {1}, {2}, {3}, Derp";
            // 
            // label10
            // 
            this.label10.AutoSize = true;
            this.label10.ForeColor = System.Drawing.Color.Silver;
            this.label10.Location = new System.Drawing.Point(12, 254);
            this.label10.Name = "label10";
            this.label10.Size = new System.Drawing.Size(48, 13);
            this.label10.TabIndex = 22;
            this.label10.Text = "T. Artists";
            // 
            // label11
            // 
            this.label11.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(40)))), ((int)(((byte)(40)))), ((int)(((byte)(40)))));
            this.label11.Location = new System.Drawing.Point(12, 280);
            this.label11.Margin = new System.Windows.Forms.Padding(3, 6, 3, 6);
            this.label11.Name = "label11";
            this.label11.Size = new System.Drawing.Size(608, 1);
            this.label11.TabIndex = 24;
            // 
            // label12
            // 
            this.label12.AutoSize = true;
            this.label12.ForeColor = System.Drawing.Color.Silver;
            this.label12.Location = new System.Drawing.Point(12, 287);
            this.label12.Name = "label12";
            this.label12.Size = new System.Drawing.Size(48, 13);
            this.label12.TabIndex = 25;
            this.label12.Text = "Progress";
            // 
            // lbProg
            // 
            this.lbProg.AutoSize = true;
            this.lbProg.ForeColor = System.Drawing.Color.White;
            this.lbProg.Location = new System.Drawing.Point(82, 287);
            this.lbProg.Name = "lbProg";
            this.lbProg.Size = new System.Drawing.Size(45, 13);
            this.lbProg.TabIndex = 26;
            this.lbProg.Text = "Inactive";
            // 
            // cmStart
            // 
            this.cmStart.ForeColor = System.Drawing.Color.White;
            this.cmStart.Location = new System.Drawing.Point(545, 338);
            this.cmStart.Name = "cmStart";
            this.cmStart.Size = new System.Drawing.Size(75, 23);
            this.cmStart.TabIndex = 27;
            this.cmStart.Text = "Start";
            this.cmStart.UseVisualStyleBackColor = true;
            this.cmStart.Click += new System.EventHandler(this.cmStart_Click);
            // 
            // cmCancel
            // 
            this.cmCancel.ForeColor = System.Drawing.Color.White;
            this.cmCancel.Location = new System.Drawing.Point(464, 338);
            this.cmCancel.Name = "cmCancel";
            this.cmCancel.Size = new System.Drawing.Size(75, 23);
            this.cmCancel.TabIndex = 28;
            this.cmCancel.Text = "Cancel";
            this.cmCancel.UseVisualStyleBackColor = true;
            this.cmCancel.Click += new System.EventHandler(this.cmCancel_Click);
            // 
            // cmTest
            // 
            this.cmTest.ForeColor = System.Drawing.Color.White;
            this.cmTest.Location = new System.Drawing.Point(383, 338);
            this.cmTest.Name = "cmTest";
            this.cmTest.Size = new System.Drawing.Size(75, 23);
            this.cmTest.TabIndex = 29;
            this.cmTest.Text = "Test";
            this.cmTest.UseVisualStyleBackColor = true;
            this.cmTest.Click += new System.EventHandler(this.cmTest_Click);
            // 
            // frmImport
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(56)))), ((int)(((byte)(56)))), ((int)(((byte)(56)))));
            this.ClientSize = new System.Drawing.Size(632, 373);
            this.Controls.Add(this.cmTest);
            this.Controls.Add(this.cmCancel);
            this.Controls.Add(this.cmStart);
            this.Controls.Add(this.lbProg);
            this.Controls.Add(this.label12);
            this.Controls.Add(this.label11);
            this.Controls.Add(this.txTArt);
            this.Controls.Add(this.label10);
            this.Controls.Add(this.txTChr);
            this.Controls.Add(this.label9);
            this.Controls.Add(this.txTSrc);
            this.Controls.Add(this.label8);
            this.Controls.Add(this.txTGen);
            this.Controls.Add(this.label7);
            this.Controls.Add(this.txDesc);
            this.Controls.Add(this.label6);
            this.Controls.Add(this.txName);
            this.Controls.Add(this.label5);
            this.Controls.Add(this.txNameMask);
            this.Controls.Add(this.label4);
            this.Controls.Add(this.label3);
            this.Controls.Add(this.lbSuggest);
            this.Controls.Add(this.ckSuggest);
            this.Controls.Add(this.ckpImgDB);
            this.Controls.Add(this.ckXMP);
            this.Controls.Add(this.lbSource);
            this.Controls.Add(this.label2);
            this.Controls.Add(this.label1);
            this.Controls.Add(this.pictureBox1);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedSingle;
            this.MaximizeBox = false;
            this.Name = "frmImport";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "Import directory";
            this.Load += new System.EventHandler(this.frmImport_Load);
            this.FormClosing += new System.Windows.Forms.FormClosingEventHandler(this.frmImport_FormClosing);
            ((System.ComponentModel.ISupportInitialize)(this.pictureBox1)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.PictureBox pictureBox1;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.Label lbSource;
        private System.Windows.Forms.CheckBox ckXMP;
        private System.Windows.Forms.CheckBox ckpImgDB;
        private System.Windows.Forms.CheckBox ckSuggest;
        private System.Windows.Forms.Label lbSuggest;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.Label label4;
        private System.Windows.Forms.TextBox txNameMask;
        private System.Windows.Forms.TextBox txName;
        private System.Windows.Forms.Label label5;
        private System.Windows.Forms.TextBox txDesc;
        private System.Windows.Forms.Label label6;
        private System.Windows.Forms.TextBox txTGen;
        private System.Windows.Forms.Label label7;
        private System.Windows.Forms.TextBox txTSrc;
        private System.Windows.Forms.Label label8;
        private System.Windows.Forms.TextBox txTChr;
        private System.Windows.Forms.Label label9;
        private System.Windows.Forms.TextBox txTArt;
        private System.Windows.Forms.Label label10;
        private System.Windows.Forms.Label label11;
        private System.Windows.Forms.Label label12;
        private System.Windows.Forms.Label lbProg;
        private System.Windows.Forms.Button cmStart;
        private System.Windows.Forms.Button cmCancel;
        private System.Windows.Forms.Button cmTest;
    }
}