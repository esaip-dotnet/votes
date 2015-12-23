# LeapMotion Appliclation for Vote System
This sample application simulates a vote system with the leap motion system as way to take a choice. The vote system can receive election from The API server, 
and send it each choice done by a user. This system support multi-election, need an improvement to be able to limit the vote to authorized people (from the google API for example).

## Architecture
This application contains two windows, one for the election choice and the connection, and the other to allows the user to vote.
The first one need an internet connection to work well. If it's not the case, it will appear a message, and a basic election will be done (BDE).
The second window will show instructions too let the user make is choice. After the user votes, the window disappear and the same person can vote for another election, 
change is choice by vote again for the same election (supported by the API), or let someone else go to vote. 
## Use
Connect your leap motion to your computer.
If it does not have any red light turned on on the leap, please download the leap motion driver. 
If the three red light are turned on on it, be sure that the service is running (windows 10 instruction : ctrl + alt +del, open the task manager, click on the service "onglet" and look if the leap motion service is turned on).
To open the project you need at least virtual studio with .net-framework 4.0 installed on your computer. 

Get the project from github: you can download it on github.com/ezria/votes and download the project on zip archive
Or download it directly by git
    git clone https://github.com/ezria/votes.git

The leap motion project is inside the folder votes/leap-sender.
To launch directly the project, open the VoteByLeapMotionProject.sln
To see the source codes, go on the VoteByLeapMotionProject folder, and you will see all the C# classes.
You can modify some value inside the App.config file allowing you to change the different choices possible, the delay that someone as to let his hands in the front of the leap motion to valid is vote, 
the url of the API server, and the PATH to send a vote.

The value you can change or add inside the project are inside the beacons appSettings.
The line with the key ClientSettingsProvider.ServiceUri should not be modified and let in state.

Modification of the web API URL: 
You just have to modify the value field of the line with the key "urlServer". The URL have to point where it has the JSON file of all the elections. On the native API, it is "http://yourserver.com/api/votes/Elections"

Modification of the URL to send Vote:
To send vote, the actual software is using the urlServer value, followed by the ID of the election, followed by a Word giving an url like it "http://yourserver.com/api/votes/Elections/BDE/Votes"
You can modify this value changing the value field

Modify or add choices
The actual project support only election with two different choices for the vote system alone, but if it doesnt have any confidential issue with the vote, you go to 4 different choices.
The pattern of the value field for the line with key "Choix" is : "Choice1,Choice2,Choice3,Choice4" without any space and seperated by a ","
For each choice you have, you have to create a new line, having as key the name of the choice and has value the number of hand.
    <add key="Choice1" value="1" />
    <add key="Choice2" value="2" />
    <add key="Choice3" value="3" />
    <add key="Choice4" value="4" />
BeCareful to have only numbers in the field value.
WARNING: if your election has an issue with the confidentiality of the choice, you can have only two choices or changing the project.

Change the Delay to valid a vote:
You just have to change the number of the line with key "Delay", accepting only numbers.

WARNING : Do not modify anything else in this file, risk of crash.