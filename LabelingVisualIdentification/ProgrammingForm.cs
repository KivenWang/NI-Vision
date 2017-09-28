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
using System.Collections.ObjectModel;
using System.IO;
using System.Configuration;

namespace LabelingVisualIdentification
{
    public partial class ProgrammingForm : Form
    {
        private ProcessPicture processPicture = new ProcessPicture();
        private Configuration config = ConfigurationManager.OpenExeConfiguration(ConfigurationUserLevel.None);
        private UserProgram userProgram = new UserProgram();
        //private TemplateConfig templateConfig = new TemplateConfig();
        private Camera camera;
        //Creat gobal single
        private static ProgrammingForm frmProgramming = null;
        public static ProgrammingForm GetInstance()
        {
            if (frmProgramming == null)
            {
                frmProgramming = new ProgrammingForm();
            }
            return frmProgramming;
        }

        private ProgrammingForm()
        {
            InitializeComponent();
            camera = Camera.GetInstance(config.AppSettings.Settings["CameraName"].Value);
        }


        private void btnLoadBarcode_Click(object sender, EventArgs e)
        {
            ImagePreviewFileDialog imageDialog = new ImagePreviewFileDialog();

            if (imageDialog.ShowDialog() == DialogResult.OK)
            {
                FileInformation fileinfo = Algorithms.GetFileInformation(imageDialog.FileName);
                imageViewer1.Image.Type = fileinfo.ImageType;
                imageViewer1.Image.ReadFile(imageDialog.FileName);
            }
        }

