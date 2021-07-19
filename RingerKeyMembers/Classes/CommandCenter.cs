using System;
using System.Collections.Generic;
using System.Text;
using RingerKeyMembers.Interfaces;
using RingerKeyMembers.Models;

// Dispatches all user commands to be executed downstream. 
namespace RingerKeyMembers.Classes
{
    public class CommandCenter : ICommandCenter
    {
        // These perform an action then return a status msg 
        private readonly List<string> _ActionCommands = new List<string>()
        {
            "ADD", "REMOVE", "KEYEXISTS", "MEMBEREXISTS", "REMOVEALL", "CLEAR", "HELP", "EXIT"
        };

        // These return a list of dictionary data 
        private readonly List<string> _ListCommands = new List<string>()
        {
            "KEYS", "MEMBERS", "ALLMEMBERS", "ITEMS"
        };

        private readonly IDictionary<string, List<string>> _keyCollection;
        private readonly KeyManager _keyMgr;
        private readonly DisplayManager _dspMgr;

        public CommandCenter(IDictionary<string, List<string>> keyCollection, KeyManager keyMgr, DisplayManager dspMgr)
        {
            _keyCollection = keyCollection;
            _keyMgr = keyMgr;
            _dspMgr = dspMgr;
        }

        public void run()
        {
            CommandInfo commandInfo;
            var commandUpper = String.Empty;

            try
            {
                // just spin until user exits 
                do
                {
                    commandInfo = getNewCommand();
                    commandUpper = commandInfo.Command.ToUpper();
                    var msg = String.Empty;
                    var list = new List<string>();

                    if (_ActionCommands.Contains(commandUpper))
                    {
                        msg = this.RunActionCommand(commandInfo);
                        if (!String.IsNullOrWhiteSpace(msg))
                        {
                            _dspMgr.PrintMessage(msg);
                        }
                    }
                    else if (_ListCommands.Contains(commandUpper))
                    {
                        list = RunListCommand(commandInfo);
                        if (list.Count > 0)
                        {
                            _dspMgr.PrintList(list);
                        }
                    }
                    else
                    {
                        Console.WriteLine($"{commandUpper} is invalid.");
                    }
                } while (commandUpper != "EXIT");
            }
            catch (Exception exc)
            {
                // TODO: Log this error somewhere 
                Console.WriteLine($"Run time ERROR: {exc.Message}. Contact 24/7 support.");
                Console.ReadKey();
            }
        }

        public CommandInfo getNewCommand()
        {
            var newCommand = String.Empty;

            do
            {
                Console.WriteLine("Please enter a Command (type HELP for usage)");
                newCommand = Console.ReadLine();
            } while (String.IsNullOrWhiteSpace(newCommand));

            var commandInfo = parseCommand(newCommand);
            return commandInfo;
        }

        // See what the user entered 
        public CommandInfo parseCommand(string userCommand)
        {
            var commandInfo = new CommandInfo();

            // sanitize input
            userCommand = !String.IsNullOrWhiteSpace(userCommand) ? userCommand : String.Empty;
            userCommand = userCommand.Replace("\t", " ");
            userCommand = userCommand.Trim();

            // break it up 
            var pieces = userCommand.Split(" ");
            if (pieces.Length >= 1)
            {
                commandInfo.Command = pieces[0];
            }

            if (pieces.Length >= 2)
            {
                commandInfo.Key = pieces[1];
            }

            if (pieces.Length >= 3)
            {
                commandInfo.Member = String.Join(" ", pieces, 2, pieces.Length - 2);
            }

            return commandInfo;
        }

        // Typically doing something specific with a key and/or member.
        // Any message returned here means an ERROR (except for HELP). 
        public string RunActionCommand(CommandInfo commandInfo)
        {
            string msg = String.Empty;
            var commandUpper = commandInfo.Command.ToUpper();

            if (commandUpper == "ADD" &&
                !String.IsNullOrWhiteSpace(commandInfo.Key) &&
                !String.IsNullOrWhiteSpace(commandInfo.Member))
            {
                msg = _keyMgr.AddMember(commandInfo, _keyCollection);
            }
            else if (commandUpper == "REMOVE" &&
                     !String.IsNullOrWhiteSpace(commandInfo.Key) &&
                     !String.IsNullOrWhiteSpace(commandInfo.Member))
            {
                msg = _keyMgr.RemoveMember(commandInfo, _keyCollection);
            }
            else if (commandUpper == "KEYEXISTS" &&
                    !String.IsNullOrWhiteSpace(commandInfo.Key) &&
                    String.IsNullOrWhiteSpace(commandInfo.Member))
            {
                msg = _keyMgr.KeyExists(commandInfo, _keyCollection);
            }
            else if (commandUpper == "MEMBEREXISTS" &&
                     !String.IsNullOrWhiteSpace(commandInfo.Key) &&
                     !String.IsNullOrWhiteSpace(commandInfo.Member))
            {
                msg = _keyMgr.MemberExists(commandInfo, _keyCollection);
            }
            else if (commandUpper == "REMOVEALL" &&
                     !String.IsNullOrWhiteSpace(commandInfo.Key) &&
                     String.IsNullOrWhiteSpace(commandInfo.Member))
            {
                msg = _keyMgr.RemoveAllMembers(commandInfo, _keyCollection);
            }
            // CLEAR needs to be all by itself 
            else if (commandUpper == "CLEAR" &&
                     String.IsNullOrWhiteSpace(commandInfo.Key) &&
                     String.IsNullOrWhiteSpace(commandInfo.Member))
            {
                _keyMgr.ClearAllKeys(_keyCollection);
            }
            else if (commandUpper == "HELP")
            {
                msg = "INFO: Please Refer to the PDF instructions and the README Addendum.";
            }
            else if (commandUpper == "EXIT" &&
                     String.IsNullOrWhiteSpace(commandInfo.Key) &&
                     String.IsNullOrWhiteSpace(commandInfo.Member))
            {
                // no operation 
            }
            else
            {
                msg = $"ERROR: {commandUpper} syntax is invalid.";
            }

            return msg;
        }

        // Generates various lists to display.
        // Empty list means could not find any data. 
        public List<String> RunListCommand(CommandInfo commandInfo)
        {
            var list = new List<String>();
            var commandUpper = commandInfo.Command.ToUpper();

            if (commandUpper == "KEYS" &&
                String.IsNullOrWhiteSpace(commandInfo.Key) &&
                String.IsNullOrWhiteSpace(commandInfo.Member))
            {
                list = _keyMgr.GetAllKeys(_keyCollection);
            }
            else if (commandUpper == "MEMBERS")  // for a specific Key 
                if (!String.IsNullOrWhiteSpace(commandInfo.Key) &&
                     String.IsNullOrWhiteSpace(commandInfo.Member))
                {
                    list = _keyMgr.GetKeyMembers(commandInfo, _keyCollection);
                }
                else
                {
                    _dspMgr.PrintMessage($"ERROR: {commandUpper} syntax is invalid.");
                }
            else if (commandUpper == "ALLMEMBERS" &&
                     String.IsNullOrWhiteSpace(commandInfo.Key) &&
                     String.IsNullOrWhiteSpace(commandInfo.Member))
            {
                list = _keyMgr.GetAllMembers(_keyCollection);
            }
            else if (commandUpper == "ITEMS" &&
                     String.IsNullOrWhiteSpace(commandInfo.Key) &&
                     String.IsNullOrWhiteSpace(commandInfo.Member))
            {
                list = _keyMgr.GetAllItems(_keyCollection);
            }

            return list;
        }

    }
}
