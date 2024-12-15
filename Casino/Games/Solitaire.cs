using Casino.Objects;
using System;
using System.Collections.Generic;

namespace Casino.Games
{
    public class Solitaire(int money) : CardGame(money)
    {
        /// <summary>
        /// The <see cref="List{Card}"/>s to place onto.
        /// </summary>
        /// <param name="money">The starting money.</param>
        private readonly List<Card>[] Board = [
            new(7),
            new(6),
            new(5),
            new(4),
            new(3),
            new(2),
            new(1)
        ];

        /// <summary>
        /// The finished piles for each <see cref="Card.Suits"/>.
        /// </summary>
        private readonly List<Card>[] FinishedPiles = [
            new(13),
            new(13),
            new(13),
            new(13)
        ];

        /// <summary>
        /// How many times <see cref="Card"/>s have been drawn from the <see cref="Deck"/>.
        /// </summary>
        private int Draws = 0;

        /// <summary>
        /// How many <see cref="Card"/>s are visible from the <see cref="MainDeck"/>.
        /// </summary>
        private int DrawnCardsCount = 0;
        /// <summary>
        /// The index of the selected <see cref="Card"/> from the <see cref="MainDeck"/>/
        /// </summary>
        private int DrawnCardsIndex = 0;

        /// <inheritdoc/>
        public override int CustomGameFlow(int bet)
        {
            base.CustomGameFlow(bet);

            Console.Clear();
            DisplayGame();
            Console.ReadKey();
            return 0;
        }

        /// <inheritdoc/>
        public override void Initialize()
        {
            base.Initialize();
            foreach (List<Card> row in Board)
            {
                int count = row.Capacity;
                for (int i = 0; i < count; i++)
                {
                    MainDeck.TransferTopCard(row);
                    row[i].Flip();
                }
                // Unhides the bottom card
                row[^1].Flip();
            }
        }

        /// <summary>
        /// Draws the next <see cref="Card"/> from the <see cref="MainDeck"/>.
        /// </summary>
        public void DrawNextCard()
        {
            DrawnCardsCount = DrawnCardsCount > 3 ? 0 : DrawnCardsCount + 1;
        }

        /// <inheritdoc/>
        /// Displays all the rows and piles.
        public override void DisplayGame()
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
                MainDeck[i].DisplayCardWithColor();
            }

            x = 0;
            y = 15;
            Console.SetCursorPosition(x, y);
            Console.WriteLine($"Cards left in deck: {MainDeck.Count}");
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
