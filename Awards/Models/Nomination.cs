using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Awards.Models
{
    public class Nomination
    {
        public int ID { get; set; }
        public int NomineeID { get; set; }
        public string Nominator { get; set; }
        public string Reason { get; set; }
        public bool Anonymous { get; set; }
    }
}