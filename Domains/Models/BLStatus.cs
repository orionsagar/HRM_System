using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Domains.Models
{
    public class BLStatus
    {
        public bool IsError { get; set; }
        public string Message { get; set; }
        public string StatusCode { get; set; }
        public dynamic Data { get; set; }
    }

    


}
