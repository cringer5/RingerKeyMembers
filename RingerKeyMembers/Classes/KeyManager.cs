using System;
using System.Collections.Generic;
using System.Text;
using RingerKeyMembers.Models;
using System.Linq;

namespace RingerKeyMembers.Classes
{
    public class KeyManager
    {
        public KeyManager()
        {

        }

        // Empty return value means success! Else it's an error message. 
        public string AddMember(CommandInfo commandInfo, IDictionary<string, List<string>> keyCollection)
        {
            var msg = String.Empty;

            // it's brand new?
            var hasKey = keyCollection.ContainsKey(commandInfo.Key);
            if (!hasKey)
            {
                var newMembersList = new List<string>();
                keyCollection.Add(commandInfo.Key, newMembersList);
            }

            var membersList = keyCollection[commandInfo.Key];
            if (!membersList.Contains(commandInfo.Member))
            {
                membersList.Add(commandInfo.Member);
            }
            else
            {
                // sorry but duplicates not allowed 
                msg = $"ERROR: {commandInfo.Key} {commandInfo.Member} already exists.";
            }

            return msg;
        }

        // Empty return value means success! Else it's an error message. 
        public string RemoveMember(CommandInfo commandInfo, IDictionary<string, List<string>> keyCollection)
        {
            var msg = String.Empty;

            var hasKey = keyCollection.ContainsKey(commandInfo.Key);
            if (!hasKey)
            {
                msg = $"ERROR: {commandInfo.Key} does not exist.";
                return msg;
            } 
            
            var membersList = keyCollection[commandInfo.Key];
            if (!membersList.Contains(commandInfo.Member))
            {
                msg = $"ERROR: {commandInfo.Key} {commandInfo.Member} does not exist.";
                return msg;
            }

            // green light to purge 
            if (membersList.Count > 1)
            {
                membersList.Remove(commandInfo.Member);
            }
            // if just one member, remove the key too! 
            else 
            {
                msg = RemoveAllMembers(commandInfo, keyCollection);
            }

            return msg;
        }

        // Starting totally over  
        public void ClearAllKeys(IDictionary<string, List<string>> keyCollection)
        {
            keyCollection.Clear();
        }

        // See if a member exists for a key 
        public string MemberExists(CommandInfo commandInfo, IDictionary<string, List<string>> keyCollection)
        {
            var msg = KeyExists(commandInfo, keyCollection);
            if (String.IsNullOrWhiteSpace(msg))
            {
                if (! keyCollection[commandInfo.Key].Contains(commandInfo.Member))
                {
                    msg = $"ERROR: {commandInfo.Key} {commandInfo.Member} was not found";
                }
            }

            return msg;
        }

        // See if key exists in dictionary 
        public string KeyExists(CommandInfo commandInfo, IDictionary<string, List<string>> keyCollection)
        {
            var msg = String.Empty;

            if (! keyCollection.ContainsKey(commandInfo.Key))
            {
                msg = $"ERROR: {commandInfo.Key} was not found";
            }

            return msg;
        }

        // Delete a specific key from collection (which deletes its members too)
        public string RemoveAllMembers(CommandInfo commandInfo, IDictionary<string, List<string>> keyCollection)
        {
            var msg = String.Empty;

            if (keyCollection.ContainsKey(commandInfo.Key))
            {
                keyCollection.Remove(commandInfo.Key);
            }
            else
            {
                msg = $"ERROR: {commandInfo.Key} was not found";
            }

            return msg;
        }

        // Get all members for a given key 
        public List<String> GetKeyMembers(CommandInfo commandInfo, IDictionary<string, List<string>> keyCollection)
        {
            var allMembers = new List<String>();

            if (keyCollection.ContainsKey(commandInfo.Key))
            {
                allMembers = keyCollection[commandInfo.Key];
            }

            return allMembers;
        }

        // Get all keys in the dictionary. May not find any but that's fine. 
        public List<String> GetAllKeys(IDictionary<string, List<string>> keyCollection)
        {
            var allKeys = keyCollection.Keys.ToList();  // Tech Note: easier to debug if 2 lines
            return allKeys;
        }

        // Not sure of the value of this to a user b/c of potential displayed duplicate values. ALLITEMS makes more sense. 
        public List<String> GetAllMembers(IDictionary<string, List<string>> keyCollection)
        {
            var allMembers = new List<String>();
            var keys = GetAllKeys(keyCollection);

            foreach (var key in keys)
            {
                allMembers = allMembers.Concat(keyCollection[key]).ToList();  // yes duplicates are allowed 
            }

            return allMembers;
        }

        // Returns all key/member pairs in the dictionary. 
        public List<String> GetAllItems(IDictionary<string, List<string>> keyCollection)
        {
            var allItems = new List<String>();
            var keys = GetAllKeys(keyCollection);

            foreach (var key in keys)
            {
                var allMembers = keyCollection[key];  
                foreach (var member in allMembers)
                {
                    allItems.Add($"{key} {member}");
                }
            }

            return allItems;
        }

    }
}
