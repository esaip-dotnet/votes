# Votes 

This sample application simulates a vote exchange mechanism with several services. The API receives, stores in memory and delivers
votes on given elections. A Windows Phone mobile application retrieves the number of votes. Two different applications are
provided to send votes, both using natural interfaces. The first one uses a Kinect peripheral, and the second one a Leap Motion.

## Architecture
The application allows four services for the API implementation:

- **api-core.net** : version written in Core .NET (1.0.0-beta4);
- **api-python** : version written in Python (Flask framework);
- **api-java** : version written in Java 8 (Spark framework);
- **api-node.js** : version written in Node.JS with Express.

The app allows the user to vote with two devices : 
	- Kinect for Windows : the app detects the arm of the user.
	- Leap Motion : the app detects the number of hands.

## Use

The easiest way to run the application is to use the docker-compose.yml file that is provided :

    git clone https://github.com/esaip-dotnet/votes.git
    cd votes
    docker-compose build
    docker-compose up

You can also download the zip achive
	
Then run the Kinect or Leap application and follow the voting instructions. Once votes are recorded, use the mobile application
to retrieve the results for the election.

## Data structure

The JSON basic form is the following for an exception:
> {
>   "id" : "BDE",
>   "votes" : [
>      { choix : 1, prenom : "Corentin" },
>      { choix : 1, prenom : "Antoine" }
>   ]
> }


## Contract

The API exposes four operations under the /api/votes base:

- **/Elections (GET)** : returns all elections;
- **/Elections/{id} (GET)** : returns a given election;
- **/Elections/{id} (PUT)** : creates an election (idempotent) - election should be created without votes;
- **/Elections/{id}/Votes (POST)** : sends a vote.


## Global project structure

See the schema.jpg file in the /resources folder.
It explains in a basic way the different interactions between each block of the program.