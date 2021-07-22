# cringer5 / RingerKeyMembers 
# C# Console Dictionary App for Key / Member Pairs 
## Build Instructions

- Compiled in Visual Studio 2019 as .NET Core 3.1 app.
 
## Run Instructions
- Download the zip file.
- Load the solution in Visual Studio and run, or run the RingerKeyMembers.exe directly in powershell. 

## Usage 
### General Information
- This is a simple Console application. The temporary dictionary is built in memory.
- Command names are not case sensitive
These are all valid and equivalent: ADD Add aDd 
- A Key is the first word after a command. 
- The member (value) are all word(s) after the Key. 
- Keys and members are case sensitive. These keys are all different:
- MyKey 
- MyKEY 
- myKey 
- These members are all different:
- Hello World
- HEllo World
- hello world 

- Tabs are treated like spaces. These commands are equivalent: 
- Add MyKey Here is the Member
- Add MyKey---Tab--->Here is the---Tab--->Member
- Valid commands do NOT have a confirmation message (it felt like unnecessary noise for a console app). 
- If you run a command and do NOT see an error message, it was successful. This can easily be changed to add a confirmation message. 

### Action Commands
These are commands that edit the dictionary.
- Add : Adds a key/member to the dictionary
- ADD MyKey My Member
- Remove: Removes a key/member
- REMOVE MyKey My Member
- RemoveAll : Removes a Key and all its members
- REMOVEALL SomeKey
- Clear : Clears the entire dictionary
- CLEAR

### List Commands 
These are commands that list content from the dictionary. A list command that returns nothing indicates an empty list (not in the dictionary).
- Keys : Lists all keys in the dictionary
- KEYS
- Members : List all members for a given Key
- MEMBERS MyKey
- AllMembers : List all members in the dictionary
- ALLMEMBERS
- Items : Lists all key and members pairs in the entire dictionary.
- ITEMS
- Intersection : See what members 2 key lists have in common
- INTERSECTION KEY1 KEY2

### Other Commands 
- KeyExists - Checks to see if a key exists in dictionary. If you don't see an error message, it exists. 
- KEYEXISTS MyKey
- MemberExists : Checks to see if a key member exists in dictionary. If you don't see an error message, it exists. 
- MEMBEREXISTS MyKey MyMember 
- Help : Display rudimentary help for this app. :-) 
- HELP
- Exit : Exits the app. 
- EXIT 

