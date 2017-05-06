using System;
using System.Collections.Generic;
using System.Text;
using NationalInstruments.Vision.Analysis;
using NationalInstruments.Vision.Acquisition.Imaqdx;

namespace LabelingVisualIdentification
{
    class Common
    {                
        public static ImaqdxSession session;
        public static string barcodeFormat = "1D Barcode";
        public static  string cameraID = "USER_ID";
        //Barcode
        public static string path = @"C:\Users\plc\Desktop\barcodePrograming\Programming\template\123.png";
        public static string templatePath = @"C:\Users\plc\Desktop\barcodePrograming\Programming\template\123.png";
        public static double  barcode1Left = 0.0;
        public static double  barcode1Top = 0.0;
        public static double barcode1Width = 0.0;
        public static double barcode1Height = 0.0;
        public static double barcode2Left = 0.0;
        public static double barcode2Top = 0.0;
        public static double barcode2Width = 0.0;
        public static double barcode2Height = 0.0;

        public static double barcode3Left = 0.0;
        public static double barcode3Top = 0.0;
        public static double barcode3Width = 0.0;
        public static double barcode3Height = 0.0;
        public static double barcode4Left = 0.0;
        public static double barcode4Top = 0.0;
        public static double barcode4Width = 0.0;
        public static double barcode4Height = 0.0;

        public static double barcode5Left = 0.0;
        public static double barcode5Top = 0.0;
        public static double barcode5Width = 0.0;
        public static double barcode5Height = 0.0;
        public static double barcode6Left = 0.0;
        public static double barcode6Top = 0.0;
        public static double barcode6Width = 0.0;
        public static double barcode6Height = 0.0;

        public static string barcode = "";
        public static bool barFound = false;

        public static int  barcodeNumber = 1;
        public static BarcodeTypes barcodeTypes=BarcodeTypes.Code128;
        public static double templatePositionX = 1000;
        public static double templatePositionY = 1000;

        //Datametrix        
        public static string datamatrixCode = "";
        public static bool dmFound = false;
        public static double dmRectLeft = 1920;
        public static double dmRectTop = 504;
        public static double dmRectWidth = 372;
        public static double dmRectHeight = 344;

        public static uint matrixSize = 24;
        public static DataMatrixPolarity polarity = DataMatrixPolarity.BlackDataOnWhiteBackground;
        public static DataMatrixCellSampleSize cellSampleSize = DataMatrixCellSampleSize.Size3x3;

        //QR
        public static string qrCode = "";
        public static bool qrFound = false;
        public static double QRRectLeft = 1220;
        public static double QRRectTop = 314;
        public static double QRRectWidth = 994;
        public static double QRRectHeight = 994;

        public static QRDimension QRDimensions = QRDimension.Size25x25;
        public static QRPolarity QRpolaritys = QRPolarity.BlackOnWhite ;
        public static QRCellSampleSize QRCellSampleSizes = QRCellSampleSize.Size3x3;
        
        //Pattern rectangle
        public static double patternRectLeft = 1920;
        public static double patternRectTop = 504;
        public static double patternRectWidth = 372;
        public static double patternRectHeight = 344;
        
        
        
    }
}
