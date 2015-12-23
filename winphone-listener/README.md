# Votes with Windows Phone

This application allows the user to access the data sent by the API and display it on a Windows Phone.


## Architecture

The application is developped with Visual C#, the .NET 4.0 framework and the Windows Phone SDK (Windows 8/8.1)
The GUI is composed by two XAML files : 
	- MainPage.xaml : this is first page of the app. You can modify its functions by editing the C# file associated (MainPage.xaml.cs).
	- DatailPage.xaml : Contains the details of an election. You can modify its functions by editing the C# file associated (DetailPage.xaml.cs).

The app is structured by two classes, declared in the models folder : 
	- election : contains all the votes about one decision to take
	- vote : The decision of one person about one asked question
	
The AppResources.resx defines the constants of the app (the server URL for example).

The JSON structure is stored in MainViewModel.cs file, it will allow the app to parse the Json and structure it in the GUI.

## Use

The user selects the data he wants to be displayed on the smartphone screen in the menu.
The app joins the API, which will send the correspondant data in JSON.
The app will put the data in the suitable place on the screen.