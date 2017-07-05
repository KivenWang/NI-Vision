using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Xml.Serialization;

namespace LabelingVisualIdentification
{
    [XmlRoot("UserProgramConfig")]
    public class UserPrograms
    {
        [XmlArray("UserPrograms"),XmlArrayItem ("UserProgram",typeof (UserProgram) )]
        public List<UserProgram> UserProgram { get; set; }
    }

    public class UserProgram
    {
        /// <summary>
        /// 程序Id
        /// </summary>
        [XmlAttribute("Id")]
        public int Id { get; set; }
        /// <summary>
        /// 程序名称
        /// </summary>
        [XmlAttribute("Name")]
        public string Name { get; set; }

        /// <summary>
        /// 条码类型
        /// </summary>
        [XmlElementAttribute("BarcodeFormat")]
        public string BarcodeFormat { get; set; }

        /// <summary>
        /// 条码数量
        /// </summary>
        [XmlElementAttribute("BarcodeNumber")]
        public int BarcodeNumber { get; set; }

        /// <summary>
        /// 模板坐标位置
        /// </summary>
        [XmlElementAttribute("TemplatePosition")]
        public TemplatePosition templatePosition { get; set; }

        /// <summary>
        /// 条码类型
        /// </summary>
        [XmlElementAttribute("BarcodeTypes")]
        public string BarcodeTypes { get; set; }

        /// <summary>
        /// 模板矩形
        /// </summary>
        [XmlElementAttribute("PatternRectangle")]
        public PatternRectangle PatternRectangle { get; set; }

        /// <summary>
        /// 模板路径
        /// </summary>
        [XmlElementAttribute("TemplatePath")]
        public string TemplatePath { get; set; }

        /// <summary>
        /// 模板矩形
        /// </summary>
        [XmlElementAttribute("Barcode1Rectangle")]
        public Barcode1Rectangle Barcode1Rectangle { get; set; }

        /// <summary>
        /// QR矩形
        /// </summary>
        [XmlElementAttribute("QRRectangle")]
        public QRRectangle QRRectangle { get; set; }

        /// <summary>
        /// QRMatrixSize
        /// </summary>
        [XmlElementAttribute("QRMatrixSize")]
        public string QRMatrixSize { get; set; }

        /// <summary>
        /// 模板矩形
        /// </summary>
        [XmlElementAttribute("QRPolarity")]
        public string QRPolarity { get; set; }

        /// <summary>
        /// 模板矩形
        /// </summary>
        [XmlElementAttribute("QRCellSampleSize")]
        public string QRCellSampleSize { get; set; }
    }
    public class TemplatePosition
    {
        [XmlAttribute("X")]
        public double X { get; set; }

        [XmlAttribute("Y")]
        public double Y { get; set; }
    }
    public class PatternRectangle
    {
        [XmlAttribute("Left")]
        public double Left { get; set; }

        [XmlAttribute("Top")]
        public double Top { get; set; }

        [XmlAttribute("Width")]
        public double Width { get; set; }

        [XmlAttribute("Height")]
        public double Height { get; set; }

    }

    public class Barcode1Rectangle
    {
        [XmlAttribute("Left")]
        public double Left { get; set; }

        [XmlAttribute("Top")]
        public double Top { get; set; }

        [XmlAttribute("Width")]
        public double Width { get; set; }

        [XmlAttribute("Height")]
        public double Height { get; set; }

    }

    public class QRRectangle
    {
        [XmlAttribute("Left")]
        public double Left { get; set; }

        [XmlAttribute("Top")]
        public double Top { get; set; }

        [XmlAttribute("Width")]
        public double Width { get; set; }

        [XmlAttribute("Height")]
        public double Height { get; set; }

    }
}
