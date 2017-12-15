using System;
using System.Collections.Generic;
using System.Security.Policy;
using System.Threading.Tasks;
using System.Net;
using System.Text.RegularExpressions;

namespace WebPagesAsync
{
    public class Downloader
    {
        public int maxDepth;
        public Dictionary<String, int> pages = new Dictionary<string, int>();

        public Downloader(int maxValueofDepth = 3)
        {
            maxDepth = maxValueofDepth;
        }

        public async Task GetPages(Uri uri, int depth = 0)
        {
            if (depth == maxDepth) return;
            //Console.WriteLine(uri.ToString());
            var str = GetPageContent(uri.ToString());

            //pages.Add(url.Value, str.Length);
            Console.WriteLine(uri.ToString() + " -- "+ str.Length + " | " + depth);

            List<String> links = new List<String>();
            List<Task> tasks = new List<Task>();

            links = GetLinks(uri);

            foreach (var link in links)
            {
                //Console.WriteLine(link);
                tasks.Add(GetPages(new Uri(link), depth + 1));
            }
            //await Task.WhenAll(tasks.ToArray());
            return;
        }
        private List<string> GetLinks(Uri uri)
        {
            string html = new WebClient().DownloadString(uri);
            Regex reHref = new Regex(@"(?inx)<a \s [^>]*href \s* = \s*(?<q> ['""] )(?<url> [^""]+ )\k<q>[^>]* >");
            var links = new List<string>();
            foreach (Match match in reHref.Matches(html))
            {
                //Console.WriteLine(match.Groups["url"].ToString());
                links.Add(match.Groups["url"].ToString());
            }
            return links;

        }

        private String GetPageContent(string url)
        {
            string str;
            using (var client = new WebClient())
            {
                var bytes = client.DownloadData(url);
                str = client.Encoding.GetString(bytes);
            }
            return str;
        }

    }
    class Program
    {
        public static void Main(string[] args)
        {

            Downloader url = new Downloader(3);

            var task = url.GetPages(new Uri("https://Yandex.ru/"));
            
            Console.ReadKey();
        }
    }

}
