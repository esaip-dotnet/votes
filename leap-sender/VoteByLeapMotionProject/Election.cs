using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using System.Threading.Tasks;

namespace VoteByLeapMotionProject
{
    [DataContract]
    public class Election
    {
        [DataMember]
        public string nom { get; set; }
        [DataMember]
        public Dictionary<int, Choix> choix { get; set; }
        [DataMember]
        public List<Vote> votes { get; set; }

        public Election(string nom)
        {
            this.nom = nom;
            choix = new Dictionary<int, Choix>();
            votes = new List<Vote>();
        }
        public override String ToString()
        {
            return nom;
        }
    }
}
