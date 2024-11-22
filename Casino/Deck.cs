using System;
using System.Collections.Generic;

namespace Casino
{
    /// <summary>
    /// Provides functions for managing a <see cref="List{Card}"/> of <see cref="Card"/>s.
    /// </summary>
    internal class Deck : List<Card>
    {
        #region Deck management functions
        /// <summary>
        /// Creates an entire 52 <see cref="Card"/> deck of all <see cref="Card.Suits"/> and <see cref="Card.Values"/>.
        /// </summary>
        /// <returns>A full deck</returns>
        public static Deck CreateFullDeck()
        {
            Deck deck = [];
            // Populate the deck of cards
            foreach (Card.Suits suit in Enum.GetValues(typeof(Card.Suits)))
            {
                foreach (Card.Values value in Enum.GetValues(typeof(Card.Values)))
                {
                    deck.Add(new(suit, value));
                }
            }
            return deck;
        }

        /// <summary>
        /// Shuffles the deck of cards in-place
        /// </summary>
        public void Shuffle()
        {
            Random random = new();
            Card[] cards = ToArray();
            random.Shuffle(cards);
            Clear();
            AddRange(cards);
        }

        /// <summary>
        /// Evenly distributes the <see cref="List{Card}"/>'s <see cref="Card"/>s between 2 <see cref="List{Card}"/>.
        /// </summary>
        /// <param name="pile1">The first <see cref="List{Card}"/>.</param>
        /// <param name="pile2">The second <see cref="List{Card}"/>.</param>
        public void Split(List<Card> pile1, List<Card> pile2)
        {
            for (int i = 0; i < 26; i++)
            {
                TransferTopCard(pile1);
                TransferTopCard(pile2);
            }
        }
        #endregion

        #region Show Functions
        /// <summary>
        /// Displays all cards in the list, with their <see cref="Card.Color"/>.
        /// </summary>
        public void ShowCards()
        {
            foreach (Card card in this)
            {
                card.DisplayCardWithColor();
                Console.WriteLine();
            }
        }
        #endregion

        #region Card Transfers
        /// <summary>
        /// Transfers card from deck to hand
        /// </summary>
        /// <param name="hand">Usually the deck</param>
        public void TransferTopCard(List<Card> hand)
        {
            hand.Add(this[0]);
            RemoveAt(0);
        }

        /// <summary>
        /// Discard the cards in the hand back to the bottom of the deck
        /// </summary>
        /// <param name="hand">The hand to remove the cards from</param>
        public void DiscardCards(List<Card> hand)
        {
            AddRange(hand);
            hand.Clear();
        }
        #endregion
    }
}
