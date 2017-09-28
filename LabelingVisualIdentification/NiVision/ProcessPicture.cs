using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Drawing;
using System.Text;
using NationalInstruments.Vision.WindowsForms;
using NationalInstruments.Vision;
using NationalInstruments.Vision.Analysis;
using Vision_Assistant.Utilities;

namespace LabelingVisualIdentification
{
    public class ProcessPicture
    {
        private  Collection<PatternMatch> pmResults;
        private DataMatrixReport vaDataMatrixReport;
        private  QRReport vaQRCode;
        private  string vaQRCodeData;

        #region  Pattern match
        private Collection<PatternMatch> IVA_MatchPattern(VisionImage image,
                                                                 IVA_Data ivaData,
                                                                 string templatePath,
                                                                 MatchMode matchMode,
                                                                 bool subpixel,
                                                                 int[] angleRangeMin,
                                                                 int[] angleRangeMax,
                                                                 int matchesRequested,
                                                                 double score,
                                                                 Roi roi,
                                                                 double matchOffset_x,
                                                                 double matchOffset_y,
                                                                 int stepIndex)
        {

            using (VisionImage imageTemplate = new VisionImage(ImageType.U8, 7))
            {
                int numObjectResults = 4;
                Collection<PatternMatch> patternMatchingResults = new Collection<PatternMatch>();
                CoordinatesReport realWorldPosition = new CoordinatesReport();
                PointContour matchLocation = new PointContour();

                // Read the image template.
                imageTemplate.ReadVisionFile(templatePath);
                //Algorithms.LearnPattern(imageTemplate);

                // If the image is calibrated, we also need to log the calibrated position (x and y) -> 6 results instead of 4
                if ((image.InfoTypes & InfoTypes.Calibration) != 0)
                {
                    numObjectResults = 6;
                }

                // Fill in the Pattern Matching options.
                MatchPatternOptions matchPatternOptions = new MatchPatternOptions(matchMode, matchesRequested);
                matchPatternOptions.MinimumContrast = 0;
                matchPatternOptions.SubpixelAccuracy = subpixel;
                for (int i = 0; i < 2; ++i)
                {
                    matchPatternOptions.RotationAngleRanges.Add(new Range(angleRangeMin[i], angleRangeMax[i]));
                }
                matchPatternOptions.MinimumMatchScore = score;

                // Searches for areas in the image that match a given pattern.
                patternMatchingResults = Algorithms.MatchPattern2(image, imageTemplate, matchPatternOptions, roi);

                // ////////////////////////////////////////
                // Store the results in the data structure.
                // ////////////////////////////////////////

                // First, delete all the results of this step (from a previous iteration)
                Functions.IVA_DisposeStepResults(ivaData, stepIndex);

                if (patternMatchingResults.Count > 0)
                {
                    ivaData.stepResults[stepIndex].results.Add(new IVA_Result("# of objects", patternMatchingResults.Count));

                    for (int i = 0; i < patternMatchingResults.Count; ++i)
                    {

                        // Adjust the match location using the specified offsets.
                        matchLocation.X = patternMatchingResults[i].Position.X + Functions.IVA_ComputePMOffset(matchOffset_x, matchOffset_y, patternMatchingResults[i].Rotation).X;
                        matchLocation.Y = patternMatchingResults[i].Position.Y + Functions.IVA_ComputePMOffset(matchOffset_x, matchOffset_y, patternMatchingResults[i].Rotation).Y;

                        ivaData.stepResults[stepIndex].results.Add(new IVA_Result(String.Format("Match {0}.X Position (Pix.)", i + 1), matchLocation.X));
                        ivaData.stepResults[stepIndex].results.Add(new IVA_Result(String.Format("Match {0}.Y Position (Pix.)", i + 1), matchLocation.Y));

                        // If the image is calibrated, convert the pixel values to real world coordinates.
                        if (numObjectResults == 6)
                        {
                            realWorldPosition = Algorithms.ConvertPixelToRealWorldCoordinates(image, new Collection<PointContour>(new PointContour[] { matchLocation }));

                            ivaData.stepResults[stepIndex].results.Add(new IVA_Result(String.Format("Match {0}.X Position (World)", i + 1), realWorldPosition.Points[0].X));
                            ivaData.stepResults[stepIndex].results.Add(new IVA_Result(String.Format("Match {0}.Y Position (World)", i + 1), realWorldPosition.Points[0].Y));
                        }

                        ivaData.stepResults[stepIndex].results.Add(new IVA_Result(String.Format("Match {0}.Angle (degrees)", i + 1), patternMatchingResults[i].Rotation));
                        ivaData.stepResults[stepIndex].results.Add(new IVA_Result(String.Format("Match {0}.Score", i + 1), patternMatchingResults[i].Score));
                    }
                }

                return patternMatchingResults;

            }
        }
        #endregion

