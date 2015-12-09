using Microsoft.AspNet.Mvc;
using System;

namespace Project_NET
{
	[Route("api/[controller]")]
	public class VotesController : Controller
	{
		private static Dictionary<string,Election> elections = new Dictionary<string, Election>();

		[HttpGet("elections")]
		public string pageElections(){
			if(elections.Values.Count==0){
				Election election = new Election("BDE", new List<Vote>(new Vote(1, "Corentin")));
				elections.Add(election.getId(), election);
			}
			return generateElectionsJson(elections);
		}
		[HttpGet("elections/{id}")]
		public string pageElection(string id){
			return generateForSpecificElectionJson(elections, id);
		}
		[HttpPut("elections/{id}")]
		public string pageAjoutElection(string id, [FromBody] String value){
			Election electionTemp  = JsonConvert.DeserializeObject(value);
			if(elections.ContainsKey(idElection)){
				
			}
		}
		[HttpPost("election/{id}/Votes")]
		public string pageAjoutVote(){
				
		}
		
		public string generateForSpecificElectionJson(List<Election> elections, String idElection)
        {
			string json = "[";
			if(elections.ContainsKey(idElection)){
				json+= generateElectionJSon(elections.Values[idElection]);
			}
			else{
                json += "{'erreur':'Pas d id correspondant dans le système.'}";
			}
            return json;
        }
		
		public string generateElectionsJson(List<Election> elections)
        {
			string json = "[";
			int  compteurElection= 0;
			foreach (Election election in elections.Values){
				json+=generateElectionJSon(election);
				if(compteurElection+1<elections.Count){
					json += ","
				}
				compteurElection++;
			}
            retour += "]";
            return json;
        }
		
		public string generateElectionJSon(Election election)
        {
            string json = "{'id':'"+election.getNom()+"','votes':[";
            for (int i = 0; i < election.getListeVote().Count; i++)
            {
                Vote vote = election.getListeVote()[i];
                json += "{'choix':'" + vote.getChoixFait() + "', 'prenom':'" + vote.getPrenom() + "'}";
                if(i+1<election.getListeVote().Count){
					json += ","
				}
            }
            json += "]}";
            return json;
        }
        /// <summary>
        /// Fonction permettant la transformation des objets liés à l'élection en chaine de caractère au format XML.
        /// </summary>
        /// <param name="election"> Objet Election contenant les autres objets Vote et Choix</param>
        /// <returns>Retourne la chaine de caractère au format XML</returns>
        public string generateXML(Election election)
        {
            string xml = "<Election id:\"" + election.getNom() + "\">";
            foreach(Vote vote in election.getListeVote()){
                xml += "<Vote choix:\"" + vote.getChoixFait().getId() + "\" prenom:\"" + vote.getPrenom() + "\"/>";
            }
            xml += "</Election>";
            return xml;
        }

	}
}