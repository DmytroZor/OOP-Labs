using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Paper_Stone_Cisor_OOP_Lab
{
    class TrainingGameFactory : IGameFactory
    {
        public IGame CreateGame()
        {
            return new TrainingGame();
        }
    }
}
