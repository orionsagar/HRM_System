using Microsoft.AspNetCore.Http;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Domains.ViewModels
{
    public class ClientViewModel
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.None)]
        public int ClientID { get; set; }

        [Required(ErrorMessage = "Name Is Required")]
        [Display(Name = "Name")]
        public string Name { get; set; }

        [Required(ErrorMessage = "Client Type Is Required")]
        [Display(Name = "Client Type")]
        public string ClientType { get; set; }
        public string ClientType1 { get; set; }

        [Display(Name = "Business Type")]
        public string BusinessType { get; set; }
        public string Address { get; set; }
        [Required(ErrorMessage = "City Is Required")]
        public string City { get; set; }        
        public string State { get; set; }

        [Display(Name = "Hot Number")]
        public string HotNumber { get; set; }

        [Required(ErrorMessage = "Contact Number Is Required")]
        [Display(Name = "Contact Number")]
        public string Phone1 { get; set; }

        [Display(Name = "Additional Contact Number")]
        public string Phone2 { get; set; }

        [Required(ErrorMessage = "Primary Email Is Required")]
        [Display(Name = "Primary Email")]
        public string Email1 { get; set; }

        [Display(Name = "Additional Email")]
        public string Email2 { get; set; }

        [Display(Name = "Business Hour")]
        public string BusinessHour { get; set; }

        public string Website { get; set; }
        public string Country { get; set; }
        public string Facebook { get; set; }
        public string Instagram { get; set; }
        public string Linkedin { get; set; }
        public string twitter { get; set; }
        public string GooglePlus { get; set; }
        public IFormFile Logofile { get; set; }
        public string Logo { get; set; }
        //public int ParentCompID { get; set; }

        [Display(Name = "Contact Person Name")]        
        public string CP_Name { get; set; }
        [Display(Name = "Contact Person Phone")]
        public string CP_Phone { get; set; }
        [Display(Name = "Contact Person Mobile")]
        public string CP_Mobile { get; set; }
        [Display(Name = "Contact Person Email")]
        public string CP_Email { get; set; }
        [Display(Name = "Contact Person Address 1")]
        public string AddressLine1 { get; set; }

        [Display(Name = "Contact Person Address 2")]
        public string AddressLine2 { get; set; }

        [Display(Name = "Contact Person Address 3")]
        public string AddressLine3 { get; set; }

        [Display(Name = "Establishment Year")]
        public string Establishment_Year { get; set; }
        [Display(Name = "Number Of Employee")]
        public string Number_Of_Employee { get; set; }
        public string Number_Of_Org { get; set; }

        [Display(Name = "Post Code")]
        [Required(ErrorMessage = "PostCode is Required")]
        public string PostCode { get; set; }
        public string RegistrationNo { get; set; }
        public string C_U_Key { get; set; }
        public string MC_U_Key { get; set; }
        public string OrganisationSize { get; set; }
        public string OrganisationType { get; set; }
        public int ROWNUM { get; set; }
        public int TOTALCOUNT { get; set; }
        public int TotalOrg { get; set; }

        [Display(Name = "Email Address")]
        public string ClientSAEmail { get; set; }
        public string Password { get; set; }
        public string ConfirmPassword { get; set; }
        public string Status { get; set; }
        public int UserId { get; set; }
        public int SRACode { get; set; }
        public bool IsDefault { get; set; }
        public string AcHolderName { get; set; }
        public string SortCode { get; set; }
        public string AccountNumber { get; set; }
    }
}
