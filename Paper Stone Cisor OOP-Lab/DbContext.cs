using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Paper_Stone_Cisor_OOP_Lab
{
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
}
