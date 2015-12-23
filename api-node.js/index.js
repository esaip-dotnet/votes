var express  = require('express');
var app = express();
var port = process.env.PORT || 5004;
var http = require('http');

var morgan = require('morgan');
var bodyParser = require('body-parser');
var methodOverride = require('method-override');

//app.use(express.static(__dirname + '/public'));
app.use(morgan('dev'));
app.use(bodyParser.json()); // parse application/json
app.use(methodOverride('X-HTTP-Method-Override'));

var elections =[];

var e1 = {
    'id': 'BDE',
    'votes': [
      {
        'choix': 1,
        'prenom': 'Adel'
      },
      {
        'choix': 2,
        'prenom': 'Quentin'
      },
      {
        'choix': 2,
        'prenom': 'Fadwa'
      }]
	  };
	  
elections.push(e1);

//HTTP Get method that allows to see all the elections
app.get('/api/Votes/Elections', function(req, res) {
	
	res.contentType('application/json');
	res.status(200);
	res.json(elections);
});

/*add callback triggers to route parameters where 'id' is the name of the parameter and function(req, res, next, id) is the callback function*/ 
app.param('id', function (req, res, next, id) {
	console.log('Id called in the URL.');
	next();
});
//HTTP Get method that allows to see all the elections by a specific id
app.get('/api/Votes/Elections/:id', function(req, res) {
	var election = '';
	console.log("id", req.params.id);
	for(var i in elections){
		console.log(elections[i]);
		if(elections[i].id === req.params.id){
			election = elections[i];
		}
	}
	
	//gestion des erreurs selon le contrat:
	/*404 ---> Not found
	200 ---> Request Succeed*/
	if(election === ""){
		res.status(404);
		res.send("404, this election does not exist!");
	}else{
		res.contentType('application/json');
		res.status(200);
		res.json(election);
	}
});

//Allows to create an elections specified by the invoqued id
app.put('/api/Votes/Elections/:id', function(req, res) {
	console.log(req.params);
	var election = {id: req.params.id, votes:[]};
	elections.push(election);
	res.status(200);
	res.send(elections);
});

//Allows to update a vote
app.post('/api/Votes/Elections/:id/Votes', function(req, res) {
	var election = '';
	console.log("id", req.params.id);
	console.log(req.body);
	for(var i in elections){
		console.log(elections[i]);
		if(elections[i].id === req.params.id){
			elections[i].votes.push(req.body);
			election = elections[i];
			
		}
	}
	//status
	if(election === ""){
		res.status(404);
		res.send("404, this election does not exist!");
	}else{
		res.status(200);
		res.json(election);
	}
});
// Error 400 
app.all('*', function(req, res){
  res.send('400, this URL does not exist!', 400);
});
//port to listen to
app.listen(port);
console.log("App listening on port " + port);
