using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;
using DevComponents.DotNetBar;
using NationalInstruments.Vision;
using NationalInstruments.Vision.Analysis;
using NationalInstruments.Vision.Acquisition.Imaqdx;
using NationalInstruments.Vision.WindowsForms;
using log4net;
using System.Collections.ObjectModel;

namespace LabelingVisualIdentification
{
    public partial class ProgramForm : DevComponents.DotNetBar.OfficeForm
    {
        private static readonly ILog logger = LogManager.GetLogger("Main");
        private UserProgram userProgram = new UserProgram();
        private ProcessPicture processPicture = new ProcessPicture();
        public ProgramForm()
        {
            InitializeComponent();
            InitData();
            this.EnableGlass = false;
        }

        private void btnNextSelectType_Click(object sender, EventArgs e)
        {
            if (txtProgramName.Text == "" | cboInputCodeType.Text == "")
            {
                MessageBox.Show("请确认程序名称及来料条码类型!");
            }
            else if (cboInputCodeType.Text == "1D Barcode")
            {
                this.tabControlMain.SelectedTabIndex = 1;
            }
            else if (cboInputCodeType.Text == "Datamatrix")
            {
                this.tabControlMain.SelectedTabIndex = 2;
            }
        }

        private void btnPreBarcode_Click(object sender, EventArgs e)
        {
            this.tabControlMain.SelectedTabIndex = 0;
        }

        private void btnNextBarcode_Click(object sender, EventArgs e)
        {
            this.tabControlMain.SelectedTabIndex = 3;
        }

        private void btnPreDataMatrix_Click(object sender, EventArgs e)
        {
            this.tabControlMain.SelectedTabIndex = 0;
        }

        private void btnNextDataMatrix_Click(object sender, EventArgs e)
        {
            this.tabControlMain.SelectedTabIndex = 3;
        }

        private void btnPreQR_Click(object sender, EventArgs e)
        {
            this.tabControlMain.SelectedTabIndex = 2;
        }

        private void ProgramForm_Load(object sender, EventArgs e)
        {
            this.txtProgramName.Text = DateTime.Now.ToString("yyyyMMdd");
            this.cboInputCodeType.SelectedIndex = 0;
            this.cboBarcodeTypes.SelectedIndex = 0;
            this.cboMatatrixSize.SelectedIndex = 0;
            this.cboPolarityDataMatrix.SelectedIndex = 0;
            this.cboCellSizeDatamatrix.SelectedIndex = 0;
            this.cboDimensionQR.SelectedIndex = 0;
            this.cboPolarityQR.SelectedIndex = 0;
            this.cboCellSizeQR.SelectedIndex = 0;
        }

        private void btnLoadImageBarcode_Click(object sender, EventArgs e)
        {
            LoadImagetoImageViewer(ref imageViewerBarcodeInfo);
        }

        private void btnSnapBarcode_Click(object sender, EventArgs e)
        {
            try
            {
                //camera.Snap(imageViewerBarcodeInfo.Image);
            }
            catch (Exception ex)
            {
                logger.FatalFormat("Snapping error when programming barcode! Error message:{0}", ex.Message);
            }
        }

        private void btnExtractBarcode_Click(object sender, EventArgs e)
        {
            ExtractTemplatePattern(ref  imageViewerBarcodeInfo, ref imageViewerTemplateBarcode);
        }

        private void btnClearBarcode_Click(object sender, EventArgs e)
        {
            ClearROI(ref imageViewerBarcodeInfo);
        }

        private void btnMatchBarcode_Click(object sender, EventArgs e)
        {
            MatchTemplate(ref imageViewerBarcodeInfo, ref imageViewerTemplateBarcode);
        }

        private void btnSearchBarcode_Click(object sender, EventArgs e)
        {
            try
            {
                List<BarcodeConfig> barcodeConfigs = new List<BarcodeConfig>();
                int barcodeNum = imageViewerBarcodeInfo.Roi.Count;
                for (int i = 0; i < barcodeNum; i++)
                {
                    BarcodeConfig barcodeConfig = new BarcodeConfig();
                    RectangleContour rect = (RectangleContour)imageViewerBarcodeInfo.Roi.GetContour(i).Shape;
                    barcodeConfig.Rectangle.Left = rect.Left;
                    barcodeConfig.Rectangle.Top = rect.Top;
                    barcodeConfig.Rectangle.Width = rect.Width;
                    barcodeConfig.Rectangle.Height = rect.Height;

                    barcodeConfig.Type = BarcodeTypes.Code128;
                    if (cboBarcodeTypes.Text == "Code39")
                    {
                        barcodeConfig.Type = BarcodeTypes.Code39;
                    }
                    barcodeConfig.Index = i;
                    barcodeConfig.Name = string.Format("{0}{1}", barcodeConfig.Type.ToString(), i);
                    barcodeConfigs.Add(barcodeConfig);
                }
                txtBarcodeResult.Text = processPicture.Process1DBarcode(imageViewerBarcodeInfo.Image,
                    userProgram.TemplateConfig, userProgram.BarcodeConfigs);
                if (string.IsNullOrEmpty(txtBarcodeResult.Text))
                {
                    MessageBox.Show("There is no barcode info in the image!");
                }
                userProgram.BarcodeConfigs = barcodeConfigs;
            }
            catch (Exception ex)
            {
                logger.WarnFormat("Searching barcode info error! Error message:{0}", ex.Message);
            }
        }

