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
    [NotMapped]
    public class LoginViewModel
    {
        [Required(ErrorMessage = "Username Required")]
        public string Username { get; set; }

        [Required(ErrorMessage = "Password Required")]
        public string Password { get; set; }
    }

    public class UserInfoViewModel
    {
        public int UserID { get; set; }
        public int CompId { get; set; }
        public int OrgId { get; set; }
        public int ClientId { get; set; }
        public string CompanyName { get; set; }

        [Required(ErrorMessage = "First Name Is Required")]
        [StringLength(200, MinimumLength = 3, ErrorMessage = "Name must be 3 to 200 characters")]
        [Display(Name = "First Name")]
        public string FirstName { get; set; }
        [Display(Name = "Last Name")]
        public string LastName { get; set; }

        //[Required(ErrorMessage = "User Name Is Required")]
        //[StringLength(200, MinimumLength = 3, ErrorMessage = "Name must be 3 to 200 characters")]
        [Display(Name = "User Name")]
        public string UserName { get; set; }


        //[Required(ErrorMessage = "Password Is Required")]
        [DataType(DataType.Password)]
        public string Password { get; set; }

        //[DataType(DataType.Password)]
        //[Required(ErrorMessage = "Confirm Password is Required")]
        //[Compare("Password", ErrorMessage = "The password and confirmation password do not match.")]
        [Display(Name = "Confirm Password")]
        public string ConfirmPassword { get; set; }


        [DataType(DataType.EmailAddress)]
        [Required(ErrorMessage = "Email is Required")]
        public string Email { get; set; }
        public string Phone { get; set; }
        [Display(Name = "User Status")]
        public string UserStatus { get; set; }
        public string EmpCode { get; set; }
        public string EmpPosition { get; set; }
        //[Required(ErrorMessage = "Role is Required")]
        public int RoleID { get; set; }
        public string RoleName { get; set; }
        public string ImagePath { get; set; }
        public IFormFile Image { get; set; }
        public DateTime CreateDate { get; set; }

        [NotMapped]
        public int wrong_login_attempt { get; set; }
        [NotMapped]
        public int today_login_attempt { get; set; }
        public string AddedBy { get; set; }
        public string UpdatedBy { get; set; }
        public DateTime AddedDate { get; set; }
        public DateTime? UpdatedDate { get; set; }
        public int TOTALCOUNT { get; set; }

    }

    public class UserRoleViewModel
    {
        [Key]
        public int RoleID { get; set; }

        [Required(ErrorMessage = "Name Is Required")]
        //[StringLength(200, MinimumLength = 3, ErrorMessage = "Name must be 3 to 200 characters")]
        [Display(Name = "Role Name")]
        public string RoleName { get; set; }
        public string RoleType { get; set; }
        public int CompId { get; set; }
        public int? OrgId { get; set; }

        [Required(ErrorMessage = "LandingPage Is Required")]
        //[StringLength(200, MinimumLength = 3, ErrorMessage = "Name must be 3 to 200 characters")]
        [Display(Name = "Landing Page")]
        public string LandingPage { get; set; }
        public string AddedBy { get; set; }
        public string UpdatedBy { get; set; }
        public DateTime AddedDate { get; set; }
        public DateTime? UpdatedDate { get; set; }
    }

    public class EditDeleteInfoVM
    {
        public int ID { get; set; }
        public int UserID { get; set; }
        public string TransectionID { get; set; }
        public string TransStatement { get; set; }
        public string ControllerName { get; set; }
        public string ActionName { get; set; }
        public string PageLink { get; set; }
        public string DocumentReferance { get; set; }
        public string CommandType { get; set; }
        public string PcName { get; set; }
        public string IPAddress { get; set; }
        public string MacAddress { get; set; }
        public string CompID { get; set; }
        public string EntryDate { get; set; }
        public string AddUTC { get; set; }
        public int TOTALCOUNT { get; set; }
        public string Name { get; set; }
        public string UserName { get; set; }
        public string Email { get; set; }
    }

    public class EditDeleteInfoFilterVM
    {
        public int UserId { get; set; }
        public DateTime FromDate { get; set; }
        public DateTime ToDate { get; set; }
        public string Controller { get; set; }
        public string Action { get; set; }
        public string CommandType { get; set; }
    }

    public class ProcessLogFilterVM
    {
        public int UserId { get; set; }
        public DateTime FromDate { get; set; }
        public DateTime ToDate { get; set; }       
        public string ProcessType { get; set; }
    }

    public class ProcessLogVM
    {
        public int LogID { get; set; }
        public int UserID { get; set; }      
        public string CompID { get; set; }
        public string Type { get; set; }       
        public string EntryDate { get; set; }
        public string FromDate { get; set; }
        public string ToDate { get; set; }
        public string AddUTC { get; set; }
        public int TOTALCOUNT { get; set; }
        public string Name { get; set; }
        public string UserName { get; set; }
        public string Email { get; set; }
    }

}
