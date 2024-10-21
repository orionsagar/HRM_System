using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Domains.Models
{
	public class RuleDefined
	{
		[Key]
		public int RuleID { get; set; }
		public string RuleName { get; set; }
		public string RuleGroup { get; set; }
		public string RuleCode { get; set; }
		public string Remarks { get; set; }
		public int? CompId { get; set; }
		[ForeignKey("CompId")]
		public virtual Company Company { get; set; }
	}
	public class CompanyRule
	{
		public int CompanyRuleID { get; set; }
		public int? RuleID { get; set; }
		[ForeignKey("RuleID")]
		public virtual RuleDefined Rules { get; set; }		
		public float ParamValue { get; set; }
		public int CompId { get; set; }
		[ForeignKey("CompId")]
		public virtual Company Company { get; set; }
		public DateTime EffectiveDate { get; set; }
		public string AddedBy { get; set; }
		public DateTime AddedDate { get; set; }
		public string UpdateBy { get; set; }
		public DateTime UpdateDate { get; set; }
        public int? OrgID { get; set; }
        public int? ClientID { get; set; }

    }

}
