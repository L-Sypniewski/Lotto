using System;
using System.Collections.Generic;
using System.Xml.Serialization;
using System.IO;
using System.Linq;
using System.Diagnostics;

namespace Lotto
{
    public static class LoadXML
    {
        private static string defaultFileXMLDirectory { get; set; }
        private static string defaultFilePythonScriptDirectory { get; set; }
        private static string defaultVirtualEnvDirectory { get; set; }

        // Enum used to define which function from the python script should be called
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
            oRootAttr.ElementName = "Draws";
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

        // Deserializes XML file and returns its content to List<> of draws
        public static List<Draw> DeserializeXML(string directory = null, string fileName = "filename.xml")
        {
            string XMLPath;
            if (directory == null)
                XMLPath = defaultFileXMLDirectory + fileName;
            else
                XMLPath = directory + fileName;
            List<Draw> draws;
            using (var reader = new StreamReader(XMLPath))
            {
                XmlSerializer deserializer = new XmlSerializer(typeof(List<Draw>),
                    new XmlRootAttribute("Draws"));
                draws = (List<Draw>)deserializer.Deserialize(reader);
            }

            return draws;
        }

        // Runs python script which dowloads date from server and saves it to xml
        public static string[] RunDownloadScript(string directory = null, string fileName = "filename.xml", string virtualEnvPath = null, Functions function = Functions.UPDATE_XML)
        {
            string XMLPath;
            if (directory == null)
                XMLPath = defaultFileXMLDirectory + fileName;
            else
                XMLPath = directory + fileName;
            ProcessStartInfo start = new ProcessStartInfo
            {
                FileName = virtualEnvPath ?? defaultVirtualEnvDirectory, // https://docs.microsoft.com/en-us/dotnet/csharp/language-reference/operators/null-conditional-operator
                Arguments = string.Format("{0} -{1} \"{2}\"", defaultFilePythonScriptDirectory, function, XMLPath),
                UseShellExecute = false,
                RedirectStandardOutput = true,
                CreateNoWindow = true,
                WindowStyle = System.Diagnostics.ProcessWindowStyle.Hidden
            };

            Process proc = new Process();
            proc.StartInfo = start;
            proc.Start();

            using (StreamReader q = proc.StandardOutput)
            {
                while (!proc.HasExited)
                    Console.WriteLine(q.ReadLine());
            }
            Console.WriteLine(string.Format("Function {0} has finished\n", function.ToString()));
            return new string[] { XMLPath, defaultFilePythonScriptDirectory };
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

        static LoadXML()
        {
            defaultFileXMLDirectory = Path.GetFullPath(Path.Combine(System.AppDomain.CurrentDomain.BaseDirectory, @"..\..\..\ExportXMLToSQL\XML\"));
            defaultFilePythonScriptDirectory = "\"" + Path.GetFullPath(Path.Combine(System.AppDomain.CurrentDomain.BaseDirectory, @"..\..\..\FetchData\FetchData\FetchData.py")) + "\"";
            defaultVirtualEnvDirectory = Path.GetFullPath(Path.Combine(System.AppDomain.CurrentDomain.BaseDirectory, @"..\..\..\FetchData\venv_Lotto\Scripts\python.exe"));
        }
    }
}