using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Paper_Stone_Cisor_OOP_Lab
{
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

        public string DisplayCommandInfo()
        {
            return ("Displays statistics for a specific player.");
        }
    }
}
