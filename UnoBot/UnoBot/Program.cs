using System;
using System.IO;
using System.Threading.Tasks;
using Discord;
using Discord.Commands;
using Discord.WebSocket;


namespace UnoBot
{
    class Program
    {
        public static void Main(string[] args)
           => new Program().MainAsync().GetAwaiter().GetResult();


        private DiscordSocketClient unoBot;
        private CommandService commands;
        private CommandHandler CH;

        public async Task MainAsync()
        {
            unoBot = new DiscordSocketClient();
            commands = new CommandService();
            CH = new CommandHandler(unoBot, commands);
            unoBot.Log += Log;

            //Assign token to string here
            //var Token = Environment.GetEnvironmentVariable("config.json");
            string Token = File.ReadAllText("secret.vdf");

            await unoBot.LoginAsync(TokenType.Bot, Token);
            await unoBot.StartAsync();
            await CH.InstallCommandsAsync();

            Console.WriteLine("press a key to exit");
            Console.ReadKey();
        }

        private Task Log(LogMessage msg)
        {
            Console.Write(msg.ToString() + "\n");
            return Task.CompletedTask;
        }
    }
}
