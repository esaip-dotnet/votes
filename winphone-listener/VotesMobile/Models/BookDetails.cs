using System;
using System.Collections.Generic;
using System.Text;


namespace VotesMobile.Models
{
    public class Elections
    {
        public String id { get; set; }
        public List<votes> votes { get; set; }
    }

    public class votes
    {
        public int choix { get; set; }
        public String prenom { get; set; }
    }
}
