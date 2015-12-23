using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;
namespace Service.Models
{
    public class BookDetails
    {
        [Required]
        public String id { get; set; }
        [Required]
        public List<votes> votes { get; set; }
    }

    public class votes
    {   [Required]
        public int choix { get; set; }
        public String prenom { get; set; }
    }
}
