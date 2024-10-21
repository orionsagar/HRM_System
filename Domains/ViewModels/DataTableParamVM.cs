using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Domains.ViewModels
{
    public class DataTableParamVM
    {
        public int DisplayLength { get; set; }
        public int DisplayStart { get; set; }
        public int SortCol { get; set; }
        public string SortDir { get; set; }
        public string Search { get; set; }
        public string RoleType { get; set; }
        public string DocType { get; set; }
        public int CompId { get; set; } = 1;
        public int OrgId { get; set; } = 0;
        public int ClientId { get; set; } = 0;
    }
}
