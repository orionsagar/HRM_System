using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Domains.ViewModels
{
    public class DocumentNotificationVM
    {
        public int EmpNewDocumentsID { get; set; }
        public string DocumentType { get; set; }
        public string Name { get; set; }
        public string IssuedDate { get; set; }
        public string ExpiryDate { get; set; }
        public string NinetyDaysNotify { get; set; }
        public string SixtyDaysNotify { get; set; }
        public string ThirtyDaysNotify { get; set; }
        public int totalcount { get; set; }
        public int rownum { get; set; }
    }
}
