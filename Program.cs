using DSharpPlus;
using System.Diagnostics;
using System.Net.Http.Headers;

namespace BankOfWayneBot
{
    internal class Program
    {
        static async Task Main(string[] args)
        {

            using HttpClient client = new();
            client.DefaultRequestHeaders.Accept.Clear();
            client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/vnd.github.v3+json"));
            client.DefaultRequestHeaders.Add("User-Agent", ".NET Foundation Repository Reporter");

            await ProcessRepositoriesAsync(client);

            var discord = new DiscordClient(new DiscordConfiguration()
            {
                Token = Environment.GetEnvironmentVariable("DiscordBow_Token"),
                TokenType = TokenType.Bot,
                Intents = DiscordIntents.AllUnprivileged | DiscordIntents.MessageContents
            });

            List<string> watchPhrase = new List<string>();
            List<string> response = new List<string>();

            foreach (string line in System.IO.File.ReadLines(@"watchPhrases.txt"))
            {
                watchPhrase.Add(line);
            }

            foreach (string line in System.IO.File.ReadLines(@"responses.txt"))
            {
                response.Add(line);
            }

            var random = new Random();

            discord.MessageCreated += async (s, e) =>
            {
                int index = random.Next(response.Count);
                foreach (var user in e.MentionedUsers)
                {
                    if (user.Username == "ramblinggeekuk" && !user.IsBot)
                    {
                        foreach (var item in watchPhrase)
                        {
                            if (e.Message.Content.ToLower().Contains(item))
                                await e.Message.RespondAsync(response[index]);
                        }
                    }
                }
            };

            await discord.ConnectAsync();
            await Task.Delay(-1);




        }

        static async Task ProcessRepositoriesAsync(HttpClient client)
        {
            var json = await client.GetStringAsync("https://api.github.com/orgs/dotnet/repos");

            Console.Write(json);
        }
    }
}