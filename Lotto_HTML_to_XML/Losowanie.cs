using System;
using System.Collections.Generic;
using System.Xml.Serialization;

namespace Lotto
{
    [Serializable]
    public class Losowanie
    {
        [XmlAttribute("ID")]
        public ushort NrLosowania { get; set; }

        [XmlElement("Data")]
        public DateTime DataLosowania { get; set; }

        [XmlElement(ElementName = "Plus", IsNullable = true)]
        public byte? Plus { get; set; }

        [XmlArray("Liczby")]
        [XmlArrayItem("L")]
        public List<byte> Liczby { get; set; }

        public Losowanie()
        {
            Liczby = new List<byte>(20);
        }

        public Losowanie(ushort dNrLosowania,DateTime dDataLosowania , byte? dPlus, List<byte> dLiczby)
        {
            Liczby = new List<byte>(20);
            Liczby = dLiczby;
            NrLosowania = dNrLosowania;
            DataLosowania = dDataLosowania;
            Plus = dPlus;
        }
    }
}