        private void btnSnapBarcode_Click(object sender, EventArgs e)
        {
            try
            {
                //camera.Snap(imageViewer1.Image);

            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }

        private void Programming_Load(object sender, EventArgs e)
        {

            //Maximize the tabcontrol region.
            this.tabControl1.Region = new Region(new RectangleF(this.tabPage1.Left, this.tabPage1.Top, this.tabPage1.Width, this.tabPage1.Height));
            //try
            //{
            //    if (Common.session == null)
            //    {
            //        Common.session = new ImaqdxSession(Common.cameraID);
            //    }
            //}
            //catch (Exception ex)
            //{
            //    MessageBox.Show(ex.Message);
            //}
        }

        private void btnTabNext0_Click(object sender, EventArgs e)
        {
            if (txbProgramName.Text == "" | cbxFormat.Text == "")
            {
                MessageBox.Show("Please confrim Program Name and Original Format!");
            }
            else if (cbxFormat.Text == "1D Barcode")
            {
                this.tabControl1.SelectedIndex = 1;
            }
            else if (cbxFormat.Text == "Datamatrix")
            {
                this.tabControl1.SelectedIndex = 2;
            }
        }

        private void btnExtract1_Click(object sender, EventArgs e)
        {
            try
            {
                RectangleContour rect = (RectangleContour)imageViewer1.Roi.GetContour(0).Shape;
                Algorithms.Extract(imageViewer1.Image, imageViewer2.Image, rect);
                Algorithms.LearnPattern(imageViewer2.Image);

            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }

        private void btnMatch1_Click(object sender, EventArgs e)
        {
            VisionRectangle rectangle = new VisionRectangle();
            VisionPosition position = new VisionPosition();
            try
            {

                imageViewer1.Image.Overlays.Default.Clear();

                MatchPatternOptions options = new MatchPatternOptions(MatchMode.RotationInvariant, 1);

                options.MinimumMatchScore = 800;
                options.MinimumContrast = 0;
                options.SubpixelAccuracy = false;

                Collection<PatternMatch> matches = Algorithms.MatchPattern(imageViewer1.Image, imageViewer2.Image, options, imageViewer1.Roi);
                RectangleContour rectROI1 = (RectangleContour)imageViewer1.Roi.GetContour(0).Shape;
                userProgram.TemplateConfig.Rectangle.Left = rectROI1.Left;
                userProgram.TemplateConfig.Rectangle.Top = rectROI1.Top;
                userProgram.TemplateConfig.Rectangle.Width = rectROI1.Width;
                userProgram.TemplateConfig.Rectangle.Height = rectROI1.Height;

                rectangle.Left = rectROI1.Left;
                rectangle.Top = rectROI1.Top;
                rectangle.Width = rectROI1.Width;
                rectangle.Height = rectROI1.Height;

                // Display results.            
                foreach (PatternMatch match in matches)
                {
                    imageViewer1.Image.Overlays.Default.AddPolygon(new PolygonContour(match.Corners), Rgb32Value.RedColor);
                    userProgram.TemplateConfig.Position.X = match.Position.X;
                    userProgram.TemplateConfig.Position.Y = match.Position.Y;
                    position.X = match.Position.X;
                    position.Y = match.Position.Y;
                }
                userProgram.TemplateConfig.Rectangle = rectangle;
                userProgram.TemplateConfig.Position = position;
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }

        }

        private void btnSave1_Click(object sender, EventArgs e)
        {
            try
            {
                string writePath = string.Format(@"{0}Programming\template\{1}.png", AppDomain.CurrentDomain.BaseDirectory, txbProgramName.Text);

                imageViewer2.Image.WriteVisionFile(writePath);
                MessageBox.Show("Template pattern has been saved successful!");
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }

        private void btnSearch1_Click(object sender, EventArgs e)
        {
            if (txbProgramName.Text != "" & cbxBarcodeNo.Text != "" & cbxFormat.Text != "" & cbxBarcodeTypes.Text != "")
            {
                try
                {
                    userProgram.TemplateConfig.TemplatePath = System.AppDomain.CurrentDomain.BaseDirectory + "\\Programming\\template\\" + txbProgramName.Text + ".png";
                    int barcodeNum = imageViewer1.Roi.Count;
                    for (int i = 0; i < barcodeNum; i++)
                    {
                        BarcodeConfig barcodeConfig = new BarcodeConfig();
                        RectangleContour rect = (RectangleContour)imageViewer1.Roi.GetContour(i).Shape;
                        barcodeConfig.Rectangle.Left = rect.Left;
                        barcodeConfig.Rectangle.Top = rect.Top;
                        barcodeConfig.Rectangle.Width = rect.Width;
                        barcodeConfig.Rectangle.Height = rect.Height;

                        barcodeConfig.Type = BarcodeTypes.Code128;
                        if (cbxBarcodeTypes.Text == "Code39")
                        {
                            barcodeConfig.Type = BarcodeTypes.Code39;
                        }
                        barcodeConfig.Index = i;
                        barcodeConfig.Name = string.Format("{0}{1}", barcodeConfig.Type.ToString(), i);
                        userProgram.BarcodeConfigs.Add(barcodeConfig);
                    }
                    userProgram.TemplateConfig.TemplatePath = string.Format(@"{0}Programming\template\{1}.png", AppDomain.CurrentDomain.BaseDirectory, txbProgramName.Text);
                    txbBarcode.Text = processPicture.Process1DBarcode(imageViewer1.Image, userProgram.TemplateConfig, userProgram.BarcodeConfigs);

                }
                catch (Exception ex)
                {
                    MessageBox.Show(ex.Message);
                }
            }
            else
            {
                MessageBox.Show("Please confirm the information Barcode format and Barcode number!");
            }
        }

        private void cbxBarcodeTypes_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (cbxBarcodeTypes.Text == "Code128")
            {
                for (int i = 0; i < userProgram.BarcodeConfigs.Count; i++)
                {
                    userProgram.BarcodeConfigs[i].Type = BarcodeTypes.Code128;
                }
            }
            if (cbxBarcodeTypes.Text == "Code39")
            {
                for (int i = 0; i < userProgram.BarcodeConfigs.Count; i++)
                {
                    userProgram.BarcodeConfigs[i].Type = BarcodeTypes.Code39;
                }
            }
        }

        private void btnClear1_Click(object sender, EventArgs e)
        {
            imageViewer1.Roi.Clear();
            imageViewer1.Image.Overlays.Default.Clear();
        }

        private void btnTab2Next_Click(object sender, EventArgs e)
        {
            int barcodeNo = Convert.ToInt32(cbxBarcodeNo.Text);
            string[] writeInfo = new string[barcodeNo + 6];
            this.tabControl1.SelectedIndex = 3;
        }

        private void btnTab2Pre_Click(object sender, EventArgs e)
        {
            this.tabControl1.SelectedIndex = 0;
        }
        private void btnSnap3_Click(object sender, EventArgs e)
        {
            try
            {
                //camera.Snap(imageViewer3.Image);

            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }
        private void btnLoadImage3_Click(object sender, EventArgs e)
        {
            ImagePreviewFileDialog imageDialog = new ImagePreviewFileDialog();

            if (imageDialog.ShowDialog() == DialogResult.OK)
            {
                FileInformation fileinfo = Algorithms.GetFileInformation(imageDialog.FileName);
                imageViewer3.Image.Type = fileinfo.ImageType;
                imageViewer3.Image.ReadFile(imageDialog.FileName);
            }
        }
        private void btnExtract3_Click(object sender, EventArgs e)
        {
            try
            {
                RectangleContour rect = (RectangleContour)imageViewer3.Roi.GetContour(0).Shape;
                Algorithms.Extract(imageViewer3.Image, imageViewer4.Image, rect);
                Algorithms.LearnPattern(imageViewer4.Image);

            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }

        private void btnMatch3_Click(object sender, EventArgs e)
        {
            try
            {
                imageViewer3.Image.Overlays.Default.Clear();
                MatchPatternOptions options = new MatchPatternOptions(MatchMode.RotationInvariant, 1);

                options.MinimumMatchScore = 800;
                options.MinimumContrast = 0;
                options.SubpixelAccuracy = false;

                Collection<PatternMatch> matches = Algorithms.MatchPattern(imageViewer3.Image, imageViewer4.Image, options, imageViewer3.Roi);
                RectangleContour rectROI1 = (RectangleContour)imageViewer3.Roi.GetContour(0).Shape;
                userProgram.TemplateConfig.Rectangle.Left = rectROI1.Left;
                userProgram.TemplateConfig.Rectangle.Top = rectROI1.Top;
                userProgram.TemplateConfig.Rectangle.Width = rectROI1.Width;
                userProgram.TemplateConfig.Rectangle.Height = rectROI1.Height;

                // Display results.            
                foreach (PatternMatch match in matches)
                {
                    imageViewer3.Image.Overlays.Default.AddPolygon(new PolygonContour(match.Corners), Rgb32Value.RedColor);
                    userProgram.TemplateConfig.Position.X = match.Position.X;
                    userProgram.TemplateConfig.Position.Y = match.Position.Y;
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }

        private void btnSaveTemplate3_Click(object sender, EventArgs e)
        {
            try
            {
                string writePath = System.AppDomain.CurrentDomain.BaseDirectory + "\\Programming\\template\\" + txbProgramName.Text + ".png";
                imageViewer4.Image.WriteVisionFile(writePath);
                MessageBox.Show("Template pattern has been saved successful!");
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }

        private void btnSearch3_Click(object sender, EventArgs e)
        {
            if (txbProgramName.Text != "" & cbxMatrixSize.Text != "" & cbxCellSample.Text != "" & cbxPolarity.Text != "")
            {
                try
                {
                    userProgram.TemplateConfig.TemplatePath = System.AppDomain.CurrentDomain.BaseDirectory + "\\Programming\\template\\" + txbProgramName.Text + ".png";
                    RectangleContour rectROI1 = (RectangleContour)imageViewer3.Roi.GetContour(0).Shape;
                    userProgram.DataMatrixConfigs[0].Rectangle.Left = rectROI1.Left;
                    userProgram.DataMatrixConfigs[0].Rectangle.Top = rectROI1.Top;
                    userProgram.DataMatrixConfigs[0].Rectangle.Width = rectROI1.Width;
                    userProgram.DataMatrixConfigs[0].Rectangle.Height = rectROI1.Height;

                    tbxDatamatrix.Text = processPicture.ProcessDatamatrix(imageViewer3.Image, userProgram.TemplateConfig, userProgram.DataMatrixConfigs);
                    if (string.IsNullOrEmpty(tbxDatamatrix.Text))
                    {
                        MessageBox.Show("There is no Datamatrix in the image!");
                    }
                }
                catch (Exception ex)
                {
                    MessageBox.Show(ex.Message);
                }
            }
            else
            {
                MessageBox.Show("Please confirm the integrate of information!");
            }
        }

        private void btnClear3_Click(object sender, EventArgs e)
        {
            imageViewer3.Roi.Clear();
            imageViewer3.Image.Overlays.Default.Clear();
        }

        private void btnPre3_Click(object sender, EventArgs e)
        {
            this.tabControl1.SelectedIndex = 0;
        }

        private void btnNext3_Click(object sender, EventArgs e)
        {

            string[] writeInfo = new string[8];

            try
            {
                if (cbxMatrixSize.Text != "" & cbxCellSample.Text != "" & cbxPolarity.Text != "")
                {
                    
                }
                else
                {
                    MessageBox.Show("Please confirm the information is integrated!");
                }
                //File.WriteAllText(@"C:\Users\plc\Desktop\barcodePrograming\Programming\123", cbxBarcodeNo.Text + "," + bar1Write + "," + bar2Write, Encoding.UTF8);

                File.WriteAllLines(System.AppDomain.CurrentDomain.BaseDirectory + "\\Programming\\" + txbProgramName.Text, writeInfo);
                this.tabControl1.SelectedIndex = 3;
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }

        private void cbxMatrixSize_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (cbxMatrixSize.Text == "Auto-detect")
            {
                userProgram.DataMatrixConfigs[0].MatrixSize = "0";
            }
            else if (cbxMatrixSize.Text == "23*23")
            {
                userProgram.DataMatrixConfigs[0].MatrixSize = "23X23";
            }
            else if (cbxMatrixSize.Text == "24*24")
            {
                userProgram.DataMatrixConfigs[0].MatrixSize = "24X24";
            }
            else if (cbxMatrixSize.Text == "25*25")
            {
                userProgram.DataMatrixConfigs[0].MatrixSize = "25X25";
            }
            else if (cbxMatrixSize.Text == "26*26")
            {
                userProgram.DataMatrixConfigs[0].MatrixSize = "26X26";
            }
        }

        private void cbxPolarity_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (cbxPolarity.Text == "Auto-detect")
            {
                userProgram.DataMatrixConfigs[0].Polarity = DataMatrixPolarity.AutoDetect;
            }
            else if (cbxPolarity.Text == "Black on White")
            {
                userProgram.DataMatrixConfigs[0].Polarity = DataMatrixPolarity.BlackDataOnWhiteBackground;
            }
            else if (cbxPolarity.Text == "White on Black")
            {
                userProgram.DataMatrixConfigs[0].Polarity = DataMatrixPolarity.WhiteDataOnBlackBackground;
            }
        }

        private void cbxCellSample_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (cbxCellSample.Text == "Auto-detect")
            {
                userProgram.DataMatrixConfigs[0].CellSize = DataMatrixCellSampleSize.AutoDetect;
            }
            else if (cbxCellSample.Text == "3*3")
            {
                userProgram.DataMatrixConfigs[0].CellSize = DataMatrixCellSampleSize.Size3x3;
            }

        }

        private void btnSnap5_Click(object sender, EventArgs e)
        {
            try
            {
                //camera.Snap(imageViewer5.Image);

            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }

        private void btnLoadImage5_Click(object sender, EventArgs e)
        {
            ImagePreviewFileDialog imageDialog = new ImagePreviewFileDialog();

            if (imageDialog.ShowDialog() == DialogResult.OK)
            {
                FileInformation fileinfo = Algorithms.GetFileInformation(imageDialog.FileName);
                imageViewer5.Image.Type = fileinfo.ImageType;
                imageViewer5.Image.ReadFile(imageDialog.FileName);
            }
        }

        private void cbxQRSize_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (cbxQRSize.Text == "Auto-detect")
            {
                userProgram.QRConfigs[0].QRDimension = QRDimension.AutoDetect;
            }
            else if (cbxQRSize.Text == "11*11")
            {
                userProgram.QRConfigs[0].QRDimension = QRDimension.Size11x11;
            }
            else if (cbxQRSize.Text == "21*21")
            {
                userProgram.QRConfigs[0].QRDimension = QRDimension.Size21x21;
            }
            else if (cbxQRSize.Text == "25*25")
            {
                userProgram.QRConfigs[0].QRDimension = QRDimension.Size25x25;
            }
        }

        private void cbxQRPolarity_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (cbxQRPolarity.Text == "Auto-detect")
            {
                userProgram.QRConfigs[0].Polarity = QRPolarity.AutoDetect;
            }
            else if (cbxQRPolarity.Text == "Black on White")
            {
                userProgram.QRConfigs[0].Polarity = QRPolarity.BlackOnWhite;
            }
            else if (cbxQRPolarity.Text == "White on Black")
            {
                userProgram.QRConfigs[0].Polarity = QRPolarity.WhiteOnBlack;
            }
        }

        private void cbxQRCell_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (cbxCellSample.Text == "Auto-detect")
            {
                userProgram.QRConfigs[0].CellSize = QRCellSampleSize.AutoDetect;
            }
            else if (cbxCellSample.Text == "3*3")
            {
                userProgram.QRConfigs[0].CellSize = QRCellSampleSize.Size3x3;
            }
        }

        private void btnSearch5_Click(object sender, EventArgs e)
        {
            if (txbProgramName.Text != "" & cbxQRSize.Text != "" & cbxQRPolarity.Text != "" & cbxQRCell.Text != "")
            {
                QRConfig qRConfig = new QRConfig();
                try
                {
                    RectangleContour rectROI1 = (RectangleContour)imageViewer5.Roi.GetContour(0).Shape;
                    userProgram.QRConfigs[0].Rectangle.Left = rectROI1.Left;
                    userProgram.QRConfigs[0].Rectangle.Top = rectROI1.Top;
                    userProgram.QRConfigs[0].Rectangle.Width = rectROI1.Width;
                    userProgram.QRConfigs[0].Rectangle.Height = rectROI1.Height;
                    qRConfig.Rectangle.Left = rectROI1.Left;
                    qRConfig.Rectangle.Top = rectROI1.Top;
                    qRConfig.Rectangle.Width = rectROI1.Width;
                    qRConfig.Rectangle.Height = rectROI1.Height;
                    qRConfig.Index = 0;
                    qRConfig.Name = string.Format("QRConfig0");

                    qRConfig.QRDimension = QRDimension.AutoDetect;

                    if (cbxQRSize.Text == "11*11")
                    {
                        qRConfig.QRDimension = QRDimension.Size11x11;
                    }
                    else if (cbxQRSize.Text == "21*21")
                    {
                        qRConfig.QRDimension = QRDimension.Size21x21;
                    }
                    else if (cbxQRSize.Text == "25*25")
                    {
                        qRConfig.QRDimension = QRDimension.Size25x25;
                    }

                    qRConfig.Polarity = QRPolarity.AutoDetect;

                    if (cbxQRPolarity.Text == "Black on White")
                    {
                        qRConfig.Polarity = QRPolarity.BlackOnWhite;
                    }
                    else if (cbxQRPolarity.Text == "White on Black")
                    {
                        qRConfig.Polarity = QRPolarity.WhiteOnBlack;
                    }

                    qRConfig.CellSize = QRCellSampleSize.AutoDetect;
                    if (cbxCellSample.Text == "3*3")
                    {
                        qRConfig.CellSize = QRCellSampleSize.Size3x3;
                    }
                    userProgram.QRConfigs.Add(qRConfig);

                    tbxQR.Text = processPicture.ProcessQR(imageViewer5.Image, userProgram.QRConfigs);


                }
                catch (Exception ex)
                {
                    MessageBox.Show(ex.Message + "Please select a ROI which contains the QR code! ");
                }

            }
            else
            {
                MessageBox.Show("Please confirm the information Barcode format and Barcode number!");
            }
        }

        private void btnClear5_Click(object sender, EventArgs e)
        {
            imageViewer5.Roi.Clear();
            imageViewer5.Image.Overlays.Default.Clear();
        }
        /// <summary>
        /// 最后一步，向程序文件中追加QR参数信息
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btnFinish_Click(object sender, EventArgs e)
        {
            try
            {
                ConfigManager.UserPrograms.UserProgram.Add(userProgram);
                ConfigManager.UserPrograms.Save();
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message + "Please save again!");
            }
            finally
            {
                this.Close();
            }
        }

        private void Programming_FormClosed(object sender, FormClosedEventArgs e)
        {
            frmProgramming = null;
        }

        private void btnPre4_Click(object sender, EventArgs e)
        {
            if (cbxFormat.Text == "1D Barcode")
            {
                this.tabControl1.SelectedIndex = 1;
            }
            else if (cbxFormat.Text == "Datamatrix")
            {
                this.tabControl1.SelectedIndex = 2;
            }
        }

        private void btnClear4_Click(object sender, EventArgs e)
        {
            imageViewer5.Roi.Clear();
            imageViewer5.Image.Overlays.Default.Clear();
        }


    }
}
