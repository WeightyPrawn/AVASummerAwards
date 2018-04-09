using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Awards.Models
{
    public class GetNomineeDTO
    {
        public int ID { get; set; }
        public int CategoryID { get; set; }
        public string Email { get; set; }
        public string Name { get; set; }
        public string Image { get; set; }
        public IEnumerable<GetNominationDTO> Nominations { get; set; }
        public IEnumerable<GetVoteDTO> Vote { get; set; }
        public int TotalVotes { get; set; }
    }
}