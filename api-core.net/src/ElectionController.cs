using Microsoft.AspNet.Mvc;
using System;
using System.Collections.Generic;

namespace VotesAPI
{
	[Route("api/[controller]")]
	public class VoteController : Controller
	{
		private static Dictionary<string,Election> elections = new Dictionary<string, Election>();

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

        public string generateForSpecificElectionJson(Dictionary<string, Election> elections, String idElection)
        {
			string json = "[";
			if(elections.ContainsKey(idElection)){
				json+= generateElectionJSon(elections[idElection]);
			}
			else{
                json += "{'erreur':'Pas d id correspondant dans le système.'}";
			}
            return json;
        }
		
		public string generateElectionsJson(Dictionary<string,Election> elections)
        {
			string json = "[";
			int  compteurElection= 0;
			foreach (Election election in elections.Values){
				json+=generateElectionJSon(election);
				if(compteurElection+1<elections.Count){
                    json += ",";
				}
				compteurElection++;
			}
            json += "]";
            return json;
        }
		
		public string generateElectionJSon(Election election)
        {
            string json = "{'id':'"+election.id+"','votes':[";
            for (int i = 0; i < election.votes.Count; i++)
            {
                Vote vote = election.votes[i];
                json += "{'choix':'" + vote.choix + "', 'prenom':'" + vote.prenom + "'}";
                if(i+1<election.votes.Count){
                    json += ",";
				}
            }
            json += "]}";
            return json;
        }
    }
}
