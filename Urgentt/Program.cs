using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using RestSharp;
using IrcDotNet;
using Newtonsoft.Json;
using System.IO;

namespace Urgentt
{
    class Program
    {
        static void Main(string[] args)
        {
            string basePath = System.AppDomain.CurrentDomain.BaseDirectory;
            string configFile = Path.Combine(basePath, "config.json");
            Config config = JsonConvert.DeserializeObject<Config>(File.ReadAllText(configFile));

            var client = new RestClient();
            client.BaseUrl = new Uri(config.URL);

            var request = new RestRequest(Method.POST);
            request.Resource = "api/Release/Push";
            request.RequestFormat = DataFormat.Json;
            request.AddHeader("X-Api-Key", config.apiKey);
            request.AddBody(new {
                title = "NCIS.S13E15.720p.HDTV.X264-DIMENSION",
                downloadURL = "http://foobar.com/download.blub",
                downloadProtocol = "torrent",
                publishDate = "2016-02-10T00:00:00Z"
            });

            IRestResponse response = client.Execute(request);

            Console.WriteLine(response.Content);

            string input = "[17:48:19] <Barney> The Secret Life of the Zoo | S01E03 | Episode | 2016 | MKV | H.264 | HDTV | 720p | Yes | Yes | 609074 | Anonymous | English | The.Secret.Life.Of.The.Zoo.S01E03.720p.HDTV.x264-C4TV";
            Console.WriteLine(Parser.title(input));

            var bot = new ircBot();

            Console.ReadKey();
        }
    }

    static class Parser
    {
        static Regex _regex = new Regex(@"(?<=[\>\|]\s)(.*?)(?:\s(?=\|)|\n|$)");

        static public string title(string input)
        {
            var allMatches = _regex.Matches(input);
            return allMatches[13].Value;
        }
    }

    public class ircBot : BasicIrcBot
    {
        public ircBot() : base()
        {
            Run();
        }

        public override IrcRegistrationInfo RegistrationInfo
        {
            get
            {
                return new IrcUserRegistrationInfo()
                {
                    NickName = "xelra_Urgentt",
                    UserName = "xelra_Urgentt",
                    RealName = "xelra_Urgentt"
                };
            }
        }
    }


    public class Config
    {
        public string apiKey { get; set; }
        public string URL { get; set; }
    }
}
