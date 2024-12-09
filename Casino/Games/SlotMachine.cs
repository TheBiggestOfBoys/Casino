using System;
using System.Collections.Generic;
using System.Threading;

namespace Casino.Games
{
    public class SlotMachine(int money) : Game(money)
    {
        /// <summary>
        /// The symbols the wheel will spin on
        /// </summary>
        private static readonly string[] allSymbols = ["7", "🍌", "🍉", "🔔", "BAR", "🍋", "🍊", "🍇", "🍒", "♥️", "♦️", "♠️", "♣️", "🍀", "?", "WIN", "👑", "🌟", "🧲"];

        /// <summary>
        /// Plays the game of <see cref="SlotMachine"/>.
        /// </summary>
        /// <returns>The money left at the end of the rounds.</returns>
        public override int CustomGameFlow(int bet)
        {
            Console.WriteLine("Here are all possible symbols:");
            Console.WriteLine(string.Join(' ', allSymbols));
            Console.WriteLine("The more symbols you chose, the lower the bet will be, the higher the reward can be, bu lower chance of getting all matches.");

            int numberOfSymbols;
            do { Console.Write("How many symbols do you want to use: "); }
            while (!int.TryParse(Console.ReadLine(), out numberOfSymbols));

            Console.Clear();

            string[] symbols = MakeSymbolSet(numberOfSymbols);

            int maxWin = numberOfSymbols * 10 * bet;

            Console.WriteLine($"With {numberOfSymbols} symbols the maximum win will be: {maxWin}.");
            Console.WriteLine($"Winnings will be from $0 to {numberOfSymbols * 10}.");
            Console.Clear();

            Console.WriteLine("Round: " + Rounds);
            Console.WriteLine($"Press Enter to spin: ");
            Console.ReadKey();
            Console.Clear();

            int earnings = -bet;
            earnings += Spin(numberOfSymbols, symbols);
            return earnings;
        }

        /// <summary>
        /// Gets the results of a spin
        /// </summary>
        /// <param name="numberOfSymbols">How many symbols to chose from</param>
        /// <returns>The money won</returns>
        public static int Spin(int numberOfSymbols, string[] symbols)
        {
            for (int i = 0; i < 10; i++)
            {
                string[] tempResults = GetResults(numberOfSymbols, symbols);
                Console.WriteLine(string.Join(' ', tempResults));
                Thread.Sleep(100);
                Console.Clear();
            }

            int money = 0;
            string[] actualResults = GetResults(numberOfSymbols, symbols);
            Dictionary<string, int> matches = CountMatches(actualResults);

            money += matches.Count * 10;

            Console.WriteLine(string.Join(' ', actualResults));
            Console.WriteLine("Matching Pairs: " + matches.Keys.Count);
            foreach (KeyValuePair<string, int> pair in matches)
            {
                money += pair.Value * 5;
                Console.WriteLine($"{pair.Key}: {pair.Value}");
            }
            Console.WriteLine("Money gained: $" + money);

            return money;
        }

        /// <summary>
        /// Gets the random symbols in the <see cref="Spin(int)"/>
        /// </summary>
        /// <param name="number">How many symbols will be used</param>
        /// <param name="symbols">The symbols to chose from.</param>
        /// <returns>The randomly chosen symbols.</returns>
        private static string[] GetResults(int number, string[] symbols)
        {
            string[] symbolResults = new string[number];
            Random random = new();

            for (int i = 0; i < number; i++)
            {
                int index = random.Next(symbols.Length);
                symbolResults[i] = symbols[index];
            }
            return symbolResults;
        }

        /// <summary>
        /// Gets all matching pairs in the symbols.
        /// </summary>
        /// <param name="symbols">The <see cref="string"/> symbols from the spin</param>
        /// <returns></returns>
        private static Dictionary<string, int> CountMatches(string[] symbols)
        {
            Dictionary<string, int> matches = [];

            foreach (string symbol in symbols)
            {
                if (matches.TryGetValue(symbol, out int value))
                {
                    matches[symbol] = ++value;
                }
                else
                {
                    matches[symbol] = 1;
                }
            }

            // Remove single occurrence items
            List<string> keysToRemove = [];
            foreach (KeyValuePair<string, int> pair in matches)
            {
                if (pair.Value == 1)
                {
                    keysToRemove.Add(pair.Key);
                }
            }

            foreach (string key in keysToRemove)
            {
                matches.Remove(key);
            }

            return matches;
        }

        /// <summary>
        /// Takes a subset of <see cref="allSymbols"/>.
        /// </summary>
        /// <param name="number"></param>
        /// <returns>A <see cref="string[]"/> of the symbol subsets.</returns>
        private static string[] MakeSymbolSet(int number)
        {
            string[] symbols = new string[number];
            Array.Copy(allSymbols, symbols, number);
            return symbols;
        }
    }
}
