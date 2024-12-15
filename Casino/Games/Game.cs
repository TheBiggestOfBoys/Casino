using System;

namespace Casino.Games
{
    public abstract class Game(int money)
    {
        /// <summary>
        /// How many round have been played.
        /// </summary>
        public byte Rounds = 0;

        /// <summary>
        /// How much money you started with.
        /// </summary>
        public readonly int StartingMoney = money;
        /// <summary>
        /// How much money you currently have.
        /// </summary>
        public int Money = money;

        /// <summary>
        /// Plays the <see cref="Game"/>.
        /// </summary>
        /// <returns>The <see cref="Money"/> left at the end of the <see cref="Rounds"/>.</returns>
        public int Play()
        {
            ShowRules();
            ConsoleKey key = ConsoleKey.Escape;
            Console.WriteLine("Press Q to exit");
            while (key != ConsoleKey.Q)
            {
                int bet = PromptForBet();
                int moneyEarned = CustomGameFlow(bet);
                Money += moneyEarned;
                Rounds++;
                PromptToContinue(ref key);
            }
            ShowFinalResults();
            return Money;
        }

        #region Prompts
        /// <summary>
        /// Asks to play another round or to quit.
        /// </summary>
        /// <param name="key">The <see cref="ConsoleKey"/> pressed</param>
        /// <returns>If the quit key (<see cref="ConsoleKey.Q"/>) was pressed.</returns>
        private static bool PromptToContinue(ref ConsoleKey key)
        {
            Console.WriteLine($"Continue? (press {ConsoleKey.Q} to quit, or any other key to continue)");
            key = Console.ReadKey().Key;
            return key != ConsoleKey.Q;
        }

        /// <summary>
        /// Asks how much to bet.
        /// </summary>
        /// <param name="bet">How much to bet</param>
        /// <returns>If the entered bet was valid.</returns>
        private int PromptForBet()
        {
            int bet;
            do { Console.Write("How much do you want to bet?: "); }
            while (!int.TryParse(Console.ReadLine(), out bet) && bet <= Money);
            return bet;
        }
        #endregion

        /// <summary>
        /// The logic for initializing the <see cref="CardGame"/>.
        /// </summary>
        public virtual void Initialize() { }

        /// <summary>
        /// Shows the current <see cref="Game"/>'s state.
        /// </summary>
        public virtual void DisplayGame() { }

        /// <summary>
        /// Contains the <see cref="Game"/>'s logic, and if the <see cref="Game"/> should keep going.
        /// </summary>
        /// <returns>The money won/lost</returns>
        public virtual int CustomGameFlow(int bet)
        {
            Initialize();
            return 0;
        }

        /// <summary>
        /// Shows the rules of Black Jack.
        /// </summary>
        public virtual void ShowRules() { }

        /// <summary>
        /// Shows the rounds played and profit gained/lost from all the games
        /// </summary>
        public void ShowFinalResults()
        {
            Console.WriteLine($"You played {Rounds} rounds, and came in with ${StartingMoney}, you now have ${Money}, resulting in a net of ${Money - StartingMoney}.");
        }
    }
}
