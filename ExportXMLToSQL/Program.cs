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
            LoadXML.RunDownloadScript(function : LoadXML.Functions.UPDATE_XML);
            LoadXML.RunDownloadScript(function: LoadXML.Functions.MAKE_PRETTY_XML);
        }
    }
}
