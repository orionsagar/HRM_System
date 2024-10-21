using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Domains.ViewModels
{
    public class DomainEventVM
    {
        public int OrgId { get; set; }
        public int ClientId { get; set; }
        public int UserId { get; set; }
        public string OrgName { get; set; }
        public string ClientName { get; set; }
        public string UserName { get; set; }
        public string AdminName { get; set; }
        public string Email { get; set; }
        public string Phone { get; set; }
        public string CountryCode { get; set; }
        public string Address { get; set; }
        public string MailSubject { get; set; }
        public string MailFor { get; set; }
        public string SupportPhone { get; set; }
        public string SupportMail { get; set; }
    }
}
