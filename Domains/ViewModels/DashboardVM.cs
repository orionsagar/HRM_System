using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Domains.ViewModels
{
    public class MonthlyAttendanceVM
    {
        public string AttDate { get; set; }
        public int TotalPresent { get; set; }
        public int TotalLeave { get; set; }
        public int TotalLPresent { get; set; }
        public int TotalHPresent { get; set; }
        public int TotalAbsent { get; set; }
        public int Holiday { get; set; }
        public int Total { get; set; }
        public string Type { get; set; }
    }

    public class StatusChartVM
    {
        public int Total { get; set; }
        public string Status { get; set; }
    }
    public class ChartVM
    {
        public string Name { get; set; }
        public List<int> data { get; set; } = new List<int>();
    }

    public class Chart1VM
    {
        public string Name { get; set; }
        public Dictionary<string, int> data { get; set; } = new Dictionary<string, int>();
    }

    public class CardVM
    {
        public int? TotalOrganisation { get; set; }
        public int? TotalActiveClient { get; set; }
        public int? TotalInactiveClient { get; set; }
        public int? TotalPendingClient { get; set; }
        public int? TotalEmployee { get; set; }
        public int? TotalLeaveRequest { get; set; }
        public int? TotalLateEntry { get; set; }
        public int? TotalDocumentExpiry { get; set; }
    }
}
