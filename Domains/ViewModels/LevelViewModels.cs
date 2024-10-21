using Domains.Models;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Domains.ViewModels
{
    public class LevelViewModels : Audit_Company
    {
        public int LevelID { get; set; }
        [Required(ErrorMessage ="Level Name is required!!!")]
        [DisplayName("Level Name")]
        public string LevelName { get; set; }

        [DisplayName("Sort Index")]
        public int SortIndex { get; set; }
        public string Status { get; set; }
        public int OrgID { get; set; }
    }
}
