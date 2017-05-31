using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Awards.Models
{
    public class GetCategoryDTO
    {
        public int ID { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        public bool HasVoted { get; set; }
        public IEnumerable<GetNomineeDTO> Nominees { get; set; }
    }
}