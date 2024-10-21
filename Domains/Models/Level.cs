using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Domains.Models
{
    public class Level : Audit_Company
    {
        public int LevelID { get; set; }
        public string LevelName { get; set; }
        public int SortIndex { get; set; }
        public string Status { get; set; }
        public int OrgID { get; set; }
    }
}
