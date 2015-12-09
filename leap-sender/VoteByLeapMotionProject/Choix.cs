using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Collections.Generic;
namespace VoteByLeapMotionProject
{
    class Election
    {
        private String nom;
        private Dictionary<int, Choix> listeChoix;
        private List<Vote> listeVote;

        public Election(String nom)
        {
            this.nom = nom;
            listeChoix = new Dictionary<int, Choix>();
            listeVote = new List<Vote>();
        }

        public void setNom(String nom)
        {
            this.nom = nom;
        }

        public String getNom()
        {
            return nom;
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
    class Choix
    {
        private String nom;
        private int nombreDeMainPourChoix, id;

        public Choix(int id, String nom, int nombreDeMainPourChoix)
        {
            this.id = id;
            this.nombreDeMainPourChoix = nombreDeMainPourChoix;
            this.nom = nom;
        }

        public void setId(int id)
        {
            this.id = id;
        }

        public int getId()
        {
            return id;
        }

        public void setNom(String nom)
        {
            this.nom = nom;
        }

        public String getNom()
        {
            return nom;
        }

        public void setNombreDeMainPourChoix(int nombreDeMainPourChoix)
        {
            this.nombreDeMainPourChoix = nombreDeMainPourChoix;
        }

        public int getNombreDeMainPourChoix()
        {
            return nombreDeMainPourChoix;
        }
    }

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
