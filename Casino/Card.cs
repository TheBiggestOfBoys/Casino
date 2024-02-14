namespace Casino
{
    internal readonly struct Card(Card.Suits suit, Card.Values value)
    {
        public readonly Values Value = value;
        public readonly Suits Suit = suit;

        public override readonly string ToString() => $"{Value} of {Suit}";

        internal enum Suits
        {
            Hearts,
            Diamonds,
            Clubs,
            Spades
        }

        internal enum Values
        {
            Ace = 1,
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
            King = 13
        }
    }
}
