using System;
using System.Collections.Generic;
using System.Text;

namespace RingerKeyMembers.Models
{
    public class CommandInfo
    {
        public string Command { get; set; }

        public string Key { get; set; }

        public string Member { get; set; }

        public int Count { get; set; }  // 0=just cmd, 1=cmd w/key, 2=has key/member 
    }
}
