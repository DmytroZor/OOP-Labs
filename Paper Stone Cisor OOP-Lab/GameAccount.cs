using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Paper_Stone_Cisor_OOP_Lab
{
    public abstract class GameAccount
    {
        public string PlayerName { get; set; }
        public int Rating { get; set; }
        public int GamesCount { get; private set; }
        public int WinCount { get; private set; }
        public int LoseCount { get; private set; }
        public int DrawCount { get; private set; }

        public GameAccount(string playerName, int rating)
        {
            if (rating < 1)
                throw new ArgumentException("Rating cannot be less than 1.");
            PlayerName = playerName;
            Rating = rating;
        }

        public void PlayGame(GameAccount opponent)
        {
            GamesCount++;
            opponent.GamesCount++;

            int playerChoice = GetPlayerChoice();
            if (playerChoice == -1)  // Check if player chose to quit
            {
                Console.WriteLine($"{PlayerName} has quit the game.");
                return;
            }

            int opponentChoice = new Random().Next(1, 4);

            Console.WriteLine($"{PlayerName} chose {GetMoveName(playerChoice)}");
            Console.WriteLine($"{opponent.PlayerName} chose {GetMoveName(opponentChoice)}");

            int result = DetermineWinner(playerChoice, opponentChoice);
            ProcessGameResult(opponent, result);
        }


        protected virtual int GetPlayerChoice()
        {
            Console.WriteLine($"{PlayerName}, choose your move:");
            Console.WriteLine("1. Rock\n2. Paper\n3. Scissors\n0. Quit");
            int choice;
            while (!int.TryParse(Console.ReadLine(), out choice) || choice < 0 || choice > 3)
            {
                Console.WriteLine("Invalid input. Please enter 0, 1, 2, or 3.");
            }
            if (choice == 0) return -1;  // Quit option
            return choice;
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
            if (playerChoice == opponentChoice) return 0;
            if ((playerChoice == 1 && opponentChoice == 3) || (playerChoice == 2 && opponentChoice == 1) || (playerChoice == 3 && opponentChoice == 2)) return 1;
            return -1;
        }

        private void ProcessGameResult(GameAccount opponent, int result)
        {
            switch (result)
            {
                case 0: // Draw
                    Console.WriteLine("It's a draw!");
                    DrawCount++;
                    opponent.DrawCount++;
                    break;
                case 1: // Current player wins
                    Console.WriteLine($"{PlayerName} wins!");
                    WinCount++;
                    Rating += 10;  // Increase rating by 10
                    opponent.LoseCount++;
                    opponent.Rating -= 10;  // Decrease opponent's rating by 10
                    break;
                case -1: // Opponent wins
                    Console.WriteLine($"{opponent.PlayerName} wins!");
                    LoseCount++;
                    Rating -= 10;  // Decrease rating by 10
                    opponent.WinCount++;
                    opponent.Rating += 10;  // Increase opponent's rating by 10
                    break;
            }
        }


        public void GetStats()
        {
            Console.WriteLine($"{PlayerName}'s Stats:");
            Console.WriteLine($"Games Played: {GamesCount}, Wins: {WinCount}, Losses: {LoseCount}, Draws: {DrawCount}, Rating: {Rating}");
        }
    }
}
