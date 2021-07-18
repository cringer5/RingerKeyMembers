using System;
using System.Collections.Generic;
using RingerKeyMembers.Classes;
using RingerKeyMembers.Models;

namespace RingerKeyMembers
{
    class Program
    {
        static void Main(string[] args)
        {
            Console.WriteLine("Welcome to the Multi-Value Dictionary Manager!\n");

            var keyCollection = new Dictionary<string, List<string>>();
            var keyMgr = new KeyManager();
            var displayMgr = new DisplayManager();
            var commandCenter = new CommandCenter(keyCollection, keyMgr, displayMgr);  // ~ inject them
            commandCenter.run();

            Console.WriteLine("\nThank you for playing! We have some lovely parting gifts for you.");
            Console.ReadKey();
        }
    }
}