        #region  Set Coordinate System
        private void IVA_CoordSys(int coordSysIndex,
                                         int originStepIndex,
                                         int originResultIndex,
                                         int angleStepIndex,
                                         int angleResultIndex,
                                         double baseOriginX,
                                         double baseOriginY,
                                         double baseAngle,
                                         AxisOrientation baseAxisOrientation,
                                         int mode,
                                         IVA_Data ivaData)
        {

            ivaData.baseCoordinateSystems[coordSysIndex].Origin.X = baseOriginX;
            ivaData.baseCoordinateSystems[coordSysIndex].Origin.Y = baseOriginY;
            ivaData.baseCoordinateSystems[coordSysIndex].Angle = baseAngle;
            ivaData.baseCoordinateSystems[coordSysIndex].AxisOrientation = baseAxisOrientation;

            ivaData.MeasurementSystems[coordSysIndex].Origin.X = baseOriginX;
            ivaData.MeasurementSystems[coordSysIndex].Origin.Y = baseOriginY;
            ivaData.MeasurementSystems[coordSysIndex].Angle = baseAngle;
            ivaData.MeasurementSystems[coordSysIndex].AxisOrientation = baseAxisOrientation;

            switch (mode)
            {
                // Horizontal motion
                case 0:
                    ivaData.MeasurementSystems[coordSysIndex].Origin.X = Functions.IVA_GetNumericResult(ivaData, originStepIndex, originResultIndex);
                    break;
                // Vertical motion
                case 1:
                    ivaData.MeasurementSystems[coordSysIndex].Origin.Y = Functions.IVA_GetNumericResult(ivaData, originStepIndex, originResultIndex + 1);
                    break;
                // Horizontal and vertical motion
                case 2:
                    ivaData.MeasurementSystems[coordSysIndex].Origin = Functions.IVA_GetPoint(ivaData, originStepIndex, originResultIndex);
                    break;
                // Horizontal, vertical and angular motion
                case 3:
                    ivaData.MeasurementSystems[coordSysIndex].Origin = Functions.IVA_GetPoint(ivaData, originStepIndex, originResultIndex);
                    ivaData.MeasurementSystems[coordSysIndex].Angle = Functions.IVA_GetNumericResult(ivaData, angleStepIndex, angleResultIndex);
                    break;
            }
        }
        #endregion

