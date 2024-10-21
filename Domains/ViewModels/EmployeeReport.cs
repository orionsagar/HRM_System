using Domains.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Domains.ViewModels
{
    public class EmployeeReport
    {
        public OrganisationVM OrganisationVM { get; set; }
        public Employee Employee { get; set; }
        public List<EmployeeVM> AllEmployee { get; set; }
        public List<EmpDocumentsVM> EmpNewDocuments { get; set; }
    }
}
