using Casino.Objects;
using System;

namespace Casino.Games
{
    /// <summary>
    /// The <see cref="War"/> game <see cref="object"/>.
    /// </summary>
    /// <param name="money">The starting money.</param>
    public class War(int money) : CardGame(money)
    {
        /// <summary>
        /// The Player's hand.
        /// </summary>
        private Deck Player = [];
        /// <summary>
        /// The CPU's hand.
        /// </summary>
        private Deck CPU = [];

        /// <summary>
        /// The amount of attacks done.
        /// </summary>
        private int Plays = 0;

        /// <inheritdoc/>
        public override int CustomGameFlow(int bet)
        {
            base.CustomGameFlow(bet);

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
                Plays++;
            }
            Player.DiscardCards(MainDeck);
            CPU.DiscardCards(MainDeck);

            Console.WriteLine($"Game ended with {Plays} plays.");
            Plays = 0;

            return Player.Count > 0 ? +bet : -bet;
        }

        /// <inheritdoc/>
        public override void Initialize()
        {
            base.Initialize();
            MainDeck.Split(out Player, out CPU);
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
                Player.DiscardCards(MainDeck);
            }
            else if (player.Value < cpu.Value)
            {
                Console.WriteLine("CPU Wins!");
                CPU.DiscardCards(MainDeck);
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
                    CPU.DiscardCards(MainDeck);
                }
                if (CPU.Count < 4)
                {
                    Console.WriteLine("CPU doesn't have enough cards to break the tie.  Player Wins!");
                    Player.DiscardCards(CPU);
                    Player.DiscardCards(MainDeck);
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
                    Player.TransferTopCard(MainDeck);
                    CPU.TransferTopCard(MainDeck);
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

            Player.TransferTopCard(MainDeck);
            CPU.TransferTopCard(MainDeck);

            CompareCards(player, cpu);
        }
        #endregion
    }
}
