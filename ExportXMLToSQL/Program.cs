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
            ExportDrawsToSQL exportToSQL = new ExportDrawsToSQL(new SQLUtils.ConnectionString(@"Praca-Laptop\SQLEXPRESS", "WynikiLotto", true, "RawData"));
            exportToSQL.ExportDrawsListToSQL(LoadXML.DeserializeXML());
            Console.WriteLine("Done");
            Console.ReadKey();
        }
    }
}
