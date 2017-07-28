using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Globalization;

namespace Lotto
{
    static class Program
    {
        static void Main(string[] args)
        {
            //SQLUtils.ConnectionString connectionString = new SQLUtils.ConnectionString(@"Praca-Laptop\SQLEXPRESS", "WynikiLotto", true, "RawData");
            //ExportDrawsToSQL con = new ExportDrawsToSQL(connectionString);
            //Console.WriteLine("Starting");
            //Console.WriteLine(SQLUtils.IsServerConnected(connectionString));
            //con.ExportDrawsListToSQL();
            //ConvertHTMLDrawsToXML.SaveXMLToFile("losowania.xml", null, localHTMLSource: true, saveHTMLSourceToLocalFile: false, sourceHTMLPath: "Test.html");
            DateTime dt = DateTime.ParseExact("03/04/2004 08:30:00", "dd/MM/yyyy HH:mm:ss", null); //DateTime.ParseExact(x, "dd/mm/yyyy", null)
            Console.WriteLine(dt.Minute);

            //DateTime dt1 = new DateTime(2008, 6, 29);
            //DateTime dt2 = new DateTime(2009, 4, 1);

            //string url = SQLUtils.CreateLinkToSourceHTML(dt1, dt2);

            //List<Losowanie> lista = ConvertHTMLDrawsToXML.GetDrawsList(ConvertHTMLDrawsToXML.SOURCE_LINK_TO_ALL);

            //foreach (Losowanie d in ConvertHTMLDrawsToXML.FilterDrawsLinq(lista, DateTime.Today.Subtract(TimeSpan.FromDays(1))))
            //{
            //    Console.WriteLine(d.ToString());
            //}



            Console.WriteLine("Done");
            Console.ReadKey();
        }
    }
}
