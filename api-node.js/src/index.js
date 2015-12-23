var express  = require('express');
var app = express();
var port = process.env.PORT || 5004;
var http = require('http');

var morgan = require('morgan');
var bodyParser = require('body-parser');
var methodOverride = require('method-override');

app.use(morgan('dev'));
app.use(bodyParser.json());
app.use(methodOverride('X-HTTP-Method-Override'));

var elections =[];

app.get('/api/Votes/Elections', function(req, res) {

	res.contentType('application/json');
	res.status(200);
	res.json(elections);
});

app.param('id', function (req, res, next, id) {
	next();
});

app.get('/api/Votes/Elections/:id', function(req, res) {
	var election = '';
	for(i in elections){
		if(elections[i].id === req.params.id){
			election = elections[i];
		}
	}

	if(election === ""){
		res.status(404);
		res.send("404, this election does not exist!");
	}else{
		res.contentType('application/json');
		res.status(200);
		res.json(election);
	}
});

app.put('/api/Votes/Elections/:id', function(req, res) {
	var election = {id: req.params.id, votes:[]};
	elections.push(election);
	res.status(200);
	res.send(elections);
});

app.post('/api/Votes/Elections/:id/Votes', function(req, res) {
	var election = '';
	for(i in elections){
		if(elections[i].id === req.params.id){
			elections[i].votes.push(req.body);
			election = elections[i];

		}
	}
	if(election === ""){
		res.status(404);
		res.send("404, this election does not exist!");
	}else{
		res.status(200)
		res.json(election);
	}
});

app.all('*', function(req, res){
  res.send('400, this URL does not exist!', 400);
});

app.listen(port);
console.log("App listening on port " + port);
