using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using Microsoft.AspNetCore.Http;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Text.Json.Serialization;
using Domains.Models;

namespace Domains.ViewModels
{
    public class MainModuleViewModel
    {
        public int ModuleID { get; set; }

        [Required(ErrorMessage = "Name Is Required")]
        public string ModuleName { get; set; }
        public string Description { get; set; }
        public string Package { get; set; }
        public int? SortIndex { get; set; }
    }

    public class SubModuleVM : Audit_Company
    {
        public int SubModuleID { get; set; }
        [Display(Name = "SubModule Name")]
        public string SubModuleName { get; set; }
        public string Description { get; set; }
        [Display(Name = "Sort Index")]
        public int SortIndex { get; set; }
        [Display(Name = "Total Page Cnt")]
        public int TotalPageCnt { get; set; }
        public int ModuleID { get; set; }
        public string ModuleName { get; set; }
        public string CompanyName { get; set; }
        public int TOTALCOUNT { get; set; }
        public int ROWNUM { get; set; }
        public virtual MainModule MainModule { get; set; }
        public virtual ICollection<UserAccessList> UserAccessLists { get; set; }
    }

    public class UserAccessToolsVM : Audit_Company
    {
        public int UAToolid { get; set; }
        public string UseraccesstoolNM { get; set; }
        public string Category { get; set; }
        public string UAPage { get; set; }
        [Required]
        public int ModuleID { get; set; }
        public virtual MainModule MainModule { get; set; }
        public bool ismenu { get; set; }
        public string ClassName { get; set; }
        public int SortIndex { get; set; }
        public int RoleID { get; set; }
        public virtual UserRole UserRole { get; set; }
        public int? SubModuleID { get; set; }
        public string ModuleName { get; set; }
        public string SubModuleName { get; set; }
        public string CompanyName { get; set; }
        public string RoleName { get; set; }
        public int TOTALCOUNT { get; set; }
        public int ROWNUM { get; set; }
        public virtual SubModule SubModule { get; set; }
    }

    public class JsonMainModuleViewModel
    {
        public int ModuleID { get; set; }
        public string ModuleName { get; set; }
        public string Description { get; set; }
        public bool isStatus { get; set; }
    }
    public class JsonSubModuleVM
    {
        public int ModuleID { get; set; }
        public int SubModuleID { get; set; }
        public string ModuleName { get; set; }
        public string SubModuleName { get; set; }
        public string Description { get; set; }
        public bool isStatus { get; set; }
    }

    public class JsonUserAccessViewModel
    {
        [JsonPropertyName("UAToolID")]
        public int UAToolID { get; set; }

        [JsonPropertyName("SubMenu")]
        public string SubMenu { get; set; }

        [JsonPropertyName("PageName")]
        public string PageName { get; set; }

        [JsonPropertyName("isView")]
        public bool isView { get; set; }

        [JsonPropertyName("isEdit")]
        public bool isEdit { get; set; }

        [JsonPropertyName("isSave")]
        public bool isSave { get; set; }

        [JsonPropertyName("isDelete")]
        public bool isDelete { get; set; }

        [JsonPropertyName("isApproved")]
        public bool isApproved { get; set; }
    }

    public class UserRoleModuleIViewModel : Audit
    {
        public int RoleID { get; set; }
        public List<SelectListItemModel> selectListItems { get; set; }
    }
    public class UserRoleSubModuleIVM : Audit
    {
        public int RoleID { get; set; }
        public int ModuelID { get; set; }
        public int SubModuleID { get; set; }
        public List<SelectListItemModel> selectListItems { get; set; }
    }

    public class RoleWiseUserAccessToolsVM
    {
        public int uatoolid { get; set; }
        public string UseraccesstoolNM { get; set; }
        public string Category { get; set; }
        public string UAPage { get; set; }
        public int ulistID { get; set; }
        public int UserID { get; set; }
        public string Accesstools { get; set; }
        public bool UAVeiw { get; set; }
        public bool UASave { get; set; }
        public bool UAEdit { get; set; }
        public bool UAdelete { get; set; }
        public bool UApproved { get; set; }
        public int RoleID { get; set; }
        public int ModuleID { get; set; }
        public int? SubModuleID { get; set; }
        public string AddedBy { get; set; }
        public string UpdatedBy { get; set; }
        public DateTime AddedDate { get; set; }
        public DateTime? UpdatedDate { get; set; }
    }
    public class HomeMenuVM
    { 
        public virtual List<MainModule> MainModule { get; set; }
        public virtual List<SubModule> SubModule { get; set; }
    }
    public class SubModule_Menu_VM
    {
        public virtual List<SubModuleVM> SubModule { get; set; }
        public virtual List<UserAccessToolsVM> Menu { get; set; }
    }

    public class ReportAccessVM
    {
        public int ReportId { get; set;}
        public string ReportName { get; set;}
        public string ReportLink { get; set;}
        public string UAPage { get; set;}
        public string ReportTypeName { get; set;}
        public string UseraccesstoolNM { get; set;}
        public string ClassName { get; set; }
        public string IdName { get; set; }
        public int RTypeSL { get; set; }
        public int SLNo { get; set; }
        public bool IsPermission { get; set;}
        public int ReportTypeId { get; set;}
        public int MenuId { get; set;}
        public int AddedBy { get; set;}
        public int CompId { get; set;}        
        public string AddedDate { get; set;}
    }
}
