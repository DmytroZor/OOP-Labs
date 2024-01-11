using System;
using System.Collections.Generic;
using System.Linq;
namespace Paper_Stone_Cisor_OOP_Lab
{

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
}