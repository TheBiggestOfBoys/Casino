using System;

namespace Casino
{
    internal class Card(Card.Suits suit, Card.Values value)
    {
        public readonly Values Value = value;
        public readonly Suits Suit = suit;

        public readonly char Symbol = GetSymbol(suit);
        public readonly string Abbreviation = GetAbbreviation(value);

        public readonly ConsoleColor Color = GetColor(suit);

        public void DisplayCardWithColor()
        {
            Console.ForegroundColor = Color;
            Console.Write(this);
            Console.ResetColor();
        }

        public override string ToString() => $"{Symbol}{Abbreviation}";

        #region Determine Methods
        private static char GetSymbol(Suits suit) => suit switch
        {
            Suits.Hearts => '♥',
            Suits.Diamonds => '♦',
            Suits.Spades => '♠',
            Suits.Clubs => '♣'
        };

        private static ConsoleColor GetColor(Suits suit) => suit switch
        {
            Suits.Hearts => ConsoleColor.Red,
            Suits.Diamonds => ConsoleColor.Red,
            Suits.Spades => ConsoleColor.DarkGray,
            Suits.Clubs => ConsoleColor.DarkGray
        };

        private static string GetAbbreviation(Values value) => value <= Values.Ten ? ((int)value).ToString() : value.ToString()[0].ToString();
        #endregion

        #region Enums
        internal enum Suits
        {
            Hearts,
            Diamonds,
            Clubs,
            Spades
        }

        internal enum Values
        {
            Two = 2,
            Three = 3,
            Four = 4,
            Five = 5,
            Six = 6,
            Seven = 7,
            Eight = 8,
            Nine = 9,
            Ten = 10,
            Jack = 11,
            Queen = 12,
            King = 13,
            Ace = 14
        }
        #endregion
    }
}
