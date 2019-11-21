using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Discord;
using Discord.Commands;
using Discord.WebSocket;

namespace UnoBot
{
    public class Golab
    {
        public static CardColor color;
        public static CardValue value;
        public static PlayerTurn turn;
        public static ITextChannel gameChannel;
        public static Card currentCard;
    }
    public class Player : ModuleBase<SocketCommandContext>
    {
        [DontInject]
        public List<Card> Hand { get; set; }
        [DontInject]
        public int Position { get; set; }
        public SocketUser discordPlayer;

        public Player()
        {
            Hand = new List<Card>();
        }

        private async Task ProcessAttack(Card currentDiscard, CardDeck drawPile)
        {

            Golab.gameChannel = Context.Guild.GetTextChannel(636288627466436631);
            PlayerTurn turn = new PlayerTurn();
            turn.Result = TurnResult.Attacked;

            turn.Card = currentDiscard;
            turn.DeclaredColor = currentDiscard.Color;

            if (currentDiscard.Value == CardValue.Skip)
            {
                await Golab.gameChannel.SendMessageAsync("Player " + discordPlayer.Mention + " was skipped!");
            }
            else if (currentDiscard.Value == CardValue.DrawTwo)
            {
                await Golab.gameChannel.SendMessageAsync("Player " + discordPlayer.Mention + " must draw two cards!");
                Hand.AddRange(drawPile.Draw(2));
            }
            else if (currentDiscard.Value == CardValue.DrawFour)
            {
                await Golab.gameChannel.SendMessageAsync("Player " + discordPlayer.Mention + " must draw four cards!");
                Hand.AddRange(drawPile.Draw(4));
            }

            Golab.turn = turn;
        }

        public async Task PlayTurn(PlayerTurn previousTurn, CardDeck drawPile)
        {
            Console.WriteLine($"Current Color is: {previousTurn.DeclaredColor}\nCurrent value is {previousTurn.Card.Value}");

        userPicksCard:
            if ((previousTurn.Result == TurnResult.Skip
                || previousTurn.Result == TurnResult.DrawTwo
                || previousTurn.Result == TurnResult.WildDrawFour))
            {
                await ProcessAttack(previousTurn.Card, drawPile);
            }
            else if ((previousTurn.Result == TurnResult.WildCard
                    || previousTurn.Result == TurnResult.Attacked
                    || previousTurn.Result == TurnResult.ForceDraw)
                    && previousTurn.Card.Color == CardColor.Wild
                    && HasMatch(previousTurn.DeclaredColor))
            {
                await selectCard();
                if (Hand.Find(x => x.Color == Golab.currentCard.Color && x.Value == Golab.currentCard.Value) != null)
                {
                    var cardPlayed = Hand.Find(x => x.Color == Golab.currentCard.Color && x.Value == Golab.currentCard.Value);
                    if (cardPlayed.Value == CardValue.Wild || cardPlayed.Color == CardColor.Wild)
                    {
                        if (!HasMatch(previousTurn.Card))
                            await playCard(cardPlayed);
                        else
                        {
                            await discordPlayer.SendMessageAsync("You have to play a matching card if you have it!");
                            goto userPicksCard;
                        }
                    }
                    else if (cardPlayed.Color != previousTurn.DeclaredColor && cardPlayed.Value != previousTurn.Card.Value)
                    {
                        await discordPlayer.SendMessageAsync("Your card needs to either be the same color or same face value of the last card played!");
                        goto userPicksCard;
                    }
                    else
                    {
                        await playCard(cardPlayed);
                    }
                }
                else
                {
                    await discordPlayer.SendMessageAsync("You don't currently have that card!");
                    goto userPicksCard;
                }
            }
            else if (HasMatch(previousTurn.Card))
            {
                await selectCard();
                if (Hand.Find(x => x.Color == Golab.currentCard.Color && x.Value == Golab.currentCard.Value) != null)
                {
                    var cardPlayed = Hand.Find(x => x.Color == Golab.currentCard.Color && x.Value == Golab.currentCard.Value);
                    if (cardPlayed.Value == CardValue.Wild || cardPlayed.Color == CardColor.Wild)
                    {
                        if (!HasMatch(previousTurn.Card))
                            await playCard(cardPlayed);
                        else
                        {
                            await discordPlayer.SendMessageAsync("You have to play a matching card if you have it!");
                            goto userPicksCard;
                        }
                    }
                    else if (cardPlayed.Color != previousTurn.DeclaredColor && cardPlayed.Value != previousTurn.Card.Value)
                    {
                        await discordPlayer.SendMessageAsync("Your card needs to either be the same color or same face value of the last card played!");
                        goto userPicksCard;
                    }
                    else
                    {
                        await playCard(cardPlayed);
                    }
                }
                else
                {
                    await discordPlayer.SendMessageAsync("You don't currently have that card!");
                    goto userPicksCard;
                }
            }
            //When the player has no matching cards
            else //Draw a card and see if it can play
            {
                Golab.turn = DrawCard(previousTurn, drawPile);
            }

            await DisplayTurn(Golab.turn);
        }

