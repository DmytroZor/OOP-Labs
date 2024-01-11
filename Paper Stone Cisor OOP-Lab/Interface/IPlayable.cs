using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Paper_Stone_Cisor_OOP_Lab
{
    public interface IPlayable
    {
        void PlayGame(IPlayable opponent);
    }
}
