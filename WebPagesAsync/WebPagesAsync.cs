using System;
using System.Collections.Generic;
using System.Net;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace WebPagesAsync
{
    
    internal class Program
    {
        public static void Main(string[] args)
        {
            Downloader url = new Downloader(2);

            Task task = url.GetPages(new Uri("https://youtube.com/"));
            task.Wait();
        }
    }
}