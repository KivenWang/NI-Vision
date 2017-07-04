using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Xml.Serialization;

namespace LabelingVisualIdentification
{
    [XmlRoot("UserPrograms")]
    public class UserPrograms
    {
        [XmlArrayItem("UserProgram")]
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
        public string PatternRectangle { get; set; }
    }
    public class TemplatePosition
    {
        [XmlAttribute("X")]
        public double X { get; set; }

        [XmlAttribute("Y")]
        public double Y { get; set; }
    }
}
