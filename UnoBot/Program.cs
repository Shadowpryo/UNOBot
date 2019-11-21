using System;
using System.IO;
using System.Threading.Tasks;
using Discord;
using Discord.Commands;
using Discord.WebSocket;
//using Microsoft.Extensions.DependencyInjection;

namespace UnoBot
{
    class Program
    {
        public static void Main(string[] args)
        {
            try
            {
                new Program().MainAsync().GetAwaiter().GetResult();
                Console.ReadLine();
            }
            catch (Exception e)
            {
                Console.WriteLine(e.Message);
                Console.ReadLine();
            }
        }


        private DiscordSocketClient unoBot;
        private CommandService commands;
        private CommandHandler CH;
        //private GameManagerService.Initialize IN;
        //private IServiceProvider service;

    public async Task MainAsync()
        {
            unoBot = new DiscordSocketClient();
            commands = new CommandService();
            //IN = new GameManagerService.Initialize(commands, unoBot);
            //service = IN.BuildServiceProvider();
            CH = new CommandHandler(unoBot, commands);
            unoBot.Log += Log;

            //Assign token to string here
            //var Token = Environment.GetEnvironmentVariable("config.json");
            string Token = File.ReadAllText("secret.vdf");

            await unoBot.LoginAsync(TokenType.Bot, Token);
            await unoBot.StartAsync();
            await CH.InstallCommandsAsync();

            //Console.WriteLine("press a key to exit");
            //Console.ReadKey();
        }

        private Task Log(LogMessage msg)
        {
            Console.Write(msg.ToString() + "\n");
            return Task.CompletedTask;
        }
    }
}
