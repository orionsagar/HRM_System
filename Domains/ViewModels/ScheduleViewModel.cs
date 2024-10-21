using Domains.Models;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Domains.ViewModels
{
    [NotMapped]
    public class EmpJobCalendarFilter : EmployeeFilterVM
    {
        public bool IsNoJobCalender { get; set; }
    }
    public class EmpJobCalendarListVM : EmployeeVM
    {
        public string CurrentJobCalender { get; set; }
        public int OrgId { get; set; }
        public int ClientId { get; set; }
    }

    [NotMapped]
    public class EmpShiftFilter : EmployeeFilterVM
    {
        public bool IsNoShift { get; set; }
    }

    public class EmpShiftListVM : EmployeeVM
    {
        public string CurrentShift { get; set; }
    }

}
