using System;
using System.Collections.Generic;
using System.Xml.Serialization;

namespace Lotto
{
    [Serializable]
    public class Draw
    {
        [XmlAttribute("ID")]
        public ushort DrawNo { get; set; }

        [XmlElement("Date")]
        public DateTime DrawDate { get; set; }

        [XmlElement(ElementName = "Plus", IsNullable = true)]
        public byte? Plus { get; set; }

        [XmlArray("Numbers")]
        [XmlArrayItem("N", typeof(byte))]
        public List<byte> Numbers { get; set; }

        public Draw()
        {
            Numbers = new List<byte>(20);
        }

        public Draw(ushort dDrawNumber,DateTime dDrawDate , byte? dPlus, List<byte> dNumbers)
        {
            Numbers = new List<byte>(20);
            Numbers = dNumbers;
            DrawNo = dDrawNumber;
            DrawDate = dDrawDate;
            Plus = dPlus;
        }

        public override string ToString()
        {
            return string.Format("DrawNo: {0}, DrawDate: {1}, Plus: {2}", this.DrawNo.ToString(), this.DrawDate.ToString(), this.Plus.ToString());
        }
    }
}

