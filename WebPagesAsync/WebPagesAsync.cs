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

        public Downloader(int maxValueofDepth = 3)
        {
            maxDepth = maxValueofDepth;
        }

        public async Task GetPages(Uri uri, int depth = 0)
        {
            if (depth == maxDepth) return;
            var str = await GetPageContent(uri.ToString());
            
            Console.WriteLine(uri.ToString() + " -- "+ str.Length + " | " + depth);

            List<String> links = new List<String>();
            List<Task> tasks = new List<Task>();

            links = GetLinks(uri);

            foreach (var link in links)
            {
                //Console.WriteLine(link);
                tasks.Add(GetPages(new Uri(link), depth + 1));
            }
            await Task.WhenAll(tasks.ToArray());
            return;
        }
        private List<string> GetLinks(Uri uri)
        {
            string html = new WebClient().DownloadString(uri);
            Regex reHref = new Regex(@"http(s)?://([\w-]+\.)+[\w-]+(/[\w- ./?%&=]*)?");
            var links = new List<string>();
            foreach (Match match in reHref.Matches(html))
            {
                //Console.WriteLine(match.Groups["url"].ToString());
                links.Add(match.Value);
            }
            return links;

        }

        private async Task<String> GetPageContent(string url)
        {
            string str;
            using (var client = new WebClient())
            {
                var bytes = await client.DownloadDataTaskAsync(url);
                str = client.Encoding.GetString(bytes);
            }
            return str;
        }

    }
    class Program
    {
        public static void Main(string[] args)
        {

            Downloader url = new Downloader(2);

            var task = url.GetPages(new Uri("https://Github.com/"));
            task.Wait();
            
        }
    }

}
