using System;
using System.Collections.Generic;
using System.Linq;

// Абстрактний клас GameAccount
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
            case 0:
                Console.WriteLine("It's a draw!");
                DrawCount++;
                opponent.DrawCount++;
                break;
            case 1:
                Console.WriteLine($"{PlayerName} wins!");
                WinCount++;
                opponent.LoseCount++;
                break;
            case -1:
                Console.WriteLine($"{opponent.PlayerName} wins!");
                LoseCount++;
                opponent.WinCount++;
                break;
        }
    }

    public void GetStats()
    {
        Console.WriteLine($"{PlayerName}'s Stats:");
        Console.WriteLine($"Games Played: {GamesCount}, Wins: {WinCount}, Losses: {LoseCount}, Draws: {DrawCount}, Rating: {Rating}");
    }
}

class StandardGameAccount : GameAccount
{
    public StandardGameAccount(string playerName, int rating) : base(playerName, rating) { }
}

class TrainingGameAccount : GameAccount
{
    public TrainingGameAccount(string playerName, int rating) : base(playerName, rating) { }
}

public interface IGameAccountRepository
{
    void CreateAccount(GameAccount account);
    GameAccount ReadAccount(string playerName);
    void UpdateAccount(GameAccount account);
    void DeleteAccount(string playerName);
}

class GameAccountRepository : IGameAccountRepository
{
    private readonly DbContext _dbContext;

    public GameAccountRepository(DbContext dbContext)
    {
        _dbContext = dbContext;
    }

    public void CreateAccount(GameAccount account)
    {
        _dbContext.GameAccounts.Add(account);
    }

    public GameAccount ReadAccount(string playerName)
    {
        return _dbContext.GameAccounts.FirstOrDefault(a => a.PlayerName == playerName);
    }

    public void UpdateAccount(GameAccount account)
    {
        var existingAccount = ReadAccount(account.PlayerName);
        if (existingAccount != null)
        {
            _dbContext.GameAccounts.Remove(existingAccount);
            _dbContext.GameAccounts.Add(account);
        }
    }

    public void DeleteAccount(string playerName)
    {
        var account = ReadAccount(playerName);
        if (account != null)
        {
            _dbContext.GameAccounts.Remove(account);
        }
    }
}

public interface IGameAccountService
{
    void CreateAccount(GameAccount account);
    GameAccount ReadAccount(string playerName);
    void UpdateAccount(GameAccount account);
    void DeleteAccount(string playerName);
}

class GameAccountService : IGameAccountService
{
    private readonly IGameAccountRepository _accountRepository;

    public GameAccountService(IGameAccountRepository accountRepository)
    {
        _accountRepository = accountRepository;
    }

    public void CreateAccount(GameAccount account)
    {
        _accountRepository.CreateAccount(account);
    }

    public GameAccount ReadAccount(string playerName)
    {
        return _accountRepository.ReadAccount(playerName);
    }

    public void UpdateAccount(GameAccount account)
    {
        _accountRepository.UpdateAccount(account);
    }

    public void DeleteAccount(string playerName)
    {
        _accountRepository.DeleteAccount(playerName);
    }
}

class DbContext
{
    public List<GameAccount> GameAccounts { get; private set; }

    public DbContext()
    {
        GameAccounts = new List<GameAccount>();
        InitializeData();
    }

    private void InitializeData()
    {
        GameAccounts.Add(new StandardGameAccount("AI_Opponent", 100));
    }
}

class Program
{
    static void Main(string[] args)
    {
        DbContext dbContext = new DbContext();
        IGameAccountRepository accountRepository = new GameAccountRepository(dbContext);
        IGameAccountService accountService = new GameAccountService(accountRepository);

        Console.WriteLine("Welcome to Rock, Paper, Scissors!");
        Console.WriteLine("Enter your name:");
        string playerName = Console.ReadLine();
        int startingRating = 100;

        GameAccount player = new StandardGameAccount(playerName, startingRating);
        accountService.CreateAccount(player);

        GameAccount opponent = accountService.ReadAccount("AI_Opponent");

        while (true)
        {
            player.PlayGame(opponent);
            player.GetStats();
            opponent.GetStats();

            Console.WriteLine("Do you want to play another round? (Y/N)");
            if (Console.ReadLine().Trim().ToUpper() != "Y")
            {
                Console.WriteLine("Thanks for playing!");
                break;
            }
        }
    }
}
