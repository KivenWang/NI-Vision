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

namespace LabelingVisualIdentification
{
    public partial class Programming : Form
    {
        //Creat gobal single
        public static  Programming frmProgramming = null;
        public static  Programming GetSingle()
        {
            if (frmProgramming == null)
            {
                frmProgramming = new Programming();
            }
            return frmProgramming;
        }

        private  Programming()
        {
            InitializeComponent();                       
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
                Common.session.Snap(imageViewer1.Image);

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
            try
            {
                if (Common.session == null)
                {
                    Common.session = new ImaqdxSession(Common.cameraID);
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
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
            try
            {

                imageViewer1.Image.Overlays.Default.Clear();

                MatchPatternOptions options = new MatchPatternOptions(MatchMode.RotationInvariant, 1);

                options.MinimumMatchScore = 800;
                options.MinimumContrast = 0;
                options.SubpixelAccuracy = false;

                Collection<PatternMatch> matches = Algorithms.MatchPattern(imageViewer1.Image, imageViewer2.Image, options, imageViewer1.Roi);
                RectangleContour rectROI1 = (RectangleContour)imageViewer1.Roi.GetContour(0).Shape;
                Common.patternRectLeft = rectROI1.Left;
                Common.patternRectTop = rectROI1.Top;
                Common.patternRectWidth = rectROI1.Width;
                Common.patternRectHeight = rectROI1.Height;

                // Display results.            
                foreach (PatternMatch match in matches)
                {
                    imageViewer1.Image.Overlays.Default.AddPolygon(new PolygonContour(match.Corners), Rgb32Value.RedColor);
                    Common.templatePositionX = match.Position.X;
                    Common.templatePositionY = match.Position.Y;
                }
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
                string writePath = Common.path + "\\Programming\\template\\" + txbProgramName.Text + ".png";
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
                    Common.templatePath = Common.path + "\\Programming\\template\\" + txbProgramName.Text + ".png";
                    Common.barcodeNumber = Convert.ToInt32(cbxBarcodeNo.Text);
                    if (Convert.ToInt32(cbxBarcodeNo.Text) > 0)
                    {
                        RectangleContour rectROI1 = (RectangleContour)imageViewer1.Roi.GetContour(0).Shape;
                        Common.barcode1Left = rectROI1.Left;
                        Common.barcode1Top = rectROI1.Top;
                        Common.barcode1Width = rectROI1.Width;
                        Common.barcode1Height = rectROI1.Height;
                    }
                    if (Convert.ToInt32(cbxBarcodeNo.Text) > 1)
                    {
                        RectangleContour rectROI2 = (RectangleContour)imageViewer1.Roi.GetContour(1).Shape;
                        Common.barcode2Left = rectROI2.Left;
                        Common.barcode2Top = rectROI2.Top;
                        Common.barcode2Width = rectROI2.Width;
                        Common.barcode2Height = rectROI2.Height;
                    }
                    if (Convert.ToInt32(cbxBarcodeNo.Text) > 2)
                    {
                        RectangleContour rectROI3 = (RectangleContour)imageViewer1.Roi.GetContour(2).Shape;
                        Common.barcode3Left = rectROI3.Left;
                        Common.barcode3Top = rectROI3.Top;
                        Common.barcode3Width = rectROI3.Width;
                        Common.barcode3Height = rectROI3.Height;
                    }
                    if (Convert.ToInt32(cbxBarcodeNo.Text) > 3)
                    {
                        RectangleContour rectROI4 = (RectangleContour)imageViewer1.Roi.GetContour(3).Shape;
                        Common.barcode4Left = rectROI4.Left;
                        Common.barcode4Top = rectROI4.Top;
                        Common.barcode4Width = rectROI4.Width;
                        Common.barcode4Height = rectROI4.Height;
                    }

                    if (Convert.ToInt32(cbxBarcodeNo.Text) > 4)
                    {
                        RectangleContour rectROI5 = (RectangleContour)imageViewer1.Roi.GetContour(4).Shape;
                        Common.barcode5Left = rectROI5.Left;
                        Common.barcode5Top = rectROI5.Top;
                        Common.barcode5Width = rectROI5.Width;
                        Common.barcode5Height = rectROI5.Height;
                    }

                    if (Convert.ToInt32(cbxBarcodeNo.Text) > 5)
                    {
                        RectangleContour rectROI6 = (RectangleContour)imageViewer1.Roi.GetContour(5).Shape;
                        Common.barcode6Left = rectROI6.Left;
                        Common.barcode6Top = rectROI6.Top;
                        Common.barcode6Width = rectROI6.Width;
                        Common.barcode6Height = rectROI6.Height;
                    }
                    Processing.Process1DBarcode(imageViewer1.Image);
                    txbBarcode.Text = Common.barcode;
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
                Common.barcodeTypes = BarcodeTypes.Code128;
            }
            if (cbxBarcodeTypes.Text == "Code39")
            {
                Common.barcodeTypes = BarcodeTypes.Code39;
            }
        }

        private void btnClear1_Click(object sender, EventArgs e)
        {
            imageViewer1.Roi.Clear();
            imageViewer1.Image.Overlays.Default.Clear();
        }

        private void btnTab2Next_Click(object sender, EventArgs e)
        {
            string bar1Write = "";
            string bar2Write = "";
            string bar3Write = "";
            string bar4Write = "";
            string bar5Write = "";
            string bar6Write = "";

            int barcodeNo = Convert.ToInt32(cbxBarcodeNo.Text);
            string[] writeInfo = new string[barcodeNo + 6];

            try
            {
                if (Convert.ToInt32(cbxBarcodeNo.Text) > 0)
                {
                    RectangleContour rectROI1 = (RectangleContour)imageViewer1.Roi.GetContour(0).Shape;
                    bar1Write = "Barcode1Rectangle:" + rectROI1.Left + "," + rectROI1.Top + "," + rectROI1.Width + "," + rectROI1.Height;
                    writeInfo[6] = bar1Write;
                }
                if (Convert.ToInt32(cbxBarcodeNo.Text) > 1)
                {
                    RectangleContour rectROI2 = (RectangleContour)imageViewer1.Roi.GetContour(1).Shape;
                    bar2Write = "Barcode2Rectangle:" + rectROI2.Left + "," + rectROI2.Top + "," + rectROI2.Width + "," + rectROI2.Height;
                    writeInfo[7] = bar2Write;
                }
                if (Convert.ToInt32(cbxBarcodeNo.Text) > 2)
                {
                    RectangleContour rectROI3 = (RectangleContour)imageViewer1.Roi.GetContour(2).Shape;
                    bar3Write = "Barcode3Rectangle:" + rectROI3.Left + "," + rectROI3.Top + "," + rectROI3.Width + "," + rectROI3.Height;
                    writeInfo[8] = bar3Write;
                }
                if (Convert.ToInt32(cbxBarcodeNo.Text) > 3)
                {
                    RectangleContour rectROI4 = (RectangleContour)imageViewer1.Roi.GetContour(3).Shape;
                    bar4Write = "Barcode4Rectangle:" + rectROI4.Left + "," + rectROI4.Top + "," + rectROI4.Width + "," + rectROI4.Height;
                    writeInfo[9] = bar4Write;
                }

                if (Convert.ToInt32(cbxBarcodeNo.Text) > 4)
                {
                    RectangleContour rectROI5 = (RectangleContour)imageViewer1.Roi.GetContour(4).Shape;
                    bar5Write = "Barcode5Rectangle:" + rectROI5.Left + "," + rectROI5.Top + "," + rectROI5.Width + "," + rectROI5.Height;
                    writeInfo[10] = bar5Write;
                }

                if (Convert.ToInt32(cbxBarcodeNo.Text) > 5)
                {
                    RectangleContour rectROI6 = (RectangleContour)imageViewer1.Roi.GetContour(5).Shape;
                    bar6Write = "Barcode6Rectangle:" + rectROI6.Left + "," + rectROI6.Top + "," + rectROI6.Width + "," + rectROI6.Height;
                    writeInfo[11] = bar6Write;
                }

                if (txbBarcode.Text != "" & cbxBarcodeNo.Text != "" & cbxFormat.Text != "" & cbxBarcodeTypes.Text != "")
                {
                    writeInfo[0] = "BarcodeFormat:" + cbxFormat.Text;
                    writeInfo[1] = "BarcodeNumber:" + cbxBarcodeNo.Text;
                    writeInfo[2] = "TemplatePosition:" + Common.templatePositionX.ToString() + "," + Common.templatePositionY.ToString();
                    writeInfo[3] = "BarcodeTypes:" + cbxBarcodeTypes.Text;
                    writeInfo[4] = "PatternRectangle:" + Common.patternRectLeft + "," + Common.patternRectTop + "," + Common.patternRectWidth + "," + Common.patternRectHeight;
                    writeInfo[5] = "TemplatePath;" + Common .templatePath ;
                }
                else
                {
                    MessageBox.Show("Please confirm the information is integrated!");
                }
                //File.WriteAllText(@"C:\Users\plc\Desktop\barcodePrograming\Programming\123", cbxBarcodeNo.Text + "," + bar1Write + "," + bar2Write, Encoding.UTF8);


                File.WriteAllLines(Common.path + "\\Programming\\" + txbProgramName.Text, writeInfo);
                this.tabControl1.SelectedIndex = 3;
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
            
        }

        private void btnTab2Pre_Click(object sender, EventArgs e)
        {
            this.tabControl1.SelectedIndex = 0;
        }

        private void btnWriteFile_Click(object sender, EventArgs e)
        {

            
        }

        private void btnSnap3_Click(object sender, EventArgs e)
        {
            try
            {
                Common.session.Snap(imageViewer3.Image);

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
                Common.patternRectLeft = rectROI1.Left;
                Common.patternRectTop = rectROI1.Top;
                Common.patternRectWidth = rectROI1.Width;
                Common.patternRectHeight = rectROI1.Height;

                // Display results.            
                foreach (PatternMatch match in matches)
                {
                    imageViewer3.Image.Overlays.Default.AddPolygon(new PolygonContour(match.Corners), Rgb32Value.RedColor);
                    Common.templatePositionX = match.Position.X;
                    Common.templatePositionY = match.Position.Y;
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
                string writePath = Common.path + "\\Programming\\template\\" + txbProgramName.Text + ".png";
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
            if (txbProgramName.Text != "" & cbxMatrixSize .Text != "" & cbxCellSample .Text  != "" & cbxPolarity .Text  != "")
            {
                try
                {
                    Common.templatePath = Common.path + "\\Programming\\template\\" + txbProgramName.Text + ".png";
                    RectangleContour rectROI1 = (RectangleContour)imageViewer3.Roi.GetContour(0).Shape;
                    Common.dmRectLeft = rectROI1.Left;
                    Common.dmRectTop  = rectROI1.Top;
                    Common.dmRectWidth  = rectROI1.Width;
                    Common.dmRectHeight = rectROI1.Height;
                    

                    Processing.ProcessDatamatrix(imageViewer3.Image);
                    if (Common.dmFound)
                    {
                        tbxDatamatrix.Text = Common.datamatrixCode;
                    }
                    else
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
                    writeInfo[0] = "BarcodeFormat:" + cbxFormat.Text;
                    writeInfo[1] = "TemplatePosition:" + Common.templatePositionX.ToString() + "," + Common.templatePositionY.ToString();
                    
                    writeInfo[2] = "PatternRectangle:" + Common.patternRectLeft + "," + Common.patternRectTop + "," + Common.patternRectWidth + "," + Common.patternRectHeight;
                    
                    writeInfo[3] = "DatamatrixRectangle:" + Common.dmRectLeft + "," + Common.dmRectTop  + "," + Common.dmRectWidth + "," + Common.dmRectHeight;

                    writeInfo[4] = "DatamatrixMatrixSize:" + Common.matrixSize;
                    writeInfo[5] = "DatamatrixPolarity:" + Common.polarity;
                    writeInfo[6] = "DatamatrixCellSampleSize:" + Common.cellSampleSize ;
                    writeInfo[7] = "TemplatePath;" + Common.templatePath;
                }
                else
                {
                    MessageBox.Show("Please confirm the information is integrated!");
                }
                //File.WriteAllText(@"C:\Users\plc\Desktop\barcodePrograming\Programming\123", cbxBarcodeNo.Text + "," + bar1Write + "," + bar2Write, Encoding.UTF8);


                File.WriteAllLines(Common.path + "\\Programming\\" + txbProgramName.Text, writeInfo);
                this.tabControl1.SelectedIndex = 3;
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }

        private void cbxMatrixSize_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (cbxMatrixSize .Text =="Auto-detect")
            {
                Common.matrixSize = 0;
            }
            else  if (cbxMatrixSize.Text == "23*23")
            {
                Common.matrixSize = 23;
            }
            else if (cbxMatrixSize.Text == "24*24")
            {
                Common.matrixSize = 24;
            }
            else if (cbxMatrixSize.Text == "25*25")
            {
                Common.matrixSize = 25;
            }
            else if (cbxMatrixSize.Text == "26*26")
            {
                Common.matrixSize = 26;
            }
        }

        private void cbxPolarity_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (cbxPolarity .Text =="Auto-detect")
            {
                Common.polarity = DataMatrixPolarity.AutoDetect;
            }
            else if (cbxPolarity.Text == "Black on White")
            {
                Common.polarity = DataMatrixPolarity.BlackDataOnWhiteBackground;
            }
            else if (cbxPolarity.Text == "White on Black")
            {
                Common.polarity = DataMatrixPolarity.WhiteDataOnBlackBackground;
            }
        }

        

        private void cbxCellSample_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (cbxCellSample .Text  == "Auto-detect")
            {
                Common.cellSampleSize = DataMatrixCellSampleSize.AutoDetect;
            }
            else if (cbxCellSample.Text == "3*3")
            {
                Common.cellSampleSize = DataMatrixCellSampleSize.Size3x3;
            }
            
        }

        private void btnSnap5_Click(object sender, EventArgs e)
        {
            try
            {
                Common.session.Snap(imageViewer5.Image);

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
            if (cbxQRSize.Text   == "Auto-detect")
            {
                Common.QRDimensions = QRDimension.AutoDetect;
            }
            else if (cbxQRSize.Text == "11*11")
            {
                Common.QRDimensions = QRDimension.Size11x11;
            }
            else if (cbxQRSize.Text == "21*21")
            {
                Common.QRDimensions = QRDimension.Size21x21;
            }
            else if (cbxQRSize.Text == "25*25")
            {
                Common.QRDimensions = QRDimension.Size25x25;
            }
        }

        private void cbxQRPolarity_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (cbxQRPolarity.Text == "Auto-detect")
            {
                Common.QRpolaritys = QRPolarity.AutoDetect;
            }
            else if (cbxQRPolarity.Text == "Black on White")
            {
                Common.QRpolaritys = QRPolarity.BlackOnWhite;
            }
            else if (cbxQRPolarity.Text == "White on Black")
            {
                Common.QRpolaritys = QRPolarity.WhiteOnBlack;
            }
        }

        private void cbxQRCell_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (cbxCellSample.Text == "Auto-detect")
            {
                Common.cellSampleSize = DataMatrixCellSampleSize.AutoDetect;
            }
            else if (cbxCellSample.Text == "3*3")
            {
                Common.cellSampleSize = DataMatrixCellSampleSize.Size3x3;
            }
        }

        private void btnSearch5_Click(object sender, EventArgs e)
        {
            if (txbProgramName.Text != "" & cbxQRSize .Text  != "" & cbxQRPolarity .Text  != "" & cbxQRCell .Text  != "")
            {
                try
                {
                    
                    RectangleContour rectROI1 = (RectangleContour)imageViewer5.Roi.GetContour(0).Shape;
                    Common.QRRectLeft = rectROI1.Left;
                    Common.QRRectTop = rectROI1.Top;
                    Common.QRRectWidth = rectROI1.Width;
                    Common.QRRectHeight = rectROI1.Height;


                    Processing.ProcessQR(imageViewer5.Image);
                    if (Common.qrFound)
                    {
                        tbxQR.Text = Common.qrCode;
                    }
                    else
                    {
                        MessageBox.Show("There is no QR code in the image!");
                    }
                }
                catch (Exception ex)
                {
                    MessageBox.Show(ex.Message+"Please select a ROI which contains the QR code! ");
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
                string[] writeInfos=new string[4];
                writeInfos[0] = "QRRectangle:" + Common.QRRectLeft + "," + Common.QRRectTop + "," + Common.QRRectWidth + "," + Common.QRRectHeight ;
                writeInfos[1] = "QRMatrixSize:" + Common.QRDimensions;
                writeInfos[2] = "QRPolarity:" + Common.QRpolaritys;
                writeInfos[3] = "QRCellSampleSize:" + Common.QRCellSampleSizes;
                File.AppendAllLines(Common.path + "\\Programming\\" + txbProgramName.Text, writeInfos, Encoding.UTF8);
                this.Close();
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message+"Please save again!");
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
