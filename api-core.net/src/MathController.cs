using Microsoft.AspNet.Mvc;
using System.Collections.Generic;
using System.Linq;

namespace VotesAPI
{
    [Route("api/[controller]")]
    public class VotesController : Controller
    {   
        private static List<Election> elections = new List<Election>() { new Election() { id = "BDE", votes = new List<Vote>() { new Vote() { choix = 1, prenom = "JP" } } } };
        
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
		
	[HttpPost("Elections")]
        public void Post([FromBody] Election value)
        {
	    elections.Add(value);
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
