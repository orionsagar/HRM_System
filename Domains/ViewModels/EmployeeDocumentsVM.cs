using Domains.Models;
using Microsoft.AspNetCore.Http;
using System;
using System.ComponentModel.DataAnnotations;

namespace Domains.ViewModels
{
    public class EmployeeDocumentsVM : Audit_Company
    {
        public int EmpDocID { get; set; }

        [Display(Name = "Select Employee")]
        [Required(ErrorMessage = "Please choose of Employee")]
        public int EmpID { get; set; }

        public string Name { get; set; }
        public string CardNo { get; set; }
        public string Address { get; set; }
        public string Mobile { get; set; }
        public string Designation { get; set; }

        #region Contact Information
        [Display(Name = "Post Code")]
        public string PostCode { get; set; }

        [Display(Name = "Adress Line 1")]
        public string ContactAddress1 { get; set; }

        [Display(Name = "Adress Line 2")]
        public string ContactAddress2 { get; set; }

        [Display(Name = "City / County")]
        public string ContactCity { get; set; }

        [Display(Name = "Country")]
        public string ContactCountry { get; set; }

        public string? ProofOfAddress { get; set; } = "";

        [Required(ErrorMessage = "Please choose of address")]
        [Display(Name = "Proof Of Address")]
        public IFormFile ProofOfAddressFile { get; set; }
        #endregion


        #region Passport Details
        [Display(Name = "Passport No.")]
        public string? PassportNo { get; set; }

        [Display(Name = "Nationality")]
        public string? PassportNationality { get; }

        [Display(Name = "Place of Birth")]
        public string? PlaceOfBirth { get; set; }

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

        public string? UploadDocument { get; set; }

        [Display(Name = "Upload Document")]
        public IFormFile? UploadDocumentFile { get; set; }

        [Display(Name = "Is this your current passport?")]
        public string? IsCurrentPassport { get; set; }

        [Display(Name = "Remarks")]
        public string? Remarks { get; set; }
        #endregion


        #region Visa/BRP Details
        [Display(Name = "Visa/BRP No.")]
        public string? VisaBRPNo { get; set; }

        [Display(Name = "Nationality")]
        public string? VisaNationality { get; set; }

        [Display(Name = "Country of Residence")]
        public string? CountryOfResidence { get; set; }

        [Display(Name = "Issued By")]
        public string? VisaIssuedBy { get; set; }

        [Display(Name = "Issued Date")]
        [DataType(DataType.Date)]
        [DisplayFormat(ApplyFormatInEditMode = true, DataFormatString = "{0:dd-MM-yyyy}")]
        public DateTime? VisaIssuedDate { get; set; }

        [Display(Name = "Expiry Date")]
        [DataType(DataType.Date)]
        [DisplayFormat(ApplyFormatInEditMode = true, DataFormatString = "{0:dd-MM-yyyy}")]
        public DateTime? VisaExpiryDate { get; set; }

        [Display(Name = "Eligible Review Date")]
        [DataType(DataType.Date)]
        [DisplayFormat(ApplyFormatInEditMode = true, DataFormatString = "{0:dd-MM-yyyy}")]
        public DateTime VisaEligibleReviewDate { get; set; }
        public string? VisaUploadDocument { get; set; }

        [Display(Name = "Upload Document")]
        public IFormFile? VisaUploadDocumentFile { get; set; }

        [Display(Name = "Is this your current passport?")]
        public string? VisaIsCurrentPassport { get; set; }

        [Display(Name = "Remarks")]
        public string? VisaRemarks { get; set; }
        #endregion

        #region EUSS/Time limit details
        [Display(Name = "Reference Number.")]
        public string? ReferenceNumber { get; set; }

        [Display(Name = "Nationality")]
        public string? EussNationality { get; set; }

        [Display(Name = "Issued Date")]
        [DataType(DataType.Date)]
        [DisplayFormat(ApplyFormatInEditMode = true, DataFormatString = "{0:dd-MM-yyyy}")]
        public DateTime? EussIssuedDate { get; set; }

        [Display(Name = "Expiry Date")]
        [DataType(DataType.Date)]
        [DisplayFormat(ApplyFormatInEditMode = true, DataFormatString = "{0:dd-MM-yyyy}")]
        public DateTime? EussExpiryDate { get; set; }

        [Display(Name = "Eligible Review Date")]
        [DataType(DataType.Date)]
        [DisplayFormat(ApplyFormatInEditMode = true, DataFormatString = "{0:dd-MM-yyyy}")]
        public DateTime? EussEligibleReviewDate { get; set; }

        public string? EussUploadDocument { get; set; }
        
        [Display(Name = "Upload Document")]
        public IFormFile? EussUploadDocumentFile { get; set; }

        [Display(Name = "Is this your current status?")]
        public string? EussIsCurrentPassport { get; set; }

        [Display(Name = "Remarks")]
        public string? EussRemarks { get; set; }
        #endregion

        #region Disclosure and Barring Service (DBS) details
        [Display(Name = "DBS Type")]
        public string? DBSType { get; set; }

        [Display(Name = "Reference Number.")]
        public string? DBSReferenceNumber { get; set; }

