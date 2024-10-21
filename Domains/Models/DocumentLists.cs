using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Domains.Models
{
    public class DocumentLists
    {
        [Key]
        public int DocID { get; set; }
        public int EmDocID { get; set; }
        public string DocName { get; set; }
        public string DocFiles { get; set; }
        public string DocSize { get; set; }
    }
}
