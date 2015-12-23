# votes

This sample application simulates a vote exchange mechanism with several services. This is part of a student project in ESAIP school.

## Components

 - 2 sender applications developed for specific devices to vote: **Leap motion** and **Kinect**. It allows simple and natural vote with movement detection.
 - A **server API** (REST) to manage and store the elections data.
 - A **Windows Phone application** to retrieve and display the data.


**Sender Application**  -->  **Server API**  -->  **.NET API**  -->  **Mobile Application**


## Architecture

There is a folder and a readme file for each component of the application. 
Each part is independent and can be updated on its own.
Sender and mobile applications are using .NET and C# language.

There are 4 API implementation:

- **api-core.net** : version written in Core .NET (1.0.0-beta4).
- **api-python** : version written in Python (Flask framework).
- **api-java** : version written in Java 8 (Spark framework).
- **api-node.js** : version written in Node.JS with Express.

## Use of the APIs
The APIs can be runned using Docker.
Easiest way to run the application is to use the docker-compose.yml file that is provided :

    git clone https://github.com/antoine-f/votes.git
    cd votes
    docker-compose build
    docker-compose up

### REST API documentation

The API exposes four routes under the /api/votes base:

- **/Elections (GET)** : returns all elections;
- **/Elections/{id} (GET)** : returns a given election;
- **/Elections/{id} (PUT)** : creates an election (idempotent) - election are created without any votes;
- **/Elections/{id}/Votes (POST)** : sends a vote.

The JSON basic format is the following for one election:
> {
>   "id" : "BDE",
>   "votes" : [
>      { choix : 1, prenom : "Corentin" },
>      { choix : 1, prenom : "Antoine" }
>   ]
> }

## How to run it

Set up one of the API on your server or locally. (using Docker)
Then run the Kinect or Leap application and follow the voting instruction. 
Once votes are recorded, use the mobile application to retrieve the results for the election.