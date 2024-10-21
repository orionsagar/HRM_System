using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Domains.Models
{
	public class MainModule : Audit_Company
	{
		[Key]
		public int ModuleID { get; set; }
		[Display(Name = "Module Name")]
		public string ModuleName { get; set; }
		public string IconPath { get; set; }
		public string Url { get; set; }
		public string ColorCode { get; set; }
		public string ClassName { get; set; }
		public string Description { get; set; }
		public string Package { get; set; }
		[Display(Name = "Sort Index")]
		public int SortIndex { get; set; }
		[NotMapped]
		[Display(Name = "Total Page Cnt")]
		public int TotalPageCnt { get; set; }
		public virtual ICollection<SubModule> SubModules { get; set; }
	}
	public class SubModule : Audit_Company
	{
		[Key]
		public int SubModuleID { get; set; }
		[Display(Name = "SubModule Name")]
		public string SubModuleName { get; set; }
		public string Description { get; set; }
		[Display(Name = "Sort Index")]
		public int SortIndex { get; set; }
		[NotMapped]
		[Display(Name = "Total Page Cnt")]
		public int TotalPageCnt { get; set; }
		public int ModuleID { get; set; }
		[ForeignKey("ModuleID")]
		public virtual MainModule MainModule { get; set; }
		public virtual ICollection<UserAccessList> UserAccessLists { get; set; }
	}

	public class UserAccessList : Audit
	{
		[Key]
		public int ulistID { get; set; }
		public string Accesstools { get; set; }
		public bool UAVeiw { get; set; }
		public bool UASave { get; set; }
		public bool UAEdit { get; set; }
		public bool UAdelete { get; set; }
		public bool UApproved { get; set; }
		public int RoleID { get; set; }
		[ForeignKey("RoleID")]
		public virtual UserRole UserRole { get; set; }
		public int UAtoolID { get; set; }
		[ForeignKey("UAtoolID")]
		public virtual UserAccessTools UserAccessTools { get; set; }
		public bool UDisburse { get; set; }
		public int? SubModuleID { get; set; }
		[ForeignKey("SubModuleID")]
		public virtual SubModule SubModule { get; set; }
        public int ModuleID { get; set; }

	}

	public class UserAccessTools :Audit_Company // Menu list
	{
		[Key]
		public int UAToolid { get; set; }
		public string UseraccesstoolNM { get; set; }
		public string Category { get; set; }
		public string UAPage { get; set; }
		[Required]
		public int ModuleID { get; set; }
		[ForeignKey("ModuleID")]
		public virtual MainModule MainModule { get; set; }
		public bool ismenu { get; set; }
		public string ClassName { get; set; }
		public int SortIndex { get; set; }
		public int RoleID { get; set; }
		[ForeignKey("RoleID")]
		public virtual UserRole UserRole { get; set; }
		public int? SubModuleID { get; set; }
		[ForeignKey("SubModuleID")]
		public virtual SubModule SubModule { get; set; }
	}
    public class SubMenu : Audit_Company
    {
        public string SubMenuId { get; set; }
        public string UAToolid { get; set; }
        public string isMenu { get; set; }
        public string ClassName { get; set; }
        public string Discription { get; set; }
        public int SortIndex { get; set; }
    }
    public class UserPageAccess
	{
		public string uatoolid { get; set; }
		public string UAPage { get; set; }
		public string Category { get; set; }
		public string ulistID { get; set; }
		public string UserID { get; set; }
		public string Accesstools { get; set; }
		public bool UAVeiw { get; set; }
		public bool UASave { get; set; }
		public bool UAEdit { get; set; }
		public bool UAdelete { get; set; }
		public bool UApproved { get; set; }
		public string RoleID { get; set; }
		public string UAtoolID { get; set; }
		public string UDisburse { get; set; }
	}

	public class ReportAccess
	{
		public int ReportAccessId { get; set; }
		public int ReportId { get; set; }
        [ForeignKey("ReportId")]
        public Reports Reports { get; set; }
        public int RoleId { get; set; }
        [ForeignKey("RoleId")]
        public UserRole Role { get; set; }
        public int MenuId { get; set; }
        [ForeignKey("MenuId")]
        public UserAccessTools UserAccessTools { get; set; }
        public int UserId { get; set; }
        [ForeignKey("UserId")]
        public UserInfo UserInfo { get; set; }
        public bool IsPermission { get; set; }
        public string ReportTypeName { get; set; }
    }
}