        public async Task playCard(Card userPlayed)
        {
        pickCard:
            PlayerTurn turn = new PlayerTurn();
            CardValue val = userPlayed.Value;
            switch (val)
            {
                case CardValue.Zero:
                    turn.Card = userPlayed;
                    turn.DeclaredColor = turn.Card.Color;
                    turn.Result = TurnResult.PlayedCard;
                    Hand.Remove(userPlayed);
                    Golab.turn = turn;
                    break;
                case CardValue.One:
                    turn.Card = userPlayed;
                    turn.DeclaredColor = turn.Card.Color;
                    turn.Result = TurnResult.PlayedCard;
                    Hand.Remove(userPlayed);
                    Golab.turn = turn;
                    break;
                case CardValue.Two:
                    turn.Card = userPlayed;
                    turn.DeclaredColor = turn.Card.Color;
                    turn.Result = TurnResult.PlayedCard;
                    Hand.Remove(userPlayed);
                    Golab.turn = turn;
                    break;
                case CardValue.Three:
                    turn.Card = userPlayed;
                    turn.DeclaredColor = turn.Card.Color;
                    turn.Result = TurnResult.PlayedCard;
                    Hand.Remove(userPlayed);
                    Golab.turn = turn;
                    break;
                case CardValue.Four:
                    turn.Card = userPlayed;
                    turn.DeclaredColor = turn.Card.Color;
                    turn.Result = TurnResult.PlayedCard;
                    Hand.Remove(userPlayed);
                    Golab.turn = turn;
                    break;
                case CardValue.Five:
                    turn.Card = userPlayed;
                    turn.DeclaredColor = turn.Card.Color;
                    turn.Result = TurnResult.PlayedCard;
                    Hand.Remove(userPlayed);
                    Golab.turn = turn;
                    break;
                case CardValue.Six:
                    turn.Card = userPlayed;
                    turn.DeclaredColor = turn.Card.Color;
                    turn.Result = TurnResult.PlayedCard;
                    Hand.Remove(userPlayed);
                    Golab.turn = turn;
                    break;
                case CardValue.Seven:
                    turn.Card = userPlayed;
                    turn.DeclaredColor = turn.Card.Color;
                    turn.Result = TurnResult.PlayedCard;
                    Hand.Remove(userPlayed);
                    Golab.turn = turn;
                    break;
                case CardValue.Eight:
                    turn.Card = userPlayed;
                    turn.DeclaredColor = turn.Card.Color;
                    turn.Result = TurnResult.PlayedCard;
                    Hand.Remove(userPlayed);
                    Golab.turn = turn;
                    break;
                case CardValue.Nine:
                    turn.Card = userPlayed;
                    turn.DeclaredColor = turn.Card.Color;
                    turn.Result = TurnResult.PlayedCard;
                    Hand.Remove(userPlayed);
                    Golab.turn = turn;
                    break;
                case CardValue.DrawFour:
                    CardColor color = selectColor();
                    turn.Card = userPlayed;
                    turn.DeclaredColor = color;
                    turn.Result = TurnResult.WildDrawFour;
                    Hand.Remove(userPlayed);
                    Golab.turn = turn;
                    break;
                case CardValue.DrawTwo:
                    turn.Card = userPlayed;
                    turn.Result = TurnResult.DrawTwo;
                    turn.DeclaredColor = turn.Card.Color;
                    Hand.Remove(userPlayed);
                    Golab.turn = turn;
                    break;
                case CardValue.Wild:
                    CardColor wColor = selectColor();
                    turn.Card = userPlayed;
                    turn.DeclaredColor = wColor;
                    turn.Result = TurnResult.WildCard;
                    Hand.Remove(userPlayed);
                    Golab.turn = turn;
                    break;
                case CardValue.Skip:
                    turn.Card = userPlayed;
                    turn.Result = TurnResult.Skip;
                    turn.DeclaredColor = turn.Card.Color;
                    Hand.Remove(userPlayed);
                    Golab.turn = turn;
                    break;
                case CardValue.Reverse:
                    turn.Card = userPlayed;
                    turn.Result = TurnResult.Reversed;
                    turn.DeclaredColor = turn.Card.Color;
                    Hand.Remove(userPlayed);
                    Golab.turn = turn;
                    break;
                default:
                    await discordPlayer.SendMessageAsync("You can only pick from zero-nine or darwTwo, drawFour");
                    goto pickCard;
            }
        }

