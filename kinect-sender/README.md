# kinect-sender

Kinect Vote application.

This part of the project is the application to vote with the Kinect. You can vote "Yes" or "No" with just one hand. The result can be sent to the server through an API.

## Vote

Choose the voter's name.
Use your hands to vote: right hand is Yes and left is No. The vote will be valid if the hand is above the shoulder for 5 seconds at least. If not the vote won't be valid and you need to vote again.

## Structure

You can find the application window in MainWindow.xaml and the application itself in the associated class MainWindow.xaml.cs
