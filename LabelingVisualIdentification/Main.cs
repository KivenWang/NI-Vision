using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using NationalInstruments.Vision;
using NationalInstruments.Vision.Analysis;
using NationalInstruments.Vision.Acquisition.Imaqdx;
using NationalInstruments.Vision.WindowsForms;
using System.IO;
using System.IO.Ports;
using System.Threading;
using System.Net.Sockets;
using System.Net;
using log4net;
using System.Configuration;
namespace LabelingVisualIdentification
{
    public partial class Main : Form
    {
        public Main()
        {
            InitializeComponent();
        }

        private static readonly ILog logger = LogManager.GetLogger("Product");
        bool run = false;
        Socket socketSend;
        private System.Timers.Timer timerPLC = new System.Timers.Timer(1000);
        //PLC communication ASCII code
        string queryPLC = "3A30313033313130303030303145410D0A";
        const string snap1 = "3A3031303330323030303146390D0A";
        const string snap2 = "3A3031303330323030303246380D0A";
        const string snap0 = "3A3031303330323030303046410D0A";
        string success1 = "3A30313036313230303030303145360D0A";
        string success2 = "3A30313036313230303030303345340D0A";
        string failure1 = "3A30313036313230303030303245350D0A";
        string failure2 = "3A30313036313230303030303445330D0A";
        string SFISCodeInfo = "";
        delegate void SHOWMESSAGE(string sr);
        delegate void SHOWINFORMATION(string info);
        public static string txtAnalysis1 = "";
        public static string txtAnalysis2 = "";

        #region  Form controls
        private void Main_Load(object sender, EventArgs e)
        {
            ////new tabcontrol1 region
            //this.tabControl1.Region = new Region(new RectangleF(this.tabPage1.Left, this.tabPage1.Top, this.tabPage1.Width, this.tabPage1.Height));
            ////Get current directory path
            //Common.path = Environment.CurrentDirectory;
            //if (!(Directory.Exists(Common.path + "\\Programming\\template")))
            //{
            //    Directory.CreateDirectory(Common.path + "\\Programming\\template");
            //}
            //TcpConnect();
            //if (!IsDebug ())
            //{

            //    Thread timerTh = new Thread(timer);
            //    timerTh.IsBackground = true;
            //    timerTh.Start();

            //    try
            //    {
            //        Common.cameraID = txtCameraID.Text;
            //        Common.session = new ImaqdxSession(txtCameraID.Text);
            //    }
            //    catch (Exception ex)
            //    {
            //        txtInformation.AppendText(DateTime.Now.ToString() + ": New imaqdx session error when form load! " + ex.Message + "\r\n");
            //        lblCamera.BackColor = Color.Red;

            //    }
            //}

            var userProgram = ConfigManager.UserPrograms1.UserProgram;
            txtInformation.AppendText(userProgram[0].BarcodeFormat);

        }

        private void btnProgramming_Click(object sender, EventArgs e)
        {

            Programming fr = Programming.GetSingle();
            fr.Show();
            fr.Activate();
        }


        private void btnMain_Click(object sender, EventArgs e)
        {
            this.tabControl1.SelectedIndex = 0;
        }

        private void btnSettings_Click(object sender, EventArgs e)
        {
            this.tabControl1.SelectedIndex = 1;
        }

        private void btnExit_Click(object sender, EventArgs e)
        {
            Application.Exit();
            //this.Dispose();
        }

        private void btnRun_Click(object sender, EventArgs e)
        {
            if (lblSFIS.BackColor == Color.Green | lblSerialPort.BackColor == Color.Green | lblCamera.BackColor == Color.Green)
            {
                if (cbxProgram.Text != "")
                {
                    cbxProgram.Enabled = false;
                    txtInformation.AppendText(DateTime.Now.ToString() + Common.templatePath + "\r\n");
                    btnRun.BackColor = Color.LightGreen;
                    run = true;
                    timerPLC.Enabled = true;
                    btnRun.Enabled = false;
                    btnRun.Text = "RUNNING";
                    txtInformation.AppendText(DateTime.Now.ToString() + ": Start run program: " + cbxProgram.Text + "\r\n");
                }
                else
                {
                    MessageBox.Show("Please select a programming which you need!");
                }

            }
        }

