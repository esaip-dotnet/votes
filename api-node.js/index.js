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

//json
var fs = require('fs');
var JsonPath = "./data.json";
var jsonObj = require(JsonPath);


app.get('/api/Votes/Elections', function(req, res) {
	
	res.contentType('application/json');
	res.status(200).json(jsonObj);
});

app.param('id', function (req, res, next, id) {
	console.log('Id called in the URL.');
	next();
});

app.get('/api/Votes/Elections/:id', function(req, res) {
	var election = '';
    for(var i in jsonObj) {
      if(jsonObj[i].id === req.params.id){
        election = jsonObj[i];
      }
    }
	if(election === ""){
		res.status(404).send("This election does not exist!");
	}else{
		res.contentType('application/json');
		res.status(200).json(election);
	}
});

app.put('/api/Votes/Elections/:id', function(req, res) {
	console.log(req.params);
    var VerifExistence = false;
    for(var i in jsonObj) {
        if(jsonObj[i].id === req.params.id){
          VerifExistence = true;
          jsonObj[i].votes = [];
        }
      }
    if(VerifExistence === true){
        res.status(200).send(jsonObj);
    }
    else{
      var cpt=0;
      for(var i in jsonObj){
          cpt++;
      }
      jsonObj[cpt] = {id: req.params.id, votes:[]};
      /*
      var election = {id: req.params.id, votes:[]};
      elections.push(election);
      */
      fs.writeFile(JsonPath, JSON.stringify(jsonObj), function(err) {
        if(err) {
          console.log(err);
        }
        else {
          console.log("JSON saved to " + JsonPath);
        }
      });
      res.status(201).send(jsonObj);
    }
});

app.post('/api/Votes/Elections/:id/Votes', function(req, res) {
  var election = '';
      console.log("id", req.params.id);
      console.log(req.body);
      for(var i in jsonObj){
          if(jsonObj[i].id === req.params.id){
            var cpt = 0;
            for(var j in jsonObj[i].votes){
              cpt++;
            }
            jsonObj[i].votes[cpt] = req.body;
            election = jsonObj[i];
            fs.writeFile(JsonPath, JSON.stringify(jsonObj), function(err) {
              if(err) {
                console.log(err);
              }
              else {
                console.log("JSON saved to " + JsonPath);
              }
            });
          }
      }      
      if(election === ""){
          res.status(404).send("This election does not exist!");
      }else{
          res.status(201).json(election);
      }
});

app.delete('*',function(req,res){
  res.status(405).send('Method not allowed!');
});

app.all('*', function(req, res){
   res.status(400).send('This URL does not exist!');
});

app.listen(port);
console.log("App listening on port " + port);
