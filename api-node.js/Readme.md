# api-node.js
This is the Node API version of the voting application

## Contract
The API exposes four operations under the localhost/api/votes url, those are:

- **/Elections (GET) : returns all elections;
- **/Elections/{id} (GET) : returns a specified election by the invoqued id ;
- **/Elections/{id} (PUT) : creates an election (idempotent) - election should be created without votes;
- **/Elections/{id}/Votes (POST) : sends a vote.

## Content
The JSON basic form is the following :
> {
>   "id" : "BDE",
>   'votes': [
>      {
>       'choix': 1,
>        'prenom': 'Adel'
>      },
 >     {
 >       'choix': 2,
 >       'prenom': 'Quentin'
 >     },
 >     {
 >       'choix': 2,
 >       'prenom': 'Fadwa'
 >     }]
> }

# Launch the api with docker:
firstly, you must install docker on your computer

==>In folder api-core.net 
==>You must have administrator privileges for the following commands
==>docker build -t test . 
==>docker run -d -p 80:5004 --name Test test 
==>docker logs Test
==>docker rm -fv `docker ps -aq` 

# Explanation docker command lines
- The first command permit to build the image test. 
- The second allows to run the api over the 5004 port with a container name 'Test'
- the third command line permit to debug if the second command line didn't work correctly. 
- With the last command line, we allows to remove all the images already built.

Done By Adel BENAMOR, student of ESAIP school