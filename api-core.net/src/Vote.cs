using System.Collections.Generic;

public class Vote
{
	public int choix { get; set; }
	public string prenom { get; set; }	

    public Vote(int choix, string prenom){
		this.choix = choix;
		this.prenom = prenom;
	}
}

