using System;
using System.Collections.Generic;

namespace Casino
{
    internal class Program
    {
        public static Random random = new();
        public static double money = 100;
        public static List<Card> deck = new(52);

        static void Main()
        {
            // Populate the deck of cards
            foreach (Card.Suits suit in Enum.GetValues(typeof(Card.Suits)))
            {
                foreach (Card.Values value in Enum.GetValues(typeof(Card.Values)))
                {
                    deck.Add(new(suit, value));
                }
            }

            Shuffle(deck);
            Console.WriteLine("Finished Shuffling Deck");

            Console.WriteLine("Welcome to the casino!");
            Console.WriteLine($"You have ${money}");

            Console.WriteLine("1.\tBlack Jack");
            Console.WriteLine("2.\tWar");
            Console.WriteLine("3.\tPoker");
            Console.WriteLine("4.\tRoulette");
            Console.Write("What game do you want to play?: ");

            ConsoleKey key = Console.ReadKey().Key;
            Console.WriteLine();

            switch (key)
            {
                case ConsoleKey.D1:
                    BlackJack();
                    break;
                case ConsoleKey.D2:
                    break;
                case ConsoleKey.D3:
                    break;
                case ConsoleKey.D4:
                    break;
            }
        }

        /// <summary>
        /// Shuffles the deck of cards in-place using the "Fisher-Yates" algoritm
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="list"></param>
        public static void Shuffle(List<Card> deck)
        {
            for (int n = deck.Count - 1; n > 0; n--)
            {
                int k = random.Next(n + 1);
                (deck[n], deck[k]) = (deck[k], deck[n]);
            }
        }

        /// <summary>
        /// Diplays all cards in the list
        /// </summary>
        /// <param name="cards">The list to get cards from</param>
        public static void ShowCards(List<Card> cards)
        {
            foreach (Card card in cards)
            {
                Console.WriteLine(card);
            }
        }

        /// <summary>
        /// Transfers card from deck to hand
        /// </summary>
        /// <param name="hand">Usually the deck</param>
        /// <param name="fromList">Usually the hand</param>
        public static void TransferTopCard(List<Card> hand)
        {
            hand.Add(deck[0]);
            deck.RemoveAt(0);
        }

        /// <summary>
        /// Discard the cards in the hand back to the bottom of the deck
        /// </summary>
        /// <param name="hand">The hand to remove the cards from</param>
        public static void DiscardCards(List<Card> hand)
        {
            foreach (Card card in hand)
                deck.Add(card);

            hand.Clear();
        }

        #region Games
        public static void BlackJack()
        {
            double startingMoney = money;
            List<Card> hand = [];

            Console.WriteLine("RULES OF BLACK JACK:");

            byte rounds = 0;
            ConsoleKey key = ConsoleKey.Escape;
            Console.WriteLine($"Press {ConsoleKey.Q} to exit");

            while (key != ConsoleKey.Q)
            {
                Console.Write("How much do you want to bet?: ");
                if (double.TryParse(Console.ReadLine(), out double bet) && bet <= money)
                {
                    TransferTopCard(hand);
                    TransferTopCard(hand);

                    byte value = CountValues(hand);

                    ShowCards(hand);
                    Console.WriteLine($"Hand value: {value}");

                    Console.WriteLine($"Press {ConsoleKey.Enter} to hit, and {ConsoleKey.Enter} to call");
                    while (value <= 21 && key != ConsoleKey.Enter)
                    {
                        key = Console.ReadKey().Key;

                        if (key == ConsoleKey.Spacebar)
                            TransferTopCard(hand);

                        value = CountValues(hand);
                        ShowCards(hand);
                        Console.WriteLine($"Hand value: {value}");
                        Console.WriteLine();
                    }
                    if (value > 21)
                    {
                        Console.WriteLine($"You went over 21 and lost {bet}");
                        money -= bet;
                    }
                    else
                    {
                        Console.WriteLine($"You won {bet}");
                        money += bet;
                    }
                }
                rounds++;
                DiscardCards(hand);
                Console.WriteLine($"Continue? (press {ConsoleKey.Q} to quit, or any other key to continue)");
                key = Console.ReadKey().Key;
            }
            Console.WriteLine($"You played {rounds} rounds, and came in with ${startingMoney}, you now have ${money}, resulting in a net of ${money - startingMoney}.");
        }

        /// <summary>
        /// Determines if the hand goes over 21 (bust)
        /// </summary>
        /// <param name="hand"></param>
        /// <returns></returns>
        public static byte CountValues(List<Card> hand)
        {
            byte totalValue = 0;

            foreach (Card card in hand)
                if (card.Value >= Card.Values.Jack)
                    totalValue += 10;
                else
                    totalValue += (byte)card.Value;

            return totalValue;
        }
        #endregion
    }
}
