# api-core.net
This is the Python version of the voting API. it uses flask web framework.

## Contract
The API exposes four operations under the /api/votes base:

- **/Elections (GET)** : returns all elections;
- **/Elections/{id} (GET)** : returns a given election;
- **/Elections/{id} (PUT)** : creates an election (idempotent) - election are created without any votes;
- **/Elections/{id}/Votes (POST)** : sends a vote.

## Content
The JSON basic format is the following for an election:
> {
>   "id" : "BDE",
>   "votes" : [
>      { choix : 1, prenom : "Corentin" },
>      { choix : 1, prenom : "Antoine" }
>   ]
> }