        public CardColor selectColor()
        {
        pickColor:
            string answer;
            Console.WriteLine("Select a color");
            string color = Console.ReadLine();
            switch (color.ToLower())
            {
                case "red":
                    Console.WriteLine("You want to declare red as the color? Y:N");
                    answer = Console.ReadLine();
                    if (answer[0].ToString().ToLower() == "y")
                    {
                        return CardColor.Red;
                    }
                    else if (answer[0].ToString().ToLower() == "n")
                        goto pickColor;
                    else
                    {
                        Console.WriteLine("Yes or No only");
                        goto pickColor;
                    }
                case "blue":
                    Console.WriteLine("You want to declare blue as the color? Y:N");
                    answer = Console.ReadLine();
                    if (answer[0].ToString().ToLower() == "y")
                    {
                        return CardColor.Blue;
                    }
                    else if (answer[0].ToString().ToLower() == "n")
                        goto pickColor;
                    else
                    {
                        Console.WriteLine("Yes or No only");
                        goto pickColor;
                    }
                case "green":
                    Console.WriteLine("You want to declare green as the color? Y:N");
                    answer = Console.ReadLine();
                    if (answer[0].ToString().ToLower() == "y")
                    {
                        return CardColor.Green;
                    }
                    else if (answer[0].ToString().ToLower() == "n")
                        goto pickColor;
                    else
                    {
                        Console.WriteLine("Yes or No only");
                        goto pickColor;
                    }
                case "yellow":
                    Console.WriteLine("You want to declare yellow as the color? Y:N");
                    answer = Console.ReadLine();
                    if (answer[0].ToString().ToLower() == "y")
                    {
                        return CardColor.Yellow;
                    }
                    else if (answer[0].ToString().ToLower() == "n")
                        goto pickColor;
                    else
                    {
                        Console.WriteLine("Yes or No only");
                        goto pickColor;
                    }
                default:
                    Console.WriteLine("You can only pick from 'Red' 'Blue' 'Green' 'Yellow'");
                    goto pickColor;
            }
        }
        public async Task selectCard()
        {
        selctingCard:
            await discordPlayer.SendMessageAsync("What card would you like to play? Type Hand To see your cards, reply in the gameChannel");
            string card = Console.ReadLine();
            string[] splitCard = card.Split(' ');
            if (splitCard.Length == 0)
            {
                await discordPlayer.SendMessageAsync("Make sure you do the full card name with spaces");
                goto selctingCard;
            }
            if (splitCard[0].ToLower() == "red")
            {
                Golab.color = CardColor.Red;
            }
            else if (splitCard[0].ToLower() == "blue")
            {
                Golab.color = CardColor.Blue;
            }
            else if (splitCard[0].ToLower() == "yellow")
            {
                Golab.color = CardColor.Yellow;
            }
            else if (splitCard[0].ToLower() == "green")
            {
                Golab.color = CardColor.Green;
            }
            else if (splitCard[0].ToLower() == "wild")
            {
                Golab.color = CardColor.Wild;
            }
            else if (splitCard[0].ToLower() == "hand")
            {
                await ShowHand();
                goto selctingCard;
            }
            else
            {
                await discordPlayer.SendMessageAsync("You have to pick either red, blue, yellow, green, wild");
                goto selctingCard;
            }

            if (splitCard[1].ToLower() != null)
            {
                string val = splitCard[1].ToLower();
                switch (val)
                {
                    case "zero":
                        Golab.value = CardValue.Zero;
                        break;
                    case "one":
                        Golab.value = CardValue.One;
                        break;
                    case "two":
                        Golab.value = CardValue.Two;
                        break;
                    case "three":
                        Golab.value = CardValue.Three;
                        break;
                    case "four":
                        Golab.value = CardValue.Four;
                        break;
                    case "five":
                        Golab.value = CardValue.Five;
                        break;
                    case "six":
                        Golab.value = CardValue.Six;
                        break;
                    case "seven":
                        Golab.value = CardValue.Seven;
                        break;
                    case "eight":
                        Golab.value = CardValue.Eight;
                        break;
                    case "nine":
                        Golab.value = CardValue.Nine;
                        break;
                    case "reverse":
                        Golab.value = CardValue.Reverse;
                        break;
                    case "drawtwo":
                        Golab.value = CardValue.DrawTwo;
                        break;
                    case "drawfour":
                        Golab.value = CardValue.DrawFour;
                        break;
                    case "skip":
                        Golab.value = CardValue.Skip;
                        break;
                    case "wild":
                        Golab.value = CardValue.Wild;
                        break;
                    default:
                        await discordPlayer.SendMessageAsync("You have to pick from zero-nine reverse, drawTwo, drawFour, skip, wild");
                        goto selctingCard;
                }
            }
            else
            {
                await discordPlayer.SendMessageAsync("Make sure you do the full card name with spaces");
                goto selctingCard;
            }
            Card currentPlayed = new Card()
            {
                Color = Golab.color,
                Value = Golab.value,
                Score = (int)Golab.value
            };
            Golab.currentCard = currentPlayed;
        }

