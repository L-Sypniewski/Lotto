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
            ExportDrawsToSQL con = new ExportDrawsToSQL(connectionString);

            DateTime dt1 = new DateTime(2008, 6, 29);
            DateTime dt2 = new DateTime(2009, 4, 1);

            string url = SQLUtils.CreateLinkToSourceHTML(dt1, dt2);
            //ConvertHTMLDrawsToXML.SaveXMLToFile("losowania.xml", ConvertHTMLDrawsToXML.SOURCE_LINK_TO_ALL, localHTMLSource: false, saveHTMLSourceToLocalFile: false, sourceHTMLPath: null);

            List<Losowanie> lista = ConvertHTMLDrawsToXML.GetDrawsList(ConvertHTMLDrawsToXML.SOURCE_LINK_TO_ALL);

            foreach (Losowanie d in ConvertHTMLDrawsToXML.FilterDrawsLinq(lista, DateTime.Today.Subtract(TimeSpan.FromDays(1))))
            {
                Console.WriteLine(d.ToString());
            }
            


            Console.WriteLine("Done");
            Console.ReadKey();
        }
    }
}
