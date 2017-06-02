using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Awards.Models
{
    public class SetNomineeDTO
    {
        public int CategoryID { get; set; }
        public string NomineeEmail { get; set; }
        public string NomineeName { get; set; }
        public string NoineeImage { get; set; }
        public SetNominationDTO Nomination { get; set; }
    }
}