        public PlayerTurn DrawCard(PlayerTurn previousTurn, CardDeck drawPile)
        {
            PlayerTurn turn = new PlayerTurn();
            var drawnCard = drawPile.Draw(1);
            Hand.AddRange(drawnCard);

            if (HasMatch(previousTurn.Card))  //If the drawn card matches the discard, play it
            {
                turn = PlayMatchingCard(previousTurn.Card);
                turn.Result = TurnResult.ForceDrawPlay;
            }
            else
            {
                turn.Result = TurnResult.ForceDraw;
                turn.Card = previousTurn.Card;
                turn.DeclaredColor = turn.Card.Color;
            }
            return turn;
        }

        private PlayerTurn PlayMatchingCard(Card currentDiscard)
        {
            var turn = new PlayerTurn();
            turn.Result = TurnResult.PlayedCard;
            var matching = Hand.Where(x => x.Color == currentDiscard.Color || x.Value == currentDiscard.Value || x.Color == CardColor.Wild).ToList();

            //We cannot play wild draw four unless there are no other matches.
            if (matching.All(x => x.Value == CardValue.DrawFour))
            {
                turn.Card = matching.First();
                turn.DeclaredColor = selectColor();
                turn.Result = TurnResult.WildCard;
                Hand.Remove(matching.First());

                return turn;
            }

            //Otherwise, we play the card that would cause the most damage to the next player.
            if (matching.Any(x => x.Value == CardValue.DrawTwo))
            {
                turn.Card = matching.First(x => x.Value == CardValue.DrawTwo);
                turn.Result = TurnResult.DrawTwo;
                turn.DeclaredColor = turn.Card.Color;
                Hand.Remove(turn.Card);

                return turn;
            }

            if (matching.Any(x => x.Value == CardValue.Skip))
            {
                turn.Card = matching.First(x => x.Value == CardValue.Skip);
                turn.Result = TurnResult.Skip;
                turn.DeclaredColor = turn.Card.Color;
                Hand.Remove(turn.Card);

                return turn;
            }

            if (matching.Any(x => x.Value == CardValue.Reverse))
            {
                turn.Card = matching.First(x => x.Value == CardValue.Reverse);
                turn.Result = TurnResult.Reversed;
                turn.DeclaredColor = turn.Card.Color;
                Hand.Remove(turn.Card);

                return turn;
            }

            // At this point the player has a choice of sorts
            // Assuming he has a match on color AND a match on value 
            // (with none of the matches being attacking cards), 
            // he can choose which to play.  For this modeling practice, we'll assume 
            // that playing the match with MORE possible matches from his hand 
            // is the better option.

            var matchOnColor = matching.Where(x => x.Color == currentDiscard.Color);
            var matchOnValue = matching.Where(x => x.Value == currentDiscard.Value);
            if (matchOnColor.Any() && matchOnValue.Any())
            {
                var correspondingColor = Hand.Where(x => x.Color == matchOnColor.First().Color);
                var correspondingValue = Hand.Where(x => x.Value == matchOnValue.First().Value);
                if (correspondingColor.Count() >= correspondingValue.Count())
                {
                    turn.Card = matchOnColor.First();
                    turn.DeclaredColor = turn.Card.Color;
                    Hand.Remove(matchOnColor.First());

                    return turn;
                }
                else //Match on value
                {
                    turn.Card = matchOnValue.First();
                    turn.DeclaredColor = turn.Card.Color;
                    Hand.Remove(matchOnValue.First());

                    return turn;
                }
            }
            else if (matchOnColor.Any()) //Play the match on color
            {
                turn.Card = matchOnColor.First();
                turn.DeclaredColor = turn.Card.Color;
                Hand.Remove(matchOnColor.First());

                return turn;
            }
            else if (matchOnValue.Any()) //Play the match on value
            {
                turn.Card = matchOnValue.First();
                turn.DeclaredColor = turn.Card.Color;
                Hand.Remove(matchOnValue.First());

                return turn;
            }

            //Play regular wilds last.  If a wild becomes our last card, we win on the next turn!
            if (matching.Any(x => x.Value == CardValue.Wild))
            {
                turn.Card = matching.First(x => x.Value == CardValue.Wild);
                turn.DeclaredColor = selectColor();
                turn.Result = TurnResult.WildCard;
                Hand.Remove(turn.Card);

                return turn;
            }

            //This should never happen
            turn.Result = TurnResult.ForceDraw;
            return turn;
        }

