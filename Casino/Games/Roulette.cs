using System;
using System.Collections.Generic;
using System.Threading;

namespace Casino.Games
{
    public class Roulette(int money) : Game(money)
    {
        /// <summary>
        /// The bets on indices, with their values.
        /// </summary>
        private readonly Dictionary<int, int> Bets = [];

        /// <summary>
        /// Plays the game of <see cref="Roulette"/>.
        /// </summary>
        /// <returns>The money left at the end of the rounds.</returns>
        public override int CustomGameFlow(int bet)
        {
            CreateBets();
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

        #region Betting
        /// <summary>
        /// Sets up bets, until the quit key is pressed.
        /// </summary>
        private void CreateBets()
        {
            Bets.Clear();
            ConsoleKey key;
            do
            {
                AddBet();
                ShowBets();
                Console.WriteLine("Press Enter to finish betting, D to delete a bet, E to edit a bet, or any other key to continue betting");
                key = Console.ReadKey().Key;
                ProcessBetEdit(key);
                Console.Clear();
            } while (key != ConsoleKey.Enter);
        }

        /// <summary>
        /// Adds a new bet to <see cref="Bets"/>, with the a value and bet amount.
        /// </summary>
        private void AddBet()
        {
            Console.WriteLine("Enter number to bet on: ");
            if (int.TryParse(Console.ReadLine(), out int number) && number >= 0 && number <= 36)
            {
                Console.WriteLine("Enter amount to bet: ");
                if (int.TryParse(Console.ReadLine(), out int value) && value > 0)
                {
                    Bets[number] = value;
                    Console.WriteLine($"Added bet on number {number} for ${value}");
                }
                else
                {
                    Console.WriteLine("Invalid bet amount. It must be greater than $0.");
                }
            }
            else
            {
                Console.WriteLine("Invalid number. It must be between 0 and 36.");
            }
        }

        /// <summary>
        /// Edits or deletes a selected bet.
        /// </summary>
        /// <param name="key">The <see cref="ConsoleKey"/> that was pressed.</param>
        private void ProcessBetEdit(ConsoleKey key)
        {
            switch (key)
            {
                case ConsoleKey.D:
                    Console.WriteLine("Enter number to delete the bet of: ");
                    if (int.TryParse(Console.ReadLine(), out int deleteNumber))
                    {
                        Bets.Remove(deleteNumber);
                        Console.WriteLine($"Removed bet on number {deleteNumber}");
                    }
                    else
                    {
                        Console.WriteLine("Invalid number. It must be a number you have bet on.");
                    }
                    break;
                case ConsoleKey.E:
                    Console.WriteLine("Enter number to edit the bet of: ");
                    if (int.TryParse(Console.ReadLine(), out int editNumber))
                    {
                        Console.WriteLine("Enter amount to edit the bet to: ");
                        if (int.TryParse(Console.ReadLine(), out int editValue))
                        {
                            Bets[editNumber] = editValue;
                            Console.WriteLine($"Edited bet on number {editNumber} to ${editValue}");
                        }
                        else
                        {
                            Console.WriteLine("Invalid bet amount. It must be greater than $0.");
                        }
                    }
                    else
                    {
                        Console.WriteLine("Invalid number. It must be a number you have bet on.");
                    }
                    break;
            }
        }

        /// <summary>
        /// Displays all active bets in <see cref="Bets"/>.
        /// </summary>
        private void ShowBets()
        {
            Console.WriteLine("Bets:");
            Console.WriteLine("Number\t\tValue");
            foreach (int number in Bets.Keys)
            {
                Console.BackgroundColor = ValueToColor(number);
                Console.Write($"{number:00}");
                Console.ResetColor();
                Console.Write("\t\t$" + Bets[number]);
                Console.WriteLine();
            }
        }

        /// <summary>
        /// Return the money from all bets.
        /// </summary>
        /// <param name="number"></param>
        /// <returns></returns>
        private int PayOutBets(int number)
        {
            int earnings = 0;
            foreach (KeyValuePair<int, int> kvp in Bets)
            {
                if (kvp.Key == number)
                {
                    earnings += kvp.Value;
                    Console.WriteLine($"You won ${kvp.Value} on {kvp.Key}!");
                }
                else
                {
                    earnings -= kvp.Value;
                    Console.WriteLine($"You lost ${kvp.Value} on {kvp.Key}!");
                }
            }
            Bets.Clear();
            return earnings;
        }
        #endregion

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
