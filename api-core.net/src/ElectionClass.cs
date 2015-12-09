using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Collections.Generic;
namespace Project_NET
{
    class Election
    {
        private String id;
        private List<Vote> Votes;

        public Election(String id)
        {
            this.id = id;
            listeChoix = new Dictionary<int, Choix>();
            listeVote = new List<Vote>();
        }
        public Election(String id, Dictionary<int, Choix> listeChoix, List<Vote> listeVote)
        {
            this.id = id;
            this.listeChoix = listeChoix;
			this.listeVote = listeVote;
		}
        public void setId(String id)
        {
            this.id = id;
        }

        public String getId()
        {
            return id;
        }

        public void setListeChoix(Dictionary<int, Choix> listeChoix)
        {
            this.listeChoix = listeChoix;
        }

        public Dictionary<int, Choix> getListeChoix()
        {
            return listeChoix;
        }

        public void setListeVote(List<Vote> listeVote)
        {
            this.listeVote = listeVote;
        }

        public List<Vote> getListeVote()
        {
            return listeVote;
        }
    }

    class Vote
    {
        private String prenom;
        private int choix;

        public Vote(int choix, String prenom)
        {
            this.prenom = prenom;
            this.choix = choix;
        }

        public void setPrenom(String prenom)
        {
            this.prenom = prenom;
        }

        public String getPrenom()
        {
            return prenom;
        }

        public void setChoix(String choix)
        {
            this.choix = choix;
        }

        public String getChoix()
        {
            return choix;
        }

    }
}