        private async Task DisplayTurn(PlayerTurn currentTurn)
        {
            if (currentTurn.Result == TurnResult.ForceDraw)
            {
                await Golab.gameChannel.SendMessageAsync("Player " + discordPlayer.Mention + " is forced to draw.");
            }
            if (currentTurn.Result == TurnResult.ForceDrawPlay)
            {
                await Golab.gameChannel.SendMessageAsync("Player " + discordPlayer.Mention + " is forced to draw AND can play the drawn card!");
            }

            if (currentTurn.Result == TurnResult.PlayedCard
                || currentTurn.Result == TurnResult.Skip
                || currentTurn.Result == TurnResult.DrawTwo
                || currentTurn.Result == TurnResult.WildCard
                || currentTurn.Result == TurnResult.WildDrawFour
                || currentTurn.Result == TurnResult.Reversed
                || currentTurn.Result == TurnResult.ForceDrawPlay)
            {
                await Golab.gameChannel.SendMessageAsync("Player " + discordPlayer.Mention + " plays a " + currentTurn.Card.DisplayValue + " card.");
                if (currentTurn.Card.Color == CardColor.Wild)
                {
                    await Golab.gameChannel.SendMessageAsync("Player " + discordPlayer.Mention + " declares " + currentTurn.DeclaredColor.ToString() + " as the new color.");
                }
                if (currentTurn.Result == TurnResult.Reversed)
                {
                    await Golab.gameChannel.SendMessageAsync("Turn order reversed!");
                }
            }

            if (Hand.Count == 1)
            {
                await Golab.gameChannel.SendMessageAsync("Player " + discordPlayer.Mention + " shouts Uno!");
            }
        }

        private bool HasMatch(Card card)
        {
            if (card != null)
                return Hand.Any(x => x.Color == card.Color || x.Value == card.Value || x.Color == CardColor.Wild);
            else
            {
                Console.WriteLine("Card is null in has match Card...");
                return false;
            }
        }

        private bool HasMatch(CardColor color)
        {
            return Hand.Any(x => x.Color == color || x.Color == CardColor.Wild);
        }


        private void SortHand()
        {
            this.Hand = this.Hand.OrderBy(x => x.Color).ThenBy(x => x.Value).ToList();
        }
        public async Task ShowHand()
        {
            SortHand();
            foreach (var card in Hand)
            {
                await discordPlayer.SendMessageAsync(Enum.GetName(typeof(CardColor), card.Color) + " " + Enum.GetName(typeof(CardValue), card.Value) + "  ");
            }
        }
    }
}
