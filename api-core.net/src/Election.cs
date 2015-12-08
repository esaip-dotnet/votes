using System.Collections.Generic;

public class Vote
{
	public int choix { get; set; }
	public string prenom { get; set; }	
}

public class Election
{
	public string id { get; set; }
	public List<Vote> votes { get; set; }
	public Election()
	{
		votes = new List<Vote>();
	}
}