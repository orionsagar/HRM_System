using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Domains.Models
{
    public class UserInfo : Audit
    {
        [Key]
        public int UserID { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string UserName { get; set; }
        public string Password { get; set; }
        public string Email { get; set; }
        public string Phone { get; set; }
        public string CountryCode { get; set; }
        public string UserStatus { get; set; }
        public string EmpCode { get; set; }
        public string EmpPosition { get; set; }

        public int? RoleID { get; set; } = 10;
        [ForeignKey("RoleID")]
        public virtual UserRole? UserRole { get; set; }
        public int wrong_login_attempt { get; set; }
        public int today_login_attempt { get; set; }
        public int? ClientId { get; set; }
        [ForeignKey("ClientId")]
        public virtual Client Client { get; set; }
        public int OrgId { get; set; }
        public string ImagePath { get; set; }
        public int CompId { get; set; }
        [ForeignKey("CompId")]
        public virtual Company Company { get; set; }


        public string FullName
        {
            get
            {
                return FirstName + ' ' + LastName;
            }
        }
    }

    public class UserRole : Audit
    {
        [Key]
        public int RoleID { get; set; }
        public string RoleName { get; set; }
        public string U_RoleName { get; set; }
        public string RoleType { get; set; }
        public int? OrgId { get; set; }
        public int? CompId { get; set; }
        [ForeignKey("CompId")]
        public virtual Company Company { get; set; }
        public string LandingPage { get; set; }

    }

    public class UserRolewiseModule : Audit
    {
        [Key]
        public int URWM_ID { get; set; }
        public int RoleID { get; set; }
        
        public int ModuleID { get; set; }
        [ForeignKey("ModuleID")]
        public virtual MainModule MainModule { get; set; }
        //public string ModuleName { get; set; }
        public string Description { get; set; }
    }

    public class UserRolewiseSubModule : Audit
    {
        [Key]
        public int URWSM_ID { get; set; }
        public int RoleID { get; set; }
        
        public int ModuleID { get; set; }
        [ForeignKey("ModuleID")]
        public virtual MainModule MainModule { get; set; }
        public int SubModuleID { get; set; }
        [ForeignKey("SubModuleID")]
        public virtual SubModule SubModule { get; set; }

        public string Description { get; set; }
    }

    
    public enum UserStatus : short
    {
        Inactive = 0, Active, Blocked,Pending
    }
   

}
