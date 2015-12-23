using Microsoft.AspNet.Mvc;
using System.Collections.Generic;
using System.Linq;

namespace VotesAPI
{
    [Route("api/[controller]")]
	/*define a static election with rough election already set*/ 
    public class VotesController : Controller
    {   
        private static List<Election> elections = new List<Election>() 
		{ 
			new Election() 
			{ 
				id = "BDE"; 
				votes = new List<Vote>()
				{ 
					new Vote() 
					{ 
						choix = 1; 
						prenom = "JP"; 
					} 
				} 
			} 
		}
		/*
		HTTP get, post, and put methods
			where Get --> returns all votes & all votes from a specific id
				  Post --> sends a vote
				  put --> return a sample vote without nothing in it*/
        
        [HttpGet("Elections")]
        public IEnumerable<Election> Elections()
        {
            return elections.AsEnumerable<Election>();
        }
		
        [HttpGet("Elections/{id}")]
        public Election Elections(string id)
        {
            return elections.Find(e => e.id == id);
        }
	
	// Idempotency
	[HttpPut("Elections/{id}")]
        public void Put(string id, [FromBody] Election value)
        {
	    elections.RemoveAll(e => e.id == id);
	    elections.Add(value);
        }
	
	[HttpPost("Elections/{id}/Votes")]
        public void Post(string id, [FromBody] Vote value)
        {
	    elections.Find(e => e.id == id).votes.Add(value);
        }
    }
}
