using System;
using System.Collections.Generic;

namespace Casino
{
    /// <summary>
    /// The <see cref="War"/> game <see cref="object"/>.
    /// </summary>
    internal class War
    {
        /// <summary>
        /// The Deck the cards will be dealt from.
        /// </summary>
        private Deck StartingDeck;
        /// <summary>
        /// The Player's hand.
        /// </summary>
        private Deck Player;
        /// <summary>
        /// The CPU's hand.
        /// </summary>
        private Deck CPU;

        /// <summary>
        /// How many round have been played.
        /// </summary>
        private byte rounds = 0;

        /// <summary>
        /// How much money you started with.
        /// </summary>
        private readonly int startingMoney;
        /// <summary>
        /// How much money you currently have.
        /// </summary>
        private int money;

        /// <summary>
        /// Initializes a game <see cref="War"/>.
        /// </summary>
        /// <param name="money">The starting money.</param>
        public War(int money)
        {
            StartingDeck = Deck.CreateFullDeck();

            StartingDeck.Shuffle();
            StartingDeck.Split(out Player, out CPU);

            this.money = money;
            startingMoney = money;
        }

        /// <summary>
        /// Plays the game of <see cref="War"/>.
        /// </summary>
        /// <returns>The money left at the end of the rounds.</returns>
        public int Play()
        {
            int bet;
            do { Console.Write("How much do you want to bet?: "); }
            while (!int.TryParse(Console.ReadLine(), out bet) && bet <= money);
            Console.WriteLine("Press ENTER to play a card");
            ConsoleKey key;
            while (Player.Count > 0 && CPU.Count > 0)
            {
                Console.WriteLine($"Player Cards: {Player.Count}\tCPU Cards: {CPU.Count}");
                key = Console.ReadKey().Key;
                if (key == ConsoleKey.Enter)
                {
                    PlayCards(Player[0], CPU[0]);
                }
                rounds++;
            }
            if (Player.Count > 0)
            {
                money += bet;
            }
            else
            {
                money -= bet;
            }
            Console.WriteLine($"You played {rounds} rounds, and came in with ${startingMoney}, you now have ${money}, resulting in a net of ${money - startingMoney}.");
            return money;
        }

        #region Gameplay Function
        /// <summary>
        /// Compares 2 <see cref="Card"/>s to see who wins.
        /// </summary>
        /// <param name="player">The Player's <see cref="Card"/>.</param>
        /// <param name="cpu">The CPU's <see cref="Card"/>.</param>
        private void CompareCards(Card player, Card cpu)
        {
            if (player.Value > cpu.Value)
            {
                Console.WriteLine("Player Wins!");
                Player.DiscardCards(StartingDeck);
            }
            else if (player.Value < cpu.Value)
            {
                Console.WriteLine("CPU Wins!");
                CPU.DiscardCards(StartingDeck);
            }
            else
            {
                BreakTie();
            }
        }

        /// <summary>
        /// Initiates a "War" between the players, where they both play 3 <see cref="Card"/>s and play a 4th <see cref="Card"/> to try and break the tie again.
        /// </summary>
        private void BreakTie()
        {
            Console.WriteLine("Tie!");

            if (Player.Count < 4 || CPU.Count < 4)
            {
                if (Player.Count < 4)
                {
                    Console.WriteLine("Player doesn't have enough cards to break the tie.  CPU Wins!");
                    CPU.DiscardCards(Player);
                    CPU.DiscardCards(StartingDeck);
                }
                if (CPU.Count < 4)
                {
                    Console.WriteLine("CPU doesn't have enough cards to break the tie.  Player Wins!");
                    Player.DiscardCards(CPU);
                    Player.DiscardCards(StartingDeck);
                }
            }
            else
            {
                for (int i = 0; i < 3; i++)
                {
                    Player[0].DisplayCardWithColor();
                    Console.Write("\t\t\t\t");
                    CPU[0].DisplayCardWithColor();
                    Console.WriteLine();
                    Player.TransferTopCard(StartingDeck);
                    CPU.TransferTopCard(StartingDeck);
                }
                PlayCards(Player[0], CPU[0]);
            }
        }

        /// <summary>
        /// Plays the next 2 <see cref="Card"/>s and determines the winner.
        /// </summary>
        /// <param name="player">The Player's <see cref="Card"/>.</param>
        /// <param name="cpu">The CPU's <see cref="Card"/>.</param>
        private void PlayCards(Card player, Card cpu)
        {
            player.DisplayCardWithColor();
            Console.Write("\t\tVS\t\t");
            cpu.DisplayCardWithColor();
            Console.WriteLine();

            Player.TransferTopCard(StartingDeck);
            CPU.TransferTopCard(StartingDeck);

            CompareCards(player, cpu);
        }
        #endregion
    }
}
