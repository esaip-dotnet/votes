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
#get method, allows to retrieve all the elections
@app.route('/api/votes/Elections', methods=['GET'])
def api_elections():
    return Response(json.dumps(elections),  mimetype='application/json')

#get all the elections specified by an invoqued id
@app.route('/Elections/<electionId>', methods=['GET'])
def api_election(electionId):
    election = [election for election in elections if election['id']== electionId]
    if len(election) == 0:
        return bad_request
    return Response(json.dumps(election[0]),mimetype='application/json')

#create a new specified election by the id
@app.route('/Elections/<electionId>', methods= ['PUT'])
def create_election():
    return Response(json.dumps(), mimetype='application/json')

#alloed to vote
@app.route('/Elections/<electionId>/Votes', methods=['POST'])
def api_createVote():
    if not request.json or not 'choix' in request.json:
        return bad_request
    vote = [vote for vote in elections if vote['vote'] == vote]
    vote.append(request.body)
    return Response(json.dumps(vote), mimetype='application/json'),201

#errors 400 
@app.errorhandler(400)
def bad_request(error=None):
    message = {
            'status': 400,
            'message': 'Not Found: ' + request.url,
    }
    resp = jsonify(message)
    resp.status_code = 400

    return resp


if __name__ == '__main__':
    #it allows the debug of application 
    app.run(debug = True)
