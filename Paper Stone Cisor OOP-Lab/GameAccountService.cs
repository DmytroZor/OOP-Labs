using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Paper_Stone_Cisor_OOP_Lab
{
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
}
