using DSharpPlus;
using System;
using Serilog;

namespace BoW
{
    public class Worker : BackgroundService
    {
        public Worker()
        {
            Log.Logger = new LoggerConfiguration()
            .WriteTo.Console()
            .CreateLogger();
        }

        protected override async Task ExecuteAsync(CancellationToken stoppingToken)
        {
            while (!stoppingToken.IsCancellationRequested)
            {
                Log.Information("Worker running at: {time}", DateTimeOffset.Now);

                string token;

                token = Environment.GetEnvironmentVariable("DiscordBow_TOKEN", EnvironmentVariableTarget.User);
                
                if (string.IsNullOrEmpty(token))
                {
                    Log.Error("Discord Bot Token Not acquired");
                }
                else
                {
                    Log.Information("Acquired Discord Bot Token");
                }

                try
                {

                    var logFactory = new LoggerFactory().AddSerilog();

                    var discord = new DiscordClient(new DiscordConfiguration()
                    {
                        Token = token,
                        TokenType = TokenType.Bot,
                        LoggerFactory = logFactory,
                        Intents = DiscordIntents.AllUnprivileged | DiscordIntents.MessageContents
                    }); ;

                    List<string> phrase = new List<string>();
                    List<string> response = new List<string>();

                    string current_path = Path.GetDirectoryName(Environment.ProcessPath);
                    string phrases = Path.Combine(current_path, "phrases.txt");
                    string responses = Path.Combine(current_path, "responses.txt");


                    try
                    {
                        foreach (string line in File.ReadLines(phrases))
                        {
                            phrase.Add(line);
                        }

                        foreach (string line in File.ReadLines(responses))
                        {
                            response.Add(line);
                        }
                    }
                    catch (Exception)
                    {
                        Log.Error("Error: One ore more data file is missing.");
                        Log.Error($"{phrases}");
                        Log.Error($"{responses}");
                    }

                    Log.Information("Phrase and reponse data acquired");
                    var random = new Random();

                    discord.MessageCreated += async (s, e) =>
                    {
                        int index = random.Next(response.Count);
                        foreach (var user in e.MentionedUsers)
                        {
                            if (user.Username == "ramblinggeek" && !user.IsBot)
                            {
                                foreach (var phrase in phrase)
                                {
                                    if (e.Message.Content.Contains(phrase))
                                    {
                                        if (e.Message.Content.ToLower().Contains(phrase))
                                            await e.Message.RespondAsync(response[index]);

                                        Log.Information($@"responded with '{response[index]}' triggered by the word/phrase '{phrase}' by the user '{e.Message.Author.Username}'");
                                        break;
                                    }
                                }
                            }
                        }

                    };

                    await discord.ConnectAsync();
                    await Task.Delay(-1);
                }
                catch (Exception)
                {

                    Log.Fatal("ERROR Something went really wrong to end up here.");
                    Environment.Exit(1);
                }

                await Task.Delay(1000, stoppingToken);

            }
        }
    }
}