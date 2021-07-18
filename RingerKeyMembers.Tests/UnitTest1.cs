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
        private readonly KeyManager _keyMgr;
        private readonly DisplayManager _dspMgr;
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

    }
}
