using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Net;
using HtmlAgilityPack;
using System.Text.RegularExpressions;
using System.Globalization;
using System.Xml.Serialization;
using System.IO;


namespace Lotto
{
    internal class MyWebClient : WebClient
    {
        public void startDownload()
        {
            WebClient client = new WebClient();
            client.DownloadProgressChanged += new DownloadProgressChangedEventHandler(client_DownloadProgressChanged);
            client.DownloadFileCompleted += new System.ComponentModel.AsyncCompletedEventHandler(client_DownloadFileCompleted);
            client.DownloadFileAsync(new Uri("http://megalotto.pl/wyniki/multi-multi/losowania-od-28-Czerwca-1995-do-1-Lipca-2018.html"), @"lua.syn");
        }
        void client_DownloadProgressChanged(object sender, DownloadProgressChangedEventArgs e)
        {

            System.Net.WebClient wc = new System.Net.WebClient();
            wc.OpenRead("http://megalotto.pl/wyniki/multi-multi/losowania-od-28-Czerwca-1995-do-1-Lipca-2018.html");
            Int64 bytes_total = Convert.ToInt64(wc.ResponseHeaders["Content-Length"]);


            double bytesIn = double.Parse(e.BytesReceived.ToString());
            double totalBytes = double.Parse(e.TotalBytesToReceive.ToString());
            double percentage = bytesIn / totalBytes * 100;
            System.Console.WriteLine( "Downloaded " + e.BytesReceived + " of " + bytes_total);
           // progressBar1.Value = int.Parse(Math.Truncate(percentage).ToString());
        }
        void client_DownloadFileCompleted(object sender, System.ComponentModel.AsyncCompletedEventArgs e)
        {
            System.Console.WriteLine("Completed");
        }

    }
}
