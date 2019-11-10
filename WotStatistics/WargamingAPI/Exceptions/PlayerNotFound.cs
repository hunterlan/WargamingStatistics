using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WotStatistics.Exceptions
{
    public class PlayerNotFound : Exception
    {
        public PlayerNotFound(string message) : base(message)
        {

        }
    }
}
