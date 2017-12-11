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

        public async Task GetPages(Url url, int depth = 0)
        {
            if (depth == maxDepth) return;
            //Console.WriteLine(url.Value + "  " + depth);
            var str = GetPageContent(url.Value);

            pages.Add(url.Value, str.Length);

            List<String> links = new List<String>();
            List<Task> tasks = new List<Task>();

            links = GetLinks(url.Value, str);

            foreach (var link in links)
            {
                tasks.Add(GetPages(new Url(link), depth + 1));
            }

            return;
        }
        private List<string> GetLinks(String url, String page)
        {

            if (!pages.ContainsKey(url)) return null;

            var regex = new Regex(@"<a.*? href=""(?<url>http(s)?[\w\.:?&-_=#/]*)""+?");
            MatchCollection matches = regex.Matches(page);

            var links = new List<string>();
            for (var i = 0; i < matches.Count; i++)
            {
                var link = matches[i].Groups["url"].Value;
                if (!pages.ContainsKey(link) && !links.Contains(link))
                    links.Add(link);
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

            var task = url.GetPages(new Url("https://fb.com/"));

            foreach (var page in url.pages)
            {
                Console.Write(page.Key + " -- " + page.Value + "\n");
            }

            Console.ReadKey();
        }
    }

}
