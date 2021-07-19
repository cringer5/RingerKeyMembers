using RingerKeyMembers.Models;
using System.Collections.Generic;

namespace RingerKeyMembers.Interfaces
{
    public interface IKeyManager
    {
        string AddMember(CommandInfo commandInfo, IDictionary<string, List<string>> keyCollection);

        void ClearAllKeys(IDictionary<string, List<string>> keyCollection);

        List<string> GetAllItems(IDictionary<string, List<string>> keyCollection);

        List<string> GetAllKeys(IDictionary<string, List<string>> keyCollection);

        List<string> GetAllMembers(IDictionary<string, List<string>> keyCollection);

        List<string> GetKeyMembers(CommandInfo commandInfo, IDictionary<string, List<string>> keyCollection);

        string KeyExists(CommandInfo commandInfo, IDictionary<string, List<string>> keyCollection);

        string MemberExists(CommandInfo commandInfo, IDictionary<string, List<string>> keyCollection);

        string RemoveAllMembers(CommandInfo commandInfo, IDictionary<string, List<string>> keyCollection);

        string RemoveMember(CommandInfo commandInfo, IDictionary<string, List<string>> keyCollection);
    }
}