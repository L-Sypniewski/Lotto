using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;

namespace Lotto
{
    static class Program
    {
        static void Main(string[] args)
        {
            SQLUtils.ConnectionString connectionString = new SQLUtils.ConnectionString(@"Praca-Laptop\SQLEXPRESS", "WynikiLotto", true);
            ExportAllDrawsToSQL con = new ExportAllDrawsToSQL(connectionString);
            ConverterRangeHTMLDrawsToXML conv = new ConverterRangeHTMLDrawsToXML();

            DateTime dt1 = new DateTime(2009, 10, 23);
            DateTime dt2 = new DateTime(2012, 3, 25);

            string url = SQLUtils.CreateLinkToSourceHTML(dt1, dt2);

            //DateTime data;
            //int numer;

            //SQLQueries.procedure_GetLastDrawDate(connectionString.ToString(), out data, out numer);
            //ConvertAllHTMLDrawsToXML.SaveXMLToFile("losowania.xml", DownloadHTML.SOURCE_LINK_TO_ALL, offlineHTMLSource: false, saveHTMLSourceToFile: true, sourceHTMLPath: "Testy.html");
            conv.SaveXMLToFile("losowania.xml", url, offlineHTMLSource: false, saveHTMLSourceToFile: true, sourceHTMLPath: "Testy.html");
            //Console.WriteLine(data.ToString() + " " + numer.ToString());
            Console.WriteLine(SQLUtils.IsServerConnected(connectionString).ToString());
            con.ExportAllDrawsListToSQL("RawData");
            Console.WriteLine("Done");
            Console.ReadKey();

            //Console.WriteLine(SQLUtils.CreateNewDatabase(connectionString, SQLUtils.databaseFolderPath).ToString());
            //DateTime dt = SQLQueries.procedure_GetLastDrawDate(new SQLUtils.ConnectionString(@"Praca-Laptop\SQLEXPRESS", "WynikiLotto", true).ToString());
            //Console.WriteLine(dt.ToString());
            Console.ReadKey();
        }
    }
}
