using System;
using Xunit;
using Moq;
using System.Linq;

using RingerKeyMembers.Interfaces;
using RingerKeyMembers.Classes;
using RingerKeyMembers.Models;
using System.Collections.Generic;

namespace RingerKeyMembers.Tests
{
    public class UnitTest1
    {
        private readonly Mock<ICommandCenter> _mockCmdCenter;
        private readonly Dictionary<string, List<string>> keyCollection; 
        private readonly IKeyManager _keyMgr;
        private readonly IDisplayManager _dspMgr;
        private readonly ICommandCenter _cmdCenter;

        public UnitTest1()
        {
            // build some helpers 
            _mockCmdCenter = new Mock<ICommandCenter>();
            keyCollection = new Dictionary<string, List<string>>();
            _keyMgr = new KeyManager();
            _dspMgr = new DisplayManager();
            _cmdCenter = new CommandCenter(keyCollection, _keyMgr, _dspMgr);  // ~ inject them
        }

        // Test adding valid keys and members
        [Fact]
        public void AddGoodKeyMembers()
        {
            // arrange 
            var expectedKeyList = new List<String>()
            {
                "Key1", "Key2"
            };

            var expectedMbrList = new List<String>()
            {
                "Mbr1", "Mbr2"
            };


            var cmdInfo1 = new CommandInfo()
            {
                Command = "",
                Key = "Key1",
                Member = "Mbr1"
            };

            var cmdInfo2 = new CommandInfo()
            {
                Command = "",
                Key = "Key1",
                Member = "Mbr2"
            };

            var cmdInfo3 = new CommandInfo()
            {
                Command = "",
                Key = "Key2",
                Member = "Mbr1"
            };

            // act
            var msg1 = _keyMgr.AddMember(cmdInfo1, keyCollection);
            var msg2 = _keyMgr.AddMember(cmdInfo2, keyCollection);
            var msg3 = _keyMgr.AddMember(cmdInfo3, keyCollection);

            // assert 
            int keyCount = keyCollection.Count;
            var keysAdded = _keyMgr.GetAllKeys(keyCollection);
            var keyDiff = expectedKeyList.Except(keysAdded);
            int keyDiffCount = keyDiff.Count();

            var mbrsAdded = _keyMgr.GetAllMembers(keyCollection);
            var mbrsAddedCount = mbrsAdded.Count;

            Assert.Equal(2, keyCount);
            Assert.Equal(0, keyDiffCount);
            Assert.Equal(3, mbrsAddedCount);
            Assert.Equal("", msg1);
            Assert.Equal("", msg2);
            Assert.Equal("", msg3);
        }

        // Test trying to add duplicate key members (not allowed) 
        [Fact]
        public void AddDuplicateKeyMembers()
        {
            // arrange 
            var cmdInfo1 = new CommandInfo()
            {
                Command = "",
                Key = "Key1",
                Member = "Mbr1"
            };

            // act
            var msg1 = _keyMgr.AddMember(cmdInfo1, keyCollection);
            var msg2 = _keyMgr.AddMember(cmdInfo1, keyCollection);

            // assert 
            Assert.Equal("", msg1);
            Assert.Contains("ERROR", msg2);
        }

        // Test trying to remove an existing key member
        [Fact]
        public void RemoveGoodKeyMember()
        {
            // arrange 
            var cmdInfo1 = new CommandInfo()
            {
                Command = "",
                Key = "Key1",
                Member = "Mbr1"
            };

            // act
            var msg1 = _keyMgr.AddMember(cmdInfo1, keyCollection);
            var allKeys1 = _keyMgr.GetAllKeys(keyCollection);
            var allKeys1Count = allKeys1.Count;
            var msg2 = _keyMgr.RemoveMember(cmdInfo1, keyCollection);
            var allKeys2 = _keyMgr.GetAllKeys(keyCollection);
            var allKeys2Count = allKeys2.Count;

            // assert 
            Assert.Equal("", msg1);
            Assert.Equal("", msg2);
            Assert.Equal(1, allKeys1Count);
            Assert.Equal(0, allKeys2Count);  // no key after last member deleted
        }

