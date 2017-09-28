namespace LabelingVisualIdentification
{
    partial class MainForm
    {
        /// <summary>
        /// 必需的设计器变量。
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// 清理所有正在使用的资源。
        /// </summary>
        /// <param name="disposing">如果应释放托管资源，为 true；否则为 false。</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Windows 窗体设计器生成的代码

        /// <summary>
        /// 设计器支持所需的方法 - 不要
        /// 使用代码编辑器修改此方法的内容。
        /// </summary>
        private void InitializeComponent()
        {
            this.components = new System.ComponentModel.Container();
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(MainForm));
            this.imageViewer1 = new NationalInstruments.Vision.WindowsForms.ImageViewer();
            this.imageViewer2 = new NationalInstruments.Vision.WindowsForms.ImageViewer();
            this.btnProgramming = new System.Windows.Forms.Button();
            this.groupBox2 = new System.Windows.Forms.GroupBox();
            this.btnWorkInfo = new System.Windows.Forms.Button();
            this.label13 = new System.Windows.Forms.Label();
            this.label3 = new System.Windows.Forms.Label();
            this.label2 = new System.Windows.Forms.Label();
            this.label1 = new System.Windows.Forms.Label();
            this.txtInformation = new System.Windows.Forms.TextBox();
            this.txtCompare = new System.Windows.Forms.TextBox();
            this.txtReceived = new System.Windows.Forms.TextBox();
            this.txtSend = new System.Windows.Forms.TextBox();
            this.tabControl1 = new System.Windows.Forms.TabControl();
            this.tabPage1 = new System.Windows.Forms.TabPage();
            this.label5 = new System.Windows.Forms.Label();
            this.cbxProgram = new System.Windows.Forms.ComboBox();
            this.btnStop = new System.Windows.Forms.Button();
            this.btnAnalysis2 = new System.Windows.Forms.Button();
            this.btnAnalysis1 = new System.Windows.Forms.Button();
            this.btnSnap2 = new System.Windows.Forms.Button();
            this.btnSnap1 = new System.Windows.Forms.Button();
            this.btnRun = new System.Windows.Forms.Button();
            this.tabPage2 = new System.Windows.Forms.TabPage();
            this.btnSave = new System.Windows.Forms.Button();
            this.groupBox4 = new System.Windows.Forms.GroupBox();
            this.txtCameraID = new System.Windows.Forms.TextBox();
            this.label11 = new System.Windows.Forms.Label();
            this.groupBox1 = new System.Windows.Forms.GroupBox();
            this.txtPort = new System.Windows.Forms.TextBox();
            this.txtIP = new System.Windows.Forms.TextBox();
            this.label10 = new System.Windows.Forms.Label();
            this.label9 = new System.Windows.Forms.Label();
            this.groupBox3 = new System.Windows.Forms.GroupBox();
            this.cbxStopbits = new System.Windows.Forms.ComboBox();
            this.cbxParity = new System.Windows.Forms.ComboBox();
            this.cbxDataBits = new System.Windows.Forms.ComboBox();
            this.cbxBaudRate = new System.Windows.Forms.ComboBox();
            this.cbxPortName = new System.Windows.Forms.ComboBox();
            this.label4 = new System.Windows.Forms.Label();
            this.label6 = new System.Windows.Forms.Label();
            this.label7 = new System.Windows.Forms.Label();
            this.label8 = new System.Windows.Forms.Label();
            this.label12 = new System.Windows.Forms.Label();
            this.btnMain = new System.Windows.Forms.Button();
            this.btnSettings = new System.Windows.Forms.Button();
            this.btnExit = new System.Windows.Forms.Button();
            this.serialPort1 = new System.IO.Ports.SerialPort(this.components);
            this.lblSFIS = new System.Windows.Forms.Label();
            this.lblSerialPort = new System.Windows.Forms.Label();
            this.lblCamera = new System.Windows.Forms.Label();
            this.lblResult1 = new System.Windows.Forms.Label();
            this.lblResult2 = new System.Windows.Forms.Label();
            this.pictureBox1 = new System.Windows.Forms.PictureBox();
            this.pictureBox2 = new System.Windows.Forms.PictureBox();
            this.groupBox2.SuspendLayout();
            this.tabControl1.SuspendLayout();
            this.tabPage1.SuspendLayout();
            this.tabPage2.SuspendLayout();
            this.groupBox4.SuspendLayout();
            this.groupBox1.SuspendLayout();
            this.groupBox3.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.pictureBox1)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.pictureBox2)).BeginInit();
            this.SuspendLayout();
            // 
            // imageViewer1
            // 
            this.imageViewer1.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.imageViewer1.Location = new System.Drawing.Point(12, 12);
            this.imageViewer1.Name = "imageViewer1";
            this.imageViewer1.ShowImageInfo = true;
            this.imageViewer1.ShowToolbar = true;
            this.imageViewer1.Size = new System.Drawing.Size(571, 407);
            this.imageViewer1.TabIndex = 0;
            this.imageViewer1.ZoomToFit = true;
            // 
            // imageViewer2
            // 
            this.imageViewer2.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.imageViewer2.Location = new System.Drawing.Point(12, 428);
            this.imageViewer2.Name = "imageViewer2";
            this.imageViewer2.ShowImageInfo = true;
            this.imageViewer2.ShowToolbar = true;
            this.imageViewer2.Size = new System.Drawing.Size(571, 397);
            this.imageViewer2.TabIndex = 0;
            this.imageViewer2.ZoomToFit = true;
            // 
            // btnProgramming
            // 
            this.btnProgramming.Font = new System.Drawing.Font("PMingLiU", 11F);
            this.btnProgramming.Location = new System.Drawing.Point(1353, 138);
            this.btnProgramming.Name = "btnProgramming";
            this.btnProgramming.Size = new System.Drawing.Size(130, 64);
            this.btnProgramming.TabIndex = 1;
            this.btnProgramming.Text = "PROGRAMMING";
            this.btnProgramming.UseVisualStyleBackColor = true;
            this.btnProgramming.Click += new System.EventHandler(this.btnProgramming_Click);
            // 
            // groupBox2
            // 
            this.groupBox2.BackColor = System.Drawing.SystemColors.ActiveCaption;
            this.groupBox2.Controls.Add(this.btnWorkInfo);
            this.groupBox2.Controls.Add(this.label13);
            this.groupBox2.Controls.Add(this.label3);
            this.groupBox2.Controls.Add(this.label2);
            this.groupBox2.Controls.Add(this.label1);
            this.groupBox2.Controls.Add(this.txtInformation);
            this.groupBox2.Controls.Add(this.txtCompare);
            this.groupBox2.Controls.Add(this.txtReceived);
            this.groupBox2.Controls.Add(this.txtSend);
            this.groupBox2.Location = new System.Drawing.Point(604, 12);
            this.groupBox2.Name = "groupBox2";
            this.groupBox2.Size = new System.Drawing.Size(391, 809);
            this.groupBox2.TabIndex = 42;
            this.groupBox2.TabStop = false;
            // 
            // btnWorkInfo
            // 
            this.btnWorkInfo.Font = new System.Drawing.Font("PMingLiU", 12F);
            this.btnWorkInfo.Location = new System.Drawing.Point(275, 586);
            this.btnWorkInfo.Name = "btnWorkInfo";
            this.btnWorkInfo.Size = new System.Drawing.Size(104, 32);
            this.btnWorkInfo.TabIndex = 6;
            this.btnWorkInfo.Text = "Clear";
            this.btnWorkInfo.UseVisualStyleBackColor = true;
            this.btnWorkInfo.Click += new System.EventHandler(this.btnWorkInfo_Click);
            // 
            // label13
            // 
            this.label13.Anchor = System.Windows.Forms.AnchorStyles.None;
            this.label13.AutoSize = true;
            this.label13.Font = new System.Drawing.Font("PMingLiU", 12F);
            this.label13.Location = new System.Drawing.Point(6, 588);
            this.label13.Name = "label13";
            this.label13.Size = new System.Drawing.Size(145, 16);
            this.label13.TabIndex = 4;
            this.label13.Text = "Working Information:";
            // 
            // label3
            // 
            this.label3.Anchor = System.Windows.Forms.AnchorStyles.None;
            this.label3.AutoSize = true;
            this.label3.Font = new System.Drawing.Font("PMingLiU", 12F);
            this.label3.Location = new System.Drawing.Point(6, 395);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(209, 16);
            this.label3.TabIndex = 4;
            this.label3.Text = "New Label Barcode Infomation:";
            // 
            // label2
            // 
            this.label2.Anchor = System.Windows.Forms.AnchorStyles.None;
            this.label2.AutoSize = true;
            this.label2.Font = new System.Drawing.Font("PMingLiU", 12F);
            this.label2.Location = new System.Drawing.Point(6, 200);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(148, 16);
            this.label2.TabIndex = 3;
            this.label2.Text = "Infomation from SFIS:";
            // 
            // label1
            // 
            this.label1.Anchor = System.Windows.Forms.AnchorStyles.None;
            this.label1.AutoSize = true;
            this.label1.Font = new System.Drawing.Font("PMingLiU", 12F);
            this.label1.Location = new System.Drawing.Point(6, 24);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(192, 16);
            this.label1.TabIndex = 3;
            this.label1.Text = "Original Barcode Infomation:";
            // 
            // txtInformation
            // 
            this.txtInformation.Anchor = System.Windows.Forms.AnchorStyles.None;
            this.txtInformation.Font = new System.Drawing.Font("PMingLiU", 9F);
            this.txtInformation.Location = new System.Drawing.Point(9, 617);
            this.txtInformation.MaxLength = 65536;
            this.txtInformation.Multiline = true;
            this.txtInformation.Name = "txtInformation";
            this.txtInformation.ReadOnly = true;
            this.txtInformation.ScrollBars = System.Windows.Forms.ScrollBars.Vertical;
            this.txtInformation.Size = new System.Drawing.Size(370, 179);
            this.txtInformation.TabIndex = 2;
            // 
            // txtCompare
            // 
            this.txtCompare.Anchor = System.Windows.Forms.AnchorStyles.None;
            this.txtCompare.Font = new System.Drawing.Font("PMingLiU", 12F);
            this.txtCompare.Location = new System.Drawing.Point(9, 420);
            this.txtCompare.Multiline = true;
            this.txtCompare.Name = "txtCompare";
            this.txtCompare.Size = new System.Drawing.Size(370, 140);
            this.txtCompare.TabIndex = 2;
            // 
            // txtReceived
            // 
            this.txtReceived.Anchor = System.Windows.Forms.AnchorStyles.None;
            this.txtReceived.Font = new System.Drawing.Font("PMingLiU", 12F);
            this.txtReceived.Location = new System.Drawing.Point(9, 223);
            this.txtReceived.Multiline = true;
            this.txtReceived.Name = "txtReceived";
            this.txtReceived.Size = new System.Drawing.Size(370, 143);
            this.txtReceived.TabIndex = 2;
            // 
            // txtSend
            // 
            this.txtSend.Anchor = System.Windows.Forms.AnchorStyles.None;
            this.txtSend.Font = new System.Drawing.Font("PMingLiU", 12F);
            this.txtSend.Location = new System.Drawing.Point(9, 46);
            this.txtSend.Multiline = true;
            this.txtSend.Name = "txtSend";
            this.txtSend.Size = new System.Drawing.Size(372, 133);
            this.txtSend.TabIndex = 2;
            // 
            // tabControl1
            // 
            this.tabControl1.Controls.Add(this.tabPage1);
            this.tabControl1.Controls.Add(this.tabPage2);
            this.tabControl1.Font = new System.Drawing.Font("PMingLiU", 9F);
            this.tabControl1.ItemSize = new System.Drawing.Size(130, 10);
            this.tabControl1.Location = new System.Drawing.Point(1010, 2);
            this.tabControl1.Name = "tabControl1";
            this.tabControl1.SelectedIndex = 0;
            this.tabControl1.Size = new System.Drawing.Size(351, 640);
            this.tabControl1.SizeMode = System.Windows.Forms.TabSizeMode.Fixed;
            this.tabControl1.TabIndex = 43;
            // 
            // tabPage1
            // 
            this.tabPage1.BackColor = System.Drawing.SystemColors.GradientActiveCaption;
            this.tabPage1.Controls.Add(this.label5);
            this.tabPage1.Controls.Add(this.cbxProgram);
            this.tabPage1.Controls.Add(this.btnStop);
            this.tabPage1.Controls.Add(this.btnAnalysis2);
            this.tabPage1.Controls.Add(this.btnAnalysis1);
            this.tabPage1.Controls.Add(this.btnSnap2);
            this.tabPage1.Controls.Add(this.btnSnap1);
            this.tabPage1.Controls.Add(this.btnRun);
            this.tabPage1.Font = new System.Drawing.Font("PMingLiU", 9F);
            this.tabPage1.Location = new System.Drawing.Point(4, 14);
            this.tabPage1.Name = "tabPage1";
            this.tabPage1.Padding = new System.Windows.Forms.Padding(3);
            this.tabPage1.Size = new System.Drawing.Size(343, 622);
            this.tabPage1.TabIndex = 0;
            this.tabPage1.Text = "tabPage1";
            // 
            // label5
            // 
            this.label5.AutoSize = true;
            this.label5.Font = new System.Drawing.Font("PMingLiU", 16F);
            this.label5.Location = new System.Drawing.Point(36, 45);
            this.label5.Name = "label5";
            this.label5.Size = new System.Drawing.Size(86, 22);
            this.label5.TabIndex = 47;
            this.label5.Text = "Program:";
            // 
            // cbxProgram
            // 
            this.cbxProgram.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cbxProgram.Font = new System.Drawing.Font("PMingLiU", 16F);
            this.cbxProgram.FormattingEnabled = true;
            this.cbxProgram.ItemHeight = 21;
            this.cbxProgram.Location = new System.Drawing.Point(128, 42);
            this.cbxProgram.Name = "cbxProgram";
            this.cbxProgram.Size = new System.Drawing.Size(160, 29);
            this.cbxProgram.TabIndex = 46;
            this.cbxProgram.DropDown += new System.EventHandler(this.cbxProgram_DropDown);
            this.cbxProgram.SelectedIndexChanged += new System.EventHandler(this.cbxProgram_SelectedIndexChanged);
            // 
            // btnStop
            // 
            this.btnStop.Font = new System.Drawing.Font("PMingLiU", 18F);
            this.btnStop.Location = new System.Drawing.Point(35, 171);
            this.btnStop.Name = "btnStop";
            this.btnStop.Size = new System.Drawing.Size(253, 64);
            this.btnStop.TabIndex = 43;
            this.btnStop.Text = "S T O P";
            this.btnStop.UseVisualStyleBackColor = true;
            this.btnStop.Click += new System.EventHandler(this.btnStop_Click);
            // 
            // btnAnalysis2
            // 
            this.btnAnalysis2.Font = new System.Drawing.Font("PMingLiU", 12F);
            this.btnAnalysis2.Location = new System.Drawing.Point(189, 414);
            this.btnAnalysis2.Name = "btnAnalysis2";
            this.btnAnalysis2.Size = new System.Drawing.Size(125, 59);
            this.btnAnalysis2.TabIndex = 44;
            this.btnAnalysis2.Text = "ANALYSIS2";
            this.btnAnalysis2.UseVisualStyleBackColor = true;
            this.btnAnalysis2.Click += new System.EventHandler(this.btnAnalysis2_Click);
            // 
            // btnAnalysis1
            // 
            this.btnAnalysis1.Font = new System.Drawing.Font("PMingLiU", 12F);
            this.btnAnalysis1.Location = new System.Drawing.Point(189, 328);
            this.btnAnalysis1.Name = "btnAnalysis1";
            this.btnAnalysis1.Size = new System.Drawing.Size(125, 60);
            this.btnAnalysis1.TabIndex = 44;
            this.btnAnalysis1.Text = "ANALYSIS1";
            this.btnAnalysis1.UseVisualStyleBackColor = true;
            this.btnAnalysis1.Click += new System.EventHandler(this.btnAnalysis1_Click);
            // 
            // btnSnap2
            // 
            this.btnSnap2.Font = new System.Drawing.Font("PMingLiU", 18F);
            this.btnSnap2.Location = new System.Drawing.Point(35, 414);
            this.btnSnap2.Name = "btnSnap2";
            this.btnSnap2.Size = new System.Drawing.Size(133, 59);
            this.btnSnap2.TabIndex = 44;
            this.btnSnap2.Text = "SNAP2";
            this.btnSnap2.UseVisualStyleBackColor = true;
            this.btnSnap2.Click += new System.EventHandler(this.btnSnap2_Click);
            // 
            // btnSnap1
            // 
            this.btnSnap1.Font = new System.Drawing.Font("PMingLiU", 18F);
            this.btnSnap1.Location = new System.Drawing.Point(35, 327);
            this.btnSnap1.Name = "btnSnap1";
            this.btnSnap1.Size = new System.Drawing.Size(133, 61);
            this.btnSnap1.TabIndex = 44;
            this.btnSnap1.Text = "SNAP1";
            this.btnSnap1.UseVisualStyleBackColor = true;
            this.btnSnap1.Click += new System.EventHandler(this.btnSnap1_Click);
            // 
            // btnRun
            // 
            this.btnRun.BackColor = System.Drawing.SystemColors.Control;
            this.btnRun.Font = new System.Drawing.Font("PMingLiU", 18F);
            this.btnRun.Location = new System.Drawing.Point(35, 101);
            this.btnRun.Name = "btnRun";
            this.btnRun.Size = new System.Drawing.Size(253, 64);
            this.btnRun.TabIndex = 45;
            this.btnRun.Text = "R U N";
            this.btnRun.UseVisualStyleBackColor = false;
            this.btnRun.Click += new System.EventHandler(this.btnRun_Click);
            // 
            // tabPage2
            // 
            this.tabPage2.BackColor = System.Drawing.SystemColors.GradientInactiveCaption;
            this.tabPage2.Controls.Add(this.btnSave);
            this.tabPage2.Controls.Add(this.groupBox4);
            this.tabPage2.Controls.Add(this.groupBox1);
            this.tabPage2.Controls.Add(this.groupBox3);
            this.tabPage2.Font = new System.Drawing.Font("PMingLiU", 9F);
            this.tabPage2.Location = new System.Drawing.Point(4, 14);
            this.tabPage2.Name = "tabPage2";
            this.tabPage2.Padding = new System.Windows.Forms.Padding(3);
            this.tabPage2.Size = new System.Drawing.Size(343, 622);
            this.tabPage2.TabIndex = 1;
            this.tabPage2.Text = "tabPage2";
            // 
            // btnSave
            // 
            this.btnSave.Font = new System.Drawing.Font("PMingLiU", 16F);
            this.btnSave.Location = new System.Drawing.Point(204, 562);
            this.btnSave.Name = "btnSave";
            this.btnSave.Size = new System.Drawing.Size(125, 52);
            this.btnSave.TabIndex = 46;
            this.btnSave.Text = "Save";
            this.btnSave.UseVisualStyleBackColor = true;
            this.btnSave.Click += new System.EventHandler(this.btnSave_Click);
            // 
            // groupBox4
            // 
            this.groupBox4.BackColor = System.Drawing.SystemColors.Control;
            this.groupBox4.Controls.Add(this.txtCameraID);
            this.groupBox4.Controls.Add(this.label11);
            this.groupBox4.Font = new System.Drawing.Font("PMingLiU", 12F);
            this.groupBox4.Location = new System.Drawing.Point(28, 25);
            this.groupBox4.Name = "groupBox4";
            this.groupBox4.Size = new System.Drawing.Size(301, 72);
            this.groupBox4.TabIndex = 45;
            this.groupBox4.TabStop = false;
            this.groupBox4.Text = "Camera Settings:";
            // 
            // txtCameraID
            // 
            this.txtCameraID.Anchor = System.Windows.Forms.AnchorStyles.None;
            this.txtCameraID.Location = new System.Drawing.Point(167, 30);
            this.txtCameraID.Name = "txtCameraID";
            this.txtCameraID.Size = new System.Drawing.Size(74, 27);
            this.txtCameraID.TabIndex = 44;
            this.txtCameraID.Text = "USER_ID";
            // 
            // label11
            // 
            this.label11.Anchor = System.Windows.Forms.AnchorStyles.None;
            this.label11.AutoSize = true;
            this.label11.Location = new System.Drawing.Point(63, 33);
            this.label11.Name = "label11";
            this.label11.Size = new System.Drawing.Size(80, 16);
            this.label11.TabIndex = 43;
            this.label11.Text = "Camera ID:";
            // 
            // groupBox1
            // 
            this.groupBox1.BackColor = System.Drawing.SystemColors.Control;
            this.groupBox1.Controls.Add(this.txtPort);
            this.groupBox1.Controls.Add(this.txtIP);
            this.groupBox1.Controls.Add(this.label10);
            this.groupBox1.Controls.Add(this.label9);
            this.groupBox1.Font = new System.Drawing.Font("PMingLiU", 12F);
            this.groupBox1.Location = new System.Drawing.Point(28, 103);
            this.groupBox1.Name = "groupBox1";
            this.groupBox1.Size = new System.Drawing.Size(301, 176);
            this.groupBox1.TabIndex = 42;
            this.groupBox1.TabStop = false;
            this.groupBox1.Text = "IP Setting:";
            // 
            // txtPort
            // 
            this.txtPort.Anchor = System.Windows.Forms.AnchorStyles.None;
            this.txtPort.Location = new System.Drawing.Point(141, 81);
            this.txtPort.Name = "txtPort";
            this.txtPort.Size = new System.Drawing.Size(100, 27);
            this.txtPort.TabIndex = 39;
            this.txtPort.Text = "5888";
            // 
            // txtIP
            // 
            this.txtIP.Anchor = System.Windows.Forms.AnchorStyles.None;
            this.txtIP.Location = new System.Drawing.Point(141, 48);
            this.txtIP.Name = "txtIP";
            this.txtIP.Size = new System.Drawing.Size(100, 27);
            this.txtIP.TabIndex = 39;
            this.txtIP.Text = "169.254.11.111";
            // 
            // label10
            // 
            this.label10.Anchor = System.Windows.Forms.AnchorStyles.None;
            this.label10.AutoSize = true;
            this.label10.Location = new System.Drawing.Point(85, 89);
            this.label10.Name = "label10";
            this.label10.Size = new System.Drawing.Size(50, 16);
            this.label10.TabIndex = 38;
            this.label10.Text = "PORT:";
            // 
            // label9
            // 
            this.label9.Anchor = System.Windows.Forms.AnchorStyles.None;
            this.label9.AutoSize = true;
            this.label9.Location = new System.Drawing.Point(85, 52);
            this.label9.Name = "label9";
            this.label9.Size = new System.Drawing.Size(25, 16);
            this.label9.TabIndex = 38;
            this.label9.Text = "IP:";
            // 
            // groupBox3
            // 
            this.groupBox3.BackColor = System.Drawing.SystemColors.Control;
            this.groupBox3.Controls.Add(this.cbxStopbits);
            this.groupBox3.Controls.Add(this.cbxParity);
            this.groupBox3.Controls.Add(this.cbxDataBits);
            this.groupBox3.Controls.Add(this.cbxBaudRate);
            this.groupBox3.Controls.Add(this.cbxPortName);
            this.groupBox3.Controls.Add(this.label4);
            this.groupBox3.Controls.Add(this.label6);
            this.groupBox3.Controls.Add(this.label7);
            this.groupBox3.Controls.Add(this.label8);
            this.groupBox3.Controls.Add(this.label12);
            this.groupBox3.Font = new System.Drawing.Font("PMingLiU", 12F);
            this.groupBox3.Location = new System.Drawing.Point(28, 294);
            this.groupBox3.Name = "groupBox3";
            this.groupBox3.Size = new System.Drawing.Size(301, 262);
            this.groupBox3.TabIndex = 41;
            this.groupBox3.TabStop = false;
            this.groupBox3.Text = "SerialPort Setting";
            // 
            // cbxStopbits
            // 
            this.cbxStopbits.Anchor = System.Windows.Forms.AnchorStyles.None;
            this.cbxStopbits.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cbxStopbits.FormattingEnabled = true;
            this.cbxStopbits.Items.AddRange(new object[] {
            "One",
            "Two",
            "OnePointFive"});
            this.cbxStopbits.Location = new System.Drawing.Point(167, 206);
            this.cbxStopbits.Name = "cbxStopbits";
            this.cbxStopbits.Size = new System.Drawing.Size(74, 24);
            this.cbxStopbits.TabIndex = 35;
            // 
            // cbxParity
            // 
            this.cbxParity.Anchor = System.Windows.Forms.AnchorStyles.None;
            this.cbxParity.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cbxParity.FormattingEnabled = true;
            this.cbxParity.Items.AddRange(new object[] {
            "None",
            "Odd",
            "Even",
            "Mark",
            "Space"});
            this.cbxParity.Location = new System.Drawing.Point(167, 171);
            this.cbxParity.Name = "cbxParity";
            this.cbxParity.Size = new System.Drawing.Size(74, 24);
            this.cbxParity.TabIndex = 34;
            // 
            // cbxDataBits
            // 
            this.cbxDataBits.Anchor = System.Windows.Forms.AnchorStyles.None;
            this.cbxDataBits.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cbxDataBits.FormattingEnabled = true;
            this.cbxDataBits.Items.AddRange(new object[] {
            "7",
            "8"});
            this.cbxDataBits.Location = new System.Drawing.Point(167, 130);
            this.cbxDataBits.Name = "cbxDataBits";
            this.cbxDataBits.Size = new System.Drawing.Size(74, 24);
            this.cbxDataBits.TabIndex = 33;
            // 
            // cbxBaudRate
            // 
            this.cbxBaudRate.Anchor = System.Windows.Forms.AnchorStyles.None;
            this.cbxBaudRate.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cbxBaudRate.FormattingEnabled = true;
            this.cbxBaudRate.Items.AddRange(new object[] {
            "300",
            "600",
            "1200",
            "2400",
            "4800",
            "9600",
            "19200"});
            this.cbxBaudRate.Location = new System.Drawing.Point(167, 92);
            this.cbxBaudRate.Name = "cbxBaudRate";
            this.cbxBaudRate.Size = new System.Drawing.Size(74, 24);
            this.cbxBaudRate.TabIndex = 32;
            // 
            // cbxPortName
            // 
            this.cbxPortName.Anchor = System.Windows.Forms.AnchorStyles.None;
            this.cbxPortName.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cbxPortName.FormattingEnabled = true;
            this.cbxPortName.Location = new System.Drawing.Point(167, 52);
            this.cbxPortName.Name = "cbxPortName";
            this.cbxPortName.Size = new System.Drawing.Size(74, 24);
            this.cbxPortName.TabIndex = 31;
            this.cbxPortName.DropDown += new System.EventHandler(this.cbxPortName_DropDown);
            // 
            // label4
            // 
            this.label4.Anchor = System.Windows.Forms.AnchorStyles.None;
            this.label4.AutoSize = true;
            this.label4.Location = new System.Drawing.Point(82, 209);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(64, 16);
            this.label4.TabIndex = 30;
            this.label4.Text = "StopBits:";
            // 
            // label6
            // 
            this.label6.Anchor = System.Windows.Forms.AnchorStyles.None;
            this.label6.AutoSize = true;
            this.label6.Location = new System.Drawing.Point(82, 174);
            this.label6.Name = "label6";
            this.label6.Size = new System.Drawing.Size(48, 16);
            this.label6.TabIndex = 29;
            this.label6.Text = "Parity:";
            // 
            // label7
            // 
            this.label7.Anchor = System.Windows.Forms.AnchorStyles.None;
            this.label7.AutoSize = true;
            this.label7.Location = new System.Drawing.Point(82, 133);
            this.label7.Name = "label7";
            this.label7.Size = new System.Drawing.Size(65, 16);
            this.label7.TabIndex = 28;
            this.label7.Text = "DataBits:";
            // 
            // label8
            // 
            this.label8.Anchor = System.Windows.Forms.AnchorStyles.None;
            this.label8.AutoSize = true;
            this.label8.Location = new System.Drawing.Point(82, 94);
            this.label8.Name = "label8";
            this.label8.Size = new System.Drawing.Size(73, 16);
            this.label8.TabIndex = 27;
            this.label8.Text = "BaudRate:";
            // 
            // label12
            // 
            this.label12.Anchor = System.Windows.Forms.AnchorStyles.None;
            this.label12.AutoSize = true;
            this.label12.Location = new System.Drawing.Point(82, 55);
            this.label12.Name = "label12";
            this.label12.Size = new System.Drawing.Size(78, 16);
            this.label12.TabIndex = 26;
            this.label12.Text = "Port Name:";
            // 
            // btnMain
            // 
            this.btnMain.Font = new System.Drawing.Font("PMingLiU", 16F);
            this.btnMain.Location = new System.Drawing.Point(1353, 15);
            this.btnMain.Name = "btnMain";
            this.btnMain.Size = new System.Drawing.Size(130, 58);
            this.btnMain.TabIndex = 1;
            this.btnMain.Text = "MAIN";
            this.btnMain.UseVisualStyleBackColor = true;
            this.btnMain.Click += new System.EventHandler(this.btnMain_Click);
            // 
            // btnSettings
            // 
            this.btnSettings.Font = new System.Drawing.Font("PMingLiU", 12F);
            this.btnSettings.Location = new System.Drawing.Point(1353, 74);
            this.btnSettings.Name = "btnSettings";
            this.btnSettings.Size = new System.Drawing.Size(131, 63);
            this.btnSettings.TabIndex = 1;
            this.btnSettings.Text = "SETTINGS";
            this.btnSettings.UseVisualStyleBackColor = true;
            this.btnSettings.Click += new System.EventHandler(this.btnSettings_Click);
            // 
            // btnExit
            // 
            this.btnExit.Font = new System.Drawing.Font("PMingLiU", 14F);
            this.btnExit.Location = new System.Drawing.Point(1353, 202);
            this.btnExit.Name = "btnExit";
            this.btnExit.Size = new System.Drawing.Size(130, 57);
            this.btnExit.TabIndex = 1;
            this.btnExit.Text = "EXIT";
            this.btnExit.UseVisualStyleBackColor = true;
            this.btnExit.Click += new System.EventHandler(this.btnExit_Click);
            // 
            // serialPort1
            // 
            this.serialPort1.DataBits = 7;
            this.serialPort1.Parity = System.IO.Ports.Parity.Even;
            this.serialPort1.DataReceived += new System.IO.Ports.SerialDataReceivedEventHandler(this.serialPort1_DataReceived);
            // 
            // lblSFIS
            // 
            this.lblSFIS.BackColor = System.Drawing.Color.Green;
            this.lblSFIS.BorderStyle = System.Windows.Forms.BorderStyle.Fixed3D;
            this.lblSFIS.Font = new System.Drawing.Font("PMingLiU", 14.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(136)));
            this.lblSFIS.Location = new System.Drawing.Point(1010, 648);
            this.lblSFIS.Name = "lblSFIS";
            this.lblSFIS.Size = new System.Drawing.Size(351, 55);
            this.lblSFIS.TabIndex = 44;
            this.lblSFIS.Text = "SFIS Communication";
            this.lblSFIS.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // lblSerialPort
            // 
            this.lblSerialPort.BackColor = System.Drawing.Color.Green;
            this.lblSerialPort.BorderStyle = System.Windows.Forms.BorderStyle.Fixed3D;
            this.lblSerialPort.Font = new System.Drawing.Font("PMingLiU", 14.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(136)));
            this.lblSerialPort.Location = new System.Drawing.Point(1011, 708);
            this.lblSerialPort.Name = "lblSerialPort";
            this.lblSerialPort.Size = new System.Drawing.Size(350, 55);
            this.lblSerialPort.TabIndex = 44;
            this.lblSerialPort.Text = "Serial Port Communication";
            this.lblSerialPort.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // lblCamera
            // 
            this.lblCamera.BackColor = System.Drawing.Color.Green;
            this.lblCamera.BorderStyle = System.Windows.Forms.BorderStyle.Fixed3D;
            this.lblCamera.Font = new System.Drawing.Font("PMingLiU", 14.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(136)));
            this.lblCamera.Location = new System.Drawing.Point(1011, 766);
            this.lblCamera.Name = "lblCamera";
            this.lblCamera.Size = new System.Drawing.Size(350, 55);
            this.lblCamera.TabIndex = 44;
            this.lblCamera.Text = "Camera Communition";
            this.lblCamera.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // lblResult1
            // 
            this.lblResult1.BackColor = System.Drawing.Color.Red;
            this.lblResult1.Font = new System.Drawing.Font("PMingLiU", 14F);
            this.lblResult1.Location = new System.Drawing.Point(508, 12);
            this.lblResult1.Name = "lblResult1";
            this.lblResult1.Size = new System.Drawing.Size(75, 41);
            this.lblResult1.TabIndex = 45;
            this.lblResult1.Text = "FAIL";
            this.lblResult1.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // lblResult2
            // 
            this.lblResult2.BackColor = System.Drawing.Color.Red;
            this.lblResult2.Font = new System.Drawing.Font("PMingLiU", 14F);
            this.lblResult2.Location = new System.Drawing.Point(508, 428);
            this.lblResult2.Name = "lblResult2";
            this.lblResult2.Size = new System.Drawing.Size(75, 41);
            this.lblResult2.TabIndex = 45;
            this.lblResult2.Text = "FAIL";
            this.lblResult2.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // pictureBox1
            // 
            this.pictureBox1.Image = ((System.Drawing.Image)(resources.GetObject("pictureBox1.Image")));
            this.pictureBox1.Location = new System.Drawing.Point(1367, 766);
            this.pictureBox1.Name = "pictureBox1";
            this.pictureBox1.Size = new System.Drawing.Size(114, 55);
            this.pictureBox1.TabIndex = 69;
            this.pictureBox1.TabStop = false;
            // 
            // pictureBox2
            // 
            this.pictureBox2.Image = ((System.Drawing.Image)(resources.GetObject("pictureBox2.Image")));
            this.pictureBox2.Location = new System.Drawing.Point(1363, 265);
            this.pictureBox2.Name = "pictureBox2";
            this.pictureBox2.Size = new System.Drawing.Size(118, 498);
            this.pictureBox2.SizeMode = System.Windows.Forms.PictureBoxSizeMode.StretchImage;
            this.pictureBox2.TabIndex = 70;
            this.pictureBox2.TabStop = false;
            // 
            // Main
            // 
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.None;
            this.AutoSize = true;
            this.ClientSize = new System.Drawing.Size(1444, 837);
            this.Controls.Add(this.pictureBox1);
            this.Controls.Add(this.lblResult2);
            this.Controls.Add(this.lblResult1);
            this.Controls.Add(this.lblCamera);
            this.Controls.Add(this.lblSerialPort);
            this.Controls.Add(this.lblSFIS);
            this.Controls.Add(this.btnSettings);
            this.Controls.Add(this.btnMain);
            this.Controls.Add(this.btnExit);
            this.Controls.Add(this.btnProgramming);
            this.Controls.Add(this.tabControl1);
            this.Controls.Add(this.groupBox2);
            this.Controls.Add(this.imageViewer2);
            this.Controls.Add(this.imageViewer1);
            this.Controls.Add(this.pictureBox2);
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.MaximizeBox = false;
            this.Name = "Main";
            this.Text = "BarcodeVisualIdentification__ShunSinTech";
            this.FormClosing += new System.Windows.Forms.FormClosingEventHandler(this.Main_FormClosing);
            this.Load += new System.EventHandler(this.Main_Load);
            this.groupBox2.ResumeLayout(false);
            this.groupBox2.PerformLayout();
            this.tabControl1.ResumeLayout(false);
            this.tabPage1.ResumeLayout(false);
            this.tabPage1.PerformLayout();
            this.tabPage2.ResumeLayout(false);
            this.groupBox4.ResumeLayout(false);
            this.groupBox4.PerformLayout();
            this.groupBox1.ResumeLayout(false);
            this.groupBox1.PerformLayout();
            this.groupBox3.ResumeLayout(false);
            this.groupBox3.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.pictureBox1)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.pictureBox2)).EndInit();
            this.ResumeLayout(false);

        }

        #endregion

        private NationalInstruments.Vision.WindowsForms.ImageViewer imageViewer1;
        private NationalInstruments.Vision.WindowsForms.ImageViewer imageViewer2;
        private System.Windows.Forms.Button btnProgramming;
        private System.Windows.Forms.GroupBox groupBox2;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.TextBox txtCompare;
        private System.Windows.Forms.TextBox txtReceived;
        private System.Windows.Forms.TextBox txtSend;
        private System.Windows.Forms.TabControl tabControl1;
        private System.Windows.Forms.TabPage tabPage1;
        private System.Windows.Forms.TabPage tabPage2;
        private System.Windows.Forms.Button btnMain;
        private System.Windows.Forms.Button btnSettings;
        private System.Windows.Forms.Button btnExit;
        private System.Windows.Forms.Button btnStop;
        private System.Windows.Forms.Button btnSnap1;
        private System.Windows.Forms.Button btnRun;
        private System.Windows.Forms.Button btnAnalysis2;
        private System.Windows.Forms.Button btnAnalysis1;
        private System.Windows.Forms.Button btnSnap2;
        private System.Windows.Forms.Label label5;
        private System.Windows.Forms.ComboBox cbxProgram;
        private System.Windows.Forms.TextBox txtCameraID;
        private System.Windows.Forms.Label label11;
        private System.Windows.Forms.GroupBox groupBox1;
        private System.Windows.Forms.TextBox txtPort;
        private System.Windows.Forms.TextBox txtIP;
        private System.Windows.Forms.Label label10;
        private System.Windows.Forms.Label label9;
        private System.Windows.Forms.GroupBox groupBox3;
        private System.Windows.Forms.ComboBox cbxStopbits;
        private System.Windows.Forms.ComboBox cbxParity;
        private System.Windows.Forms.ComboBox cbxDataBits;
        private System.Windows.Forms.ComboBox cbxBaudRate;
        private System.Windows.Forms.ComboBox cbxPortName;
        private System.Windows.Forms.Label label4;
        private System.Windows.Forms.Label label6;
        private System.Windows.Forms.Label label7;
        private System.Windows.Forms.Label label8;
        private System.Windows.Forms.Label label12;
        private System.Windows.Forms.GroupBox groupBox4;
        private System.Windows.Forms.Button btnSave;
        private System.IO.Ports.SerialPort serialPort1;
        private System.Windows.Forms.Label label13;
        private System.Windows.Forms.Button btnWorkInfo;
        private System.Windows.Forms.Label lblSFIS;
        private System.Windows.Forms.Label lblSerialPort;
        private System.Windows.Forms.Label lblCamera;
        public System.Windows.Forms.Label lblResult1;
        public System.Windows.Forms.Label lblResult2;
        private System.Windows.Forms.TextBox txtInformation;
        private System.Windows.Forms.PictureBox pictureBox1;
        private System.Windows.Forms.PictureBox pictureBox2;
    }
}

