using System;
using System.Collections.Generic;
using System.Text;


namespace BiblioMobile.Models
{
    public class ElectionDetails
    {
        public String id { get; set; }
        public List<VoteDetails> VoteDetails { get; set; }
    }
}
