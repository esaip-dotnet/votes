package sparkexample;

import static spark.Spark.*;
import java.io.File;
import java.io.InputStream;
import java.io.FileOutputStream;
import java.io.OutputStreamWriter;
import java.net.URL;
import java.net.HttpURLConnection;

public class votesAPI {


    String data = '
    {
        "id": "BDE2",
        "votes": [
        {
            "choix": 42,
            "prenom": "Adel"
        },
        {
            "choix": 43,
            "prenom": "Quentin"
        },
        {
            "choix": 44,
            "prenom": "Fadwa"
        }
        ]
    }
    ';


    public static void main(String[] args) {
        setPort(5004);

        get("/api/votes/Elections", (request, response) -> {

            response.type("application/json")
            return data;

        }

        get("/api/votes/Elections/:id", (request, response) -> {

            response.type("application/json")
            return data;

        }

        put("//api/votes/Elections/:id", (request, response) -> {

            response.type("application/json")
            return data;

        }

        post("/api/votes/Elections/:id/votes", (request, response) -> {

            response.type("application/json")
            return data;

        }
    }
}
