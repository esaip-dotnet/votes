var express  = require('express');
var app = express();
var port = process.env.PORT || 5004;
var http = require('http');
var path = require('path');

var morgan = require('morgan');
var bodyParser = require('body-parser');
var methodOverride = require('method-override');
var cookieParser = require('cookie-parser');

var passport = require('passport');
var util = require('util');
var GoogleStrategy = require('passport-google-oauth').OAuth2Strategy;

app.use(morgan('dev'));
app.use(methodOverride('X-HTTP-Method-Override'));
app.use(bodyParser.json());

//Passport & oauth utilities
app.use(cookieParser());
app.use(require('express-session')({
    secret: 'keyboard cat',
    resave: false,
    saveUninitialized: false
}));
app.use(passport.initialize());
app.use(passport.session());

//Json file and utilities
var fs = require('fs');
var JsonPath = "./data.json";
var jsonObj = require(JsonPath);

//To manage Competition on post & put request
var OperationLock = false;



var GOOGLE_CLIENT_ID = "--insert your google client id here--";
var GOOGLE_CLIENT_SECRET = "--insert your google client secret here--";

//Passport session setup.
passport.serializeUser(function(user, done) {
  done(null, user);
});
passport.deserializeUser(function(obj, done) {
  done(null, obj);
});

//Use the GoogleStrategy.
passport.use(new GoogleStrategy({
    clientID: GOOGLE_CLIENT_ID,
    clientSecret: GOOGLE_CLIENT_SECRET,
    callbackURL: "--insert your google callback here--"
  },
  function(accessToken, refreshToken, profile, done) {
    process.nextTick(function () {
      return done(null, profile);
    });
  }
));

//Use passport.authenticate() as route middleware to authenticate the request.
app.get('/', 
        passport.authenticate('google', { scope: ['https://www.googleapis.com/auth/plus.login'] }),
        function(req, res){
});
app.get('/auth/google', 
        passport.authenticate('google', { scope: ['https://www.googleapis.com/auth/plus.login'] }),
        function(req, res){
});

app.get('/auth/google/callback', 
  passport.authenticate('google', { failureRedirect: '/login' }),
  function(req, res) {
    //redirect to your soft
    res.redirect('/api/Votes/Elections');
  });

//To ensure that the user is authentificated.
function ensureAuthenticated(req, res, next) {
  if (req.isAuthenticated()) { return next(); }
  res.redirect('/auth/google');
}


//Show all elections, saved on the Json file
app.get('/api/Votes/Elections', function(req, res) {
    if (req.isAuthenticated()){
        res.contentType('application/json');
        res.status(200).json(jsonObj);
    }
    else{
        res.status(401).send("Unauthorized");
    }
});

//Define id as param for all routes.
app.param('id', function (req, res, next, id) {
	next();
});

//Show all vote on @param:id election.
app.get('/api/Votes/Elections/:id', function(req, res) {
    if (req.isAuthenticated()){
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
    }
    else{
        res.status(401).send("Unauthorized");
    }
});


//Delete all votes in the @param:id election.
app.put('/api/Votes/Elections/:id', function(req, res) {
    if (req.isAuthenticated()){
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
    }
    else{
        res.status(401).send("Unauthorized");
    }
});

//Add a new vote in the @param:id Election.
app.post('/api/Votes/Elections/:id/Votes', function(req, res) {
    if (req.isAuthenticated()){
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
    }
    else{
        res.status(401).send("Unauthorized");
    }
});

//Redirection for delete method (error 405).
app.delete('*',function(req,res){
    res.status(405).send('Method not allowed!');
});

//Redirection for wrong URL (error 404).
app.all('*', function(req, res){
    res.status(400).send('This URL does not exist!');
});

//Listening on port 5004.
app.listen(port);
console.log("App listening on port " + port);
