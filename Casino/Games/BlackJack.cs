using Casino.Objects;
using System;
using System.Collections.Generic;
using System.Linq;
using static Casino.Objects.Card;

namespace Casino.Games
{
    /// <summary>
    /// The <see cref="BlackJack"/> game object.
    /// </summary>
    /// <param name="money">Home much money was brought in.</param>
    public class BlackJack(int money) : CardGame(money)
    {
        /// <summary>
        /// The Player's hand.
        /// </summary>
        private List<Card> PlayerHand = [];
        /// <summary>
        /// The Dealer's hand (only one <see cref="Card"/> is visible).
        /// </summary>
        private List<Card> DealerHand = [];

        /// <inheritdoc/>
        public override int CustomGameFlow(int bet)
        {
            base.CustomGameFlow(bet);

            DisplayGame();

            int playerValue = CountValues(PlayerHand);
            int dealerValue = CountValues(DealerHand);
            while (!PlayerTurn() && !DealerTurn())
            {
                playerValue = CountValues(PlayerHand);
                dealerValue = CountValues(DealerHand);
                DisplayGame();
            }
            MainDeck.DiscardCards(PlayerHand);
            MainDeck.DiscardCards(DealerHand);

            return DetermineWinner(playerValue, dealerValue, bet);
        }

        /// <inheritdoc/>
        public override void Initialize()
        {
            base.Initialize();
            DealInitialCards(PlayerHand);
            DealInitialCards(DealerHand);
        }

        #region Showing Functions
        /// <inheritdoc/>
        public override void DisplayGame()
        {
            Console.WriteLine();
            Console.WriteLine("Current Hands:");
            ShowHand(PlayerHand, false);
            HideDealerCards(DealerHand);
            ShowHand(DealerHand, true);
        }

        /// <inheritdoc/>
        public override void ShowRules()
        {
            Console.WriteLine("RULES OF BLACK JACK:");
            Console.WriteLine("The goal is to get to 21, or as close as possible.");
            Console.WriteLine("If you go over 21, you automatically lose.");
            Console.WriteLine("All face cards count as 10 and Aces can count as either 1 or 11");
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

        #region Turns
        /// <summary>
        /// Prompt the Player to hit or stay
        /// </summary>
        /// <returns>If the Player has gone over 21 and busted or if they stayed.</returns>
        private bool PlayerTurn()
        {
            int handValue = CountValues(PlayerHand);
            if (handValue < 21)
            {
                ConsoleKey key;
                key = Console.ReadKey().Key;
                if (key == ConsoleKey.Spacebar)
                {
                    MainDeck.TransferTopCard(PlayerHand);
                    DisplayGame();
                }
                else if (key == ConsoleKey.Enter)
                {
                    return false;
                }
            }
            handValue = CountValues(PlayerHand);
            return handValue > 21;
        }

        /// <summary>
        /// Have the dealer hit or stay, depending on its <see cref="Deck"/>'s total value.
        /// </summary>
        /// <returns>If the Dealer has gone over 21 and busted or if they stayed.</returns>
        private bool DealerTurn()
        {
            Random random = new();

            int handValue = CountValues(DealerHand);

            if (handValue < 21)
            {
                // Compute the hit probability using a linear scale
                int hitProbability = Math.Max(0, 100 - (handValue * 5)); // 100% at 0, decreasing linearly to 0% at 20

                if (random.Next(0, 100) < hitProbability)
                {
                    MainDeck.TransferTopCard(DealerHand);
                    DisplayGame();
                }
                else return false;
            }
            handValue = CountValues(DealerHand);
            return handValue > 21;
        }
        #endregion

        #region Gameplay Functions
        /// <summary>
        /// Start each <see cref="Deck"/> with the 2 starting <see cref="Card"/>s.
        /// </summary>
        /// <param name="hand">The <see cref="List{Card}"/> to deal into.</param>
        private void DealInitialCards(List<Card> hand)
        {
            MainDeck.TransferTopCard(hand);
            MainDeck.TransferTopCard(hand);
        }

        /// <summary>
        /// Figures out won the hand.
        /// </summary>
        /// <param name="playerValue">The value of the Player's <see cref="List{Card}"/>.</param>
        /// <param name="dealerValue">The value of the Dealer's <see cref="List{Card}"/>.</param>
        /// <param name="bet">How much the Player has bet.</param>
        /// <returns>If someone has lost the game</returns>
        private static int DetermineWinner(int playerValue, int dealerValue, int bet)
        {
            if (dealerValue > playerValue)
            {
                Console.WriteLine($"Dealer wins with {dealerValue}!  You lose ${bet}.");
                return +bet;
            }
            else if (playerValue > dealerValue)
            {
                Console.WriteLine($"You win with{playerValue}!  You win {bet}.");
                return -bet;
            }
            else
            {
                Console.WriteLine($"Tie!");
                return 0;
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
                totalValue += card.Value >= Values.Jack ? (byte)10 : (byte)card.Value;
            }
            return totalValue;
        }
        #endregion
    }
}
