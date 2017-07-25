
using System.Net;
using System.IO;

namespace Lotto
{
    class DownloadHTML
    {
        public const string SOURCE_LINK_TO_ALL = "http://megalotto.pl/wyniki/multi-multi/losowania-od-28-Czerwca-1995-do-1-Lipca-2018"; // "http://megalotto.pl/wyniki/multi-multi"

        // Downloads all draws without saving an HTML source to a disk
        public static Stream DownloadAll(string url)
        {
            using (WebClient client = new WebClient()) // WebClient class inherits IDisposable
            {
                return client.OpenRead(url);
            }
        }

        // Downloads all draws and saves an HTML source to a disk
        public static void DownloadAll(string url, string sFilePath)
        {
            using (WebClient client = new WebClient()) // WebClient class inherits IDisposable
            {
               client.DownloadFile(url, sFilePath);
            }
        }
    }
}
