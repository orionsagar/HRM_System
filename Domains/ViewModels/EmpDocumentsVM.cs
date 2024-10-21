using Domains.Models;
using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Domains.ViewModels
{
    public class EmpDocumentsVM : Audit_OrgWithClient
    {
        [Key]
        public int EmpNewDocumentsID { get; set; }

        [Display(Name = "Select Employee")]
        [Required(ErrorMessage = "Please choose of Employee")]
        public int EmpID { get; set; }
        public string? Name { get; set; }
        public string? CardNo { get; set; }
        public string? Designation { get; set; }

        [Display(Name = "Document Type")]
        [Required(ErrorMessage = "Please choose of Document Type")]
        public string? DocumentType { get; set; }

        [Display(Name = "Post Code/Passport No/Visa No/Ref No")]
        public string? PostCode { get; set; }

        [Display(Name = "Nationality")]
        public string? Nationality { get; set; }

        [Display(Name = "Country Of Residence")]
        public string? CountryOfResidence { get; set; }



        [Display(Name = "Issued By")]
        public string? IssueBy { get; set; }



        [Display(Name = "Issued Date")]
        [DataType(DataType.Date)]
        [DisplayFormat(ApplyFormatInEditMode = true, DataFormatString = "{0:dd-MM-yyyy}")]
        public DateTime? IssuedDate { get; set; }



        [Display(Name = "Expiry Date")]
        [DataType(DataType.Date)]
        [DisplayFormat(ApplyFormatInEditMode = true, DataFormatString = "{0:dd-MM-yyyy}")]
        public DateTime? ExpiryDate { get; set; }



        [Display(Name = "Eligible Review Date")]
        [DataType(DataType.Date)]
        [DisplayFormat(ApplyFormatInEditMode = true, DataFormatString = "{0:dd-MM-yyyy}")]
        public DateTime? EligibleReviewDate { get; set; }


        [Display(Name = "Is this your current passport?")]
        public string? IsCurrentPassport { get; set; }


        [Display(Name = "Remarks")]
        public string? Remarks { get; set; }


        [Display(Name = "DBS Type")]
        public string? DBSType { get; set; }
        [NotMapped]
        public List<IFormFile>? Files { get; set; }

        public List<DocumentLists>? AllFiles { get; set; }

        public string NinetyDaysNotify { get; set; }
        public string SixtyDaysNotify { get; set; }
        public string ThirtyDaysNotify { get; set; }
        public int totalcount { get; set; }
        public int rownum { get; set; }
    }
}
