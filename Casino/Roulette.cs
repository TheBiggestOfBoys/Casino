using System;
using System.Collections.Generic;
using System.Threading;

namespace Casino
{
    internal class Roulette(int money)
    {
        private readonly Dictionary<int, int> Bets = [];

        private byte rounds = 0;
        private readonly int startingMoney = money;
        private int money = money;

        public int Play()
        {
            Console.WriteLine("Press Q to exit");

            ConsoleKey key;
            do
            {
                CreateBets();
                Console.WriteLine("Press Enter to spin: ");
                DisplayWheel(null);
                HighlightRandom();
                int number = Spin();
                Console.Clear();
                DisplayWheel(number);

                rounds++;
                Console.WriteLine("Continue? (press Q to quit, or any other key to continue)");
                key = Console.ReadKey().Key;
            } while (key != ConsoleKey.Q);

            Console.WriteLine($"You played {rounds} rounds, and started with ${startingMoney}. You now have ${money}, resulting in a net of ${money - startingMoney}.");
            return money;
        }

        private void CreateBets()
        {
            Bets.Clear();
            ConsoleKey key;
            do
            {
                AddBet(); ShowBets();
                Console.WriteLine("Press Enter to finish betting, D to delete a bet, E to edit a bet, or any other key to continue betting");
                key = Console.ReadKey().Key;
                ProcessBetEdit(key);
                Console.Clear();
            } while (key != ConsoleKey.Enter);
        }

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

        private int Spin()
        {
            int number = new Random().Next(37);
            PayOutBets(number);
            return number;
        }

        private void PayOutBets(int number)
        {
            foreach (KeyValuePair<int, int> kvp in Bets)
            {
                if (kvp.Key == number)
                {
                    money += kvp.Value;
                    Console.WriteLine($"You won ${kvp.Value} on {kvp.Key}!");
                }
                else
                {
                    money -= kvp.Value;
                    Console.WriteLine($"You lost ${kvp.Value} on {kvp.Key}!");
                }
            }
            Bets.Clear();
        }


        private static ConsoleColor ValueToColor(int value) => value == 0 ? ConsoleColor.Green : value % 2 == 0 ? ConsoleColor.DarkGray : ConsoleColor.Red;

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

        private static void DisplayWheel(int? highlightNumber)
        {
            for (int i = 0; i < 37; i++)
            {
                Console.BackgroundColor = (i == highlightNumber) ? ConsoleColor.Yellow : ValueToColor(i);
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
    }
}
