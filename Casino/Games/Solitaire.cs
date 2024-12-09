using Casino.Objects;
using System;
using System.Collections.Generic;

namespace Casino.Games
{
    public class Solitaire(int money) : Game(money)
    {
        /// <summary>
        /// The <see cref="List{Card}"/>s to place onto.
        /// </summary>
        /// <param name="money">The starting money.</param>
        private readonly List<Card>[] Board = [
            new List<Card>(7),
            new List<Card>(6),
            new List<Card>(5),
            new List<Card>(4),
            new List<Card>(3),
            new List<Card>(2),
            new List<Card>(1),
        ];

        /// <summary>
        /// The finished piles for each <see cref="Card.Suits"/>.
        /// </summary>
        private readonly List<Card>[] FinishedPiles = [
            new List<Card>(13),
            new List<Card>(13),
            new List<Card>(13),
            new List<Card>(13),
        ];

        /// <summary>
        /// The <see cref="Deck"/> to draw <see cref="Card"/>s from.
        /// </summary>
        private readonly Deck Cards = Deck.CreateFullDeck();

        /// <summary>
        /// How many times <see cref="Card"/>s have been drawn from the <see cref="Deck"/>.
        /// </summary>
        private readonly int Draws = 0;

        /// <summary>
        /// How many <see cref="Card"/>s are visible from the <see cref="Cards"/>.
        /// </summary>
        private int DrawnCardsCount = 0;
        /// <summary>
        /// The index of the selected <see cref="Card"/> from the <see cref="Cards"/>/
        /// </summary>
        private readonly int DrawnCardsIndex = 0;

        /// <summary>
        /// Plays the game
        /// </summary>
        /// <returns></returns>
        public override int CustomGameFlow(int bet)
        {
            foreach (List<Card> row in Board)
            {
                int count = row.Capacity;
                for (int i = 0; i < count; i++)
                {
                    Cards.TransferTopCard(row);
                    row[i].Flip();
                }
                // Unhides the bottom card
                row[^1].Flip();
            }

            Console.Clear();
            DisplayGame();
            Console.ReadKey();
            return 0;
        }

        /// <summary>
        /// Draws the next <see cref="Card"/> from the <see cref="Cards"/>.
        /// </summary>
        public void DrawNextCard()
        {
            DrawnCardsCount = DrawnCardsCount > 3 ? 0 : DrawnCardsCount + 1;
        }

        /// <summary>
        /// Shows the Board in color with piles, finished decks, and the draw deck
        /// </summary>
        public void DisplayGame()
        {
            int x = 0;
            int y;
            foreach (List<Card> row in Board)
            {
                y = 0;
                foreach (Card card in row)
                {
                    Console.SetCursorPosition(x, y);
                    card.DisplayCardWithColor();
                    y++;
                }
                x += 4;
            }

            x = 35;

            foreach (List<Card> row in FinishedPiles)
            {
                y = 0;

                Console.SetCursorPosition(x, y);
                if (row.Count == 0)
                {
                    Console.Write("__");
                }
                else
                {
                    row[^1].DisplayCardWithColor();
                }
                x += 4;
            }

            for (int i = DrawnCardsIndex; i < DrawnCardsIndex + DrawnCardsCount; i++)
            {
                Cards[i].DisplayCardWithColor();
            }

            x = 0;
            y = 15;
            Console.SetCursorPosition(x, y);
            Console.WriteLine($"Cards left in deck: {Cards.Count}");
            Console.WriteLine($"Draws: {Draws}");
        }

        /// <summary>
        /// Checks if the <see cref="Card"/> can be played on the given <see cref="List{Card}"/>.
        /// </summary>
        /// <param name="card">The <see cref="Card"/> to play.</param>
        /// <param name="pile">The <see cref="List{Card}"/> to play on.</param>
        /// <returns>If the value of the <see cref="Card"/> is lower than the <see cref="Card"/> at the bottom of the <see cref="List{Card}"/>, and if the <see cref="Card.Color"/>s are different.</returns>
        private static bool CanPlay(Card card, List<Card> pile) => card.Value < pile[^1].Value && card.Color != pile[^1].Color;
    }
}
