using System;
using System.Collections.Generic;
using System.Text;
using RingerKeyMembers.Models;

namespace RingerKeyMembers.Interfaces
{
    interface ICommandCenter
    {
        public CommandInfo parseCommand(string userCommand);

    }
}
