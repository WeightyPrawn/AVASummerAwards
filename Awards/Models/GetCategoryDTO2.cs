using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Awards.Models
{
    public class GetCategoryDTO2
    {
        public string Name { get; set; }
        public IEnumerable<GetNomineeDTO2> Nominees { get; set; }
    }
}