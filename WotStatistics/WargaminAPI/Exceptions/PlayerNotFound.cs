using System;
using System.Collections.Generic;
using System.Text;

namespace WargaminAPI.Exceptions
{
    public class PlayerNotFound : Exception
    {
        public PlayerNotFound(string message) : base(message)
        {

        }
    }
}