        [Display(Name = "Nationality")]
        public string? DBSNationality { get; set; }

        [Display(Name = "Issued Date")]
        [DataType(DataType.Date)]
        [DisplayFormat(ApplyFormatInEditMode = true, DataFormatString = "{0:dd-MM-yyyy}")]
        public DateTime? DBSIssuedDate { get; set; }

        [Display(Name = "Expiry Date")]
        [DataType(DataType.Date)]
        [DisplayFormat(ApplyFormatInEditMode = true, DataFormatString = "{0:dd-MM-yyyy}")]
        public DateTime? DBSExpiryDate { get; set; }

        [Display(Name = "Eligible Review Date")]
        [DataType(DataType.Date)]
        [DisplayFormat(ApplyFormatInEditMode = true, DataFormatString = "{0:dd-MM-yyyy}")]
        public DateTime? DBSEligibleReviewDate { get; set; }

        public string? DBSUploadDocument { get; set; }

        [Display(Name = "Upload Document")]
        public IFormFile? DBSUploadDocumentFile { get; set; }

        [Display(Name = "Is this your current status?")]
        public string? DBSIsCurrentPassport { get; set; }

        [Display(Name = "Remarks")]
        public string? DBSRemarks { get; set; }
        #endregion

        #region National Id details
        [Display(Name = "National id number.")]
        public string? NationalIdNo { get; set; }

        [Display(Name = "Nationality")]
        public string? IdNationality { get; set; }

        [Display(Name = "Country of Residence")]
        public string? IdCountryOfResidence { get; set; }

        [Display(Name = "Issued Date")]
        [DataType(DataType.Date)]
        [DisplayFormat(ApplyFormatInEditMode = true, DataFormatString = "{0:dd-MM-yyyy}")]
        public DateTime? IdIssuedDate { get; set; }

        [Display(Name = "Expiry Date")]
        [DataType(DataType.Date)]
        [DisplayFormat(ApplyFormatInEditMode = true, DataFormatString = "{0:dd-MM-yyyy}")]
        public DateTime? IdExpiryDate { get; set; }

        [Display(Name = "Eligible Review Date")]
        [DataType(DataType.Date)]
        [DisplayFormat(ApplyFormatInEditMode = true, DataFormatString = "{0:dd-MM-yyyy}")]
        public DateTime? IdEligibleReviewDate { get; set; }

        public string? IdUploadDocument { get; set; }

        [Display(Name = "Upload Document")]
        public IFormFile? IdUploadDocumentFile { get; set; }

        [Display(Name = "Is this your current status?")]
        public string? IdIsCurrentPassport { get; set; }

        [Display(Name = "Remarks")]
        public string? IdRemarks { get; set; }
        #endregion

        #region Other Details
        [Display(Name = "Document name")]
        public string? DocumentName { get; set; }

        [Display(Name = "Document reference number")]
        public string? DocumentRefNum { get; set; }

        [Display(Name = "Nationality")]
        public string? OtherNationality { get; set; }

        [Display(Name = "Issued Date")]
        [DataType(DataType.Date)]
        [DisplayFormat(ApplyFormatInEditMode = true, DataFormatString = "{0:dd-MM-yyyy}")]
        public DateTime? OtherIssuedDate { get; set; }

        [Display(Name = "Expiry Date")]
        [DataType(DataType.Date)]
        [DisplayFormat(ApplyFormatInEditMode = true, DataFormatString = "{0:dd-MM-yyyy}")]
        public DateTime? OtherExpiryDate { get; set; }

        [Display(Name = "Eligible Review Date")]
        [DataType(DataType.Date)]
        [DisplayFormat(ApplyFormatInEditMode = true, DataFormatString = "{0:dd-MM-yyyy}")]
        public DateTime? OtherEligibleReviewDate { get; set; }

        public string? OtherUploadDocument { get; set; }

        [Display(Name = "Upload Document")]
        public IFormFile? OtherUploadDocumentFile { get; set; }

        [Display(Name = "Is this your current status?")]
        public string? OtherIsCurrentPassport { get; set; }

        [Display(Name = "Remarks")]
        public string? OtherRemarks { get; set; }

        #endregion


        [Required(ErrorMessage = "Please choose Date")]
        [Display(Name = "Changed Date")]
        [DataType(DataType.Date)]
        [DisplayFormat(ApplyFormatInEditMode = true, DataFormatString = "{0:dd-MM-yyyy}")]

        public DateTime ChangedDate { get; set; }


        [Display(Name = "Remarks/Restriction to work")]
        public string RestrictionOfWork { get; set; }

        [Required(ErrorMessage = "Please choose of the option")]
        [Display(Name = "Are Sponsored migrants aware that they must inform [HR/line manager] promptly of changes in contact Details")]
        public string HRLineContactDetails { get; set; }

        [Required(ErrorMessage = "Please choose of the option")]
        [Display(Name = "Are Sponsored migrants aware that they need to cooperate Home Office interview by presenting original passports during the Interview (In applicable cases)?")]
        public string HomeOfficeInterview { get; set; }
        public string NinetyDaysNotify { get; set; }
        public string SixtyDaysNotify { get; set; }
        public string ThirtyDaysNotify { get; set; }
    }
}
