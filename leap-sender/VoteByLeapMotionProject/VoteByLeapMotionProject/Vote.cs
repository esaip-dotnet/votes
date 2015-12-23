using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace VoteByLeapMotionProject
{
  /// <summary>
  ///  class how permit the creation of object Vote
  ///  It's here then we find setters ans getters of Vote object. 
  /// </summary>
  class Vote
  {
    private String prenom;
    private Choix choixFait;

    public Vote(String prenom, Choix choixFait)
    {
      this.prenom = prenom;
      this.choixFait = choixFait;
    }
    
    public void setPrenom(String prenom)
    {
      this.prenom = prenom;
    }

    public String getPrenom()
    {
      return prenom;
    }
     
    public void setChoixFait(Choix choixFait)
    {
      this.choixFait = choixFait;
    }

    public Choix getChoixFait()
    {
      return choixFait;
    }
  }
}
