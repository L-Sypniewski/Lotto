using System.Net;
using System.IO;

namespace Lotto
{
    static class DownloadSource
    {
        // Downloads all draws without saving an HTML source to a disk
        public static Stream DownloadAll(string url)
        {
            using (WebClient client = new WebClient())
            {
                return client.OpenRead(url);
            }
        }

        // Downloads all draws and saves an HTML source to a disk
        public static void DownloadAll(string url, string sFilePath)
        {
            using (WebClient client = new WebClient())
            {
               client.DownloadFile(url, sFilePath);
            }
        }
    }
}
