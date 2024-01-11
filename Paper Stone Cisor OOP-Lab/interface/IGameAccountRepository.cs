using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Paper_Stone_Cisor_OOP_Lab
{
    public interface IGameAccountRepository
    {
        void CreateAccount(GameAccount account);
        GameAccount ReadAccount(string playerName);
        void UpdateAccount(GameAccount account);
        void DeleteAccount(string playerName);
        List<GameAccount> GetAllAccounts();
    }
}
