using DSharpPlus;
using System;
using Serilog;
using System.Linq;

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

                var token = Environment.GetEnvironmentVariable("DiscordBow_TOKEN");
                var url = Environment.GetEnvironmentVariable("SB_BoW_URL");
                var key = Environment.GetEnvironmentVariable("SB_BoW_API_Key");
                var options = new Supabase.SupabaseOptions
                {
                    AutoConnectRealtime = true
                };

               

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
                    });

                    var supabase = new Supabase.Client(url, key, options);
                    await supabase.InitializeAsync();

                    var Phrases = await supabase.From<Phrases>().Get();
                    var phrases = Phrases.Models;

                    var Responses = await supabase.From<Responses>().Get();
                    var responses = Responses.Models;

                    Log.Information("Phrase and reponse data acquired");
                    var random = new Random();

                    discord.MessageCreated += async (s, e) =>
                    {
                        int index = random.Next(responses.Count);
                        foreach (var user in e.MentionedUsers)
                        {
                            if (user.Username == "ramblinggeek" && !user.IsBot)
                            {
                                foreach (var phrase in phrases)
                                {
                                    if (e.Message.Content.Contains(phrase.phrase))
                                    {
                                        if (e.Message.Content.ToLower().Contains(phrase.phrase))
                                            await e.Message.RespondAsync(responses[index].response);

                                        Log.Information($@"responded with {responses[index].response}, Triggered by the word/phrase '{phrase.phrase}' by the user '{e.Message.Author.Username}'");
                                        break;
                                    }
                                }
                            }
                        }

                    };

                    await discord.ConnectAsync();
                    await Task.Delay(-1);
                }
                catch (Exception e)
                {

                    Log.Fatal($"ERROR {e}");
                    Environment.Exit(1);
                }

                await Task.Delay(1000, stoppingToken);

            }
        }
    }
}