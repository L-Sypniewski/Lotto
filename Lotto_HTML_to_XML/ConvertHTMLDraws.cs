using System;
using System.Collections.Generic;
using HtmlAgilityPack;
using System.Text.RegularExpressions;
using System.Globalization;
using System.Xml.Serialization;
using System.IO;
using System.Linq;


namespace Lotto
{
    public static class ConvertHTMLDraws
    {     

        // Serializes List<Losowanie> to an XML and saves it to a file
        private static void SerializeToXMLAndSaveToFile(List<Draw> drawsList, string xmlPath = @"Losowania.xml")
        {
            XmlRootAttribute oRootAttr = new XmlRootAttribute();
            oRootAttr.ElementName = "Losowania";
            oRootAttr.IsNullable = true;
            XmlSerializer oSerializer = new XmlSerializer(typeof(List<Draw>), oRootAttr);
            StreamWriter oStreamWriter = null;
            try
            {
                oStreamWriter = new StreamWriter(xmlPath);
                oSerializer.Serialize(oStreamWriter, drawsList);
            }
            catch (Exception oException)
            {
                Console.WriteLine("Exception: " + oException.Message);
            }
            finally
            {
                if (null != oStreamWriter)
                {
                    oStreamWriter.Dispose();
                }
            }
        }

        //Deserializes XML file and returns its content to List<> of draws
        public static List<Draw> DeserializeXML()
        {
            string path = @"F:\filename_pretty.xml";
            List<Draw> draws;
            using (var reader = new StreamReader(path))
            {
                XmlSerializer deserializer = new XmlSerializer(typeof(List<Draw>),
                    new XmlRootAttribute("Draws"));
                draws = (List<Draw>)deserializer.Deserialize(reader);
            }

            return draws;
        }

        static public List<Draw> FilterDraws(List<Draw> list, int drawNumber)
        {
            var collection = list.Select(c => c).Where(c => c.DrawNo == drawNumber);
            return collection.ToList<Draw>();
        }
        static public List<Draw> FilterDraws(List<Draw> list, DateTime drawDate)
        {
            DateTime from = new DateTime(drawDate.Year, drawDate.Month, drawDate.Day, 0, 0, 0);
            DateTime to = new DateTime(drawDate.Year, drawDate.Month, drawDate.Day, 23, 59, 59);
            var collection = list.Select(c => c).Where(c => c.DrawDate >= from && c.DrawDate <= to);
            return collection.ToList<Draw>();
        }
    }
}
