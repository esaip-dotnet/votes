using System.Collections.Generic;

public class Election
{
	public string id { get; set; }
	public List<Vote> votes { get; set; }
	
	//constructed votes is a list of Vote, it represents one class
	public Election()
	{
		votes = new List<Vote>();
	}
}
