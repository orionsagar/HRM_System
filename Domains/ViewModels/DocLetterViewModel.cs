using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Domains.ViewModels
{
    public class DocLetterViewModel
    {
        public string EmployeeName { get; set; }
        public string EmpAddress { get; set; }
        public string DocType { get; set; }
        public string DocDate { get; set; }
        public string DocExpiryDate { get; set; }
        public string OrgLogo { get; set; }
        public string LetterDays { get; set; }
        public string SendedBy { get; set; }
        public string SenderDesignation { get; set; }
    }
}
