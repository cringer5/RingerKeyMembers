using System;
using System.Collections.Generic;
using System.Text;
using RingerKeyMembers.Models;
using RingerKeyMembers.Interfaces;

namespace RingerKeyMembers.Classes
{
    // If were a web page, would want to encode all output strings  
    public class DisplayManager : IDisplayManager
    {
        public DisplayManager()
        {

        }

        public void PrintMessage(string message, bool addNewLine = true)
        {
            var newLine = addNewLine ? "\n" : String.Empty;
            Console.Write($"{message}{newLine}");
        }

        public void PrintList(List<String> messages, bool addNewLine = true)
        {
            foreach (var message in messages)
            {
                PrintMessage(message, addNewLine);
            }
        }
    }
}