        private void btnSaveTemplateBarcode_Click(object sender, EventArgs e)
        {
            SaveTemplate(imageViewerTemplateBarcode);
        }

        private void btnSnapDataMatrix_Click(object sender, EventArgs e)
        {
            try
            {
                //camera.Snap(imageViewerDatamatrix.Image);
            }
            catch (Exception ex)
            {
                logger.FatalFormat("Snapping error when programming datamatrix! Error message:{0}", ex.Message);
            }
        }

        private void btnLoadDatamatrix_Click(object sender, EventArgs e)
        {
            LoadImagetoImageViewer(ref  imageViewerDatamatrix);
        }

        private void btnExtractDatamatrix_Click(object sender, EventArgs e)
        {
            ExtractTemplatePattern(ref  imageViewerDatamatrix, ref imageViewerTemplateDataMatrix);
        }

        private void btnClearDatamatrix_Click(object sender, EventArgs e)
        {
            ClearROI(ref imageViewerDatamatrix);
        }

        private void btnMatchDatamatrix_Click(object sender, EventArgs e)
        {
            MatchTemplate(ref imageViewerDatamatrix, ref imageViewerTemplateDataMatrix);
        }

        private void btnSaveDatamatrix_Click(object sender, EventArgs e)
        {
            SaveTemplate(imageViewerTemplateDataMatrix);
        }

        private void btnSearchDatamatrix_Click(object sender, EventArgs e)
        {
            DataMatrixConfig dataMatrixConfig = new DataMatrixConfig();
            dataMatrixConfig.MatrixSize = cboMatatrixSize.Text;

            dataMatrixConfig.Polarity = DetectDataMatrixPolarity(cboPolarityDataMatrix.Text);
            dataMatrixConfig.CellSize = DetectDataMatrixCellSampleSize(cboCellSizeDatamatrix.Text);

            RectangleContour rectROI1 = (RectangleContour)imageViewerDatamatrix.Roi.GetContour(0).Shape;
            userProgram.DataMatrixConfigs[0].Rectangle.Left = rectROI1.Left;
            userProgram.DataMatrixConfigs[0].Rectangle.Top = rectROI1.Top;
            userProgram.DataMatrixConfigs[0].Rectangle.Width = rectROI1.Width;
            userProgram.DataMatrixConfigs[0].Rectangle.Height = rectROI1.Height;

            txtResultDatamatrix.Text = processPicture.ProcessDatamatrix(imageViewerDatamatrix.Image,
                userProgram.TemplateConfig, userProgram.DataMatrixConfigs);
            if (string.IsNullOrEmpty(txtResultDatamatrix.Text))
            {
                MessageBox.Show("There is no Datamatrix in the image!");
            }
        }

        private void btnSnapQR_Click(object sender, EventArgs e)
        {
            try
            {
                //camera.Snap(imageViewerQR.Image);
            }
            catch (Exception ex)
            {
                logger.FatalFormat("Snapping error when programming QR! Error message:{0}", ex.Message);
            }
        }

        private void btnLoadQR_Click(object sender, EventArgs e)
        {
            LoadImagetoImageViewer(ref imageViewerQR);
        }

        private void btnExtractQR_Click(object sender, EventArgs e)
        {
            ExtractTemplatePattern(ref  imageViewerQR, ref imageViewerTemplateQR);
        }

        private void btnClearQR_Click(object sender, EventArgs e)
        {
            ClearROI(ref imageViewerQR);
        }

        private void btnMatchQR_Click(object sender, EventArgs e)
        {
            MatchTemplate(ref imageViewerQR, ref imageViewerTemplateQR);
        }

        private void btnSaveQR_Click(object sender, EventArgs e)
        {
           
        }

        private void btnSearchQR_Click(object sender, EventArgs e)
        {
            QRConfig qRConfig = new QRConfig();
            RectangleContour rectROI1 = (RectangleContour)imageViewerQR.Roi.GetContour(0).Shape;
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

            qRConfig.QRDimension = DetectQRDimension(cboDimensionQR.Text);
            qRConfig.Polarity = DetectQRPolarity(cboPolarityQR.Text);
            qRConfig.CellSize = DetectQRCellSampleSize(cboCellSizeQR.Text);

            userProgram.QRConfigs.Add(qRConfig);
            txtResultQR.Text = processPicture.ProcessQR(imageViewerQR.Image, userProgram.QRConfigs);
        }

        private void btnFinish_Click(object sender, EventArgs e)
        {
            try
            {
                ConfigManager.UserPrograms.UserProgram.Add(userProgram);
                ConfigManager.UserPrograms.Save();
                this.Close();
            }
            catch (Exception ex)
            {
                logger.ErrorFormat("Saving program error! Error message:{0}", ex.Message);
            }
        }