        // Test trying to remove a key / member that don't exist
        [Fact]
        public void RemoveBadKeyMember()
        {
            // arrange 
            var cmdInfo1 = new CommandInfo()
            {
                Command = "",
                Key = "Key1",
                Member = "Mbr1"
            };

            var cmdInfo2 = new CommandInfo()
            {
                Command = "",
                Key = "Key1",
                Member = "MbrX"
            };

            var cmdInfo3 = new CommandInfo()
            {
                Command = "",
                Key = "KeyBad",
                Member = "MbrX"
            };

            // act
            var msg1 = _keyMgr.AddMember(cmdInfo1, keyCollection);
            var msg2 = _keyMgr.RemoveMember(cmdInfo2, keyCollection);
            var msg3 = _keyMgr.RemoveMember(cmdInfo3, keyCollection);

            // assert 
            Assert.Equal("", msg1);
            Assert.Contains("ERROR", msg2);
            Assert.Contains("ERROR", msg3);
        }

        // Test trying to remove all existing members for a valid key
        [Fact]
        public void RemoveGoodKeyAllMembers()
        {
            // arrange 
            var cmdInfo1 = new CommandInfo()
            {
                Command = "",
                Key = "Key1",
                Member = "Mbr1"
            };

            var cmdInfo2 = new CommandInfo()
            {
                Command = "",
                Key = "Key1",
                Member = "Mbr2"
            };

            var cmdInfo3 = new CommandInfo()
            {
                Command = "",
                Key = "Key2",
                Member = "Mbr1"
            };

            var expectedKeyList = new List<String>()
            {
                "Key2"
            };

            // act
            var msg1 = _keyMgr.AddMember(cmdInfo1, keyCollection);
            var msg2 = _keyMgr.AddMember(cmdInfo2, keyCollection);
            var msg3 = _keyMgr.AddMember(cmdInfo3, keyCollection);

            var allKeys1 = _keyMgr.GetAllKeys(keyCollection);
            var allKeys1Count = allKeys1.Count;
            var msg4 = _keyMgr.RemoveAllMembers(cmdInfo1, keyCollection);
            var allKeys4 = _keyMgr.GetAllKeys(keyCollection);
            var allKeys4Count = allKeys4.Count;

            var keysLeft = _keyMgr.GetAllKeys(keyCollection);
            var keyDiff = expectedKeyList.Except(keysLeft);
            int keyDiffCount = keyDiff.Count();

            // assert 
            Assert.Equal("", msg1);
            Assert.Equal("", msg2);
            Assert.Equal("", msg3);
            Assert.Equal("", msg4);
            Assert.Equal(2, allKeys1Count);
            Assert.Equal(1, allKeys4Count);
            Assert.Equal(0, keyDiffCount);
        }

        // Test trying to remove all members for an invalid key
        [Fact]
        public void RemoveBadKeyAllMembers()
        {
            // arrange 
            var cmdInfo1 = new CommandInfo()
            {
                Command = "",
                Key = "Key1",
                Member = "Mbr1"
            };

            var cmdInfo2 = new CommandInfo()
            {
                Command = "",
                Key = "KeyX",
                Member = "Mbr1"
            };

            var expectedKeyList = new List<String>()
            {
                "Key1"
            };

            // act
            var msg1 = _keyMgr.AddMember(cmdInfo1, keyCollection);
            var msg2 = _keyMgr.RemoveAllMembers(cmdInfo2, keyCollection);
            var allKeys1 = _keyMgr.GetAllKeys(keyCollection);
            var allKeys1Count = allKeys1.Count;

            var keysLeft = _keyMgr.GetAllKeys(keyCollection);
            var keyDiff = expectedKeyList.Except(keysLeft);
            int keyDiffCount = keyDiff.Count();

            // assert 
            Assert.Equal("", msg1);
            Assert.Contains("ERROR", msg2);
            Assert.Equal(1, allKeys1Count);
            Assert.Equal(0, keyDiffCount);
        }

        // Test for a valid key in the collection 
        [Fact]
        public void CheckForGoodKey()
        {
            // arrange 
            var cmdInfo1 = new CommandInfo()
            {
                Command = "",
                Key = "Key1",
                Member = "Mbr1"
            };

            // act
            var msg1 = _keyMgr.AddMember(cmdInfo1, keyCollection);
            var msg2 = _keyMgr.KeyExists(cmdInfo1, keyCollection);

            // assert 
            Assert.Equal("", msg1);
            Assert.Equal("", msg2);
        }

