# api-core.net
This Core .NET version of the voting API.

## Contract
The API exposes four operations under the /api/votes base:

- **/Elections (GET)** : returns all elections;
- **/Elections/{id} (GET)** : returns a given election;
- **/Elections/{id} (PUT)** : creates an election (idempotent) - election should be created without votes;
- **/Elections/{id}/Votes (POST)** : sends a vote.

## Content
The JSON basic form is the following for an exception:
> {
>   "id" : "BDE",
>   "votes" : [
>      { choix : 1, prenom : "Corentin" },
>      { choix : 1, prenom : "Antoine" }
>   ]
> }