        #region  Process 1D Barcode
        public string Process1DBarcode(VisionImage image,TemplateConfig templateConfig,List <BarcodeConfig > barcodeConfigs)
        {
            // Initialize the IVA_Data structure to pass results and coordinate systems.
            IVA_Data ivaData = new IVA_Data(5, 1);

            // Creates a new, empty region of interest.
            Roi roi = new Roi();

            // Creates a new RotatedRectangleContour using the given values.
            //PointContour vaCenter = new PointContour(1294, 972);
            //RotatedRectangleContour vaRotatedRect = new RotatedRectangleContour(vaCenter, 2548, 1904, 0);

            // Creates a new RectangleContour using the given values.
            RectangleContour vaRotatedRect = new RectangleContour(templateConfig.Rectangle.Left, 
                templateConfig.Rectangle.Top, templateConfig.Rectangle.Width, templateConfig.Rectangle.Height);
            roi.Add(vaRotatedRect);

            image.Overlays.Default.AddRoi(roi);

            // MatchPattern Grayscale
            MatchMode vaMode = MatchMode.RotationInvariant;
            bool vaSubpixelVal = false;
            int[] minAngleVals = { -30, 0 };
            int[] maxAngleVals = { 30, 0 };
            int vaNumMatchesRequested = 1;
            double vaMinMatchScore = 800;
            double vaOffsetX = 0;
            double vaOffsetY = 0;
            pmResults = IVA_MatchPattern(image, ivaData, templateConfig.TemplatePath , vaMode, vaSubpixelVal,
                minAngleVals, maxAngleVals, vaNumMatchesRequested, vaMinMatchScore, roi, vaOffsetX, vaOffsetY, 0);

            foreach (PatternMatch match in pmResults)
            {
                image.Overlays.Default.AddPolygon(new PolygonContour(match.Corners), Rgb32Value.RedColor);
            }
            roi.Dispose();
            // Set Coordinate System
            int vaCoordSystemIndex = 0;
            int stepIndexOrigin = 0;
            int resultIndexOrigin = 1;
            int stepIndexAngle = 0;
            int resultIndexAngle = 3;
            double refSysOriginX = templateConfig.Position.X;
            double refSysOriginY = templateConfig.Position.Y;
            double refSysAngle = 0;
            AxisOrientation refSysAxisOrientation = AxisOrientation.Direct;
            int vaCoordSystemType = 3;
            IVA_CoordSys(vaCoordSystemIndex, stepIndexOrigin, resultIndexOrigin, stepIndexAngle,
                resultIndexAngle, refSysOriginX, refSysOriginY, refSysAngle, refSysAxisOrientation, vaCoordSystemType, ivaData);

            string barcodeInfo = "";
            for (int i = 0; i < barcodeConfigs.Count;i++ )
            {
                Roi roiBarcode = new Roi();
                RectangleContour vaRect = new RectangleContour(barcodeConfigs[i].Rectangle.Left,
                    barcodeConfigs[i].Rectangle.Top, barcodeConfigs[i].Rectangle.Width, barcodeConfigs[i].Rectangle.Height);
                roiBarcode.Add(vaRect);
                // Reposition the region of interest based on the coordinate system.
                int coordSystemIndex = 0;
                Algorithms.TransformRoi(roiBarcode, new CoordinateTransform(ivaData.baseCoordinateSystems[coordSystemIndex], 
                    ivaData.MeasurementSystems[coordSystemIndex]));
                image.Overlays.Default.AddRoi(roiBarcode);
                // Reads the barcode from the image.
                BarcodeReport vaBarcode = Algorithms.ReadBarcode(image, barcodeConfigs[i].Type , roiBarcode, false);

                barcodeInfo += string.Format("{0},",vaBarcode.Text );

                roiBarcode.Dispose();
            }
            barcodeInfo = barcodeInfo.Substring(0, barcodeInfo.Length - 1);
            // Dispose the IVA_Data structure.
            ivaData.Dispose();
            return barcodeInfo;
        }        
        #endregion

