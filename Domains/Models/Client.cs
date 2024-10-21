using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Domains.Models
{
    public class InviteClient
    {
        public string UserName { get; set; }
        public string Email { get; set; }
        public int AddedById { get; set; }
    }
    public class Client
    {
        [Key]
        public int ClientID { get; set; }
        public string Name { get; set; }
        public string SRACode { get; set; }
        public string ClientType { get; set; }
        public string BusinessType { get; set; }
        public string Attention { get; set; }
        public string Address { get; set; }
        public string City { get; set; }
        public string State { get; set; }
        public string HotNumber { get; set; }
        public string BKashNumber { get; set; }
        public string Phone1 { get; set; }
        public string CountryCode { get; set; }
        public string Phone2 { get; set; }
        public string Email1 { get; set; }
        public string Email2 { get; set; }
        public string BusinessHour { get; set; }
        public string Website { get; set; }
        public string Country { get; set; }
        public string Facebook { get; set; }
        public string Instagram { get; set; }
        public string Linkedin { get; set; }
        public string twitter { get; set; }
        public string GooglePlus { get; set; }
        public string Logo { get; set; }
        public int ParentCompID { get; set; }
        public string CP_Name { get; set; }
        public string CP_Phone { get; set; }
        public string CP_Mobile { get; set; }
        public string CP_Email { get; set; }
        public string AddressLine1 { get; set; }
        public string AddressLine2 { get; set; }
        public string AddressLine3 { get; set; }
        public string Establishment_Year { get; set; }
        public string Number_Of_Employee { get; set; }
        public bool IsChangeOrg { get; set; }
        public bool IsFacedPenalty { get; set; }
        public string NameofSector { get; set; }
        public string PostCode { get; set; }
        public string RegistrationNo { get; set; }
        public string TradingName { get; set; }
        public string TradingPeriod { get; set; }
        public string C_U_Key { get; set; }
        public string MC_U_Key { get; set; }
        public string OrganisationSize { get; set; }
        public string OrganisationType { get; set; }
        public string Status { get; set; }
        public bool IsDefault { get; set; }
        public string AcHolderName { get; set; }
        public string SortCode { get; set; }
        public string AccountNumber { get; set; }
    }
}
