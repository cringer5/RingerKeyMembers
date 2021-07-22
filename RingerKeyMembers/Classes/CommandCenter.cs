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
            "KEYS", "MEMBERS", "ALLMEMBERS", "ITEMS", "INTERSECTION"
        };

        private readonly IDictionary<string, List<string>> _keyCollection;
        private readonly IKeyManager _keyMgr;
        private readonly IDisplayManager _dspMgr;

        public CommandCenter(IDictionary<string, List<string>> keyCollection, IKeyManager keyMgr, IDisplayManager dspMgr)
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
                        Console.WriteLine($"{commandUpper} command is invalid.");
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

            commandInfo.Count = 0;
            if (!String.IsNullOrEmpty(commandInfo.Member))
            {
                commandInfo.Count = 2;
            } else if (!String.IsNullOrEmpty(commandInfo.Key))
            {
                commandInfo.Count = 1;
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
                commandInfo.Count == 2)
            {
                msg = _keyMgr.AddMember(commandInfo, _keyCollection);
            }
            else if (commandUpper == "REMOVE" &&
                     commandInfo.Count == 2)
            {
                msg = _keyMgr.RemoveMember(commandInfo, _keyCollection);
            }
            else if (commandUpper == "KEYEXISTS" &&
                     commandInfo.Count == 1)
            {
                msg = _keyMgr.KeyExists(commandInfo, _keyCollection);
            }
            else if (commandUpper == "MEMBEREXISTS" &&
                     commandInfo.Count == 2)
            {
                msg = _keyMgr.MemberExists(commandInfo, _keyCollection);
            }
            else if (commandUpper == "REMOVEALL" &&
                     commandInfo.Count == 1)
            {
                msg = _keyMgr.RemoveAllMembers(commandInfo, _keyCollection);
            }
            // CLEAR needs to be all by itself 
            else if (commandUpper == "CLEAR" &&
                     commandInfo.Count == 0)
            {
                _keyMgr.ClearAllKeys(_keyCollection);
            }
            else if (commandUpper == "HELP")
            {
                msg = "INFO: Please Refer to the PDF instructions and the README Addendum.";
            }
            else if (commandUpper == "EXIT" &&
                     commandInfo.Count == 0)
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
                commandInfo.Count == 0)
            {
                list = _keyMgr.GetAllKeys(_keyCollection);
            }
            else if (commandUpper == "MEMBERS" &&  // for a specific Key 
                     commandInfo.Count == 1)
            {
                list = _keyMgr.GetKeyMembers(commandInfo.Key, _keyCollection);
                    if (list.Count == 0)
                    {
                        _dspMgr.PrintMessage($"ERROR: {commandInfo.Key} does not exist");
                    }
                }
            else if (commandUpper == "ALLMEMBERS" &&
                     commandInfo.Count == 0)
            {
                list = _keyMgr.GetAllMembers(_keyCollection);
            }
            else if (commandUpper == "ITEMS" &&
                     commandInfo.Count == 0)
            {
                list = _keyMgr.GetAllItems(_keyCollection);
            }
            else if (commandUpper == "INTERSECTION" &&
                     commandInfo.Count == 2)
            {
                list = _keyMgr.GetIntersection(commandInfo.Key, commandInfo.Member, _keyCollection);
            }
            else
            {
                _dspMgr.PrintMessage($"ERROR: {commandUpper} syntax is invalid.");
            }

            return list;
        }

    }
}
