using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Paper_Stone_Cisor_OOP_Lab
{
    class GameAccount : IPlayable, IStats
    {
        public string PlayerName { get; set; }
        public int Rating { get; set; }
        public int GamesCount { get; private set; }
        public int WinCount { get; private set; }
        public int LoseCount { get; private set; }
        public int DrawCount { get; private set; }
        protected IGame selectedGame;

        public GameAccount(string playerName, int rating, IGame selectedGame)
        {
            if (rating < 1)
                throw new ArgumentException("Rating cannot be less than 1.");
            PlayerName = playerName;
            Rating = rating;
            this.selectedGame = selectedGame;
        }

        public void PlayGame(IPlayable opponent)
        {
            Console.WriteLine("Welcome to Rock, Paper, Scissors!");
            Console.WriteLine($"Enter your move, {PlayerName}:");

            while (true)
            {
                Random random = new Random();
                int playerChoice;
                int opponentChoice = random.Next(1, 4);

                Console.WriteLine("Choose your move:");
                Console.WriteLine("1. Rock");
                Console.WriteLine("2. Paper");
                Console.WriteLine("3. Scissors");
                Console.WriteLine("0. Quit");

                while (true)
                {
                    if (int.TryParse(Console.ReadLine(), out playerChoice) && (playerChoice >= 0 && playerChoice <= 3))
                    {
                        break;
                    }
                    Console.WriteLine("Invalid input. Please enter 0, 1, 2, or 3.");
                }

                if (playerChoice == 0)
                {
                    Console.WriteLine("You quit the game.");
                    break;
                }

                GamesCount++;

                string playerMove = GetMoveName(playerChoice);
                string opponentMove = GetMoveName(opponentChoice);

                Console.WriteLine($"{PlayerName} chose {playerMove}");
                Console.WriteLine($"{((GameAccount)opponent).PlayerName} chose {opponentMove}");

                int result = DetermineWinner(playerChoice, opponentChoice);

                if (result == 0)
                {
                    Console.WriteLine("It's a draw!");
                    DrawCount++;
                    ((GameAccount)opponent).DrawCount++;
                }
                else if (result == 1)
                {
                    Console.WriteLine($"{PlayerName} wins the game!");
                    WinCount++;
                    ((GameAccount)opponent).LoseCount++;
                    Rating += CalculatePoints(1);
                    ((GameAccount)opponent).Rating += CalculatePoints(-1);
                }
                else
                {
                    Console.WriteLine($"{((GameAccount)opponent).PlayerName} wins the game!");
                    LoseCount++;
                    ((GameAccount)opponent).WinCount++;
                    Rating += CalculatePoints(-1);
                    ((GameAccount)opponent).Rating += CalculatePoints(1);
                }
            }

            ((GameAccount)opponent).GamesCount = GamesCount;
        }

        private int CalculatePoints(int result)
        {
            return selectedGame.CalculatePoints(result);
        }

        private string GetMoveName(int move)
        {
            switch (move)
            {
                case 1:
                    return "Rock";
                case 2:
                    return "Paper";
                case 3:
                    return "Scissors";
                default:
                    return "Unknown";
            }
        }

        private int DetermineWinner(int playerChoice, int opponentChoice)
        {
            if (playerChoice == opponentChoice)
            {
                return 0;
            }

            if ((playerChoice == 1 && opponentChoice == 3) || (playerChoice == 2 && opponentChoice == 1) || (playerChoice == 3 && opponentChoice == 2))
            {
                return 1;
            }

            return -1;
        }

        public void GetStats()
        {
            Console.WriteLine($"{PlayerName}'s Stats:");
            Console.WriteLine($"Games Played: {GamesCount}");
            Console.WriteLine($"Games Won: {WinCount}");
            Console.WriteLine($"Games Lost: {LoseCount}");
            Console.WriteLine($"Games Drawn: {DrawCount}");
            Console.WriteLine($"Current Rating: {Rating}");
            Console.WriteLine();
        }
    }

}