        #region  ProcessDatamatrix
        public string ProcessDatamatrix(VisionImage image,TemplateConfig templateConfig,List <DataMatrixConfig> dataMatrixConfigs)
        {
            string dataMatrixInfo = string.Empty;
            // Initialize the IVA_Data structure to pass results and coordinate systems.
            IVA_Data ivaData = new IVA_Data(3, 1);

            // Creates a new, empty region of interest.
            Roi roi = new Roi();
            //// Creates a new RotatedRectangleContour using the given values.
            //RotatedRectangleContour vaRotatedRect = new RotatedRectangleContour(vaCenter, 1268, 1220, 0);
            // Creates a new RectangleContour using the given values.
            RectangleContour vaRotatedRect = new RectangleContour(templateConfig.Rectangle.Left,
                templateConfig.Rectangle.Top, templateConfig.Rectangle.Width, templateConfig.Rectangle.Height);
            roi.Add(vaRotatedRect);
            image.Overlays.Default.AddRoi(roi);
            // MatchPattern Grayscale
            MatchMode vaMode = MatchMode.RotationInvariant;
            bool vaSubpixelVal = false;
            int[] minAngleVals = { -30, 0 };
            int[] maxAngleVals = { 30, 0 };
            int vaNumMatchesRequested = 1;
            double vaMinMatchScore = 800;
            double vaOffsetX = 0;
            double vaOffsetY = 0;
            pmResults = IVA_MatchPattern(image, ivaData, templateConfig .TemplatePath , vaMode, vaSubpixelVal, 
                minAngleVals, maxAngleVals, vaNumMatchesRequested, vaMinMatchScore, roi, vaOffsetX, vaOffsetY, 0);

            foreach (PatternMatch match in pmResults)
            {
                image.Overlays.Default.AddPolygon(new PolygonContour(match.Corners), Rgb32Value.RedColor);
            }
            roi.Dispose();

            // Set Coordinate System
            int vaCoordSystemIndex = 0;
            int stepIndexOrigin = 0;
            int resultIndexOrigin = 1;
            int stepIndexAngle = 0;
            int resultIndexAngle = 3;
            double refSysOriginX = templateConfig.Position.X;
            double refSysOriginY = templateConfig.Position.Y;
            double refSysAngle = 0;
            AxisOrientation refSysAxisOrientation = AxisOrientation.Direct;
            int vaCoordSystemType = 3;
            IVA_CoordSys(vaCoordSystemIndex, stepIndexOrigin, resultIndexOrigin, stepIndexAngle, 
                resultIndexAngle, refSysOriginX, refSysOriginY, refSysAngle, refSysAxisOrientation, vaCoordSystemType, ivaData);

            for (int i = 0; i < dataMatrixConfigs.Count; i++)
            {
                // Creates a new, empty region of interest.
                Roi roiDM = new Roi();
                // Creates a new RectangleContour using the given values.
                RectangleContour vaRect = new RectangleContour(dataMatrixConfigs[i].Rectangle.Left,
                    dataMatrixConfigs[i].Rectangle.Top, dataMatrixConfigs[i].Rectangle.Width, dataMatrixConfigs[i].Rectangle.Height);

                roiDM.Add(vaRect);

                // Reposition the region of interest based on the coordinate system.
                int coordSystemIndex = 0;
                Algorithms.TransformRoi(roiDM, new CoordinateTransform(ivaData.baseCoordinateSystems[coordSystemIndex], ivaData.MeasurementSystems[coordSystemIndex]));
                image.Overlays.Default.AddRoi(roiDM);

                // Read DataMatrix Barcode
                DataMatrixDescriptionOptions vaDescriptionOptions = new DataMatrixDescriptionOptions();
                vaDescriptionOptions.AspectRatio = 0;
                vaDescriptionOptions.CellFill = DataMatrixCellFillMode.AutoDetect;
                uint matrixSizeColumns = 25;
                uint.TryParse(dataMatrixConfigs[i].MatrixSize.Split('X')[1], out matrixSizeColumns);
                vaDescriptionOptions.Columns = matrixSizeColumns;
                vaDescriptionOptions.MinimumBorderIntegrity = 90;
                vaDescriptionOptions.MirrorMode = DataMatrixMirrorMode.AutoDetect;
                vaDescriptionOptions.Polarity = dataMatrixConfigs[0].Polarity;
                vaDescriptionOptions.Rectangle = false;
                uint matrixSizeRows = 25;
                uint.TryParse(dataMatrixConfigs[i].MatrixSize.Split('X')[0], out matrixSizeRows);
                vaDescriptionOptions.Rows = matrixSizeRows;

                DataMatrixSizeOptions vaSizeOptions = new DataMatrixSizeOptions();
                vaSizeOptions.MaximumSize = 250;
                vaSizeOptions.MinimumSize = 50;
                vaSizeOptions.QuietZoneWidth = 0;

                DataMatrixSearchOptions vaSearchOptions = new DataMatrixSearchOptions();
                vaSearchOptions.CellFilterMode = DataMatrixCellFilterMode.AutoDetect;
                vaSearchOptions.CellSampleSize = dataMatrixConfigs[0].CellSize;
                vaSearchOptions.DemodulationMode = DataMatrixDemodulationMode.AutoDetect;
                vaSearchOptions.EdgeThreshold = 30;
                vaSearchOptions.InitialSearchVectorWidth = 5;
                vaSearchOptions.MaximumIterations = 150;
                vaSearchOptions.RotationMode = DataMatrixRotationMode.Unlimited;
                vaSearchOptions.SkewDegreesAllowed = 5;
                vaSearchOptions.SkipLocation = false;

                // Reads the data matrix from the image.
                vaDataMatrixReport = Algorithms.ReadDataMatrixBarcode(image, roiDM, DataMatrixGradingMode.None, 
                    vaDescriptionOptions, vaSizeOptions, vaSearchOptions);

                if (vaDataMatrixReport.Found)
                {
                    image.Overlays.Default.AddPolygon(new PolygonContour(vaDataMatrixReport.Corners), 
                        Rgb32Value.RedColor, DrawingMode.DrawValue);
                }
                dataMatrixInfo += string.Format("{0},", vaDataMatrixReport.StringData);
                roiDM.Dispose();
            }
            dataMatrixInfo = dataMatrixInfo.Substring(0, dataMatrixInfo.Length - 1);
            // Dispose the IVA_Data structure.
            ivaData.Dispose();

            // Return the palette type of the final image.
            return dataMatrixInfo;
        }
        #endregion

