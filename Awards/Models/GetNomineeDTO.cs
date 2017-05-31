using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Awards.Models
{
    public class GetNomineeDTO
    {
        public int CategoryID { get; set; }
        public string NomineeEmail { get; set; }
        public string NomineeName { get; set; }
        public IEnumerable<GetNominationDTO> Nominations { get; set; }
        public GetVoteDTO Vote { get; set; }
    }
}