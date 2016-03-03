# votes
This sample application simulates a vote exchange mechanism with several services. The API receives, stores in memory and delivers
votes on given elections. A Windows Phone mobile application retrieves the number of votes. Two different applications are
provided to send votes, both using natural interfaces. The first one uses a Kinect peripheral, and the second one a Leap Motion.

## Architecture
The application allows for four services for the API implementation:

- **api-core.net** : version written in Core .NET (1.0.0-beta4);
- **api-python** : version written in Python (Flask framework);
- **api-java** : version written in Java 8 (Spark framework);
- **api-node.js** : version written in Node.JS with Express.

## Use
Easiest way to run the application is to use the docker-compose.yml file that is provided :

    git clone https://github.com/esaip-dotnet/votes.git
    cd votes
    docker-compose build
    docker-compose up

Then run the Kinect or Leap application and follow the voting instruction. Once votes are recorded, use the mobile application
to retrieve the results for the election.