        #region  Process QR
        public string ProcessQR(VisionImage image,List<QRConfig> qRConfigs)
        {
            string qRInfo = string.Empty;
            // Initialize the IVA_Data structure to pass results and coordinate systems.
            IVA_Data ivaData = new IVA_Data(1, 0);
               
            for (int i = 0; i < qRConfigs.Count; i++)
            {
                // Creates a new, empty region of interest.
                Roi roi = new Roi();
                //// Creates a new RectangleContour using the given values.
                //RectangleContour vaRect = new RectangleContour(720, 96, 1792, 1240);
                RectangleContour vaRect = new RectangleContour(qRConfigs[i ].Rectangle.Left,
                    qRConfigs[i].Rectangle.Top, qRConfigs[i].Rectangle.Height, qRConfigs[i].Rectangle.Width);
                roi.Add(vaRect);
                image.Overlays.Default.AddRoi(roi);
                // Read QR Code
                QRDescriptionOptions vaQROptions = new QRDescriptionOptions();
                vaQROptions.Dimensions = qRConfigs[i].QRDimension;
                vaQROptions.MirrorMode = QRMirrorMode.AutoDetect;
                vaQROptions.ModelType = QRModelType.AutoDetect;
                vaQROptions.Polarity = qRConfigs[i].Polarity;
                QRSizeOptions vaQRSizeOptions = new QRSizeOptions(3, 15);
                QRSearchOptions vaQRSearchOptions = new QRSearchOptions();
                vaQRSearchOptions.CellFilterMode = QRCellFilterMode.AutoDetect;
                vaQRSearchOptions.CellSampleSize = qRConfigs[i].CellSize;
                vaQRSearchOptions.DemodulationMode = QRDemodulationMode.AutoDetect;
                vaQRSearchOptions.EdgeThreshold = 30;
                vaQRSearchOptions.RotationMode = QRRotationMode.Unlimited;
                vaQRSearchOptions.SkewDegreesAllowed = 10;
                vaQRSearchOptions.SkipLocation = false;
                vaQRCode = Algorithms.ReadQRCode(image, roi, vaQROptions, vaQRSizeOptions, vaQRSearchOptions);
                if (vaQRCode.Found)
                {
                    image.Overlays.Default.AddPolygon(new PolygonContour(vaQRCode.Corners), Rgb32Value.RedColor, DrawingMode.DrawValue);
                }
                System.Text.ASCIIEncoding vaASCIIEncoding = new System.Text.ASCIIEncoding();
                vaQRCodeData = vaASCIIEncoding.GetString(vaQRCode.GetData());

                qRInfo += string.Format("{0},", vaQRCodeData);

                roi.Dispose();
            }
                qRInfo = qRInfo.Substring(0, qRInfo.Length - 1);
            // Dispose the IVA_Data structure.
            ivaData.Dispose();

            // Return the palette type of the final image.
            return qRInfo;

        }
        #endregion

