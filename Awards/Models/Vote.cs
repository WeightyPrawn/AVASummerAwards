using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Awards.Models
{
    public class Vote
    {
        public int ID { get; set; }
        public int NomineeID { get; set; }
        public string Voter { get; set; }
    }
}