        // Test for a bad key in the collection
        [Fact]
        public void CheckForBadKey()
        {
            // arrange 
            var cmdInfo1 = new CommandInfo()
            {
                Command = "",
                Key = "Key1",
                Member = "Mbr1"
            };

            var cmdInfo2 = new CommandInfo()
            {
                Command = "",
                Key = "KeyX",
                Member = "Mbr1"
            };


            // act
            var msg1 = _keyMgr.AddMember(cmdInfo1, keyCollection);
            var msg2 = _keyMgr.KeyExists(cmdInfo2, keyCollection);

            // assert 
            Assert.Equal("", msg1);
            Assert.Contains("ERROR", msg2);
        }

        // Test for a good member in a key in the collection
        [Fact]
        public void CheckForGoodMemberInKey()
        {
            // arrange 
            var cmdInfo1 = new CommandInfo()
            {
                Command = "",
                Key = "Key1",
                Member = "Mbr1"
            };

            // act
            var msg1 = _keyMgr.AddMember(cmdInfo1, keyCollection);
            var msg2 = _keyMgr.MemberExists(cmdInfo1, keyCollection);

            // assert 
            Assert.Equal("", msg1);
            Assert.Equal("", msg2);
        }

        // Test for a bad member for a key in the collection
        [Fact]
        public void CheckForBadMemberInKey()
        {
            // arrange 
            var cmdInfo1 = new CommandInfo()
            {
                Command = "",
                Key = "Key1",
                Member = "Mbr1"
            };

            var cmdInfo2 = new CommandInfo()
            {
                Command = "",
                Key = "Key1",
                Member = "Mbr2"
            };

            // act
            var msg1 = _keyMgr.AddMember(cmdInfo1, keyCollection);
            var msg2 = _keyMgr.MemberExists(cmdInfo2, keyCollection);

            // assert 
            Assert.Equal("", msg1);
            Assert.Contains("ERROR", msg2);
        }

        // Test clearing the entire dictionary 
        [Fact]
        public void ClearTheDictionary()
        {
            // arrange 
            var cmdInfo1 = new CommandInfo()
            {
                Command = "",
                Key = "Key1",
                Member = "Mbr1"
            };

            var cmdInfo2 = new CommandInfo()
            {
                Command = "",
                Key = "Key1",
                Member = "Members Only"
            };

            var cmdInfo3 = new CommandInfo()
            {
                Command = "",
                Key = "Key2",
                Member = "Mbr One"
            };

            // act
            var msg1 = _keyMgr.AddMember(cmdInfo1, keyCollection);
            msg1     = _keyMgr.AddMember(cmdInfo2, keyCollection);
            msg1     = _keyMgr.AddMember(cmdInfo3, keyCollection);
            var mbrsAdded = _keyMgr.GetAllMembers(keyCollection);
            var mbrsAddedCount = mbrsAdded.Count;

            _keyMgr.ClearAllKeys(keyCollection);
            var mbrsGone = _keyMgr.GetAllMembers(keyCollection);
            var mbrsGoneCount = mbrsGone.Count;

            // assert 
            Assert.Equal(3, mbrsAddedCount);
            Assert.Equal(0, mbrsGoneCount);
        }

        // Test retrieving a list of all items (all keys/members)
        [Fact]
        public void RetrieveAllItemsFromDictionary()
        {
            // arrange 
            var cmdInfo1 = new CommandInfo()
            {
                Command = "",
                Key = "Key1",
                Member = "Mbr1"
            };

            var cmdInfo2 = new CommandInfo()
            {
                Command = "",
                Key = "Key1",
                Member = "Members Only"
            };

            var cmdInfo3 = new CommandInfo()
            {
                Command = "",
                Key = "Key2",
                Member = "Mbr One"
            };

            // act
            var msg1 = _keyMgr.AddMember(cmdInfo1, keyCollection);
            msg1 = _keyMgr.AddMember(cmdInfo2, keyCollection);
            msg1 = _keyMgr.AddMember(cmdInfo3, keyCollection);
            var mbrsAdded = _keyMgr.GetAllMembers(keyCollection);
            var mbrsAddedCount = mbrsAdded.Count;

            var allKeysMembers = _keyMgr.GetAllItems(keyCollection);
            var allKeysMembersCount = allKeysMembers.Count;

            // assert 
            Assert.Equal(mbrsAddedCount, allKeysMembersCount);
        }

    }
}