        #region  Process QR Coordinate
        /// <summary>
        /// Process QR code with coordinate
        /// </summary>
        /// <param name="image"></param>
        /// <param name="userProgram"></param>
        /// <returns>split with ','</returns>
        public string ProcessQRCoordinate(VisionImage image, UserProgram userProgram)
        {
            string qRInfo = string.Empty;
            TemplateConfig templateConfig = userProgram.TemplateConfig;
            List<QRConfig> qRConfigs = userProgram.QRConfigs;
            // Initialize the IVA_Data structure to pass results and coordinate systems.
            IVA_Data ivaData = new IVA_Data(3, 1);

            // Creates a new, empty region of interest.
            Roi roiFullRange = new Roi();
            // Creates a new RotatedRectangleContour using the given values.
            PointContour vaCenter = new PointContour(1405.5, 954);
            RotatedRectangleContour vaRotatedRect = new RotatedRectangleContour(vaCenter, 1661, 1184, 0);
            RectangleContour rectangle = new RectangleContour(templateConfig.Rectangle.Left, templateConfig.Rectangle.Top,
                templateConfig.Rectangle.Width, templateConfig.Rectangle.Height);
            roiFullRange.Add(rectangle);
            // MatchPattern Grayscale
            string vaTemplateFile = templateConfig.TemplatePath;
            MatchMode vaMode = MatchMode.RotationInvariant;
            bool vaSubpixelVal = false;
            int[] minAngleVals = { -20, 0 };
            int[] maxAngleVals = { 20, 0 };
            int vaNumMatchesRequested = 1;
            double vaMinMatchScore = 800;
            double vaOffsetX = 0;
            double vaOffsetY = 0;
            pmResults = IVA_MatchPattern(image, ivaData, vaTemplateFile, vaMode, vaSubpixelVal, 
                minAngleVals, maxAngleVals, vaNumMatchesRequested, vaMinMatchScore, roiFullRange, vaOffsetX, vaOffsetY, 0);
            if (pmResults.Count < 1)
            {
                return string.Empty;
            }
            foreach (PatternMatch match in pmResults)
            {
                image.Overlays.Default.AddPolygon(new PolygonContour(match.Corners), Rgb32Value.RedColor);
            }
            roiFullRange.Dispose();

            // Set Coordinate System
            int vaCoordSystemIndex = 0;
            int stepIndexOrigin = 0;
            int resultIndexOrigin = 1;
            int stepIndexAngle = 0;
            int resultIndexAngle = 3;
            double refSysOriginX = templateConfig.Position.X;
            double refSysOriginY = templateConfig.Position.Y;
            double refSysAngle = 0;
            AxisOrientation refSysAxisOrientation = AxisOrientation.Direct;
            int vaCoordSystemType = 3;
            IVA_CoordSys(vaCoordSystemIndex, stepIndexOrigin, resultIndexOrigin, stepIndexAngle, 
                resultIndexAngle, refSysOriginX, refSysOriginY, refSysAngle, refSysAxisOrientation, vaCoordSystemType, ivaData);

            for (int i = 0; i < qRConfigs.Count; i++)
            {
                // Creates a new, empty region of interest.
                Roi roi = new Roi();
                // Creates a new RectangleContour using the given values.
                RectangleContour vaRect = new RectangleContour(qRConfigs[i].Rectangle.Left,
                    qRConfigs[i].Rectangle.Top, qRConfigs[i].Rectangle.Width, qRConfigs[i].Rectangle.Height);
                roi.Add(vaRect);
                // Reposition the region of interest based on the coordinate system.
                int coordSystemIndex = 0;
                Algorithms.TransformRoi(roi, new CoordinateTransform(ivaData.baseCoordinateSystems[coordSystemIndex],
                    ivaData.MeasurementSystems[coordSystemIndex]));
                // Read QR Code
                QRDescriptionOptions vaQROptions = new QRDescriptionOptions();
                vaQROptions.Dimensions = qRConfigs[i].QRDimension;
                vaQROptions.MirrorMode = QRMirrorMode.AutoDetect;
                vaQROptions.ModelType = QRModelType.AutoDetect;
                vaQROptions.Polarity = qRConfigs[i].Polarity;
                QRSizeOptions vaQRSizeOptions = new QRSizeOptions(3, 15);
                QRSearchOptions vaQRSearchOptions = new QRSearchOptions();
                vaQRSearchOptions.CellFilterMode = QRCellFilterMode.AutoDetect;
                vaQRSearchOptions.CellSampleSize = qRConfigs[i].CellSize;
                vaQRSearchOptions.DemodulationMode = QRDemodulationMode.AutoDetect;
                vaQRSearchOptions.EdgeThreshold = 30;
                vaQRSearchOptions.RotationMode = QRRotationMode.Unlimited;
                vaQRSearchOptions.SkewDegreesAllowed = 5;
                vaQRSearchOptions.SkipLocation = false;
                vaQRCode = Algorithms.ReadQRCode(image, roi, vaQROptions, vaQRSizeOptions, vaQRSearchOptions);

                if (vaQRCode.Found)
                {
                    image.Overlays.Default.AddPolygon(new PolygonContour(vaQRCode.Corners), Rgb32Value.RedColor, DrawingMode.DrawValue);
                }

                System.Text.ASCIIEncoding vaASCIIEncoding = new System.Text.ASCIIEncoding();
                vaQRCodeData = vaASCIIEncoding.GetString(vaQRCode.GetData());
                qRInfo += string.Format("{0},", vaQRCodeData);
                roi.Dispose();
            }
            if (!string.IsNullOrEmpty(qRInfo))
            {
                qRInfo = qRInfo.Substring(0, qRInfo.Length - 1);
            }
            // Dispose the IVA_Data structure.
            ivaData.Dispose();
            // Return the palette type of the final image.
            return qRInfo;
        }
        #endregion
    }
}