using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using System.Threading.Tasks;

namespace VoteByLeapMotionProject
{
    [DataContract(Name = "Election", Namespace = "VoteByLeapMotionProject")]
    public class Election
    {
        public string nom { get; set; }
        public List<Choix> choix { get; set; }
        public List<Vote> votes { get; set; }

        public Election(string nom)
        {
            this.nom = nom;
            choix = new List<Choix>();
            votes = new List<Vote>();
        }
        public override String ToString()
        {
            return nom;
        }
    }
}
