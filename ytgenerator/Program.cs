using System;
using System.Threading.Tasks;
using System.Threading;
using System.Web;
using System.Net;
using System.IO;


namespace ytgenerator
{


    class Program
    {
        private static readonly Random rnd = new Random();
        private static string url = "https://www.youtube.com/watch?v=";     
        static private string[] charList = { "0","1","2","3","4","5","6","7","8","9","A","B","C","D","E","F","G","H","I","J","K","L","M","N","O","P","Q","R","S","T","U","V","W","X","Y","Z","_"+"#"};        

        static private string RandomCharOrNmb()
        {
            return charList[rnd.Next(0,charList.Length)];                    
        }

        static private string GenerateIndentifier()
        {
            string URI = "";

            for (int i = 0; i < 11; i++)
            {
                int lowUp = rnd.Next(0, 2);
                switch (lowUp)
                {
                    case 0:
                        URI += RandomCharOrNmb().ToLower();
                        break;
                    case 1:
                        URI += RandomCharOrNmb();
                        break;
                }               
            }
            return URI;
        }

        public static string GetTitle(string url)
        {
            var api = $"http://youtube.com/get_video_info?video_id={GetArgs(url, "v", '?')}";
            return GetArgs(new WebClient().DownloadString(api), "title", '&');
        }

        private static string GetArgs(string args, string key, char query)
        {
            var iqs = args.IndexOf(query);
            return iqs == -1
                ? string.Empty
                : HttpUtility.ParseQueryString(iqs < args.Length - 1
                    ? args.Substring(iqs + 1) : string.Empty)[key];
        }

        private static void Search()
        {
            string path = @"C:\Users\Aleksander\Desktop\Test.txt";
            string URL;

            if (!File.Exists(path))
            {
                string createText = "URL's:" + Environment.NewLine;
                File.WriteAllText(path, createText);
            }

            for (int i = 0; i < 10000; i++)
            {
                string urlTitle;

                URL = url + GenerateIndentifier();
                urlTitle = GetTitle(url);
                
                if (urlTitle != null)
                {
                    string appendText = i.ToString() + "\t-\t" + URL + "\tTitle:" + urlTitle + Environment.NewLine;
                    File.AppendAllText(path, appendText);
                }
            }

            Thread.CurrentThread.Abort();
        }

        static void Main(string[] args)
        {
            int thrCount = 4;                  
            
            for(int i = 0; i < thrCount; i++)
            {                
                Thread thr = new Thread(Search);
                thr.Name = "thr" + i.ToString();
                thr.Start();

                Console.WriteLine("Starting thread: " + thr.Name);
            }

            Console.ReadKey();
        }
    }
}
