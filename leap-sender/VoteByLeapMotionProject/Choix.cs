using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
namespace VoteByLeapMotionProject
{
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
}
