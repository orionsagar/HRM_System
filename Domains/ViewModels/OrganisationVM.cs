using Domains.Models;
using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Domains.ViewModels
{
    public class OrganisationVM : Audit
    {
        [Key]
        public int OrgId { get; set; }
        [MaxLength(500)]
        [Display(Name = "Organization Name")]
        public string OrgName { get; set; }

        [Required(ErrorMessage = "Organization Code is required.")]
        [StringLength(3, MinimumLength = 3, ErrorMessage = "Organization Code must be {1} characters.")]
        [Display(Name = "Organization Code")]
        public string OrgCode { get; set; }

        [MaxLength(500)]
        [Display(Name = "Organization Type")]
        public string OrgType { get; set; }
        [MaxLength(500)]
        [Display(Name = "Organization Type")]
        public string OrgType1 { get; set; }

        [MaxLength(250)]
        [Display(Name = "Registration No")]
        public string RegNo { get; set; }

        [MaxLength(30)]
        [Display(Name = "Contact No")]
        public string ContactNo { get; set; }

        [MaxLength(150)]
        [Display(Name = "Organisation Email")]
        public string OrgEmail { get; set; }

        [MaxLength(30)]
        [Display(Name = "Post Code")]
        public string PostCode { get; set; }

        [Display(Name = "Address Line 1")]
        public string AddressLine1 { get; set; }

        [Display(Name = "Address Line 2")]
        public string AddressLine2 { get; set; }

        [Display(Name = "Address Line 3")]
        public string AddressLine3 { get; set; }

        [Display(Name = "City")]
        public string City { get; set; }

        [Display(Name = "Country")]
        public int CountryId { get; set; }

        [MaxLength(500)]
        public string Website { get; set; }

        [MaxLength(30)]
        [Display(Name = "Landline Number")]
        public string LandlineNumber { get; set; }

        [MaxLength(500)]
        [Display(Name = "Trading Name")]
        public string TradingName { get; set; }

        [MaxLength(250)]
        [Display(Name = "Trading Period")]
        public string TradingPeriod { get; set; }

        [MaxLength(500)]
        [Display(Name = "Name of Sector ")]
        public string SectorName { get; set; }
        [MaxLength(500)]
        [Display(Name = "Name of Sector ")]
        public string SectorName1 { get; set; }

        [MaxLength(250)]
        [Display(Name = "Logo")]
        public string LogoPath { get; set; }
        public IFormFile Logofile { get; set; }

        [MaxLength(250)]
        [Display(Name = "First Name")]
        public string AP_FirstName { get; set; }

        [MaxLength(250)]
        [Display(Name = "Last Name")]
        public string AP_LastName { get; set; }

        
        [Display(Name = "Designation")]
        public int AP_DesignationID { get; set; }

        [MaxLength(30)]
        [Display(Name = "Phone No")]
        public string AP_PhoneNo { get; set; }

        [MaxLength(150)]
        [Display(Name = "Email")]
        public string AP_Email { get; set; }

        [MaxLength(250)]
        [Display(Name = "Proof Of Id")]
        public string AP_NIdPath { get; set; }
        [Display(Name = "NID")]
        public IFormFile AP_NId { get; set; }


        [Display(Name = "Do you have a history of Criminal\r\nconviction/Bankruptcy/Disqualification? ")]
        public string AP_IsCriminalHistory { get; set; }

        [MaxLength(250)]
        [Display(Name = "First Name")]
        public string KC_FirstName { get; set; }

        [MaxLength(250)]
        [Display(Name = "Last Name")]
        public string KC_LastName { get; set; }

        
        [Display(Name = "Designation")]
        public int KC_DesignationID { get; set; }

        [MaxLength(30)]
        [Display(Name = "Phone No")]
        public string KC_PhoneNo { get; set; }

        [MaxLength(150)]
        [Display(Name = "Email")]
        public string KC_Email { get; set; }

        [MaxLength(250)]
        [Display(Name = "Proof Of Id")]
        public string KC_NIdPath { get; set; }
        public IFormFile KC_NId { get; set; }


        [Display(Name = "Do you have a history of Criminal conviction\r\n/Bankruptcy/Disqualification?")]
        public string KC_IsCriminalHistory { get; set; }

        [MaxLength(250)]
        [Display(Name = "First Name")]
        public string SA_FirstName { get; set; }

        [MaxLength(250)]
        [Display(Name = "Last Name")]
        public string SA_LastName { get; set; }

        
        [Display(Name = "Designation")]
        public int? SA_DesignationID { get; set; }

        [MaxLength(30)]
        [Display(Name = "Phone No")]
        public string SA_PhoneNo { get; set; }

        [MaxLength(150)]
        [Display(Name = "Email")]
        public string SA_Email { get; set; }

        [MaxLength(250)]
        [Display(Name = "Proof Of Id")]
        public string SA_NIdPath { get; set; }
        public IFormFile SA_NId { get; set; }

        
        [Display(Name = "Do you have a history of Criminal conviction\r\n/Bankruptcy/Disqualification?")]
        public string SA_IsCriminalHistory { get; set; }

        //public string? numberOfEmp { get; set; }
        public int TOTALCOUNT { get; set; }
        public int ROWNUM { get; set; }
        public int ClientId { get; set; }
        public string ClientName { get; set; }
        public int PackageId { get; set; }
        public string PackageName { get; set; }
        public int NumberOfEmp { get; set; }
        public string Status { get; set; }
        public string? CountryName { get; set; }
        public string? AP_DesignationName { get; set; }
        public virtual PayScale PayScale { get; set; }
    }
}
