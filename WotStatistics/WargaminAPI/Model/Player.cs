using System;
using System.Collections.Generic;
using System.Text;

namespace WargaminAPI.Model
{
    public class Player
    {
        public string Nickname { get; set; }
        public int Id { get; set; }

        public override string ToString()
        {
            return Nickname;
        }
    }
}
