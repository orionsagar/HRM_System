using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Domains.Models
{
    public class UserProfileVM
    {
        [Key]
        public int UserID { get; set; }
        public string SRACode { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string Name { get; set; }
        public string ClientType { get; set; }
        public string BusinessType { get; set; }
        public string Address { get; set; }
        public string City { get; set; }
        public string State { get; set; }
        public string Country { get; set; }
        public string HotNumber { get; set; }
        public string Email1 { get; set; }
        public string Phone { get; set; }
        public string Logo { get; set; }
        public string CP_Name { get; set; }
        public string CP_Phone { get; set; }
        public string CP_Mobile { get; set; }
        public string CP_Email { get; set; }
        public string Facebook { get; set; }
        public string Instagram { get; set; }
        public string Linkedin { get; set; }
        public string twitter { get; set; }
        public string GooglePlay { get; set; }
        public string Website { get; set; }
        public string Establishment_Year { get; set; }
        public string PostCode { get; set; }
        public int UserStatus { get; set; }
        public int ApprovedId { get; set; }
        public string ApprovedByName { get; set; }
        public List<MiniOrgCard_solicitor> MiniOrgCard { get; set; }
    }
    public class MiniOrgCard_solicitor
    {
        //This will get list as per solicitor ID
        [Key]
        public int OrgId { get; set; }
        public string OrgName { get; set; }
        public string OrgEmail { get; set; }
        public string ContactNo { get; set; }
        public string LogoPath { get; set; }
        public int PackageId { get; set; }
        public string PackageName { get; set; }
        public string MaxEmp {  get; set; }
        public int NumberOfEmployees { get; set; }
    }
 
}
