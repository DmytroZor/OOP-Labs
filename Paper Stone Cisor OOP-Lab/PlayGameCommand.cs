using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Paper_Stone_Cisor_OOP_Lab
{
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

}
