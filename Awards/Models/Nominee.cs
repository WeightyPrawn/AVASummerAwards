using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Awards.Models
{
    public class Nominee
    {
        public int ID { get; set; }
        public int CategoryID { get; set; }
        public string Name { get; set; }
        public string Email { get; set; }
        public virtual ICollection<Nomination> Nominations { get; set; }
        public virtual ICollection<Vote> Votes { get; set; }
    }
}