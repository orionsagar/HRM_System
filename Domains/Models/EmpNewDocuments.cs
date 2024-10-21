using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Domains.Models
{
 
    public class EmpNewDocuments : Audit_OrgWithClient
    {
        [Key]
        public int EmpNewDocumentsID { get; set; }

        public int EmpID { get; set; }
        public string? Name { get; set; }
        public string? CardNo { get; set; }
        public string? Designation { get; set; }

        public string? DocumentType { get; set; }
        public string? PostCode { get; set; }
        public string? Nationality { get; set; }

        public string? CountryOfResidence { get; set; }
        public string? IssueBy { get; set; }
        public DateTime? IssuedDate { get; set; }
        public DateTime? ExpiryDate { get; set; }
        public DateTime? EligibleReviewDate { get; set; }
        public string? IsCurrentPassport { get; set; }
        public string? Remarks { get; set; }
        public string? DBSType { get; set; }
        public List<DocumentLists>? AllFiles { get; set; }
    }
}
