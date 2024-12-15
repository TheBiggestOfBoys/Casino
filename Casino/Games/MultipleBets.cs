using System;
using System.Collections.Generic;

namespace Casino.Games
{
    public abstract class MultipleBets(int money) : Game(money)
    {
        /// <summary>
        /// The bets on indices, with their values.
        /// </summary>
        public Dictionary<int, int> Bets = [];

        #region Betting
        /// <summary>
        /// Sets up bets, until the quit key is pressed.
        /// </summary>
        public void CreateBets()
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
        public void AddBet()
        {
            Console.WriteLine("Enter number to bet on: ");
            if (int.TryParse(Console.ReadLine(), out int number) && number >= 0 && number <= 36)
            {
                Console.WriteLine("Enter amount to bet: ");
                if (int.TryParse(Console.ReadLine(), out int value) && value > 0)
                {
                    Bets[number] = value;
                    Console.WriteLine($"Added bet on number {number} for {value:C}");
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
        public void ProcessBetEdit(ConsoleKey key)
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
                            Console.WriteLine($"Edited bet on number {editNumber} to {editValue:C}");
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
        public virtual void ShowBets()
        {
            Console.WriteLine("Bets:");
            foreach (int number in Bets.Keys)
            {
                Console.WriteLine($"{number}\t\t{Bets[number]:C}");
            }
        }

        /// <summary>
        /// Return the money from all bets.
        /// </summary>
        /// <param name="number"></param>
        /// <returns></returns>
        public int PayOutBets(int number)
        {
            int earnings = 0;
            foreach (KeyValuePair<int, int> kvp in Bets)
            {
                if (kvp.Key == number)
                {
                    earnings += kvp.Value;
                    Console.WriteLine($"You won {kvp.Value:C} on {kvp.Key}!");
                }
                else
                {
                    earnings -= kvp.Value;
                    Console.WriteLine($"You lost {-kvp.Value:C} on {kvp.Key}!");
                }
            }
            Console.WriteLine("Press Any key to continue");
            Console.ReadLine();
            Console.Clear();
            Bets.Clear();
            return earnings;
        }
        #endregion

        /// <inheritdoc/>
        public override void Initialize()
        {
            CreateBets();
        }
    }
}
