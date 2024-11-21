using System;

namespace Casino
{
    internal class War
    {
        private Deck StartingDeck;
        private Deck Player;
        private Deck CPU;

        private byte rounds = 0;
        private int money;
        private readonly int startingMoney;

        public War(int money)
        {
            StartingDeck = Deck.CreateFullDeck();
            Player = [];
            CPU = [];

            StartingDeck.Shuffle();
            StartingDeck.Split(Player, CPU);

            this.money = money;
            startingMoney = money;
        }

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

        private void BreakTie()
        {
            Console.WriteLine("Tie!");

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
    }
}
