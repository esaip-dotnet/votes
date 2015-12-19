# votes
This sample application simulates a vote exchange mechanism with several services. The API receives, stores in memory and delivers
votes on given elections. A Windows Phone mobile application retrieves the number of votes. Two different applications are
provided to send votes, both using natural interfaces. The first one uses a Kinect peripheral, and the second one a Leap Motion.

## Architecture
This API 

## Use
Authentification

Premièrement il faut gérer l'identification avant que l'API puisse recevoir/transmettre de quelconques données.

## Authentification

L'authentification utilise un système oauth2 de google. Dans un premier temps vous aurait besoin de gérer votre API dev google et de renseigner les informations manquantes dans le code.
Pour toutes difficultés dans cette partie voir : 
    https://developers.google.com/identity/protocols/OAuth2
    https://github.com/jaredhanson/passport-google-oauth/
    
Avant que l'authentification puisse marcher, il faut régler le soucis lier à l'envoie via la kinnect et la leap motion. Sans cette amélioration ajoutée, l'authentification ne peut marcher.


## Redirection

L'API sous node permet de récupérer les informations envoyées par la Leap et la Kinnect et d'offrir un accès à la donnée à l'application sous windows phone.

Via des requêtes post et put de données sous json, les élections et votes sont enregistrées dans un fichier data.json.

Les informations enregistrées dans le data.json sont alors renvoyées dans une URL get.

L'API gère de manière minimale la concurrence (via un booléen). Il n'authorise pas une requête post ou put de se faire au même moment.
Les gestions d'exception et de traitement http sont également gérés.