        private void InitData()
        {
            this.cboDimensionQR.Items.AddRange(new object[]{
                "Auto-detect",
                "11X11",
                "21X21",
                "25X25"
            });
            this.cboPolarityQR.Items.AddRange(new object[] {
                "Auto-detect",
                "Black on White",
                "White on Black"
            });
            this.cboCellSizeQR.Items.AddRange(new object[] {
                "Auto-detect",
                "3X3"
            });
        }
        private bool LoadImagetoImageViewer(ref ImageViewer imageViewer)
        {
            try
            {
                ImagePreviewFileDialog imageDialog = new ImagePreviewFileDialog();
                if (imageDialog.ShowDialog() == DialogResult.OK)
                {
                    FileInformation fileinfo = Algorithms.GetFileInformation(imageDialog.FileName);
                    imageViewer.Image.Type = fileinfo.ImageType;
                    imageViewer.Image.ReadFile(imageDialog.FileName);
                    return true;
                }
            }
            catch (Exception ex)
            {
                logger.ErrorFormat("Load image to imageviewer error! Error message:{0}", ex.Message);
            }
            return false;
        }
        private void ExtractTemplatePattern(ref ImageViewer image, ref ImageViewer imageTemplate)
        {
            if (image.Roi.Count < 1)
                return;
            RectangleContour rect = (RectangleContour)image.Roi.GetContour(0).Shape;
            Algorithms.Extract(image.Image, imageTemplate.Image, rect);
            Algorithms.LearnPattern(imageTemplate.Image);
        }
        private void ClearROI(ref ImageViewer imageViewer)
        {
            imageViewer.Roi.Clear();
            imageViewer.Image.Overlays.Default.Clear();
        }
        private bool MatchTemplate(ref ImageViewer imageWiewer, ref ImageViewer imageTemplate)
        {
            imageWiewer.Image.Overlays.Default.Clear();
            MatchPatternOptions options = new MatchPatternOptions(MatchMode.RotationInvariant, 1);

            options.MinimumMatchScore = 800;
            options.MinimumContrast = 0;
            options.SubpixelAccuracy = false;

            Collection<PatternMatch> matches = Algorithms.MatchPattern(imageWiewer.Image,
                imageTemplate.Image, options, imageViewerDatamatrix.Roi);
            if (matches.Count < 1)
            {
                return false;
            }
            // Display results.            
            foreach (PatternMatch match in matches)
            {
                imageWiewer.Image.Overlays.Default.AddPolygon(new PolygonContour(match.Corners), Rgb32Value.RedColor);
                userProgram.TemplateConfig.Position.X = match.Position.X;
                userProgram.TemplateConfig.Position.Y = match.Position.Y;
            }
            RectangleContour rectROI1 = (RectangleContour)imageWiewer.Roi.GetContour(0).Shape;
            userProgram.TemplateConfig.Rectangle.Left = rectROI1.Left;
            userProgram.TemplateConfig.Rectangle.Top = rectROI1.Top;
            userProgram.TemplateConfig.Rectangle.Width = rectROI1.Width;
            userProgram.TemplateConfig.Rectangle.Height = rectROI1.Height;
            return false;
        }
        private bool SaveTemplate(ImageViewer imageViewer)
        {
            try
            {
                string writePath = string.Format(@"{0}Programming\template\{1}.png",
                        AppDomain.CurrentDomain.BaseDirectory, txtProgramName.Text);
                userProgram.TemplateConfig.TemplatePath = writePath;
                imageViewer.Image.WriteVisionFile(writePath);
                return true;
            }
            catch (Exception ex)
            {
                logger.ErrorFormat("Save template error! Error message:{0}", ex.Message);
            }
            return false;
        }
        private QRDimension DetectQRDimension(string type)
        {
            if (type == "11X11")
            {
                return QRDimension.Size11x11;
            }
            else if (type == "21X21")
            {
                return QRDimension.Size21x21;
            }
            else if (type  == "25X25")
            {
                return QRDimension.Size25x25;
            }
            return QRDimension.AutoDetect;
        }
        private QRPolarity  DetectQRPolarity(string type)
        {
            if (type == "Black on White")
            {
                return QRPolarity.BlackOnWhite;
            }
            else if (type == "White on Black")
            {
                return QRPolarity.WhiteOnBlack;
            }
            return QRPolarity.AutoDetect;
        }
        private QRCellSampleSize DetectQRCellSampleSize(string type)
        {
            if (type == "3*3")
            {
                return QRCellSampleSize.Size3x3;
            }
            return QRCellSampleSize.AutoDetect;
        }
        private DataMatrixPolarity DetectDataMatrixPolarity(string type)
        {
            if (type == "Black on White")
            {
                return DataMatrixPolarity.BlackDataOnWhiteBackground;
            }
            else if (type == "White on Black")
            {
                return DataMatrixPolarity.WhiteDataOnBlackBackground;
            }
            return DataMatrixPolarity.AutoDetect;
        }
        private DataMatrixCellSampleSize DetectDataMatrixCellSampleSize(string type)
        {
            if (type == "3*3")
            {
                return DataMatrixCellSampleSize.Size3x3;
            }
            return DataMatrixCellSampleSize.AutoDetect;
        }
    }
}