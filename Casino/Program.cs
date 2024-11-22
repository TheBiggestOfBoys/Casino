using System;
using System.Text;

namespace Casino
{
    internal class Program
    {
        static void Main()
        {
            int money = 100;
            ConsoleKey key = ConsoleKey.Escape;

            Console.OutputEncoding = Encoding.UTF8;

            while (key != ConsoleKey.Q)
            {
                Console.WriteLine("Welcome to the casino!");
                Console.WriteLine($"You have ${money}");

                Console.WriteLine("1.\tBlack Jack");
                Console.WriteLine("2.\tWar");
                Console.WriteLine("3.\tRoulette");
                Console.Write("What game do you want to play?: ");

                key = Console.ReadKey().Key;
                Console.WriteLine();

                switch (key)
                {
                    case ConsoleKey.D1 or ConsoleKey.NumPad1:
                        BlackJack blackJack = new(money);
                        money = blackJack.Play();
                        break;
                    case ConsoleKey.D2 or ConsoleKey.NumPad2:
                        War war = new(money);
                        money = war.Play();
                        break;
                    case ConsoleKey.D3 or ConsoleKey.NumPad3:
                        Roulette roulette = new(money);
                        money = roulette.Play();
                        break;
                }
            }

            Console.WriteLine($"You left the Casino with ${money}.");
        }
    }
}
