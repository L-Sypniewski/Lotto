using System;
using System.Collections.Generic;
using HtmlAgilityPack;
using System.Text.RegularExpressions;
using System.Globalization;
using System.Xml.Serialization;
using System.IO;
using System.Linq;
using System.Diagnostics;
namespace Lotto
{
    public static class LoadXML
    {     
        public enum Functions
        {
            UPDATE_XML,
            DOWNLOAD_ALL_DRAWS,
            MAKE_PRETTY_XML
        };

        // Serializes List<Draw> to an XML and saves it to a file
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

        //Runs python script which dowloads date from server and saves it to xml
        public static void RunDownloadScript(string directory = null, string fileName = "filename.xml", Functions function = Functions.UPDATE_XML)
        {
            string folderPath = directory;
            if (folderPath == null)
                folderPath = Path.GetFullPath(Path.Combine(System.AppDomain.CurrentDomain.BaseDirectory, @"..\..\XML\"));
            string filePath = fileName;
            string XMLpath = folderPath + filePath;
            string ScriptPath = "\"C:\\Users\\Sypcio\\Documents\\Dropbox\\Docs\\Programming\\!Visual Studio 2015\\Projects\\Lotto\\FetchData\\FetchData\\FetchData.py\"";

            ProcessStartInfo start = new ProcessStartInfo();
            start.FileName = "python.exe";
            start.Arguments = string.Format("{0} -{1} \"{2}\"", ScriptPath, function, XMLpath);
            start.UseShellExecute = false;
            start.RedirectStandardOutput = true;
            start.CreateNoWindow = true;
            start.WindowStyle = System.Diagnostics.ProcessWindowStyle.Hidden;

            Process proc = new Process();
            proc.StartInfo = start;
            proc.Start();

            using (StreamReader q = proc.StandardOutput)
            {
                while (!proc.HasExited)
                    Console.WriteLine(q.ReadLine());
            }
            Console.WriteLine("Press any key to exit.");
            Console.ReadKey();
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


