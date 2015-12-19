using System;
using System.Collections.Generic;
using System.Text;
using ElectionsMobile.Models;


namespace ElectionsMobile.Models
{   
    //Declaration of the classe ElectionDetails
    public class ElectionDetails
    {
        public String id { get; set; }
        
        public List<VoteDetails> VoteDetails { get; set; }    
    }
}
