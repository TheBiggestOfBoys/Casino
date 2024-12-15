using System;
using System.Threading;

namespace Casino.Games
{
    public class Roulette(int money) : MultipleBets(money)
    {
        /// <inheritdoc/>
        public override int CustomGameFlow(int bet)
        {
            base.CustomGameFlow(bet);

            Console.WriteLine("Press Enter to spin: ");
            DisplayWheel(null);
            HighlightRandom();
            int number = Spin();
            Console.Clear();
            DisplayWheel(number);
            return PayOutBets(number);
        }

        /// <summary>
        /// Spins the wheel.
        /// </summary>
        /// <returns>A random index on the wheel.</returns>
        private int Spin()
        {
            int number = new Random().Next(37);
            PayOutBets(number);
            return number;
        }

        #region Color Wheel
        /// <summary>
        /// Get the <see cref="ConsoleColor"/> of the number.
        /// </summary>
        /// <param name="value">The number the determine.</param>
        /// <returns><see cref="ConsoleColor.Red"/> if even, <see cref="ConsoleColor.DarkGray"/> if odd, and <see cref="ConsoleColor.Green"/> if 0.</returns>
        private static ConsoleColor ValueToColor(int value) => value == 0 ? ConsoleColor.Green : value % 2 == 0 ? ConsoleColor.DarkGray : ConsoleColor.Red;

        /// <summary>
        /// Highlights 9 random indices if the wheel, and highlights them on the wheel before arriving at the final index.
        /// </summary>
        private static void HighlightRandom()
        {
            int[] randomIndices = new int[9];
            Random random = new();

            for (int i = 0; i < randomIndices.Length; i++)
            {
                randomIndices[i] = random.Next(37);
            }

            foreach (int index in randomIndices)
            {
                Console.Clear();
                DisplayWheel(index);
                Thread.Sleep(150);
            }
        }

        /// <summary>
        /// Displays the wheel with the <see cref="ConsoleColor"/>s.
        /// </summary>
        /// <param name="highlightNumber">The index to highlight.</param>
        private static void DisplayWheel(int? highlightNumber)
        {
            for (int i = 0; i < 37; i++)
            {
                Console.BackgroundColor = i == highlightNumber ? ConsoleColor.Yellow : ValueToColor(i);
                Console.Write($"{i:00}");
                if (i == 18)
                {
                    Console.ResetColor();
                    Console.WriteLine();
                }
            }
            Console.ResetColor();
            Console.WriteLine();
        }
        #endregion
    }
}
