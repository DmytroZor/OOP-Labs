using System;

namespace Paper_Stone_Cisor_OOP_Lab
{
    class Program
    {
        static void Main(string[] args)
        {
            Console.WriteLine("Welcome to Rock, Paper, Scissors!");
            Console.WriteLine("Enter your name:");

            string playerName = Console.ReadLine();
            int startingRating = 100;
            IPlayable player1 = null;
            IPlayable player2 = new GameAccount("AI Opponent", startingRating, new StandardGame());

            Console.WriteLine("Choose your account type:");
            Console.WriteLine("1. Standard Account (win (+ or -) 10 rating)");
            Console.WriteLine("2. Training Account (no rating change)");

            while (true)
            {
                if (int.TryParse(Console.ReadLine(), out int accountType) && (accountType == 1 || accountType == 2))
                {
                    IGameFactory gameFactory = null;
                    if (accountType == 1)
                    {
                        gameFactory = new StandardGameFactory();
                    }
                    else
                    {
                        gameFactory = new TrainingGameFactory();
                    }

                    IGame selectedGame = gameFactory.CreateGame();
                    player1 = new GameAccount(playerName, startingRating, selectedGame);
                    break;
                }
                Console.WriteLine("Invalid input. Please enter 1 or 2.");
            }

            Console.WriteLine($"You and AI Opponent both start with a rating of {startingRating}.");

            while (true)
            {
                player1.PlayGame(player2);

                ((GameAccount)player1).GetStats();
                ((GameAccount)player2).GetStats();

                Console.WriteLine("Do you want to play another round? (Y/N)");
                string playAgain = Console.ReadLine();

                if (playAgain != "Y" && playAgain != "y")
                {
                    Console.WriteLine("Thanks for playing!");
                    break;
                }
            }
        }
    }
}
