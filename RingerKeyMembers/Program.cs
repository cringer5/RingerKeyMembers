using System;
using System.Collections.Generic;
using RingerKeyMembers.Classes;
using RingerKeyMembers.Interfaces;

namespace RingerKeyMembers
{
    class Program
    {
        static void Main(string[] args)
        {
            IDisplayManager displayMgr = new DisplayManager();
            displayMgr.PrintMessage("Welcome to the Multi-Value Dictionary Manager!\n");

            var keyCollection = new Dictionary<string, List<string>>();
            IKeyManager keyMgr = new KeyManager(displayMgr);
            ICommandCenter commandCenter = new CommandCenter(keyCollection, keyMgr, displayMgr);  // ~ inject them
            commandCenter.run();

            displayMgr.PrintMessage("\nThank you for playing! We have some lovely parting gifts for you.");
            Console.ReadKey();
        }
    }
}
