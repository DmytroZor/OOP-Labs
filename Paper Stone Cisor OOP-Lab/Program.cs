using System;

abstract class Game
{
    public abstract int CalculatePoints(int result);
}

class StandardGame : Game
{
    public override int CalculatePoints(int result)
    {
        return result * 10; // Стандартний розрахунок балів
    }
}

class TrainingGame : Game
{
    public override int CalculatePoints(int result)
    {
        return 0; // Нульовий розрахунок балів, оскільки тренувальний режим
    }
}

abstract class GameAccount
{
    public string PlayerName { get; set; }
    public int Rating { get; set; }
    public int GamesCount { get; private set; }
    public int WinCount { get; private set; }
    public int LoseCount { get; private set; }
    public int DrawCount { get; private set; }
    protected Game selectedGame;

    public GameAccount(string playerName, int rating, Game selectedGame)
    {
        if (rating < 1)
            throw new ArgumentException("Rating cannot be less than 1.");
        PlayerName = playerName;
        Rating = rating;
        this.selectedGame = selectedGame;
    }

    public void PlayGame(GameAccount opponent)
    {
        if (opponent.Rating < 1)
            throw new ArgumentException("Opponent's rating cannot be less than 1.");

        Console.WriteLine("Welcome to Rock, Paper, Scissors!");
        Console.WriteLine($"Enter your move, {PlayerName}:");

        while (true)
        {
            GamesCount++;
            Random random = new Random();
            int playerChoice;
            int opponentChoice = random.Next(1, 4); // 1 for Rock, 2 for Paper, 3 for Scissors

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

            string playerMove = GetMoveName(playerChoice);
            string opponentMove = GetMoveName(opponentChoice);

            Console.WriteLine($"{PlayerName} chose {playerMove}");
            Console.WriteLine($"{opponent.PlayerName} chose {opponentMove}");

            int result = DetermineWinner(playerChoice, opponentChoice);

            if (result == 0)
            {
                Console.WriteLine("It's a draw!");
                DrawCount++;
                opponent.DrawCount++;
            }
            else if (result == 1)
            {
                Console.WriteLine($"{PlayerName} wins the game!");
                WinCount++;
                opponent.LoseCount++;
                Rating += CalculatePoints(1);
                opponent.Rating += CalculatePoints(-1);
            }
            else
            {
                Console.WriteLine($"{opponent.PlayerName} wins the game!");
                LoseCount++;
                opponent.WinCount++;
                Rating += CalculatePoints(-1);
                opponent.Rating += CalculatePoints(1);
            }
        }

        opponent.GamesCount = GamesCount;
    }
    //Нужно для того что бы возвращать очки независимо от того какой типо аккаунт выбирает игрок(не забыть!!!)
    protected virtual int CalculatePoints(int result)
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
    //проверка для победы 
    private int DetermineWinner(int playerChoice, int opponentChoice)
    {
        if (playerChoice == opponentChoice)
        {
            return 0; // нічия
        }

        if ((playerChoice == 1 && opponentChoice == 3) || (playerChoice == 2 && opponentChoice == 1) || (playerChoice == 3 && opponentChoice == 2))
        {
            return 1; // перемога
        }

        return -1; // поразка
    }
    //Вивід статистики
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
//різні види акаутів //base для визова конструктора базового класса(не забыть!!!)
class StandardGameAccount : GameAccount //дефолт як і просто якщо б нічого не було
{
    public StandardGameAccount(string playerName, int rating) : base(playerName, rating, new StandardGame()) { }
}

class TrainingGameAccount : GameAccount //тренувальна гра(рейтинг не змінюється) працює через класс game і його нащадків
{
    public TrainingGameAccount(string playerName, int rating) : base(playerName, rating, new TrainingGame()) { }
}

class Program
{
    static void Main(string[] args)
    {
        Console.WriteLine("Welcome to Rock, Paper, Scissors!");
        Console.WriteLine("Enter your name:");

        string playerName = Console.ReadLine();
        int startingRating = 100;
        GameAccount player1 = null;
        GameAccount player2 = new StandardGameAccount("AI Opponent", startingRating);

        Console.WriteLine("Choose your account type:");
        Console.WriteLine("1. Standard Account (win (+ or -) 10 rating)");
        Console.WriteLine("2. Training Account (no rating change)");

        while (true)
        {
            if (int.TryParse(Console.ReadLine(), out int accountType) && (accountType == 1 || accountType == 2)) //трай парс для конвертации нецелых чисел //out -передча за посиланням(не забыть!!!)
            {
                switch (accountType)
                {
                    case 1:
                        player1 = new StandardGameAccount(playerName, startingRating);
                        break;
                    case 2:
                        player1 = new TrainingGameAccount(playerName, startingRating);
                        break;
                }
                break;
            }
            Console.WriteLine("Invalid input. Please enter 1 or 2.");
        }

        Console.WriteLine($"You and AI Opponent both start with a rating of {startingRating}.");

        while (true)
        {
            player1.PlayGame(player2);

            player1.GetStats();
            player2.GetStats();

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
