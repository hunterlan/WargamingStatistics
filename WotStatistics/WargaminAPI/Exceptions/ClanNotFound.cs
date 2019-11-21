using System;
using System.Collections.Generic;
using System.Text;

namespace WargaminAPI.Exceptions
{
    public class ClanNotFound : Exception
    {
        public ClanNotFound(string message): base(message)
        {

        }
    }
}
