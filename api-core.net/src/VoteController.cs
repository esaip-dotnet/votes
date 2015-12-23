using Microsoft.AspNet.Mvc;
using System;
using System.Collections.Generic;
using System.Text;

namespace VotesAPI
{
	[Route("api/[controller]")]
	//api vote with a duplication control system
	public class VoteController : Controller
	{
		//dictionary of string and Election class to be used
		private static Dictionary<string,Election> elections = new Dictionary<string, Election>();
		
		/*
		HTTP get, post, and put methods
		where Get --> returns all votes & all votes from a specific id
		Post --> sends a vote
	    put --> return a sample vote without nothing in it*/

		[HttpGet("Init")]
		public string pageInit(){
			Election election = new Election("BDE", new List<Vote>(new Vote(1, "Corentin")));
			elections.Add(election.id, election);
			return "Elections initialized";
		}
		
		[HttpGet("Elections")]
		public string pageElections(){
			return generateElectionsJson(elections);
		}
		[HttpGet("Elections/{id}")]
		public string pageElection(string id){
			return generateForSpecificElectionJson(elections, id);
		}
		[HttpPut("Elections/{id}")]
		public string pageAjoutElection(string id, [FromBody] Election value){
			if(elections.ContainsKey(id)){ 
				elections[id] = value;
			}
			else{
				elections.Add(value.id, value);
			}
			return "coucou";
		}
		[HttpPost("Election/{id}/Votes")]
		public string pageAjoutVote(string id,[FromBody] Election value){
			if (elections.ContainsKey(id)){
                elections[id].votes.AddRange(value.votes);
            }
			return "coucou";	
		}
		
		//construction of the Json and it will be called with HTTP methods
        public string generateForSpecificElectionJson(Dictionary<string, Election> elections, String idElection)
        {
			string json = "[";
			StringBuilder error = new StringBuilder();
			StringBuilder generate = new StringBuilder();
			string messageError = "{'erreur':'Pas d id correspondant dans le système.'}";
			if(elections.ContainsKey(idElection)){
				json = generate.Append(generateJSon(elections[idElection]));
			}
			else{
                json = error.Append(messageError);
			}
            return json;
        }
		
		//construction of the Json string to be used in the Specific Election JSON
		public string generateElectionsJson(Dictionary<string,Election> elections)
        {
			StringBuilder comma = new StringBuilder();
			string json = "[";
			int  compteurElection= 0;
			foreach (Election election in elections.Values){
				json+=generateJSon(election);
				if(compteurElection+1<elections.Count){
                    json = comma.Append(",");
				}
				compteurElection++;
			}
            json = comma.Append("]");
            return json;
        }
		//construction of the Json to be used in the Election Json and Specific Election Json
		public string generateJSon(Election election)
        {
			StringBuilder comma2 = new StringBuilder();
            string json = "{'id':'"+election.id+"','votes':[";
            for (int i = 0; i < election.votes.Count; i++)
            {
                Vote vote = election.votes[i];
                json += "{'choix':'" + vote.choix + "', 'prenom':'" + vote.prenom + "'}";
                if(i+1<election.votes.Count){
                    json = comma2.Append(",");
				}
            }
            json= comma2.Append("]}");
            return json;
        }
    }
}