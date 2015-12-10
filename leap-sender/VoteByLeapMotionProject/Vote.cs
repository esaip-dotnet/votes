using System;
using System.Text;

namespace VoteByLeapMotionProject
{
    class Vote
    {
        public string prenom { get; set; }
        public Choix choix { get; set; }

        public Vote(string prenom, Choix choix)
        {
            this.prenom = prenom;
            this.choix = choix;
        }
    }
}
