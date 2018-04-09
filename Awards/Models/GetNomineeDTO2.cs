using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Awards.Models
{
    public class GetNomineeDTO2
    {
        public string Name { get; set; }
        public IEnumerable<GetVoteDTO2> Vote { get; set; }
        public int TotalVotes { get; set; }
    }
}