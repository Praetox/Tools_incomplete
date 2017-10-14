namespace png_crc
{
    partial class Form1
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
            this.label1 = new System.Windows.Forms.Label();
            this.imSrc = new System.Windows.Forms.TextBox();
            this.tgSrc = new System.Windows.Forms.TextBox();
            this.label2 = new System.Windows.Forms.Label();
            this.imDst = new System.Windows.Forms.TextBox();
            this.label3 = new System.Windows.Forms.Label();
            this.button1 = new System.Windows.Forms.Button();
            this.button2 = new System.Windows.Forms.Button();
            this.button3 = new System.Windows.Forms.Button();
            this.cmCreate = new System.Windows.Forms.Button();
            this.txName = new System.Windows.Forms.TextBox();
            this.label4 = new System.Windows.Forms.Label();
            this.txDesc = new System.Windows.Forms.TextBox();
            this.label5 = new System.Windows.Forms.Label();
            this.txTagsGen = new System.Windows.Forms.TextBox();
            this.label6 = new System.Windows.Forms.Label();
            this.cmBuild = new System.Windows.Forms.Button();
            this.txTagsSrc = new System.Windows.Forms.TextBox();
            this.txTagsChr = new System.Windows.Forms.TextBox();
            this.txTagsArt = new System.Windows.Forms.TextBox();
            this.groupBox1 = new System.Windows.Forms.GroupBox();
            this.groupBox2 = new System.Windows.Forms.GroupBox();
            this.groupBox3 = new System.Windows.Forms.GroupBox();
            this.optReadProp = new System.Windows.Forms.RadioButton();
            this.optReadXMP = new System.Windows.Forms.RadioButton();
            this.optWriteXMP = new System.Windows.Forms.RadioButton();
            this.groupBox1.SuspendLayout();
            this.groupBox2.SuspendLayout();
            this.groupBox3.SuspendLayout();
            this.SuspendLayout();
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(6, 22);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(36, 13);
            this.label1.TabIndex = 0;
            this.label1.Text = "Image";
            // 
            // imSrc
            // 
            this.imSrc.Location = new System.Drawing.Point(69, 19);
            this.imSrc.Name = "imSrc";
            this.imSrc.Size = new System.Drawing.Size(173, 20);
            this.imSrc.TabIndex = 1;
            this.imSrc.Text = "src.png";
            // 
            // tgSrc
            // 
            this.tgSrc.Location = new System.Drawing.Point(69, 45);
            this.tgSrc.Name = "tgSrc";
            this.tgSrc.Size = new System.Drawing.Size(173, 20);
            this.tgSrc.TabIndex = 3;
            this.tgSrc.Text = "src.tag";
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(6, 48);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(31, 13);
            this.label2.TabIndex = 2;
            this.label2.Text = "Tags";
            // 
            // imDst
            // 
            this.imDst.Location = new System.Drawing.Point(69, 71);
            this.imDst.Name = "imDst";
            this.imDst.Size = new System.Drawing.Size(173, 20);
            this.imDst.TabIndex = 5;
            this.imDst.Text = "out.png";
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Location = new System.Drawing.Point(6, 74);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(39, 13);
            this.label3.TabIndex = 4;
            this.label3.Text = "Output";
            // 
            // button1
            // 
            this.button1.Location = new System.Drawing.Point(248, 19);
            this.button1.Name = "button1";
            this.button1.Size = new System.Drawing.Size(26, 20);
            this.button1.TabIndex = 6;
            this.button1.Text = "...";
            this.button1.UseVisualStyleBackColor = true;
            // 
            // button2
            // 
            this.button2.Location = new System.Drawing.Point(248, 44);
            this.button2.Name = "button2";
            this.button2.Size = new System.Drawing.Size(26, 20);
            this.button2.TabIndex = 7;
            this.button2.Text = "...";
            this.button2.UseVisualStyleBackColor = true;
            // 
            // button3
            // 
            this.button3.Location = new System.Drawing.Point(248, 71);
            this.button3.Name = "button3";
            this.button3.Size = new System.Drawing.Size(26, 20);
            this.button3.TabIndex = 8;
            this.button3.Text = "...";
            this.button3.UseVisualStyleBackColor = true;
            // 
            // cmCreate
            // 
            this.cmCreate.Location = new System.Drawing.Point(199, 97);
            this.cmCreate.Name = "cmCreate";
            this.cmCreate.Size = new System.Drawing.Size(75, 23);
            this.cmCreate.TabIndex = 9;
            this.cmCreate.Text = "Create";
            this.cmCreate.UseVisualStyleBackColor = true;
            this.cmCreate.Click += new System.EventHandler(this.cmCreate_Click);
            // 
            // txName
            // 
            this.txName.Location = new System.Drawing.Point(69, 19);
            this.txName.Name = "txName";
            this.txName.Size = new System.Drawing.Size(205, 20);
            this.txName.TabIndex = 11;
            this.txName.Text = "Imaeg";
            // 
            // label4
            // 
            this.label4.AutoSize = true;
            this.label4.Location = new System.Drawing.Point(6, 22);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(35, 13);
            this.label4.TabIndex = 10;
            this.label4.Text = "Name";
            // 
            // txDesc
            // 
            this.txDesc.Location = new System.Drawing.Point(69, 45);
            this.txDesc.Name = "txDesc";
            this.txDesc.Size = new System.Drawing.Size(205, 20);
            this.txDesc.TabIndex = 13;
            this.txDesc.Text = "Descripshun";
            // 
            // label5
            // 
            this.label5.AutoSize = true;
            this.label5.Location = new System.Drawing.Point(6, 48);
            this.label5.Name = "label5";
            this.label5.Size = new System.Drawing.Size(32, 13);
            this.label5.TabIndex = 12;
            this.label5.Text = "Desc";
            // 
            // txTagsGen
            // 
            this.txTagsGen.Location = new System.Drawing.Point(69, 71);
            this.txTagsGen.Name = "txTagsGen";
            this.txTagsGen.Size = new System.Drawing.Size(205, 20);
            this.txTagsGen.TabIndex = 15;
            this.txTagsGen.Text = "Wallpaper,animu,game,fanart";
            // 
            // label6
            // 
            this.label6.AutoSize = true;
            this.label6.Location = new System.Drawing.Point(6, 74);
            this.label6.Name = "label6";
            this.label6.Size = new System.Drawing.Size(31, 13);
            this.label6.TabIndex = 14;
            this.label6.Text = "Tags";
            // 
            // cmBuild
            // 
            this.cmBuild.Location = new System.Drawing.Point(199, 175);
            this.cmBuild.Name = "cmBuild";
            this.cmBuild.Size = new System.Drawing.Size(75, 23);
            this.cmBuild.TabIndex = 16;
            this.cmBuild.Text = "Build";
            this.cmBuild.UseVisualStyleBackColor = true;
            this.cmBuild.Click += new System.EventHandler(this.cmBuild_Click);
            // 
            // txTagsSrc
            // 
            this.txTagsSrc.Location = new System.Drawing.Point(69, 97);
            this.txTagsSrc.Name = "txTagsSrc";
            this.txTagsSrc.Size = new System.Drawing.Size(205, 20);
            this.txTagsSrc.TabIndex = 17;
            this.txTagsSrc.Text = "4chan,w-568395,Ragnarok";
            // 
            // txTagsChr
            // 
            this.txTagsChr.Location = new System.Drawing.Point(69, 123);
            this.txTagsChr.Name = "txTagsChr";
            this.txTagsChr.Size = new System.Drawing.Size(205, 20);
            this.txTagsChr.TabIndex = 18;
            this.txTagsChr.Text = "Unknown";
            // 
            // txTagsArt
            // 
            this.txTagsArt.Location = new System.Drawing.Point(69, 149);
            this.txTagsArt.Name = "txTagsArt";
            this.txTagsArt.Size = new System.Drawing.Size(205, 20);
            this.txTagsArt.TabIndex = 19;
            this.txTagsArt.Text = "Unknown";
            // 
            // groupBox1
            // 
            this.groupBox1.Controls.Add(this.button1);
            this.groupBox1.Controls.Add(this.label1);
            this.groupBox1.Controls.Add(this.imSrc);
            this.groupBox1.Controls.Add(this.label2);
            this.groupBox1.Controls.Add(this.tgSrc);
            this.groupBox1.Controls.Add(this.label3);
            this.groupBox1.Controls.Add(this.imDst);
            this.groupBox1.Controls.Add(this.button2);
            this.groupBox1.Controls.Add(this.button3);
            this.groupBox1.Controls.Add(this.cmCreate);
            this.groupBox1.Enabled = false;
            this.groupBox1.Location = new System.Drawing.Point(12, 12);
            this.groupBox1.Name = "groupBox1";
            this.groupBox1.Size = new System.Drawing.Size(280, 126);
            this.groupBox1.TabIndex = 20;
            this.groupBox1.TabStop = false;
            this.groupBox1.Text = "Write prepared tags (deprecated)";
            // 
            // groupBox2
            // 
            this.groupBox2.Controls.Add(this.txName);
            this.groupBox2.Controls.Add(this.label4);
            this.groupBox2.Controls.Add(this.txTagsArt);
            this.groupBox2.Controls.Add(this.label5);
            this.groupBox2.Controls.Add(this.txTagsChr);
            this.groupBox2.Controls.Add(this.txDesc);
            this.groupBox2.Controls.Add(this.txTagsSrc);
            this.groupBox2.Controls.Add(this.label6);
            this.groupBox2.Controls.Add(this.cmBuild);
            this.groupBox2.Controls.Add(this.txTagsGen);
            this.groupBox2.Location = new System.Drawing.Point(12, 144);
            this.groupBox2.Name = "groupBox2";
            this.groupBox2.Size = new System.Drawing.Size(280, 204);
            this.groupBox2.TabIndex = 21;
            this.groupBox2.TabStop = false;
            this.groupBox2.Text = "Generate and write tags";
            // 
            // groupBox3
            // 
            this.groupBox3.Controls.Add(this.optReadProp);
            this.groupBox3.Controls.Add(this.optReadXMP);
            this.groupBox3.Controls.Add(this.optWriteXMP);
            this.groupBox3.Location = new System.Drawing.Point(298, 12);
            this.groupBox3.Name = "groupBox3";
            this.groupBox3.Size = new System.Drawing.Size(150, 91);
            this.groupBox3.TabIndex = 22;
            this.groupBox3.TabStop = false;
            this.groupBox3.Text = "Action to perform";
            // 
            // optReadProp
            // 
            this.optReadProp.AutoSize = true;
            this.optReadProp.Location = new System.Drawing.Point(6, 65);
            this.optReadProp.Name = "optReadProp";
            this.optReadProp.Size = new System.Drawing.Size(76, 17);
            this.optReadProp.TabIndex = 2;
            this.optReadProp.TabStop = true;
            this.optReadProp.Text = "Read Prop";
            this.optReadProp.UseVisualStyleBackColor = true;
            // 
            // optReadXMP
            // 
            this.optReadXMP.AutoSize = true;
            this.optReadXMP.Location = new System.Drawing.Point(6, 42);
            this.optReadXMP.Name = "optReadXMP";
            this.optReadXMP.Size = new System.Drawing.Size(77, 17);
            this.optReadXMP.TabIndex = 1;
            this.optReadXMP.TabStop = true;
            this.optReadXMP.Text = "Read XMP";
            this.optReadXMP.UseVisualStyleBackColor = true;
            // 
            // optWriteXMP
            // 
            this.optWriteXMP.AutoSize = true;
            this.optWriteXMP.Checked = true;
            this.optWriteXMP.Location = new System.Drawing.Point(6, 19);
            this.optWriteXMP.Name = "optWriteXMP";
            this.optWriteXMP.Size = new System.Drawing.Size(76, 17);
            this.optWriteXMP.TabIndex = 0;
            this.optWriteXMP.TabStop = true;
            this.optWriteXMP.Text = "Write XMP";
            this.optWriteXMP.UseVisualStyleBackColor = true;
            // 
            // Form1
            // 
            this.AllowDrop = true;
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(577, 379);
            this.Controls.Add(this.groupBox3);
            this.Controls.Add(this.groupBox2);
            this.Controls.Add(this.groupBox1);
            this.Name = "Form1";
            this.Text = "Form1";
            this.DragDrop += new System.Windows.Forms.DragEventHandler(this.Form1_DragDrop);
            this.DragOver += new System.Windows.Forms.DragEventHandler(this.Form1_DragOver);
            this.groupBox1.ResumeLayout(false);
            this.groupBox1.PerformLayout();
            this.groupBox2.ResumeLayout(false);
            this.groupBox2.PerformLayout();
            this.groupBox3.ResumeLayout(false);
            this.groupBox3.PerformLayout();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.TextBox imSrc;
        private System.Windows.Forms.TextBox tgSrc;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.TextBox imDst;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.Button button1;
        private System.Windows.Forms.Button button2;
        private System.Windows.Forms.Button button3;
        private System.Windows.Forms.Button cmCreate;
        private System.Windows.Forms.TextBox txName;
        private System.Windows.Forms.Label label4;
        private System.Windows.Forms.TextBox txDesc;
        private System.Windows.Forms.Label label5;
        private System.Windows.Forms.TextBox txTagsGen;
        private System.Windows.Forms.Label label6;
        private System.Windows.Forms.Button cmBuild;
        private System.Windows.Forms.TextBox txTagsSrc;
        private System.Windows.Forms.TextBox txTagsChr;
        private System.Windows.Forms.TextBox txTagsArt;
        private System.Windows.Forms.GroupBox groupBox1;
        private System.Windows.Forms.GroupBox groupBox2;
        private System.Windows.Forms.GroupBox groupBox3;
        private System.Windows.Forms.RadioButton optReadProp;
        private System.Windows.Forms.RadioButton optReadXMP;
        private System.Windows.Forms.RadioButton optWriteXMP;
    }
}

