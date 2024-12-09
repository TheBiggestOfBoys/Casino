using System;

namespace Casino.Objects
{
    /// <summary>
    /// The <see cref="Card"/> <see cref="object"/> for card games.
    /// </summary>
    /// <param name="suit">The <see cref="Suits"/> of the <see cref="Card"/></param>
    /// <param name="value">The <see cref="Values"/> of the <see cref="Card"/></param>
    public class Card(Card.Suits suit, Card.Values value)
    {
        /// <summary>
        /// The <see cref="Values"/> of the <see cref="Card"/>
        /// </summary>
        public readonly Values Value = value;
        /// <summary>
        /// The <see cref="Suits"/> of the <see cref="Card"/>
        /// </summary>
        public readonly Suits Suit = suit;

        /// <summary>
        /// If the card is visible or not
        /// </summary>
        public bool IsHidden { get; private set; } = false;

        /// <summary>
        /// The <see cref="char"/> of the <see cref="Suit"/>.
        /// </summary>
        public readonly char Symbol = GetSymbol(suit);
        /// <summary>
        /// The <see cref="string"/> of the <see cref="Value"/>.
        /// </summary>
        public readonly string Abbreviation = GetAbbreviation(value);

        /// <summary>
        /// The <see cref="ConsoleKeyInfo"/> of the <see cref="Suit"/>
        /// </summary>
        public readonly ConsoleColor Color = GetColor(suit);

        /// <summary>
        /// Prints the <see cref="ToString()"/> with the <see cref="Color"/>.
        /// </summary>
        public void DisplayCardWithColor()
        {
            if (!IsHidden)
            {
                Console.ForegroundColor = Color;
            }
            Console.Write(ToString());
            Console.ResetColor();
        }

        /// <summary>
        /// Toggles this <see cref="Card"/>'s visibility.
        /// </summary>
        public void Flip()
        {
            IsHidden = !IsHidden;
        }

        /// <summary>
        /// Show the abbreviation of the card's <see cref="Suit"/> & <see cref="Value"/>.
        /// </summary>
        /// <returns>The <see cref="Suit"/>'s <see cref="char"/> & the <see cref="Value"/>'s <see cref="string"/>.</returns>
        public override string ToString() => IsHidden ? "?" : Symbol + Abbreviation;

        #region Determine Methods
        /// <summary>
        /// Gets the correct <see cref="char"/> of a <see cref="Suits"/>.
        /// </summary>
        /// <param name="suit">The <see cref="Suits"/> to get the <see cref="ConsoleColor"/> of.</param>
        /// <returns>The corresponding <see cref="char"/>.</returns>
        private static char GetSymbol(Suits suit) => suit switch
        {
            Suits.Hearts => '♥',
            Suits.Diamonds => '♦',
            Suits.Spades => '♠',
            Suits.Clubs => '♣',
            _ => '?'
        };

        /// <summary>
        /// Gets the correct <see cref="ConsoleColor"/> of a <see cref="Suits"/>.
        /// </summary>
        /// <param name="suit">The <see cref="Suits"/> to get the <see cref="ConsoleColor"/> of.</param>
        /// <returns>The corresponding <see cref="ConsoleColor"/>.</returns>
        private static ConsoleColor GetColor(Suits suit) => suit switch
        {
            Suits.Hearts or Suits.Diamonds => ConsoleColor.Red,
            Suits.Spades or Suits.Clubs => ConsoleColor.DarkGray,
            _ => ConsoleColor.White
        };

        /// <summary>
        /// Gets the <see cref="string"/> representation of a <see cref="Values"/>.
        /// </summary>
        /// <param name="value">The <see cref="Values"/> to determine the <see cref="string"/> of.</param>
        /// <returns>A number if the <see cref="Values"/> is less <= 10, or the first <see cref="char"/> of the <see cref="Values"/>'s <see cref="string"/> representation.</returns>
        private static string GetAbbreviation(Values value) => value <= Values.Ten ? ((int)value).ToString() : value.ToString()[0].ToString();
        #endregion

        #region Enums
        /// <summary>
        /// The <see cref="Suits"/> of a <see cref="Card"/>.
        /// </summary>
        public enum Suits : byte
        {
            Hearts,
            Diamonds,
            Clubs,
            Spades
        }

        /// <summary>
        /// The <see cref="Values"/> of a <see cref="Card"/>, including number and face cards.
        /// </summary>
        public enum Values : byte
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
