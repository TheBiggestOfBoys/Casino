using System;
using System.Collections.Generic;

namespace Casino
{
    internal class Solitaire
    {
        private List<Card>[] Board = [
            new List<Card>(7),
            new List<Card>(6),
            new List<Card>(5),
            new List<Card>(4),
            new List<Card>(3),
            new List<Card>(2),
            new List<Card>(1),
        ];

        List<Card>[] FinishedPiles = [
            new List<Card>(13),
            new List<Card>(13),
            new List<Card>(13),
            new List<Card>(13),
        ];

        private Deck cards = Deck.CreateFullDeck();

        private int Draws = 0;

        private int DrawnCardsCount = 0;
        private int DrawnCardsIndex = 0;

        public Solitaire()
        {
            foreach (List<Card> row in Board)
            {
                int count = row.Capacity;
                for (int i = 0; i < count; i++)
                {
                    cards.TransferTopCard(row);
                    row[i].Flip();
                }
                // Unhides the bottom card
                row[^1].Flip();
            }
        }

        public int Play()
        {
            Console.Clear();
            DisplayGame();
            Console.ReadKey();
            return 0;
        }

        public void DrawNextCard()
        {
            if (DrawnCardsCount > 3)
            {
                DrawnCardsCount = 0;
            }
            else
            {
                DrawnCardsCount++;
            }
        }

        public void DisplayGame()
        {
            int x = 0;
            int y = 0;
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
            y = 0;

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

            x = 35;
            y = 5;

            for (int i = DrawnCardsIndex; i < DrawnCardsIndex + DrawnCardsCount; i++)
            {
                cards[i].DisplayCardWithColor();
            }

            x = 0;
            y = 15;
            Console.SetCursorPosition(x, y);
            Console.WriteLine($"Cards left in deck: {cards.Count}");
            Console.WriteLine($"Draws: {Draws}");
        }
    }
}
