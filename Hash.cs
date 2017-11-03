using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Security.Cryptography;
using System.Threading.Tasks;
using System.Text;

namespace Hash
{
    class Program
    {
        static void Main(string[] args)
        {
            string path = Console.ReadLine(); ;

            if (File.Exists(path) || Directory.Exists(path))
            {
                Stopwatch timer = new Stopwatch();
                timer.Start();

                string hash = getHash(path);

                Console.WriteLine(hash);

                timer.Stop();

                Console.WriteLine("Time spent: {0}", timer.Elapsed);

                Console.ReadKey();
            }
            else
            {
                Console.WriteLine("directory does not exists");
            }
        }
        public static string getHash(string path)
        {
            string ans = " ";
            string[] folders = Directory.GetDirectories(path);
            string[] files = Directory.GetFiles(path);
            
            Task<string>[] hashFolder = new Task<string>[folders.Length];
            if (folders.Length != 0)
            {
                for (int i = 0; i < folders.Length; ++i)
                {
                    int j = i;
                    hashFolder[j] = Task.Run(() =>
                    {
                        return getHash(folders[j]);
                    });
                }
            } 
            
            for (int i = 0; i < files.Length; ++i)
            {
                var stream = File.OpenRead(files[i]);
                ans += hashFile(stream);
            }

            if (folders.Length != 0)
            {
                for (int i = 0; i < folders.Length; ++i)
                {
                    hashFolder[i].Wait();
                    ans += hashFolder[i].Result;
                }
            }
            return hashString(ans);
        }
        private static string hashString(String path)
        {
            var md5 = MD5.Create();
            byte[] data = md5.ComputeHash(Encoding.UTF8.GetBytes(path));
            StringBuilder str = new StringBuilder();

            for (int i = 0; i < data.Length; i++)
            {
                str.Append(data[i].ToString("x2"));
            }
            return str.ToString();
        }
        private static string hashFile(FileStream path)
        {
            var md5 = MD5.Create();
            byte[] data = md5.ComputeHash(path);
            StringBuilder str = new StringBuilder();

            for (int i = 0; i < data.Length; i++)
            {
                str.Append(data[i].ToString("x2"));
            }
            return str.ToString();
        }
    }
}
