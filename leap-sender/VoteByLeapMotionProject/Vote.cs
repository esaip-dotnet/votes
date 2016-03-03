using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace VoteByLeapMotionProject
{
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