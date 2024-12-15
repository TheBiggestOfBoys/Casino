using System;
using System.Threading;

namespace Casino.Games
{
    public class HorseRacing(int money) : MultipleBets(money)
    {
        private int NumberOfHorses;
        private int WinningHorseNumber;

        private bool RaceOver = false;

        private Horse[] Horses = [];
        private class Horse
        {
            public int Progress;
            public bool Finished => Progress >= 10;

            public override string ToString() => new string('-', Progress) + "🏇" + (Finished ? "🏁" : string.Empty);
        }

        /// <inheritdoc/>
        public override int CustomGameFlow(int bet)
        {
            base.CustomGameFlow(bet);

            // Game Loop
            Random random = new();
            while (!RaceOver)
            {
                for (int index = 0; index < Horses.Length; index++)
                {
                    Horse horse = Horses[index];
                    if (!horse.Finished)
                    {
                        if (random.NextDouble() < 0.5)
                        {
                            horse.Progress++;
                        }
                    }
                    else
                    {
                        WinningHorseNumber = index;
                        RaceOver = true;
                        break;
                    }
                    Console.WriteLine(horse);
                }
                Thread.Sleep(250);
                Console.Clear();
            }
            Console.WriteLine("Horse #" + WinningHorseNumber + " won the race!");
            return PayOutBets(WinningHorseNumber);
        }

        /// <summary>
        /// Displays all active bets in <see cref="Bets"/>.
        /// </summary>
        public override void ShowBets()
        {
            Console.WriteLine("Bets:");
            Console.WriteLine("Number\t\tValue");
            foreach (int number in Bets.Keys)
            {
                Console.WriteLine(number + "\t$" + Bets[number]);
            }
        }

        /// <inheritdoc/>
        public override void Initialize()
        {
            do { Console.Write("How many horses do you want to race?: "); }
            while (!int.TryParse(Console.ReadLine(), out NumberOfHorses));

            Horses = new Horse[NumberOfHorses];
            for (int index = 0; index < Horses.Length; index++)
            {
                Horses[index] = new();
            }
            base.Initialize();
        }
    }
}
