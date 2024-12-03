using System;
using System.Collections.Generic;
using System.Linq;
using static Casino.Card;

namespace Casino
{
    /// <summary>
    /// The <see cref="BlackJack"/> game object.
    /// </summary>
    /// <param name="money">Home much money was brought in.</param>
    internal class BlackJack(int money)
    {
        /// <summary>
        /// The Deck the cards will be dealt from.
        /// </summary>
        private Deck deck = Deck.CreateFullDeck();
        /// <summary>
        /// The Player's hand.
        /// </summary>
        private List<Card> playerHand = [];
        /// <summary>
        /// The Dealer's hand (only one <see cref="Card"/> is visible).
        /// </summary>
        private List<Card> dealerHand = [];

        /// <summary>
        /// How many round have been played.
        /// </summary>
        private byte rounds = 0;

        /// <summary>
        /// How much money you started with.
        /// </summary>
        private readonly int startingMoney = money;
        /// <summary>
        /// How much money you currently have.
        /// </summary>
        private int money = money;

        /// <summary>
        /// Plays the game of <see cref="BlackJack"/>.
        /// </summary>
        /// <returns>The money left at the end of the rounds.</returns>
        public int Play()
        {
            deck.Shuffle(); ShowRules();
            ConsoleKey key = ConsoleKey.Escape;
            Console.WriteLine("Press Q to exit");
            while (key != ConsoleKey.Q)
            {
                if (!PromptForBet(out int bet))
                {
                    continue;
                }

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
                {
                    break;
                }
            }
            ShowFinalResults(); return money;
        }

        #region Showing Functions
        /// <summary>
        /// Displays the Dealer & Player's hands.
        /// </summary>
        private void ShowHands()
        {
            Console.WriteLine();
            Console.WriteLine("Current Hands:");
            ShowHand(playerHand, false);
            HideDealerCards(dealerHand);
            ShowHand(dealerHand, true);
        }

        /// <summary>
        /// Shows the rules of Black Jack.
        /// </summary>
        private static void ShowRules()
        {
            Console.WriteLine("RULES OF BLACK JACK:");
            Console.WriteLine("The goal is to get to 21, or as close as possible.");
            Console.WriteLine("If you go over 21, you automatically lose.");
            Console.WriteLine("All face cards count as 10 and Aces can count as either 1 or 11");
        }

        /// <summary>
        /// Shows the rounds played and profit gained/lost from all the games
        /// </summary>
        private void ShowFinalResults()
        {
            Console.WriteLine($"You played {rounds} rounds, and came in with ${startingMoney}, you now have ${money}, resulting in a net of ${money - startingMoney}.");
        }

        /// <summary>
        /// Displays a <see cref="Deck"/>'s <see cref="Card"/>s and their total value of the <see cref="Card"/>'s <see cref="Values"/>.
        /// </summary>
        /// <param name="hand">The <see cref="List{Card}"/> to display.</param>
        /// <param name="isDealer">If the <see cref="List{Card}"/> is the dealer's deck</param>
        private static void ShowHand(List<Card> hand, bool isDealer)
        {
            string playerName = isDealer ? "Dealer" : "Player";
            Console.Write($"{playerName}'s hand: ");
            for (int i = 0; i < hand.Count; i++)
            {
                hand[i].DisplayCardWithColor();
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
                Console.Write($" (value: {(int)hand[0].Value}+?)");
            }

            Console.WriteLine();
        }

        /// <summary>
        /// Hides all of the dealer's <see cref="Card"/>s except the first one.
        /// </summary>
        /// <param name="dealerHand">The dealer's hand to read cards from.</param>
        private static void HideDealerCards(List<Card> dealerHand)
        {
            if (dealerHand[0].IsHidden)
            {
                dealerHand[0].Flip();
            }
            for (int i = 1; i < dealerHand.Count; i++)
            {
                if (!dealerHand[i].IsHidden)
                {
                    dealerHand[i].Flip();
                }
            }
        }
        #endregion

        #region Prompts
        /// <summary>
        /// Asks how much to bet.
        /// </summary>
        /// <param name="bet">How much to bet</param>
        /// <returns>If the entered bet was valid.</returns>
        private bool PromptForBet(out int bet)
        {
            Console.Write("How much do you want to bet?: ");
            return int.TryParse(Console.ReadLine(), out bet) && bet <= money;
        }

        /// <summary>
        /// Asks to play another round or to quit.
        /// </summary>
        /// <param name="key">The <see cref="ConsoleKey"/> pressed</param>
        /// <returns>If the quit key (<see cref="ConsoleKey.Q"/>) was pressed.</returns>
        private static bool PromptToContinue(ref ConsoleKey key)
        {
            Console.WriteLine($"Continue? (press {ConsoleKey.Q} to quit, or any other key to continue)");
            key = Console.ReadKey().Key;
            return key != ConsoleKey.Q;
        }
        #endregion

        #region Turns
        /// <summary>
        /// Prompt the Player to hit or stay
        /// </summary>
        /// <returns>If the Player has gone over 21 and busted.</returns>
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

        /// <summary>
        /// Have the dealer hit or stay, depending on its <see cref="Deck"/>'s total value.
        /// </summary>
        private void DealerTurn()
        {
            Random random = new();

            while (true)
            {
                int handValue = CountValues(dealerHand);

                // Dealer stays if the hand value is 21 or more
                if (handValue >= 21)
                {
                    break;
                }

                // Compute the hit probability using a linear scale
                int hitProbability = Math.Max(0, 100 - (handValue * 5)); // 100% at 0, decreasing linearly to 0% at 20

                if (random.Next(0, 100) < hitProbability)
                {
                    deck.TransferTopCard(dealerHand);
                }
                else
                {
                    break;
                }
            }
        }
        #endregion

        #region Gameplay Functions
        /// <summary>
        /// Start each <see cref="Deck"/> with the 2 starting <see cref="Card"/>s.
        /// </summary>
        /// <param name="hand">The <see cref="List{Card}"/> to deal into.</param>
        private void DealInitialCards(List<Card> hand)
        {
            deck.TransferTopCard(hand);
            deck.TransferTopCard(hand);
        }

        /// <summary>
        /// Figures out won the hand.
        /// </summary>
        /// <param name="playerValue">The value of the Player's <see cref="List{Card}"/>.</param>
        /// <param name="dealerValue">The value of the Dealer's <see cref="List{Card}"/>.</param>
        /// <param name="bet">How much the Player has bet.</param>
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

        /// <summary>
        /// Determines if the given <see cref="List{Card}"/> has a <see cref="Card"/> that has a value of <see cref="Values.Ace"/>.
        /// This is used to show the 2 possible values of a hand where an <see cref="Values.Ace"/> is counted as a 1 or 11.
        /// </summary>
        /// <param name="hand"></param>
        /// <returns>If the <see cref="List{Card}"/> has an <see cref="Card"/> with a value of <see cref="Values.Ace"/></returns>
        private static bool HasAce(List<Card> hand) => hand.Any(card => card.Value == Values.Ace);

        /// <summary>
        /// Determines if the hand goes over 21 (bust).
        /// </summary>
        /// <param name="hand"></param>
        /// <returns>The value of the <see cref="Card"/>s in the <see cref="List{Card}"/>.</returns>
        private static byte CountValues(List<Card> hand)
        {
            byte totalValue = 0;
            foreach (Card card in hand)
            {
                totalValue += (card.Value >= Values.Jack) ? (byte)10 : (byte)card.Value;
            }
            return totalValue;
        }
        #endregion
    }
}
