using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Domains.Models
{
    public class ReportTypes
    {
        [Key]
        public int ReportTypeId { get; set; }
        public string ReportTypeName { get; set; }
        public int SLNo { get; set; }
    }
    public class Reports
    {
        [Key]
        public int ReportId { get; set; }
        public string ReportName { get; set; }
        public string ShortName { get; set; }
        public string ReportLink { get; set; }
        public int ReportTypeId { get; set; }
        [ForeignKey("ReportTypeId")]
        public ReportTypes ReportType { get; set; }
        public string ClassName { get; set; }
        public string IdName { get; set; }
        public string Remarks { get; set; }
        public int SLNo { get; set; }
        public int CompId { get; set; }
        public DateTime AddedDate { get; set; }
        public int AddedBy { get; set; }
        public int MenuId { get; set;}
        [ForeignKey("MenuId")]
        public UserAccessTools UserAccessTools { get; set; }

    }
}
