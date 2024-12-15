using Casino.Objects;

namespace Casino.Games
{
    public abstract class CardGame(int money) : Game(money)
    {
        public Deck MainDeck = Deck.CreateFullDeck();

        /// <inheritdoc/>
        public override void Initialize()
        {
            MainDeck.Shuffle();
        }
    }
}
