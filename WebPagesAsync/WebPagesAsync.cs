using System;
using System.Collections.Generic;
using System.Net;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace WebPagesAsync
{
    public class Downloader
    {
        public int MaxDepth;

        public Downloader(int maxValueofDepth = 3)
        {
            MaxDepth = maxValueofDepth;
        }

        public async Task GetPages(Uri uri, int depth = 0)
        {
            if (depth == MaxDepth)
            {
                return;
            }
            string str = await GetPageContent(uri.ToString());

            Console.WriteLine(uri + " -- " + str.Length + " | " + depth);

            List<string> links = new List<string>();
            List<Task> tasks = new List<Task>();

            links = GetLinks(uri);

            foreach (string link in links)
            {
                //Console.WriteLine(link);
                tasks.Add(GetPages(new Uri(link), depth + 1));
            }
            await Task.WhenAll(tasks.ToArray());
        }

        private List<string> GetLinks(Uri uri)
        {
            string html = new WebClient().DownloadString(uri);
            Regex reHref = new Regex(@"http(s)?://([\w-]+\.)+[\w-]+(/[\w- ./?%&=]*)?");
            List<string> links = new List<string>();
            foreach (Match match in reHref.Matches(html))
            {
                //Console.WriteLine(match.Groups["url"].ToString());
                links.Add(match.Value);
            }
            return links;
        }

        private async Task<string> GetPageContent(string url)
        {
            string str;
            using (WebClient client = new WebClient())
            {
                byte[] bytes = await client.DownloadDataTaskAsync(url);
                str = client.Encoding.GetString(bytes);
            }
            return str;
        }
    }

    internal class Program
    {
        public static void Main(string[] args)
        {
            Downloader url = new Downloader(2);

            Task task = url.GetPages(new Uri("https://Github.com/"));
            task.Wait();
        }
    }
}