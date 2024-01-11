using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Linq;

namespace Paper_Stone_Cisor_OOP_Lab
{
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
}
