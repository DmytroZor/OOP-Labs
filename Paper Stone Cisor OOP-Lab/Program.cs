using System;
using System.Collections.Generic;
using System.Linq;

// Abstract class GameAccount
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
    List<GameAccount> GetAllAccounts();
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

    public List<GameAccount> GetAllAccounts()
    {
        return _dbContext.GameAccounts;
    }
}

public interface IGameAccountService
{
    void CreateAccount(GameAccount account);
    GameAccount ReadAccount(string playerName);
    void UpdateAccount(GameAccount account);
    void DeleteAccount(string playerName);
    List<GameAccount> GetAllAccounts();
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

    public List<GameAccount> GetAllAccounts()
    {
        return _accountRepository.GetAllAccounts();
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

public interface IGameCommand
{
    void Execute();
    void DisplayCommandInfo();
}

public class DisplayPlayersCommand : IGameCommand
{
    private readonly IGameAccountService _accountService;

    public DisplayPlayersCommand(IGameAccountService accountService)
    {
        _accountService = accountService;
    }

    public void Execute()
    {
        var players = _accountService.GetAllAccounts();
        foreach (var player in players)
        {
            Console.WriteLine($"Player: {player.PlayerName}, Rating: {player.Rating}");
        }
    }

    public void DisplayCommandInfo()
    {
        Console.WriteLine("Displays all players.");
    }
}

public class AddPlayerCommand : IGameCommand
{
    private readonly IGameAccountService _accountService;
    private readonly string _playerName;
    private readonly int _rating;

    public AddPlayerCommand(IGameAccountService accountService, string playerName, int rating)
    {
        _accountService = accountService;
        _playerName = playerName;
        _rating = rating;
    }

    public void Execute()
    {
        GameAccount newPlayer = new StandardGameAccount(_playerName, _rating);
        _accountService.CreateAccount(newPlayer);

        Console.WriteLine($"Player {_playerName} added successfully with rating {_rating}.");
    }

    public void DisplayCommandInfo()
    {
        Console.WriteLine("Adds a new player.");
    }
}


public class ShowPlayerStatsCommand : IGameCommand
{
    private readonly IGameAccountService _accountService;
    private readonly string _playerName;

    public ShowPlayerStatsCommand(IGameAccountService accountService, string playerName)
    {
        _accountService = accountService;
        _playerName = playerName;
    }

    public void Execute()
    {
        var player = _accountService.ReadAccount(_playerName);
        if (player != null)
        {
            player.GetStats();
        }
        else
        {
            Console.WriteLine("Player not found.");
        }
    }

    public void DisplayCommandInfo()
    {
        Console.WriteLine("Displays statistics for a specific player.");
    }
}


public class PlayGameCommand : IGameCommand
{
    private readonly IGameAccountService _accountService;
    private GameAccount _player1;
    private GameAccount _player2;
    private int _numberOfGames;

    public PlayGameCommand(IGameAccountService accountService, string player1Name, string player2Name, int numberOfGames = 3)
    {
        _accountService = accountService;
        _player1 = _accountService.ReadAccount(player1Name);
        _player2 = _accountService.ReadAccount(player2Name);
        _numberOfGames = numberOfGames;
    }

    public void Execute()
    {
        if (_player1 == null || _player2 == null)
        {
            Console.WriteLine("One or both players not found.");
            return;
        }

        for (int i = 0; i < _numberOfGames; i++)
        {
            Console.WriteLine($"Game {i + 1}:");
            _player1.PlayGame(_player2);
            _player1.GetStats();
            _player2.GetStats();
        }
    }

    public void DisplayCommandInfo()
    {
        Console.WriteLine($"Plays {_numberOfGames} games between two players.");
    }
}


class Program
{
    static void Main(string[] args)
    {
        DbContext dbContext = new DbContext();
        IGameAccountRepository accountRepository = new GameAccountRepository(dbContext);
        IGameAccountService accountService = new GameAccountService(accountRepository);

        Console.WriteLine("Enter the name of the first player:");
        string player1Name = Console.ReadLine();
        Console.WriteLine("Enter the rating of the first player:");
        int player1Rating = int.Parse(Console.ReadLine());

        var addPlayer1Command = new AddPlayerCommand(accountService, player1Name, player1Rating);
        addPlayer1Command.Execute();

        Console.WriteLine("Enter the name of the second player:");
        string player2Name = Console.ReadLine();
        Console.WriteLine("Enter the rating of the second player:");
        int player2Rating = int.Parse(Console.ReadLine());

        var addPlayer2Command = new AddPlayerCommand(accountService, player2Name, player2Rating);
        addPlayer2Command.Execute();

        var playGameCommand = new PlayGameCommand(accountService, player1Name, player2Name);
        playGameCommand.Execute();


        while (true)
        {
            Console.WriteLine("Enter a player's name to view their stats, or just press Enter to exit:");
            string playerName = Console.ReadLine();

            if (string.IsNullOrWhiteSpace(playerName))
            {
                break;
            }

            var showPlayerStatsCommand = new ShowPlayerStatsCommand(accountService, playerName);
            showPlayerStatsCommand.Execute();
        }

        Console.WriteLine("Thank you for playing!");


    }

}
