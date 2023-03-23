using DSharpPlus;
using System.Runtime.CompilerServices;

namespace BoWWorker
{
    public class Worker : BackgroundService
    {
        private readonly ILogger<Worker> _logger;

        public Worker(ILogger<Worker> logger)
        {
            _logger = logger;
        }

        protected override async Task ExecuteAsync(CancellationToken stoppingToken)
        {
            while (!stoppingToken.IsCancellationRequested)
            {
                _logger.LogInformation("Worker running at: {time}", DateTimeOffset.Now);

                string value;
                value = Environment.GetEnvironmentVariable("DiscordBow_TOKEN");

                if (value == null)
                {
                    Console.WriteLine("Token Not Found");
                    Environment.Exit(0);
                };

                var discord = new DiscordClient(new DiscordConfiguration()
                {
                    Token = value,
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
                                if(e.Message.Content.Contains(item))
                                { 
                                    if (e.Message.Content.ToLower().Contains(item))
                                        await e.Message.RespondAsync(response[index]);

                                    _logger.LogInformation($@"reponded with {response[index]} which was triggered by {item} by the user {e.Message.Author.Username}");
                                    break;
                                }
                            }
                        }
                    }
                    
                };

                await discord.ConnectAsync();
                await Task.Delay(-1);
            }





            await Task.Delay(1000, stoppingToken);
            }
        }
    }
