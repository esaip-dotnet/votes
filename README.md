# sqlinjection
This sample application shows an SQL injection in MySQL from a Python web application

## Architecture
The application is composed with two services:

- **injector** : the web application that causes an SQL injection;
- **mysql** : the database used by the application.

## Use

Easiest way to run the application is to use the docker-compose.yml file that is provided :

    git clone https://github.com/ensibs-cybersec/sqlinjection.git
    cd sqlinjection
    docker-compose build
    docker-compose up

The application is then available under 
> http://localhost:8080

Start by initializing the database.

Then, in the text area, try the following values, and click on the second button:
- **Lagaffe**: this should bring you one result, namely Gaston Lagaffe;
- **Lagaffe' OR '1'='1**: this should bring all results in the database table (2 rows);
- **Lagaffe'; DROP TABLE individuals; --**: this will destroy the database table, so that every later request will fail.

## Notes

An SQL injection is a security breach. This application serves only the academic purpose of explaining such a code failure.
It should of course never be used for anything different.
