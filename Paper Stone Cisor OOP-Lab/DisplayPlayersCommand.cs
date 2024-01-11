using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Paper_Stone_Cisor_OOP_Lab
{
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
}
