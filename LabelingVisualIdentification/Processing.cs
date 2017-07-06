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
    static class Processing
    {
        public static Collection<PatternMatch> pmResults;
        public static BarcodeReport vaBarcode;
        public static BarcodeReport vaBarcode2;
        public static BarcodeReport vaBarcode3;
        public static BarcodeReport vaBarcode4;
        public static BarcodeReport vaBarcode5;
        public static BarcodeReport vaBarcode6;

        public static DataMatrixReport vaDataMatrixReport;

        public static QRReport vaQRCode;
        public static string vaQRCodeData;

        #region  Pattern match
        private static Collection<PatternMatch> IVA_MatchPattern(VisionImage image,
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
        private static void IVA_CoordSys(int coordSysIndex,
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
        public static PaletteType Process1DBarcode(VisionImage image)
        {
            // Initialize the IVA_Data structure to pass results and coordinate systems.
            IVA_Data ivaData = new IVA_Data(5, 1);

            // Creates a new, empty region of interest.
            Roi roi = new Roi();

            //// Creates a new RotatedRectangleContour using the given values.
            //PointContour vaCenter = new PointContour(1294, 972);
            //RotatedRectangleContour vaRotatedRect = new RotatedRectangleContour(vaCenter, 2548, 1904, 0);

            // Creates a new RectangleContour using the given values.
            RectangleContour vaRotatedRect = new RectangleContour(Common.patternRectLeft, Common.patternRectTop, Common.patternRectWidth, Common.patternRectHeight);
            roi.Add(vaRotatedRect);

            image.Overlays.Default.AddRoi(roi);

            // MatchPattern Grayscale
            string vaTemplateFile = Common.templatePath;//@"C:\Users\plc\Desktop\barcodePrograming\Programming\template\123.png";
            MatchMode vaMode = MatchMode.RotationInvariant;
            bool vaSubpixelVal = false;
            int[] minAngleVals = { -30, 0 };
            int[] maxAngleVals = { 30, 0 };
            int vaNumMatchesRequested = 1;
            double vaMinMatchScore = 800;
            double vaOffsetX = 0;
            double vaOffsetY = 0;
            pmResults = IVA_MatchPattern(image, ivaData, vaTemplateFile, vaMode, vaSubpixelVal, minAngleVals, maxAngleVals, vaNumMatchesRequested, vaMinMatchScore, roi, vaOffsetX, vaOffsetY, 0);


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
            double refSysOriginX = Common.templatePositionX;//1728.5;//patternPositionX;
            double refSysOriginY = Common.templatePositionY;//822.5; //patternPositionY;
            double refSysAngle = 0;
            AxisOrientation refSysAxisOrientation = AxisOrientation.Direct;
            int vaCoordSystemType = 3;
            IVA_CoordSys(vaCoordSystemIndex, stepIndexOrigin, resultIndexOrigin, stepIndexAngle, resultIndexAngle, refSysOriginX, refSysOriginY, refSysAngle, refSysAxisOrientation, vaCoordSystemType, ivaData);

            string barcodeInfo = "";
            if (Common.barcodeNumber > 0)
            {
                // Creates a new, empty region of interest.
                Roi roi2 = new Roi();
                // Creates a new RectangleContour using the given values.
                RectangleContour vaRect = new RectangleContour(Common.barcode1Left, Common.barcode1Top, Common.barcode1Width, Common.barcode1Height);
                roi2.Add(vaRect);

                // Reposition the region of interest based on the coordinate system.
                int coordSystemIndex = 0;
                Algorithms.TransformRoi(roi2, new CoordinateTransform(ivaData.baseCoordinateSystems[coordSystemIndex], ivaData.MeasurementSystems[coordSystemIndex]));
                image.Overlays.Default.AddRoi(roi2);
                // Reads the barcode from the image.
                vaBarcode = Algorithms.ReadBarcode(image, Common.barcodeTypes, roi2, false);

                barcodeInfo = barcodeInfo + vaBarcode.Text + ",";

                roi2.Dispose();
            }
            if (Common.barcodeNumber > 1)
            {
                // Creates a new, empty region of interest.
                Roi roi3 = new Roi();
                // Creates a new RectangleContour using the given values.
                RectangleContour vaRect2 = new RectangleContour(Common.barcode2Left, Common.barcode2Top, Common.barcode2Width, Common.barcode2Height);
                roi3.Add(vaRect2);
                // Reposition the region of interest based on the coordinate system.
                int coordSystemIndex2 = 0;
                Algorithms.TransformRoi(roi3, new CoordinateTransform(ivaData.baseCoordinateSystems[coordSystemIndex2], ivaData.MeasurementSystems[coordSystemIndex2]));
                image.Overlays.Default.AddRoi(roi3);
                // Reads the barcode from the image.
                vaBarcode2 = Algorithms.ReadBarcode(image, Common.barcodeTypes, roi3, false);

                barcodeInfo = barcodeInfo + vaBarcode2.Text + ",";

                roi3.Dispose();
            }

            if (Common.barcodeNumber > 2)
            {
                // Creates a new, empty region of interest.
                Roi roi4 = new Roi();
                // Creates a new RectangleContour using the given values.
                RectangleContour vaRect3 = new RectangleContour(Common.barcode3Left, Common.barcode3Top, Common.barcode3Width, Common.barcode3Height);
                roi4.Add(vaRect3);
                // Reposition the region of interest based on the coordinate system.
                int coordSystemIndex3 = 0;
                Algorithms.TransformRoi(roi4, new CoordinateTransform(ivaData.baseCoordinateSystems[coordSystemIndex3], ivaData.MeasurementSystems[coordSystemIndex3]));
                image.Overlays.Default.AddRoi(roi4);
                // Reads the barcode from the image.
                vaBarcode3 = Algorithms.ReadBarcode(image, Common.barcodeTypes, roi4, false);
                barcodeInfo = barcodeInfo + vaBarcode3.Text + ",";

                roi4.Dispose();
            }

            if (Common.barcodeNumber > 3)
            {
                // Creates a new, empty region of interest.
                Roi roi5 = new Roi();
                // Creates a new RectangleContour using the given values.
                RectangleContour vaRect4 = new RectangleContour(Common.barcode4Left, Common.barcode4Top, Common.barcode4Width, Common.barcode4Height);
                roi5.Add(vaRect4);
                // Reposition the region of interest based on the coordinate system.
                int coordSystemIndex3 = 0;
                Algorithms.TransformRoi(roi5, new CoordinateTransform(ivaData.baseCoordinateSystems[coordSystemIndex3], ivaData.MeasurementSystems[coordSystemIndex3]));
                image.Overlays.Default.AddRoi(roi5);
                // Reads the barcode from the image.
                vaBarcode4 = Algorithms.ReadBarcode(image, Common.barcodeTypes, roi5, false);
                barcodeInfo = barcodeInfo + vaBarcode4.Text + ",";

                roi5.Dispose();
            }

            if (Common.barcodeNumber > 4)
            {
                // Creates a new, empty region of interest.
                Roi roi6 = new Roi();
                // Creates a new RectangleContour using the given values.
                RectangleContour vaRect5 = new RectangleContour(Common.barcode5Left, Common.barcode5Top, Common.barcode5Width, Common.barcode5Height);
                roi6.Add(vaRect5);
                // Reposition the region of interest based on the coordinate system.
                int coordSystemIndex3 = 0;
                Algorithms.TransformRoi(roi6, new CoordinateTransform(ivaData.baseCoordinateSystems[coordSystemIndex3], ivaData.MeasurementSystems[coordSystemIndex3]));
                image.Overlays.Default.AddRoi(roi6);
                // Reads the barcode from the image.
                vaBarcode5 = Algorithms.ReadBarcode(image, Common.barcodeTypes, roi6, false);
                barcodeInfo = barcodeInfo + vaBarcode5.Text + ",";

                roi6.Dispose();
            }

            if (Common.barcodeNumber > 5)
            {
                // Creates a new, empty region of interest.
                Roi roi7 = new Roi();
                // Creates a new RectangleContour using the given values.
                RectangleContour vaRect6 = new RectangleContour(Common.barcode6Left, Common.barcode6Top, Common.barcode6Width, Common.barcode6Height);
                roi7.Add(vaRect6);
                // Reposition the region of interest based on the coordinate system.
                int coordSystemIndex3 = 0;
                Algorithms.TransformRoi(roi7, new CoordinateTransform(ivaData.baseCoordinateSystems[coordSystemIndex3], ivaData.MeasurementSystems[coordSystemIndex3]));
                image.Overlays.Default.AddRoi(roi7);
                // Reads the barcode from the image.
                vaBarcode6 = Algorithms.ReadBarcode(image, Common.barcodeTypes, roi7, false);
                barcodeInfo = barcodeInfo + vaBarcode6.Text + ",";

                roi7.Dispose();
            }
            if (barcodeInfo.Substring(barcodeInfo.Length - 1, 1) == ",")
            {
                barcodeInfo = barcodeInfo.Substring(0, barcodeInfo.Length - 1);
            }

            Common.barcode = barcodeInfo;

            // Dispose the IVA_Data structure.
            ivaData.Dispose();

            // Return the palette type of the final image.
            return PaletteType.Gray;

        }

        /// <summary>
        /// Author by Kiven
        /// </summary>
        /// <param name="image"></param>
        /// <param name="userProgram"></param>
        /// <returns></returns>
        public static string Process1DBarcode(VisionImage image,UserProgram  userProgram)
        {
            string barcodeInfo = "";
            TemplateConfig templateConfig = userProgram.TemplateConfig;
            List<BarcodeConfig> barcodeConfigs = userProgram.BarcodeConfigs;

            // Initialize the IVA_Data structure to pass results and coordinate systems.
            IVA_Data ivaData = new IVA_Data(5, 1);

            // Creates a new, empty region of interest.
            Roi roiFullRange = new Roi();

            //// Creates a new RotatedRectangleContour using the given values.
            //PointContour vaCenter = new PointContour(1294, 972);
            //RotatedRectangleContour vaRotatedRect = new RotatedRectangleContour(vaCenter, 2548, 1904, 0);

            // Creates a new RectangleContour using the given values.
            RectangleContour vaRotatedRect = new RectangleContour(templateConfig.Rectangle.Left, 
                templateConfig.Rectangle.Top, templateConfig.Rectangle.Width, templateConfig.Rectangle.Height);
            roiFullRange.Add(vaRotatedRect);

            image.Overlays.Default.AddRoi(roiFullRange);

            // MatchPattern Grayscale
            string vaTemplateFile = templateConfig.TemplatePath;
            MatchMode vaMode = MatchMode.RotationInvariant;
            bool vaSubpixelVal = false;
            int[] minAngleVals = { -30, 0 };
            int[] maxAngleVals = { 30, 0 };
            int vaNumMatchesRequested = 1;
            double vaMinMatchScore = 800;
            double vaOffsetX = 0;
            double vaOffsetY = 0;
            pmResults = IVA_MatchPattern(image, ivaData, vaTemplateFile, vaMode, vaSubpixelVal, minAngleVals, maxAngleVals, vaNumMatchesRequested, vaMinMatchScore, roiFullRange, vaOffsetX, vaOffsetY, 0);

            roiFullRange.Dispose();

            foreach (PatternMatch match in pmResults)
            {
                image.Overlays.Default.AddPolygon(new PolygonContour(match.Corners), Rgb32Value.RedColor);
            }

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
            IVA_CoordSys(vaCoordSystemIndex, stepIndexOrigin, resultIndexOrigin, stepIndexAngle, resultIndexAngle, refSysOriginX, refSysOriginY, refSysAngle, refSysAxisOrientation, vaCoordSystemType, ivaData);

            for (int i = 0; i < barcodeConfigs.Count; i++)
            {
                // Creates a new, empty region of interest.
                Roi roi = new Roi();
                // Creates a new RectangleContour using the given values.
                RectangleContour vaRect = new RectangleContour(barcodeConfigs[i].Rectangle.Left, barcodeConfigs[i].Rectangle.Top , barcodeConfigs[i].Rectangle.Width , barcodeConfigs[i].Rectangle.Height );
                roi.Add(vaRect);

                // Reposition the region of interest based on the coordinate system.
                int coordSystemIndex = 0;
                Algorithms.TransformRoi(roi, new CoordinateTransform(ivaData.baseCoordinateSystems[coordSystemIndex], ivaData.MeasurementSystems[coordSystemIndex]));
                image.Overlays.Default.AddRoi(roi);
                // Reads the barcode from the image.
                vaBarcode = Algorithms.ReadBarcode(image, barcodeConfigs[i].Type , roi, false);

                barcodeInfo += string.Format("{0},", vaBarcode.Text);
                roi.Dispose();
            }
            
            if (barcodeInfo.Substring(barcodeInfo.Length - 1, 1) == ",")
            {
                barcodeInfo = barcodeInfo.Substring(0, barcodeInfo.Length - 1);
            }

            // Dispose the IVA_Data structure.
            ivaData.Dispose();



            // Return the palette type of the final image.
            return barcodeInfo;

        }
        #endregion


        #region  ProcessDatamatrix
        public static PaletteType ProcessDatamatrix(VisionImage image)
        {
            Common.dmFound = false;
            // Initialize the IVA_Data structure to pass results and coordinate systems.
            IVA_Data ivaData = new IVA_Data(3, 1);

            // Creates a new, empty region of interest.
            Roi roi = new Roi();
            //// Creates a new RotatedRectangleContour using the given values.
            //PointContour vaCenter = new PointContour(1666, 874);
            //RotatedRectangleContour vaRotatedRect = new RotatedRectangleContour(vaCenter, 1268, 1220, 0);
            // Creates a new RectangleContour using the given values.
            RectangleContour vaRotatedRect = new RectangleContour(Common.patternRectLeft, Common.patternRectTop, Common.patternRectWidth, Common.patternRectHeight);
            roi.Add(vaRotatedRect);
            image.Overlays.Default.AddRoi(roi);
            // MatchPattern Grayscale
            string vaTemplateFile = Common.templatePath;
            MatchMode vaMode = MatchMode.RotationInvariant;
            bool vaSubpixelVal = false;
            int[] minAngleVals = { -30, 0 };
            int[] maxAngleVals = { 30, 0 };
            int vaNumMatchesRequested = 1;
            double vaMinMatchScore = 800;
            double vaOffsetX = 0;
            double vaOffsetY = 0;
            pmResults = IVA_MatchPattern(image, ivaData, vaTemplateFile, vaMode, vaSubpixelVal, minAngleVals, maxAngleVals, vaNumMatchesRequested, vaMinMatchScore, roi, vaOffsetX, vaOffsetY, 0);

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
            double refSysOriginX = Common.templatePositionX;//1728.5;//patternPositionX;
            double refSysOriginY = Common.templatePositionY;//822.5; //patternPositionY;
            double refSysAngle = 0;
            AxisOrientation refSysAxisOrientation = AxisOrientation.Direct;
            int vaCoordSystemType = 3;
            IVA_CoordSys(vaCoordSystemIndex, stepIndexOrigin, resultIndexOrigin, stepIndexAngle, resultIndexAngle, refSysOriginX, refSysOriginY, refSysAngle, refSysAxisOrientation, vaCoordSystemType, ivaData);

            // Creates a new, empty region of interest.
            Roi roi2 = new Roi();
            // Creates a new RectangleContour using the given values.
            RectangleContour vaRect = new RectangleContour(Common.dmRectLeft, Common.dmRectTop, Common.dmRectWidth, Common.dmRectHeight);

            roi2.Add(vaRect);

            // Reposition the region of interest based on the coordinate system.
            int coordSystemIndex = 0;
            Algorithms.TransformRoi(roi2, new CoordinateTransform(ivaData.baseCoordinateSystems[coordSystemIndex], ivaData.MeasurementSystems[coordSystemIndex]));
            image.Overlays.Default.AddRoi(roi2);

            // Read DataMatrix Barcode
            DataMatrixDescriptionOptions vaDescriptionOptions = new DataMatrixDescriptionOptions();
            vaDescriptionOptions.AspectRatio = 0;
            vaDescriptionOptions.CellFill = DataMatrixCellFillMode.AutoDetect;
            vaDescriptionOptions.Columns = Common.matrixSize;
            vaDescriptionOptions.Ecc = DataMatrixEcc.AutoDetect;
            vaDescriptionOptions.MinimumBorderIntegrity = 90;
            vaDescriptionOptions.MirrorMode = DataMatrixMirrorMode.AutoDetect;
            vaDescriptionOptions.Polarity = Common.polarity;//DataMatrixPolarity.BlackDataOnWhiteBackground ;
            vaDescriptionOptions.Rectangle = false;
            vaDescriptionOptions.Rows = Common.matrixSize;


            DataMatrixSizeOptions vaSizeOptions = new DataMatrixSizeOptions();
            vaSizeOptions.MaximumSize = 250;
            vaSizeOptions.MinimumSize = 50;
            vaSizeOptions.QuietZoneWidth = 0;

            DataMatrixSearchOptions vaSearchOptions = new DataMatrixSearchOptions();
            vaSearchOptions.CellFilterMode = DataMatrixCellFilterMode.AutoDetect;
            vaSearchOptions.CellSampleSize = Common.cellSampleSize;//DataMatrixCellSampleSize.Size3x3;
            vaSearchOptions.DemodulationMode = DataMatrixDemodulationMode.AutoDetect;
            vaSearchOptions.EdgeThreshold = 30;
            vaSearchOptions.InitialSearchVectorWidth = 5;
            vaSearchOptions.MaximumIterations = 150;
            vaSearchOptions.RotationMode = DataMatrixRotationMode.Unlimited;
            vaSearchOptions.SkewDegreesAllowed = 5;
            vaSearchOptions.SkipLocation = false;

            // Reads the data matrix from the image.
            vaDataMatrixReport = Algorithms.ReadDataMatrixBarcode(image, roi2, DataMatrixGradingMode.None, vaDescriptionOptions, vaSizeOptions, vaSearchOptions);

            Common.datamatrixCode = vaDataMatrixReport.StringData;
            if (vaDataMatrixReport.Found)
            {
                Common.dmFound = true;
                image.Overlays.Default.AddPolygon(new PolygonContour(vaDataMatrixReport.Corners), Rgb32Value.RedColor, DrawingMode.DrawValue);
            }

            roi2.Dispose();

            // Dispose the IVA_Data structure.
            ivaData.Dispose();

            // Return the palette type of the final image.
            return PaletteType.Gray;

        }

        /// <summary>
        /// author by kiven
        /// </summary>
        /// <param name="image"></param>
        /// <param name="userProgram"></param>
        /// <returns></returns>
        public static PaletteType ProcessDatamatrix(VisionImage image,UserProgram userProgram)
        {
            TemplateConfig templateConfig = userProgram .TemplateConfig ;
            List<DataMatrixConfig> dataMatrixConfigs = userProgram.DataMatrixConfigs;

            // Initialize the IVA_Data structure to pass results and coordinate systems.
            IVA_Data ivaData = new IVA_Data(3, 1);

            // Creates a new, empty region of interest.
            Roi roiFullRange = new Roi();
            //// Creates a new RotatedRectangleContour using the given values.
            //PointContour vaCenter = new PointContour(1666, 874);
            //RotatedRectangleContour vaRotatedRect = new RotatedRectangleContour(vaCenter, 1268, 1220, 0);
            // Creates a new RectangleContour using the given values.
            RectangleContour vaRotatedRect = new RectangleContour(templateConfig.Rectangle.Left,templateConfig.Rectangle.Top , 
                templateConfig.Rectangle.Width, templateConfig.Rectangle.Height);
            roiFullRange.Add(vaRotatedRect);
            image.Overlays.Default.AddRoi(roiFullRange);
            // MatchPattern Grayscale
            string vaTemplateFile = templateConfig.TemplatePath;
            MatchMode vaMode = MatchMode.RotationInvariant;
            bool vaSubpixelVal = false;
            int[] minAngleVals = { -30, 0 };
            int[] maxAngleVals = { 30, 0 };
            int vaNumMatchesRequested = 1;
            double vaMinMatchScore = 800;
            double vaOffsetX = 0;
            double vaOffsetY = 0;
            pmResults = IVA_MatchPattern(image, ivaData, vaTemplateFile, vaMode, vaSubpixelVal, minAngleVals, maxAngleVals, vaNumMatchesRequested, vaMinMatchScore, roiFullRange, vaOffsetX, vaOffsetY, 0);

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
            double refSysOriginY = templateConfig.Position.Y ;
            double refSysAngle = 0;
            AxisOrientation refSysAxisOrientation = AxisOrientation.Direct;
            int vaCoordSystemType = 3;
            IVA_CoordSys(vaCoordSystemIndex, stepIndexOrigin, resultIndexOrigin, stepIndexAngle, resultIndexAngle, refSysOriginX, refSysOriginY, refSysAngle, refSysAxisOrientation, vaCoordSystemType, ivaData);

            for (int i = 0; i < dataMatrixConfigs.Count; i++)
            {
                // Creates a new, empty region of interest.
                Roi roi = new Roi();
                // Creates a new RectangleContour using the given values.
                RectangleContour vaRect = new RectangleContour(dataMatrixConfigs[i].Rectangle.Left, 
                    dataMatrixConfigs[i].Rectangle.Top , dataMatrixConfigs[i].Rectangle.Width , dataMatrixConfigs[i].Rectangle.Height );

                roi.Add(vaRect);

                // Reposition the region of interest based on the coordinate system.
                int coordSystemIndex = 0;
                Algorithms.TransformRoi(roi, new CoordinateTransform(ivaData.baseCoordinateSystems[coordSystemIndex], ivaData.MeasurementSystems[coordSystemIndex]));
                image.Overlays.Default.AddRoi(roi);

                // Read DataMatrix Barcode
                DataMatrixDescriptionOptions vaDescriptionOptions = new DataMatrixDescriptionOptions();
                vaDescriptionOptions.AspectRatio = 0;
                vaDescriptionOptions.CellFill = DataMatrixCellFillMode.AutoDetect;
                vaDescriptionOptions.Columns = Common.matrixSize;
                vaDescriptionOptions.Ecc = DataMatrixEcc.AutoDetect;
                vaDescriptionOptions.MinimumBorderIntegrity = 90;
                vaDescriptionOptions.MirrorMode = DataMatrixMirrorMode.AutoDetect;
                vaDescriptionOptions.Polarity = Common.polarity;//DataMatrixPolarity.BlackDataOnWhiteBackground ;
                vaDescriptionOptions.Rectangle = false;
                vaDescriptionOptions.Rows = Common.matrixSize;


                DataMatrixSizeOptions vaSizeOptions = new DataMatrixSizeOptions();
                vaSizeOptions.MaximumSize = 250;
                vaSizeOptions.MinimumSize = 50;
                vaSizeOptions.QuietZoneWidth = 0;

                DataMatrixSearchOptions vaSearchOptions = new DataMatrixSearchOptions();
                vaSearchOptions.CellFilterMode = DataMatrixCellFilterMode.AutoDetect;
                vaSearchOptions.CellSampleSize = Common.cellSampleSize;//DataMatrixCellSampleSize.Size3x3;
                vaSearchOptions.DemodulationMode = DataMatrixDemodulationMode.AutoDetect;
                vaSearchOptions.EdgeThreshold = 30;
                vaSearchOptions.InitialSearchVectorWidth = 5;
                vaSearchOptions.MaximumIterations = 150;
                vaSearchOptions.RotationMode = DataMatrixRotationMode.Unlimited;
                vaSearchOptions.SkewDegreesAllowed = 5;
                vaSearchOptions.SkipLocation = false;

                // Reads the data matrix from the image.
                vaDataMatrixReport = Algorithms.ReadDataMatrixBarcode(image, roi, DataMatrixGradingMode.None, vaDescriptionOptions, vaSizeOptions, vaSearchOptions);

                Common.datamatrixCode = vaDataMatrixReport.StringData;
                if (vaDataMatrixReport.Found)
                {
                    image.Overlays.Default.AddPolygon(new PolygonContour(vaDataMatrixReport.Corners), Rgb32Value.RedColor, DrawingMode.DrawValue);
                }

                roi.Dispose();
            }
            // Dispose the IVA_Data structure.
            ivaData.Dispose();

            // Return the palette type of the final image.
            return PaletteType.Gray;

        }

        #endregion


        #region  Process QR
        public static PaletteType ProcessQR(VisionImage image)
        {
            Common.qrFound = false;
            // Initialize the IVA_Data structure to pass results and coordinate systems.
            IVA_Data ivaData = new IVA_Data(1, 0);

            // Creates a new, empty region of interest.
            Roi roi = new Roi();
            //// Creates a new RectangleContour using the given values.
            //RectangleContour vaRect = new RectangleContour(720, 96, 1792, 1240);

            RectangleContour vaRect = new RectangleContour(Common.QRRectLeft, Common.QRRectTop, Common.QRRectWidth, Common.QRRectHeight);
            roi.Add(vaRect);
            image.Overlays.Default.AddRoi(roi);
            // Read QR Code
            QRDescriptionOptions vaQROptions = new QRDescriptionOptions();
            vaQROptions.Dimensions = Common.QRDimensions;//QRDimension.Size25x25;
            vaQROptions.MirrorMode = QRMirrorMode.AutoDetect;
            vaQROptions.ModelType = QRModelType.AutoDetect;
            vaQROptions.Polarity = Common.QRpolaritys;//QRPolarity.BlackOnWhite;
            QRSizeOptions vaQRSizeOptions = new QRSizeOptions(3, 15);
            QRSearchOptions vaQRSearchOptions = new QRSearchOptions();
            vaQRSearchOptions.CellFilterMode = QRCellFilterMode.AutoDetect;
            vaQRSearchOptions.CellSampleSize = Common.QRCellSampleSizes;//QRCellSampleSize.Size3x3;
            vaQRSearchOptions.DemodulationMode = QRDemodulationMode.AutoDetect;
            vaQRSearchOptions.EdgeThreshold = 30;
            vaQRSearchOptions.RotationMode = QRRotationMode.Unlimited;
            vaQRSearchOptions.SkewDegreesAllowed = 10;
            vaQRSearchOptions.SkipLocation = false;
            vaQRCode = Algorithms.ReadQRCode(image, roi, vaQROptions, vaQRSizeOptions, vaQRSearchOptions);
            if (vaQRCode.Found)
            {
                Common.qrFound = true;
                image.Overlays.Default.AddPolygon(new PolygonContour(vaQRCode.Corners), Rgb32Value.RedColor, DrawingMode.DrawValue);
            }
            System.Text.ASCIIEncoding vaASCIIEncoding = new System.Text.ASCIIEncoding();
            vaQRCodeData = vaASCIIEncoding.GetString(vaQRCode.GetData());

            Common.qrCode = vaQRCodeData;

            roi.Dispose();

            // Dispose the IVA_Data structure.
            ivaData.Dispose();

            // Return the palette type of the final image.
            return PaletteType.Gray;

        }
        #endregion


        #region  Process QR Coordinate
        public static PaletteType ProcessQRCoordinate(VisionImage image)
        {
            // Initialize the IVA_Data structure to pass results and coordinate systems.
            IVA_Data ivaData = new IVA_Data(3, 1);

            // Creates a new, empty region of interest.
            Roi roi = new Roi();
            // Creates a new RotatedRectangleContour using the given values.
            PointContour vaCenter = new PointContour(1405.5, 954);
            RotatedRectangleContour vaRotatedRect = new RotatedRectangleContour(vaCenter, 1661, 1184, 0);
            roi.Add(vaRotatedRect);
            // MatchPattern Grayscale
            string vaTemplateFile = "C:\\Users\\Misadmin\\Desktop\\picture\\11.png";
            MatchMode vaMode = MatchMode.RotationInvariant;
            bool vaSubpixelVal = false;
            int[] minAngleVals = { -20, 0 };
            int[] maxAngleVals = { 20, 0 };
            int vaNumMatchesRequested = 1;
            double vaMinMatchScore = 800;
            double vaOffsetX = 0;
            double vaOffsetY = 0;
            pmResults = IVA_MatchPattern(image, ivaData, vaTemplateFile, vaMode, vaSubpixelVal, minAngleVals, maxAngleVals, vaNumMatchesRequested, vaMinMatchScore, roi, vaOffsetX, vaOffsetY, 0);
            roi.Dispose();

            // Set Coordinate System
            int vaCoordSystemIndex = 0;
            int stepIndexOrigin = 0;
            int resultIndexOrigin = 1;
            int stepIndexAngle = 0;
            int resultIndexAngle = 3;
            double refSysOriginX = 1330;
            double refSysOriginY = 769;
            double refSysAngle = 0;
            AxisOrientation refSysAxisOrientation = AxisOrientation.Direct;
            int vaCoordSystemType = 3;
            IVA_CoordSys(vaCoordSystemIndex, stepIndexOrigin, resultIndexOrigin, stepIndexAngle, resultIndexAngle, refSysOriginX, refSysOriginY, refSysAngle, refSysAxisOrientation, vaCoordSystemType, ivaData);

            // Creates a new, empty region of interest.
            Roi roi2 = new Roi();
            // Creates a new RectangleContour using the given values.
            RectangleContour vaRect = new RectangleContour(1564, 604, 328, 308);
            roi2.Add(vaRect);
            // Reposition the region of interest based on the coordinate system.
            int coordSystemIndex = 0;
            Algorithms.TransformRoi(roi2, new CoordinateTransform(ivaData.baseCoordinateSystems[coordSystemIndex], ivaData.MeasurementSystems[coordSystemIndex]));
            // Read QR Code
            QRDescriptionOptions vaQROptions = new QRDescriptionOptions();
            vaQROptions.Dimensions = QRDimension.Size25x25;
            vaQROptions.MirrorMode = QRMirrorMode.AutoDetect;
            vaQROptions.ModelType = QRModelType.AutoDetect;
            vaQROptions.Polarity = QRPolarity.BlackOnWhite;
            QRSizeOptions vaQRSizeOptions = new QRSizeOptions(3, 15);
            QRSearchOptions vaQRSearchOptions = new QRSearchOptions();
            vaQRSearchOptions.CellFilterMode = QRCellFilterMode.AutoDetect;
            vaQRSearchOptions.CellSampleSize = QRCellSampleSize.Size3x3;
            vaQRSearchOptions.DemodulationMode = QRDemodulationMode.AutoDetect;
            vaQRSearchOptions.EdgeThreshold = 30;
            vaQRSearchOptions.RotationMode = QRRotationMode.Unlimited;
            vaQRSearchOptions.SkewDegreesAllowed = 5;
            vaQRSearchOptions.SkipLocation = false;
            vaQRCode = Algorithms.ReadQRCode(image, roi2, vaQROptions, vaQRSizeOptions, vaQRSearchOptions);

            if (vaQRCode.Found)
            {
                Common.qrFound = true;
                image.Overlays.Default.AddPolygon(new PolygonContour(vaQRCode.Corners), Rgb32Value.RedColor, DrawingMode.DrawValue);
            }

            System.Text.ASCIIEncoding vaASCIIEncoding = new System.Text.ASCIIEncoding();
            vaQRCodeData = vaASCIIEncoding.GetString(vaQRCode.GetData());

            roi2.Dispose();

            // Dispose the IVA_Data structure.
            ivaData.Dispose();

            // Return the palette type of the final image.
            return PaletteType.Gray;

        }
        #endregion
    }
}

