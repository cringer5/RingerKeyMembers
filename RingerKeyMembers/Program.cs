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
            Console.WriteLine("Welcome to the Multi-Value Dictionary Manager!\n");

            var keyCollection = new Dictionary<string, List<string>>();
            IKeyManager keyMgr = new KeyManager();
            IDisplayManager displayMgr = new DisplayManager();
            ICommandCenter commandCenter = new CommandCenter(keyCollection, keyMgr, displayMgr);  // ~ inject them
            commandCenter.run();

            Console.WriteLine("\nThank you for playing! We have some lovely parting gifts for you.");
            Console.ReadKey();
        }
    }
}