        private void btnStop_Click(object sender, EventArgs e)
        {
            timerPLC.Enabled = false;
            btnRun.BackColor = SystemColors.Control;
            run = false;
            btnRun.Enabled = true;
            btnRun.Text = "RUN";
            cbxProgram.Enabled = true;
            txtInformation.AppendText(DateTime.Now.ToString() + ": Stop program: " + cbxProgram.Text + "\r\n");
        }

        private void cbxProgram_DropDown(object sender, EventArgs e)
        {
            if (!run)
            {
                cbxProgram.Items.Clear();
                DirectoryInfo dti = new DirectoryInfo(Common.path + "\\Programming");
                FileInfo[] name = dti.GetFiles();

                foreach (FileInfo dif in name)
                {
                    cbxProgram.Items.Add(dif);

                }
            }
            else
            {
                MessageBox.Show("Please stop the program before selecting program!");
            }
        }

        private void cbxProgram_SelectedIndexChanged(object sender, EventArgs e)
        {
            try
            {
                ReadProgram(cbxProgram.Text);
            }
            catch (Exception ex)
            {
                MessageBox.Show("Read program config error: " + ex.Message);
            }
        }

        private void btnSnap1_Click(object sender, EventArgs e)
        {
            if (!IsDebug())
            {
                if (run)
                {
                    MessageBox.Show("Please stop it and try again!");
                }
                else
                {
                    try
                    {
                        Common.session.Snap(imageViewer1.Image);
                    }
                    catch (Exception ex)
                    {
                        txtInformation.AppendText(DateTime.Now.ToString() + ": Manual snap1 error!\r\n");
                        MessageBox.Show(ex.Message + "---Manual snap1 error!");

                    }
                }
            }
            else
            {
                ImagePreviewFileDialog imageDialog = new ImagePreviewFileDialog();

                if (imageDialog.ShowDialog() == DialogResult.OK)
                {
                    FileInformation fileinfo = Algorithms.GetFileInformation(imageDialog.FileName);
                    imageViewer1.Image.Type = fileinfo.ImageType;
                    imageViewer1.Image.ReadFile(imageDialog.FileName);
                }
            }
        }
        private void btnWorkInfo_Click(object sender, EventArgs e)
        {
            try
            {
                File.AppendAllText(Common.path + "\\History\\" + DateTime.Now.Month + DateTime.Now.Day, txtInformation.Text, Encoding.UTF8);
                txtInformation.Text = "";
                txtInformation.AppendText(DateTime.Now.ToString() + ": Saved successful!\r\n");
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message + " Write Working Infomation error!");
                txtInformation.AppendText(DateTime.Now.ToString() + ": Saved failure!\r\n");
            }
        }

        private void btnAnalysis1_Click(object sender, EventArgs e)
        {
            if (run)
            {
                MessageBox.Show("Please stop it and try again!");
            }
            else
            {
                Analysis(1);
            }


        }

        private void btnSnap2_Click(object sender, EventArgs e)
        {
            if (!IsDebug())
            {
                if (run)
                {
                    MessageBox.Show("Please stop it and try again!");
                }
                else
                {
                    try
                    {
                        Common.session.Snap(imageViewer2.Image);
                    }
                    catch (Exception ex)
                    {
                        txtInformation.AppendText(DateTime.Now.ToString() + ": Manual snap2 error!\r\n");
                        MessageBox.Show(ex.Message + "---Manual snap2 error!");
                    }
                }
            }
            else
            {
                ImagePreviewFileDialog imageDialog = new ImagePreviewFileDialog();

                if (imageDialog.ShowDialog() == DialogResult.OK)
                {
                    FileInformation fileinfo = Algorithms.GetFileInformation(imageDialog.FileName);
                    imageViewer2.Image.Type = fileinfo.ImageType;
                    imageViewer2.Image.ReadFile(imageDialog.FileName);
                }
            }
        }

