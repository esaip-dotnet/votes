var express  = require('express');
var app = express();
var port = process.env.PORT || 5004;
var http = require('http');

var morgan = require('morgan');
var bodyParser = require('body-parser');
var methodOverride = require('method-override');

app.use(morgan('dev'));
app.use(methodOverride('X-HTTP-Method-Override'));
app.use(bodyParser.json());

//Json file and utilities
var fs = require('fs');
var JsonPath = "./data.json";
var jsonObj = require(JsonPath);

var OperationLock = false;

//Show all elections
app.get('/api/Votes/Elections', function(req, res) {
	res.contentType('application/json');
	res.status(200).json(jsonObj);
});


//
app.param('id', function (req, res, next, id) {
	next();
});

//
app.get('/api/Votes/Elections/:id', function(req, res) {
    var election = '';
    for(var i in jsonObj) {
        if(jsonObj[i].id === req.params.id){
            election = jsonObj[i];
        }
    }
    if(election === ""){
        res.status(404).send("This election does not exist!");
    }
    else{
        res.contentType('application/json');
        res.status(200).json(election);
    }
});


//
app.put('/api/Votes/Elections/:id', function(req, res) {
    if(OperationLock == false){
        OperationLock = true;
        var CheckExistence = false;
        for(var i in jsonObj) {
            if(jsonObj[i].id === req.params.id){
                CheckExistence = true;
                jsonObj[i].votes = [];
            }
        }
        if(CheckExistence === true){
            res.status(200).send(jsonObj);
        }
        else{
            var cptSize=0;
            for(var i in jsonObj){
                cptSize++;
            }
            jsonObj[cptSize] = {id: req.params.id, votes:[]};
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
        OperationLock = false;
    }
    else{
        res.status(409).send("Conflict");
    }
});

//
app.post('/api/Votes/Elections/:id/Votes', function(req, res) {
    if(OperationLock == false){
        OperationLock = true;
        var requestType = req.get('Content-Type');
        if(requestType == 'application/json'){
            var election = '';
            for(var i in jsonObj){
                if(jsonObj[i].id === req.params.id){
                    var cptSize = 0;
                    for(var j in jsonObj[i].votes){
                        cptSize++;
                    }
                    jsonObj[i].votes[cptSize] = req.body;
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
        }
        else{
            res.status(406).send("Header type not acceptable");
        }
        OperationLock = false;
    }
    else{
        res.status(409).send("Conflict");        
    }
    
});

//Redirection for delete method (error 405)
app.delete('*',function(req,res){
    res.status(405).send('Method not allowed!');
});

//Redirection for wrong URL (error 404)
app.all('*', function(req, res){
    res.status(400).send('This URL does not exist!');
});

//Listening on port 5004
app.listen(port);
console.log("App listening on port " + port);
