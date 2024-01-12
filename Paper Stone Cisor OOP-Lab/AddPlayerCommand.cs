using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Paper_Stone_Cisor_OOP_Lab
{
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

        public string DisplayCommandInfo()
        {
            return "Display all players";
        }
    }
}