        private void btnAnalysis2_Click(object sender, EventArgs e)
        {
            if (run)
            {
                MessageBox.Show("Please stop it and try again!");
            }
            else
            {
                Analysis(2);
            }

        }

        //Settings save button.
        private void btnSave_Click(object sender, EventArgs e)
        {

            if (cbxPortName.Text == "" | cbxParity.Text == "" | cbxDataBits.Text == "" | cbxBaudRate.Text == "" | cbxStopbits.Text == "" | txtCameraID.Text == "" | txtIP.Text == "" | txtPort.Text == "")
            {
                MessageBox.Show("Please confirm the settings!");
            }
            else
            {
                if (serialPort1.IsOpen)
                {
                    serialPort1.Close();
                }
                serialPort1.PortName = cbxPortName.Text;
                serialPort1.BaudRate = int.Parse(cbxBaudRate.Text);
                serialPort1.DataBits = int.Parse(cbxDataBits.Text);
                if (cbxStopbits.Text == "One")
                {
                    serialPort1.StopBits = StopBits.One;
                }
                if (cbxStopbits.Text == "Two")
                {
                    serialPort1.StopBits = StopBits.Two;
                }
                if (cbxStopbits.Text == "OnePointFive")
                {
                    serialPort1.StopBits = StopBits.OnePointFive;
                }
                if (cbxParity.Text == "None")
                {
                    serialPort1.Parity = Parity.None;
                }
                if (cbxParity.Text == "Odd")
                {
                    serialPort1.Parity = Parity.Odd;
                }
                if (cbxParity.Text == "Even")
                {
                    serialPort1.Parity = Parity.Even;
                }
            }
            lblSerialPort.BackColor = Color.Green;
            lblSFIS.BackColor = Color.Green;
            lblCamera.BackColor = Color.Green;

            TcpConnect();
            if (!IsDebug())
            {
                if (Common.session == null)
                {
                    try
                    {
                        Common.session = new ImaqdxSession(txtCameraID.Text);
                    }
                    catch (Exception ex)
                    {
                        txtInformation.AppendText(DateTime.Now.ToString() + ": new imaqdx session error when save configuration!" + ex.Message + "\r\n");
                        lblCamera.BackColor = Color.Red;
                    }
                }
            }
            txtInformation.AppendText(DateTime.Now.ToString() + ": has saved the configuration!\r\n");
        }

        private void cbxPortName_DropDown(object sender, EventArgs e)
        {
            string[] ports = SerialPort.GetPortNames();
            if (ports.Length != cbxPortName.Items.Count)
            {
                Array.Sort(ports);
                cbxPortName.Items.AddRange(ports);
            }
        }
        #endregion

        //Reading the configurations.
        //是否为调试模式？预留调试
        private bool IsDebug()
        {
            string isDebug = ConfigurationManager.AppSettings["Debug"];
            if (isDebug.Equals("true"))
            {
                return true;
            }
            else
            {
                return false;
            }
        }

