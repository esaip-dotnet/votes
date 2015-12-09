from flask import Flask, jsonify
from flask import make_response
from flask import request
from reportlab.pdfgen import canvas

import requests, json

app = Flask(__name__)

elections = [
        {

            "id":"BDE",
            "votes":
            [
                {
                    "choix":1,
                    "prenom":"JP"
                }
            ]
        }
]

@app.route('/Elections', methods=['GET'])
def api_elections():
    return jsonify({'elections':elections})

@app.route('/Elections/<electionId>', methods=['GET'])
def api_election(electionId):
    election = [election for election in elections if election['id']== electionId]
    if len(election) == 0:
        abort(404)
    return jsonify({'election':election[0]})



@app.route('/Elections/<electionId>/votes', methods=['POST'])
def api_createVote():
    if not request.json or not 'choix' in request.json
        abort(404)
    vote = {
        #'id': elections[-1]['id'] +1,
                'choix': request.json['choix']
                'prenom': request.json['prenom',""]
            }
        
    
    elections.append(vote)
    return jsonify({'vote':vote}),201

if __name__ == '__main__':
    app.run(host='0.0.0.0')
