using System;
using System.Collections.Generic;
using System.Linq;
using Discord.Commands;
using Discord.WebSocket;
using System.Threading.Tasks;

namespace UnoBot
{
    public class GameManager : ModuleBase<SocketCommandContext>
    {
        [DontInject]
        public List<Player> Players { get; set; }
        [DontInject]
        public CardDeck DrawPile { get; set; }
        [DontInject]
        public List<Card> DiscardPile { get; set; }

        public GameManager()
        {

            int numPlayers = Globals.numOfPlayers;
            List<SocketUser> users = Globals.usersInGame;
            
            if (users.Count >= 1)
            {

                Players = new List<Player>();
                DrawPile = new CardDeck();
                DrawPile.Shuffle();
                for (int i = 0; i < users.Count; i++)
                {
                    Players.Add(new Player()
                    {
                        Position = i + 1,
                        discordPlayer = users[i]

                    });
                }


                int maxCards = 7 * Players.Count;
                int dealtCards = 0;
                while (dealtCards < maxCards)
                {
                    for (int i = 0; i < numPlayers; i++)
                    {
                        Players[i].Hand.Add(DrawPile.Cards.First());
                        DrawPile.Cards.RemoveAt(0);
                        dealtCards++;
                    }
                }
                Console.WriteLine("Users added and hands set");
                DiscardPile = new List<Card>();
                DiscardPile.Add(DrawPile.Cards.First());
                DrawPile.Cards.RemoveAt(0);

                while (DiscardPile.First().Value == CardValue.Wild || DiscardPile.First().Value == CardValue.DrawFour)
                {
                    DiscardPile.Insert(0, DrawPile.Cards.First());
                    DrawPile.Cards.RemoveAt(0);
                }
            }
        }

        public async Task PlayGame()
        {
            int i = 0;
            int k = Players.Count + 1;
            bool isAscending = true;

            //First, let's show what each player starts with
            foreach (var player in Players)
            {
                await player.ShowHand();
            }

            PlayerTurn currentTurn = new PlayerTurn()
            {
                Result = TurnResult.GameStart,
                Card = DiscardPile.First(),
                DeclaredColor = DiscardPile.First().Color
            };

            Console.WriteLine("Current turn set up");

            await Golab.gameChannel.SendMessageAsync("First card is a " + currentTurn.Card.DisplayValue + ".");

            while (!Players.Any(x => !x.Hand.Any()))
            {
                if (DrawPile.Cards.Count < 4) //Cheating a bit here
                {
                    var currentCard = DiscardPile.First();

                    //Take the discarded cards, shuffle them, and make them the new draw pile.
                    DrawPile.Cards = DiscardPile.Skip(1).ToList();
                    DrawPile.Shuffle();

                    //Reset the discard pile to only have the current card.
                    DiscardPile = new List<Card>();
                    DiscardPile.Add(currentCard);

                    Console.WriteLine("Shuffling cards!");
                }

                var currentPlayer = Players[i];
                currentTurn = Golab.turn;
                await Players[i].PlayTurn(currentTurn, DrawPile);
                AddToDiscardPile(currentTurn);



                if (currentTurn.Result == TurnResult.Reversed)
                {
                    isAscending = !isAscending;
                }

                if (isAscending)
                {
                    i++;
                    if (i >= Players.Count) //Reset player counter
                    {
                        i = 0;
                    }
                }
                else
                {
                    i--;
                    if (i < 0)
                    {
                        i = Players.Count - 1;
                    }
                }
            }

            var winningPlayer = Players.Where(x => !x.Hand.Any()).First();
            await Golab.gameChannel.SendMessageAsync("Player " + winningPlayer.discordPlayer.Mention + " wins!!");

            foreach (var player in Players)
            {
                await Golab.gameChannel.SendMessageAsync("Player " + player.discordPlayer.Mention + " has " + player.Hand.Sum(x => x.Score).ToString() + " points in his hand.");
            }
        }

        private void AddToDiscardPile(PlayerTurn currentTurn)
        {
            if (currentTurn.Result == TurnResult.PlayedCard
                    || currentTurn.Result == TurnResult.DrawTwo
                    || currentTurn.Result == TurnResult.Skip
                    || currentTurn.Result == TurnResult.WildCard
                    || currentTurn.Result == TurnResult.WildDrawFour
                    || currentTurn.Result == TurnResult.Reversed)
            {
                DiscardPile.Insert(0, currentTurn.Card);
            }
        }
    }
}
