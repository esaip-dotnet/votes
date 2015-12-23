# Votes with the Node.js API

## Architecture

This API is developped with Node.js.


## Use

First of all, the authentication must be done before the API receives or transmits any data.

The Node API allows the user to get data sent by the Leap and the Kinect, and offers an access to these data with the Windows Phone app.


## Authentication

//Dans un premier temps vous aurait besoin de gérer votre API dev google et de renseigner les informations manquantes dans le code.

Authentication uses the Google OAuth2 API. Code samples have to be inserted into your app code . More informations here : 
    https://developers.google.com/identity/protocols/OAuth2
    https://github.com/jaredhanson/passport-google-oauth/
	
Before the authentication can work properly, the problem with the data sending by Kinect & Leap Motion has to be fixed. Without this upgrade, the authentication cannot work.


## Redirection

With POST and PUT request formatted with JSON, the elections and the votes are recorded in a data.json file.

Les informations enregistrées dans le data.json sont alors renvoyées dans une URL get.
Data stored in the data.json file are sent with the GET method to be accessed.

The API can manage the competition in a minimalistic way (a simple boolean). The API forbids the access when 2 or more POST and/or PUT resquests are happening in the same time (one will be accepted, the other ones will be discarded).

Les gestions d'exception et de traitement http sont également gérés.
HTTP treatments and exceptions are also managed by the API.


## Contract

The API exposes four operations under the /api/votes base:

- **/Elections (GET)** : returns all elections;
- **/Elections/{id} (GET)** : returns a given election;
- **/Elections/{id} (PUT)** : creates an election (idempotent) - election should be created without votes;
- **/Elections/{id}/Votes (POST)** : sends a vote.