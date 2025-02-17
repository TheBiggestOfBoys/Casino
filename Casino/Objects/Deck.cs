﻿using System;
using System.Collections.Generic;
using System.Linq;

namespace Casino.Objects
{
    /// <summary>
    /// Provides functions for managing a <see cref="List{Card}"/> of <see cref="Card"/>s.
    /// </summary>
    public class Deck : List<Card>
    {
        /// <summary>
        /// Is an Ace the highest card (<c><see cref="Card.Value"/> == 14</c>) or the lowest card (<c><see cref="Card.Value"/> == 1</c>)?
        /// </summary>
        public readonly bool IsAceHighest;

        #region Constructors
        public Deck() { }

        /// <summary>
        /// Initializes the <see cref="Deck"/> with a user determined <see cref="List{Card}"/> of <see cref="Card"/>s.
        /// </summary>
        /// <param name="cards">The <see cref="List{Card}"/> of <see cref="Card"/>s to out in the <see cref="Deck"/>.</param>
        public Deck(List<Card> cards)
        {
            AddRange(cards);
            IsAceHighest = false;
        }

        /// <param name="isAceHighest">Is an Ace the highest card (14) or the lowest card (1)?</param>
        public Deck(bool isAceHighest)
        {
            IsAceHighest = isAceHighest;
            IsAceHighest = false;
        }

        /// <summary>
        /// Initializes the <see cref="Deck"/> with a user determined <see cref="List{Card}"/> of <see cref="Card"/>s.
        /// </summary>
        /// <param name="cards">The <see cref="List{Card}"/> of <see cref="Card"/>s to out in the <see cref="Deck"/>.</param>
        /// <param name="isAceHighest">Is an Ace the highest card (14) or the lowest card (1)?</param>
        public Deck(List<Card> cards, bool isAceHighest)
        {
            IsAceHighest = isAceHighest;
            AddRange(cards);
        }
        #endregion

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
        /// Evenly distributes the <see cref="Deck"/>'s <see cref="Card"/>s between 2 <see cref="List{Card}"/>.
        /// </summary>
        /// <param name="pile1">The first <see cref="List{Card}"/>.</param>
        /// <param name="pile2">The second <see cref="List{Card}"/>.</param>
        public void Split(out List<Card> pile1, out List<Card> pile2)
        {
            int halfIndex = Count / 2;
            pile1 = this.Take(halfIndex).ToList();
            pile2 = this.Skip(halfIndex).ToList();
        }

        /// <summary>
        /// Evenly distributes the <see cref="Deck"/>'s <see cref="Card"/>s between 2 <see cref="Deck"/>.
        /// </summary>
        /// <param name="pile1">The first <see cref="Deck"/>.</param>
        /// <param name="pile2">The second <see cref="Deck"/>.</param>
        public void Split(out Deck deck1, out Deck deck2)
        {
            int halfIndex = Count / 2;
            deck1 = new(this.Take(halfIndex).ToList());
            deck2 = new(this.Skip(halfIndex).ToList());
            Clear();
        }

        /// <summary>
        /// Gets the total value of the <see cref="Card"/>s in the <see cref="Deck"/>.
        /// Includes logic for <see cref="IsAceHighest"/>.
        /// </summary>
        /// <returns>The total value</returns>
        public int Value()
        {
            int value = 0;
            foreach (Card card in this)
            {
                value += card.Value == Card.Values.Ace
                    ? (IsAceHighest ? 14 : 1)
                    : (int)card.Value;
            }
            return value;
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

        #region Operators
        /// <summary>
        /// Adding 2 deck together.  Basically just a nice shorthand
        /// </summary>
        /// <param name="deck1">The <see cref="Deck"/> to add to.</param>
        /// <param name="deck2">The <see cref="Deck"/> to add from.</param>
        /// <returns></returns>
        public static Deck operator +(Deck deck1, Deck deck2)
        {
            deck1.AddRange(deck2);
            return deck1;
        }

        /// <summary>
        /// Gets the total value of the <see cref="Deck"/>
        /// </summary>
        /// <param name="deck"></param>
        /// <param name="isAceHighest">Is an Ace the highest card (14) or the lowest card (1)?</param>
        public static explicit operator int(Deck deck) => deck.Value();
        #endregion
    }
}