        //Reading production programming in local.
        //每个程序文件保存该程序的相关参数，自动运行时选择对应程序可省略重复编程的麻烦。
        public void ReadProgram(string name)
        {
            try
            {
                string[] prgConfig = File.ReadAllLines(Common.path + "\\Programming\\" + name, Encoding.UTF8);

                foreach (string s in prgConfig)
                {
                    if (s.StartsWith("TemplatePath"))
                    {
                        string[] temPath = s.Split(';');
                        Common.templatePath = temPath[1];

                    }
                    if (s.StartsWith("BarcodeFormat"))
                    {
                        string[] format = s.Split(':');
                        Common.barcodeFormat = format[1];
                    }
                    if (s.StartsWith("BarcodeNumber"))
                    {
                        string[] numbert = s.Split(':');
                        Common.barcodeNumber = Convert.ToInt32(numbert[1]);
                    }
                    if (s.StartsWith("TemplatePosition"))
                    {
                        string[] position = s.Split(':');
                        string[] positionXY = position[1].Split(',');
                        Common.templatePositionX = Convert.ToDouble(positionXY[0]);
                        Common.templatePositionY = Convert.ToDouble(positionXY[1]);

                    }
                    if (s.StartsWith("BarcodeTypes"))
                    {
                        string[] types = s.Split(':');
                        if (types[1] == "Code128")
                        {
                            Common.barcodeTypes = BarcodeTypes.Code128;
                        }
                        if (types[1] == "Code39")
                        {
                            Common.barcodeTypes = BarcodeTypes.Code39;
                        }

                    }
                    if (s.StartsWith("Barcode1Rectangle"))
                    {
                        string[] rect1 = s.Split(':');
                        string[] bar1 = rect1[1].Split(',');
                        Common.barcode1Left = Convert.ToDouble(bar1[0]);
                        Common.barcode1Top = Convert.ToDouble(bar1[1]);
                        Common.barcode1Width = Convert.ToDouble(bar1[2]);
                        Common.barcode1Height = Convert.ToDouble(bar1[3]);

                    }
                    if (s.StartsWith("Barcode2Rectangle"))
                    {
                        string[] rect2 = s.Split(':');
                        string[] bar2 = rect2[1].Split(',');
                        Common.barcode2Left = Convert.ToDouble(bar2[0]);
                        Common.barcode2Top = Convert.ToDouble(bar2[1]);
                        Common.barcode2Width = Convert.ToDouble(bar2[2]);
                        Common.barcode2Height = Convert.ToDouble(bar2[3]);

                    }
                    if (s.StartsWith("Barcode3Rectangle"))
                    {
                        string[] rect3 = s.Split(':');
                        string[] bar3 = rect3[1].Split(',');
                        Common.barcode3Left = Convert.ToDouble(bar3[0]);
                        Common.barcode3Top = Convert.ToDouble(bar3[1]);
                        Common.barcode3Width = Convert.ToDouble(bar3[2]);
                        Common.barcode3Height = Convert.ToDouble(bar3[3]);

                    }
                    if (s.StartsWith("Barcode4Rectangle"))
                    {
                        string[] rect4 = s.Split(':');
                        string[] bar4 = rect4[1].Split(',');
                        Common.barcode4Left = Convert.ToDouble(bar4[0]);
                        Common.barcode4Top = Convert.ToDouble(bar4[1]);
                        Common.barcode4Width = Convert.ToDouble(bar4[2]);
                        Common.barcode4Height = Convert.ToDouble(bar4[3]);

                    }
                    if (s.StartsWith("Barcode5Rectangle"))
                    {
                        string[] rect5 = s.Split(':');
                        string[] bar5 = rect5[1].Split(',');
                        Common.barcode5Left = Convert.ToDouble(bar5[0]);
                        Common.barcode5Top = Convert.ToDouble(bar5[1]);
                        Common.barcode5Width = Convert.ToDouble(bar5[2]);
                        Common.barcode5Height = Convert.ToDouble(bar5[3]);

                    }
                    if (s.StartsWith("Barcode6Rectangle"))
                    {
                        string[] rect6 = s.Split(':');
                        string[] bar6 = rect6[1].Split(',');
                        Common.barcode6Left = Convert.ToDouble(bar6[0]);
                        Common.barcode6Top = Convert.ToDouble(bar6[1]);
                        Common.barcode6Width = Convert.ToDouble(bar6[2]);
                        Common.barcode6Height = Convert.ToDouble(bar6[3]);

                    }
                    if (s.StartsWith("PatternRectangle"))
                    {
                        string[] rectPattern = s.Split(':');
                        string[] pat = rectPattern[1].Split(',');
                        Common.patternRectLeft = Convert.ToDouble(pat[0]);
                        Common.patternRectTop = Convert.ToDouble(pat[1]);
                        Common.patternRectWidth = Convert.ToDouble(pat[2]);
                        Common.patternRectHeight = Convert.ToDouble(pat[3]);

                    }
                    if (s.StartsWith("DatamatrixRectangle"))
                    {
                        string[] rectDm = s.Split(':');
                        string[] dmrec = rectDm[1].Split(',');
                        Common.dmRectLeft = Convert.ToDouble(dmrec[0]);
                        Common.dmRectTop = Convert.ToDouble(dmrec[1]);
                        Common.dmRectWidth = Convert.ToDouble(dmrec[2]);
                        Common.dmRectHeight = Convert.ToDouble(dmrec[3]);

                    }
                    if (s.StartsWith("QRRectangle"))
                    {
                        string[] rectQR = s.Split(':');
                        string[] qrrec = rectQR[1].Split(',');
                        Common.QRRectLeft = Convert.ToDouble(qrrec[0]);
                        Common.QRRectTop = Convert.ToDouble(qrrec[1]);
                        Common.QRRectWidth = Convert.ToDouble(qrrec[2]);
                        Common.QRRectHeight = Convert.ToDouble(qrrec[3]);

                    }
                    if (s.StartsWith("DatamatrixMatrixSize"))
                    {
                        string[] dmSize = s.Split(':');
                        Common.matrixSize = Convert.ToUInt32(dmSize[1]);
                    }
                    if (s.StartsWith("DatamatrixPolarity"))
                    {
                        string[] dmPolarity = s.Split(':');
                        if (dmPolarity[1].StartsWith("Auto"))
                        {
                            Common.polarity = DataMatrixPolarity.AutoDetect;
                        }
                        if (dmPolarity[1].StartsWith("Black"))
                        {
                            Common.polarity = DataMatrixPolarity.BlackDataOnWhiteBackground;
                        }
                        if (dmPolarity[1].StartsWith("White"))
                        {
                            Common.polarity = DataMatrixPolarity.WhiteDataOnBlackBackground;
                        }
                    }
                    if (s.StartsWith("DatamatrixCellSampleSize"))
                    {
                        string[] dmCellSize = s.Split(':');
                        if (dmCellSize[1].StartsWith("Auto"))
                        {
                            Common.cellSampleSize = DataMatrixCellSampleSize.AutoDetect;
                        }
                        if (dmCellSize[1].StartsWith("Size3x3"))
                        {
                            Common.cellSampleSize = DataMatrixCellSampleSize.Size3x3;
                        }

                    }

                    if (s.StartsWith("QRMatrixSize"))
                    {
                        string[] qrSize = s.Split(':');
                        if (qrSize[1].StartsWith("Auto"))
                        {
                            Common.QRDimensions = QRDimension.AutoDetect;
                        }
                        if (qrSize[1].StartsWith("Size25x25"))
                        {
                            Common.QRDimensions = QRDimension.Size25x25;
                        }
                        if (qrSize[1].StartsWith("Size21x21"))
                        {
                            Common.QRDimensions = QRDimension.Size21x21;
                        }
                        if (qrSize[1].StartsWith("Size11x11"))
                        {
                            Common.QRDimensions = QRDimension.Size11x11;
                        }
                    }
                    if (s.StartsWith("QRPolarity"))
                    {
                        string[] qrPolarity = s.Split(':');
                        if (qrPolarity[1].StartsWith("Auto"))
                        {
                            Common.QRpolaritys = QRPolarity.AutoDetect;
                        }
                        if (qrPolarity[1].StartsWith("Black"))
                        {
                            Common.QRpolaritys = QRPolarity.BlackOnWhite;
                        }
                        if (qrPolarity[1].StartsWith("White"))
                        {
                            Common.QRpolaritys = QRPolarity.WhiteOnBlack;
                        }
                    }
                    if (s.StartsWith("QRCellSampleSize"))
                    {
                        string[] qrCellSize = s.Split(':');
                        if (qrCellSize[1].StartsWith("Auto"))
                        {
                            Common.QRCellSampleSizes = QRCellSampleSize.AutoDetect;
                        }
                        if (qrCellSize[1].StartsWith("Size3x3"))
                        {
                            Common.QRCellSampleSizes = QRCellSampleSize.Size3x3;
                        }

                    }

                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message + "---read program config error!");
                txtInformation.AppendText(DateTime.Now.ToString() + ": read program config error!\r\n");
            }
        }
        //Image processing.
        //通过传入参数i判断分析哪一张image，imageprocess的参数为当前程序读取参数。
        public void Analysis(int i)
        {
            txtAnalysis1 = "";
            txtAnalysis2 = "";
            try
            {
                Common.barcode = "";
                Common.datamatrixCode = "";
                Common.qrCode = "";
                if (i == 1)
                {
                    lblResult1.BackColor = Color.Red;
                    lblResult1.Text = "FAIL";
                    if (Common.barcodeFormat == "1D Barcode")
                    {
                        Processing.Process1DBarcode(imageViewer1.Image);
                        txtAnalysis1 = Common.barcode;
                        txtSend.Text = DateTime.Now.ToString() + ": " + Common.barcode;
                    }
                    if (Common.barcodeFormat == "Datamatrix")
                    {
                        Processing.ProcessDatamatrix(imageViewer1.Image);
                        txtAnalysis1 = Common.datamatrixCode;
                        txtSend.Text = DateTime.Now.ToString() + ": " + Common.datamatrixCode;
                    }
                    if (txtAnalysis1 != "")
                    {
                        lblResult1.BackColor = Color.Green;
                        lblResult1.Text = "PASS";
                    }
                }

                if (i == 2)
                {
                    //读取状态label
                    lblResult2.BackColor = Color.Red;
                    lblResult2.Text = "FAIL";
                    Processing.ProcessQR(imageViewer2.Image);
                    txtAnalysis2 = Common.qrCode;
                    txtCompare.Text = DateTime.Now.ToString() + ": " + Common.qrCode;
                    if (txtAnalysis2 != "")
                    {
                        lblResult2.BackColor = Color.Green;
                        lblResult2.Text = "PASS";
                    }
                }
            }
            catch (Exception ex)
            {
                //MessageBox.Show(ex.Message + "---Analysis error!");
                txtInformation.AppendText(DateTime.Now.ToString() + ": Analysis error!\r\n");
            }


        }

