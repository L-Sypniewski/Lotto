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
    static class ConvertHTMLDrawsToXML
    {
        #region Consts
         const int INDEX_OF_FIRST_DRAW_WITH_PLUS = 4347;
         const int AMOUNT_OF_NUMBERS_IN_A_SINGLE_DRAW = 20;        
         const string REGEX = @"(?<NrLosowania>\d{1,5})\. (?<DzienLosowania>\d{1,2}-\d{1,2}-\d{4})(?<Liczba1>\d{1,2}) (?<Liczba2>\d{1,2}) (?<Liczba3>\d{1,2}) (?<Liczba4>\d{1,2}) (?<Liczba5>\d{1,2}) (?:(?<GodzinaLosowania>\d{2}:\d{2})?)(?<Liczba6>\d{1,2}) (?<Liczba7>\d{1,2}) (?<Liczba8>\d{1,2}).*?(?<Liczba9>\d{1,2}) (?<Liczba10>\d{1,2}) (?<Liczba11>\d{1,2}) (?<Liczba12>\d{1,2}) (?<Liczba13>\d{1,2}) (?<Liczba14>\d{1,2}) (?<Liczba15>\d{1,2}) (?<Liczba16>\d{1,2}) (?<Liczba17>\d{1,2}) (?<Liczba18>\d{1,2}) (?<Liczba19>\d{1,2}) (?<Liczba20>\d{1,2}).*(?:plus:(?<Plus>\d{1,2})?)";
        public const string SOURCE_LINK_TO_ALL = "http://megalotto.pl/wyniki/multi-multi/losowania-od-28-Czerwca-1995-do-1-Lipca-2018"; // "http://megalotto.pl/wyniki/multi-multi"
        #endregion

        // Puts HTML code of each draw into a List. Regex against these HTML codes is executed later.
        private static void AddSeparateValuesToList(List<string> listSeparatedDraws, int iterateFrom, int iterateTo, HtmlNodeCollection nodesSeparateDraws, HtmlNodeCollection nodesPluses = null)
        {
            if (nodesPluses == null)
            {
                for (int i = iterateFrom; i < iterateTo; i++)
                {
                    // Plus can be null
                    listSeparatedDraws.Add(string.Format("{0} plus:", nodesSeparateDraws[i].InnerText));
                }
            }
            else
            {
                for (int i = iterateFrom; i < iterateTo; i++)
                {
                    listSeparatedDraws.Add(string.Format("{0} plus:{1}", nodesSeparateDraws[i].InnerText, nodesPluses[i].InnerText));
                }
            }
        }

        private static void AddValuesToList(List<string> listSeparateDraws, HtmlNodeCollection nodesSeparateDraws, HtmlNodeCollection nodesPluses)
        {
            int HowManyIterationsWithPlus = Int32.Parse(new Regex(@"(?<NrLosowania>\d{1,5})\.").Match(nodesSeparateDraws[0].InnerText).Groups[1].Value) - INDEX_OF_FIRST_DRAW_WITH_PLUS + 1;
            AddSeparateValuesToList(listSeparateDraws, 0, HowManyIterationsWithPlus, nodesSeparateDraws, nodesPluses);
            AddSeparateValuesToList(listSeparateDraws, HowManyIterationsWithPlus, nodesSeparateDraws.Count, nodesSeparateDraws);
        }

        // Loads HTML source and uses XPath to separate HTML code of each draw and puts it into a List<string>
        private static List<string> HTMLDivideIntoSeparateDraws(string url, bool useLocalFile = false , bool saveSourceToLocalFile = false, string localFilePath = null)
        {
            // Saving source to local file makes no sense when a local source is used
            if ((useLocalFile == true) && (saveSourceToLocalFile == true))
                throw new ArgumentException("A parameter saveSourceToLocalFile cannot be set to true when a parameter useLocalFile is also set to true", "saveSourceToLocalFile");
            // There is no use to set localFilePath when both useLocalFile and saveSourceToLocalFile are set to false
            else if ((localFilePath != null) && ((useLocalFile == false) && (saveSourceToLocalFile == false)))
                throw new ArgumentException("There is no point in setting localFilePath when both useLocalFile and saveSourceToLocalFile are set to false", "localFilePath");

            // Loading HTML either from a website or from a previously downloaded file
            HtmlDocument document = new HtmlDocument();
            if (useLocalFile == false)
                if (saveSourceToLocalFile == true)
                {
                    DownloadSource.DownloadAll(url, localFilePath);
                    document.Load(localFilePath);
                }
                else
                    document.Load(DownloadSource.DownloadAll(url));
            else
                document.Load(localFilePath);

            //Extracting separate draws. A draw consists of 20 numbers
            HtmlNodeCollection nodesSeparateDraws = document.DocumentNode.SelectNodes("//ul[@style='position: relative;']");
            HtmlNodeCollection nodesPluses = document.DocumentNode.SelectNodes("(//ul[@style='position: relative;']/div/li[contains(@class, 'plus')]) | (//ul[@style='position: relative;']/div/li/span[contains(@class, 'plus')])");
            List<string> listSeparatedDraws = new List<string>();

            AddValuesToList(listSeparatedDraws, nodesSeparateDraws, nodesPluses);
            return listSeparatedDraws;
        }

        // Extracts data from of each draw from HTML code and puts it to a List
        private static List<Losowanie> ParseHTML(List<string> listSeparatedDraws)
        {
            List<Losowanie> losowaniaList = new List<Losowanie>(listSeparatedDraws.Count);
            ushort nrLosowania = 0;
            DateTime dataLosowania = new DateTime();
            byte? plus = null;

            foreach (var separatedDraw in listSeparatedDraws)
            {
                List<byte> liczbyList = new List<byte>(20);
                Regex r = new Regex(REGEX, RegexOptions.Singleline);
                Match m = r.Match(separatedDraw);
                GroupCollection groups = m.Groups;

                // Loop used to add extracted numbers to a list
                foreach (string groupName in r.GetGroupNames())
                {
                    if (groupName.Contains("Liczba"))
                        liczbyList.Add(Byte.Parse(groups[groupName].Value));
                }

                nrLosowania = ushort.Parse(m.Groups["NrLosowania"].Value);

                // At the beginning draws were conducted once a day, so and hour of a draw was not provided until there were two draws each day
                if (m.Groups["GodzinaLosowania"].Success == true)
                    dataLosowania = DateTime.ParseExact(m.Groups["DzienLosowania"].Value + " " + m.Groups["GodzinaLosowania"].Value, "dd-MM-yyyy HH:mm", CultureInfo.CurrentUICulture);
                else
                    dataLosowania = DateTime.ParseExact(m.Groups["DzienLosowania"].Value, "dd-MM-yyyy", CultureInfo.CurrentUICulture);

                // At the beginning Plus number was not being chosen
                if (m.Groups["Plus"].Success == true)
                    plus = Byte.Parse(m.Groups["Plus"].Value);
                else
                    plus = null;
                losowaniaList.Add(new Losowanie(nrLosowania, dataLosowania, plus, liczbyList));
            }
            return losowaniaList;
        }

        // Serializes List<Losowanie> to an XML and saves it to a file
        private static void SerializeToXMLAndSaveToFile(List<Losowanie> drawsList, string xmlPath = @"Losowania.xml")
        {
            XmlRootAttribute oRootAttr = new XmlRootAttribute();
            oRootAttr.ElementName = "Losowania";
            oRootAttr.IsNullable = true;
            XmlSerializer oSerializer = new XmlSerializer(typeof(List<Losowanie>), oRootAttr);
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

        //Saves draws data to an xml file
        public static void SaveXMLToFile(string xmlPath, string url, bool localHTMLSource = false,  bool saveHTMLSourceToLocalFile = false, string sourceHTMLPath = null)
        {
            SerializeToXMLAndSaveToFile(ParseHTML(HTMLDivideIntoSeparateDraws(url, localHTMLSource, saveHTMLSourceToLocalFile, sourceHTMLPath)), xmlPath);
        }

        //returns List<> of draws
        public static List<Losowanie> GetDrawsList(string url, bool useLocalFile = false, string localFilePath = null, bool saveSourceToLocalFile = false)
        {
            return ParseHTML(HTMLDivideIntoSeparateDraws(url, useLocalFile, saveSourceToLocalFile, localFilePath));
        }

        static public List<Losowanie> FilterDrawsLinq(List<Losowanie> list, int drawNumber)
        {
            var collection = list.Select(c => c).Where(c => c.NrLosowania == drawNumber);
            return collection.ToList<Losowanie>();
        }
        static public List<Losowanie> FilterDrawsLinq(List<Losowanie> list, DateTime drawDate)
        {
            DateTime from = new DateTime(drawDate.Year, drawDate.Month, drawDate.Day, 0, 0, 0);
            DateTime to = new DateTime(drawDate.Year, drawDate.Month, drawDate.Day, 23, 59, 59);
            var collection = list.Select(c => c).Where(c => c.DataLosowania >= from && c.DataLosowania <= to);
            return collection.ToList<Losowanie>();
        }
    }
}
