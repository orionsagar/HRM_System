using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Domains.Models
{
    public class OraganizationProfileModel
    {
        public virtual OrganizationInfo OrganizationInfo { get; set; }
        public virtual OrganizationAuthorizePerson OrganizationAuthorizePerson { get; set; }
        public List<OrganizationEmployee> organizationEmployees { get; set; }
    }
    public class  OrganizationInfo
    {
        public int OrgId { get; set; }
        public string OrgName { get; set; }
        public string RegNo { get; set; }
        public string OrgEmail { get; set; }
        public string OrgType { get; set; }
        public string TradingName { get; set; }
        public string TradingPeriod { get; set;}
        public string Website { get; set; }
        public string AddressLine1 { get; set;}
    }

    public class OrganizationAuthorizePerson
    {
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string Phone { get; set; }
        public string Email { get; set; }
    }
    public class OrganizationEmployee
    {
        public string Name { get; set; }
        public string Email { get; set; }
    }
}
