using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WotStatistics.Model
{
    public class Player
    {
        public string Nickname { get; set; }
        public int Id { get; set; }

        public override string ToString()
        {
            return Nickname.Replace("_", @"\_");
        }
    }
}
