using System;
using System.Collections.Generic;
using System.Linq;
using static Casino.Card;

namespace Casino
{
    internal class BlackJack(int money)
    {
        private Deck deck = Deck.CreateFullDeck();
        private Deck playerHand = [];
        private Deck dealerHand = [];

        private byte rounds = 0;
        private readonly int startingMoney = money;
        private int money = money;

        /// <summary>
        /// Plays the game of Black Jack
        /// </summary>
        /// <returns>The money left at the end of the rounds.</returns>
        public int Play()
        {
            deck.Shuffle(); DisplayRules();
            ConsoleKey key = ConsoleKey.Escape;
            Console.WriteLine("Press Q to exit");
            while (key != ConsoleKey.Q)
            {
                if (!PromptForBet(out int bet))
                    continue;

                DealInitialCards(playerHand);
                DealInitialCards(dealerHand);
                ShowHands();
                bool playerBust = PlayerTurn();
                if (playerBust)
                {
                    Console.WriteLine($"You went over 21 and lost {bet}"); money -= bet;
                }
                else
                {
                    DealerTurn();
                    ShowHands(); // Show final hands after dealer's turn
                    int playerValue = CountValues(playerHand);
                    int dealerValue = CountValues(dealerHand);
                    DetermineWinner(playerValue, dealerValue, bet);
                }
                rounds++;
                deck.DiscardCards(playerHand);
                deck.DiscardCards(dealerHand);
                if (!PromptToContinue(ref key))
                    break;
            }
            DisplayFinalResults(); return money;
        }

        private void ShowHands()
        {
            Console.WriteLine();
            Console.WriteLine("Current Hands:");
            ShowHand(playerHand, false);
            ShowHand(dealerHand, true);
        }

        private static void DisplayRules()
        {
            Console.WriteLine("RULES OF BLACK JACK:");
            Console.WriteLine("The goal is to get to 21, or as close as possible.");
            Console.WriteLine("If you go over 21, you automatically lose.");
            Console.WriteLine("All face cards count as 10 and Aces can count as either 1 or 11");
        }

        private bool PromptForBet(out int bet)
        {
            Console.Write("How much do you want to bet?: ");
            return int.TryParse(Console.ReadLine(), out bet) && bet <= money;
        }

        private void DealInitialCards(Deck hand)
        {
            deck.TransferTopCard(hand);
            deck.TransferTopCard(hand);
        }

        private bool PlayerTurn()
        {
            ConsoleKey key;
            byte value = CountValues(playerHand);
            while (value <= 21 && (key = Console.ReadKey().Key) != ConsoleKey.Enter)
            {
                if (key == ConsoleKey.Spacebar)
                {
                    deck.TransferTopCard(playerHand);
                    value = CountValues(playerHand);
                    ShowHand(playerHand, false);
                }
            }
            return value > 21;
        }

        private void DealerTurn()
        {
            while (CountValues(dealerHand) <= 16)
            {
                deck.TransferTopCard(dealerHand);
            }
        }

        private void DetermineWinner(int playerValue, int dealerValue, int bet)
        {
            if (dealerValue > 21)
            {
                Console.WriteLine("Dealer busts! You win."); money += bet;
            }
            else if (playerValue > dealerValue)
            {
                Console.WriteLine($"You win! Dealer had {dealerValue}."); money += bet;
            }
            else if (dealerValue > playerValue)
            {
                Console.WriteLine($"Dealer wins with {dealerValue}. You lose {bet}."); money -= bet;
            }
            else
            {
                Console.WriteLine("It's a tie!");
            }
        }

        private void ShowHand(Deck hand, bool isDealer)
        {
            string playerName = isDealer ? "Dealer" : "Player";
            Console.Write($"{playerName}'s hand: ");
            for (int i = 0; i < hand.Count; i++)
            {
                if (isDealer && i > 0)
                {
                    Console.Write('?');
                }
                else
                {
                    hand[i].DisplayCardWithColor();
                }
            }

            if (!isDealer)
            {
                byte value = CountValues(hand);
                Console.WriteLine($" (value: {value})");
                if (HasAce(hand))
                {
                    Console.WriteLine($" (with Ace as 11: {value + 10})");
                }
            }
            else
            {
                Console.Write(" (value: ?)");
            }

            Console.WriteLine();
        }

        private static bool HasAce(Deck hand) => hand.Any(card => card.Value == Card.Values.Ace);

        private static bool PromptToContinue(ref ConsoleKey key)
        {
            Console.WriteLine($"Continue? (press {ConsoleKey.Q} to quit, or any other key to continue)");
            key = Console.ReadKey().Key;
            return key != ConsoleKey.Q;
        }

        private void DisplayFinalResults()
        {
            Console.WriteLine($"You played {rounds} rounds, and came in with ${startingMoney}, you now have ${money}, resulting in a net of ${money - startingMoney}.");
        }

        /// <summary>
        /// Determines if the hand goes over 21 (bust)
        /// </summary>
        /// <param name="hand"></param>
        /// <returns></returns>
        private static byte CountValues(List<Card> hand)
        {
            byte totalValue = 0;
            foreach (Card card in hand)
            {
                totalValue += (card.Value >= Card.Values.Jack) ? (byte)10 : (byte)card.Value;
            }
            return totalValue;
        }
    }
}
