using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Xml.Serialization;
using NationalInstruments.Vision.Analysis;

namespace LabelingVisualIdentification
{
    [XmlRoot("UserProgramConfig")]
    public class UserProgramConfig
    {
        [XmlArray("UserPrograms"), XmlArrayItem("UserProgram", typeof(UserProgram))]
        public List<UserProgram> UserProgram { get; set; }

        public void Save()
        {
            object obj=new object ();
            lock (obj)
            {
                string configPath = string.Format(@"{0}ConfigManager\UserPrograms.xml", AppDomain.CurrentDomain.BaseDirectory);
                ObjectXmlSerializer<UserProgramConfig>.Save(this, configPath);
            }
        }
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
        /// 模板信息
        /// </summary>
        [XmlElementAttribute("TemplateConfig")]
        public TemplateConfig TemplateConfig { get; set; }
        /// <summary>
        /// 一维码信息
        /// </summary>
        [XmlArray ("BarcodeConfigs"), XmlArrayItem ("BarcodeConfig",typeof (BarcodeConfig ))]
        public List<BarcodeConfig> BarcodeConfigs { get; set; }
        /// <summary>
        /// DataMatrix信息
        /// </summary>
        [XmlArray("DataMatrixConfigs"), XmlArrayItem("DataMatrixConfig", typeof(DataMatrixConfig))]
        public List<DataMatrixConfig> DataMatrixConfigs { get; set; }
        /// <summary>
        /// QR信息
        /// </summary>        
        [XmlArray("QRConfigs"), XmlArrayItem("QRConfig", typeof(QRConfig))]
        public List<QRConfig> QRConfigs { get; set; }
       
    }
    public class TemplateConfig
    {
        [XmlElementAttribute("TemplatePath")]
        public string TemplatePath { get; set; }

        [XmlElementAttribute("Position")]
        public VisionPosition Position { get; set; }

        [XmlElementAttribute("Rectangle")]
        public VisionRectangle Rectangle { get; set; }
    }

    public class BarcodeConfig
    {
        /// <summary>
        /// 条码名称
        /// </summary>
        [XmlAttribute("Name")]
        public string Name { get; set; }

        [XmlAttribute("Index")]
        public int Index { get; set; }

        [XmlElementAttribute("Type")]
        public BarcodeTypes Type { get; set; }

        [XmlElementAttribute("Rectangle")]
        public VisionRectangle Rectangle { get; set; }
    }

    public class DataMatrixConfig
    {
        [XmlAttribute("Name")]
        public string Name { get; set; }

        [XmlAttribute("Index")]
        public int Index { get; set; }

        [XmlElementAttribute("MatrixSize")]
        public string MatrixSize { get; set; }

        [XmlElementAttribute("Polarity")]
        public DataMatrixPolarity  Polarity { get; set; }

        [XmlElementAttribute("CellSize")]
        public DataMatrixCellSampleSize CellSize { get; set; }

        [XmlElementAttribute("Rectangle")]
        public VisionRectangle Rectangle { get; set; }
    }

    public class QRConfig
    {
        [XmlAttribute("Name")]
        public string Name { get; set; }

        [XmlAttribute("Index")]
        public int Index { get; set; }

        [XmlElementAttribute("QRDimension")]
        public QRDimension QRDimension { get; set; }

        [XmlElementAttribute("Polarity")]
        public QRPolarity Polarity { get; set; }

        [XmlElementAttribute("CellSize")]
        public QRCellSampleSize CellSize { get; set; }

        [XmlElementAttribute("Rectangle")]
        public VisionRectangle Rectangle { get; set; }
    }

    /// <summary>
    /// 视觉用到的位置信息
    /// </summary>
    public class VisionPosition
    {
        [XmlAttribute("X")]
        public double X { get; set; }

        [XmlAttribute("Y")]
        public double Y { get; set; }
    }
    /// <summary>
    /// 视觉用到的矩形位置
    /// </summary>
    public class VisionRectangle
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
