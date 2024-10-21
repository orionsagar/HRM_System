using Microsoft.AspNetCore.Http;
using System;
using System.ComponentModel.DataAnnotations;

namespace Domains.Models
{
    public class EmployeeDocuments : Audit_Company
    {
        [Key]
        public int EmpDocID { get; set; }

        public int EmpID { get; set; }
        public string Name { get; set; }
        public string CardNo { get; set; }
        public string Address { get; set; }
        public string Mobile { get; set; }
        public string Designation { get; set; }

        #region Contact Information

        public string PostCode { get; set; }
        public string ContactAddress1 { get; set; }
        public string ContactAddress2 { get; set; }
        public string ContactCity { get; set; }
        public string ContactCountry { get; set; }
        public string ProofOfAddress { get; set; }
        #endregion


        #region Passport Details

        public string PassportNo { get; set; }
        public string PassportNationality { get; }
        public string PlaceOfBirth { get; set; }
        public string IssueBy { get; set; }
        public DateTime IssuedDate { get; set; }
        public DateTime ExpiryDate { get; set; }
        public DateTime EligibleReviewDate { get; set; }
        public string UploadDocument { get; set; }
        public string IsCurrentPassport { get; set; }
        public string Remarks { get; set; }
        #endregion


        #region Visa/BRP Details

        public string VisaBRPNo { get; set; }
        public string VisaNationality { get; set; }
        public string CountryOfResidence { get; set; }
        public string VisaIssuedBy { get; set; }
        public DateTime VisaIssuedDate { get; set; }
        public DateTime VisaExpiryDate { get; set; }
        public DateTime VisaEligibleReviewDate { get; set; }
        public string VisaUploadDocument { get; set; }
        public string VisaIsCurrentPassport { get; set; }
        public string VisaRemarks { get; set; }
        #endregion

        #region EUSS/Time limit details

        public string ReferenceNumber { get; set; }
        public string EussNationality { get; set; }
        public DateTime EussIssuedDate { get; set; }
        public DateTime EussExpiryDate { get; set; }
        public DateTime EussEligibleReviewDate { get; set; }
        public string EussUploadDocument { get; set; }
        public string EussIsCurrentPassport { get; set; }
        public string EussRemarks { get; set; }
        #endregion

        #region Disclosure and Barring Service (DBS) details

        public string DBSType { get; set; }
        public string DBSReferenceNumber { get; set; }
        public string DBSNationality { get; set; }
        public DateTime DBSIssuedDate { get; set; }
        public DateTime DBSExpiryDate { get; set; }
        public DateTime DBSEligibleReviewDate { get; set; }
        public string DBSUploadDocument { get; set; }
        public string DBSIsCurrentPassport { get; set; }
        public string DBSRemarks { get; set; }
        #endregion

        #region National Id details

        public string NationalIdNo { get; set; }
        public string IdNationality { get; set; }
        public string IdCountryOfResidence { get; set; }
        public DateTime IdIssuedDate { get; set; }
        public DateTime IdExpiryDate { get; set; }
        public DateTime IdEligibleReviewDate { get; set; }
        public string IdUploadDocument { get; set; }
        public string IdIsCurrentPassport { get; set; }
        public string IdRemarks { get; set; }
        #endregion

        #region Other Details

        public string DocumentName { get; set; }
        public string DocumentRefNum { get; set; }
        public string OtherNationality { get; set; }
        public DateTime OtherIssuedDate { get; set; }
        public DateTime OtherExpiryDate { get; set; }
        public DateTime OtherEligibleReviewDate { get; set; }
        public string OtherUploadDocument { get; set; }
        public string OtherIsCurrentPassport { get; set; }
        public string OtherRemarks { get; set; }

        #endregion



        public DateTime ChangedDate { get; set; }
        public string RestrictionOfWork { get; set; }
        public string HRLineContactDetails { get; set; }
        public string HomeOfficeInterview { get; set; }
    }
}
