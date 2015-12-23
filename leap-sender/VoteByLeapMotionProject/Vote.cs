using System;
using System.Runtime.Serialization;
using System.Text;

namespace VoteByLeapMotionProject
{
    [DataContract]
    public class Vote
    {   
        [DataMember]
        public string prenom { get; set; }
        [DataMember]
        public int choix { get; set; }

        public Vote(string prenom, int choix)
        {
            this.prenom = prenom;
            this.choix = choix;
        }
    }
}
