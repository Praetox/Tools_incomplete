namespace LOICNet
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
            this.nico = new System.Windows.Forms.NotifyIcon(this.components);
            this.lbCustomSpd = new System.Windows.Forms.Label();
            this.tGetConfig = new System.Windows.Forms.Timer(this.components);
            this.tbDelay = new System.Windows.Forms.TrackBar();
            this.pictureBox1 = new System.Windows.Forms.PictureBox();
            this.groupBox1 = new System.Windows.Forms.GroupBox();
            this.label1 = new System.Windows.Forms.Label();
            this.label2 = new System.Windows.Forms.Label();
            this.label3 = new System.Windows.Forms.Label();
            this.label4 = new System.Windows.Forms.Label();
            this.label5 = new System.Windows.Forms.Label();
            this.label6 = new System.Windows.Forms.Label();
            this.label7 = new System.Windows.Forms.Label();
            this.label8 = new System.Windows.Forms.Label();
            this.label9 = new System.Windows.Forms.Label();
            this.label10 = new System.Windows.Forms.Label();
            this.label11 = new System.Windows.Forms.Label();
            this.label12 = new System.Windows.Forms.Label();
            this.lbXXPMsg = new System.Windows.Forms.Label();
            this.lbTimeout = new System.Windows.Forms.Label();
            this.lbSubsite = new System.Windows.Forms.Label();
            this.lbDelay = new System.Windows.Forms.Label();
            this.lbResp = new System.Windows.Forms.Label();
            this.lbThreads = new System.Windows.Forms.Label();
            this.lbMethod = new System.Windows.Forms.Label();
            this.lbPort = new System.Windows.Forms.Label();
            this.lbIP = new System.Windows.Forms.Label();
            this.lbRFreq = new System.Windows.Forms.Label();
            this.label16 = new System.Windows.Forms.Label();
            this.label24 = new System.Windows.Forms.Label();
            this.tHide = new System.Windows.Forms.Timer(this.components);
            this.groupBox2 = new System.Windows.Forms.GroupBox();
            this.lbFailed = new System.Windows.Forms.Label();
            this.lbRequested = new System.Windows.Forms.Label();
            this.label22 = new System.Windows.Forms.Label();
            this.label23 = new System.Windows.Forms.Label();
            this.lbDownloaded = new System.Windows.Forms.Label();
            this.lbDownloading = new System.Windows.Forms.Label();
            this.lbRequesting = new System.Windows.Forms.Label();
            this.lbConnecting = new System.Windows.Forms.Label();
            this.lbIdle = new System.Windows.Forms.Label();
            this.label13 = new System.Windows.Forms.Label();
            this.label14 = new System.Windows.Forms.Label();
            this.label15 = new System.Windows.Forms.Label();
            this.label17 = new System.Windows.Forms.Label();
            this.label18 = new System.Windows.Forms.Label();
            this.tShowStats = new System.Windows.Forms.Timer(this.components);
            ((System.ComponentModel.ISupportInitialize)(this.tbDelay)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.pictureBox1)).BeginInit();
            this.groupBox1.SuspendLayout();
            this.groupBox2.SuspendLayout();
            this.SuspendLayout();
            // 
            // nico
            // 
            this.nico.Text = "notifyIcon1";
            this.nico.Visible = true;
            this.nico.DoubleClick += new System.EventHandler(this.nico_DoubleClick);
            // 
            // lbCustomSpd
            // 
            this.lbCustomSpd.Font = new System.Drawing.Font("Microsoft Sans Serif", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lbCustomSpd.Location = new System.Drawing.Point(202, 10);
            this.lbCustomSpd.Name = "lbCustomSpd";
            this.lbCustomSpd.Size = new System.Drawing.Size(268, 24);
            this.lbCustomSpd.TabIndex = 2;
            this.lbCustomSpd.Text = "Entering background mode in 5 seconds...";
            this.lbCustomSpd.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // tGetConfig
            // 
            this.tGetConfig.Interval = 1000;
            this.tGetConfig.Tick += new System.EventHandler(this.tGetConfig_Tick);
            // 
            // tbDelay
            // 
            this.tbDelay.Location = new System.Drawing.Point(202, 37);
            this.tbDelay.Maximum = 20;
            this.tbDelay.Name = "tbDelay";
            this.tbDelay.Size = new System.Drawing.Size(268, 45);
            this.tbDelay.TabIndex = 3;
            this.tbDelay.Visible = false;
            this.tbDelay.ValueChanged += new System.EventHandler(this.tbDelay_ValueChanged);
            // 
            // pictureBox1
            // 
            this.pictureBox1.Image = ((System.Drawing.Image)(resources.GetObject("pictureBox1.Image")));
            this.pictureBox1.Location = new System.Drawing.Point(12, 10);
            this.pictureBox1.Name = "pictureBox1";
            this.pictureBox1.Size = new System.Drawing.Size(184, 463);
            this.pictureBox1.TabIndex = 4;
            this.pictureBox1.TabStop = false;
            // 
            // groupBox1
            // 
            this.groupBox1.Controls.Add(this.label24);
            this.groupBox1.Controls.Add(this.label16);
            this.groupBox1.Controls.Add(this.lbTimeout);
            this.groupBox1.Controls.Add(this.lbSubsite);
            this.groupBox1.Controls.Add(this.lbDelay);
            this.groupBox1.Controls.Add(this.lbResp);
            this.groupBox1.Controls.Add(this.lbThreads);
            this.groupBox1.Controls.Add(this.lbMethod);
            this.groupBox1.Controls.Add(this.lbPort);
            this.groupBox1.Controls.Add(this.lbIP);
            this.groupBox1.Controls.Add(this.lbRFreq);
            this.groupBox1.Controls.Add(this.lbXXPMsg);
            this.groupBox1.Controls.Add(this.label7);
            this.groupBox1.Controls.Add(this.label8);
            this.groupBox1.Controls.Add(this.label9);
            this.groupBox1.Controls.Add(this.label10);
            this.groupBox1.Controls.Add(this.label11);
            this.groupBox1.Controls.Add(this.label12);
            this.groupBox1.Controls.Add(this.label5);
            this.groupBox1.Controls.Add(this.label6);
            this.groupBox1.Controls.Add(this.label3);
            this.groupBox1.Controls.Add(this.label4);
            this.groupBox1.Controls.Add(this.label2);
            this.groupBox1.Controls.Add(this.label1);
            this.groupBox1.ForeColor = System.Drawing.Color.LightBlue;
            this.groupBox1.Location = new System.Drawing.Point(202, 88);
            this.groupBox1.Name = "groupBox1";
            this.groupBox1.Size = new System.Drawing.Size(268, 233);
            this.groupBox1.TabIndex = 5;
            this.groupBox1.TabStop = false;
            this.groupBox1.Text = "Configuration:";
            // 
            // label1
            // 
            this.label1.Location = new System.Drawing.Point(6, 16);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(100, 13);
            this.label1.TabIndex = 0;
            this.label1.Text = "Get config every";
            // 
            // label2
            // 
            this.label2.Location = new System.Drawing.Point(6, 29);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(100, 13);
            this.label2.TabIndex = 1;
            // 
            // label3
            // 
            this.label3.Location = new System.Drawing.Point(6, 55);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(100, 13);
            this.label3.TabIndex = 3;
            this.label3.Text = "Target Port";
            // 
            // label4
            // 
            this.label4.Location = new System.Drawing.Point(6, 42);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(100, 13);
            this.label4.TabIndex = 2;
            this.label4.Text = "Target IP";
            // 
            // label5
            // 
            this.label5.Location = new System.Drawing.Point(6, 81);
            this.label5.Name = "label5";
            this.label5.Size = new System.Drawing.Size(100, 13);
            this.label5.TabIndex = 5;
            this.label5.Text = "Emulated users";
            // 
            // label6
            // 
            this.label6.Location = new System.Drawing.Point(6, 68);
            this.label6.Name = "label6";
            this.label6.Size = new System.Drawing.Size(100, 13);
            this.label6.TabIndex = 4;
            this.label6.Text = "Attack method";
            // 
            // label7
            // 
            this.label7.Location = new System.Drawing.Point(6, 211);
            this.label7.Name = "label7";
            this.label7.Size = new System.Drawing.Size(100, 13);
            this.label7.TabIndex = 11;
            this.label7.Text = "Timeout";
            // 
            // label8
            // 
            this.label8.Location = new System.Drawing.Point(6, 198);
            this.label8.Name = "label8";
            this.label8.Size = new System.Drawing.Size(100, 13);
            this.label8.TabIndex = 10;
            this.label8.Text = "Subsite";
            // 
            // label9
            // 
            this.label9.Location = new System.Drawing.Point(6, 133);
            this.label9.Name = "label9";
            this.label9.Size = new System.Drawing.Size(100, 13);
            this.label9.TabIndex = 9;
            this.label9.Text = "XXP message";
            // 
            // label10
            // 
            this.label10.Location = new System.Drawing.Point(6, 120);
            this.label10.Name = "label10";
            this.label10.Size = new System.Drawing.Size(100, 13);
            this.label10.TabIndex = 8;
            // 
            // label11
            // 
            this.label11.Location = new System.Drawing.Point(6, 107);
            this.label11.Name = "label11";
            this.label11.Size = new System.Drawing.Size(100, 13);
            this.label11.TabIndex = 7;
            this.label11.Text = "Slowdown delay";
            // 
            // label12
            // 
            this.label12.Location = new System.Drawing.Point(6, 94);
            this.label12.Name = "label12";
            this.label12.Size = new System.Drawing.Size(100, 13);
            this.label12.TabIndex = 6;
            this.label12.Text = "Wait for response";
            // 
            // lbXXPMsg
            // 
            this.lbXXPMsg.Location = new System.Drawing.Point(6, 146);
            this.lbXXPMsg.Name = "lbXXPMsg";
            this.lbXXPMsg.Size = new System.Drawing.Size(256, 52);
            this.lbXXPMsg.TabIndex = 12;
            this.lbXXPMsg.Text = "No config loaded yet";
            // 
            // lbTimeout
            // 
            this.lbTimeout.Location = new System.Drawing.Point(112, 211);
            this.lbTimeout.Name = "lbTimeout";
            this.lbTimeout.Size = new System.Drawing.Size(150, 13);
            this.lbTimeout.TabIndex = 22;
            this.lbTimeout.Text = "No config loaded yet";
            // 
            // lbSubsite
            // 
            this.lbSubsite.Location = new System.Drawing.Point(112, 198);
            this.lbSubsite.Name = "lbSubsite";
            this.lbSubsite.Size = new System.Drawing.Size(150, 13);
            this.lbSubsite.TabIndex = 21;
            this.lbSubsite.Text = "No config loaded yet";
            // 
            // lbDelay
            // 
            this.lbDelay.Location = new System.Drawing.Point(112, 107);
            this.lbDelay.Name = "lbDelay";
            this.lbDelay.Size = new System.Drawing.Size(150, 13);
            this.lbDelay.TabIndex = 19;
            this.lbDelay.Text = "No config loaded yet";
            // 
            // lbResp
            // 
            this.lbResp.Location = new System.Drawing.Point(112, 94);
            this.lbResp.Name = "lbResp";
            this.lbResp.Size = new System.Drawing.Size(150, 13);
            this.lbResp.TabIndex = 18;
            this.lbResp.Text = "No config loaded yet";
            // 
            // lbThreads
            // 
            this.lbThreads.Location = new System.Drawing.Point(112, 81);
            this.lbThreads.Name = "lbThreads";
            this.lbThreads.Size = new System.Drawing.Size(150, 13);
            this.lbThreads.TabIndex = 17;
            this.lbThreads.Text = "No config loaded yet";
            // 
            // lbMethod
            // 
            this.lbMethod.Location = new System.Drawing.Point(112, 68);
            this.lbMethod.Name = "lbMethod";
            this.lbMethod.Size = new System.Drawing.Size(150, 13);
            this.lbMethod.TabIndex = 16;
            this.lbMethod.Text = "No config loaded yet";
            // 
            // lbPort
            // 
            this.lbPort.Location = new System.Drawing.Point(112, 55);
            this.lbPort.Name = "lbPort";
            this.lbPort.Size = new System.Drawing.Size(150, 13);
            this.lbPort.TabIndex = 15;
            this.lbPort.Text = "No config loaded yet";
            // 
            // lbIP
            // 
            this.lbIP.Location = new System.Drawing.Point(112, 42);
            this.lbIP.Name = "lbIP";
            this.lbIP.Size = new System.Drawing.Size(150, 13);
            this.lbIP.TabIndex = 14;
            this.lbIP.Text = "No config loaded yet";
            // 
            // lbRFreq
            // 
            this.lbRFreq.Location = new System.Drawing.Point(112, 16);
            this.lbRFreq.Name = "lbRFreq";
            this.lbRFreq.Size = new System.Drawing.Size(150, 13);
            this.lbRFreq.TabIndex = 13;
            this.lbRFreq.Text = "No config loaded yet";
            // 
            // label16
            // 
            this.label16.Location = new System.Drawing.Point(112, 29);
            this.label16.Name = "label16";
            this.label16.Size = new System.Drawing.Size(150, 13);
            this.label16.TabIndex = 23;
            // 
            // label24
            // 
            this.label24.Location = new System.Drawing.Point(112, 120);
            this.label24.Name = "label24";
            this.label24.Size = new System.Drawing.Size(150, 13);
            this.label24.TabIndex = 24;
            // 
            // tHide
            // 
            this.tHide.Interval = 5000;
            this.tHide.Tick += new System.EventHandler(this.tHide_Tick);
            // 
            // groupBox2
            // 
            this.groupBox2.Controls.Add(this.lbFailed);
            this.groupBox2.Controls.Add(this.lbRequested);
            this.groupBox2.Controls.Add(this.label22);
            this.groupBox2.Controls.Add(this.label23);
            this.groupBox2.Controls.Add(this.lbDownloaded);
            this.groupBox2.Controls.Add(this.lbDownloading);
            this.groupBox2.Controls.Add(this.lbRequesting);
            this.groupBox2.Controls.Add(this.lbConnecting);
            this.groupBox2.Controls.Add(this.lbIdle);
            this.groupBox2.Controls.Add(this.label13);
            this.groupBox2.Controls.Add(this.label14);
            this.groupBox2.Controls.Add(this.label15);
            this.groupBox2.Controls.Add(this.label17);
            this.groupBox2.Controls.Add(this.label18);
            this.groupBox2.ForeColor = System.Drawing.Color.LightBlue;
            this.groupBox2.Location = new System.Drawing.Point(202, 327);
            this.groupBox2.Name = "groupBox2";
            this.groupBox2.Size = new System.Drawing.Size(268, 87);
            this.groupBox2.TabIndex = 6;
            this.groupBox2.TabStop = false;
            this.groupBox2.Text = "Statistics";
            // 
            // lbFailed
            // 
            this.lbFailed.Location = new System.Drawing.Point(180, 46);
            this.lbFailed.Name = "lbFailed";
            this.lbFailed.Size = new System.Drawing.Size(81, 15);
            this.lbFailed.TabIndex = 52;
            this.lbFailed.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // lbRequested
            // 
            this.lbRequested.Location = new System.Drawing.Point(93, 46);
            this.lbRequested.Name = "lbRequested";
            this.lbRequested.Size = new System.Drawing.Size(81, 15);
            this.lbRequested.TabIndex = 51;
            this.lbRequested.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // label22
            // 
            this.label22.Location = new System.Drawing.Point(180, 61);
            this.label22.Name = "label22";
            this.label22.Size = new System.Drawing.Size(81, 15);
            this.label22.TabIndex = 50;
            this.label22.Text = "Failed";
            this.label22.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // label23
            // 
            this.label23.Location = new System.Drawing.Point(93, 61);
            this.label23.Name = "label23";
            this.label23.Size = new System.Drawing.Size(81, 15);
            this.label23.TabIndex = 49;
            this.label23.Text = "Requested";
            this.label23.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // lbDownloaded
            // 
            this.lbDownloaded.Location = new System.Drawing.Point(6, 46);
            this.lbDownloaded.Name = "lbDownloaded";
            this.lbDownloaded.Size = new System.Drawing.Size(81, 15);
            this.lbDownloaded.TabIndex = 48;
            this.lbDownloaded.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // lbDownloading
            // 
            this.lbDownloading.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(12)))), ((int)(((byte)(24)))), ((int)(((byte)(32)))));
            this.lbDownloading.Location = new System.Drawing.Point(201, 31);
            this.lbDownloading.Name = "lbDownloading";
            this.lbDownloading.Size = new System.Drawing.Size(59, 15);
            this.lbDownloading.TabIndex = 47;
            this.lbDownloading.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // lbRequesting
            // 
            this.lbRequesting.Location = new System.Drawing.Point(136, 31);
            this.lbRequesting.Name = "lbRequesting";
            this.lbRequesting.Size = new System.Drawing.Size(59, 15);
            this.lbRequesting.TabIndex = 46;
            this.lbRequesting.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // lbConnecting
            // 
            this.lbConnecting.Location = new System.Drawing.Point(71, 31);
            this.lbConnecting.Name = "lbConnecting";
            this.lbConnecting.Size = new System.Drawing.Size(59, 15);
            this.lbConnecting.TabIndex = 45;
            this.lbConnecting.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // lbIdle
            // 
            this.lbIdle.Location = new System.Drawing.Point(10, 31);
            this.lbIdle.Name = "lbIdle";
            this.lbIdle.Size = new System.Drawing.Size(59, 15);
            this.lbIdle.TabIndex = 44;
            this.lbIdle.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // label13
            // 
            this.label13.Location = new System.Drawing.Point(6, 61);
            this.label13.Name = "label13";
            this.label13.Size = new System.Drawing.Size(81, 15);
            this.label13.TabIndex = 43;
            this.label13.Text = "Downloaded";
            this.label13.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // label14
            // 
            this.label14.Location = new System.Drawing.Point(201, 16);
            this.label14.Name = "label14";
            this.label14.Size = new System.Drawing.Size(59, 15);
            this.label14.TabIndex = 42;
            this.label14.Text = "D/L";
            this.label14.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // label15
            // 
            this.label15.Location = new System.Drawing.Point(136, 16);
            this.label15.Name = "label15";
            this.label15.Size = new System.Drawing.Size(59, 15);
            this.label15.TabIndex = 41;
            this.label15.Text = "Req";
            this.label15.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // label17
            // 
            this.label17.Location = new System.Drawing.Point(71, 16);
            this.label17.Name = "label17";
            this.label17.Size = new System.Drawing.Size(59, 15);
            this.label17.TabIndex = 40;
            this.label17.Text = "Conn";
            this.label17.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // label18
            // 
            this.label18.Location = new System.Drawing.Point(10, 16);
            this.label18.Name = "label18";
            this.label18.Size = new System.Drawing.Size(59, 15);
            this.label18.TabIndex = 39;
            this.label18.Text = "Idle";
            this.label18.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // frmMain
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(12)))), ((int)(((byte)(24)))), ((int)(((byte)(32)))));
            this.ClientSize = new System.Drawing.Size(482, 485);
            this.Controls.Add(this.groupBox2);
            this.Controls.Add(this.groupBox1);
            this.Controls.Add(this.pictureBox1);
            this.Controls.Add(this.tbDelay);
            this.Controls.Add(this.lbCustomSpd);
            this.ForeColor = System.Drawing.Color.LightBlue;
            this.Name = "frmMain";
            this.ShowInTaskbar = false;
            this.Text = "LOICNet v";
            this.Load += new System.EventHandler(this.frmMain_Load);
            ((System.ComponentModel.ISupportInitialize)(this.tbDelay)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.pictureBox1)).EndInit();
            this.groupBox1.ResumeLayout(false);
            this.groupBox2.ResumeLayout(false);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.NotifyIcon nico;
        private System.Windows.Forms.Label lbCustomSpd;
        private System.Windows.Forms.Timer tGetConfig;
        private System.Windows.Forms.TrackBar tbDelay;
        private System.Windows.Forms.PictureBox pictureBox1;
        private System.Windows.Forms.GroupBox groupBox1;
        private System.Windows.Forms.Label lbXXPMsg;
        private System.Windows.Forms.Label label7;
        private System.Windows.Forms.Label label8;
        private System.Windows.Forms.Label label9;
        private System.Windows.Forms.Label label10;
        private System.Windows.Forms.Label label11;
        private System.Windows.Forms.Label label12;
        private System.Windows.Forms.Label label5;
        private System.Windows.Forms.Label label6;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.Label label4;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Label label24;
        private System.Windows.Forms.Label label16;
        private System.Windows.Forms.Label lbTimeout;
        private System.Windows.Forms.Label lbSubsite;
        private System.Windows.Forms.Label lbDelay;
        private System.Windows.Forms.Label lbResp;
        private System.Windows.Forms.Label lbThreads;
        private System.Windows.Forms.Label lbMethod;
        private System.Windows.Forms.Label lbPort;
        private System.Windows.Forms.Label lbIP;
        private System.Windows.Forms.Label lbRFreq;
        private System.Windows.Forms.Timer tHide;
        private System.Windows.Forms.GroupBox groupBox2;
        private System.Windows.Forms.Label lbFailed;
        private System.Windows.Forms.Label lbRequested;
        private System.Windows.Forms.Label label22;
        private System.Windows.Forms.Label label23;
        private System.Windows.Forms.Label lbDownloaded;
        private System.Windows.Forms.Label lbDownloading;
        private System.Windows.Forms.Label lbRequesting;
        private System.Windows.Forms.Label lbConnecting;
        private System.Windows.Forms.Label lbIdle;
        private System.Windows.Forms.Label label13;
        private System.Windows.Forms.Label label14;
        private System.Windows.Forms.Label label15;
        private System.Windows.Forms.Label label17;
        private System.Windows.Forms.Label label18;
        private System.Windows.Forms.Timer tShowStats;
    }
}

