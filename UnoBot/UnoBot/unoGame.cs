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

namespace UnoBot
{
    static class Globals
    {
        public static bool bGameRunning = false;
        public static bool bGameStarted = false;
        public static List<string> usersInGame = new List<string>();
        public static int numOfPlayers;
        public static System.Timers.Timer timer1;
        public static int counter = 12;
        
    }
    class unoGame : ModuleBase<SocketCommandContext>
    {
        
    }

    [Group("setup")]
    public class Setup : ModuleBase<SocketCommandContext>
    {
        public string[] colors = { "red", "blue", "yellow", "green", "pick"};
        public string[] cards = { "0", "1", "2", "3", "4", "5", "6", "7", "8", "9", "skip", "reverse", "draw2", "draw4", "userPick"};
        Dictionary<string, string> hand = new Dictionary<string, string>();
        [Command("Start Game")]
        [Alias("start", "Start")]
        public async Task StartGame(int numPlayers)
        {
            if (!Globals.bGameRunning)
            {
                if (numPlayers <= 1 || numPlayers >= 8)
                {
                    await ReplyAsync("You need at least 2, and no more than 8 players to start a game.");
                }
                else
                {
                    Globals.numOfPlayers = numPlayers;
                    Globals.bGameRunning = true;
                    Globals.timer1 = new System.Timers.Timer();
                    Globals.timer1.Interval = 1000;
                    Globals.timer1.Start();
                    Globals.timer1.Elapsed += timer1_Tick;
                    await ReplyAsync($"Game started up with {numPlayers} players, use the command join to get in.");
                }
            }
        }
        private static void timer1_Tick(object sender, EventArgs e)
        {
            Globals.counter--;
            if (Globals.counter == 0)
            {
                Globals.timer1.Stop();
                Console.WriteLine("Start game");
                Globals.bGameStarted = true;
            }
        }

        [Command("Join")]
        [Alias("join")]
        public async Task Join()
        {
            string caller = Context.Message.Author.Username;
            if (!Globals.bGameRunning)
            {
                await ReplyAsync("There is not a game currently running, use the command start, if you want to make a game.");
            }
            else
            {
                if (!Globals.bGameStarted)
                {
                    if (Globals.usersInGame.Count <= Globals.numOfPlayers - 1)
                    {
                        if (Globals.usersInGame.Contains(caller))
                        {
                            await ReplyAsync($"You already joined please wait for it to stat in {Globals.counter} seconds.");
                        }
                        else
                        {
                            Globals.usersInGame.Add(caller);
                            //string userProfile = "User: " + caller + " Hand: " + hand + "\n";
                            //System.IO.File.WriteAllText(@"SavedPlayerProfiles.txt", userProfile);
                            //await ReplyAsync("Saved your profile");
                            await ReplyAsync($"You joined and the game will start in {Globals.counter} seconds");
                        }
                    }
                    else
                    {
                        await ReplyAsync("Game is full");
                    }
                }
                else
                {
                    await ReplyAsync("Game is started");
                }
            }
        }

        [Command("Debug")]
        [Summary("Test to see if the bot is running")]
        public async Task Debug()
        {
            if (Globals.bGameStarted)
                await dealCards();
            else
                await ReplyAsync("Game is not started yet");
        }

        [Command("Deal")]
        [Alias("deal")]
        public async Task dealCards()
        {
            string card;
            if (Globals.bGameStarted)
            {
                Console.WriteLine($"cards count: {cards.Count()}.");
                Console.WriteLine($"colors count: {colors.Count()}.");
                await Context.Channel.SendMessageAsync("Game has started you will get a message with your cards.");
                for(int i = 0; i <= 6; i++)
                {
                    int ran1 = RandomNumber(0, colors.Count());
                    int ran2 = RandomNumber(0, cards.Count());

                    Console.WriteLine($"colors count: {ran1}, {ran2}.");
                    if (ran1 == 4)
                    {
                        int ranPick = RandomNumber(13, 14);
                        card = colors[ran1] + " " + cards[ranPick];
                        hand.Add(colors[ran1], cards[ranPick]);
                    }
                    else
                    {
                        card = colors[ran1] + " " + cards[ran2];
                        hand.Add(colors[ran1], cards[ran2]);
                    }
                    await ReplyAsync($"Random card is {card}");
                    Console.WriteLine(i);
                }
                await ReplyAsync("Outside the loop");
                //foreach (string user in Globals.usersInGame)
                //{

                //}
            }
        }
        public int RandomNumber(int min, int max)
        {
            Random random = new Random();
            return random.Next(min, max);
        }
    }
}
