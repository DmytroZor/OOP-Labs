using System;

public interface IGame
{
    int CalculatePoints(int result);
}

public interface IPlayable
{
    void PlayGame(IPlayable opponent);
}

public interface IStats
{
    void GetStats();
}

class StandardGame : IGame
{
    public int CalculatePoints(int result)
    {
        return result * 10;
    }
}

class TrainingGame : IGame
{
    public int CalculatePoints(int result)
    {
        return 0;
    }
}

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

            GamesCount++; // Перенесено в цей рядок, після перевірки виходу з гри

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
                if (accountType == 1)
                {
                    player1 = new GameAccount(playerName, startingRating, new StandardGame());
                }
                else
                {
                    player1 = new GameAccount(playerName, startingRating, new TrainingGame());
                }
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