        public void TcpConnect()
        {
            try
            {
                //创建负责通信的Socket
                socketSend = new Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp);
                IPAddress ipAddress = IPAddress.Parse(txtIP.Text);
                IPEndPoint ipPoint = new IPEndPoint(ipAddress, Convert.ToInt32(txtPort.Text));
                //获得要连接的远程服务器应用程序的IP地址和端口号
                socketSend.Connect(ipPoint);
                //ShowMsg("连接成功");

                //开启一个新的线程不停的接收服务端发来的消息
                Thread th = new Thread(TcpRecive);
                th.IsBackground = true;
                th.Start();
                txtInformation.AppendText(DateTime.Now.ToString() + ": Connect SFIS successful!" + txtIP.Text + "," + txtPort.Text + "\r\n");
                lblSFIS.BackColor = Color.Green;
            }
            catch (Exception ex)
            {
                run = false;
                btnRun.BackColor = SystemColors.Control;
                lblSFIS.BackColor = Color.Red;
                txtInformation.AppendText(DateTime.Now.ToString() + ": Can't connect the servers " + txtIP.Text + "," + txtPort.Text + ex.Message + "\r\n");
                lblSFIS.BackColor = Color.Red;
            }
        }
        /// <summary>
        /// 不停的接受服务器发来的消息
        /// </summary>
        void TcpRecive()
        {
            while (true)
            {
                try
                {
                    byte[] buffer = new byte[1024];
                    int r = socketSend.Receive(buffer);
                    //实际接收到的有效字节数
                    if (r == 0)
                    {
                        break;
                    }
                    //以UTF-8编码读取buffer中的数据
                    string receiveSIFS = Encoding.UTF8.GetString(buffer, 0, r);
                    //跨线程调用SFISReceive textBox.text信息
                    ShowSFISReceive(receiveSIFS);
                    if (receiveSIFS.StartsWith("Receiving OK!"))
                    {
                        PlcWrite(success1);
                        SFISCodeInfo = receiveSIFS.Substring(13);
                        txtInformation.AppendText(DateTime.Now.ToString() + ": Receive message from SFIS: " + SFISCodeInfo + "\r\n");
                        timerPLC.Enabled = true;
                    }
                    else if (receiveSIFS.StartsWith("Receiving error"))
                    {
                        PlcWrite(failure1);
                        txtInformation.AppendText(DateTime.Now.ToString() + ": Receive message from SFIS: Receiving error!\r\n");
                        timerPLC.Enabled = true;
                    }



                }
                catch
                {
                    //MessageBox.Show(ex.Message);
                }

            }
        }
        //Send message to  SFIS.
        public void TcpSend(string tc)
        {
            try
            {
                byte[] buffer = System.Text.Encoding.UTF8.GetBytes(tc);
                socketSend.Send(buffer);
                txtInformation.AppendText(DateTime.Now.ToString() + ": Send message to SFIS: " + tc + "\r\n");
            }
            catch (Exception ex)
            {
                txtInformation.AppendText(DateTime.Now.ToString() + " Send message to SFIS error!: " + ex.Message + "\r\n");
                lblSFIS.BackColor = Color.Green;
            }
        }
        /// <summary>
        /// show message from SFIS received on main UI
        /// </summary>
        /// <param name="strST"></param>
        public void ShowSFISReceive(string s)
        {
            try
            {
                //跨线程调用
                if (txtReceived.InvokeRequired)
                {

                    Invoke(new SHOWMESSAGE(ShowSFISReceive), s);

                }
                else
                {
                    txtReceived.Text = s;
                }
            }

            catch
            {
            }
        }
        /// <summary>
        /// show work information on main UI
        /// </summary>
        /// <param name="strST"></param>
        public void ShowWorkInfo(string st)
        {
            try
            {
                //跨线程调用
                if (txtInformation.InvokeRequired)
                {

                    Invoke(new SHOWINFORMATION(ShowWorkInfo), st);

                }
                else
                {
                    txtInformation.AppendText(st);
                }
            }

            catch
            {
            }
        }
        /// <summary>
        /// 
        /// </summary>
        private void timer()
        {
            timerPLC.Elapsed += new System.Timers.ElapsedEventHandler(timerTh_Tick);

        }

        int ijk = 0;
        /// <summary>
        /// TimerTh_Tick   PLC query and write
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void timerTh_Tick(object sender, System.Timers.ElapsedEventArgs e)
        {

            try
            {
                PlcWrite(queryPLC);
                //ijk = ijk + 1;
                //ShowWorkInfo(DateTime.Now.ToString() +":  "+ ijk .ToString ()+"\r\n");
            }
            catch (Exception ex)
            {
                run = false;
                btnRun.BackColor = SystemColors.Control;
                timerPLC.Enabled = false;
                string runError = (DateTime.Now.ToString() + ": PLC write error in timer!" + ex.Message + "\r\n");
                ShowWorkInfo(runError);
            }

        }

        /// <summary>
        /// Write in PLC， Serial Port communication.
        /// </summary>
        /// <param name="strPlcWrite"></param>
        public void PlcWrite(string strPlcWrite)
        {
            try
            {
                if (serialPort1.IsOpen == false)
                {
                    serialPort1.Open();

                }
                int pWN = strPlcWrite.Length / 2;
                byte[] btPWN = new byte[pWN];
                for (int i = 0; i < pWN; i++)
                {

                    string strPWN = strPlcWrite.Substring(i * 2, 2);
                    btPWN[i] = Convert.ToByte(strPWN, 16);

                }
                //以字节方式写入ASCII值
                serialPort1.Write(btPWN, 0, pWN);

            }
            catch (System.Exception ex)
            {
                lblSerialPort.BackColor = Color.Red;
                txtInformation.AppendText(DateTime.Now.ToString() + ": PLC write error!" + ex.Message + "\r\n");
            }
        }

        private void serialPort1_DataReceived(object sender, SerialDataReceivedEventArgs e)
        {
            System.Threading.Thread.Sleep(50);
            //timerPLC.Enabled = false;
            this.Invoke(new EventHandler(PlcDataReceived));
        }
        /// </summary>
        /// Read in PLC : PLC return data
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void PlcDataReceived(object sender, EventArgs e)
        {
            timerPLC.Enabled = false;
            char[] cArray = serialPort1.ReadExisting().ToCharArray();

            string strTemp = "";
            foreach (char c in cArray)
            {
                // Get the integral value of the character.
                int cNum = Convert.ToInt32(c);
                // Convert the decimal value to a hexadecimal value in string form.
                string strHex = String.Format("{0:X2}", cNum);

                strTemp += strHex;

            }

            txtInformation.AppendText(DateTime.Now.ToString() + ": Response of PLC: " + strTemp + "\r\n");
            if (run)
            {
                switch (strTemp)
                {

                    // Snap1 command form PLC.
                    case snap1:
                        {
                            try
                            {
                                txtInformation.AppendText(DateTime.Now.ToString() + " Path: " + Common.templatePath + "\r\n");
                                Common.session.Snap(imageViewer1.Image);
                                Analysis(1);
                            }
                            catch (Exception ex)
                            {
                                lblCamera.BackColor = Color.Red;
                                MessageBox.Show(ex.Message + "---Snap1 errorF!");
                                txtInformation.AppendText(DateTime.Now.ToString() + ":snap1 error in automatic process!\r\n");
                            }


                            if (txtAnalysis1 == "")
                            {
                                timerPLC.Enabled = false;
                                PlcWrite(failure1);

                                timerPLC.Enabled = true;
                            }
                            else
                            {
                                timerPLC.Enabled = false;
                                TcpSend(txtAnalysis1);
                            }
                        }
                        break;
                    //Snap2 command from PLC.
                    case snap2:
                        {
                            try
                            {
                                Common.session.Snap(imageViewer2.Image);
                                Analysis(2);
                            }
                            catch (Exception ex)
                            {
                                MessageBox.Show(ex.Message + "---Snap2 error!");
                                txtInformation.AppendText(DateTime.Now.ToString() + ":snap2 error in automatic process!\r\n");
                            }
                            txtInformation.AppendText(DateTime.Now.ToString() + ": Analysis2: " + txtAnalysis2);
                            txtInformation.AppendText(DateTime.Now.ToString() + ": SFISCodeInfo: " + SFISCodeInfo);
                            txtInformation.AppendText(DateTime.Now.ToString() + ": Analysis2: " + txtAnalysis2.Length.ToString() + "\r\n");
                            txtInformation.AppendText(DateTime.Now.ToString() + ": SFISCodeInfo: " + SFISCodeInfo.Length.ToString() + "\r\n");
                            if (txtAnalysis2 + "\r" == SFISCodeInfo && SFISCodeInfo != "")
                            {
                                timerPLC.Enabled = false;
                                PlcWrite(success2);
                                txtInformation.AppendText(DateTime.Now.ToString() + ": Receiving ok!\r\n");

                            }
                            else
                            {
                                timerPLC.Enabled = false;
                                PlcWrite(failure2);
                                txtInformation.AppendText(DateTime.Now.ToString() + ":receiving failure!\r\n");

                            }
                            timerPLC.Enabled = true;
                        }
                        break;
                    case snap0:
                        {
                            timerPLC.Enabled = true;
                        }
                        break;
                }
            }

        }

        private void Main_FormClosing(object sender, FormClosingEventArgs e)
        {
            if (MessageBox.Show("确定要退出吗？", "确定Yes 取消No", MessageBoxButtons.YesNo, MessageBoxIcon.Asterisk) == DialogResult.No)
            {
                e.Cancel = true;
            }
        }


    }
}
