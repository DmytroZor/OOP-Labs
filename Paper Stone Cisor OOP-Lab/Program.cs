using System;
using System.Collections.Generic;

namespace Paper_Stone_Cisor_OOP_Lab
{
    class Program
    {
        static List<IGameCommand> Setup(DbContext dbContext, IGameAccountService accountService)
        {
            return new List<IGameCommand>
            {
                new DisplayPlayersCommand(accountService),

            };
        }

        static void Main(string[] args)
        {
            DbContext dbContext = new DbContext();
            IGameAccountRepository accountRepository = new GameAccountRepository(dbContext);
            IGameAccountService accountService = new GameAccountService(accountRepository);

            List<IGameCommand> commands = Setup(dbContext, accountService);

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
            commands.Add(playGameCommand);

            while (true)
            {
                for (int i = 0; i < commands.Count; i++)
                {
                    Console.WriteLine($"{i + 1}. {commands[i].DisplayCommandInfo()}");
                }

                Console.Write("Enter command number or 'q' to quit: ");
                string input = Console.ReadLine();

                if (input.ToLower() == "q")
                    break;

                if (int.TryParse(input, out int selectedCommand) && selectedCommand >= 1 && selectedCommand <= commands.Count)
                {
                    commands[selectedCommand - 1].Execute();
                }
                else
                {
                    Console.WriteLine("Invalid input. Please enter a valid command number.");
                }
            }

            Console.WriteLine("Thank you for playing!");
        }
    }
}
