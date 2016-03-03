from flask import Flask,jsonify
from flask import make_response
from flask import Response
from flask import request
from reportlab.pdfgen import canvas

import requests, json

app = Flask(__name__)

elections = [
        {"id":"BDE",
             "votes":
            [
                {
                    "choix":1,
                    "prenom":"JP"
                },
                {
                    "choix":2,
                    "prenom":"Antoine"
                }
            ]
         },
         {   
         "id":"BDI",
             "votes":
            [
                {
                    "choix":7,
                    "prenom":"Martial"
                },
                {
                    "choix":2,
                    "prenom":"Zara"
                }
            ]     

        }
      
]

@app.route('/api/Votes/Elections', methods=['GET'])
def api_elections():
    return Response(json.dumps(elections),  mimetype='application/json')

@app.route('/api/Votes/Elections/<electionId>', methods=['GET'])
def api_election(electionId):
    election = [election for election in elections if election['id']== electionId]
    if len(election) == 0:
        abort(404)
    return Response(json.dumps(election[0]),mimetype='application/json')


@app.route('/api/Votes/Elections/<electionId>', methods= ['PUT'])
def create_election():
    return Response(json.dumps(), mimetype='application/json')


@app.route('/api/Votes/Elections/<electionId>/Votes', methods=['POST'])
def api_createVote(vote):
    if not request.json or not 'choix' in request.json:
        abort(400)
    vote.append(request.json)
    return Response(json.dumps(vote), mimetype='application/json'),201

if __name__ == '__main__':
    app.run(host='0.0.0.0')
