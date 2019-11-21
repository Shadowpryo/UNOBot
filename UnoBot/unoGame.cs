using System;
using System.Linq;
using System.Timers;
using System.Diagnostics;
using System.Threading;
using System.Threading.Tasks;
using Discord;
using Discord.Commands;
using Discord.WebSocket;
using System.Collections.Generic;
using System.Data.OleDb;
using Newtonsoft.Json;
using System.IO;

namespace UnoBot
{
    static class Globals
    {
        public static bool bGameRunning = false;
        public static bool bGameStarted = false;
        public static List<SocketUser> usersInGame = new List<SocketUser>();
        public static int numOfPlayers;
        public static System.Timers.Timer timer1;
        public static int counter = 2;
        public static GameManager gm;

    }

    public class user
    {
        public string userN { get; set; }
        public string hand { get; set; }
        [DontInject]
        public int numOfCards { get; set; }
        public bool isTurn { get; set; }

    }
    public class unoGame : ModuleBase<SocketCommandContext>
    {
        [Command("BotInfo")]
        [Alias("bot")]
        public async Task About()
        {
            var Shadow = Context.Guild.GetUser(186646766005911552);
            await ReplyAsync($"This bot is simply made and used for playing uno with 2-8 users.\nIf you wanna support it just send {Shadow.Mention} some love!");
        }
    }

    [Group("game")]
    public class Setup : ModuleBase<SocketCommandContext>
    {
        public int totalNumber = 0;
        [Command("Setup")]
        [Alias("setup")]
        public async Task SetupGame(int numUsers = 0)
        {
            var Shadow = Context.Guild.GetUser(186646766005911552);
            if (numUsers < 2)
            {
                await ReplyAsync("You need at least two players to start a game!");
            }else if(numUsers > 8)
            {
                await ReplyAsync($"You can't have more than 8 players currenlty, maybe {Shadow.Mention} will change that later");
            }
            else
            {
                Globals.usersInGame.Add(Context.Message.Author);
                Globals.numOfPlayers += 1;
                Globals.timer1 = new System.Timers.Timer();
                Globals.timer1.Interval = 1000; // 1 second
                Globals.timer1.Elapsed += timer1_Tick;
                Globals.timer1.Start();
                await ReplyAsync($"You started a game with {numUsers} players, you are already in and the game will start in ");
            }
        }

        [Command("Join")]
        [Alias("join")]
        public async Task Join()
        {
            if(Globals.usersInGame.Count >= totalNumber)
            {
                await ReplyAsync("Game is full, and about to start!");
            }
            else
            {
                if (Globals.usersInGame.Contains(Context.Message.Author))
                {
                    await ReplyAsync("You already joined the game!");
                }
                else
                {
                    Globals.numOfPlayers += 1;
                    await ReplyAsync("You joined the game!");
                }
            }
        }

        [Command("Start")]
        [Alias("start")]
        public async Task Start()
        {
            if (Globals.bGameStarted && !Globals.bGameRunning)
            {
                await ReplyAsync("Game is starting you should, if you are playing you will get a message with your hand shortly");
                //GameManager gm = new GameManager();
                await ReplyAsync("Game setup you should have your cards now!");
                Globals.bGameRunning = true;
                Golab.gameChannel = Context.Guild.GetTextChannel(629499940141268992);
                await Globals.gm.PlayGame();
            }
            else
            {
                await ReplyAsync("A game is currently running");
            }
        }

        public void timer1_Tick(object sender, ElapsedEventArgs e)
        {
            Globals.counter--;
            if (Globals.counter <= 0)
            {
                Globals.timer1.Stop();
                Globals.bGameStarted = true;
                Console.WriteLine("Game can start now");
                Globals.gm = new GameManager();
            }
        }
    }
}
