using System;
using System.Collections.Generic;
using System.Text;
using BiblioMobile.Models;


namespace BiblioMobile.Models
{
    public class ElectionDetails
    {
        public String id { get; set; }
        
        public List<VoteDetails> votes { get; set; }    
    }
}
