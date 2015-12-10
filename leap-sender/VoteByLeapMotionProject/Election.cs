using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace VoteByLeapMotionProject
{
    class Election
    {
        public string nom { get; set; }
        public Dictionary<int, Choix> choix { get; set; }
        public List<Vote> votes { get; set; }

        public Election(string nom)
        {
            this.nom = nom;
            choix = new Dictionary<int, Choix>();
            votes = new List<Vote>();
        }
    }
}
