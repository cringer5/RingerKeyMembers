using RingerKeyMembers.Models;
using System.Collections.Generic;

namespace RingerKeyMembers.Interfaces
{
    public interface ICommandCenter
    {
        CommandInfo getNewCommand();

        CommandInfo parseCommand(string userCommand);

        void run();

        string RunActionCommand(CommandInfo commandInfo);

        List<string> RunListCommand(CommandInfo commandInfo);
    }
}