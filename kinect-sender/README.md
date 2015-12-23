# Votes with Kinect for Windows 

This application allows the user to vote "yes" or "no" in the context of the vote project by using the Kinect for Windows device
It proposes a GUI to guide the user with the vote process in a simple and efficient way.


## Architecture

The project is developped with Visual C#, the Kinect SDK and libraries and the .NET  4.0 framework.


## Use

Connect your Kinect device to your computer.

The right hand stands for "Yes", the left hand stands for "No".

The process of vote is simple and offers some features to avoid errors during the use.

Once the election is done, the data is formatted in JSON and sent to the API.

Before voting, we need to enter voter's name. Then, voter need to face the kinect in order to vote.
The right hand is to vote "Yes", the left hand is to vote "No".
To validate this vote, voter need to block his left or right hand during 3 seconds above his shoulder. If he lowers his hand before 3 seconds, the vote won't be taken into account.


## Vote process

To be validated, a vote have to respect some prerequisites : 
	- The user have to lift his hand (right for "yes", left for "no") above his shoulder.
	- He must keep his hand lifted during 5 seconds to terminate his vote
	- If the user drop his arm, the vote is cancelled and he has to lift his arm to restart a vote
