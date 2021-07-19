using System.Collections.Generic;

namespace RingerKeyMembers.Interfaces
{
    public interface IDisplayManager
    {
        void PrintList(List<string> messages, bool addNewLine = true);

        void PrintMessage(string message, bool addNewLine = true);
    }
}