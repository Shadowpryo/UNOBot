using Discord.Commands;
using System;
using System.Collections.Generic;
using System.Text;

namespace UnoBot
{
    public class Card
    {
        public CardColor Color { get; set; }
        public CardValue Value { get; set; }
        [DontInject]
        public int Score { get; set; }

        public string DisplayValue
        {
            get
            {
                if (Value == CardValue.Wild)
                {
                    return Value.ToString();
                }
                return Color.ToString() + " " + Value.ToString();
            }
        }
    }
}
